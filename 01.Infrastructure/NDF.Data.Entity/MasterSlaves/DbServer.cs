using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDF.Data.Entity.MasterSlaves
{
    /// <summary>
    /// 表示 EF 读写分离服务中的一个数据库服务器节点。
    /// </summary>
    public abstract class DbServer
    {

        /// <summary>
        /// 以指定的数据库连接字符串值作为 <see cref="ConnectionString"/> 属性值初始化类型 <see cref="DbServer"/> 的新实例。
        /// </summary>
        /// <param name="connectionString"></param>
        public DbServer(string connectionString)
        {
            connectionString = Check.EmptyCheck(connectionString);
            this.ConnectionString = connectionString;
            this.State = DbServerState.Online;
        }


        /// <summary>
        /// 表示该数据库服务器节点所使用的数据库连接字符串。
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// 表示该数据库服务器节点的在线状态。
        /// </summary>
        public DbServerState State { get; internal set; }


        /// <summary>
        /// 获取该 数据库服务器节点 的类型。
        /// </summary>
        public abstract DbServerType Type { get; }

    }
}
