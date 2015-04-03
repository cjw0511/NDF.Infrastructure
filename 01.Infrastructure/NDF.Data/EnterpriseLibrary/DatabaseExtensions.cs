using Microsoft.Practices.EnterpriseLibrary.Data;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NDF.Data.EnterpriseLibrary
{
    /// <summary>
    /// 为 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 实例提供一组工具方法扩展。
    /// </summary>
    public static partial class DatabaseExtensions
    {
        /// <summary>
        /// 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbConnection"/> 对象，并在返回前立即将数据库链接对象打开(执行 Open 方法)。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbConnection"/> 对象，该对象处于打开状态。</returns>
        public static DbConnection GetNewOpenConnection(this Database database)
        {
            DbConnection connection = null;
            try
            {
                connection = database.CreateConnection();
                connection.Open();
            }
            catch
            {
                if (connection != null)
                    connection.Close();

                throw;
            }
            return connection;
        }



        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的数据库提供程序名称。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <returns>表示与当前 <see cref="DbProviderFactory"/> 关联的数据库提供程序名称。</returns>
        public static string GetProviderName(this Database database)
        {
            return NDF.Data.Common.DbProviderFactoryExtensions.GetProviderName(database.DbProviderFactory);
        }

        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="IScriptParameterParser"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <returns>表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="IScriptParameterParser"/> 对象。</returns>
        public static NDF.Data.Common.DbScriptParameterParser GetDbScriptParameterParser(this Database database)
        {
            return NDF.Data.Common.DbProviderFactoryExtensions.GetScriptParameterParser(database.DbProviderFactory);
        }

        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 查询参数名称前缀字符。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <returns>表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 查询参数名称前缀字符。</returns>
        public static string GetParameterToken(this Database database)
        {
            return GetDbScriptParameterParser(database).ParameterToken;
        }

        /// <summary>
        /// 解析 SQL 脚本中的参数名称列表并返回。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="sqlScript">待解析的 SQL 脚本。</param>
        /// <returns>返回 <paramref name="sqlScript"/> 中定义的所有脚本参数名称所构成的一个数组。</returns>
        public static string[] GetParameterNames(this Database database, string sqlScript)
        {
            return GetDbScriptParameterParser(database).GetParameterNames(sqlScript);
        }



        /// <summary>
        /// 通过执行 <paramref name="command"/> 来填充 <paramref name="dataTable"/> 数据表对象。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="command">表示待执行的 <see cref="System.Data.Common.DbCommand"/> 对象，其返回结果用于填充 <paramref name="dataTable"/>。</param>
        /// <param name="dataTable">待填充数据的 <see cref="System.Data.DataTable"/> 对象。</param>
        internal static void DoLoadDataTable(this Database database, DbCommand command, DataTable dataTable)
        {
            Check.NotNull(dataTable, "dataTable");
            using (DbDataAdapter adapter = database.GetDataAdapter())
            {
                ((IDbDataAdapter)adapter).SelectCommand = command;
                adapter.Fill(dataTable);
            }
        }

        /// <summary>
        /// 通过执行 <paramref name="insertCommand"/>、<paramref name="updateCommand"/> 和 <paramref name="deleteCommand"/> 来更新 <paramref name="dataTable"/> 数据表对象。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="dataTable">待更新数据的 <see cref="System.Data.DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示待执行的 <see cref="System.Data.Common.DbCommand"/> 对象，该对象执行一个插入数据的命令，其执行结果用于更新 <paramref name="dataTable"/>。</param>
        /// <param name="updateCommand">表示待执行的 <see cref="System.Data.Common.DbCommand"/> 对象，该对象执行一个修改数据的命令，其执行结果用于更新 <paramref name="dataTable"/>。</param>
        /// <param name="deleteCommand">表示待执行的 <see cref="System.Data.Common.DbCommand"/> 对象，该对象执行一个删除数据的命令，其执行结果用于更新 <paramref name="dataTable"/>。</param>
        /// <param name="updateBehavior">一个 <see cref="UpdateBehavior"/> 值，用于控制执行 Update 操作时当出现错误时的事务处理机制。</param>
        /// <param name="updateBatchSize">
        /// 该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。
        /// 值为 效果 0 批大小没有限制。 1 禁用批量更新。 > 1 更改是使用 System.Data.Common.DbDataAdapter.UpdateBatchSize 操作的批处理一次性发送的。 
        /// 将此属性设置为 1 以外的值时，所有与 <see cref="System.Data.Common.DbDataAdapter"/> 关联的命令都必须将它们
        /// 的 <seealso cref="System.Data.IDbCommand.UpdatedRowSource"/> 属性设置为 None 或 OutputParameters。 否则，将引发异常。
        /// </param>
        /// <returns><paramref name="insertCommand"/>、<paramref name="updateCommand"/> 和 <paramref name="deleteCommand"/>命令更新数据的总行数。</returns>
        internal static int DoUpdateDataTable(this Database database, DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, UpdateBehavior updateBehavior, int? updateBatchSize = null)
        {
            Check.NotNull(dataTable, "dataTable");
            Check.AnyNotNull(new DbCommand[] { insertCommand, updateCommand, deleteCommand }, "insertCommand、updateCommand、deleteCommand");
            using (DbDataAdapter adapter = database.GetDataAdapter())
            {
                IDbDataAdapter explicitAdapter = adapter;
                if (insertCommand != null)
                {
                    explicitAdapter.InsertCommand = insertCommand;
                }
                if (updateCommand != null)
                {
                    explicitAdapter.UpdateCommand = updateCommand;
                }
                if (deleteCommand != null)
                {
                    explicitAdapter.DeleteCommand = deleteCommand;
                }
                if (updateBatchSize != null)
                {
                    adapter.UpdateBatchSize = (int)updateBatchSize;
                    if (insertCommand != null)
                        adapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                    if (updateCommand != null)
                        adapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                    if (deleteCommand != null)
                        adapter.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
                }
                int rows = adapter.Update(dataTable);
                return rows;
            }
        }



        /// <summary>
        /// 在需要的情况下为 <paramref name="command"/> 配置一个 <paramref name="connection"/>。
        /// </summary>
        /// <param name="command">包含预处理查询的 <see cref="System.Data.Common.DbCommand"/> 对象。</param>
        /// <param name="connection">为 <paramref name="command"/> 配置的 <see cref="System.Data.Common.DbConnection"/> 对象。</param>
        internal static void PrepareCommand(DbCommand command, DbConnection connection)
        {
            Check.NotNull(command, "command");
            Check.NotNull(connection, "connection");
            command.Connection = connection;
        }

        /// <summary>
        /// 在需要的情况下为 <paramref name="command"/> 配置一个 <paramref name="transaction"/>。
        /// </summary>
        /// <param name="command">包含预处理查询的 <see cref="System.Data.Common.DbCommand"/> 对象。</param>
        /// <param name="transaction">为 <paramref name="command"/> 配置的 <see cref="System.Data.Common.DbTransaction"/> 对象。</param>
        internal static void PrepareCommand(DbCommand command, DbTransaction transaction)
        {
            Check.NotNull(command, "command");
            Check.NotNull(transaction, "transaction");
            PrepareCommand(command, transaction.Connection);
            command.Transaction = transaction;
        }



        /// <summary>
        /// 获取 <paramref name="database"/> 中已经打开的 <see cref="System.Data.Common.DbConnection"/> 包装器。
        /// 该方法的结果通过反射方式调用 <paramref name="database"/> 对象的受保护(protected)方法 GetOpenConnection 执行返回。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <returns><paramref name="database"/> 中已经打开的 <see cref="System.Data.Common.DbConnection"/> 包装器。</returns>
        internal static DatabaseConnectionWrapper GetOpenConnection(this Database database)
        {
            MethodInfo method = null;
            Type type = database.GetType();
            if (!GetOpenConnectionCache.TryGetValue(type, out method))
            {
                method = type.GetAllMethods("GetOpenConnection").FirstOrDefault();
                if (method == null)
                {
                    throw new MethodAccessException("在 database 对象所属的 System.Type 类型中未找到方法 GetOpenConnection。");
                }
                GetOpenConnectionCache.Add(type, method);
            }
            return method.Invoke(database, null) as DatabaseConnectionWrapper;
        }


        private static Dictionary<Type, MethodInfo> GetOpenConnectionCache = new Dictionary<Type, MethodInfo>();
    }
}
