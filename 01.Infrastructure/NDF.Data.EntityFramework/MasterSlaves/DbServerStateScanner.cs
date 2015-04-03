using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves
{
    /// <summary>
    /// 定义一个数据库服务器连接状态扫描器，用于检测数据库服务器的连接状态。
    /// </summary>
    public class DbServerStateScanner
    {
        private static object _locker = new object();
        private static Dictionary<DbProviderFactory, DbServerStateScanner> _scanners = new Dictionary<DbProviderFactory, DbServerStateScanner>();

        private DbProviderFactory _factory;


        /// <summary>
        /// 以一个 <see cref="DbProviderFactory"/> 作为 ADO.NET 对象工厂对象初始化类型 <see cref="DbServerStateScanner"/> 的新实例。
        /// </summary>
        /// <param name="factory"></param>
        private DbServerStateScanner(DbProviderFactory factory)
        {
            this._factory = factory;
        }


        /// <summary>
        /// 获取当前扫描器用于创建 ADO.NET 对象的工厂对象。
        /// </summary>
        public DbProviderFactory ProviderFactory
        {
            get { return this._factory; }
        }


        /// <summary>
        /// 使用同步方式检测一个数据库连接字符串所表示的数据库服务器的可连接状态。
        /// 注意：在连接请求异常的情况下，该方法可能需要很长时间（具体取决于传入的参数 <paramref name="connectionString"/> 中设定的 timeout 值）才能获得返回结果。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public DbServerState Scan(string connectionString)
        {
            DbServerState state = DbServerState.Online;
            using (DbConnection connection = this.GetConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    state = DbServerState.Online;
                    connection.Close();
                }
                catch
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                    state = DbServerState.Offline;
                }
            }
            return state;
        }


        /// <summary>
        /// 使用异步方式检测一个数据库连接字符串所表示的数据库服务器的可连接状态。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public Task<DbServerState> ScanAsync(string connectionString)
        {
            return Task.Run(
                () =>
                {
                    return this.Scan(connectionString);
                });
        }

        /// <summary>
        /// 使用异步方式检测一个数据库连接字符串所表示的数据库服务器的可连接状态。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<DbServerState> ScanAsync(string connectionString, CancellationToken cancellationToken)
        {
            return Task.Run(
                () =>
                {
                    return this.Scan(connectionString);
                }, cancellationToken);
        }


        /// <summary>
        /// 获取一个当前扫描器所使用的数据库连接对象。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected DbConnection GetConnection(string connectionString)
        {
            connectionString = Check.EmptyCheck(connectionString);
            DbConnection connection = this.ProviderFactory.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }



        /// <summary>
        /// 获取 <see cref="DbServerStateScanner"/> 类型可用于检测指定的 ADO.NET 对象工厂 <paramref name="factory"/> 生成的数据库连接可用性状态的一个实例。
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static DbServerStateScanner GetDbServerStateScanner(DbProviderFactory factory)
        {
            Check.NotNull(factory);
            lock (_locker)
            {
                DbServerStateScanner scanner = null;
                if (!_scanners.TryGetValue(factory, out scanner))
                {
                    scanner = new DbServerStateScanner(factory);
                    _scanners.Add(factory, scanner);
                }
                return scanner;
            }
        }


    }
}
