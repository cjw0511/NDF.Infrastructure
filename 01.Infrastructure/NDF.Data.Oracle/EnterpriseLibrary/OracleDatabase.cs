using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using NDF.Data.Oracle.EnterpriseLibrary.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Oracle.EnterpriseLibrary
{
    /// <summary>
    /// Represents an Oracle database.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When retrieving a result set, it will build the package name. The package name should be set based
    /// on the stored procedure prefix and this should be set via configuration. For
    /// example, a package name should be set as prefix of "ENTLIB_" and package name of
    /// "pkgENTLIB_ARCHITECTURE". For your applications, this is required only if you are defining your stored procedures returning
    /// ref cursors.
    /// </para>
    /// <para>
    /// This is a direct copy of the Enterprise Library Oracle Data Provider from Microsoft only using the ODP.NET data client from
    /// Oracle instead of the Microsoft one plus a few additional features.
    /// </para>
    /// </remarks>
    [OraclePermission(SecurityAction.Demand)]
    [ConfigurationElementType(typeof(OracleDatabaseData))]
    public class OracleDatabase : Database
    {
        /// <summary>
        /// The OracleClientFactory instance.
        /// </summary>
        public static readonly OracleClientFactory Instance = OracleClientFactory.Instance;

        private const string RefCursorName = "cur_OUT";
        private readonly IEnumerable<IOraclePackage> packages;
        private static readonly IEnumerable<IOraclePackage> emptyPackages = new List<IOraclePackage>(0);
        private readonly IDictionary<string, ParameterTypeRegistry> registeredParameterTypes
            = new Dictionary<string, ParameterTypeRegistry>();

        static readonly ParameterCache parameterCache = new ParameterCache();

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabase"/> class with a connection string and a list of Oracle packages.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        public OracleDatabase(string connectionString)
            : this(connectionString, emptyPackages)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabase"/> class with a connection string and a list of Oracle packages.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <param name="packages">A list of <see cref="IOraclePackage"/> objects.</param>
#pragma warning disable 612, 618
        public OracleDatabase(string connectionString, IEnumerable<IOraclePackage> packages)
            : base(connectionString, Instance)
        {
            if (packages == null) throw new ArgumentNullException("packages");

            this.packages = packages;
        }
#pragma warning restore 612, 618

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>       
        public override void AddParameter(DbCommand command, string name, DbType dbType, int size,
            ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn,
            DataRowVersion sourceVersion, object value)
        {
            if (DbType.Guid.Equals(dbType))
            {
                object convertedValue = ConvertGuidToByteArray(value);

#pragma warning disable 612, 618
                AddParameter((OracleCommand)command, name, OracleDbType.Raw, 16, direction, nullable, precision,
                    scale, sourceColumn, sourceVersion, convertedValue);
#pragma warning restore 612, 618

                RegisterParameterType(command, name, dbType);
            }
            else if (DbType.Boolean.Equals(dbType))
            {
                object convertedValue = ConvertBoolToShort(value);

                AddParameter(command, name, OracleDbType.Int16, 4, direction, nullable, precision, scale, sourceColumn, sourceVersion, convertedValue);

                RegisterParameterType(command, name, dbType);
            }
            else
            {
                base.AddParameter(command, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            }
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="OracleParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The <see cref="OracleCommand"/> to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="oracleType"><para>One of the <see cref="OracleDbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>      
#pragma warning disable 612, 618
        public void AddParameter(DbCommand command, string name, OracleDbType oracleType, int size,
            ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn,
            DataRowVersion sourceVersion, object value)
        {
            if (command == null) throw new ArgumentNullException("command");

            OracleParameter param = CreateParameter(name, DbType.AnsiString, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value) as OracleParameter;
            param.OracleDbType = oracleType;
            command.Parameters.Add(param);
        }
#pragma warning restore 612, 618

        /// <summary>
        /// Creates an <see cref="OracleDataReader"/> based on the <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command wrapper to execute.</param>        
        /// <returns>An <see cref="OracleDataReader"/> object.</returns>        
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        public override IDataReader ExecuteReader(DbCommand command)
        {
            PrepareCWRefCursor(command);
            return base.ExecuteReader(command);
        }

        /// <summary>
        /// All data readers get wrapped in objects so that they properly manage connections.
        /// Some derived Database classes will need to create a different wrapper, so this
        /// method is provided so that they can do this.
        /// </summary>
        /// <param name="connection">Connection + refcount.</param>
        /// <param name="innerReader">The reader to wrap.</param>
        /// <returns>The new reader.</returns>
        protected override IDataReader CreateWrappedReader(DatabaseConnectionWrapper connection, IDataReader innerReader)
        {
            return new RefCountingOracleDataReaderWrapper(connection, (OracleDataReader)innerReader);
        }

        /// <summary>
        /// <para>Creates an <see cref="OracleDataReader"/> based on the <paramref name="command"/>.</para>
        /// </summary>        
        /// <param name="command"><para>The command wrapper to execute.</para></param>        
        /// <param name="transaction"><para>The transaction to participate in when executing this reader.</para></param>        
        /// <returns><para>An <see cref="OracleDataReader"/> object.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="transaction"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        public override IDataReader ExecuteReader(DbCommand command, DbTransaction transaction)
        {
            PrepareCWRefCursor(command);
            return new OracleDataReaderWrapper((OracleDataReader)base.ExecuteReader(command, transaction));
        }

        /// <summary>
        /// <para>Executes a command and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="command"><para>The command to execute to fill the <see cref="DataSet"/></para></param>
        /// <returns><para>A <see cref="DataSet"/> filed with records and, if necessary, schema.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        public override DataSet ExecuteDataSet(DbCommand command)
        {
            PrepareCWRefCursor(command);
            return base.ExecuteDataSet(command);
        }

        /// <summary>
        /// <para>Executes a command and returns the result in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="command"><para>The command to execute to fill the <see cref="DataSet"/></para></param>
        /// <param name="transaction"><para>The transaction to participate in when executing this reader.</para></param>        
        /// <returns><para>A <see cref="DataSet"/> filed with records and, if necessary, schema.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> can not be <see langword="null"/> (<b>Nothing</b> in Visual Basic).</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> can not be <see langword="null"/> (<b>Nothing</b> in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="transaction"/> can not be <see langword="null"/> (<b>Nothing</b> in Visual Basic).</para>
        /// </exception>
        public override DataSet ExecuteDataSet(DbCommand command, DbTransaction transaction)
        {
            PrepareCWRefCursor(command);
            return base.ExecuteDataSet(command, transaction);
        }

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> from a <see cref="DbCommand"/>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        public override void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
        {
            PrepareCWRefCursor(command);
            base.LoadDataSet(command, dataSet, tableNames);
        }

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> from a <see cref="DbCommand"/> in a transaction.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command in.</para>
        /// </param>
        public override void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames, DbTransaction transaction)
        {
            PrepareCWRefCursor(command);
            base.LoadDataSet(command, dataSet, tableNames, transaction);
        }

        /// <summary>
        /// Gets a parameter value.
        /// </summary>
        /// <param name="command">The command that contains the parameter.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The value of the parameter.</returns>
        public override object GetParameterValue(DbCommand command, string parameterName)
        {
            if (command == null) throw new ArgumentNullException("command");

            object convertedValue = base.GetParameterValue(command, parameterName);

            ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
            if (registry != null)
            {
                if (registry.HasRegisteredParameterType(parameterName))
                {
                    DbType dbType = registry.GetRegisteredParameterType(parameterName);

                    if (DbType.Guid == dbType)
                    {
                        convertedValue = ConvertByteArrayToGuid(convertedValue);
                    }
                    else if (DbType.Boolean == dbType)
                    {
                        convertedValue = ConvertShortToBool(convertedValue);
                    }
                }
            }

            return convertedValue;
        }

        /// <summary>
        /// Sets a parameter value.
        /// </summary>
        /// <param name="command">The command with the parameter.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        public override void SetParameterValue(DbCommand command, string parameterName, object value)
        {
            if (command == null) throw new ArgumentNullException("command");

            object convertedValue = value;

            ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
            if (registry != null)
            {
                if (registry.HasRegisteredParameterType(parameterName))
                {
                    DbType dbType = registry.GetRegisteredParameterType(parameterName);

                    if (DbType.Guid == dbType)
                    {
                        convertedValue = ConvertGuidToByteArray(value);
                    }
                }
            }

            base.SetParameterValue(command, parameterName, convertedValue);
        }

        /// <devdoc>
        /// This is a private method that will build the Oracle package name if your stored procedure
        /// has proper prefix and postfix. 
        /// This functionality is include for
        /// the portability of the architecture between SQL and Oracle datbase.
        /// This method also adds the reference cursor to the command writer if not already added.
        /// </devdoc>        
        private void PrepareCWRefCursor(DbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            if (CommandType.StoredProcedure == command.CommandType)
            {
                // Check for ref. cursor in the command writer, if it does not exist, add a know reference cursor out
                // of "cur_OUT"
                if (!CommandHasCursorParameter(command))
                {
#pragma warning disable 612, 618
                    AddParameter(command as OracleCommand, RefCursorName, OracleDbType.RefCursor, 0, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull);
#pragma warning restore 612, 618
                }
            }
        }

        private ParameterTypeRegistry GetParameterTypeRegistry(string commandText)
        {
            ParameterTypeRegistry registry;
            registeredParameterTypes.TryGetValue(commandText, out registry);
            return registry;
        }


        private void RegisterParameterType(DbCommand command, string parameterName, DbType dbType)
        {
            ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
            if (registry == null)
            {
                registry = new ParameterTypeRegistry(command.CommandText);
                registeredParameterTypes.Add(command.CommandText, registry);
            }

            registry.RegisterParameterType(parameterName, dbType);
        }

        /// <summary>
        /// Converts a Boolean object to a short or DBNull if null.
        /// </summary>
        /// <remarks>This is specifically used in the conversion of a Boolean to OracleDbType.Int16.
        /// Contrary to most interpretations of a boolean value a value of 0 (zero) stored in a Database
        /// is generally regarded as representing false.</remarks>
        /// <param name="value">The boolean value.</param>
        /// <returns>short integer of 0 for false and 1 for true</returns>
        private static object ConvertBoolToShort(object value)
        {
            return ((value is DBNull) || (value == null)) ? Convert.DBNull : ((bool)value ? (short)1 : (short)0);
        }

        /// <summary>
        /// Converts a short object to Boolean.
        /// </summary>
        /// <remarks>This is specifically used in the conversion of OracleDbType.Int16 back to Boolean</remarks>
        /// <param name="value">The short value.</param>
        /// <returns>Boolean object or DBNull</returns>
        private static object ConvertShortToBool(object value)
        {
            if (value == (Object)null)
                return DBNull.Value;

            short shortValue;
            // OracleDbType.Int16 is internally stored as an OracleDecimal instance
            if (value is OracleDecimal)
                shortValue = ((OracleDecimal)value).ToInt16();
            else
                shortValue = (short)value;

            return (shortValue != 0);
        }

        /// <summary>
        /// Converts a GUID object to a byte array or DBNull if null.
        /// </summary>
        /// <remarks>This is specifically used in the conversion of a Guid to OracleDbType.Raw</remarks>
        /// <param name="value">The Guid value.</param>
        /// <returns>byte array or DBNull</returns>
        private static object ConvertGuidToByteArray(object value)
        {
            return ((value is DBNull) || (value == null)) ? Convert.DBNull : ((Guid)value).ToByteArray();
        }

        /// <summary>
        /// Converts a byte array object to GUID.
        /// </summary>
        /// <remarks>This is specifically used in the conversion of OracleDbType.Raw back to Guid</remarks>
        /// <param name="value">The value.</param>
        /// <returns>Guid object or DBNull</returns>
        private static object ConvertByteArrayToGuid(object value)
        {
            byte[] buffer;
            // OracleDbType.Raw is internally stored as an OracleBinary instance
            if (value is OracleBinary)
                buffer = (byte[])((OracleBinary)value);
            else
                buffer = (byte[])value;

            if (buffer.Length == 0)
            {
                return DBNull.Value;
            }
            else
            {
                return new Guid(buffer);
            }
        }

        private static bool CommandHasCursorParameter(DbCommand command)
        {
            foreach (OracleParameter parameter in command.Parameters)
            {
                if (parameter.OracleDbType == OracleDbType.RefCursor)
                {
                    return true;
                }
            }
            return false;
        }

        /// <devdoc>
        /// Listens for the RowUpdate event on a data adapter to support UpdateBehavior.Continue
        /// </devdoc>
        private void OnOracleRowUpdated(object sender, OracleRowUpdatedEventArgs args)
        {
            if (args.RecordsAffected == 0)
            {
                if (args.Errors != null)
                {
                    args.Row.RowError = "Failed to update row";
                    args.Status = UpdateStatus.SkipCurrentRow;
                }
            }
        }

        /// <summary>
        /// Does this <see cref='Database'/> object support parameter discovery?
        /// </summary>
        /// <value>true.</value>
        public override bool SupportsParemeterDiscovery
        {
            get { return true; }
        }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object. 
        /// </summary>
        /// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
        /// <remarks>
        /// The <see cref="DbCommand"/> must be an instance of a <see cref="OracleCommand"/> object.
        /// </remarks>
        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
#pragma warning disable 612, 618
            OracleCommandBuilder.DeriveParameters((OracleCommand)discoveryCommand);
#pragma warning restore 612, 618
        }

        /// <summary>
        /// <para>Creates a <see cref="DbCommand"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <param name="parameterValues"><para>The list of parameters for the procedure.</para></param>
        /// <returns><para>The <see cref="DbCommand"/> for the stored procedure.</para></returns>
        /// <remarks>
        /// <para>The parameters for the stored procedure will be discovered and the values are assigned in positional order.</para>
        /// </remarks>        
        public override DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues)
        {
            // need to do this before of eventual parameter discovery
            string updatedStoredProcedureName = TranslatePackageSchema(storedProcedureName);
            DbCommand command = base.GetStoredProcCommand(updatedStoredProcedureName, parameterValues);
            return command;
        }

        /// <summary>
        /// <para>Discovers parameters on the <paramref name="command"/> and assigns the values from <paramref name="parameterValues"/> to the <paramref name="command"/>s Parameters list.</para>
        /// </summary>
        /// <param name="command">The command the parameeter values will be assigned to</param>
        /// <param name="parameterValues">The parameter values that will be assigned to the command.</param>
        public override void AssignParameters(DbCommand command, object[] parameterValues)
        {
            if (command == null) throw new ArgumentNullException("command");

            // need to do this before of eventual parameter discovery
            string updatedStoredProcedureName = TranslatePackageSchema(command.CommandText);

            base.AssignParameters(command, parameterValues);
        }

        /// <summary>
        /// <para>Creates a <see cref="DbCommand"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>		
        /// <returns><para>The <see cref="DbCommand"/> for the stored procedure.</para></returns>
        /// <remarks>
        /// <para>The parameters for the stored procedure will be discovered and the values are assigned in positional order.</para>
        /// </remarks>        
        public override DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            // need to do this before of eventual parameter discovery
            string updatedStoredProcedureName = TranslatePackageSchema(storedProcedureName);
            DbCommand command = base.GetStoredProcCommand(updatedStoredProcedureName);
            return command;
        }

        /// <devdoc>
        /// Looks into configuration and gets the information on how the command wrapper should be updated if calling a package on this
        /// connection.
        /// </devdoc>        
        private string TranslatePackageSchema(string storedProcedureName)
        {
            const string allPrefix = "*";
            string packageName = String.Empty;
            string updatedStoredProcedureName = storedProcedureName;

            if (packages != null && !string.IsNullOrEmpty(storedProcedureName))
            {
                foreach (IOraclePackage oraPackage in packages)
                {
                    if ((oraPackage.Prefix == allPrefix) || (storedProcedureName.StartsWith(oraPackage.Prefix, StringComparison.Ordinal)))
                    {
                        //use the package name for the matching prefix
                        packageName = oraPackage.Name;
                        //prefix = oraPackage.Prefix;
                        break;
                    }
                }
            }
            if (0 != packageName.Length)
            {
                updatedStoredProcedureName = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", packageName, storedProcedureName);
            }

            return updatedStoredProcedureName;
        }

        /// <summary>
        /// Sets the RowUpdated event for the data adapter.
        /// </summary>
        /// <param name="adapter">The <see cref="DbDataAdapter"/> to set the event.</param>
        /// <remarks>The <see cref="DbDataAdapter"/> must be an <see cref="OracleDataAdapter"/>.</remarks>
        protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
        {
#pragma warning disable 612, 618
            ((OracleDataAdapter)adapter).RowUpdated += OnOracleRowUpdated;
#pragma warning restore 612, 618
        }

        /// <summary>
        /// Determines if the number of parameters in the command matches the array of parameter values.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> containing the parameters.</param>
        /// <param name="values">The array of parameter values.</param>
        /// <returns><see langword="true"/> if the number of parameters and values match; otherwise, <see langword="false"/>.</returns>
        protected override bool SameNumberOfParametersAndValues(DbCommand command, object[] values)
        {
            int numberOfParametersToStoredProcedure = command.Parameters.Count;

            if (CommandHasCursorParameter(command))
            {
                numberOfParametersToStoredProcedure--;
            }

            int numberOfValuesProvidedForStoredProcedure = values.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The targetElement type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteOracleSprocAccessor<TResult>(string procedureName, params object[] parameterValues)
            where TResult : new()
        {
            return CreateOracleSprocAccessor<TResult>(procedureName).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The targetElement type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteOracleSprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateOracleSprocAccessor<TResult>(procedureName, parameterMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The targetElement type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteOracleSprocAccessor<TResult>(string procedureName, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateOracleSprocAccessor(procedureName, rowMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The targetElement type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteOracleSprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateOracleSprocAccessor(procedureName, parameterMapper, rowMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The targetElement type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteOracleSprocAccessor<TResult>(string procedureName, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateOracleSprocAccessor(procedureName, resultSetMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The targetElement type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteOracleSprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateOracleSprocAccessor(procedureName, parameterMapper, resultSetMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateOracleSprocAccessor<TResult>(string procedureName)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateOracleSprocAccessor(procedureName, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateOracleSprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateOracleSprocAccessor(procedureName, parameterMapper, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateOracleSprocAccessor<TResult>(string procedureName, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException();

            return new OracleSprocAccessor<TResult>(this, procedureName, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateOracleSprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException();

            return new OracleSprocAccessor<TResult>(this, procedureName, parameterMapper, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateOracleSprocAccessor<TResult>(string procedureName, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException();

            return new OracleSprocAccessor<TResult>(this, procedureName, resultSetMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateOracleSprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException();

            return new OracleSprocAccessor<TResult>(this, procedureName, parameterMapper, resultSetMapper);
        }
    }

    internal sealed class ParameterTypeRegistry
    {
        private string commandText;
        private IDictionary<string, DbType> parameterTypes;

        internal ParameterTypeRegistry(string commandText)
        {
            this.commandText = commandText;
            this.parameterTypes = new Dictionary<string, DbType>();
        }

        internal void RegisterParameterType(string parameterName, DbType parameterType)
        {
            this.parameterTypes[parameterName] = parameterType;
        }

        internal bool HasRegisteredParameterType(string parameterName)
        {
            return this.parameterTypes.ContainsKey(parameterName);
        }

        internal DbType GetRegisteredParameterType(string parameterName)
        {
            return this.parameterTypes[parameterName];
        }
    }
}
