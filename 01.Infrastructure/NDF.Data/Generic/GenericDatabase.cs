using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Generic
{
    /// <summary>
    /// 提供对普通 SQL 数据库(适用于 Access 等 OleDb 接口数据库) 访问基础组件基类的定义；该组件提供一组方法，用于封装对 ADO.NET 中 OleDb 接口数据库等类型通用数据源便捷的进行访问。
    /// <remarks>在实现原理上该类型是 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase"/> 的一个包装器。</remarks>
    /// </summary>
    public class GenericDatabase : Database
    {
        private Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase _database;

        /// <summary>
        /// 以指定的数据库连接字符串初始化 <see cref="NDF.Data.Generic.GenericDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <param name="providerName">数据库提供程序名称。</param>
        public GenericDatabase(string connectionString, string providerName) : base(connectionString, providerName)
        {
        }


        /// <summary>
        /// 以 <paramref name="genericDatabase"/> 作为 <seealso cref="PrimitiveDatabase"/> 值初始化 <see cref="NDF.Data.Generic.GenericDatabase"/> 对象。
        /// </summary>
        /// <param name="genericDatabase">指定的 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase"/> 对象，表示微软企业库提供的一个普通 SQL 数据库(适用于 Access 等 OleDb 接口数据库) 访问组件。</param>
        public GenericDatabase(Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase genericDatabase) : base(genericDatabase) { }


        /// <summary>
        /// 以指定的数据库连接字符串获取一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据连接字符串。</param>
        /// <returns>一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase"/> 对象。</returns>
        protected override Microsoft.Practices.EnterpriseLibrary.Data.Database GetPrimitiveDatabase(string connectionString)
        {
            return this.GetPrimitiveGenericDatabase(connectionString);
        }

        /// <summary>
        /// 以指定的数据库连接字符串获取一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase"/> 对象。</returns>
        protected Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase GetPrimitiveGenericDatabase(string connectionString)
        {
            if (this._database == null)
            {
                this._database = new Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase(connectionString, this.DbProviderFactory);
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
                return this.PrimitiveGenericDatabase;
            }
            protected set
            {
                this.PrimitiveGenericDatabase = value as Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase;
            }
        }

        /// <summary>
        /// 获取当前 <see cref="NDF.Data.Generic.GenericDatabase"/> 对象引用的原始 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase"/> 操作对象。
        /// </summary>
        public Microsoft.Practices.EnterpriseLibrary.Data.GenericDatabase PrimitiveGenericDatabase
        {
            get;
            protected set;
        }
    }
}
