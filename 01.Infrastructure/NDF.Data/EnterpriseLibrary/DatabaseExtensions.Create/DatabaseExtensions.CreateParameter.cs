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
        /// 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database)
        {
            return database.DbProviderFactory.CreateParameter();
        }

        /// <summary>
        /// 以指定的名称创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示当前 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database, string parameterName)
        {
            DbParameter parameter = CreateParameter(database);
            parameter.ParameterName = database.BuildParameterName(parameterName);
            return parameter;
        }


        /// <summary>
        /// 以指定的名称、数据类型为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database, string parameterName, DbType dbType)
        {
            DbParameter parameter = CreateParameter(database, parameterName);
            parameter.DbType = dbType;
            return parameter;
        }

        /// <summary>
        /// 以指定的名称、长度为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="size">参数长度。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database, string parameterName, int size)
        {
            DbParameter parameter = CreateParameter(database, parameterName);
            parameter.Size = size;
            return parameter;
        }

        /// <summary>
        /// 以指定的名称、数据类型、长度为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database, string parameterName, DbType dbType, int size)
        {
            DbParameter parameter = CreateParameter(database, parameterName, dbType);
            parameter.Size = size;
            return parameter;
        }

        /// <summary>
        /// 以指定的名称、数据类型、长度、输入输出类型、可空类型为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <param name="direction">一个表示参数为输入还是输出类型的枚举值。</param>
        /// <param name="isNullable">参数是否可为空(DBNull.Value)。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database, string parameterName, DbType dbType, int size, ParameterDirection direction, bool isNullable)
        {
            DbParameter parameter = CreateParameter(database, parameterName, dbType, size);
            parameter.Direction = direction;
            parameter.IsNullable = isNullable;
            return parameter;
        }

        /// <summary>
        /// 以指定的名称、数据类型、长度、输入输出类型、可空类型等配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。 
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <param name="direction">一个表示参数为输入还是输出类型的枚举值。</param>
        /// <param name="isNullable">参数是否可为空(DBNull.Value)。</param>
        /// <param name="sourceColumn">参数的源列名称，该源列映射到 <see cref="System.Data.DataSet"/> 并用于加载或返回 <seealso cref="System.Data.Common.DbParameter.Value"/>。</param>
        /// <param name="sourceColumnNullMapping">指示源列是否可以为 null。 这使得 <see cref="System.Data.Common.DbCommandBuilder"/> 能够正确地为可以为 null 的列生成 Update 语句。</param>
        /// <param name="sourceVersion">指示参数在加载 <seealso cref="System.Data.Common.DbParameter.Value"/> 时使用的 <see cref="System.Data.DataRowVersion"/>。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database, string parameterName, DbType dbType, int size, ParameterDirection direction, bool isNullable, string sourceColumn, bool sourceColumnNullMapping, DataRowVersion sourceVersion)
        {
            DbParameter parameter = CreateParameter(database, parameterName, dbType, size, direction, isNullable);
            parameter.SourceColumn = sourceColumn;
            parameter.SourceColumnNullMapping = sourceColumnNullMapping;
            parameter.SourceVersion = sourceVersion;
            return parameter;
        }


        /// <summary>
        /// 以指定的名称、值为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database, string parameterName, object value)
        {
            DbParameter parameter = CreateParameter(database, parameterName);
            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }

        /// <summary>
        /// 以指定的名称、值、数据类型为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database, string parameterName, object value, DbType dbType)
        {
            DbParameter parameter = CreateParameter(database, parameterName, value);
            parameter.DbType = dbType;
            return parameter;
        }

        /// <summary>
        /// 以指定的名称、值、长度为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="size">参数长度。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database, string parameterName, object value, int size)
        {
            DbParameter parameter = CreateParameter(database, parameterName, value);
            parameter.Size = size;
            return parameter;
        }

        /// <summary>
        /// 以指定的名称、值、数据类型、长度为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database, string parameterName, object value, DbType dbType, int size)
        {
            DbParameter parameter = CreateParameter(database, parameterName, value, dbType);
            parameter.Size = size;
            return parameter;
        }

        /// <summary>
        /// 以指定的名称、值、数据类型、长度、输入输出类型、可空类型为配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="dbType">参数数据类型。</param>
        /// <param name="size">参数长度。</param>
        /// <param name="direction">一个表示参数为输入还是输出类型的枚举值。</param>
        /// <param name="isNullable">参数是否可为空(DBNull.Value)。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public static DbParameter CreateParameter(this Database database, string parameterName, object value, DbType dbType, int size, ParameterDirection direction, bool isNullable)
        {
            DbParameter parameter = CreateParameter(database, parameterName, value, dbType, size);
            parameter.Direction = direction;
            parameter.IsNullable = isNullable;
            return parameter;
        }

        /// <summary>
        /// 以指定的名称、值、数据类型、长度、输入输出类型、可空类型等配置创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
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
        public static DbParameter CreateParameter(this Database database, string parameterName, object value, DbType dbType, int size, ParameterDirection direction, bool isNullable, string sourceColumn, bool sourceColumnNullMapping, DataRowVersion sourceVersion)
        {
            DbParameter parameter = CreateParameter(database, parameterName, value, dbType, size, direction, isNullable);
            parameter.SourceColumn = sourceColumn;
            parameter.SourceColumnNullMapping = sourceColumnNullMapping;
            parameter.SourceVersion = sourceVersion;
            return parameter;
        }

    }
}
