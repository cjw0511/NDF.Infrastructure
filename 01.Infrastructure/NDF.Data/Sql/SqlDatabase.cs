using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Sql
{
    /// <summary>
    /// 提供对 SQL Server 数据库访问基础组件基类的定义；该组件提供一组方法，用于封装对 ADO.NET 中 SQL Server 数据源便捷的进行访问。
    /// <remarks>在实现原理上该类型是 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase"/> 的一个包装器。</remarks>
    /// </summary>
    [DbProvider("System.Data.SqlClient")]
    public class SqlDatabase : Database
    {
        private Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase _database;


        /// <summary>
        /// 以指定的数据库连接字符串初始化 <see cref="NDF.Data.Sql.SqlDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        public SqlDatabase(string connectionString) : base(connectionString) { }

        /// <summary>
        /// 以 <paramref name="sqlDatabase"/> 作为 <seealso cref="PrimitiveDatabase"/> 值初始化 <see cref="NDF.Data.Sql.SqlDatabase"/> 对象。
        /// </summary>
        /// <param name="sqlDatabase">指定的 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase"/> 对象，表示微软企业库提供的一个 SQL Server 数据库访问组件。</param>
        public SqlDatabase(Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase sqlDatabase) : base(sqlDatabase) { }



        /// <summary>
        /// 以指定的 SQL Server 数据库连接字符串获取一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">SQL Server 数据连接字符串。</param>
        /// <returns>一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</returns>
        protected override Microsoft.Practices.EnterpriseLibrary.Data.Database GetPrimitiveDatabase(string connectionString)
        {
            return this.GetPrimitiveSqlDatabase(connectionString);
        }


        /// <summary>
        /// 以指定的 SQL Server 数据库连接字符串获取一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">SQL Server数据库连接字符串</param>
        /// <returns>一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase"/> 对象。</returns>
        protected Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase GetPrimitiveSqlDatabase(string connectionString)
        {
            if (this._database == null)
            {
                this._database = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(connectionString);
            }
            return this._database;
        }



        /// <summary>
        /// 获取当前 <see cref="NDF.Data.Common.Database"/> 对象引用的原始 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 操作对象。
        /// </summary>
        public override Microsoft.Practices.EnterpriseLibrary.Data.Database PrimitiveDatabase
        {
            get
            {
                return this.PrimitiveSqlDatabase;
            }
            protected set
            {
                this.PrimitiveSqlDatabase = value as Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase;
            }
        }

        /// <summary>
        /// 获取当前 <see cref="NDF.Data.Sql.SqlDatabase"/> 对象引用的原始 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase"/> 操作对象。
        /// </summary>
        public Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase PrimitiveSqlDatabase
        {
            get;
            protected set;
        }
    }
}
