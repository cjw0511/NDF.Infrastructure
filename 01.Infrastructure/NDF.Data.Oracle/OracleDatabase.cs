using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Oracle
{
    /// <summary>
    /// 提供对 Oracle 数据库访问基础组件基类的定义；该组件提供一组方法，用于封装对 ADO.NET 中 Oracle 数据源便捷的进行访问。
    /// <remarks>在实现原理上该类型是 <see cref="NDF.Data.Oracle.EnterpriseLibrary.OracleDatabase"/> 的一个包装器。</remarks>
    /// </summary>
    [DbProvider("Oracle.ManagedDataAccess.Client")]
    public class OracleDatabase : Database
    {
        private NDF.Data.Oracle.EnterpriseLibrary.OracleDatabase _database;
        

        /// <summary>
        /// 以指定的数据库连接字符串初始化 <see cref="NDF.Data.Oracle.OracleDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        public OracleDatabase(string connectionString) : base(connectionString) { }

        /// <summary>
        /// 以 <paramref name="oracleDatabase"/> 作为 <seealso cref="PrimitiveDatabase"/> 值初始化 <see cref="NDF.Data.Oracle.OracleDatabase"/> 对象。
        /// </summary>
        /// <param name="oracleDatabase">指定的 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Oracle.OracleDatabase"/> 对象，表示微软企业库提供的一个 Oracle 数据库访问组件。</param>
        public OracleDatabase(NDF.Data.Oracle.EnterpriseLibrary.OracleDatabase oracleDatabase) : base(oracleDatabase) { }



        /// <summary>
        /// 以指定的 Oracle 数据库连接字符串获取一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">Oracle 数据连接字符串。</param>
        /// <returns>一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</returns>
        protected override Microsoft.Practices.EnterpriseLibrary.Data.Database GetPrimitiveDatabase(string connectionString)
        {
            return this.GetPrimitiveOracleDatabase(connectionString);
        }

        /// <summary>
        /// 以指定的 Oracle 数据库连接字符串获取一个 <see cref="NDF.Data.Oracle.EnterpriseLibrary.OracleDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">Oracle 数据库连接字符串</param>
        /// <returns>一个 <see cref="NDF.Data.Oracle.EnterpriseLibrary.OracleDatabase"/> 对象。</returns>
        protected NDF.Data.Oracle.EnterpriseLibrary.OracleDatabase GetPrimitiveOracleDatabase(string connectionString)
        {
            if (this._database == null)
            {
                this._database = new NDF.Data.Oracle.EnterpriseLibrary.OracleDatabase(connectionString);
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
                return this.PrimitiveOracleDatabase;
            }
            protected set
            {
                this.PrimitiveOracleDatabase = value as NDF.Data.Oracle.EnterpriseLibrary.OracleDatabase;
            }
        }

        /// <summary>
        /// 获取当前 <see cref="NDF.Data.Oracle.OracleDatabase"/> 对象引用的原始 <see cref="NDF.Data.Oracle.EnterpriseLibrary.OracleDatabase"/> 操作对象。
        /// </summary>
        public NDF.Data.Oracle.EnterpriseLibrary.OracleDatabase PrimitiveOracleDatabase
        {
            get;
            protected set;
        }
    }
}
