using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.MySql
{
    /// <summary>
    /// 定义解析 SQL(适用于 MySQL 数据库) 脚本中参数名称列表的方法。
    /// </summary>
    [DbProvider("MySql.Data.MySqlClient")]
    public class MySqlScriptParameterParser : DbScriptParameterParser
    {
        /// <summary>
        /// 初始化 <see cref="MySqlScriptParameterParser"/> 类型的实例。
        /// </summary>
        public MySqlScriptParameterParser() : base() { }

        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序名称初始化 <see cref="MySqlScriptParameterParser"/> 类型的实例。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称。该参数不能为除 "MySql.Data.MySqlClient" 之外的其他值。</param>
        public MySqlScriptParameterParser(string providerName) : base(providerName)
        {
            MySqlDatabaseFactory.CheckProviderNameIsMySqlClient(providerName);
        }
    }
}
