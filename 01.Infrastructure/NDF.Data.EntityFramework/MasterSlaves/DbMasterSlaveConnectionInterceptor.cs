using NDF.Data.EntityFramework.Interception;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves
{
    /// <summary>
    /// 定义一个 EF 实体数据库主从读写分离服务的数据库连接动作执行拦截器，用于配合 <see cref="DbMasterSlaveCommandInterceptor"/> 实现数据库读写分离操作。
    /// </summary>
    internal class DbMasterSlaveConnectionInterceptor : NullDbConnectionInterceptor
    {
        private DbMasterSlaveCommandInterceptor _commandInterceptor;

        /// <summary>
        /// 以指定的 <see cref="DbMasterSlaveCommandInterceptor"/> 类型对象作为 <see cref="CommandInterceptor"/> 属性值初始化类型 <see cref="DbMasterSlaveConnectionInterceptor"/> 的新实例。
        /// </summary>
        /// <param name="commandInterceptor"></param>
        internal DbMasterSlaveConnectionInterceptor(DbMasterSlaveCommandInterceptor commandInterceptor)
        {
            Check.NotNull(commandInterceptor);
            this._commandInterceptor = commandInterceptor;
        }


        internal DbMasterSlaveCommandInterceptor CommandInterceptor
        {
            get { return this._commandInterceptor; }
        }


        /// <summary>
        /// 在打开数据库连接动作执行前瞬间触发。将数据库连接字符串更新至 EF 数据库主从读写分离服务中配置的相关值。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public override void Opening(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            string connectionString = this.CommandInterceptor.MasterConnectionString;

            if (!DbMasterSlaveCommandInterceptor.ConnectionStringEquals(connection, connectionString))
            {
                DbMasterSlaveCommandInterceptor.UpdateConnectionString(connection, connectionString);
            }
        }

    }
}
