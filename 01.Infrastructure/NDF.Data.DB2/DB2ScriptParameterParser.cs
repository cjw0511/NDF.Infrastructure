using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.DB2
{
    /// <summary>
    /// 定义解析 SQL(适用于 IBM DB2 数据库) 脚本中参数名称列表的方法。
    /// </summary>
    [DbProvider("IBM.Data.DB2")]
    public class DB2ScriptParameterParser : DbScriptParameterParser
    {
        /// <summary>
        /// 初始化 <see cref="DB2ScriptParameterParser"/> 类型的实例。
        /// </summary>
        public DB2ScriptParameterParser() : base() { }

        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序名称初始化 <see cref="DB2ScriptParameterParser"/> 类型的实例。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称。该参数不能为除 "IBM.Data.DB2" 之外的其他值。</param>
        public DB2ScriptParameterParser(string providerName)
            : base(providerName)
        {
            DB2DatabaseFactory.CheckProviderNameIsDB2Client(providerName);
        }
    }
}
