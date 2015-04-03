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
        /// 执行 <paramref name="command"/> 命令。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        public static void ExecuteNone(this Database database, DbCommand command)
        {
            database.ExecuteNonQuery(command);
        }

        /// <summary>
        /// 执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        public static void ExecuteNone(this Database database, string commandText)
        {
            ExecuteNone(database, commandText, CommandType.Text);
        }

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public static void ExecuteNone(this Database database, string commandText, params DbParameter[] parameterValues)
        {
            ExecuteNone(database, commandText, CommandType.Text, parameterValues);
        }

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public static void ExecuteNone(this Database database, string commandText, params object[] parameterValues)
        {
            ExecuteNone(database, commandText, CommandType.Text, parameterValues);
        }

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        public static void ExecuteNone(this Database database, string commandText, CommandType commandType)
        {
            using (DbCommand command = CreateCommand(database, commandText, commandType))
            {
                ExecuteNone(database, command);
            }
        }

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public static void ExecuteNone(this Database database, string commandText, CommandType commandType, params DbParameter[] parameterValues)
        {
            using (DbCommand command = CreateCommand(database, commandText, commandType, parameterValues))
            {
                ExecuteNone(database, command);
            }
        }

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public static void ExecuteNone(this Database database, string commandText, CommandType commandType, params object[] parameterValues)
        {
            using (DbCommand command = CreateCommand(database, commandText, commandType, parameterValues))
            {
                ExecuteNone(database, command);
            }
        }


        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行 <paramref name="command"/> 命令。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        public static void ExecuteNone(this Database database, DbTransaction transaction, DbCommand command)
        {
            ExecuteNonQuery(database, transaction, command);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        public static void ExecuteNone(this Database database, DbTransaction transaction, string commandText)
        {
            ExecuteNone(database, transaction, commandText, CommandType.Text);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public static void ExecuteNone(this Database database, DbTransaction transaction, string commandText, params DbParameter[] parameterValues)
        {
            ExecuteNone(database, transaction, commandText, CommandType.Text, parameterValues);
        }
        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>

        public static void ExecuteNone(this Database database, DbTransaction transaction, string commandText, params object[] parameterValues)
        {
            ExecuteNone(database, transaction, commandText, CommandType.Text, parameterValues);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        public static void ExecuteNone(this Database database, DbTransaction transaction, string commandText, CommandType commandType)
        {
            using (DbCommand command = CreateCommand(database, commandText, commandType))
            {
                ExecuteNone(database, transaction, command);
            }
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public static void ExecuteNone(this Database database, DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] parameterValues)
        {
            using (DbCommand command = CreateCommand(database, commandText, commandType, parameterValues))
            {
                ExecuteNone(database, transaction, command);
            }
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public static void ExecuteNone(this Database database, DbTransaction transaction, string commandText, CommandType commandType, params object[] parameterValues)
        {
            using (DbCommand command = CreateCommand(database, commandText, commandType, parameterValues))
            {
                ExecuteNone(database, transaction, command);
            }
        }

    }
}
