using NDF.Data.EntityFramework.Annotations;
using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework
{
    /// <summary>
    /// DbContext 实例表示工作单元和存储库模式的组合，可用来查询数据库并将更改组合在一起，这些更改稍后将
    /// 作为一个单元写回存储区中。 DbContext 在概念上与 ObjectContext 类似。
    /// </summary>
    /// <remarks>
    /// DbContext 通常将与包含模型的根实体的 DbSet(TEntity) 属性的派生类型一起使用。在创建派生类的实例时，自动初始化这些
    ///     集。 可以通过将 SuppressDbSetInitializationAttribute 特性应用于整个派生上下文类或该类上的单个属性来修改此行
    ///     为。 可通过多种方法指定支持上下文的实体数据模型。 在使用 Code First 方法时，派生上下文上的 DbSet(TEntity) 属性
    ///     将用于按约定生成模型。可重写受保护的 OnModelCreating 方法来修改此模型。 通过从 DbModelBuilder 显式创建 DbCompiledModel 并
    ///     将此模型传递给 DbContext 构造函数之一，可以获取对用于 Model First 方法的模型的更精细的控制。 在使用 Database First 或
    ///     Model First 方法时，可使用实体设计器（或通过手动创建 EDMX 文件）来创建实体数据模型，然后可使用实体连接字符串
    ///     或 EntityConnection 对象来指定此模型。 可通过多种方法指定与数据库的连接（包括数据库的名称）。 如果从派生上下
    ///     文中调用无参数的 DbContext 构造函数，则可使用派生上下文的名称在 app.config 或 web.config 文件中查找连接字符
    ///     串。 如果未找到任何连接字符串，则将名称传递给在 Database 类上注册的 DefaultConnectionFactory。 然后，连接工厂
    ///     会在默认连接字符串中将上下文名称用作数据库名称。 （此默认连接字符串指向本地计算机上的 . \SQLEXPRESS，除非注册
    ///     一个不同的 DefaultConnectionFactory。）作为使用派生上下文名称的替代，还可通过将连接/数据库名称传递给采用一个
    ///     字符串的 DbContext 构造函数之一来显式指定该名称。 还可以“name=myname”的形式传递名称，在此情况下，必须在 config 文件中
    ///     找到该名称，否则将引发异常。 请注意在 app.config 或 web.config 文件中找到的连接可以是一个常规数据库连接字符串（而非特殊
    ///     的实体框架连接字符串），在此情况下，DbContext 将使用 Code First。 但是，如果在配置文件中找到的连接是一个特殊的实体框架
    ///     连接字符串，则 DbContext 将使用 Database First 或 Model First，并且将使用连接字符串中指定的模型。 还可使用现有的或显式
    ///     创建的 DbConnection 来代替数据库/连接名称。 可将 DbModelBuilderVersionAttribute 应用于派生自 DbContext 的类以设置创建
    ///     模型时上下文所使用的约定版本。 如果未应用任何特性，则将使用约定的最新版本。
    /// </remarks>
    public class DbContext : System.Data.Entity.DbContext
    {
        private Database _database;
        private ObjectContext _objectContext;
        private MetadataWorkspace _metadataWorkspace;
        private EntitySet[] _entitySets;
        private EntityTable[] _tables;


        #region 构造函数

        /// <summary>
        /// 使用约定构造一个新的上下文实例以创建将连接到的数据库的名称。 按照约定，该名称是派生上下文类的
        /// 全名（命名空间与类名称的组合）。 请参见有关这如何用于创建连接的类备注。
        /// </summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected DbContext() : base() { }

        /// <summary>
        /// 使用约定构造一个新的上下文实例以创建将连接到的数据库的名称，并从给定模型初始化该名称。 按照约定，该名称是派生
        /// 上下文类的全名（命名空间与类名称的组合）。 请参见有关这如何用于创建连接的类备注。
        /// </summary>
        /// <param name="model">支持此上下文的模型。</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected DbContext(DbCompiledModel model) : base(model) { }

        /// <summary>
        /// 可以将给定字符串用作将连接到的数据库的名称或连接字符串来构造一个新的上下文实例。 请参见有关这如何用于创建连接的类备注。
        /// </summary>
        /// <param name="nameOrConnectionString">数据库名称或连接字符串。</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        /// <summary>
        /// 通过现有连接来连接到数据库以构造一个新的上下文实例。 如果 contextOwnsConnection 为 false，则在释放上下文时将不释放该连接。
        /// </summary>
        /// <param name="existingConnection">要用于新的上下文的现有连接。</param>
        /// <param name="contextOwnsConnection">如果设置为 true，则释放上下文时将释放该连接；否则调用方必须释放该连接。</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) { }

        /// <summary>
        /// 围绕现有 ObjectContext 构造一个新的上下文实例。
        /// </summary>
        /// <param name="objectContext">要使用新的上下文包装的现有 ObjectContext。</param>
        /// <param name="dbContextOwnsObjectContext">如果设置为 true，则释放 DbContext 时将释放 ObjectContext；否则调用方必须释放该连接。</param>
        public DbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext) { }

        /// <summary>
        /// 可以将给定字符串用作将连接到的数据库的名称或连接字符串来构造一个新的上下文实例，并从给定模型初始化该实例。 请参见有关这如何用于创建连接的类备注。
        /// </summary>
        /// <param name="nameOrConnectionString">数据库名称或连接字符串。</param>
        /// <param name="model">支持此上下文的模型。</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model) { }

        /// <summary>
        /// 通过使用现有连接来连接到数据库以构造一个新的上下文实例，并从给定模型初始化该实例。 如果 contextOwnsConnection 为 false，则在释放上下文时将不释放该连接。
        /// </summary>
        /// <param name="existingConnection">要用于新的上下文的现有连接。</param>
        /// <param name="model">支持此上下文的模型。</param>
        /// <param name="contextOwnsConnection">如果设置为 true，则释放上下文时将释放该连接；否则调用方必须释放该连接。</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection) { }

        #endregion


        #region 公共属性

        /// <summary>
        /// 获取基于当前数据库上下文环境的数据库访问基础组件对象 <see cref="NDF.Data.Common.Database"/>。该对象可用于实现数据库的快速访问操作。
        /// </summary>
        public Database GeneralDatabase
        {
            get
            {
                if (this._database == null)
                {
                    this._database = this.Database.GetGeneralDatabase();
                }
                return this._database;
            }
        }

        /// <summary>
        /// 获取基于当前数据库上下文环境的数据库访问基础组件对象 <see cref="NDF.Data.Common.Database"/>。该对象可用于实现数据库的快速访问操作。
        /// </summary>
        public ObjectContext ObjectContext
        {
            get
            {
                if (this._objectContext == null)
                    this._objectContext = this.GetObjectContext();

                return this._objectContext;
            }
        }

        /// <summary>
        /// 获取当前 实体数据上下文 的 元数据工作区 对象。
        /// </summary>
        public MetadataWorkspace MetadataWorkspace
        {
            get
            {
                if (this._metadataWorkspace == null)
                    this._metadataWorkspace = this.GetMetadataWorkspace();

                return this._metadataWorkspace;
            }
        }

        /// <summary>
        /// 获取当前 实体数据上下文 对象中定义的所有实体集合信息。
        /// </summary>
        public EntitySet[] EntitySets
        {
            get
            {
                if (this._entitySets == null)
                    this._entitySets = this.GetEntitySets();

                return this._entitySets;
            }
        }

        /// <summary>
        /// 获取当前 实体数据模型 上下文中定义的所有 实体数据模型 类型所映射的数据表信息。
        /// </summary>
        public EntityTable[] Tables
        {
            get
            {
                if (this._tables == null)
                    this._tables = this.GetEntityTables();

                return this._tables;
            }
        }


        #endregion


    }
}
