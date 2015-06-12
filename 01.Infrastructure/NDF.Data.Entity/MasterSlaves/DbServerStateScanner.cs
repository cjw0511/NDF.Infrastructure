using NDF.Data.Entity.MasterSlaves.Interception;
using NDF.Data.Utilities;
using NDF.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NDF.Data.Entity.MasterSlaves
{
    /// <summary>
    /// 定义一个数据库服务器连接状态扫描器，用于检测数据库服务器的连接状态。
    /// </summary>
    public class DbServerStateScanner : Disposable
    {
        private DbMasterSlaveConfigContext _config;

        private Timer _timer;
        //private List<CancellationTokenSource> _cancellationTokens = new List<CancellationTokenSource>();
        private ConcurrentDictionary<CancellationToken, CancellationTokenSource> _cancellationTokens = new ConcurrentDictionary<CancellationToken, CancellationTokenSource>();


        #region 构造函数定义

        /// <summary>
        /// 初始化类型 <see cref="DbServerStateScanner"/> 的新实例。
        /// </summary>
        private DbServerStateScanner()
        {
            this.Disposing += DbServerStateScanner_Disposing;
            this.InitializeInterceptorRegister();
        }

        /// <summary>
        /// 初始化类型 <see cref="DbServerStateScanner"/> 的新实例。
        /// </summary>
        internal DbServerStateScanner(DbMasterSlaveConfigContext config)
            : this()
        {
            Check.NotNull(config);
            this._config = config;
        }

        #endregion



        /// <summary>
        /// 开启针对当前对象使用的配置对象中所示配置内容的数据库连接状态可用性检测功能。
        /// </summary>
        /// <param name="factory"></param>
        public void StartScanTask(DbProviderFactory factory)
        {
            Check.NotNull(factory);

            this.ClearResources();

            TimerCallback callback = state => this.ScanDbServersState(factory);
            int douTime = this._config.ServerStateScanInterval * 1000;
            int period = this._config.ServerStateScanInterval * 1000;

            this._timer = new Timer(callback, null, douTime, period);
        }


        private void ScanDbServersState(DbProviderFactory factory)
        {
            DbServer[] servers = this.GetDbServers();
            if (servers.Length > 0)
            {
                foreach (DbServer server in servers)
                {
                    this.ScanDbServerState(server, factory);
                }
            }
        }

        private void ScanDbServerState(DbServer server, DbProviderFactory factory)
        {
            DbConnectionStringTester tester = DbConnectionStringTester.GetConnectionStringTester(factory);
            CancellationTokenSource cts = new CancellationTokenSource();

            cts.Token.Register(() => this.RemoveCancellationToken(cts));
            //this._cancellationTokens.Add(cts);
            this._cancellationTokens.TryAdd(cts.Token, cts);

            this.OnScanning(server.ConnectionString, server.Type, server.State);
            tester.TestAsync(server.ConnectionString, cts.Token).ContinueWith(
                t =>
                {
                    if (t.IsCompleted || t.IsFaulted)
                        this.RemoveCancellationToken(cts);

                    if (t.IsCompleted && server != null)
                    {
                        server.State = t.Result ? DbServerState.Online : DbServerState.Offline;
                        this.OnScanned(server.ConnectionString, server.Type, server.State);
                    }
                });
        }


        private void RemoveCancellationToken(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource != null)
            {
                //this._cancellationTokens.Remove(cancellationTokenSource);
                CancellationTokenSource cts;
                this._cancellationTokens.TryRemove(cancellationTokenSource.Token, out cts);
            }
        }


        private DbServer[] GetDbServers()
        {
            List<DbServer> list = new List<DbServer>();

            if (this._config.ServerStateScanWithNonOffline)
            {
                if (this._config.MasterServer.State == DbServerState.Online)
                    list.Add(this._config.MasterServer);

                list.AddRange(this._config.OnlineSlaveServers);
            }
            else
            {
                list.Add(this._config.MasterServer);
                list.AddRange(this._config.SlaveServers);
            }

            return list.ToArray();
        }



        #region 定义数据库连接状态可用性轮询检测的相依事件触发机制

        /// <summary>
        /// 引发 <see cref="DbServerStateScanner"/> 类型对象的 <see cref="Scanning"/> 事件。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="serverType"></param>
        /// <param name="serverState"></param>
        protected void OnScanning(string connectionString, DbServerType serverType, DbServerState serverState)
        {
            if (this.Scanning != null)
            {
                DbServerStateScanEventArgs e = new DbServerStateScanEventArgs(connectionString, serverType, serverState);
                this.Scanning(this, e);
            }
        }

        /// <summary>
        /// 引发 <see cref="DbServerStateScanner"/> 类型对象的 <see cref="Scanned"/> 事件。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="serverType"></param>
        /// <param name="serverState"></param>
        protected void OnScanned(string connectionString, DbServerType serverType, DbServerState serverState)
        {
            if (this.Scanned != null)
            {
                DbServerStateScanEventArgs e = new DbServerStateScanEventArgs(connectionString, serverType, serverState);
                this.Scanned(this, e);
            }
        }


        /// <summary>
        /// 表示 EF 数据库主从读写分离服务在自动扫描数据库服务器节点的可用性状态执行瞬间前所引发的事件动作。
        /// </summary>
        public event DbServerStateScanEventHandler Scanning;

        /// <summary>
        /// 表示 EF 数据库主从读写分离服务在自动扫描数据库服务器节点的可用性状态完成瞬间后所引发的事件动作。
        /// </summary>
        public event DbServerStateScanEventHandler Scanned;



        private void InitializeInterceptorRegister()
        {
            this.Scanning += DbServerStateScanner_Scanning;
            this.Scanned += DbServerStateScanner_Scanned;
        }

        private void DbServerStateScanner_Scanning(object sender, DbServerStateScanEventArgs e)
        {
            MasterSlaveInterception.Dispatcher.DbServerStateScanning(e.ConnectionString, e.DbServerType, e.DbServerState, this._config.TargetContextType);
        }

        private void DbServerStateScanner_Scanned(object sender, DbServerStateScanEventArgs e)
        {
            MasterSlaveInterception.Dispatcher.DbServerStateScanned(e.ConnectionString, e.DbServerType, e.DbServerState, this._config.TargetContextType);
        }

        #endregion


        #region Dispose 和资源释放相关操作

        private void DbServerStateScanner_Disposing(object sender, EventArgs e)
        {
            this.ClearResources();
        }

        private void ClearResources()
        {
            if (this._timer != null)
            {
                this._timer.Dispose();
                this._timer = null;
            }
            if (this._cancellationTokens != null && this._cancellationTokens.Count > 0)
            {
                //foreach (CancellationTokenSource cts in this._cancellationTokens.ToArray())
                //{
                //    if (cts != null && !cts.IsCancellationRequested)
                //        cts.Cancel(false);
                //}
                //this._cancellationTokens.Clear();
                foreach (CancellationTokenSource cts in this._cancellationTokens.Values.ToArray())
                {
                    cts.Cancel(false);
                }
                this._cancellationTokens.Clear();
            }
        }

        #endregion

    }
}
