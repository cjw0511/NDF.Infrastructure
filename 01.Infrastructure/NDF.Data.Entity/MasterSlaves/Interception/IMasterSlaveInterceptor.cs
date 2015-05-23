using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.MasterSlaves.Interception
{
    /// <summary>
    /// 该接口定义 EF 数据库主从读写分离服务中一组面向切面事件注入点功能的动作拦截器。
    /// 通过实现该接口，可以在当 EF 数据库主从读写分离服务执行特定动作时收到相应的通知并触发自定义的其他响应动作。
    /// </summary>
    public interface IMasterSlaveInterceptor
    {

        /// <summary>
        /// 表示该 EF 数据库主从读写分离服务动作拦截器所适用的 <see cref="System.Data.Entity.DbContext"/> 类型。
        /// <para>意即该动作拦截器仅限于在 EF 数据库主从读写分离服务操作当前属性定义的 <see cref="System.Data.Entity.DbContext"/> 实体数据库上下文对象时才生效。</para>
        /// </summary>
        Type TargetContextType { get; }


        /// <summary>
        /// 在检测 EF 数据库主从读写分离服务的数据库服务器节点可用状态前瞬间触发。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="serverType"></param>
        /// <param name="serverState"></param>
        void DbServerStateScanning(string connectionString, DbServerType serverType, DbServerState serverState);

        /// <summary>
        /// 在检测 EF 数据库主从读写分离服务的数据库服务器节点可用状态后瞬间触发。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="serverType"></param>
        /// <param name="serverState"></param>
        void DbServerStateScanned(string connectionString, DbServerType serverType, DbServerState serverState);


        /// <summary>
        /// 在 EF 数据库主从读写分离服务更改数据库操作命令的连接字符串前瞬间触发。
        /// </summary>
        /// <param name="command"></param>
        void ConnectionStringUpdating(DbCommand command);

        /// <summary>
        /// 在 EF 数据库主从读写分离服务更改数据库操作命令的连接字符串后瞬间触发。
        /// </summary>
        /// <param name="command"></param>
        void ConnectionStringUpdated(DbCommand command);

    }
}
