using NDF.ComponentModel.Paging;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.Practices
{
    /// <summary>
    /// 实体数据模型访问器的泛型类型定义，用于快速操作数据库并访问指定类型的实体数据模型对象。
    /// </summary>
    /// <typeparam name="TEntity">指定的 实体数据模型 类型。</typeparam>
    public class EntityAccessor<TEntity> : EntityAccessor where TEntity : class, new()
    {
        private Type _entityType = typeof(TEntity);
        private MethodInfo _orderByMethod;
        private string _tableName;


        #region 构造方法定义

        /// <summary>
        /// 初始化一个 <see cref="EntityAccessor&lt;TEntity&gt;"/> 实体数据访问器对象。
        /// </summary>
        protected EntityAccessor() : base() { }

        /// <summary>
        /// 以指定的 实体数据库上下文 对象初始化一个 <see cref="EntityAccessor&lt;TEntity&gt;"/> 实体数据访问器对象。
        /// </summary>
        /// <param name="context">指定的 实体数据库上下文 对象，该对象将会被设置为新创建的 <see cref="EntityAccessor"/> 实体数据访问器对象的 <seealso cref="DbContext"/> 属性中。</param>
        public EntityAccessor(System.Data.Entity.DbContext context)
            : base(context)
        {
        }

        #endregion


        #region 公共属性定义

        /// <summary>
        /// 获取当前实体数据模型访问器中可操作的实体数据类型。
        /// <para>注意，该属性调用 set 属性器将会抛出异常 <see cref="InvalidOperationException"/>。</para>
        /// </summary>
        public override Type EntityType
        {
            get { return this._entityType; }
            set { throw new InvalidOperationException("不能更改泛型类型 实体数据模型访问器 的可操作类型 EntityType 属性。"); }
        }

        /// <summary>
        /// 获取当前实体数据模型访问器的 <see cref="EntityType"/> 所示实体类型所对应的数据库表的表名称。
        /// </summary>
        public string TableName
        {
            get
            {
                if (this._tableName == null)
                    this._tableName = this.GetTableName(this.EntityType);

                return this._tableName;
            }
        }

        #endregion



        #region 泛型操作：新增数据（单个新增、批量新增）

        public virtual void Add(TEntity entity)
        {
            if (entity == null)
                return;

            this.DbContext.Set<TEntity>().Add(entity);
        }

        public override void Add(object entity)
        {
            this.Add(entity.CastTo<TEntity>());
        }

        public override void AddRange(IEnumerable<object> entities)
        {
            if (entities.IsNullOrEmpty())
                return;

            DbSet<TEntity> set = this.DbContext.Set<TEntity>();

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            foreach (object entity in entities)
            {
                if (entity == null)
                    continue;

                set.Add(entity.CastTo<TEntity>());
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            if (entities.IsNullOrEmpty())
                return;

            DbSet<TEntity> set = this.DbContext.Set<TEntity>();

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            foreach (TEntity entity in entities)
            {
                if (entity == null)
                    continue;

                set.Add(entity);
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public override void AddRange(params object[] entities)
        {
            this.AddRange(entities.AsEnumerable());
        }

        public virtual void AddRange(params TEntity[] entities)
        {
            this.AddRange(entities.AsEnumerable());
        }

        #endregion


        #region 泛型操作：附加数据（判断当前实体数据上下文中是否存在该实体对象，如果存在则修改，如果不存在则新增，单个附加、批量附加）

        public virtual void AddOrUpdate(TEntity entity)
        {
            this.AddOrUpdateRange(new TEntity[]{ entity });
        }

        public override void AddOrUpdate(object entity)
        {
            this.AddOrUpdateRange(new object[]{ entity });
        }


        public override void AddOrUpdateRange(IEnumerable<object> entities)
        {
            this.DbContext.Set<TEntity>().AddOrUpdateRange(entities);
        }

        public virtual void AddOrUpdateRange(IEnumerable<TEntity> entities)
        {
            this.DbContext.Set<TEntity>().AddOrUpdateRange(entities);
        }

        public override void AddOrUpdateRange(params object[] entities)
        {
            this.AddOrUpdateRange(entities.AsEnumerable());
        }

        public virtual void AddOrUpdateRange(params TEntity[] entities)
        {
            this.AddOrUpdateRange(entities.AsEnumerable());
        }

        #endregion


        #region 泛型操作：修改数据（单个修改、批量修改）

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                return;
            entity = entity.DuplicateWithNonNavigations(this.DbContext);

            DbEntityEntry<TEntity> entry = this.DbContext.FindEntry(entity, true);
            if (entry.State == EntityState.Detached)
            {
                this.DbContext.Set<TEntity>().Attach(entity);
            }
            entry.State = EntityState.Modified;
        }

        public virtual void Update(TEntity entity, IEnumerable<string> properties)
        {
            if (entity == null || properties.IsNullOrEmpty())
                return;
            entity = entity.DuplicateWithNonNavigations(this.DbContext);

            DbSet<TEntity> set = this.DbContext.Set<TEntity>();

            DbEntityEntry<TEntity> entry = this.DbContext.FindEntry(entity, false);
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

        public virtual void Update(TEntity entity, params string[] properties)
        {
            this.Update(entity, properties.AsEnumerable());
        }


        public override void Update(object entity)
        {
            this.Update(entity.CastTo<TEntity>());
        }

        public override void Update(object entity, IEnumerable<string> properties)
        {
            this.Update(entity.CastTo<TEntity>(), properties);
        }

        public override void Update(object entity, params string[] properties)
        {
            this.Update(entity, properties.AsEnumerable());
        }



        public override void UpdateRange(IEnumerable<object> entities)
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

        public override void UpdateRange(IEnumerable<object> entities, IEnumerable<string> properties)
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

        public override void UpdateRange(IEnumerable<object> entities, params string[] properties)
        {
            this.UpdateRange(entities, properties.AsEnumerable());
        }


        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            if (entities.IsNullOrEmpty())
                return;

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            foreach (TEntity entity in entities)
            {
                if (entity == null)
                    continue;

                this.Update(entity);
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities, IEnumerable<string> properties)
        {
            if (entities.IsNullOrEmpty() || properties.IsNullOrEmpty())
                return;

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            foreach (TEntity entity in entities)
            {
                if (entity == null)
                    continue;

                this.Update(entity, properties);
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities, params string[] properties)
        {
            this.UpdateRange(entities, properties.AsEnumerable());
        }


        public override void UpdateRange(params object[] entities)
        {
            this.UpdateRange(entities.AsEnumerable());
        }

        public virtual void UpdateRange(params TEntity[] entities)
        {
            this.UpdateRange(entities.AsEnumerable());
        }
        
        #endregion


        #region 泛型操作：删除数据（单个删除、批量删除）

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                return;
            entity = entity.DuplicateWithNonNavigations(this.DbContext);

            DbSet<TEntity> set = this.DbContext.Set<TEntity>();
            DbEntityEntry<TEntity> entry = this.DbContext.FindEntry(entity, false);
            if (entry.State == EntityState.Detached)
            {
                set.Attach(entity);
            }
            set.Remove(entity);
        }

        public override void Delete(object entity)
        {
            if (entity == null)
                return;

            this.Delete(entity.CastTo<TEntity>());
        }


        public override void DeleteRange(IEnumerable<object> entities)
        {
            if (entities.IsNullOrEmpty())
                return;

            this.DeleteRange(entities.Select(entity => entity.CastTo<TEntity>()));
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            if (entities.IsNullOrEmpty())
                return;

            bool autoDetectChangesEnabled = this.DbContext.Configuration.AutoDetectChangesEnabled;
            this.DbContext.Configuration.AutoDetectChangesEnabled = false;

            //DbSet<TEntity> set = this.DbContext.Set<TEntity>();
            //set.RemoveRange(entities);
            foreach (TEntity entity in entities)
            {
                if (entity == null)
                    continue;

                this.Delete(entity);
            }

            this.DbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public override void DeleteRange(params object[] entities)
        {
            this.DeleteRange(entities.AsEnumerable());
        }

        public virtual void DeleteRange(params TEntity[] entities)
        {
            this.DeleteRange(entities.AsEnumerable());
        }


        public virtual void DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            this.DeleteRange(this.DbContext.Set<TEntity>().Where(predicate));
        }

        public virtual void DeleteRange(Expression<Func<TEntity, int, bool>> predicate)
        {
            this.DeleteRange(this.DbContext.Set<TEntity>().Where(predicate));
        }

        #endregion


        #region 泛型操作：清空表数据

        public virtual void DeleteAll()
        {
            this.DeleteRange(entity => 1 == 1);
        }

        /// <summary>
        /// 清空指定的实体数据模型类型所对应的数据表中的所有数据。
        /// </summary>
        /// <remarks>注意：该操作不受数据库访问事务影响（不管有无设定事务或者事务有无提交，该操作都立即生效且不可逆）。</remarks>
        public virtual void Truncate()
        {
            this.Truncate(typeof(TEntity));
        }

        #endregion


        #region 泛型操作：查询数据（查询单个实体、查询多个、分页查询）

        public virtual TEntity SelectSingle(IEnumerable<object> keyValues)
        {
            Check.NotEmpty(keyValues);
            keyValues = keyValues.Where(keyValue => keyValue != null);

            DbSet<TEntity> set = this.DbContext.Set<TEntity>();
            return set.Find(keyValues).Duplicate();
        }

        public virtual TEntity SelectSingle(params object[] keyValues)
        {
            return this.SelectSingle(keyValues.AsEnumerable());
        }


        public virtual TEntity SelectSingle(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbContext.Set<TEntity>().Where(predicate).FirstOrDefault().Duplicate();
        }

        public virtual TEntity SelectSingle(Expression<Func<TEntity, int, bool>> predicate)
        {
            return this.DbContext.Set<TEntity>().Where(predicate).FirstOrDefault().Duplicate();
        }

        public virtual TEntity SelectSingle(Expression<Func<TEntity, bool>> predicate, string keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotEmpty(keySelector);
            IQueryable<TEntity> queryable = this.DbContext.Set<TEntity>().Where(predicate);
            return this.OrderBy(queryable, keySelector, sortDirection).FirstOrDefault().Duplicate();
        }

        public virtual TEntity SelectSingle(Expression<Func<TEntity, int, bool>> predicate, string keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotEmpty(keySelector);
            IQueryable<TEntity> queryable = this.DbContext.Set<TEntity>().Where(predicate);
            return this.OrderBy(queryable, keySelector, sortDirection).FirstOrDefault().Duplicate();
        }

        public virtual TEntity SelectSingle<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotNull(keySelector);
            return this.DbContext.Set<TEntity>().Where(predicate).OrderBy(keySelector, sortDirection).FirstOrDefault().Duplicate();
        }

        public virtual TEntity SelectSingle<TKey>(Expression<Func<TEntity, int, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotNull(keySelector);
            return this.DbContext.Set<TEntity>().Where(predicate).OrderBy(keySelector, sortDirection).FirstOrDefault().Duplicate();
        }


        public virtual TEntity[] Select(IEnumerable<object> keyValues)
        {
            Check.NotEmpty(keyValues);
            keyValues = keyValues.Where(keyValue => keyValue != null);

            DbSet<TEntity> set = this.DbContext.Set<TEntity>();
            List<TEntity> list = new List<TEntity>();
            foreach (object keyValue in keyValues)
            {
                TEntity entity = set.Find(keyValue);
                if (entity != null)
                    list.Add(entity);
            }
            return list.Select(item => item.Duplicate()).ToArray();
        }

        public virtual TEntity[] Select(params object[] keyValues)
        {
            return this.Select(keyValues.AsEnumerable());
        }

        public virtual TEntity[] Select(IEnumerable<IEnumerable<object>> keyValuesArray)
        {
            Check.NotEmpty(keyValuesArray);
            keyValuesArray = keyValuesArray.Where(keyValues => !keyValues.IsNullOrEmpty());

            DbSet<TEntity> set = this.DbContext.Set<TEntity>();
            List<TEntity> list = new List<TEntity>();
            foreach (object keyValues in keyValuesArray)
            {
                TEntity entity = set.Find(keyValues);
                if (entity != null)
                    list.Add(entity);
            }
            return list.Select(item => item.Duplicate()).ToArray();
        }

        public virtual TEntity[] Select(params IEnumerable<object>[] keyValuesArray)
        {
            return this.Select(keyValuesArray.AsEnumerable());
        }

        public virtual TEntity[] Select(Expression<Func<TEntity, bool>> predicate)
        {
            Check.NotNull(predicate);
            return this.DbContext.Set<TEntity>().Where(predicate).ToArray().Select(item => item.Duplicate()).ToArray();
        }

        public virtual TEntity[] Select(Expression<Func<TEntity, int, bool>> predicate)
        {
            Check.NotNull(predicate);
            return this.DbContext.Set<TEntity>().Where(predicate).ToArray().Select(item => item.Duplicate()).ToArray();
        }

        public virtual TEntity[] Select(Expression<Func<TEntity, bool>> predicate, string keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotEmpty(keySelector);
            IQueryable<TEntity> queryable = this.DbContext.Set<TEntity>().Where(predicate);
            return this.OrderBy(queryable, keySelector, sortDirection).ToArray().Select(item => item.Duplicate()).ToArray();
        }

        public virtual TEntity[] Select(Expression<Func<TEntity, int, bool>> predicate, string keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotEmpty(keySelector);
            IQueryable<TEntity> queryable = this.DbContext.Set<TEntity>().Where(predicate);
            return this.OrderBy(queryable, keySelector, sortDirection).ToArray().Select(item => item.Duplicate()).ToArray();
        }

        public virtual TEntity[] Select<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotNull(keySelector);
            return this.DbContext.Set<TEntity>().Where(predicate).OrderBy(keySelector, sortDirection).ToArray().Select(item => item.Duplicate()).ToArray();
        }

        public virtual TEntity[] Select<TKey>(Expression<Func<TEntity, int, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotNull(keySelector);
            return this.DbContext.Set<TEntity>().Where(predicate).OrderBy(keySelector, sortDirection).ToArray().Select(item => item.Duplicate()).ToArray();
        }



        public virtual TEntity[] SelectPage(int pageIndex, int pageSize)
        {
            //return this.DbContext.Set<TEntity>().SplitPage(pageIndex, pageSize).ToArray().Select(item => item.Duplicate()).ToArray();
            ListSortDirection sortDirection = ListSortDirection.Ascending;
            return this.SelectPage(pageIndex, pageSize, item => 0, sortDirection);
        }

        public virtual TEntity[] SelectPage(int pageIndex, int pageSize, string keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotEmpty(keySelector);
            DbSet<TEntity> set = this.DbContext.Set<TEntity>();
            return this.OrderBy(set, keySelector, sortDirection).SplitPage(pageIndex, pageSize).ToArray().Select(item => item.Duplicate()).ToArray();
        }

        public virtual TEntity[] SelectPage<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(keySelector);
            return this.DbContext.Set<TEntity>().OrderBy(keySelector, sortDirection).SplitPage(pageIndex, pageSize).ToArray().Select(item => item.Duplicate()).ToArray();
        }


        public virtual TEntity[] SelectPage(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            Check.NotNull(predicate);
            //return this.DbContext.Set<TEntity>().Where(predicate).SplitPage(pageIndex, pageSize).ToArray().Select(item => item.Duplicate()).ToArray();
            ListSortDirection sortDirection = ListSortDirection.Ascending;
            return this.SelectPage(predicate, pageIndex, pageSize, item => 0, sortDirection);
        }

        public virtual TEntity[] SelectPage(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, string keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotEmpty(keySelector);
            IQueryable<TEntity> queryable = this.DbContext.Set<TEntity>().Where(predicate);
            return this.OrderBy(queryable, keySelector, sortDirection).SplitPage(pageIndex, pageSize).ToArray().Select(item => item.Duplicate()).ToArray();
        }

        public virtual TEntity[] SelectPage<TKey>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotNull(keySelector);
            return this.DbContext.Set<TEntity>().Where(predicate).OrderBy(keySelector, sortDirection).SplitPage(pageIndex, pageSize).ToArray().Select(item => item.Duplicate()).ToArray();
        }


        public virtual TEntity[] SelectPage(Expression<Func<TEntity, int, bool>> predicate, int pageIndex, int pageSize)
        {
            Check.NotNull(predicate);
            //return this.DbContext.Set<TEntity>().Where(predicate).SplitPage(pageIndex, pageSize).ToArray().Select(item => item.Duplicate()).ToArray();
            ListSortDirection sortDirection = ListSortDirection.Ascending;
            return this.SelectPage(predicate, pageIndex, pageSize, item => 0, sortDirection);
        }

        public virtual TEntity[] SelectPage(Expression<Func<TEntity, int, bool>> predicate, int pageIndex, int pageSize, string keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotEmpty(keySelector);
            IQueryable<TEntity> queryable = this.DbContext.Set<TEntity>().Where(predicate);
            return this.OrderBy(queryable, keySelector, sortDirection).SplitPage(pageIndex, pageSize).ToArray().Select(item => item.Duplicate()).ToArray();
        }

        public virtual TEntity[] SelectPage<TKey>(Expression<Func<TEntity, int, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotNull(keySelector);
            return this.DbContext.Set<TEntity>().Where(predicate).OrderBy(keySelector, sortDirection).SplitPage(pageIndex, pageSize).ToArray().Select(item => item.Duplicate()).ToArray();
        }



        public virtual PagingData<TEntity> SelectPaging(int pageIndex, int pageSize)
        {
            //PagingData<TEntity> data = this.DbContext.Set<TEntity>().ToPagingData(pageIndex, pageSize);
            //data.Data = data.Data.Select(item => item.Duplicate());
            //return data;
            ListSortDirection sortDirection = ListSortDirection.Ascending;
            return this.SelectPaging(pageIndex, pageSize, item => 0, sortDirection);
        }

        public virtual PagingData<TEntity> SelectPaging(int pageIndex, int pageSize, string keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotEmpty(keySelector);
            DbSet<TEntity> set = this.DbContext.Set<TEntity>();

            PagingData<TEntity> data = this.OrderBy(set, keySelector, sortDirection).ToPagingData(pageIndex, pageSize);
            data.Data = data.Data.Select(item => item.Duplicate());
            return data;
        }

        public virtual PagingData<TEntity> SelectPaging<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(keySelector);

            PagingData<TEntity> data = this.DbContext.Set<TEntity>().OrderBy(keySelector, sortDirection).ToPagingData(pageIndex, pageSize);
            data.Data = data.Data.Select(item => item.Duplicate());
            return data;
        }


        public virtual PagingData<TEntity> SelectPaging(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            //Check.NotNull(predicate);
            //PagingData<TEntity> data = this.DbContext.Set<TEntity>().Where(predicate).ToPagingData(pageIndex, pageSize);
            //data.Data = data.Data.Select(item => item.Duplicate());
            //return data;
            ListSortDirection sortDirection = ListSortDirection.Ascending;
            return this.SelectPaging(predicate, pageIndex, pageSize, item => 0, sortDirection);
        }

        public virtual PagingData<TEntity> SelectPaging(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, string keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotEmpty(keySelector);

            IQueryable<TEntity> queryable = this.DbContext.Set<TEntity>().Where(predicate);
            PagingData<TEntity> data = this.OrderBy(queryable, keySelector, sortDirection).ToPagingData(pageIndex, pageSize);
            data.Data = data.Data.Select(item => item.Duplicate());
            return data;
        }

        public virtual PagingData<TEntity> SelectPaging<TKey>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotNull(keySelector);

            PagingData<TEntity> data = this.DbContext.Set<TEntity>().Where(predicate).OrderBy(keySelector, sortDirection).ToPagingData(pageIndex, pageSize);
            data.Data = data.Data.Select(item => item.Duplicate());
            return data;
        }


        public virtual PagingData<TEntity> SelectPaging(Expression<Func<TEntity, int, bool>> predicate, int pageIndex, int pageSize)
        {
            //Check.NotNull(predicate);
            //PagingData<TEntity> data = this.DbContext.Set<TEntity>().Where(predicate).ToPagingData(pageIndex, pageSize);
            //data.Data = data.Data.Select(item => item.Duplicate());
            //return data;
            ListSortDirection sortDirection = ListSortDirection.Ascending;
            return this.SelectPaging(predicate, pageIndex, pageSize, item => 0, sortDirection);
        }

        public virtual PagingData<TEntity> SelectPaging(Expression<Func<TEntity, int, bool>> predicate, int pageIndex, int pageSize, string keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotEmpty(keySelector);
            IQueryable<TEntity> queryable = this.DbContext.Set<TEntity>().Where(predicate);

            PagingData<TEntity> data = this.OrderBy(queryable, keySelector, sortDirection).ToPagingData(pageIndex, pageSize);
            data.Data = data.Data.Select(item => item.Duplicate());
            return data;
        }

        public virtual PagingData<TEntity> SelectPaging<TKey>(Expression<Func<TEntity, int, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Check.NotNull(predicate);
            Check.NotNull(keySelector);

            PagingData<TEntity> data = this.DbContext.Set<TEntity>().Where(predicate).OrderBy(keySelector, sortDirection).ToPagingData(pageIndex, pageSize);
            data.Data = data.Data.Select(item => item.Duplicate());
            return data;
        }


        public virtual TEntity[] SelectAll()
        {
            return this.DbContext.Set<TEntity>().ToArray().Select(item => item.Duplicate()).ToArray();
        }


        #endregion



        #region Internal Methods

        /// <summary>
        /// 获取静态类型 <see cref="NDF.Utilities.Queryable"/> 中定义的
        ///     public static IOrderedQueryable&lt;TSource&gt; OrderBy&lt;TSource, TKey&gt;(IQueryable&lt;TSource&gt;, Expression&lt;Func&lt;TSource, TKey&gt;&gt;, ListSortDirection)
        ///     方法。
        /// </summary>
        private MethodInfo OrderByMethod
        {
            get
            {
                if (this._orderByMethod == null)
                {
                    this._orderByMethod = typeof(NDF.Utilities.Queryable).GetMethods().First(m =>
                    {
                        if (m.Name != "OrderBy" || !m.IsGenericMethod)
                            return false;
                        ParameterInfo[] parameters = m.GetParameters();
                        return parameters.Length == 3 && parameters[2].ParameterType == typeof(ListSortDirection);
                    });
                }
                return this._orderByMethod;
            }
        }

        /// <summary>
        /// 根据 <paramref name="propertyOrFieldName"/> 参数所示的字段对查询进行排序操作。
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="propertyOrFieldName"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        private IQueryable<TEntity> OrderBy(IQueryable<TEntity> queryable, string propertyOrFieldName, ListSortDirection sortDirection)
        {
            Type entityType = typeof(TEntity);
            Type propertyOrFieldType = null;
            LambdaExpression expr = Expressions.Lambda(entityType, propertyOrFieldName, out propertyOrFieldType);
            MethodInfo orderbyMethod = this.OrderByMethod.MakeGenericMethod(entityType, propertyOrFieldType);
            return orderbyMethod.Invoke(null, new object[] { queryable, expr, sortDirection }) as IQueryable<TEntity>;
        }

        #endregion


    }
}
