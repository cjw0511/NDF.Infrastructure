using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.MasterSlaves
{
    /// <summary>
    /// 表示 EF 读写分离服务中数据库服务器节点的可用（连接）状态。
    /// </summary>
    public enum DbServerState
    {

        /// <summary>
        /// 表示该数据库服务器当前处于在线状态，默认值。
        /// </summary>
        Online = 0,

        /// <summary>
        /// 表示该数据库服务器当前处于离线状态。
        /// </summary>
        Offline = 1,

    }
}
