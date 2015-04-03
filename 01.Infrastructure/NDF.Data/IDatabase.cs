using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data
{
    /// <summary>
    /// 提供对数据库访问基础组件的定义；该组件提供一组方法，用于封装对 ADO.NET 数据源便捷的进行访问。
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// 找出 <paramref name="command"/> 脚本中定义的参数列表，并将 <paramref name="parameterValues"/> 作为参数值分配给该参数列表。
        /// </summary>
        /// <param name="command">用于查找参数列表的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="parameterValues">用于作为参数值分配给 <paramref name="command"/> 参数列表的值集合。</param>
        void AssignParameters(DbCommand command, params object[] parameterValues);

        /// <summary>
        /// 找出 <paramref name="command"/> 脚本中定义的参数列表，并将参数列表加入其属性 Parameters 中。
        /// </summary>
        /// <param name="command">用于查找参数列表的 <see cref="DbCommand"/> 对象。</param>
        void DiscoverParameters(DbCommand command);



        /// <summary>
        /// 开始一个数据库事务。
        /// </summary>
        /// <returns>一个数据库事务 <see cref="System.Data.Common.DbTransaction"/> 对象。</returns>
        DbTransaction BeginTransaction();

        /// <summary>
        /// 以指定的事务锁定级别开始一个数据库事务。
        /// </summary>
        /// <param name="isolationLevel">指定的数据库事务锁定级别 <see cref="IsolationLevel"/>。</param>
        /// <returns>一个指定了事务锁定级别的数据库事务 <see cref="System.Data.Common.DbTransaction"/> 对象。</returns>
        DbTransaction BeginTransaction(IsolationLevel isolationLevel);


        /// <summary>
        /// 根据指明的变量名称，创建一个适用于当前数据库类型（依据当前对象的 <see cref="DbProviderFactory"/> 属性所创建的 <see cref="DbConnection"/>）的变量参数名称。
        /// </summary>
        /// <param name="name">指明的用于包装变量参数名称。</param>
        /// <returns>一个表示可用于表示当前数据库类型（依据当前对象的 <see cref="DbProviderFactory"/> 属性所创建的 <see cref="DbConnection"/>）的变量参数名称。</returns>
        string BuildParameterName(string name);



        /// <summary>
        /// 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <returns>一个 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        DbCommand CreateCommand();

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="commandText">指定的 SQL 脚本命令文本内容。</param>
        /// <returns>一个指定了 CommandText 属性的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        DbCommand CreateCommand(string commandText);

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容和 <see cref="CommandType"/> 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="commandText">指定的SQL脚本命令文本内容。</param>
        /// <param name="commandType">指定的 <see cref="CommandType"/> 类型枚举值。</param>
        /// <returns>一个指定了 CommandText 和 CommandType 属性的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        DbCommand CreateCommand(string commandText, System.Data.CommandType commandType);

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容执行参数列表创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="commandText">指定的SQL脚本命令文本内容。</param>
        /// <param name="parameterValues">表示 <see cref="System.Data.Common.DbCommand"/> 的执行参数。</param>
        /// <returns>一个指定了 CommandText 属性和执行参数列表的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        DbCommand CreateCommand(string commandText, params System.Data.Common.DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容执行参数列表创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="commandText">指定的SQL脚本命令文本内容。</param>
        /// <param name="parameterValues">表示 <see cref="System.Data.Common.DbCommand"/> 的执行参数。</param>
        /// <returns>一个指定了 CommandText 属性和执行参数列表的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        DbCommand CreateCommand(string commandText, params object[] parameterValues);

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容、<see cref="CommandType"/> 和执行参数列表创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="commandText">指定的SQL脚本命令文本内容。</param>
        /// <param name="commandType">指定的 <see cref="CommandType"/> 类型枚举值。</param>
        /// <param name="parameterValues">表示 <see cref="System.Data.Common.DbCommand"/> 的执行参数。</param>
        /// <returns>一个指定了 CommandText、CommandType 属性和执行参数列表的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        DbCommand CreateCommand(string commandText, System.Data.CommandType commandType, params System.Data.Common.DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的 SQL 脚本命令文本内容、<see cref="CommandType"/> 和执行参数列表创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 对象。
        /// </summary>
        /// <param name="commandText">指定的SQL脚本命令文本内容。</param>
        /// <param name="commandType">指定的 <see cref="CommandType"/> 类型枚举值。</param>
        /// <param name="parameterValues">表示 <see cref="System.Data.Common.DbCommand"/> 的执行参数。</param>
        /// <returns>一个指定了 CommandText、CommandType 属性和执行参数列表的 <see cref="DbCommand"/> 对象，同时其与当前 <see cref="DbProviderFactory"/> 关联。</returns>
        DbCommand CreateCommand(string commandText, System.Data.CommandType commandType, params object[] parameterValues);


        /// <summary>
        /// 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbConnection"/> 对象。
        /// </summary>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbConnection"/> 对象。</returns>
        DbConnection CreateConnection();

        /// <summary>
        /// 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbDataAdapter CreateDataAdapter();



        /// <summary>
        /// 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter();

        /// <summary>
        /// 以指定的名称创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter(string parameterName);


        /// <summary>
        /// 以指定的名称、数据类型为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter(string parameterName, DbType dbType);

        /// <summary>
        /// 以指定的名称、长度为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="size">参数长度。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter(string parameterName, int size);

        /// <summary>
        /// 以指定的名称、数据类型、长度为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter(string parameterName, DbType dbType, int size);

        /// <summary>
        /// 以指定的名称、数据类型、长度、输入输出类型、可空类型为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <param name="direction">一个表示参数为输入还是输出类型的枚举值。</param>
        /// <param name="isNullable"></param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter(string parameterName, DbType dbType, int size, ParameterDirection direction, bool isNullable);

        /// <summary>
        /// 以指定的名称、数据类型、长度、输入输出类型、可空类型等配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。 
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <param name="direction">一个表示参数为输入还是输出类型的枚举值。</param>
        /// <param name="isNullable">参数是否可为空(DBNull.Value)。</param>
        /// <param name="sourceColumn">参数的源列名称，该源列映射到 <see cref="System.Data.DataSet"/> 并用于加载或返回 <seealso cref="System.Data.Common.DbParameter.Value"/>。</param>
        /// <param name="sourceColumnNullMapping">指示源列是否可以为 null。 这使得 <see cref="System.Data.Common.DbCommandBuilder"/> 能够正确地为可以为 null 的列生成 Update 语句。</param>
        /// <param name="sourceVersion">指示参数在加载 <seealso cref="System.Data.Common.DbParameter.Value"/> 时使用的 <see cref="System.Data.DataRowVersion"/>。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter(string parameterName, DbType dbType, int size, ParameterDirection direction, bool isNullable, string sourceColumn, bool sourceColumnNullMapping, DataRowVersion sourceVersion);


        /// <summary>
        /// 以指定的名称、值为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter(string parameterName, object value);

        /// <summary>
        /// 以指定的名称、值、数据类型为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter(string parameterName, object value, DbType dbType);

        /// <summary>
        /// 以指定的名称、值、长度为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="size">参数长度。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter(string parameterName, object value, int size);

        /// <summary>
        /// 以指定的名称、值、数据类型、长度为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <returns></returns>
        DbParameter CreateParameter(string parameterName, object value, DbType dbType, int size);

        /// <summary>
        /// 以指定的名称、值、数据类型、长度、输入输出类型、可空类型为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <param name="direction">一个表示参数为输入还是输出类型的枚举值。</param>
        /// <param name="isNullable">参数是否可为空(DBNull.Value)。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter(string parameterName, object value, DbType dbType, int size, ParameterDirection direction, bool isNullable);

        /// <summary>
        /// 以指定的名称、值、数据类型、长度、输入输出类型、可空类型等配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <param name="direction">一个表示参数为输入还是输出类型的枚举值。</param>
        /// <param name="isNullable">参数是否可为空(DBNull.Value)。</param>
        /// <param name="sourceColumn">参数的源列名称，该源列映射到 <see cref="System.Data.DataSet"/> 并用于加载或返回 <seealso cref="System.Data.Common.DbParameter.Value"/>。</param>
        /// <param name="sourceColumnNullMapping">指示源列是否可以为 null。 这使得 <see cref="System.Data.Common.DbCommandBuilder"/> 能够正确地为可以为 null 的列生成 Update 语句。</param>
        /// <param name="sourceVersion">指示参数在加载 <seealso cref="System.Data.Common.DbParameter.Value"/> 时使用的 <see cref="System.Data.DataRowVersion"/>。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        DbParameter CreateParameter(string parameterName, object value, DbType dbType, int size, ParameterDirection direction, bool isNullable, string sourceColumn, bool sourceColumnNullMapping, DataRowVersion sourceVersion);



        /// <summary>
        /// 执行 <paramref name="command"/> 命令并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示 <paramref name="command"/> 执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(DbCommand command);

        /// <summary>
        /// 执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(string commandText);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(string commandText, params object[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(string commandText, CommandType commandType);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(string commandText, CommandType commandType, params object[] parameterValues);


        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行 <paramref name="command"/> 命令并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(DbTransaction transaction, DbCommand command);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(DbTransaction transaction, string commandText);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(DbTransaction transaction, string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(DbTransaction transaction, string commandText, params object[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(DbTransaction transaction, string commandText, CommandType commandType);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataSet"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataSet"/> 对象。</returns>
        DataSet ExecuteDataSet(DbTransaction transaction, string commandText, CommandType commandType, params object[] parameterValues);




        /// <summary>
        /// 执行 <paramref name="command"/> 命令并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示 <paramref name="command"/> 执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(DbCommand command);

        /// <summary>
        /// 执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(string commandText);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(string commandText, params object[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(string commandText, CommandType commandType);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(string commandText, CommandType commandType, params object[] parameterValues);


        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行 <paramref name="command"/> 命令并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(DbTransaction transaction, DbCommand command);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(DbTransaction transaction, string commandText);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(DbTransaction transaction, string commandText, params DbParameter[] parameterValues);
        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>

        DataTable ExecuteDataTable(DbTransaction transaction, string commandText, params object[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(DbTransaction transaction, string commandText, CommandType commandType);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataTable"/> 对象。</returns>
        DataTable ExecuteDataTable(DbTransaction transaction, string commandText, CommandType commandType, params object[] parameterValues);





        /// <summary>
        /// 执行 <paramref name="command"/> 命令并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示 <paramref name="command"/> 执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(DbCommand command);

        /// <summary>
        /// 执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(string commandText);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(string commandText, params object[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(string commandText, CommandType commandType);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(string commandText, CommandType commandType, params object[] parameterValues);


        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行 <paramref name="command"/> 命令并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(DbTransaction transaction, DbCommand command);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(DbTransaction transaction, string commandText);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(DbTransaction transaction, string commandText, params DbParameter[] parameterValues);
        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>

        DataView ExecuteDataView(DbTransaction transaction, string commandText, params object[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(DbTransaction transaction, string commandText, CommandType commandType);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="DataView"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="DataView"/> 对象。</returns>
        DataView ExecuteDataView(DbTransaction transaction, string commandText, CommandType commandType, params object[] parameterValues);





        /// <summary>
        /// 执行 <paramref name="command"/> 命令。
        /// </summary>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        void ExecuteNone(DbCommand command);

        /// <summary>
        /// 执行一个 SQL 脚本。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        void ExecuteNone(string commandText);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void ExecuteNone(string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void ExecuteNone(string commandText, params object[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        void ExecuteNone(string commandText, CommandType commandType);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void ExecuteNone(string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void ExecuteNone(string commandText, CommandType commandType, params object[] parameterValues);


        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行 <paramref name="command"/> 命令。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        void ExecuteNone(DbTransaction transaction, DbCommand command);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        void ExecuteNone(DbTransaction transaction, string commandText);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void ExecuteNone(DbTransaction transaction, string commandText, params DbParameter[] parameterValues);
        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>

        void ExecuteNone(DbTransaction transaction, string commandText, params object[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        void ExecuteNone(DbTransaction transaction, string commandText, CommandType commandType);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void ExecuteNone(DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void ExecuteNone(DbTransaction transaction, string commandText, CommandType commandType, params object[] parameterValues);





        /// <summary>
        /// 执行 <paramref name="command"/> 命令并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(DbCommand command);

        /// <summary>
        /// 执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(string commandText);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(string commandText, params object[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(string commandText, CommandType commandType);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(string commandText, CommandType commandType, params object[] parameterValues);


        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行 <paramref name="command"/> 命令并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(DbTransaction transaction, DbCommand command);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(DbTransaction transaction, string commandText);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(DbTransaction transaction, string commandText, params DbParameter[] parameterValues);
        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>

        int ExecuteNonQuery(DbTransaction transaction, string commandText, params object[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(DbTransaction transaction, string commandText, CommandType commandType);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回受影响的行数。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int ExecuteNonQuery(DbTransaction transaction, string commandText, CommandType commandType, params object[] parameterValues);





        /// <summary>
        /// 执行 <paramref name="command"/> 命令并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示 <paramref name="command"/> 执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(DbCommand command);

        /// <summary>
        /// 执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(string commandText);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(string commandText, params object[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(string commandText, CommandType commandType);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(string commandText, CommandType commandType, params object[] parameterValues);


        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行 <paramref name="command"/> 命令并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(DbTransaction transaction, DbCommand command);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(DbTransaction transaction, string commandText);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(DbTransaction transaction, string commandText, params DbParameter[] parameterValues);
        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>

        IDataReader ExecuteReader(DbTransaction transaction, string commandText, params object[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(DbTransaction transaction, string commandText, CommandType commandType);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回一个新创建的 <see cref="IDataReader"/> 对象。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="IDataReader"/> 对象。</returns>
        IDataReader ExecuteReader(DbTransaction transaction, string commandText, CommandType commandType, params object[] parameterValues);





        /// <summary>
        /// 执行 <paramref name="command"/> 命令并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示 <paramref name="command"/> 执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(DbCommand command);

        /// <summary>
        /// 执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(string commandText);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(string commandText, params object[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(string commandText, CommandType commandType);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(string commandText, CommandType commandType, params object[] parameterValues);


        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行 <paramref name="command"/> 命令并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(DbTransaction transaction, DbCommand command);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(DbTransaction transaction, string commandText);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(DbTransaction transaction, string commandText, params DbParameter[] parameterValues);
        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>

        object ExecuteScalar(DbTransaction transaction, string commandText, params object[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(DbTransaction transaction, string commandText, CommandType commandType);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并根据执行结果返回查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略。
        /// </summary>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        /// <returns>表示脚本命令执行的查询所返回的结果集中的第一行第一列。所有其他的行和列将会被忽略</returns>
        object ExecuteScalar(DbTransaction transaction, string commandText, CommandType commandType, params object[] parameterValues);




        /// <summary>
        /// 执行 <paramref name="command"/> 命令并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        void LoadDataSet(DataSet dataSet, DbCommand command);

        /// <summary>
        /// 执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        void LoadDataSet(DataSet dataSet, string commandText);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, string commandText, params object[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        void LoadDataSet(DataSet dataSet, string commandText, CommandType commandType);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, string commandText, CommandType commandType, params object[] parameterValues);


        /// <summary>
        /// 执行 <paramref name="command"/> 命令并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        void LoadDataSet(DataSet dataSet, string[] tableNames, DbCommand command);

        /// <summary>
        /// 执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText, params object[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText, CommandType commandType);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText, CommandType commandType, params object[] parameterValues);



        /// <summary>
        /// 作为事务处理的一部分执行 <paramref name="command"/> 命令并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, DbCommand command);

        /// <summary>
        /// 作为事务处理的一部分执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText, params object[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText, CommandType commandType);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText, CommandType commandType, params object[] parameterValues);


        /// <summary>
        /// 作为事务处理的一部分执行 <paramref name="command"/> 命令并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, DbCommand command);

        /// <summary>
        /// 作为事务处理的一部分执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText);

        /// <summary>
        /// 作为事务处理的一部分以指定的参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText, params object[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText, CommandType commandType);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText, CommandType commandType, params object[] parameterValues);




        /// <summary>
        /// 执行 <paramref name="command"/> 命令并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="command"></param>
        void LoadDataTable(DataTable dataTable, DbCommand command);

        /// <summary>
        /// 执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        void LoadDataTable(DataTable dataTable, string commandText);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataTable(DataTable dataTable, string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataTable(DataTable dataTable, string commandText, params object[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        void LoadDataTable(DataTable dataTable, string commandText, CommandType commandType);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataTable(DataTable dataTable, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataTable(DataTable dataTable, string commandText, CommandType commandType, params object[] parameterValues);



        /// <summary>
        /// 作为事务处理的一部分执行 <paramref name="command"/> 命令并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="command"></param>
        void LoadDataTable(DataTable dataTable, DbTransaction transaction, DbCommand command);

        /// <summary>
        /// 作为事务处理的一部分执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        void LoadDataTable(DataTable dataTable, DbTransaction transaction, string commandText);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataTable(DataTable dataTable, DbTransaction transaction, string commandText, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataTable(DataTable dataTable, DbTransaction transaction, string commandText, params object[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        void LoadDataTable(DataTable dataTable, DbTransaction transaction, string commandText, CommandType commandType);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataTable(DataTable dataTable, DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataTable"/> 中。
        /// </summary>
        /// <param name="dataTable">用于填充脚本命令返回结果数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        void LoadDataTable(DataTable dataTable, DbTransaction transaction, string commandText, CommandType commandType, params object[] parameterValues);




        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataSet"/> 中特定名称的表的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataSet">待更新数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableName">指示 <paramref name="dataSet"/> 中待更新的数据表的名称。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataSet(DataSet dataSet, string tableName, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand);

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataSet"/> 中特定名称的表的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataSet">待更新数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableName">指示 <paramref name="dataSet"/> 中待更新的数据表的名称。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBehavior">一个 <see cref="UpdateBehavior"/> 值，用于控制执行 Update 操作时当出现错误时的事务处理机制。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataSet(DataSet dataSet, string tableName, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, UpdateBehavior updateBehavior);

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataSet"/> 中特定名称的表的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataSet">待更新数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableName">指示 <paramref name="dataSet"/> 中待更新的数据表的名称。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataSet(DataSet dataSet, string tableName, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, int? updateBatchSize);

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataSet"/> 中特定名称的表的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataSet">待更新数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableName">指示 <paramref name="dataSet"/> 中待更新的数据表的名称。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <param name="updateBehavior">一个 <see cref="UpdateBehavior"/> 值，用于控制执行 Update 操作时当出现错误时的事务处理机制。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataSet(DataSet dataSet, string tableName, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, UpdateBehavior updateBehavior, int? updateBatchSize);


        /// <summary>
        /// 作为事务处理的一部分以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataSet"/> 中特定名称的表的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataSet">待更新数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableName">指示 <paramref name="dataSet"/> 中待更新的数据表的名称。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataSet(DataSet dataSet, string tableName, DbTransaction transaction, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand);

        /// <summary>
        /// 作为事务处理的一部分以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataSet"/> 中特定名称的表的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataSet">待更新数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableName">指示 <paramref name="dataSet"/> 中待更新的数据表的名称。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataSet(DataSet dataSet, string tableName, DbTransaction transaction, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, int? updateBatchSize);



        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataTable(DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand);

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBehavior">一个 <see cref="UpdateBehavior"/> 值，用于控制执行 Update 操作时当出现错误时的事务处理机制。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataTable(DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, UpdateBehavior updateBehavior);

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataTable(DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, int? updateBatchSize);

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBehavior">一个 <see cref="UpdateBehavior"/> 值，用于控制执行 Update 操作时当出现错误时的事务处理机制。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataTable(DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, UpdateBehavior updateBehavior, int? updateBatchSize);


        /// <summary>
        /// 作为事务处理的一部分以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataTable(DataTable dataTable, DbTransaction transaction, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand);

        /// <summary>
        /// 作为事务处理的一部分以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        int UpdateDataTable(DataTable dataTable, DbTransaction transaction, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, int? updateBatchSize);




        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 查询参数名称前缀字符。
        /// </summary>
        string ParameterToken { get; }

        /// <summary>
        /// 获取该数据库访问对象的数据库连接字符串值。
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// 返回当前 IDatabase 所使用的 <see cref="DbProviderFactory"/> 工厂对象。
        /// </summary>
        DbProviderFactory DbProviderFactory { get; }


        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="IScriptParameterParser"/> 对象。
        /// </summary>
        IScriptParameterParser ScriptParameterParser { get; }
    }
}
