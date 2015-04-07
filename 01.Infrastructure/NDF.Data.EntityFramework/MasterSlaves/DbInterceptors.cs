using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves
{
    internal class DbInterceptors
    {
        private Type _contextType;

        private DbMasterSlaveCommandInterceptor _commandInterceptor;
        private DbMasterSlaveConnectionInterceptor _connectionInterceptor;


        /// <summary>
        /// 初始化类型 <see cref="DbInterceptors"/> 的新实例。
        /// </summary>
        /// <param name="contextType"></param>
        internal DbInterceptors(Type contextType)
        {
            Check.NotNull(contextType);
            this._contextType = contextType;
            this.InitializeInterceptors();
        }


        /// <summary>
        /// 将将当前对象中所有的拦截器字段添加至 EF 实体数据库执行上下文中。
        /// </summary>
        internal void Register()
        {
            this.RemoveInterceptors();
            this.AddInterceptors();
        }


        private void AddInterceptors()
        {
            DbInterception.Add(this._commandInterceptor);
            DbInterception.Add(this._connectionInterceptor);
        }

        private void RemoveInterceptors()
        {
            DbInterception.Remove(this._commandInterceptor);
            DbInterception.Remove(this._connectionInterceptor);
        }


        private void InitializeInterceptors()
        {
            this._commandInterceptor = new DbMasterSlaveCommandInterceptor(this._contextType);
            this._connectionInterceptor = new DbMasterSlaveConnectionInterceptor(this._commandInterceptor);
        }


    }
}
