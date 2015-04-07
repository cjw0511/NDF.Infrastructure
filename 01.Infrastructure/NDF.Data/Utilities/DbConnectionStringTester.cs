using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NDF.Data.Utilities
{
    /// <summary>
    /// 用于测试数据库服务器的连接状态。
    /// </summary>
    public class DbConnectionStringTester
    {
        private static readonly Dictionary<DbProviderFactory, DbConnectionStringTester> _dictionary = new Dictionary<DbProviderFactory, DbConnectionStringTester>();
        private static object _locker = new object();


        /// <summary>
        /// 以指定的 <see cref="DbProviderFactory"/> 作为 <see cref="ProviderFactory"/> 属性值
        /// 初始化类型 <see cref="DbConnectionStringTester"/> 的新实例。
        /// </summary>
        /// <param name="factory"></param>
        private DbConnectionStringTester(DbProviderFactory factory)
        {
            Check.NotNull(factory);
            this.ProviderFactory = factory;
        }

        /// <summary>
        /// 获取当前 <see cref="DbConnectionStringTester"/> 对象所使用的 <see cref="DbProviderFactory"/> 对象。
        /// </summary>
        public DbProviderFactory ProviderFactory
        {
            get;
            private set;
        }



        /// <summary>
        /// 使用同步方式检测一个数据库连接字符串所表示的数据库服务器的可连接状态。
        /// <para>注意：在连接请求异常的情况下，该方法可能需要很长时间（具体取决于传入的参数 <paramref name="connectionString"/> 中设定的 timeout 值）才能获得返回结果。</para>
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool Test(string connectionString)
        {
            bool ret = false;
            using (DbConnection conn = this.CreateConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    ret = true;
                }
                catch
                {
                    ret = false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return ret;
        }

        /// <summary>
        /// 使用异步方式检测一个数据库连接字符串所表示的数据库服务器的可连接状态。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public Task<bool> TestAsync(string connectionString)
        {
            return Task.Run(
                () => this.Test(connectionString));
        }

        /// <summary>
        /// 使用异步方式检测一个数据库连接字符串所表示的数据库服务器的可连接状态。
        /// <paramref name="cancellationToken"/> 指定该异步任务的取消标记。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> TestAsync(string connectionString, CancellationToken cancellationToken)
        {
            return Task.Run(
                () => this.Test(connectionString),
                cancellationToken);
        }



        /// <summary>
        /// 使用当前对象的 ADO.NET 提供程序与指定的数据库连接字符串创建一个数据库连接对象。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected DbConnection CreateConnection(string connectionString)
        {
            connectionString = Check.EmptyCheck(connectionString);
            DbConnection connection = this.ProviderFactory.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }
        

        /// <summary>
        /// 根据指定的 <see cref="DbProviderFactory"/> 获取其对应的数据库连接字符串有效性测试程序 <see cref="DbConnectionStringTester"/> 对象。
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static DbConnectionStringTester GetConnectionStringTester(DbProviderFactory factory)
        {
            Check.NotNull(factory);
            lock (_locker)
            {
                DbConnectionStringTester tester = null;
                if (!_dictionary.TryGetValue(factory, out tester))
                {
                    tester = new DbConnectionStringTester(factory);
                    _dictionary.Add(factory, tester);
                }
                return tester;
            }
        }

        /// <summary>
        /// 根据指定的 ADO.NET 提供程序固定名称 获取其对应的数据库连接字符串有效性测试程序 <see cref="DbConnectionStringTester"/> 对象。
        /// </summary>
        /// <param name="providerInvariantName"></param>
        /// <returns></returns>
        public static DbConnectionStringTester GetConnectionStringTester(string providerInvariantName)
        {
            providerInvariantName = Check.EmptyCheck(providerInvariantName);
            DbProviderFactory factory = DbProviderFactories.GetFactory(providerInvariantName);
            return GetConnectionStringTester(factory);
        }

    }
}
