using NDF.Data.Generic;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Common
{
    /// <summary>
    /// 定义解析 SQL 脚本中参数名称列表的方法。
    /// </summary>
    public abstract class DbScriptParameterParser : IScriptParameterParser
    {
        private string _parameterToken;
        private string _providerName;
        static Dictionary<string, DbScriptParameterParser> _scriptParameterParserCache;

        /// <summary>
        /// 初始化 <see cref="DbScriptParameterParser"/> 类型的实例。
        /// </summary>
        public DbScriptParameterParser() { }

        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序名称初始化 <see cref="DbScriptParameterParser"/> 类型的实例。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称</param>
        public DbScriptParameterParser(string providerName)
        {
            Check.NotEmpty(providerName, "providerName");
            DbProviderFactoryExtensions.CheckProviderNameRegisted(providerName);
            this._providerName = providerName;
        }



        /// <summary>
        /// 解析 SQL 脚本中的参数名称列表并返回。
        /// </summary>
        /// <param name="sqlScript">待解析的 SQL 脚本。</param>
        /// <returns>返回 <paramref name="sqlScript"/> 中定义的所有脚本参数名称所构成的一个数组。</returns>
        public virtual string[] GetParameterNames(string sqlScript)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrWhiteSpace(sqlScript))
            {
                return list.ToArray();
            }
            foreach (string line in sqlScript.Split('\n', '\r', '\t'))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                for (int i = 0; i < line.Length; i++)
                {
                    var c = line[i];
                    if (c.ToString() == this.ParameterToken)
                    {
                        StringBuilder builder = new StringBuilder();
                        while (i < (line.Length - 1) && line[++i].IsNormal())
                        {
                            builder.Append(line[i]);
                        }
                        var name = builder.ToString();
                        if (name.Length > 0) { list.Add(name); }
                    }
                }
            }
            return list.Distinct(StringComparer.CurrentCultureIgnoreCase).ToArray();
        }


        /// <summary>
        /// 获取表示查询参数名称前缀的字符。
        /// </summary>
        public virtual string ParameterToken
        {
            get
            {
                if (this._parameterToken == null)
                {
                    this._parameterToken = DbProviderFactoryExtensions.GetParemeterToken(this.ProviderName);
                }
                return this._parameterToken;
            }
        }

        /// <summary>
        /// 获取表示与当前工厂对象的数据库提供程序名称。
        /// </summary>
        public virtual string ProviderName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._providerName))
                {
                    DbProviderAttribute attribute = this.GetType().GetCustomAttributes(typeof(DbProviderAttribute), true).OfType<DbProviderAttribute>().FirstOrDefault();
                    if (attribute == null)
                    {
                        throw new InvalidOperationException(Resources.NotDefinedDbProviderAttribute);
                    }
                    this._providerName = attribute.ProviderName;
                }
                return this._providerName;
            }
        }


        /// <summary>
        /// 根据数据库提供程序名称获取 <see cref="NDF.Data.Common.DbScriptParameterParser"/> 对象，该对象可用于对 SQL 脚本中定义的参数列表进行解析。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称.</param>
        /// <returns>返回一个 <see cref="NDF.Data.Common.DbScriptParameterParser"/> 对象，该对象可用于对 SQL 脚本中定义的参数列表进行解析。</returns>
        public static DbScriptParameterParser GetScriptParameterParser(string providerName)
        {
            Check.NotEmpty(providerName, "providerName");
            DbScriptParameterParser parser = null;
            if (!DbScriptParameterParserCache.TryGetValue(providerName, out parser))
            {
                Type type = typeof(DbScriptParameterParser).GetSubClass().FirstOrDefault(t =>
                {
                    var attribute = t.GetCustomAttributes(typeof(DbProviderAttribute), true).OfType<DbProviderAttribute>().FirstOrDefault();
                    return attribute != null && attribute.ProviderName == providerName;
                });
                if (type == null)
                {
                    if (!DbProviderFactoryExtensions.DbProviderFactoryClasses.Any(item => item.InvariantName == providerName))
                    {
                        throw new InvalidOperationException(string.Format(Resources.NotExistsAllowedDbProviderAttributeDbScriptParameterParser, providerName));
                    }
                    type = typeof(GenericScriptParameterParser);
                }
                parser = Activator.CreateInstance(type, providerName) as DbScriptParameterParser;
                DbScriptParameterParserCache.Add(providerName, parser);
            }
            return parser;
        }




        /// <summary>
        /// 表示数据库提供程序名称与 SQL 脚本参数列表解析器 <see cref="DbScriptParameterParser"/> 的映射缓存。
        /// </summary>
        internal static Dictionary<string, DbScriptParameterParser> DbScriptParameterParserCache
        {
            get
            {
                if (_scriptParameterParserCache == null)
                {
                    _scriptParameterParserCache = new Dictionary<string, DbScriptParameterParser>();
                }
                return _scriptParameterParserCache;
            }
        }
    }
}
