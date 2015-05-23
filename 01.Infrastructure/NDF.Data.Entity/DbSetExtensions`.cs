using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity
{
    /// <summary>
    /// 提供一组对 <see cref="IDbSet&lt;TEntity&gt;"/> 对象的扩展 API。
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// 将数据实体对象保存至数据库。
        /// 如果数据库中被判定存在该记录，则进行 Update 操作，否则进行 Insert 操作。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_this"></param>
        /// <param name="entity"></param>
        public static void AddOrUpdate<TEntity>(this IDbSet<TEntity> _this, object entity) where TEntity : class
        {
            AddOrUpdate<TEntity>(_this, entity.CastTo<TEntity>());
        }

        /// <summary>
        /// 将数据实体对象保存至数据库。
        /// 如果数据库中被判定存在该记录，则进行 Update 操作，否则进行 Insert 操作。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_this"></param>
        /// <param name="entity"></param>
        public static void AddOrUpdate<TEntity>(this IDbSet<TEntity> _this, TEntity entity) where TEntity : class
        {
            DbSetMigrationsExtensions.AddOrUpdate<TEntity>(_this, entity);
        }


        /// <summary>
        /// 批量将一组 EF 数据实体对象保存至数据库。
        /// 针对集合中的每个数据实体对象，如果数据库中被判定存在该记录，则进行 Update 操作，否则进行 Insert 操作。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_this"></param>
        /// <param name="entities"></param>
        public static void AddOrUpdateRange<TEntity>(this IDbSet<TEntity> _this, IEnumerable<object> entities) where TEntity : class
        {
            AddOrUpdateRange<TEntity>(_this, entities.Select(entity => entity.CastTo<TEntity>()));
        }

        /// <summary>
        /// 批量将一组 EF 数据实体对象保存至数据库。
        /// 针对集合中的每个数据实体对象，如果数据库中被判定存在该记录，则进行 Update 操作，否则进行 Insert 操作。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_this"></param>
        /// <param name="entities"></param>
        public static void AddOrUpdateRange<TEntity>(this IDbSet<TEntity> _this, params object[] entities) where TEntity : class
        {
            AddOrUpdateRange<TEntity>(_this, entities.AsEnumerable());
        }


        /// <summary>
        /// 批量将一组 EF 数据实体对象保存至数据库。
        /// 针对集合中的每个数据实体对象，如果数据库中被判定存在该记录，则进行 Update 操作，否则进行 Insert 操作。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_this"></param>
        /// <param name="entities"></param>
        public static void AddOrUpdateRange<TEntity>(this IDbSet<TEntity> _this, IEnumerable<TEntity> entities) where TEntity : class
        {
            DbSetMigrationsExtensions.AddOrUpdate<TEntity>(_this, entities.ToArray());
        }

        /// <summary>
        /// 批量将一组 EF 数据实体对象保存至数据库。
        /// 针对集合中的每个数据实体对象，如果数据库中被判定存在该记录，则进行 Update 操作，否则进行 Insert 操作。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_this"></param>
        /// <param name="entities"></param>
        public static void AddOrUpdateRange<TEntity>(this IDbSet<TEntity> _this, params TEntity[] entities) where TEntity : class
        {
            AddOrUpdateRange<TEntity>(_this, entities.AsEnumerable());
        }

    }
}
