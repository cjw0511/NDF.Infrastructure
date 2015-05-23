using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.Migrations
{
    /// <summary>
    /// IDatabaseInitializer 的实现。
    /// 它判断当数据库不存在时重新创建数据库并选择重新设置数据库的种子。
    /// 或者当模型自数据库创建后发生更改（如增加了新表或表中的列发生了改动/删除/增加新列）时执行数据库合并（创建或更改/删除列）操作并选择重新设置数据库的种子。
    /// 若要设置数据库的种子，请创建一个派生类并重写 Seed 方法。
    /// </summary>
    /// <typeparam name="TContext">表示一个 <see cref="System.Data.Entity.DbContext"/> 类型。</typeparam>
    /// <typeparam name="TMigrationsConfiguration">表示一个 <see cref="System.Data.Entity.Migrations.DbMigrationsConfiguration&lt;TContext&gt;"/></typeparam>
    public class MigrationDatabaseIfModelChanges<TContext, TMigrationsConfiguration> : MigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration>
        where TContext : System.Data.Entity.DbContext
        where TMigrationsConfiguration : System.Data.Entity.Migrations.DbMigrationsConfiguration<TContext>, new()
    {
        /// <summary>
        /// 初始化 <see cref="MigrationDatabaseIfModelChanges&lt;TContext, TMigrationsConfiguration&gt;"/> 类型的一个新实例。
        /// </summary>
        public MigrationDatabaseIfModelChanges() : base()
        {
        }

        /// <summary>
        /// 初始化 <see cref="MigrationDatabaseIfModelChanges&lt;TContext, TMigrationsConfiguration&gt;"/> 类型的一个新实例。
        /// </summary>
        /// <param name="useSuppliedContext">
        /// If set to true the initializer is run using the connection information from
        ///     the context that triggered initialization. Otherwise, the connection information
        ///     will be taken from a context constructed using the default constructor or
        ///     registered factory if applicable.
        /// </param>
        public MigrationDatabaseIfModelChanges(bool useSuppliedContext)
            : base(useSuppliedContext)
        {
        }

        /// <summary>
        /// 初始化 <see cref="MigrationDatabaseIfModelChanges&lt;TContext, TMigrationsConfiguration&gt;"/> 类型的一个新实例。
        /// </summary>
        /// <param name="connectionStringName">
        /// The name of the connection string to use for migration.
        /// </param>
        public MigrationDatabaseIfModelChanges(string connectionStringName)
            : base(connectionStringName)
        {
        }
    }
}
