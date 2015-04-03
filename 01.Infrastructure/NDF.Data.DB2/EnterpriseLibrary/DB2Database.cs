using IBM.Data.DB2;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NDF.Data.DB2.EnterpriseLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NDF.Data.DB2.EnterpriseLibrary
{
    /// <summary>
    /// 表示一个 IBM DB2 数据库的命令执行程序。
    /// </summary>
    [ConfigurationElementType(typeof(DB2DatabaseData))]
    public class DB2Database : Database
    {
        /// <summary>
        /// 以指定的 IBM DB2 数据库连接字符串初始化一个 <see cref="DB2Database"/> 实例。
        /// </summary>
        /// <param name="connectionString">表示一个 IBM DB2 数据库连接字符串。</param>
        public DB2Database(string connectionString)
            : base(connectionString, DB2Factory.Instance)
        {
        }


        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="DB2Command"/> 实例。</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        public void AddInParameter(DbCommand command, string name, DB2Type dbType)
        {
            this.AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="DB2Command"/> 实例。</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        /// <param name="value">表示被加入的查询参数的参数值。</param>
        public void AddInParameter(DbCommand command, string name, DB2Type dbType, object value)
        {
            this.AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="DB2Command"/> 实例。</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        /// <param name="sourceColumn">参数的源列名称，该源列映射到 <see cref="System.Data.DataSet"/> 并用于加载或返回 <seealso cref="System.Data.Common.DbParameter.Value"/>。</param>
        /// <param name="sourceVersion">指示参数在加载 <seealso cref="System.Data.Common.DbParameter.Value"/> 时使用的 <see cref="System.Data.DataRowVersion"/>。</param>
        public void AddInParameter(DbCommand command, string name, DB2Type dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            this.AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }

        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="DB2Command"/> 实例。</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        public void AddOutParameter(DbCommand command, string name, DB2Type dbType, int size)
        {
            ParameterDirection direction = ParameterDirection.Output;
            string conns = ConnectionString.ToUpper(CultureInfo.CurrentCulture);
            if (conns.Contains("SERVERTYPE=UNIDATA") && direction == ParameterDirection.Output)
            {
                direction = ParameterDirection.InputOutput;
            }
            // standard set of OutParameter for DB2 and IDS and UniVerse
            this.AddParameter(command, name, dbType, size, direction, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="DB2Command"/> 实例。</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <param name="direction">一个表示参数为输入还是输出类型的枚举值。</param>
        /// <param name="nullable">参数是否可为空(DBNull.Value)。</param>
        /// <param name="precision">表示查询参数的小数精度。</param>
        /// <param name="scale">表述参数的 Scale 属性。</param>
        /// <param name="sourceColumn">参数的源列名称，该源列映射到 <see cref="System.Data.DataSet"/> 并用于加载或返回 <seealso cref="System.Data.Common.DbParameter.Value"/>。</param>
        /// <param name="sourceVersion">指示参数在加载 <seealso cref="System.Data.Common.DbParameter.Value"/> 时使用的 <see cref="System.Data.DataRowVersion"/>。</param>
        /// <param name="value">表示被加入的查询参数的参数值。</param>
        public virtual void AddParameter(DbCommand command, string name, DB2Type dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DbParameter parameter = this.CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="DB2Command"/> 实例。</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        /// <param name="direction"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        /// <param name="value">表示被加入的查询参数的参数值。</param>
        public void AddParameter(DbCommand command, string name, DB2Type dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            this.AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }



        /// <summary>
        /// 创建一个 <see cref="DbParameter"/> 脚本查询参数对象。
        /// </summary>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <param name="direction">一个表示参数为输入还是输出类型的枚举值。</param>
        /// <param name="nullable">参数是否可为空(DBNull.Value)。</param>
        /// <param name="precision">表示查询参数的小数精度。</param>
        /// <param name="scale">表述参数的 Scale 属性。</param>
        /// <param name="sourceColumn">参数的源列名称，该源列映射到 <see cref="System.Data.DataSet"/> 并用于加载或返回 <seealso cref="System.Data.Common.DbParameter.Value"/>。</param>
        /// <param name="sourceVersion">指示参数在加载 <seealso cref="System.Data.Common.DbParameter.Value"/> 时使用的 <see cref="System.Data.DataRowVersion"/>。</param>
        /// <param name="value">表示被加入的查询参数的参数值。</param>
        /// <returns>一个 <see cref="DbParameter"/> 脚本查询参数对象。</returns>
        protected DbParameter CreateParameter(string name, DB2Type dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DB2Parameter parameter = this.CreateParameter(name) as DB2Parameter;
            ConfigureParameter(parameter, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return parameter;
        }


        /// <summary>
        /// 设置 <see cref="DB2Parameter"/> 参数对象的值。
        /// </summary>
        /// <param name="parameter">一个<see cref="DB2Parameter"/> 参数对象</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <param name="direction">一个表示参数为输入还是输出类型的枚举值。</param>
        /// <param name="nullable">参数是否可为空(DBNull.Value)。</param>
        /// <param name="precision">表示查询参数的小数精度。</param>
        /// <param name="scale">表述参数的 Scale 属性。</param>
        /// <param name="sourceColumn">参数的源列名称，该源列映射到 <see cref="System.Data.DataSet"/> 并用于加载或返回 <seealso cref="System.Data.Common.DbParameter.Value"/>。</param>
        /// <param name="sourceVersion">指示参数在加载 <seealso cref="System.Data.Common.DbParameter.Value"/> 时使用的 <see cref="System.Data.DataRowVersion"/>。</param>
        /// <param name="value">表示被加入的查询参数的参数值。</param>
        protected virtual void ConfigureParameter(DB2Parameter parameter, string name, DB2Type dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            parameter.DB2Type = dbType;
            parameter.Size = size;
            parameter.Value = value ?? DBNull.Value;
            parameter.Direction = direction;
            parameter.IsNullable = nullable;
            parameter.Precision = precision;
            parameter.Scale = scale;
            parameter.SourceColumn = sourceColumn;
            parameter.SourceVersion = sourceVersion;
        }




        /// <summary>
        /// 执行 <paramref name="command"/> 命令并根据执行结果返回一个新创建的 <see cref="XmlReader"/> 对象。
        /// </summary>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <returns>表示 <paramref name="command"/> 执行的结果的一个 <see cref="XmlReader"/> 对象。</returns>
        public XmlReader ExecuteXmlReader(DbCommand command)
        {
            DB2Command db2Command = CheckIfDB2Command(command);
            using (var wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                return this.DoExecuteXmlReader(db2Command);
            }
        }

        /// <summary>
        /// 作为事务处理的一部分以指定的脚本类型和参数执行 <paramref name="command"/> 命令并根据执行结果返回一个新创建的 <see cref="XmlReader"/> 对象。
        /// </summary>
        /// <param name="command">要被执行的 <see cref="DbCommand"/> 命令。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <returns>表示脚本命令执行的结果的一个 <see cref="XmlReader"/> 对象。</returns>
        public XmlReader ExecuteXmlReader(DbCommand command, DbTransaction transaction)
        {
            DB2Command db2Command = CheckIfDB2Command(command);
            PrepareCommand(db2Command, transaction);
            return this.DoExecuteXmlReader(db2Command);
        }



        /// <summary>
        /// Determines if the number of parameters in the command matches the array of parameter values.
        /// </summary>
        /// <param name="command">The System.Data.Common.DbCommand containing the parameters.</param>
        /// <param name="values">The array of parameter values.</param>
        /// <returns>true if the number of parameters and values match; otherwise, false.</returns>
        protected override bool SameNumberOfParametersAndValues(DbCommand command, object[] values)
        {
            int returnParameterCount = 1;
            string conns = ConnectionString.ToUpper(CultureInfo.CurrentCulture);
            if (conns.Contains("SERVERTYPE=UNI") && (values.Length == 1) && (command.Parameters.Count == 1))
            {
                // if UniData or UniVerse databases, set to 0 for ExecuteScalar
                returnParameterCount = 0;
            }
            int numberOfParametersToStoredProcedure = command.Parameters.Count - returnParameterCount;
            int numberOfValuesProvidedForStoredProcedure = values.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }

        /// <summary>
        /// Returns the starting index for parameters in a command.
        /// </summary>
        /// <returns>The starting index for parameters in a command.</returns>
        protected override int UserParametersStartIndex()
        {
            string conns = ConnectionString.ToUpper(CultureInfo.CurrentCulture);
            Int16 userparamstartindex = 1;
            if (conns.Contains("SERVERTYPE=UNI"))
            {
                // if UniData or UniVerse databases, this needs to be 0
                userparamstartindex = 0;
            }
            return userparamstartindex;
        }



        /// <summary>
        /// 从在 <see cref="DbCommand"/> 中指定的存储过程中检索参数信息并填充指定的 <see cref="DbCommand"/> 对象的 Parameters 集合，其必须为一个 <see cref="DB2Command"/> 实例。
        /// </summary>
        /// <param name="discoveryCommand">
        /// 引用将从其中导出参数信息的存储过程的 <see cref="DbCommand"/>，其必须为一个 <see cref="DB2Command"/> 实例。
        /// 将派生参数添加到 <see cref="DbCommand"/> 的 Parameters 集合中。
        /// </param>
        protected override void DeriveParameters(System.Data.Common.DbCommand discoveryCommand)
        {
            DB2CommandBuilder.DeriveParameters((DB2Command)discoveryCommand);
        }

        private XmlReader DoExecuteXmlReader(DB2Command command)
        {
            XmlReader reader = command.ExecuteXmlReader();
            return reader;
        }



        /// <summary>
        /// 设置 <paramref name="adapter"/> 对象的 RowUpdated 事件回调函数。
        /// </summary>
        /// <param name="adapter">需要设置 RowUpdated 事件的 <see cref="System.Data.Common.DbDataAdapter"/> 对象实例。</param>
        protected override void SetUpRowUpdatedEvent(System.Data.Common.DbDataAdapter adapter)
        {
            ((DB2DataAdapter)adapter).RowUpdated += DB2Database_RowUpdated;
            base.SetUpRowUpdatedEvent(adapter);
        }

        private void DB2Database_RowUpdated(object sender, DB2RowUpdatedEventArgs e)
        {
            if (e.RecordsAffected == 0)
            {
                if (e.Errors != null)
                {
                    e.Row.RowError = "更新数据行时出错。";
                    e.Status = UpdateStatus.SkipCurrentRow;
                }
            }
        }




        /// <summary>
        /// 获取表示该数据库 SQL 脚本环境中查询语句中脚本参数的名称前缀字符。该属性返回一个 char 字符 @。
        /// </summary>
        protected char ParameterToken
        {
            get { return '@'; }
        }

        /// <summary>
        /// 判断 <paramref name="command"/> 是否为一个 <see cref="DB2Command"/> 类型的实例。
        /// </summary>
        /// <param name="command">要确认是否为 <see cref="DB2Command"/> 类型的 <see cref="DbCommand"/> 命令对象。</param>
        /// <returns>如果 <paramref name="command"/> 是一个 <see cref="DB2Command"/> 类型的实例，则返回 <paramref name="command"/> 转换后的 <see cref="DB2Command"/> 类型对象；否则返回 null(实际上不能返回 null，因为转型不成功则抛出异常)。</returns>
        public static DB2Command CheckIfDB2Command(DbCommand command)
        {
            DB2Command db2Command = command as DB2Command;
            if (db2Command == null)
            {
                throw new ArgumentException("传入的参数 command 不是一个 DB2Command 对象。");
            }
            return db2Command;
        }
    }
}
