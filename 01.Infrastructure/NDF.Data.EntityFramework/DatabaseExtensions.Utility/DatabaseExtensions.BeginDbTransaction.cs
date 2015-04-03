using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NDF.Data.EntityFramework
{
    public static partial class DatabaseExtensions
    {

        /// <summary>
        /// 开始一个数据库事务。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>一个数据库事务 <see cref="System.Data.Common.DbTransaction"/> 对象。</returns>
        public static DbTransaction BeginDbTransaction(this Database database)
        {
            return GetGeneralDatabase(database).BeginTransaction();
        }

        /// <summary>
        /// 以指定的事务锁定级别开始一个数据库事务。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="isolationLevel">指定的数据库事务锁定级别 <see cref="IsolationLevel"/>。</param>
        /// <returns>一个指定了事务锁定级别的数据库事务 <see cref="System.Data.Common.DbTransaction"/> 对象。</returns>
        public static DbTransaction BeginDbTransaction(this Database database, System.Data.IsolationLevel isolationLevel)
        {
            return GetGeneralDatabase(database).BeginTransaction(isolationLevel);
        }

    }
}
