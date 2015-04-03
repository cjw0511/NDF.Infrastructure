using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Generic
{
    /// <summary>
    /// 定义解析普通 SQL(适用于 Access 等 OleDb 接口数据库) 脚本中参数名称列表的方法。
    /// </summary>
    [DbProvider("System.Data.OleDb")]
    public class GenericScriptParameterParser : DbScriptParameterParser
    {
        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序名称初始化 <see cref="GenericScriptParameterParser"/> 类型的实例。
        /// </summary>
        public GenericScriptParameterParser(string providerName) : base(providerName) { }

    }
}
