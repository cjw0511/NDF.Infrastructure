using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.MasterSlaves
{
    /// <summary>
    /// 表示当 EF 数据库主从读写分离服务扫描配置中的数据库服务器节点可用性状态时所引发事件的数据。
    /// </summary>
    public class DbServerStateScanEventArgs : EventArgs
    {

        /// <summary>
        /// 初始化类型 <see cref="DbServerStateScanEventArgs"/> 的新实例。
        /// </summary>
        private DbServerStateScanEventArgs()
        {
            this.DbServerType = DbServerType.Master;
            this.DbServerState = DbServerState.Online;
        }

        /// <summary>
        /// 以指定的属性值初始化类型 <see cref="DbServerStateScanEventArgs"/> 的新实例。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="serverType"></param>
        /// <param name="serverState"></param>
        public DbServerStateScanEventArgs(string connectionString, DbServerType serverType, DbServerState serverState)
            : this()
        {
            connectionString = Check.EmptyCheck(connectionString);
            this.ConnectionString = connectionString;
            this.DbServerType = serverType;
            this.DbServerState = serverState;
        }


        /// <summary>
        /// 获取该引发该事件的数据库服务器节点可用性状态检测动作所扫描的数据库连接字符串。
        /// </summary>
        public string ConnectionString
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取该引发该事件的数据库服务器节点可用性状态检测动作所扫描的数据库服务器类型。
        /// </summary>
        public DbServerType DbServerType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取该引发该事件的数据库服务器节点可用性状态检测动作所扫描的数据库服务器的可用性状态。
        /// </summary>
        public DbServerState DbServerState
        {
            get;
            private set;
        }


    }
}
