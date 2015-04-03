using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Oracle
{
    /// <summary>
    /// 定义解析 PL-SQL(适用于 Oracle 数据库) 脚本中参数名称列表的方法。
    /// </summary>
    [DbProvider("Oracle.ManagedDataAccess.Client")]
    public class OracleScriptParameterParser : DbScriptParameterParser
    {
        /// <summary>
        /// 初始化 <see cref="OracleScriptParameterParser"/> 类型的实例。
        /// </summary>
        public OracleScriptParameterParser() : base() { }

        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序名称初始化 <see cref="OracleScriptParameterParser"/> 类型的实例。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称。该参数不能为除 "Oracle.ManagedDataAccess.Client" 之外的其他值。</param>
        public OracleScriptParameterParser(string providerName) : base(providerName)
        {
            OracleDatabaseFactory.CheckProviderNameIsOracleClient(providerName);
        }
    }
}
