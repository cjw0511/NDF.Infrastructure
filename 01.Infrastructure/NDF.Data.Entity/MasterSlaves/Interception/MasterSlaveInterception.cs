using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.MasterSlaves.Interception
{
    /// <summary>
    /// 提供 EF 数据库主从读写分离服务动作拦截器 <see cref="IMasterSlaveInterceptor"/> 的注册点。
    /// <para>EF 数据库主从读写分离服务执行特定动作时，将发出拦截命令通知并执行相应拦截器代码。</para>
    /// <para>关于拦截器请参见 <see cref="IMasterSlaveInterceptor"/>。</para>
    /// </summary>
    public static class MasterSlaveInterception
    {
        private static readonly Lazy<MasterSlaveDispatcher> _dispatcher = new Lazy<MasterSlaveDispatcher>(() => new MasterSlaveDispatcher());


        /// <summary>
        /// 将一个拦截器对象注册至当前 EF 数据库主从读写分离服务上下文。
        /// 以便在 EF 数据库主从读写分离服务执行特定动作时执行拦截器代码。
        /// </summary>
        /// <param name="interceptor"></param>
        public static void Add(IMasterSlaveInterceptor interceptor)
        {
            Check.NotNull(interceptor);
            _dispatcher.Value.AddInterceptor(interceptor);
        }

        /// <summary>
        /// 将一个拦截器对象从 EF 数据库主从读写分离服务上下文中解除注册。
        /// 以便在 EF 数据库主从读写分离服务执行特定动作时将不执行该拦截器中的代码。
        /// </summary>
        /// <param name="interceptor"></param>
        public static void Remove(IMasterSlaveInterceptor interceptor)
        {
            Check.NotNull(interceptor);
            _dispatcher.Value.RemoveInterceptor(interceptor);
        }


        internal static MasterSlaveDispatcher Dispatcher
        {
            get { return _dispatcher.Value; }
        }

    }
}
