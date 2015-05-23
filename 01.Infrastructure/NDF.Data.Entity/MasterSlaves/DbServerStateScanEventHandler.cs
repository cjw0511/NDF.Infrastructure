using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.MasterSlaves
{
    
    /// <summary>
    /// 表示 EF 数据库主从读写分离服务在自动扫描数据库服务器节点的可用性状态执行前或执行后所引发的事件动作。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DbServerStateScanEventHandler(object sender, DbServerStateScanEventArgs e);

}
