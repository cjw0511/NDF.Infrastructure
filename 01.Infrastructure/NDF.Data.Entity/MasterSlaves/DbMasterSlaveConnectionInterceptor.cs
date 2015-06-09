using NDF.Data.Entity.Interception;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.MasterSlaves
{
    /// <summary>
    /// 定义一个 EF 实体数据库主从读写分离服务的数据库连接动作执行拦截器，用于配合 <see cref="DbMasterSlaveCommandInterceptor"/> 实现数据库读写分离操作。
    /// </summary>
    internal class DbMasterSlaveConnectionInterceptor : NullDbConnectionInterceptor
    {
        private DbMasterSlaveConfigContext _config;


        /// <summary>
        /// 以指定的 EF 数据库主从读写分离配置上下文对象作为 <see cref="Config"/> 属性值初始化类型 <see cref="DbMasterSlaveConnectionInterceptor"/> 的新实例。
        /// </summary>
        /// <param name="config"></param>
        internal DbMasterSlaveConnectionInterceptor(DbMasterSlaveConfigContext config)
        {
            Check.NotNull(config);
            this._config = config;
        }


        internal DbMasterSlaveConfigContext Config
        {
            get { return this._config; }
        }


        /// <summary>
        /// 在开启一个数据库事务动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public override void BeginningTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext)
        {
            UpdateToMasterIfNeed(connection, interceptionContext, this.Config);
        }

        /// <summary>
        /// 在打开数据库连接动作执行前瞬间触发。将数据库连接字符串更新至 EF 数据库主从读写分离服务中配置的相关值。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public override void Opening(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            UpdateToMasterIfNeed(connection, interceptionContext, this.Config);
        }

        internal static void UpdateToMasterIfNeed(DbConnection connection,
            DbInterceptionContext interceptionContexts, DbMasterSlaveConfigContext config)
        {
            //判断是否是该过滤器的数据库
            if (!interceptionContexts.DbContexts.Any(config.CanApplyTo))
            {
                return;
            }
            //比较连接字符串
            string connectionString = config.UsableMasterConnectionString;
            if (!DbMasterSlaveCommandInterceptor.ConnectionStringEquals(connection, connectionString))
            {
                DbMasterSlaveCommandInterceptor.UpdateConnectionString(connection, connectionString);
            }
        }

    }
}
