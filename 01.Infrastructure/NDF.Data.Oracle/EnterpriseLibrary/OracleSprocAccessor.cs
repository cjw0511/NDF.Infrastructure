using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Oracle.EnterpriseLibrary
{
    /// <summary>
    /// Represents a stored procedure call to the database that will return an enumerable of <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The element type that will be used to consume the result set.</typeparam>
    public class OracleSprocAccessor<TResult> : CommandAccessor<TResult>
    {
        readonly IParameterMapper parameterMapper;
        readonly string procedureName;

        /// <summary>
        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="rowMapper"/> to convert the returned rows to clr type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
        /// <param name="procedureName">The stored procedure that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        public OracleSprocAccessor(OracleDatabase database, string procedureName, IRowMapper<TResult> rowMapper)
            : this(database, procedureName, new DefaultParameterMapper(database), rowMapper)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="resultSetMapper"/> to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
        /// <param name="procedureName">The stored procedure that will be executed.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        public OracleSprocAccessor(OracleDatabase database, string procedureName, IResultSetMapper<TResult> resultSetMapper)
            : this(database, procedureName, new DefaultParameterMapper(database), resultSetMapper)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="rowMapper"/> to convert the returned rows to clr type <typeparamref name="TResult"/>.
        /// The <paramref name="parameterMapper"/> will be used to interpret the parameters passed to the Execute method.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
        /// <param name="procedureName">The stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        public OracleSprocAccessor(OracleDatabase database, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
            : base(database, rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException("The value can not be a null or empty string.");
            if (parameterMapper == null) throw new ArgumentNullException("parameterMapper");

            this.procedureName = procedureName;
            this.parameterMapper = parameterMapper;
        }

        /// <summary>
        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="resultSetMapper"/> to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.
        /// The <paramref name="parameterMapper"/> will be used to interpret the parameters passed to the Execute method.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
        /// <param name="procedureName">The stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        public OracleSprocAccessor(OracleDatabase database, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
            : base(database, resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException("The value can not be a null or empty string.");
            if (parameterMapper == null) throw new ArgumentNullException("parameterMapper");

            this.procedureName = procedureName;
            this.parameterMapper = parameterMapper;
        }

        /// <summary>
        /// Executes the stored procedure and returns an enumerable of <typeparamref name="TResult"/>.
        /// The enumerable returned by this method uses deferred loading to return the results.
        /// </summary>
        /// <param name="parameterValues">Values that will be interpret by an <see cref="IParameterMapper"/> and function as parameters to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public override IEnumerable<TResult> Execute(params object[] parameterValues)
        {
            DbCommand command = null;

            try
            {
                command = Database.GetStoredProcCommand(procedureName);

                parameterMapper.AssignParameters(command, parameterValues);

                return base.Execute(command).ToArray();
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }

        private class DefaultParameterMapper : IParameterMapper
        {
            readonly Database database;
            public DefaultParameterMapper(Database database)
            {
                this.database = database;
            }

            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                if (parameterValues.Length > 0)
                {
                    GuardParameterDiscoverySupported();
                    database.AssignParameters(command, parameterValues);
                }
            }

            private void GuardParameterDiscoverySupported()
            {
                if (!database.SupportsParemeterDiscovery)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.CurrentCulture,
                                      "The database type \"{0}\" does not support automatic parameter discovery. Use an IParameterMapper instead.",
                                      database.GetType().FullName));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public override IAsyncResult BeginExecute(AsyncCallback callback, object state, params object[] parameterValues)
        {
            throw new NotSupportedException();
        }
    }
}
