using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves
{
    /// <summary>
    /// 提供对 EntityFramework 的数据库操作读写分离服务的注册配置功能。
    /// </summary>
    public class EFMasterSlaveConfig
    {

        /// <summary>
        /// 将指定的 EF 实体数据库上下文类型注册到读写分离服务中。这是 EF 读写分离服务启动的入口点。
        /// <para>注意：传入的参数 <paramref name="contextType"/> 所表示的类型必须是 <see cref="System.Data.Entity.DbContext"/> 或者该类型的子类型。</para>
        /// </summary>
        /// <param name="contextType"></param>
        public static void Register(Type contextType)
        {
            DbInterceptors interceptors = new DbInterceptors(contextType);
            interceptors.Register();
        }

    }
}
