using NDF.Data.Common;
using NDF.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity
{
    /// <summary>
    /// 为 <see cref="System.Data.Entity.Database"/> 实例提供一组工具方法扩展。
    /// </summary>
    public static partial class DatabaseExtensions
    {
        private static readonly Lazy<Dictionary<string, NDF.Data.Common.Database>> _databaseCache = new Lazy<Dictionary<string, Common.Database>>(() => new Dictionary<string, NDF.Data.Common.Database>());


        /// <summary>
        /// 基于当前 <paramref name="database"/> 获取与其相同数据库环境的 <see cref="NDF.Data.Common.Database"/> 实例对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>与 <paramref name="database"/> 相同数据库环境的 <see cref="NDF.Data.Common.Database"/> 实例对象。</returns>
        public static NDF.Data.Common.Database GetGeneralDatabase(this System.Data.Entity.Database database)
        {
            string connectionString = GetConnectionString(database);
            lock (_databaseCache)
            {
                NDF.Data.Common.Database generalDatabase = null;
                if (!_databaseCache.Value.TryGetValue(connectionString, out generalDatabase))
                {
                    string providerName = GetProviderName(database);
                    generalDatabase = DatabaseFactories.GetFactory(providerName).CreateDatabase(connectionString);
                    _databaseCache.Value.Add(connectionString, generalDatabase);
                }
                return generalDatabase;
            }
        }

        
        /// <summary>
        /// 解析 SQL 脚本中的参数名称列表并返回。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <param name="sqlScript">待解析的 SQL 脚本。</param>
        /// <returns>返回 <paramref name="sqlScript"/> 中定义的所有脚本参数名称所构成的一个数组。</returns>
        public static string[] GetParameterNames(this System.Data.Entity.Database database, string sqlScript)
        {
            DbScriptParameterParser parser = GetDbScriptParameterParser(database);
            return parser.GetParameterNames(sqlScript);
        }


        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 查询参数名称前缀字符。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 查询参数名称前缀字符。</returns>
        public static string GetParameterToken(this System.Data.Entity.Database database)
        {
            DbScriptParameterParser parser = GetDbScriptParameterParser(database);
            return parser.ParameterToken;
        }

        /// <summary>
        /// 获取该数据库访问对象的数据库连接字符串值。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>该数据库访问对象的数据库连接字符串值。</returns>
        public static string GetConnectionString(this System.Data.Entity.Database database)
        {
            return database.Connection.ConnectionString;
        }

        /// <summary>
        /// 返回当前 <paramref name="database"/> 所使用的 <see cref="DbProviderFactory"/> 工厂对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>当前 <paramref name="database"/> 所使用的 <see cref="DbProviderFactory"/> 工厂对象。</returns>
        public static DbProviderFactory GetDbProviderFactory(this System.Data.Entity.Database database)
        {
            Check.NotNull(database);
            DbConnection connection = database.Connection;
            DbProviderFactory factory = DbProviderFactories.GetFactory(connection);
            return factory;
        }

        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的数据库提供程序名称。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>表示与当前 <see cref="DbProviderFactory"/> 关联的数据库提供程序名称。</returns>
        public static string GetProviderName(this System.Data.Entity.Database database)
        {
            DbProviderFactory factory = GetDbProviderFactory(database);
            string providerName = factory.GetProviderName();
            return providerName;
        }

        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="IScriptParameterParser"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="IScriptParameterParser"/> 对象。</returns>
        public static DbScriptParameterParser GetDbScriptParameterParser(this System.Data.Entity.Database database)
        {
            string providerName = GetProviderName(database);
            DbScriptParameterParser parser = DbScriptParameterParser.GetScriptParameterParser(providerName);
            return parser;
        }


        /// <summary>
        /// 检查数据库是否与当前 Code First 模型兼容。
        /// <para>该方法等效于调用方法时 <see cref="System.Data.Entity.Database.CompatibleWithModel(Boolean)"/> 传入参数 false。</para>
        /// </summary>
        /// <param name="_this">表示 <see cref="System.Data.Entity.DbContext"/> 实体上下文对象中引用的 数据库实例。</param>
        /// <returns>如果该数据库实例与当前 Code First 模型兼容，则返回 true，否则返回 false。</returns>
        /// <remarks>
        /// 该方法内部调用 <see cref="System.Data.Entity.Database"/> 对象的 CompatibleWithModel(false) 进行判断。
        /// </remarks>
        public static bool CompatibleWithModel(this System.Data.Entity.Database _this)
        {
            return _this.CompatibleWithModel(false);
        }

    }
}
