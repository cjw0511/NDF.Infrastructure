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
    /// <summary>
    /// 提供对数据库访问基础组件基类的定义；该组件提供一组方法，用于封装对 ADO.NET 数据源便捷的进行访问。
    /// <remarks>在实现原理上该类型是 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 的一个包装器。</remarks>
    /// </summary>
    public abstract partial class Database : IDatabase
    {
        private string _providerName;
        private DbProviderFactory _dbProviderFactory;
        private DbScriptParameterParser _scriptParameterParser;



        /// <summary>
        /// 以指定的数据库连接字符串初始化 <see cref="NDF.Data.Common.Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        protected Database(string connectionString)
        {
            this.PrimitiveDatabase = this.GetPrimitiveDatabase(connectionString);
        }

        /// <summary>
        /// 以指定的数据库连接字符串和数据库提供程序名称初始化 <see cref="NDF.Data.Common.Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <param name="providerName">数据库提供程序名称。</param>
        protected Database(string connectionString, string providerName)
        {
            DbProviderFactoryExtensions.CheckProviderNameRegisted(providerName);
            this._providerName = providerName;
            this.PrimitiveDatabase = this.GetPrimitiveDatabase(connectionString);
        }

        /// <summary>
        /// 以 <paramref name="database"/> 作为 <seealso cref="PrimitiveDatabase"/> 值初始化 <see cref="NDF.Data.Common.Database"/> 对象。
        /// </summary>
        /// <param name="database">指定的 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象，表示微软企业库提供的一个数据库访问组件。</param>
        protected Database(Microsoft.Practices.EnterpriseLibrary.Data.Database database)
        {
            this.PrimitiveDatabase = database;
        }



        /// <summary>
        /// 获取当前 <see cref="NDF.Data.Common.Database"/> 对象引用的原始 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 操作对象。
        /// </summary>
        public virtual Microsoft.Practices.EnterpriseLibrary.Data.Database PrimitiveDatabase
        {
            get;
            protected set;
        }

        /// <summary>
        /// 以指定的数据库连接字符串获取一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据连接字符串。</param>
        /// <returns>一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</returns>
        protected abstract Microsoft.Practices.EnterpriseLibrary.Data.Database GetPrimitiveDatabase(string connectionString);




        /// <summary>
        /// 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbConnection"/> 对象，并在返回前立即将数据库链接对象打开(执行 Open 方法)。
        /// </summary>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbConnection"/> 对象，该对象处于打开状态。</returns>
        public virtual DbConnection GetNewOpenConnection()
        {
            return DatabaseExtensions.GetNewOpenConnection(this.PrimitiveDatabase);
        }


        /// <summary>
        /// 获取 <see cref="Database"/> 对象中已经打开的 <see cref="System.Data.Common.DbConnection"/> 对象。
        /// </summary>
        /// <returns><see cref="Database"/> 对象中已经打开的 <see cref="System.Data.Common.DbConnection"/> 包装器。</returns>
        public virtual DbConnection GetOpenConnection()
        {
            return DatabaseExtensions.GetOpenConnection(this.PrimitiveDatabase).Connection;
        }




        /// <summary>
        /// 解析 SQL 脚本中的参数名称列表并返回。
        /// </summary>
        /// <param name="sqlScript">待解析的 SQL 脚本。</param>
        /// <returns>返回 <paramref name="sqlScript"/> 中定义的所有脚本参数名称所构成的一个数组。</returns>
        public virtual string[] GetParameterNames(string sqlScript)
        {
            return this.DbScriptParameterParser.GetParameterNames(sqlScript);
        }



        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 查询参数名称前缀字符。
        /// </summary>
        public virtual string ParameterToken
        {
            get { return this.DbScriptParameterParser.ParameterToken; }
        }

        /// <summary>
        /// 获取该数据库访问对象的数据库连接字符串值。
        /// </summary>
        public string ConnectionString
        {
            get { return this.PrimitiveDatabase.ConnectionString; }
        }

        /// <summary>
        /// 获取该数据库访问对象的不带安全凭据信息的数据库连接字符串值。
        /// </summary>
        public string ConnectionStringWithoutCredentials
        {
            get { return this.PrimitiveDatabase.ConnectionStringWithoutCredentials; }
        }

        /// <summary>
        /// 返回当前 IDatabase 所使用的 <see cref="DbProviderFactory"/> 工厂对象。
        /// </summary>
        public virtual DbProviderFactory DbProviderFactory
        {
            get
            {
                if (this._dbProviderFactory == null)
                {
                    this._dbProviderFactory = string.IsNullOrWhiteSpace(this._providerName) ? this.PrimitiveDatabase.DbProviderFactory : DbProviderFactories.GetFactory(this._providerName);
                }
                return this._dbProviderFactory;
            }
        }



        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的数据库提供程序名称。
        /// </summary>
        public virtual string ProviderName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._providerName))
                {
                    this._providerName = this.DbScriptParameterParser.ProviderName;
                }
                return this._providerName;
            }
        }

        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="IScriptParameterParser"/> 对象。
        /// </summary>
        public IScriptParameterParser ScriptParameterParser
        {
            get
            {
                return this.DbScriptParameterParser;
            }
        }

        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="IScriptParameterParser"/> 对象。
        /// </summary>
        public virtual DbScriptParameterParser DbScriptParameterParser
        {
            get
            {
                if (this._scriptParameterParser == null)
                {
                    this._scriptParameterParser = DbScriptParameterParser.GetScriptParameterParser(this.ProviderName);
                }
                return this._scriptParameterParser;
            }
        }
    }
}
