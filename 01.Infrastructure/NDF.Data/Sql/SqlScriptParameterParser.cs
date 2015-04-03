using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Sql
{
    /// <summary>
    /// 定义解析 T-SQL(适用于 Microsoft SQL Server 数据库) 脚本中参数名称列表的方法。
    /// </summary>
    [DbProvider("System.Data.SqlClient")]
    public class SqlScriptParameterParser : DbScriptParameterParser
    {
        /// <summary>
        /// 初始化 <see cref="SqlScriptParameterParser"/> 类型的实例。
        /// </summary>
        public SqlScriptParameterParser() : base() { }

        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序名称初始化 <see cref="SqlScriptParameterParser"/> 类型的实例。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称。该参数不能为除 "System.Data.SqlClient" 之外的其他值。</param>
        public SqlScriptParameterParser(string providerName) : base(providerName)
        {
            SqlDatabaseFactory.CheckProviderNameIsSqlClient(providerName);
        }
    }
}
