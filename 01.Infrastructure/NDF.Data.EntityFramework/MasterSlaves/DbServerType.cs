using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves
{
    /// <summary>
    /// 表示 EF 读写分离服务中数据库服务器节点的类型。
    /// </summary>
    public enum DbServerType
    {

        /// <summary>
        /// 表示该 EF 读写分离服务的数据库服务器是一个 Master 节点。
        /// </summary>
        Master = 0,

        /// <summary>
        /// 表示该 EF 读写分离服务的数据库服务器是一个 Slave 节点。
        /// </summary>
        Slave = 1,

    }
}
