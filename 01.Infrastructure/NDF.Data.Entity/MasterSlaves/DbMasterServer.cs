using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDF.Data.Entity.MasterSlaves
{
    /// <summary>
    /// 表示 EF 读写分离服务中的 Master 数据库服务器节点。
    /// </summary>
    public class DbMasterServer : DbServer
    {

        /// <summary>
        /// 以指定的数据库连接字符串值作为 <see cref="DbServer.ConnectionString"/> 属性值初始化类型 <see cref="DbMasterServer"/> 的新实例。
        /// </summary>
        /// <param name="connectionString"></param>
        public DbMasterServer(string connectionString)
            : base(connectionString)
        {
        }


        /// <summary>
        /// 获取该 数据库服务器节点 的类型，该属性始终返回 <see cref="DbServerType.Master"/>。
        /// </summary>
        public override DbServerType Type
        {
            get { return DbServerType.Master; }
        }

    }
}
