using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.MySql
{
    /// <summary>
    /// 提供对 MySQL 数据库访问基础组件基类的定义；该组件提供一组方法，用于封装对 ADO.NET 中 MySQL 数据源便捷的进行访问。
    /// <remarks>在实现原理上该类型是 <see cref="MySqlDatabase"/> 的一个包装器。</remarks>
    /// </summary>
    public class MySqlDatabase : Database
    {
        private NDF.Data.MySql.EnterpriseLibrary.MySqlDatabase _database;

        /// <summary>
        /// 以指定的数据库连接字符串初始化 <see cref="NDF.Data.MySql.MySqlDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        public MySqlDatabase(string connectionString) : base(connectionString) { }

        /// <summary>
        /// 以 <paramref name="mySqlDatabase"/> 作为 <seealso cref="PrimitiveDatabase"/> 值初始化 <see cref="NDF.Data.MySql.MySqlDatabase"/> 对象。
        /// </summary>
        /// <param name="mySqlDatabase">指定的 <see cref="MySqlDatabase"/> 对象，表示微软企业库提供的一个 MySQL 数据库访问组件。</param>
        public MySqlDatabase(NDF.Data.MySql.EnterpriseLibrary.MySqlDatabase mySqlDatabase) : base(mySqlDatabase) { }



        /// <summary>
        /// 以指定的 MySQL 数据库连接字符串获取一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">MySQL 数据连接字符串。</param>
        /// <returns>一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</returns>
        protected override Microsoft.Practices.EnterpriseLibrary.Data.Database GetPrimitiveDatabase(string connectionString)
        {
            return this.GetPrimitiveMySqlDatabase(connectionString);
        }


        /// <summary>
        /// 以指定的 MySQL 数据库连接字符串获取一个 <see cref="NDF.Data.MySql.EnterpriseLibrary.MySqlDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">MySQL 数据库连接字符串</param>
        /// <returns>一个 <see cref="NDF.Data.MySql.EnterpriseLibrary.MySqlDatabase"/> 对象。</returns>
        protected NDF.Data.MySql.EnterpriseLibrary.MySqlDatabase GetPrimitiveMySqlDatabase(string connectionString)
        {
            if (this._database == null)
            {
                this._database = new NDF.Data.MySql.EnterpriseLibrary.MySqlDatabase(connectionString);
            }
            return this._database;
        }





        /// <summary>
        /// 获取当前 <see cref="NDF.Data.Common.Database"/> 对象引用的原始 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 操作对象。
        /// 该对象实际上是一个 <see cref="NDF.Data.MySql.EnterpriseLibrary.MySqlDatabase"/>。
        /// </summary>
        public override Microsoft.Practices.EnterpriseLibrary.Data.Database PrimitiveDatabase
        {
            get
            {
                return this.PrimitiveMySqlDatabase;
            }
            protected set
            {
                this.PrimitiveMySqlDatabase = value as NDF.Data.MySql.EnterpriseLibrary.MySqlDatabase;
            }
        }

        /// <summary>
        /// 获取当前 <see cref="NDF.Data.MySql.MySqlDatabase"/> 对象引用的原始 <see cref="NDF.Data.MySql.EnterpriseLibrary.MySqlDatabase"/> 操作对象。
        /// </summary>
        public NDF.Data.MySql.EnterpriseLibrary.MySqlDatabase PrimitiveMySqlDatabase
        {
            get;
            protected set;
        }
    }
}
