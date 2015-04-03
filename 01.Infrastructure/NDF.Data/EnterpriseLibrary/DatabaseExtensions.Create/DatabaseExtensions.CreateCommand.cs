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
        /// 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <returns>一个 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        public static DbCommand CreateCommand(this Database database)
        {
            return database.DbProviderFactory.CreateCommand();
        }

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">指定的 SQL 脚本命令文本内容。</param>
        /// <returns>一个指定了 CommandText 属性的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        public static DbCommand CreateCommand(this Database database, string commandText)
        {
            var command = CreateCommand(database);
            command.CommandText = commandText;
            return command;
        }

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容和 <see cref="CommandType"/> 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">指定的SQL脚本命令文本内容。</param>
        /// <param name="commandType">指定的 <see cref="CommandType"/> 类型枚举值。</param>
        /// <returns>一个指定了 CommandText 和 CommandType 属性的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        public static DbCommand CreateCommand(this Database database, string commandText, System.Data.CommandType commandType)
        {
            var command = CreateCommand(database, commandText);
            command.CommandType = commandType;
            return command;
        }

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容执行参数列表创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">指定的SQL脚本命令文本内容。</param>
        /// <param name="parameterValues">表示 <see cref="System.Data.Common.DbCommand"/> 的执行参数。</param>
        /// <returns>一个指定了 CommandText 属性和执行参数列表的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        public static DbCommand CreateCommand(this Database database, string commandText, params System.Data.Common.DbParameter[] parameterValues)
        {
            var command = CreateCommand(database, commandText);
            command.Parameters.AddRange(parameterValues);
            return command;
        }

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容执行参数列表创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">指定的SQL脚本命令文本内容。</param>
        /// <param name="parameterValues">表示 <see cref="System.Data.Common.DbCommand"/> 的执行参数。</param>
        /// <returns>一个指定了 CommandText 属性和执行参数列表的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        public static DbCommand CreateCommand(this Database database, string commandText, params object[] parameterValues)
        {
            var command = CreateCommand(database, commandText);
            AssignParameters(database, command, parameterValues);
            return command;
        }

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容、<see cref="CommandType"/> 和执行参数列表创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">指定的SQL脚本命令文本内容。</param>
        /// <param name="commandType">指定的 <see cref="CommandType"/> 类型枚举值。</param>
        /// <param name="parameterValues">表示 <see cref="System.Data.Common.DbCommand"/> 的执行参数。</param>
        /// <returns>一个指定了 CommandText、CommandType 属性和执行参数列表的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        public static DbCommand CreateCommand(this Database database, string commandText, System.Data.CommandType commandType, params System.Data.Common.DbParameter[] parameterValues)
        {
            var command = CreateCommand(database, commandText, commandType);
            command.Parameters.AddRange(parameterValues);
            return command;
        }

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容、<see cref="CommandType"/> 和执行参数列表创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">指定的SQL脚本命令文本内容。</param>
        /// <param name="commandType">指定的 <see cref="CommandType"/> 类型枚举值。</param>
        /// <param name="parameterValues">表示 <see cref="System.Data.Common.DbCommand"/> 的执行参数。</param>
        /// <returns>一个指定了 CommandText、CommandType 属性和执行参数列表的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        public static DbCommand CreateCommand(this Database database, string commandText, System.Data.CommandType commandType, params object[] parameterValues)
        {
            var command = CreateCommand(database, commandText, commandType);
            AssignParameters(database, command, parameterValues);
            return command;
        }

    }
}
