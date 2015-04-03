using Microsoft.Practices.EnterpriseLibrary.Data;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EnterpriseLibrary
{
    public static partial class DatabaseExtensions
    {

        /// <summary>
        /// 找出 <paramref name="command"/> 脚本中定义的参数列表，并将参数列表加入其属性 Parameters 中。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="command">用于查找参数列表的 <see cref="DbCommand"/> 对象。</param>
        public static void DiscoverParameters(Database database, DbCommand command)
        {
            if (command.CommandType == CommandType.StoredProcedure)
            {
                database.DiscoverParameters(command);
                return;
            }
            Check.NotEmpty(command.CommandText, "command.CommandText");
            var parser = NDF.Data.Common.DbProviderFactoryExtensions.GetScriptParameterParser(database.DbProviderFactory);
            string[] parameterNames = parser.GetParameterNames(command.CommandText);
            foreach (var paremeterName in parameterNames)
            {
                DbParameter parameter = CreateParameter(database, paremeterName);
                command.Parameters.Add(parameter);
            }
        }

    }
}
