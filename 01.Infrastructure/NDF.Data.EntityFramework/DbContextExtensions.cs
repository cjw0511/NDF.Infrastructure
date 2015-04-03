using NDF.Data.EntityFramework.Annotations;
using NDF.Data.EntityFramework.Edm;
using NDF.Data.EntityFramework.Practices;
using NDF.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework
{
    /// <summary>
    /// 为 <see cref="System.Data.Entity.DbContext"/> 实例提供一组工具方法扩展。
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// 获取基于当前数据库上下文环境的数据库访问基础组件对象 <see cref="NDF.Data.Common.Database"/>。该对象可用于实现数据库的快速访问操作。
        /// </summary>
        /// <param name="context">表示当前数据库访问上下文 <see cref="System.Data.Entity.DbContext"/> 对象。</param>
        /// <returns>返回基于当前数据库上下文环境的数据库访问基础组件对象 <see cref="NDF.Data.Common.Database"/>。</returns>
        public static NDF.Data.Common.Database GetNDFDatabase(this System.Data.Entity.DbContext context)
        {
            return context.Database.GetGeneralDatabase();
        }

        /// <summary>
        /// 获取当前 实体上下文 对象中所用的 对象上下文 <see cref="System.Data.Entity.Core.Objects.ObjectContext"/> 对象。
        /// </summary>
        /// <param name="_this">当前 实体上下文 对象。</param>
        /// <returns>返回一个对象上下文 <see cref="System.Data.Entity.Core.Objects.ObjectContext"/> 对象。</returns>
        public static ObjectContext GetObjectContext(this System.Data.Entity.DbContext _this)
        {
            return ((IObjectContextAdapter)_this).ObjectContext;
        }



        /// <summary>
        /// 获取给定实体的 <see cref="DbEntityEntry"/> 对象，以便提供对与该实体有关的信息的访问以及对实体执行操作的功能。
        /// <para>参数 <paramref name="updateValues"/> 指示在找到该实体对象后是否根据传入的 <paramref name="entity"/> 实体值更新找到的对象。</para>
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="entity"></param>
        /// <param name="updateValues"></param>
        /// <returns></returns>
        public static DbEntityEntry FindEntry(this System.Data.Entity.DbContext _this, object entity, bool updateValues = true)
        {
            Check.NotNull(entity);
            DbEntityEntry entry = _this.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                Type entityType = entity.GetType();
                var entries = _this.ChangeTracker.Entries().Where(e => e != null && e.Entity != null && e.Entity.GetType() == entityType).ToArray();
                if (entries.Length > 0)
                {
                    EntityTable table = GetEntityTable(_this, entityType);
                    if (table != null)
                    {
                        var properties = table.ModelType.KeyProperties;
                        var temp = entries.FirstOrDefault(e => properties.All(p => Object.Equals(e.Property(p.Name).CurrentValue, entry.Property(p.Name).CurrentValue)));

                        if (temp != null)
                        {
                            if (updateValues)
                                temp.CurrentValues.SetValues(entity);

                            entry = temp;
                        }
                    }
                }
            }
            return entry;
        }

        /// <summary>
        /// 获取给定实体的 <see cref="DbEntityEntry&lt;TEntity&gt;"/> 对象，以便提供对与该实体有关的信息的访问以及对实体执行操作的功能。
        /// <para>参数 <paramref name="updateValues"/> 指示在找到该实体对象后是否根据传入的 <paramref name="entity"/> 实体值更新找到的对象。</para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_this"></param>
        /// <param name="entity"></param>
        /// <param name="updateValues"></param>
        /// <returns></returns>
        public static DbEntityEntry<TEntity> FindEntry<TEntity>(this System.Data.Entity.DbContext _this, TEntity entity, bool updateValues = true)
            where TEntity : class, new()
        {
            Check.NotNull(entity);
            DbEntityEntry<TEntity> entry = _this.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                var entries = _this.ChangeTracker.Entries<TEntity>().Where(e => e != null && e.Entity != null).ToArray();
                if (entries.Length > 0)
                {
                    EntityTable table = GetEntityTable<TEntity>(_this);
                    if (table != null)
                    {
                        var properties = table.ModelType.KeyProperties;
                        var temp = entries.FirstOrDefault(e => properties.All(p => Object.Equals(e.Property(p.Name).CurrentValue, entry.Property(p.Name).CurrentValue)));

                        if (temp != null && updateValues)
                        {
                            if (updateValues)
                                temp.CurrentValues.SetValues(entity);

                            entry = temp;
                        }
                    }
                }
            }
            return entry;
        }



        /// <summary>
        /// 将在此上下文中所做的所有更改保存到基础数据库。<paramref name="validateOnSaveEnabled"/> 参数指示在调用 SaveChanges() 时，是否应自动验证所跟踪的实体。
        /// </summary>
        /// <param name="context">表示当前数据库访问上下文 <see cref="System.Data.Entity.DbContext"/> 对象。</param>
        /// <param name="validateOnSaveEnabled">指示在调用 SaveChanges() 时，是否应自动验证所跟踪的实体。</param>
        /// <returns>已写入基础数据库的对象的数目。</returns>
        public static int SaveChanges(this System.Data.Entity.DbContext context, bool validateOnSaveEnabled)
        {
            bool enabled = context.Configuration.ValidateOnSaveEnabled;
            context.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
            try
            {
                context.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
                return context.SaveChanges();
            }
            finally
            {
                context.Configuration.ValidateOnSaveEnabled = enabled;
            }
        }


        /// <summary>
        /// 将在此上下文中所做的所有更改异步保存到基础数据库。<paramref name="validateOnSaveEnabled"/> 参数指示在调用 SaveChanges() 时，是否应自动验证所跟踪的实体。
        /// </summary>
        /// <param name="context">表示当前数据库访问上下文 <see cref="System.Data.Entity.DbContext"/> 对象。</param>
        /// <param name="validateOnSaveEnabled">指示在调用 SaveChanges() 时，是否应自动验证所跟踪的实体。</param>
        /// <returns>表示异步保存操作的任务。任务结果包含已写入基础数据库的对象数目。</returns>
        public static Task<int> SaveChangesAsync(this System.Data.Entity.DbContext context, bool validateOnSaveEnabled)
        {
            bool enabled = context.Configuration.ValidateOnSaveEnabled;
            context.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
            try
            {
                context.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
                return context.SaveChangesAsync();
            }
            finally
            {
                context.Configuration.ValidateOnSaveEnabled = enabled;
            }
        }

        /// <summary>
        /// 将在此上下文中所做的所有更改异步保存到基础数据库。<paramref name="validateOnSaveEnabled"/> 参数指示在调用 SaveChanges() 时，是否应自动验证所跟踪的实体。
        /// </summary>
        /// <param name="context">表示当前数据库访问上下文 <see cref="System.Data.Entity.DbContext"/> 对象。</param>
        /// <param name="cancellationToken">等待任务完成期间要观察的 <see cref="CancellationToken"/>。</param>
        /// <param name="validateOnSaveEnabled">指示在调用 SaveChanges() 时，是否应自动验证所跟踪的实体。</param>
        /// <returns>表示异步保存操作的任务。任务结果包含已写入基础数据库的对象数目。</returns>
        public static Task<int> SaveChangesAsync(this System.Data.Entity.DbContext context, CancellationToken cancellationToken, bool validateOnSaveEnabled)
        {
            bool enabled = context.Configuration.ValidateOnSaveEnabled;
            context.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
            try
            {
                context.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
                return context.SaveChangesAsync(cancellationToken);
            }
            finally
            {
                context.Configuration.ValidateOnSaveEnabled = enabled;
            }
        }



        /// <summary>
        /// 获取当前 实体数据库上下文对象 所示类型中定义的所有 <see cref="DbSet&lt;TEntity&gt;"/> 数据集合属性。
        /// </summary>
        /// <param name="_this">表示一个 实体数据库上下文对象。</param>
        /// <returns>返回 <paramref name="_this"/> 所示的 实体数据库上下文 对象类型中定义的所有 <see cref="DbSet&lt;TEntity&gt;"/> 数据集合属性。</returns>
        public static PropertyInfo[] GetSetProperties(this System.Data.Entity.DbContext _this)
        {
            return GetSetProperties(_this, true);
        }

        /// <summary>
        /// 获取当前 实体数据库上下文对象 所示类型中定义的所有 <see cref="DbSet&lt;TEntity&gt;"/> 数据集合属性。
        /// </summary>
        /// <param name="_this">表示一个 实体数据库上下文对象。</param>
        /// <param name="notHasGeneric">一个布尔类型值，表示该方法返回的结果中是否包含非泛型的 <see cref="DbSet"/> 属性定义，该值默认为 true。</param>
        /// <returns>返回 <paramref name="_this"/> 所示的 实体数据库上下文 对象类型中定义的所有 <see cref="DbSet&lt;TEntity&gt;"/> 数据集合属性。</returns>
        public static PropertyInfo[] GetSetProperties(this System.Data.Entity.DbContext _this, bool notHasGeneric = true)
        {
            Check.NotNull(_this);
            return _this.GetType().GetProperties().Where(p =>
            {
                Type type = p.PropertyType, dbSetTypeG = typeof(DbSet<>);
                return notHasGeneric
                    ? (typeof(DbSet).IsAssignableFrom(type)
                        || dbSetTypeG.IsAssignableFrom(type.IsGenericType ? type.GetGenericTypeDefinition() : type))
                    : dbSetTypeG.IsAssignableFrom(type.IsGenericType ? type.GetGenericTypeDefinition() : type);

            }).ToArray();
        }

        /// <summary>
        /// 获取当前 实体数据库上下文对象 所示类型中定义的所有 <see cref="DbSet&lt;TEntity&gt;"/> 数据集合属性的属性数据类型（即 <see cref="DbSet&lt;TEntity&gt;"/> 类型）集合。
        /// </summary>
        /// <param name="_this">表示一个 实体数据库上下文对象。</param>
        /// <returns>返回 <paramref name="_this"/> 所示的 实体数据库上下文 对象类型中定义的所有 <see cref="DbSet&lt;TEntity&gt;"/> 数据集合属性的属性数据类型（即 <see cref="DbSet&lt;TEntity&gt;"/> 类型）集合。</returns>
        public static Type[] GetSetTypes(this System.Data.Entity.DbContext _this)
        {
            return GetSetTypes(_this, true);
        }

        /// <summary>
        /// 获取当前 实体数据库上下文对象 所示类型中定义的所有 <see cref="DbSet&lt;TEntity&gt;"/> 数据集合属性的属性数据类型（即 <see cref="DbSet&lt;TEntity&gt;"/> 类型）集合。
        /// </summary>
        /// <param name="_this">表示一个 实体数据库上下文对象。</param>
        /// <param name="notHasGeneric">一个布尔类型值，表示该方法返回的结果中是否包含非泛型的 <see cref="DbSet"/> 属性定义，该值默认为 true。</param>
        /// <returns>所示的 实体数据库上下文 对象类型中定义的所有 <see cref="DbSet&lt;TEntity&gt;"/> 数据集合属性的属性数据类型（即 <see cref="DbSet&lt;TEntity&gt;"/> 类型）集合。</returns>
        public static Type[] GetSetTypes(this System.Data.Entity.DbContext _this, bool notHasGeneric = true)
        {
            return GetSetProperties(_this, notHasGeneric).Map(p => p.PropertyType).ToArray();
        }


        /// <summary>
        /// 验证当前 实体数据库上下文对象 所示类型中是否定义了实体类型为 <paramref name="type"/> 的数据集合 <see cref="DbSet&lt;TEntity&gt;"/> 属性。
        /// </summary>
        /// <param name="_this">表示一个 实体数据库上下文对象。</param>
        /// <param name="type">表示需要验证的 实体数据类型。</param>
        /// <returns>如果 <paramref name="_this"/> 所示的 实体数据库上下文对象 所示类型中定义了实体类型为 <paramref name="type"/> 的数据集合 <see cref="DbSet&lt;TEntity&gt;"/> 属性，则返回 true，否则返回 false。</returns>
        public static bool IsDefinedEntity(this System.Data.Entity.DbContext _this, Type type)
        {
            Check.NotNull(type);
            return GetSetTypes(_this, false).Where(t => t.IsGenericType && t.GenericTypeArguments != null && t.GenericTypeArguments.Length > 0).ToArray().Map(t => t.GenericTypeArguments[0]).Any(t => t.Equals(type));
        }

        /// <summary>
        /// 验证当前 实体数据库上下文对象 所示类型中是否定义了实体类型为 <typeparamref name="TEntity"/> 的数据集合 <see cref="DbSet&lt;TEntity&gt;"/> 属性。
        /// </summary>
        /// <param name="_this">表示一个 实体数据库上下文对象。</param>
        /// <typeparam name="TEntity">表示需要验证的 实体数据类型。</typeparam>
        /// <returns>如果 <paramref name="_this"/> 所示的 实体数据库上下文对象 所示类型中定义了实体类型为 <typeparamref name="TEntity"/> 的数据集合 <see cref="DbSet&lt;TEntity&gt;"/> 属性，则返回 true，否则返回 false。</returns>
        public static bool IsDefinedEntity<TEntity>(this System.Data.Entity.DbContext _this)
            where TEntity : class, new()
        {
            return IsDefinedEntity(_this, typeof(TEntity));
        }



        /// <summary>
        /// 清空数据库表中的数据。
        /// </summary>
        /// <param name="_this">表示一个 实体数据库上下文对象。</param>
        /// <param name="tableName">数据库表名称。</param>
        /// <remarks>注意：该操作不受数据库访问事务影响（不管有无设定事务或者事务有无提交，该操作都立即生效且不可逆）。</remarks>
        public static void TruncateTable(this System.Data.Entity.DbContext _this, string tableName)
        {
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                _this.Database.ExecuteSqlCommand("TRUNCATE TABLE " + tableName);
            }
        }

        /// <summary>
        /// 清空指定的实体数据模型类型所对应的数据库表中的所有数据。
        /// </summary>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <typeparam name="TEntity">实体数据模型类型，对应当前数据库上下文中的一个表。</typeparam>
        /// <remarks>注意：该操作不受数据库访问事务影响（不管有无设定事务或者事务有无提交，该操作都立即生效且不可逆）。</remarks>
        public static void TruncateTable<TEntity>(this System.Data.Entity.DbContext _this)
            where TEntity : class, new()
        {
            TruncateTable(_this, typeof(TEntity));
        }

        /// <summary>
        /// 清空指定的实体数据模型类型所对应的数据库表中的所有数据。
        /// </summary>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <param name="entityType">实体数据模型类型，对应当前数据库上下文中的一个表。</param>
        /// <remarks>注意：该操作不受数据库访问事务影响（不管有无设定事务或者事务有无提交，该操作都立即生效且不可逆）。</remarks>
        public static void TruncateTable(this System.Data.Entity.DbContext _this, Type entityType)
        {
            string tableName = GetTableName(_this, entityType);
            TruncateTable(_this, tableName);
        }



        /// <summary>
        /// 获取当前 实体数据上下文 的 元数据工作区 对象。
        /// </summary>
        /// <param name="_this">实体数据上下文 对象。</param>
        /// <returns>返回当前 实体数据上下文 的 元数据工作区 对象。</returns>
        public static MetadataWorkspace GetMetadataWorkspace(this System.Data.Entity.DbContext _this)
        {
            Check.NotNull(_this);
            return GetObjectContext(_this).MetadataWorkspace;
        }

        /// <summary>
        /// 获取当前 实体数据上下文 对象中定义的所有实体集合信息。
        /// </summary>
        /// <param name="_this">实体数据上下文 对象。</param>
        /// <returns>返回当前 实体数据上下文 对象中定义的所有实体集合信息。</returns>
        public static EntitySet[] GetEntitySets(this System.Data.Entity.DbContext _this)
        {
            Check.NotNull(_this);
            Type thisType = _this.GetType();
            EntitySet[] sets = null;
            if (!EntitySetsCache.TryGetValue(thisType, out sets))
            {
                MetadataWorkspace metadata = GetMetadataWorkspace(_this);
                EntityContainer container = metadata.GetItems<EntityContainer>(DataSpace.SSpace).FirstOrDefault();
                if (container == null)
                    throw new InvalidConstraintException("获取实体数据上下文对象中的 实体容器对象失败，无法获取其 EntityContainer。");
                sets = container.EntitySets.ToArray();
                EntitySetsCache.TryAdd(thisType, sets);
            }
            return sets;
        }


        /// <summary>
        /// 获取当前 实体数据模型 上下文中定义的所有 实体数据模型 类型所映射的数据表信息。
        /// </summary>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <returns>返回当前 实体数据模型 上下文中定义的所有 实体数据模型 类型所映射的数据表信息。</returns>
        public static EntityTable[] GetEntityTables(this System.Data.Entity.DbContext _this)
        {
            Check.NotNull(_this);
            Type thisType = _this.GetType();
            EntityTable[] tables = null;
            if (!EntityTablesCache.TryGetValue(thisType, out tables))
            {
                MetadataWorkspace metadata = GetMetadataWorkspace(_this);
                ObjectItemCollection collection = metadata.GetItemCollection(DataSpace.OSpace) as ObjectItemCollection;

                EntityContainer container = metadata.GetItems<EntityContainer>(DataSpace.CSpace).FirstOrDefault();
                if (container == null)
                    throw new InvalidConstraintException("获取实体数据上下文对象中的 实体容器对象失败，无法获取其 EntityContainer。");

                var entitySets = container.EntitySets;
                var entitySetMappings = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace).FirstOrDefault().EntitySetMappings;
                var entityTypes = metadata.GetItems<EntityType>(DataSpace.OSpace);

                List<EntityTable> list = new List<EntityTable>();
                foreach (var entityType in entityTypes)
                {
                    var elemType = collection.GetClrType(entityType);
                    if (elemType == null)
                        continue;

                    var entitySet = entitySets.First(s => s.ElementType.Name == entityType.Name);
                    var mapping = entitySetMappings.First(s => s.EntitySet == entitySet);
                    var mappingFragment = (mapping.EntityTypeMappings.FirstOrDefault(a => a.IsHierarchyMapping) ?? mapping.EntityTypeMappings.First()).Fragments.Single();

                    EntityTable table = new EntityTable();
                    table.ModelSet = entitySet;
                    table.StoreSet = mappingFragment.StoreEntitySet;
                    table.ModelType = entityType;
                    table.StoreType = mappingFragment.StoreEntitySet.ElementType;
                    table.EntityType = elemType;
                    table.TableName = entitySet.GetTableName();
                    table.Schema = entitySet.Schema;
                    list.Add(table);
                }
                tables = list.ToArray();
                EntityTablesCache.TryAdd(thisType, tables);
            }
            return tables;
        }


        /// <summary>
        /// 获取当前 实体数据模型 上下问中指定实体类型的数据库表映射关系。
        /// </summary>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <param name="entityType">实体数据模型类型，对应当前数据库上下文中的一个表。</param>
        /// <returns>返回实体数据模型类型 <paramref name="entityType"/> 对应当前实体数据上下文对象中数据库的相应数据表的映射关系描述对象。</returns>
        public static EntityTable GetEntityTable(this System.Data.Entity.DbContext _this, Type entityType)
        {
            Check.NotNull(_this);
            Check.NotNull(entityType);
            Type thisType = _this.GetType();
            ConcurrentDictionary<Type, EntityTable> dict = null;
            EntityTable table = null;
            if (!EntityTableCache.TryGetValue(thisType, out dict))
            {
                dict = new ConcurrentDictionary<Type, EntityTable>();
                EntityTableCache.TryAdd(thisType, dict);
            }
            if (!dict.TryGetValue(entityType, out table))
            {
                table = GetEntityTables(_this).FirstOrDefault(t => t.EntityType == entityType);
                if (table != null)
                    dict.TryAdd(entityType, table);
            }
            return table;
        }

        /// <summary>
        /// 获取当前 实体数据模型 上下问中指定实体类型的数据库表映射关系。
        /// </summary>
        /// <typeparam name="TEntity">实体数据模型类型，对应当前数据库上下文中的一个表。</typeparam>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <returns>返回实体数据模型类型 <typeparamref name="TEntity"/> 对应当前实体数据上下文对象中数据库的相应数据表的映射关系描述对象。</returns>
        public static EntityTable GetEntityTable<TEntity>(this System.Data.Entity.DbContext _this)
            where TEntity : class, new()
        {
            return GetEntityTable(_this, typeof(TEntity));
        }


        /// <summary>
        /// 根据 实体数据模型 类型获取其对应的数据库表名称。
        /// </summary>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <param name="entityType">实体数据模型类型，对应当前数据库上下文中的一个表。</param>
        /// <returns>返回实体数据模型类型 <paramref name="entityType"/> 对应当前实体数据上下文对象中数据库的相应数据表的表名称。</returns>
        public static string GetTableName(this System.Data.Entity.DbContext _this, Type entityType)
        {
            Check.NotNull(entityType);
            TableAttribute tableAttr = entityType.GetCustomAttributes<TableAttribute>().FirstOrDefault();
            if (tableAttr != null)
                return tableAttr.Name;

            EntityTable table = GetEntityTable(_this, entityType);
            if (table == null)
                throw new ArgumentException("当前实体上下文对象中没有定义类型为 {0} 的实体集合。".Format(entityType));

            return table.TableName;
        }

        /// <summary>
        /// 根据 实体数据模型 类型获取其对应的数据库表名称。
        /// </summary>
        /// <typeparam name="TEntity">实体数据模型类型，对应当前数据库上下文中的一个表。</typeparam>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <returns>返回实体数据模型类型 <typeparamref name="TEntity"/> 对应当前实体数据上下文对象中数据库的相应数据表的表名称。</returns>
        public static string GetTableName<TEntity>(this System.Data.Entity.DbContext _this)
            where TEntity : class, new()
        {
            return GetTableName(_this, typeof(TEntity));
        }



        /// <summary>
        /// 根据 实体数据模型 类型获取其对应的一组主键名称。
        /// </summary>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static string[] GetPrimaryKeyNames(this System.Data.Entity.DbContext _this, Type entityType)
        {
            EntityTable table = GetEntityTable(_this, entityType);
            if (table == null || table.ModelType == null)
                throw new InvalidOperationException(string.Format("类型 {0} 不是实体数据库上下文 {1} 中定义的一个实体模型类型。", entityType, _this));

            if (table.ModelType.KeyMembers == null || table.ModelType.KeyMembers.Count == 0)
                return Enumerable.Empty<string>().ToArray();

            return table.ModelType.KeyMembers.Select(key => key.Name).ToArray();
        }

        /// <summary>
        /// 根据 实体数据模型 类型获取其对应的一组主键名称。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <returns></returns>
        public static string[] GetPrimaryKeyNames<TEntity>(this System.Data.Entity.DbContext _this)
            where TEntity : class, new()
        {
            return GetPrimaryKeyNames(_this, typeof(TEntity));
        }



        /// <summary>
        /// 创建或获取一个非泛型类型的 <see cref="EntityAccessor"/> 实体数据模型访问器对象。<br />
        /// 该 实体数据模型访问器对象 可用于操作任意类型的 实体数据模型。<br />
        /// 因实体数据模型访问器对象由 实体数据上下文 <see cref="DbContext"/> 创建，其 AutoDisposeContext 属性值为 false，表示不销毁其 DbContext 属性。
        /// </summary>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <returns>返回一个非泛型类型的 <see cref="EntityAccessor"/> 实体数据模型访问器对象。</returns>
        public static EntityAccessor Accessor(this System.Data.Entity.DbContext _this)
        {
            EntityAccessor accessor = new EntityAccessor(_this);
            accessor.AutoDisposeContext = false;
            return accessor;
        }

        /// <summary>
        /// 创建或获取一个非泛型类型的 <see cref="EntityAccessor"/> 实体数据模型访问器对象，并指定其可操作的 实体数据模型类型。<br />
        /// 该 实体数据模型访问器对象 在未更改 EntityType 属性的情况下仅可用于操作 指定的实体数据模型类型。<br />
        /// 因实体数据模型访问器对象由 实体数据上下文 <see cref="DbContext"/> 创建，其 AutoDisposeContext 属性值为 false，表示不销毁其 DbContext 属性。
        /// </summary>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <param name="entityType">指定的实体数据模型类型。</param>
        /// <returns>返回一个非泛型类型的 <see cref="EntityAccessor"/> 实体数据模型访问器对象。</returns>
        public static EntityAccessor Accessor(this System.Data.Entity.DbContext _this, Type entityType)
        {
            EntityAccessor accessor = new EntityAccessor(_this, entityType);
            accessor.AutoDisposeContext = false;
            return accessor;
        }

        /// <summary>
        /// 创建或获取一个泛型类型的 <see cref="EntityAccessor"/> 实体数据模型访问器对象，并指定其可操作的 实体数据模型类型。<br />
        /// 该 实体数据模型访问器对象 仅可用于操作 指定的实体数据模型类型。<br />
        /// 因实体数据模型访问器对象由 实体数据上下文 <see cref="DbContext"/> 创建，其 AutoDisposeContext 属性值为 false，表示不销毁其 DbContext 属性。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体数据模型类型。</typeparam>
        /// <param name="_this">实体数据上下文对象。</param>
        /// <returns>返回一个泛型类型的 <see cref="EntityAccessor&lt;TEntity&gt;"/> 实体数据模型访问器对象。</returns>
        public static EntityAccessor<TEntity> Accessor<TEntity>(this System.Data.Entity.DbContext _this)
            where TEntity : class, new()
        {
            EntityAccessor<TEntity> accessor = new EntityAccessor<TEntity>(_this);
            accessor.AutoDisposeContext = false;
            return accessor;
        }



        #region Internal Cache Defined

        private static ConcurrentDictionary<Type, EntitySet[]> _entitySetsCache;
        private static ConcurrentDictionary<Type, EntityTable[]> _entityTablesCache;
        private static ConcurrentDictionary<Type, ConcurrentDictionary<Type, EntityTable>> _entityTableCache;

        internal static ConcurrentDictionary<Type, EntitySet[]> EntitySetsCache
        {
            get
            {
                if (_entitySetsCache == null)
                    _entitySetsCache = new ConcurrentDictionary<Type, EntitySet[]>();
                return _entitySetsCache;
            }
        }

        internal static ConcurrentDictionary<Type, EntityTable[]> EntityTablesCache
        {
            get
            {
                if (_entityTablesCache == null)
                    _entityTablesCache = new ConcurrentDictionary<Type, EntityTable[]>();
                return _entityTablesCache;
            }
        }

        internal static ConcurrentDictionary<Type, ConcurrentDictionary<Type, EntityTable>> EntityTableCache
        {
            get
            {
                if (_entityTableCache == null)
                    _entityTableCache = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, EntityTable>>();
                return _entityTableCache;
            }
        }


        #endregion

    }
}
