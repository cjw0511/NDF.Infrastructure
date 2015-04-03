using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves
{
    /// <summary>
    /// 用于比较两个数据库连接字符串是否表示等效的数据库连接。
    /// </summary>
    public class ConnectionStringEqualityComparer : EqualityComparer<string>
    {
        private static object _locker = new object();
        private static Dictionary<DbProviderFactory, ConnectionStringEqualityComparer> dictionary = new Dictionary<DbProviderFactory, ConnectionStringEqualityComparer>();


        /// <summary>
        /// 以指定的 <see cref="DbProviderFactory"/> 作为 <see cref="ProviderFactory"/> 属性值初始化类型 <see cref="ConnectionStringEqualityComparer"/> 的新实例。
        /// </summary>
        /// <param name="factory"></param>
        private ConnectionStringEqualityComparer(DbProviderFactory factory)
        {
            Check.NotNull(factory);
            this.ProviderFactory = factory;
        }


        /// <summary>
        /// 确认 <paramref name="x"/> 所表示的数据库连接字符串与 <paramref name="y"/> 所表示的数据库连接字符串是否表示同一个数据库连接。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool Equals(string x, string y)
        {
            x = Check.EmptyCheck(x);
            y = Check.EmptyCheck(y);

            DbConnectionStringBuilder a = this.ProviderFactory.CreateConnectionStringBuilder();
            a.ConnectionString = x;

            DbConnectionStringBuilder b = this.ProviderFactory.CreateConnectionStringBuilder();
            b.ConnectionString = y;

            return a.EquivalentTo(b);
        }

        /// <summary>
        /// 获取数据库连接字符串的 HashCode 值。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public override int GetHashCode(string connectionString)
        {
            connectionString = Check.EmptyCheck(connectionString);
            DbConnectionStringBuilder builder = this.ProviderFactory.CreateConnectionStringBuilder();
            builder.ConnectionString = connectionString;
            return builder.ToString().GetHashCode();
        }


        /// <summary>
        /// 获取当前 <see cref="ConnectionStringEqualityComparer"/> 数据库连接字符串比较器所使用的 <see cref="DbProviderFactory"/> 对象。
        /// </summary>
        public DbProviderFactory ProviderFactory
        {
            get;
            private set;
        }


        /// <summary>
        /// 根据指定的 <see cref="DbProviderFactory"/> 获取其对应的数据库连接字符串等效性比较程序 <see cref="ConnectionStringEqualityComparer"/> 对象。
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ConnectionStringEqualityComparer GetComparer(DbProviderFactory factory)
        {
            Check.NotNull(factory);
            lock (_locker)
            {
                ConnectionStringEqualityComparer comparer = null;
                if (!dictionary.TryGetValue(factory, out comparer))
                {
                    comparer = new ConnectionStringEqualityComparer(factory);
                    dictionary.Add(factory, comparer);
                }
                return comparer;
            }
        }


    }
}
