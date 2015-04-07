using NDF.Data.EntityFramework.MasterSlaves.Interception;
using NDF.Data.Utilities;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NDF.Data.EntityFramework.MasterSlaves
{
    /// <summary>
    /// 定义一个 EF 实体数据库主从读写分离服务命令拦截器，用于实现数据库读写分离操作。
    /// </summary>
    public class DbMasterSlaveCommandInterceptor : DbCommandInterceptor
    {
        private Type _contextType;
        private DbMasterSlaveConfigContext _config;
        private bool _taskIsRunning = false;

        private readonly object _locker = new object();


        /// <summary>
        /// 以指定的 <see cref="Type"/> 类型参数初始化类型 <see cref="DbMasterSlaveCommandInterceptor"/> 的新实例。
        /// </summary>
        /// <param name="contextType"></param>
        internal DbMasterSlaveCommandInterceptor(Type contextType)
        {
            this._contextType = contextType;
            this.ResetMasterSlaveContextConfig();
            this.RegisterAppConfigChangedEvent();
        }



        /// <summary>
        /// 获取当前 <see cref="DbMasterSlaveCommandInterceptor"/> 指定读写分离命令拦截操作时所使用的配置对象。
        /// </summary>
        public DbMasterSlaveConfigContext Config
        {
            get { return this._config; }
        }

        /// <summary>
        /// 获取该 <see cref="DbMasterSlaveCommandInterceptor"/> 对象所作用于的目标 EF 实体数据库上下文类型。
        /// </summary>
        public Type TargetContextType
        {
            get { return this.Config.TargetContextType; }
        }


        /// <summary>
        /// 读取 EF 数据库读写分离服务可用的 Master 数据库节点连接字符串。
        /// </summary>
        public string MasterConnectionString
        {
            get { return this.Config.UsableMasterConnectionString; }
        }

        /// <summary>
        /// 读取 EF 数据库读写分离服务可用的 Slave 数据库节点连接字符串。
        /// <para>注意，如果当前数使用 EntityFramework 执行的数据库操作位于一个事务中，该属性将取 <see cref="MasterConnectionString"/> 的值。</para>
        /// <para>也就是说，在事务中进行的 EF 读写操作将直接在 Master 上操作而不进行读写分离。</para>
        /// </summary>
        public string SlaveConnectionString
        {
            get { return this.Config.UsableSlaveConnectionString; }
        }



        /// <summary>
        /// 在执行 数据查询操作前 调用该方法，以按需调整 <see cref="DbCommand"/> 命令所使用的 <see cref="DbConnection"/> 的连接字符串为 <see cref="SlaveConnectionString"/> 值。
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            this.UpdateToSlaveIfNeed(command, interceptionContext);
        }

        /// <summary>
        /// 在执行 获取单个字段数据的查询操作前 调用该方法，以按需调整 <see cref="DbCommand"/> 命令所使用的 <see cref="DbConnection"/> 的连接字符串为 <see cref="SlaveConnectionString"/> 值。
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            this.UpdateToSlaveIfNeed(command, interceptionContext);
        }

        /// <summary>
        /// 在执行 数据更改（Insert、Update、Delete） 操作前调用该方法，以按需调整 <see cref="DbCommand"/> 命令所使用的 <see cref="DbConnection"/> 的连接字符串为 <see cref="MasterConnectionString"/> 值。
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            this.UpdateToMasterIfNeed(command, interceptionContext);
        }



        /// <summary>
        /// 在需要的情况下，将命令拦截到的 EF 实体上下文中使用到的数据库连接切换至 Master 数据库连接。
        /// <para>同时按需启动数据库连接状态轮询检测服务。</para>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        protected virtual void UpdateToMasterIfNeed(DbCommand command, DbInterceptionContext interceptionContext)
        {
            var contexts = from context in interceptionContext.DbContexts
                           where this.ValidateContextCanApplyTo(context)
                           select context;

            string connectionString = this.MasterConnectionString;
            foreach (var context in contexts)
            {
                this.UpdateConnectionStringIfNeed(command, context.Database.Connection, connectionString);
                this.StartServersStateScanIfNeed(context);
            }
        }

        /// <summary>
        /// 在需要的情况下，将命令拦截到的 EF 实体上下文中使用到的数据库连接切换至 Slave 数据库连接。
        /// <para>同时按需启动数据库连接状态轮询检测服务。</para>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        protected virtual void UpdateToSlaveIfNeed(DbCommand command, DbInterceptionContext interceptionContext)
        {
            var contexts = from context in interceptionContext.DbContexts
                           where this.ValidateContextCanApplyTo(context)
                           select context;

            foreach (var context in contexts)
            {
                string connectionString = this.GetSlaveConnectionString(context);
                this.UpdateConnectionStringIfNeed(command, context.Database.Connection, connectionString);
                this.StartServersStateScanIfNeed(context);
            }
        }


        private string GetSlaveConnectionString(System.Data.Entity.DbContext context)
        {
            Transaction tran = Transaction.Current;
            return (tran == null || tran.TransactionInformation.Status == TransactionStatus.Committed) && context.Database.CurrentTransaction == null
                ? this.Config.UsableSlaveConnectionString
                : this.MasterConnectionString;
        }

        /// <summary>
        /// 确定当前的 EF 读写分离服务命令拦截器是否可应用于指定的 <see cref="System.Data.Entity.DbContext"/> 对象。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool ValidateContextCanApplyTo(System.Data.Entity.DbContext context)
        {
            return this.Config.CanApplyTo(context);
        }


        /// <summary>
        /// 确定数据库连接对象 <see cref="DbConnection"/> 所使用的连接字符串是否等效于指定的文本值。
        /// 如果不等效，则用传入的文本值 <paramref name="connectionString"/> 更新该数据库连接对象所使用的连接字符串。
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <param name="connectionString"></param>
        protected void UpdateConnectionStringIfNeed(DbCommand command, DbConnection connection, string connectionString)
        {
            if (!ConnectionStringEquals(connection, connectionString))
            {
                MasterSlaveInterception.Dispatcher.ConnectionStringUpdating(command, this.TargetContextType);
                UpdateConnectionString(connection, connectionString);
                MasterSlaveInterception.Dispatcher.ConnectionStringUpdated(command, this.TargetContextType);
            }
        }

        /// <summary>
        /// 更新数据库连接对象所使用的连接字符串。
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="connectionString"></param>
        internal static void UpdateConnectionString(DbConnection conn, string connectionString)
        {
            ConnectionState state = conn.State;
            if (state == ConnectionState.Open)
                conn.Close();

            conn.ConnectionString = connectionString;

            if (state == ConnectionState.Open)
                conn.Open();
        }


        /// <summary>
        /// 比较指定的数据库连接对象 <paramref name="connection"/> 所使用的连接字符串信息是否等效于一个字符串  <paramref name="connectionString"/> 所表示的连接。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        internal static bool ConnectionStringEquals(DbConnection connection, string connectionString)
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory(connection);
            DbConnectionStringEqualityComparer comparer = GetConnectionStringEqualityComparer(factory);
            return comparer.Equals(connection.ConnectionString, connectionString);
        }

        /// <summary>
        /// 获取一个用于比较数据库连接字符串等效性的 <see cref="DbConnectionStringEqualityComparer"/> 对象。
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        internal static DbConnectionStringEqualityComparer GetConnectionStringEqualityComparer(DbProviderFactory factory)
        {
            return DbConnectionStringEqualityComparer.GetConnectionStringEqualityComparer(factory);
        }



        /// <summary>
        /// 开启当前命令拦截器所用配置对象中的所有数据库连接状态可用性检测轮询服务。
        /// </summary>
        /// <param name="context"></param>
        protected void StartServersStateScanIfNeed(System.Data.Entity.DbContext context)
        {
            lock (this._locker)
            {
                if (this._taskIsRunning)
                    return;

                Type contextType = context.GetType();
                DbProviderFactory factory = DbProviderFactories.GetFactory(context.Database.Connection);
                this.Config.StartDbServersStateScanTaskIfNeed(factory);

                this._taskIsRunning = true;
            }
        }


        private void ResetMasterSlaveContextConfig()
        {
            DbMasterSlaveConfigContext newConfig = new DbMasterSlaveConfigContext(this._contextType);
            DbMasterSlaveConfigContext oldConfig = this._config;

            this._config = newConfig;
            this._taskIsRunning = false;

            if (oldConfig != null)
            {
                oldConfig.Dispose();
            }
        }


        private void RegisterAppConfigChangedEvent()
        {
            AppConfig.DefaultInstance.ConfigChanged += AppConfig_FileChanged;
        }

        private void AppConfig_FileChanged(object sender, EventArgs e)
        {
            this.ResetMasterSlaveContextConfig();
        }


    }
}
