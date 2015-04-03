using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDF.Data.EntityFramework.MasterSlaves
{
    /// <summary>
    /// 表示 EF 读写分离服务中的 Slave 数据库服务器节点模型。
    /// </summary>
    public class SlaveServer : DbServer
    {

        /// <summary>
        /// 以指定的数据库连接字符串值作为 <see cref="DbServer.ConnectionString"/> 属性值初始化类型 <see cref="SlaveServer"/> 的新实例。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="order"></param>
        protected internal SlaveServer(string connectionString, int order)
            : base(connectionString)
        {
            this.Order = order;
        }


        /// <summary>
        /// 获取该 Slave 数据库服务器节点的序号。
        /// </summary>
        public int Order { get; private set; }

    }
}
