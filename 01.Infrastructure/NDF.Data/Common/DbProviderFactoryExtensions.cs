using NDF.Utilities;
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
    /// 
    /// </summary>
    public static class DbProviderFactoryExtensions
    {
        static DbProviderFactoryClass[] _factoryClasses;
        static Dictionary<Type, string> _factoryMapping;
        static Dictionary<string, string> _parameterTokenMapping;



        /// <summary>
        /// 获取该数据库提供程序的名称。
        /// </summary>
        /// <param name="factory">表示一个 <see cref="System.Data.Common.DbProviderFactory"/> 对象。</param>
        /// <returns>该数据库提供程序的名称，例如 <see cref="System.Data.SqlClient.SqlClientFactory"/> 对象返回 "System.Data.SqlClient"。</returns>
        public static string GetProviderName(this DbProviderFactory factory)
        {
            Type factoryType = factory.GetType();
            string providerName = null;
            if (!DbProviderFactoryMapping.TryGetValue(factoryType, out providerName))
            {
                DbProviderFactoryClass factoryClass = DbProviderFactoryClasses.FirstOrDefault(item => Type.GetType(item.AssemblyQualifiedName) == factoryType);
                providerName = factoryClass == null ? factoryType.Namespace : factoryClass.InvariantName;
                DbProviderFactoryMapping.Add(factoryType, providerName);
            }
            return providerName;
        }

        /// <summary>
        /// 根据数据库提供程序的描述信息对象获取其 SQL 脚本环境下的查询参数名称前缀字符。
        /// </summary>
        /// <param name="providerFactory">数据库提供程序的描述信息对象 <see cref="DbProviderFactoryClass"/> 。</param>
        /// <returns>根据数据库提供程序的描述信息对象获取其 SQL 脚本环境下的查询参数名称前缀字符。</returns>
        public static string GetParameterToken(this DbProviderFactoryClass providerFactory)
        {
            return GetParemeterToken(providerFactory.InvariantName);
        }

        /// <summary>
        /// 为 <see cref="System.Data.Common.DbProviderFactory"/> 实例提供一组工具方法扩展。
        /// </summary>
        /// <param name="factory">表示一个 <see cref="System.Data.Common.DbProviderFactory"/> 对象。</param>
        /// <returns>返回一个 <see cref="NDF.Data.Common.DbScriptParameterParser"/> 对象，该对象可用于对 SQL 脚本中定义的参数列表进行解析。</returns>
        public static DbScriptParameterParser GetScriptParameterParser(this DbProviderFactory factory)
        {
            string providerName = GetProviderName(factory);
            return DbScriptParameterParser.GetScriptParameterParser(providerName);
        }







        /// <summary>
        /// 检查传入的数据库提供程序名称 <paramref name="providerName"/> 是否在当前应用程序运行环境中已经注册。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称。</param>
        /// <exception cref="InvalidOperationException">如果传入的数据库提供程序名称 <paramref name="providerName"/> 在当前应用程序运行环境中尚未注册，则抛出该异常。</exception>
        internal static void CheckProviderNameRegisted(string providerName)
        {
            if (!DbProviderFactoryClasses.Any(item => item.InvariantName == providerName))
            {
                throw new InvalidOperationException(string.Format(Resources.UnregisteredProviderName, providerName));
            }
        }



        /// <summary>
        /// 根据数据库提供程序名称获取其 SQL 脚本环境下的查询参数名称前缀字符。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称</param>
        /// <returns>根据数据库提供程序名称获取其 SQL 脚本环境下的查询参数名称前缀字符。</returns>
        internal static string GetParemeterToken(string providerName)
        {
            Check.NotEmpty(providerName, "providerName");
            string parameterToken = null;
            if (!ParameterTokenMapping.TryGetValue(providerName, out parameterToken))
            {
                throw new ArgumentException(string.Format(Resources.UnregisteredProviderName, providerName), "providerName");
            }
            return parameterToken;
        }



        /// <summary>
        /// 获取当前应用程序运行环境中的所有已安装数据库提供程序的描述信息。
        /// </summary>
        public static DbProviderFactoryClass[] DbProviderFactoryClasses
        {
            get
            {
                if (_factoryClasses == null)
                {
                    List<DbProviderFactoryClass> list = new List<DbProviderFactoryClass>();
                    DataTable classes = DbProviderFactories.GetFactoryClasses();
                    foreach (DataRow row in classes.Rows)
                    {
                        DbProviderFactoryClass item = new DbProviderFactoryClass();
                        item.Name = Convert.ToString(row["Name"]);
                        item.Description = Convert.ToString(row["Description"]);
                        item.InvariantName = Convert.ToString(row["InvariantName"]);
                        item.AssemblyQualifiedName = Convert.ToString(row["AssemblyQualifiedName"]);
                        list.Add(item);
                    }
                    var types = typeof(DbProviderFactory).GetSubClass().Where(t => !t.IsAbstract);
                    foreach (Type type in types)
                    {
                        if (!list.Any(item => item.AssemblyQualifiedName == type.AssemblyQualifiedName || item.InvariantName == type.Namespace))
                        {
                            DbProviderFactoryClass item = new DbProviderFactoryClass();
                            string name = type.Name.Replace("Factory", "");
                            item.Name = name + " Data Provider";
                            item.Description = ".NET Framework Data Provider for " + name;
                            item.InvariantName = type.Namespace;
                            item.AssemblyQualifiedName = type.AssemblyQualifiedName;
                            list.Add(item);
                        }
                    }
                    _factoryClasses = list.Where(item =>
                    {
                        Type type = null;
                        return Types.TryGetType(item.AssemblyQualifiedName, out type);
                    }).ToArray();
                }
                return _factoryClasses;
            }
        }

        /// <summary>
        /// 获取实现 <see cref="System.Data.Common.DbProviderFactory"/> 的所有已安装提供程序的信息集合。
        /// 该属性返回一个 <see cref="Dictionary&lt;TKey, TValue&gt;"/>，字典对象中每个元素的 string(Key) 表示数据库提供程序名称，元素的 Type(Value) 表示数据库提供程序的实例。
        /// </summary>
        internal static Dictionary<Type, string> DbProviderFactoryMapping
        {
            get
            {
                if (_factoryMapping == null)
                {
                    _factoryMapping = new Dictionary<Type, string>();
                    foreach (DbProviderFactoryClass item in DbProviderFactoryClasses)
                    {
                        Type providerType = Type.GetType(item.AssemblyQualifiedName);
                        if (providerType != null)
                        {
                            _factoryMapping.Add(providerType, item.InvariantName);
                        }
                    }
                }
                return _factoryMapping;
            }
        }

        /// <summary>
        /// 获取数据库提供程序名称与其其 SQL 脚本环境下的查询参数名称前缀字符的关联关系映射集合。
        /// </summary>
        internal static Dictionary<string, string> ParameterTokenMapping
        {
            get
            {
                if (_parameterTokenMapping == null)
                {
                    _parameterTokenMapping = new Dictionary<string,string>();
                    _parameterTokenMapping.Add("System.Data.Odbc", "@");
                    _parameterTokenMapping.Add("System.Data.OleDb", "@");
                    _parameterTokenMapping.Add("System.Data.OracleClient", ":");
                    _parameterTokenMapping.Add("System.Data.SqlClient", "@");
                    _parameterTokenMapping.Add("System.Data.SqlServerCe.3.5", "@");
                    _parameterTokenMapping.Add("System.Data.SqlServerCe.4.0", "@");
                    _parameterTokenMapping.Add("MySql.Data.MySqlClient", "@");
                    _parameterTokenMapping.Add("Oracle.ManagedDataAccess.Client", "@");
                    _parameterTokenMapping.Add("IBM.Data.DB2", "@");
                }
                return _parameterTokenMapping;
            }
        }
    }
}
