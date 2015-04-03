using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.DB2
{
    /// <summary>
    /// 提供对 IBM DB2 数据库访问基础组件基类的定义；该组件提供一组方法，用于封装对 ADO.NET 中 IBM DB2 数据源便捷的进行访问。
    /// <remarks>在实现原理上该类型是 <see cref="NDF.Data.DB2.EnterpriseLibrary.DB2Database"/> 的一个包装器。</remarks>
    /// </summary>
    public class DB2Database : Database
    {
        private NDF.Data.DB2.EnterpriseLibrary.DB2Database _database;

        /// <summary>
        /// 以指定的数据库连接字符串初始化 <see cref="NDF.Data.DB2.DB2Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        public DB2Database(string connectionString) : base(connectionString) { }

        /// <summary>
        /// 以 <paramref name="db2Database"/> 作为 <seealso cref="PrimitiveDatabase"/> 值初始化 <see cref="NDF.Data.DB2.DB2Database"/> 对象。
        /// </summary>
        /// <param name="db2Database">指定的 <see cref="DB2Database"/> 对象，表示微软企业库提供的一个 IBM DB2 数据库访问组件。</param>
        public DB2Database(NDF.Data.DB2.EnterpriseLibrary.DB2Database db2Database) : base(db2Database) { }



        /// <summary>
        /// 以指定的 IBM DB2 数据库连接字符串获取一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">IBM DB2 数据连接字符串。</param>
        /// <returns>一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</returns>
        protected override Microsoft.Practices.EnterpriseLibrary.Data.Database GetPrimitiveDatabase(string connectionString)
        {
            return this.GetPrimitiveDB2Database(connectionString);
        }


        /// <summary>
        /// 以指定的 IBM DB2 数据库连接字符串获取一个 <see cref="NDF.Data.DB2.EnterpriseLibrary.DB2Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">IBM DB2 数据库连接字符串</param>
        /// <returns>一个 <see cref="NDF.Data.DB2.EnterpriseLibrary.DB2Database"/> 对象。</returns>
        protected NDF.Data.DB2.EnterpriseLibrary.DB2Database GetPrimitiveDB2Database(string connectionString)
        {
            if (this._database == null)
            {
                this._database = new NDF.Data.DB2.EnterpriseLibrary.DB2Database(connectionString);
            }
            return this._database;
        }





        /// <summary>
        /// 获取当前 <see cref="NDF.Data.Common.Database"/> 对象引用的原始 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 操作对象。
        /// 该对象实际上是一个 <see cref="NDF.Data.DB2.EnterpriseLibrary.DB2Database"/>。
        /// </summary>
        public override Microsoft.Practices.EnterpriseLibrary.Data.Database PrimitiveDatabase
        {
            get
            {
                return this.PrimitiveDB2Database;
            }
            protected set
            {
                this.PrimitiveDB2Database = value as NDF.Data.DB2.EnterpriseLibrary.DB2Database;
            }
        }

        /// <summary>
        /// 获取当前 <see cref="NDF.Data.DB2.DB2Database"/> 对象引用的原始 <see cref="NDF.Data.DB2.EnterpriseLibrary.DB2Database"/> 操作对象。
        /// </summary>
        public NDF.Data.DB2.EnterpriseLibrary.DB2Database PrimitiveDB2Database
        {
            get;
            protected set;
        }
    }
}
