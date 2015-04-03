using NDF.Data.EnterpriseLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Common
{
    public abstract partial class Database
    {
        /// <summary>
        /// 开始一个数据库事务。
        /// </summary>
        /// <returns>一个数据库事务 <see cref="System.Data.Common.DbTransaction"/> 对象。</returns>
        public DbTransaction BeginTransaction()
        {
            return DatabaseExtensions.BeginTransaction(this.PrimitiveDatabase);
        }

        /// <summary>
        /// 以指定的事务锁定级别开始一个数据库事务。
        /// </summary>
        /// <param name="isolationLevel">指定的数据库事务锁定级别 <see cref="IsolationLevel"/>。</param>
        /// <returns>一个指定了事务锁定级别的数据库事务 <see cref="System.Data.Common.DbTransaction"/> 对象。</returns>
        public DbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return DatabaseExtensions.BeginTransaction(this.PrimitiveDatabase, isolationLevel);
        }
    }
}
