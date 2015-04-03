using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MySql.Data.MySqlClient;
using NDF.Data.MySql.EnterpriseLibrary.Configuration;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.MySql.EnterpriseLibrary
{
    /// <summary>
    /// 表示一个 MySQL 数据库的命令执行程序。
    /// </summary>
    [ConfigurationElementType(typeof(MySqlDatabaseData))]
    public class MySqlDatabase : Database
    {
        /// <summary>
        /// 以指定的 MySQL 数据库连接字符串初始化一个 <see cref="MySqlDatabase"/> 实例。
        /// </summary>
        /// <param name="connectionString">表示一个 MySQL 数据库连接字符串。</param>
        public MySqlDatabase(string connectionString)
            : base(connectionString, MySqlClientFactory.Instance)
        {
        }



        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="MySqlCommand"/> 实例。</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        //[CLSCompliantAttribute(false)]
        public void AddInParameter(DbCommand command, string name, MySqlDbType dbType)
        {
            this.AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="MySqlCommand"/> 实例。</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        /// <param name="value">表示被加入的查询参数的参数值。</param>
        //[CLSCompliantAttribute(false)]
        public void AddInParameter(DbCommand command, string name, MySqlDbType dbType, object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="MySqlCommand"/> 实例。</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        /// <param name="sourceColumn">参数的源列名称，该源列映射到 <see cref="System.Data.DataSet"/> 并用于加载或返回 <seealso cref="System.Data.Common.DbParameter.Value"/>。</param>
        /// <param name="sourceVersion">指示参数在加载 <seealso cref="System.Data.Common.DbParameter.Value"/> 时使用的 <see cref="System.Data.DataRowVersion"/>。</param>
        //[CLSCompliantAttribute(false)]
        public void AddInParameter(DbCommand command, string name, MySqlDbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            this.AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }

        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="MySqlCommand"/> 实例。</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        //[CLSCompliantAttribute(false)]
        public void AddOutParameter(DbCommand command, string name, MySqlDbType dbType, int size)
        {
            this.AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="MySqlCommand"/> 实例。</param>
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
        //[CLSCompliantAttribute(false)]
        public virtual void AddParameter(DbCommand command, string name, MySqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DbParameter parameter = this.CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// 往 <paramref name="command"/> 对象中加入一个查询参数；
        /// </summary>
        /// <param name="command">表示一个 <see cref="DbCommand"/> 对象，其必须为一个 <see cref="MySqlCommand"/> 实例。</param>
        /// <param name="name">表示被加入的查询参数的参数名。</param>
        /// <param name="dbType">表示被加入的查询参数的参数数据类型。</param>
        /// <param name="direction"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        /// <param name="value">表示被加入的查询参数的参数值。</param>
        //[CLSCompliantAttribute(false)]
        public void AddParameter(DbCommand command, string name, MySqlDbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
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
        //[CLSCompliantAttribute(false)]
        protected DbParameter CreateParameter(string name, MySqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            MySqlParameter parameter = this.CreateParameter(name) as MySqlParameter;
            this.ConfigureParameter(parameter, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return parameter;
        }

        /// <summary>
        /// 设置 <see cref="MySqlParameter"/> 参数对象的值。
        /// </summary>
        /// <param name="parameter">一个<see cref="MySqlParameter"/> 参数对象</param>
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
        //[CLSCompliantAttribute(false)]
        protected virtual void ConfigureParameter(MySqlParameter parameter, string name, MySqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            parameter.MySqlDbType = dbType;
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
        /// 根据指定的名称创建一个可用于作为 <see cref="MySqlParameter"/> 参数名称的字符串。
        /// </summary>
        /// <param name="name">指定的 <see cref="MySqlParameter"/> 参数名称。</param>
        /// <returns>一个可用于作为 <see cref="MySqlParameter"/> 参数名称的字符串。</returns>
        public override string BuildParameterName(string name)
        {
            Check.NotEmpty(name, "name");
            if (name[0] != ParameterToken)
            {
                return name.Insert(0, new string(ParameterToken, 1));
            }
            return name;
        }


        /// <summary>
        /// 从在 <see cref="DbCommand"/> 中指定的存储过程中检索参数信息并填充指定的 <see cref="DbCommand"/> 对象的 Parameters 集合，其必须为一个 <see cref="MySqlCommand"/> 实例。
        /// </summary>
        /// <param name="discoveryCommand">
        /// 引用将从其中导出参数信息的存储过程的 <see cref="DbCommand"/>，其必须为一个 <see cref="MySqlCommand"/> 实例。
        /// 将派生参数添加到 <see cref="DbCommand"/> 的 Parameters 集合中。
        /// </param>
        protected override void DeriveParameters(System.Data.Common.DbCommand discoveryCommand)
        {
            MySqlCommandBuilder.DeriveParameters((MySqlCommand)discoveryCommand);
        }



        /// <summary>
        /// 确认 <paramref name="command"/> 对象的 Parameters 属性中参数数量是否和传入的 <paramref name="values"/> 所表示的参数元素数量一样。
        /// </summary>
        /// <param name="command">表示需要验证参数数量的 <see cref="System.Data.Common.DbCommand"/> 对象。</param>
        /// <param name="values">表示一组用于作为数据库查询命令参数的对象数组。</param>
        /// <returns>如果 <paramref name="command"/> 对象的 Parameters 属性中参数数量和 <paramref name="values"/> 所表示的参数元素数量一样，则返回 true，否则返回 false。</returns>
        protected override bool SameNumberOfParametersAndValues(System.Data.Common.DbCommand command, object[] values)
        {
            int returnParameterCount = 0;
            int numberOfParametersToStoredProcedure = command.Parameters.Count - returnParameterCount;
            int numberOfValuesProvidedForStoredProcedure = values.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }


        /// <summary>
        /// 设置 <paramref name="adapter"/> 对象的 RowUpdated 事件回调函数。
        /// </summary>
        /// <param name="adapter">需要设置 RowUpdated 事件的 <see cref="System.Data.Common.DbDataAdapter"/> 对象实例。</param>
        protected override void SetUpRowUpdatedEvent(System.Data.Common.DbDataAdapter adapter)
        {
            base.SetUpRowUpdatedEvent(adapter);
            ((MySqlDataAdapter)adapter).RowUpdated += MySqlDatabase_RowUpdated;
        }

        private void MySqlDatabase_RowUpdated(object sender, MySqlRowUpdatedEventArgs e)
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
        /// 判断 <paramref name="command"/> 是否为一个 <see cref="MySqlCommand"/> 类型的实例。
        /// </summary>
        /// <param name="command">要确认是否为 <see cref="MySqlCommand"/> 类型的 <see cref="DbCommand"/> 命令对象。</param>
        /// <returns>如果 <paramref name="command"/> 是一个 <see cref="MySqlCommand"/> 类型的实例，则返回 <paramref name="command"/> 转换后的 <see cref="MySqlCommand"/> 类型对象；否则返回 null(实际上不能返回 null，因为转型不成功则抛出异常)。</returns>
        public static MySqlCommand CheckIfMySqlCommand(DbCommand command)
        {
            MySqlCommand mySqlCommand = command as MySqlCommand;
            if (mySqlCommand == null)
            {
                throw new ArgumentException("传入的参数 command 不是一个 MySqlCommand 对象。");
            }
            return mySqlCommand;
        }
    }
}
