using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDF.Data.EntityFramework.MasterSlaves
{
    /// <summary>
    /// 表示 EF 读写分离服务中的 Master 数据库服务器节点模型。
    /// </summary>
    public class MasterServer : DbServer
    {

        /// <summary>
        /// 以指定的数据库连接字符串值作为 <see cref="DbServer.ConnectionString"/> 属性值初始化类型 <see cref="MasterServer"/> 的新实例。
        /// </summary>
        /// <param name="connectionString"></param>
        protected internal MasterServer(string connectionString)
            : base(connectionString)
        {
        }


    }
}
