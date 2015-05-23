using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDF.Data.Entity.MasterSlaves
{
    /// <summary>
    /// 表示 EF 读写分离服务中的 Slave 数据库服务器节点。
    /// </summary>
    public class DbSlaveServer : DbServer
    {

        /// <summary>
        /// 以指定的数据库连接字符串值作为 <see cref="DbServer.ConnectionString"/> 属性值初始化类型 <see cref="DbSlaveServer"/> 的新实例。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="order"></param>
        public DbSlaveServer(string connectionString, int order)
            : base(connectionString)
        {
            this.Order = order;
        }


        /// <summary>
        /// 获取该 Slave 数据库服务器节点的序号。
        /// </summary>
        public int Order { get; private set; }


        /// <summary>
        /// 获取该 数据库服务器节点 的类型，该属性始终返回 <see cref="DbServerType.Slave"/>。
        /// </summary>
        public override DbServerType Type
        {
            get { return DbServerType.Slave; }
        }

    }
}
