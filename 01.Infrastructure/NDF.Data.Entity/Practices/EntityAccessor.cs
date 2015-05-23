using NDF.ComponentModel.Paging;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace NDF.Data.Entity.Practices
{
    /// <summary>
    /// 实体数据模型访问器，用于快速的操作数据库并访问实体数据模型对象。
    /// </summary>
    /// <remarks>
    /// 备注：
    /// 1、因为存在事务操作的可能性，通过该对象操作数据库后执行保存操作时，请直接调用该对象的 SaveChanges 或 SaveChangesAsync 方法，而不是调用
    ///     其 DbContext.Database.SaveChanges、DbContext.Database.SaveChangsAsync 方法。
    /// 2、该对象是 IDispose 接口的实现，在 Dispose 方法中将自动释放其内部 DbContext 属性的资源；在大多数情况下，请在 using (...) { ... } 语句块中
    ///     中声明和创建该类型的对象实例。
    /// </remarks>
    public class EntityAccessor : Disposable
    {
        private System.Data.Entity.DbContext _context;
        private DbTransaction _transaction;
        private MethodInfo _addOrUpdateMethod;
        private MethodInfo _setMethod;
        private Type _entityType;


        #region 构造方法定义

        /// <summary>
        /// 初始化一个 <see cref="EntityAccessor"/> 实体数据访问器对象。
        /// </summary>
        protected EntityAccessor()
        {
            this.Disposing += InternalDispose;

            this.AutoCommitOnSaveChanges = false;
            this.AutoDisposeContext = true;
        }

        /// <summary>
        /// 以指定的可操作实体类型初始化一个 <see cref="EntityAccessor"/> 实体数据访问器对象。
        /// </summary>
        /// <param name="entityType">可操作的实体类型。</param>
        public EntityAccessor(Type entityType) : this()
        {
            this.EntityType = entityType;
        }

        /// <summary>
        /// 以指定的 实体数据库上下文 对象初始化一个 <see cref="EntityAccessor"/> 实体数据访问器对象。
        /// </summary>
        /// <param name="context">指定的 实体数据库上下文 对象，该对象将会被设置为新创建的 <see cref="EntityAccessor"/> 实体数据访问器对象的 <seealso cref="DbContext"/> 属性中。</param>
        public EntityAccessor(System.Data.Entity.DbContext context) : this()
        {
            this.DbContext = context;
        }

        /// <summary>
        /// 以指定的 实体数据库上下文 对象和可操作的实体类型初始化一个 <see cref="EntityAccessor"/> 实体数据访问器对象。
        /// </summary>
        /// <param name="context">指定的 实体数据库上下文 对象，该对象将会被设置为新创建的 <see cref="EntityAccessor"/> 实体数据访问器对象的 <seealso cref="DbContext"/> 属性中。</param>
        /// <param name="entityType">可操作的实体类型。</param>
        public EntityAccessor(System.Data.Entity.DbContext context, Type entityType) : this(context)
        {
            this.EntityType = entityType;
        }

        #endregion


        #region 公共属性定义

        /// <summary>
        /// 获取或设置用于当前实体数据模型访问器的 EntityFramework 数据库上下文 操作对象。
        /// </summary>
        public System.Data.Entity.DbContext DbContext
        {
            get { return this._context; }
            protected internal set { this._context = value; }
        }

        /// <summary>
        /// 获取一个 <see cref="System.Data.Entity.Database"/> 对象，该对象可用于对底层数据库进行 增、删、该、查 操作。
        /// </summary>
        /// <remarks>该属性值相当于当前对象 <seealso cref="DbContext"/> 属性的 <seealso cref="Database"/> 值。</remarks>
        public Database Database
        {
            get { return this.DbContext.Database; }
        }

        /// <summary>
        /// 获取或设置当前 实体数据模型访问器 进行数据库持久化操作时所使用的数据库事务对象。
        /// 将该属性设置为 Null 可以取消数据库事务操作。
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// 如果设置的新事务是已完成的事务（已经进行了 Commit 操作）、
        ///     或者设置的新事物所关联的数据库连接与当前 实体数据模型访问器 的实体数据库上下文属性 <seealso cref="DbContext"/> 中的 <see cref="System.Data.Entity.Database"/> 所关联的数据库连接不匹配、
        ///     或者当前 实体数据模型访问器 已经设置了一个存在的 <see cref="Transaction"/> 属性作为事务、
        ///     或者当前 实体数据模型访问器 的实体数据库上下文属性 <seealso cref="DbContext"/> 中的 <see cref="System.Data.Entity.Database"/> 已经设置了一个事务，
        ///     则抛出该异常。
        /// </exception>
        public DbTransaction Transaction
        {
            get { return this._transaction; }
            set
            {
                this.DbContext.Database.UseTransaction(value);
                this._transaction = value;
            }
        }

        /// <summary>
        /// 获取或设置一个布尔值属性；该属性指示当设置了当前实体数据模型访问器的数据库事务属性 <see cref="Transaction"/> 的情况下，是否在执行保存 SaveChanges 操作时自动提交事务。
        /// <para>默认值为 false。</para>
        /// </summary>
        public bool AutoCommitOnSaveChanges { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值；该值指示当 EntityAccessor 实体数据模型访问器在执行 Dispose 操作时，是否也自动对其 DbContext 属性执行 Dispose 操作。
        /// 默认值为 true。
        /// 如果当前 <see cref="EntityAccessor"/> 对象由 <see cref="System.Data.Entity.DbContext"/> 的扩展<br />
        ///     方法 Accessor 或 Accessor&lt;TEnitity&gt; 创建，则该属性默认值为 false。
        /// </summary>
        public bool AutoDisposeContext { get; set; }

        /// <summary>
        /// 获取或设置当前实体数据模型访问器中可操作的实体数据类型。
        /// </summary>
        public virtual Type EntityType
        {
            get { return this._entityType; }
            set
            {
                CheckEntityType(value);
                this._entityType = value;
            }
        }

        #endregion


        #region 公共方法定义

        /// <summary>
        /// 获取当前实体数据模型访问器的 <see cref="EntityType"/> 所示实体类型所对应的数据库表的表名称。
        /// <para>如果当前对象的 <see cref="EntityType"/> 属性为 null，则该方法将会抛出 <see cref="InvalidOperationException"/> 异常。</para>
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            Type entityType = this.EntityType;
            if (entityType == null)
                throw new InvalidOperationException("该实体数据模型访问器未定义 EntityType 属性的值，无法获取其映射的数据库表名称。");

            return this.GetTableName(entityType);
        }

        /// <summary>
        /// 用于当前实体数据模型访问器获取数据库对象模型中指定的实体类型所映射的数据库表的表名称。
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public string GetTableName(Type entityType)
        {
            return this.DbContext.GetTableName(entityType);
        }

        #endregion



        #region 新增方法：将实体数据模型对象添加至数据库（单个新增、批量新增）。

        /// <summary>
        /// 将给定对象作为数据实体以“已添加”状态添加到基础上下文中。
        /// 如果当前访问器定义了 EntityType 属性，则会将该对象以属性复制方式转换成 EntityType 属性所示的实体数据然后执行添加操作。
        /// 当调用 SaveChanges 时，会将该实体插入到数据库中。
        /// </summary>
        /// <param name="entity">要添加的对象实体。</param>
        public virtual void Add(object entity)
        {
            if (entity == null)
                return;

            this.Add(this._entityType ?? entity.GetType(), entity);
        }

        /// <summary>
        /// 将给定对象作为数据实体以“已添加”状态添加到基础上下文中。
        /// 该方法在执行添加操作前会先将 <paramref name="entity"/> 以属性复制方式转换成 <paramref name="entityType"/> 所示的实体数据类型。
        /// 当调用 SaveChanges 时，会将该实体插入到数据库中。
        /// </summary>
        /// <param name="entityType">要将 <paramref name="entity"/> 对象转换成的目标实体数据类型。</param>
        /// <param name="entity">>要添加的对象实体。</param>
        public virtual void Add(Type entityType, object entity)
        {
            if (entity == null)
                return;

            Check.NotNull(entityType);
            this.DbContext.Set(entityType).Add(entity.CastTo(entityType));
        }


        /// <summary>
        /// 将一组给定的对象作为数据实体以“已添加”状态添加到基础上下文中。
        /// 如果当前访问器定义了 EntityType 属性，则会将该组对象中的每个元素以属性复制方式转换成 EntityType 属性所示的实体数据然后执行添加操作。
        /// 当调用 SaveChanges 时，会将该组实体插入到数据库中。
        /// </summary>
        /// <param name="entities">要添加的对象实体序列。</param>
        public virtual void AddRange(IEnumerable<object> entities)
        {
            if (entities.IsNullOrEmpty())
                return;

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            foreach (object entity in entities)
            {
                if (entity == null)
                    continue;

                this.Add(entity);
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        /// <summary>
        /// 将一组给定的对象作为数据实体以“已添加”状态添加到基础上下文中。
        /// 如果当前访问器定义了 EntityType 属性，则会将该组对象中的每个元素以属性复制方式转换成 EntityType 属性所示的实体数据然后执行添加操作。
        /// 当调用 SaveChanges 时，会将该组实体插入到数据库中。
        /// </summary>
        /// <param name="entities">要添加的对象实体序列数组。</param>
        public virtual void AddRange(params object[] entities)
        {
            this.AddRange(entities.AsEnumerable());
        }

        /// <summary>
        /// 将一组给定的对象作为数据实体以“已添加”状态添加到基础上下文中。
        /// 该方法在执行添加操作前会先将 <paramref name="entities"/> 中的每个元素以属性复制方式转换成 <paramref name="entityType"/> 所示的实体数据类型。
        /// 当调用 SaveChanges 时，会将该组实体插入到数据库中。
        /// </summary>
        /// <param name="entityType">要将 <paramref name="entities"/> 对象转换成的目标实体数据类型。</param>
        /// <param name="entities">要添加的对象实体序列数组。</param>
        public virtual void AddRange(Type entityType, IEnumerable<object> entities)
        {
            if (entities.IsNullOrEmpty())
                return;

            Check.NotNull(entityType);

            DbSet set = this.DbContext.Set(entityType);

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            foreach (object entity in entities)
            {
                if (entity == null)
                    continue;

                set.Add(entity.CastTo(entityType));
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        /// <summary>
        /// 将一组给定的对象作为数据实体以“已添加”状态添加到基础上下文中。
        /// 该方法在执行添加操作前会先将 <paramref name="entities"/> 中的每个元素以属性复制方式转换成 <paramref name="entityType"/> 所示的实体数据类型。
        /// 当调用 SaveChanges 时，会将该组实体插入到数据库中。
        /// </summary>
        /// <param name="entityType">要将 <paramref name="entities"/> 对象转换成的目标实体数据类型。</param>
        /// <param name="entities">要添加的对象实体序列数组。</param>
        public virtual void AddRange(Type entityType, params object[] entities)
        {
            this.AddRange(entityType, entities.AsEnumerable());
        }

        #endregion


        #region 附加数据方法：判断当前实体数据上下文中是否存在该实体对象，如果存在则修改，如果不存在则新增（单个附加、批量附加）。

        public virtual void AddOrUpdate(object entity)
        {
            if (entity == null)
                return;

            this.AddOrUpdate(this._entityType ?? entity.GetType(), entity);
        }

        public virtual void AddOrUpdate(Type entityType, object entity)
        {
            if (entity == null)
                return;

            this.AddOrUpdateRange(entityType, new object[] { entity });
        }


        public virtual void AddOrUpdateRange(IEnumerable<object> entities)
        {
            if (entities.IsNullOrEmpty())
                return;

            foreach (object entity in entities)
            {
                if (entity == null)
                    continue;

                this.AddOrUpdate(entity);
            }
        }

        public virtual void AddOrUpdateRange(params object[] entities)
        {
            this.AddOrUpdateRange(entities.AsEnumerable());
        }

        public virtual void AddOrUpdateRange(Type entityType, IEnumerable<object> entities)
        {
            if (entities.IsNullOrEmpty())
                return;
            Check.NotNull(entityType);

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            this.InnerAddOrUpdate(entities.ToArray(), entityType);

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public virtual void AddOrUpdateRange(Type entityType, params object[] entities)
        {
            this.AddOrUpdateRange(entityType, entities.AsEnumerable());
        }

        #endregion


        #region 修改方法：（单个修改、批量修改）。

        public virtual void Update(object entity)
        {
            if (entity == null)
                return;

            this.Update(this._entityType ?? entity.GetType(), entity);
        }

        public virtual void Update(object entity, IEnumerable<string> properties)
        {
            if (entity == null)
                return;

            this.Update(this._entityType ?? entity.GetType(), entity, properties);
        }

        public virtual void Update(object entity, params string[] properties)
        {
            this.Update(entity, properties.AsEnumerable());
        }


        public virtual void Update(Type entityType, object entity)
        {
            if (entity == null)
                return;
            Check.NotNull(entityType);
            entity = entity.CastTo(entityType);
            entity = entity.DuplicateWithNonNavigations(this.DbContext);

            DbEntityEntry entry = this.DbContext.FindEntry(entity, true);
            if (entry.State == EntityState.Detached)
            {
                this.DbContext.Set(entityType).Attach(entity);
            }
            entry.State = EntityState.Modified;
        }

        public virtual void Update(Type entityType, object entity, IEnumerable<string> properties)
        {
            if (entity == null || properties.IsNullOrEmpty())
                return;
            Check.NotNull(entityType);
            entity = entity.CastTo(entityType);
            entity = entity.DuplicateWithNonNavigations(this.DbContext);

            DbSet set = this.DbContext.Set(entityType);
            DbEntityEntry entry = this.DbContext.FindEntry(entity, false);

            if (entry.State == EntityState.Detached)
            {
                set.Attach(entity);
            }
            entry.State = EntityState.Unchanged;

            var values = entity.ToDictionary();
            foreach (string name in properties)
            {
                DbPropertyEntry property = entry.Property(name);
                property.IsModified = true;
                property.CurrentValue = values[name];
            }
        }

        public virtual void Update(Type entityType, object entity, params string[] properties)
        {
            this.Update(entityType, entity, properties.AsEnumerable());
        }


        public virtual void UpdateRange(IEnumerable<object> entities)
        {
            if (entities.IsNullOrEmpty())
                return;

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            foreach (object entity in entities)
            {
                if (entity == null)
                    continue;

                this.Update(entity);
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public virtual void UpdateRange(IEnumerable<object> entities, IEnumerable<string> properties)
        {
            if (entities.IsNullOrEmpty() || properties.IsNullOrEmpty())
                return;

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            foreach (object entity in entities)
            {
                if (entity == null)
                    continue;

                this.Update(entity, properties);
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public virtual void UpdateRange(IEnumerable<object> entities, params string[] properties)
        {
            this.UpdateRange(entities, properties.AsEnumerable());
        }


        public virtual void UpdateRange(params object[] entities)
        {
            this.UpdateRange(entities.AsEnumerable());
        }

        public virtual void UpdateRange(Type entityType, IEnumerable<object> entities)
        {
            if (entities.IsNullOrEmpty())
                return;
            Check.NotNull(entityType);

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            //DbSet set = this.DbContext.Set(entityType);
            //foreach (object entity in entities)
            //{
            //    var model = entity.CastTo(entityType);
            //    model = model.DuplicateWithNonNavigationProperties(this.DbContext);
            //    DbEntityEntry entry = this.DbContext.FindEntry(model);

            //    if (entry.State == EntityState.Detached)
            //    {
            //        set.Attach(model);
            //    }
            //    entry.State = EntityState.Modified;
            //}
            foreach (object entity in entities)
            {
                if (entity == null)
                    continue;

                this.Update(entityType, entity);
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public virtual void UpdateRange(Type entityType, IEnumerable<object> entities, IEnumerable<string> properties)
        {
            if (entities.IsNullOrEmpty() || properties.IsNullOrEmpty())
                return;
            Check.NotNull(entityType);

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            DbSet set = this.DbContext.Set(entityType);
            //foreach (object entity in entities)
            //{
            //    var model = entity.CastTo(entityType);
            //    model = model.DuplicateWithNonNavigationProperties(this.DbContext);
            //    DbEntityEntry entry = this.DbContext.FindEntry(model);

            //    if (entry.State == EntityState.Detached)
            //    {
            //        set.Attach(model);
            //    }
            //    entry.State = EntityState.Unchanged;

            //    foreach (string name in properties)
            //    {
            //        entry.Property(name).IsModified = true;
            //    }
            //}
            foreach (object entity in entities)
            {
                if (entity == null)
                    continue;

                this.Update(entityType, entity, properties);
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public virtual void UpdateRange(Type entityType, IEnumerable<object> entities, params string[] properties)
        {
            this.UpdateRange(entityType, entities, properties.AsEnumerable());
        }

        public virtual void UpdateRange(Type entityType, params object[] entities)
        {
            this.UpdateRange(entityType, entities.AsEnumerable());
        }

        #endregion


        #region 删除方法：（单个删除、批量删除）。

        public virtual void Delete(object entity)
        {
            if (entity == null)
                return;

            this.Delete(this._entityType ?? entity.GetType(), entity);
        }

        public virtual void Delete(Type entityType, object entity)
        {
            if (entity == null)
                return;
            Check.NotNull(entityType);

            entity = entity.CastTo(entityType);
            entity = entity.DuplicateWithNonNavigations(this.DbContext);
            DbSet set = this.DbContext.Set(entityType);
            DbEntityEntry entry = this.DbContext.FindEntry(entity, false);
            if (entry.State == EntityState.Detached)
            {
                set.Attach(entry);
            }
            set.Remove(entity);
        }


        public virtual void DeleteRange(IEnumerable<object> entities)
        {
            if (entities.IsNullOrEmpty())
                return;

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            foreach (object entity in entities)
            {
                if (entity == null)
                    continue;

                this.Delete(entity);
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public virtual void DeleteRange(params object[] entities)
        {
            this.DeleteRange(entities.AsEnumerable());
        }

        public virtual void DeleteRange(Type entityType, IEnumerable<object> entities)
        {
            if (entities.IsNullOrEmpty())
                return;
            Check.NotNull(entityType);

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            //DbSet set = this.DbContext.Set(entityType);
            //IEnumerable<object> array = entities.Select(entity => entities.CastTo(entityType));
            //set.RemoveRange(array);

            foreach (object entity in entities.Select(entity => entities.CastTo(entityType)))
            {
                if (entity == null)
                    continue;

                this.Delete(entity);
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public virtual void DeleteRange(Type entityType, params object[] entities)
        {
            this.DeleteRange(entityType, entities.AsEnumerable());
        }

        #endregion


        #region 清空表数据

        /// <summary>
        /// 以逐行删除方式清空指定的实体数据模型类型所对应的数据表中的所有数据。
        /// </summary>
        /// <param name="entityType">实体数据模型类型，对应当前数据库上下文中的一个表。</param>
        public virtual void DeleteAll(Type entityType)
        {
            this.DeleteRange(this.SelectAll(entityType));
        }

        /// <summary>
        /// 以 SQL "TRUNCATE TABLE [TableName]" 操作方式清空指定的实体数据模型类型所对应的数据表中的所有数据。
        /// </summary>
        /// <param name="entityType">实体数据模型类型，对应当前数据库上下文中的一个表。</param>
        /// <remarks>注意：该操作不受数据库访问事务影响（不管有无设定事务或者事务有无提交，该操作都立即生效且不可逆）。</remarks>
        public virtual void Truncate(Type entityType)
        {
            this.DbContext.TruncateTable(entityType);
        }

        /// <summary>
        /// 以 SQL "TRUNCATE TABLE [TableName]" 操作方式清空指定的数据表中的所有数据。
        /// </summary>
        /// <param name="tableName">数据库表名称。</param>
        /// <remarks>注意：该操作不受数据库访问事务影响（不管有无设定事务或者事务有无提交，该操作都立即生效且不可逆）。</remarks>
        public virtual void Truncate(string tableName)
        {
            this.DbContext.TruncateTable(tableName);
        }

        #endregion


        #region 查询方法：（查询单个实体、查询多个、分页查询）。

        public virtual object SelectSingle(Type entityType, IEnumerable<object> keyValues)
        {
            Check.NotNull(entityType);
            Check.NotEmpty(keyValues);
            keyValues = keyValues.Where(keyValue => keyValue != null);

            DbSet set = this.DbContext.Set(entityType);
            return set.Find(keyValues).Duplicate();
        }

        public virtual object SelectSingle(Type entityType, params object[] keyValues)
        {
            return this.SelectSingle(entityType, keyValues.AsEnumerable()).Duplicate();
        }


        public virtual object[] Select(Type entityType, IEnumerable<object> keyValues)
        {
            Check.NotNull(entityType);
            Check.NotEmpty(keyValues);
            keyValues = keyValues.Where(keyValue => keyValue != null);

            DbSet set = this.DbContext.Set(entityType);
            List<object> list = new List<object>();
            foreach (object keyValue in keyValues)
            {
                object entity = set.Find(keyValue);
                if (entity != null)
                    list.Add(entity);
            }
            return list.Select(item => item.Duplicate()).ToArray();
        }

        public virtual object[] Select(Type entityType, params object[] keyValues)
        {
            return this.Select(entityType, keyValues.AsEnumerable());
        }

        public virtual object[] Select(Type entityType, IEnumerable<IEnumerable<object>> keyValuesArray)
        {
            Check.NotNull(entityType);
            Check.NotEmpty(keyValuesArray);
            keyValuesArray = keyValuesArray.Where(keyValues => !keyValues.IsNullOrEmpty());

            DbSet set = this.DbContext.Set(entityType);
            List<object> list = new List<object>();
            foreach (IEnumerable<object> keyValues in keyValuesArray)
            {
                object entity = set.Find(keyValues);
                if (entity != null)
                    list.Add(entity);
            }
            return list.Select(item => item.Duplicate()).ToArray();
        }

        public virtual object[] Select(Type entityType, params IEnumerable<object>[] keyValuesArray)
        {
            return this.Select(entityType, keyValuesArray.AsEnumerable());
        }


        public virtual object[] SelectPage(Type entityType, int pageIndex, int pageSize)
        {
            Check.NotNull(entityType);
            return this.DbContext.Set(entityType).ToGeneric().SplitPage(pageIndex, pageSize).Select(item => item.Duplicate()).ToArray();
        }


        public virtual PagingData<object> SelectPaging(Type entityType, int pageIndex, int pageSize)
        {
            Check.NotNull(entityType);
            PagingData<object> data = this.DbContext.Set(entityType).ToGeneric().ToPagingData(pageIndex, pageSize);
            data.Data = data.Data.Select(item => item.Duplicate()).ToArray();
            return data;
        }


        public virtual object[] SelectAll(Type entityType)
        {
            Check.NotNull(entityType);
            return this.DbContext.Set(entityType).ToGeneric().Select(item => item.Duplicate()).ToArray();
        }


        #endregion




        #region Internal Methods

        private void InternalDispose(object sender, EventArgs e)
        {
            if (this.DbContext == null)
                return;

            if (this.AutoDisposeContext)
                this.DbContext.Dispose();
        }


        private void CheckEntityType(Type entityType)
        {
            Check.NotNull(entityType);
            if (this.DbContext != null && !this.DbContext.IsDefinedEntity(entityType))
                throw new InvalidOperationException("实体数据模型访问器设置 EntityType 属性失败，该访问器指定了 DbContext，但是该 DbContext 未定义类型 {0} 所示的实体数据集合。".Format(entityType));
        }

        private MethodInfo AddOrUpdateMethod
        {
            get
            {
                if (this._addOrUpdateMethod == null)
                {
                    this._addOrUpdateMethod = typeof(System.Data.Entity.Migrations.DbSetMigrationsExtensions).GetMethods().First(m =>
                        {
                            if (m.Name != "AddOrUpdate" || !m.IsGenericMethod)
                                return false;
                            ParameterInfo[] parameters = m.GetParameters();
                            return parameters.Length == 2
                                && parameters[0].ParameterType.IsGenericType && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IDbSet<>)
                                && parameters[1].ParameterType.IsArray && parameters[1].ParameterType.ContainsGenericParameters;
                        });
                }
                return this._addOrUpdateMethod;
            }
        }

        private MethodInfo SetMethod
        {
            get
            {
                if (this._setMethod == null)
                {
                    this._setMethod = this.DbContext.GetType().GetMethods().First(m =>
                        {
                            if (m.Name != "Set" || !m.IsGenericMethod)
                                return false;
                            ParameterInfo[] parameters = m.GetParameters();
                            return parameters.Length == 0;
                        });
                }
                return this._setMethod;
            }
        }

        private void InnerAddOrUpdate(IEnumerable<object> entities, Type entityType)
        {
            entities = entities.Select(entity => entity.CastTo(entityType)).ToArray();
            MethodInfo addOrUpdateMethod = this.AddOrUpdateMethod.MakeGenericMethod(entityType);
            object set = this.SetMethod.MakeGenericMethod(entityType).Invoke(this.DbContext, new object[] { });
            addOrUpdateMethod.Invoke(null, new object[] { set, entities });
        }

        #endregion


        #region 二次封装 SaveChanges、SaveChangesAsync，用于在当前 EntityModelAccessor 外部的保存操作。

        /// <summary>
        /// 将在此上下文中所做的所有更改保存到基础数据库。
        /// </summary>
        /// <returns>返回保存操作写入到持久化数据库中时所影响的行数。</returns>
        /// <remarks>
        /// 如果当前 实体数据模型访问器 对象启用了事务操作（<seealso cref="Transaction"/> 属性不为空时），则该操作
        ///     将会基于事务进行保存操作（<seealso cref="Transaction"/> 在保存时执行 Commit 操作，出现异常时执行 Rollback 操作）。
        /// </remarks>
        public int SaveChanges()
        {
            if (this.DbContext == null)
                throw new InvalidOperationException("当前 实体数据模型访问器 对象的 DbContext 属性未配置，不能执行 SaveChanges 操作。");

            int ret = this.DbContext.SaveChanges();
            this.AutoCommitTranIfNeed();
            return ret;
        }

        /// <summary>
        /// 将在此上下文中所做的所有更改保存到基础数据库。
        /// validateOnSaveEnabled 参数指示在调用 SaveChanges() 时，是否自动验证所跟踪的实体。
        /// </summary>
        /// <param name="validateOnSaveEnabled">指示在调用 SaveChanges() 时，是否自动验证所跟踪的实体。</param>
        /// <returns>返回保存操作写入到持久化数据库中时所影响的行数。</returns>
        /// <remarks>
        /// 如果当前 实体数据模型访问器 对象启用了事务操作（<seealso cref="Transaction"/> 属性不为空时），则该操作
        ///     将会基于事务进行保存操作（<seealso cref="Transaction"/> 在保存时执行 Commit 操作，出现异常时执行 Rollback 操作）。
        /// </remarks>
        public int SaveChanges(bool validateOnSaveEnabled)
        {
            if (this.DbContext == null)
                throw new InvalidOperationException("当前 实体数据模型访问器 对象的 DbContext 属性未配置，不能执行 SaveChanges 操作。");

            int ret = this.DbContext.SaveChanges(validateOnSaveEnabled);
            this.AutoCommitTranIfNeed();
            return ret;
        }


        /// <summary>
        /// 通过异步等待方式将在此上下文中所做的所有更改保存到基础数据库。
        /// </summary>
        /// <returns>
        /// 返回一个异步等待的操作对象 <see cref="Task&lt;T&gt;"/>，该对象中的异步操作返回值即为写入到持久化数据库中时所影响的行数。
        /// </returns>
        /// <remarks>
        /// 如果当前 实体数据模型访问器 对象启用了事务操作（<seealso cref="Transaction"/> 属性不为空时），则该操作
        ///     将会基于事务进行保存操作（<seealso cref="Transaction"/> 在保存时执行 Commit 操作，出现异常时执行 Rollback 操作）。
        /// </remarks>
        public Task<int> SaveChangesAsync()
        {
            if (this.DbContext == null)
                throw new InvalidOperationException("当前 实体数据模型访问器 对象的 DbContext 属性未配置，不能执行 SaveChangesAsync 操作。");

            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            this.DbContext.SaveChangesAsync().ContinueWith(
                t => UpdateTaskCompletionSource(tcs, t));
            return tcs.Task;
        }

        /// <summary>
        /// 通过异步等待方式将在此上下文中所做的所有更改保存到基础数据库。
        /// validateOnSaveEnabled 参数指示在调用 SaveChanges() 时，是否自动验证所跟踪的实体。
        /// </summary>
        /// <param name="validateOnSaveEnabled">指示在调用 SaveChanges() 时，是否自动验证所跟踪的实体。</param>
        /// <returns>
        /// 返回一个异步等待的操作对象 <see cref="Task&lt;T&gt;"/>，该对象中的异步操作返回值即为写入到持久化数据库中时所影响的行数。
        /// </returns>
        /// <remarks>
        /// 如果当前 实体数据模型访问器 对象启用了事务操作（<seealso cref="Transaction"/> 属性不为空时），则该操作
        ///     将会基于事务进行保存操作（<seealso cref="Transaction"/> 在保存时执行 Commit 操作，出现异常时执行 Rollback 操作）。
        /// </remarks>
        public Task<int> SaveChangesAsync(bool validateOnSaveEnabled)
        {
            if (this.DbContext == null)
                throw new InvalidOperationException("当前 实体数据模型访问器 对象的 DbContext 属性未配置，不能执行 SaveChangesAsync 操作。");

            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            this.DbContext.SaveChangesAsync(validateOnSaveEnabled).ContinueWith(
                t => UpdateTaskCompletionSource(tcs, t));

            return tcs.Task;
        }

        /// <summary>
        /// 通过异步等待方式将在此上下文中所做的所有更改保存到基础数据库。
        /// </summary>
        /// <param name="cancellationToken">该参数可以用于在执行中取消异步等待操作。</param>
        /// <returns>
        /// 返回一个异步等待的操作对象 <see cref="Task&lt;T&gt;"/>，该对象中的异步操作返回值即为写入到持久化数据库中时所影响的行数。
        /// </returns>
        /// <remarks>
        /// 如果当前 实体数据模型访问器 对象启用了事务操作（<seealso cref="Transaction"/> 属性不为空时），则该操作
        ///     将会基于事务进行保存操作（<seealso cref="Transaction"/> 在保存时执行 Commit 操作，出现异常时执行 Rollback 操作）。
        /// </remarks>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            if (this.DbContext == null)
                throw new InvalidOperationException("当前 实体数据模型访问器 对象的 DbContext 属性未配置，不能执行 SaveChangesAsync 操作。");
            
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            this.DbContext.SaveChangesAsync(cancellationToken).ContinueWith(
                t => UpdateTaskCompletionSource(tcs, t));

            return tcs.Task;
        }

        /// <summary>
        /// 通过异步等待方式将在此上下文中所做的所有更改保存到基础数据库。
        /// validateOnSaveEnabled 参数指示在调用 SaveChanges() 时，是否自动验证所跟踪的实体。
        /// </summary>
        /// <param name="cancellationToken">该参数可以用于在执行中取消异步等待操作。</param>
        /// <param name="validateOnSaveEnabled">指示在调用 SaveChanges() 时，是否自动验证所跟踪的实体。</param>
        /// <returns>
        /// 返回一个异步等待的操作对象 <see cref="Task&lt;T&gt;"/>，该对象中的异步操作返回值即为写入到持久化数据库中时所影响的行数。
        /// </returns>
        /// <remarks>
        /// 如果当前 实体数据模型访问器 对象启用了事务操作（<seealso cref="Transaction"/> 属性不为空时），则该操作
        ///     将会基于事务进行保存操作（<seealso cref="Transaction"/> 在保存时执行 Commit 操作，出现异常时执行 Rollback 操作）。
        /// </remarks>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken, bool validateOnSaveEnabled)
        {
            if (this.DbContext == null)
                throw new InvalidOperationException("当前 实体数据模型访问器 对象的 DbContext 属性未配置，不能执行 SaveChangesAsync 操作。");

            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            this.DbContext.SaveChangesAsync(cancellationToken, validateOnSaveEnabled).ContinueWith(
                t => UpdateTaskCompletionSource(tcs, t));

            return tcs.Task;
        }


        private void UpdateTaskCompletionSource<TResult>(TaskCompletionSource<TResult> tcs, Task<TResult> task)
        {
            if (task.IsFaulted)
            {
                tcs.TrySetException(task.Exception);
            }
            else if (task.IsCanceled)
            {
                tcs.TrySetCanceled();
            }
            else
            {
                tcs.TrySetResult(task.Result);
                this.AutoCommitTranIfNeed();
            }
        }

        #endregion


        #region 关于启用事务的封装操作。

        /// <summary>
        /// 创建并开启一个数据库操作事务。
        /// </summary>
        /// <returns>返回一个对数据库的事务 <see cref="DbTransaction"/> 对象。</returns>
        /// <remarks>
        /// 该操作会将新开启的数据库操作事务立即设置为当前对象中 <seealso cref="Transaction"/> 属性的值。
        /// 如果需要开启一个与当前 实体数据模型访问器 无关的数据库操作事务，请使用 DbContext.Database.BeginTransaction()。
        /// </remarks>
        public DbTransaction BeginTransaction()
        {
            if (this.Transaction != null)
                throw new InvalidOperationException("当前 实体数据上下文 中不可开启一个新的数据库事务，因为已存在一个未关闭的事务。");

            DbContextTransaction contextTransaction = this.DbContext.Database.BeginTransaction();
            this.Transaction = contextTransaction.UnderlyingTransaction;
            return contextTransaction.UnderlyingTransaction;
        }

        /// <summary>
        /// 创建并开启一个数据库操作事务，并指定事务的隔离级别锁定行为。
        /// </summary>
        /// <param name="isolationLevel">事务的隔离级别锁定行为</param>
        /// <returns>返回一个对数据库的事务 <see cref="DbTransaction"/> 对象。</returns>
        /// <remarks>
        /// 该操作会将新开启的数据库操作事务立即设置为当前对象中 <seealso cref="Transaction"/> 属性的值。
        /// 如果需要开启一个与当前 实体数据模型访问器 无关的数据库操作事务，请使用 DbContext.Database.BeginTransaction()。
        /// </remarks>
        public DbTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            if (this.Transaction != null)
                throw new InvalidOperationException("当前 实体数据上下文 中不可开启一个新的数据库事务，因为已存在一个未关闭的事务。");

            DbContextTransaction contextTransaction = this.DbContext.Database.BeginTransaction(isolationLevel);
            this.Transaction = contextTransaction.UnderlyingTransaction;
            return contextTransaction.UnderlyingTransaction;
        }


        /// <summary>
        /// 回滚当前 实体数据模型访问器 上下文的数据库访问事务。
        /// 并在事务回滚后清除当前 实体数据模型访问器 上下文的数据库访问事务（即设置 this.Transaction 属性的值为 Null）。
        /// </summary>
        /// <returns>如果回滚并清除事务成功，则返回 true，否则返回 false。</returns>
        public bool CancelTransaction()
        {
            if (this.Transaction == null)
                throw new InvalidOperationException("当前 实体数据上下文 尚未开启或设置一个数据库事务（this.Transaction == null），不能进行取消并清除事务操作。");

            try
            {
                this.Transaction.Rollback();
                this.Transaction = null;
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 提交当前 实体数据模型访问器 上下文的数据库访问事务。
        /// </summary>
        public void Commit()
        {
            this.CommitTranIfNeed();
        }

        /// <summary>
        /// 回滚当前 实体数据模型访问器 上下文的数据库访问事务。
        /// </summary>
        public void Rollback()
        {
            this.RollbackTranIfNeed();
        }



        private void CommitTranIfNeed()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Commit();
            }
        }

        private void RollbackTranIfNeed()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Rollback();
            }
        }

        private void AutoCommitTranIfNeed()
        {
            if (this.AutoCommitOnSaveChanges)
            {
                this.CommitTranIfNeed();
            }
        }

        #endregion


    }
}
