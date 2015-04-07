using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves.Interception
{
    /// <summary>
    /// 接口 <see cref="IMasterSlaveInterceptor"/> 的默认实现。
    /// <para>通过继承该类型，并将该类型实例通过 <see cref="MasterSlaveInterception.Add"/> 方法注册至 EF 数据库主从读写分离服务上下文中，可以实现在当 EF 数据库主从读写分离服务执行特定动作时收到相应的通知并触发自定义的其他响应动作。</para>
    /// <para>详情参见 <see cref="IMasterSlaveInterceptor"/>。</para>
    /// </summary>
    public class MasterSlaveInterceptor : IMasterSlaveInterceptor
    {
        private Type _contextType = typeof(System.Data.Entity.DbContext);


        /// <summary>
        /// 表示该 EF 数据库主从读写分离服务动作拦截器所适用的 <see cref="System.Data.Entity.DbContext"/> 类型。
        /// <para>意即该动作拦截器仅限于在 EF 数据库主从读写分离服务操作当前属性定义的 <see cref="System.Data.Entity.DbContext"/> 实体数据库上下文对象时才生效。</para>
        /// <para>在类型 <see cref="MasterSlaveInterceptor"/> 中，该属性始终返回 typeof(<see cref="System.Data.Entity.DbContext"/>)，表示该拦截器可适用于所有 EF 实体数据库上下文对象。</para>
        /// <para>如需指定该继承类型拦截器只适用于特定的 EF 实体数据库上下文对象，请重写该属性。</para>
        /// </summary>
        public virtual Type TargetContextType
        {
            get { return this._contextType; }
        }


        /// <summary>
        /// 在检测 EF 数据库主从读写分离服务的数据库服务器节点可用状态前瞬间触发。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="serverType"></param>
        /// <param name="serverState"></param>
        public virtual void DbServerStateScanning(string connectionString, DbServerType serverType, DbServerState serverState)
        {
        }

        /// <summary>
        /// 在检测 EF 数据库主从读写分离服务的数据库服务器节点可用状态后瞬间触发。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="serverType"></param>
        /// <param name="serverState"></param>
        public virtual void DbServerStateScanned(string connectionString, DbServerType serverType, DbServerState serverState)
        {
        }


        /// <summary>
        /// 在 EF 数据库主从读写分离服务更改数据库操作命令的连接字符串前瞬间触发。
        /// </summary>
        /// <param name="command"></param>
        public virtual void ConnectionStringUpdating(System.Data.Common.DbCommand command)
        {
        }

        /// <summary>
        /// 在 EF 数据库主从读写分离服务更改数据库操作命令的连接字符串后瞬间触发。
        /// </summary>
        /// <param name="command"></param>
        public virtual void ConnectionStringUpdated(System.Data.Common.DbCommand command)
        {
        }


    }
}
