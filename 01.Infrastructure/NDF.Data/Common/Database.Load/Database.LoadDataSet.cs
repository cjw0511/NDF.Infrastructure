using NDF.Data.EnterpriseLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Common
{
    public abstract partial class Database
    {
        /// <summary>
        /// 执行 <paramref name="command"/> 命令并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        public virtual void LoadDataSet(DataSet dataSet, DbCommand command)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, command);
        }

        /// <summary>
        /// 执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        public void LoadDataSet(DataSet dataSet, string commandText)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, commandText);
        }

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, string commandText, params DbParameter[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, commandText, parameterValues);
        }

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, string commandText, params object[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, commandText, parameterValues);
        }

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        public void LoadDataSet(DataSet dataSet, string commandText, CommandType commandType)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, commandText, commandType);
        }

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, string commandText, CommandType commandType, params DbParameter[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, commandText, commandType, parameterValues);
        }

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, string commandText, CommandType commandType, params object[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, commandText, commandType, parameterValues);
        }


        /// <summary>
        /// 执行 <paramref name="command"/> 命令并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        public virtual void LoadDataSet(DataSet dataSet, string[] tableNames, DbCommand command)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, tableNames, command);
        }

        /// <summary>
        /// 执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        public void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, tableNames, commandText);
        }

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText, params DbParameter[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, tableNames, commandText, parameterValues);
        }

        /// <summary>
        /// 以指定的脚本参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText, params object[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, tableNames, commandText, parameterValues);
        }

        /// <summary>
        /// 以指定的脚本类型执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        public void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText, CommandType commandType)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, tableNames, commandText, commandType);
        }

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText, CommandType commandType, params DbParameter[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, tableNames, commandText, commandType, parameterValues);
        }

        /// <summary>
        /// 以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, string[] tableNames, string commandText, CommandType commandType, params object[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, tableNames, commandText, commandType, parameterValues);
        }



        /// <summary>
        /// 作为事务处理的一部分执行 <paramref name="command"/> 命令并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        public virtual void LoadDataSet(DataSet dataSet, DbTransaction transaction, DbCommand command)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, command);
        }

        /// <summary>
        /// 作为事务处理的一部分执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, commandText);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText, params DbParameter[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, commandText, parameterValues);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText, params object[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, commandText, parameterValues);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText, CommandType commandType)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, commandText, commandType);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, commandText, commandType, parameterValues);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string commandText, CommandType commandType, params object[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, commandText, commandType, parameterValues);
        }


        /// <summary>
        /// 作为事务处理的一部分执行 <paramref name="command"/> 命令并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        public virtual void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, DbCommand command)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, tableNames, command);
        }

        /// <summary>
        /// 作为事务处理的一部分执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, tableNames, commandText);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText, params DbParameter[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, tableNames, commandText, parameterValues);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText, params object[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, tableNames, commandText, parameterValues);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText, CommandType commandType)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, tableNames, commandText, commandType);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText, CommandType commandType, params DbParameter[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, tableNames, commandText, commandType, parameterValues);
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行一个 SQL 脚本并将返回结果数据以指定的表名称填充至 <paramref name="dataSet"/> 中。
        /// </summary>
        /// <param name="dataSet">用于填充脚本命令返回结果数据的 <see cref="DataSet"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="tableNames">表示指定的表名称数组。</param>
        /// <param name="commandText">表示要执行的 SQL 脚本文本内容。</param>
        /// <param name="commandType">表示一个 <see cref="CommandType"/> 值用于指示 <paramref name="commandText"/> 的类型。</param>
        /// <param name="parameterValues">表示用于执行 <paramref name="commandText"/> 命令的参数列表。</param>
        public void LoadDataSet(DataSet dataSet, DbTransaction transaction, string[] tableNames, string commandText, CommandType commandType, params object[] parameterValues)
        {
            DatabaseExtensions.LoadDataSet(this.PrimitiveDatabase, dataSet, transaction, tableNames, commandText, commandType, parameterValues);
        }

    }
}
