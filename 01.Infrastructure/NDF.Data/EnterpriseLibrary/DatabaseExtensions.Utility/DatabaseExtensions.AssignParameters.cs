using Microsoft.Practices.EnterpriseLibrary.Data;
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
        /// 找出 <paramref name="command"/> 脚本中定义的参数列表，并将 <paramref name="parameterValues"/> 作为参数值分配给该参数列表。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="command">用于查找参数列表的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="parameterValues">用于作为参数值分配给 <paramref name="command"/> 参数列表的值集合。</param>
        public static void AssignParameters(Database database, DbCommand command, params object[] parameterValues)
        {
            if (command.CommandType == CommandType.StoredProcedure)
            {
                database.AssignParameters(command, parameterValues);
                return;
            }
            DiscoverParameters(database, command);
            if (command.Parameters.Count != parameterValues.Length)
            {
                throw new InvalidOperationException("command 对象中的参数集合元素数量与 parameterValues 元素数量不一致。");
            }
            for (int i = 0; i < parameterValues.Length; i++)
            {
                IDataParameter parameter = command.Parameters[i];
                string parameterName = database.BuildParameterName(parameter.ParameterName);
                command.Parameters[parameterName].Value = parameterValues[i] ?? DBNull.Value;
            }
        }

    }
}
