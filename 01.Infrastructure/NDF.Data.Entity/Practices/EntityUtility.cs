using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.Practices
{
    /// <summary>
    /// 提供一组 EntityFramework 实体框架环境下的工具方法。
    /// </summary>
    public static class EntityUtility
    {
        /// <summary>
        /// 获取 实体数据模型类型 中的所有公共属性。
        /// </summary>
        /// <param name="entityType">表示一个实体数据模型类型。</param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(Type entityType)
        {
            Check.NotNull(entityType);
            return entityType.GetProperties();
        }

        /// <summary>
        /// 获取 实体数据模型类型 中的所有公共属性。
        /// </summary>
        /// <typeparam name="TEntity">表示一个实体数据模型类型。</typeparam>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties<TEntity>() where TEntity : EntityModel
        {
            return GetProperties(typeof(TEntity));
        }



        /// <summary>
        /// 获取 实体数据模型类型 中的所有主键组成的一个数组。如果该 实体数据模型类型 没有定义主键，则返回一个空数组。
        /// </summary>
        /// <param name="entityType">表示一个实体数据模型类型。</param>
        /// <returns></returns>
        public static PropertyInfo[] GetPrimaryKeys(Type entityType)
        {
            List<PropertyInfo> list = new List<PropertyInfo>();
            foreach (PropertyInfo property in GetProperties(entityType))
            {
                var attrs = property.GetCustomAttributes<KeyAttribute>(inherit: true);
                if (!attrs.IsNullOrEmpty())
                {
                    list.Add(property);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 获取 实体数据模型类型 中的所有主键组成的一个数组。如果该 实体数据模型类型 没有定义主键，则返回一个空数组。
        /// </summary>
        /// <typeparam name="TEntity">表示一个实体数据模型类型。</typeparam>
        /// <returns></returns>
        public static PropertyInfo[] GetPrimaryKeys<TEntity>() where TEntity : EntityModel
        {
            return GetPrimaryKeys(typeof(TEntity));
        }



        /// <summary>
        /// 获取 实体数据模型类型 中的所有主键的名称组成的一个数组。如果该 实体数据模型类型 没有定义主键，则返回一个空数组。
        /// </summary>
        /// <param name="entityType">表示一个实体数据模型类型。</param>
        /// <returns></returns>
        public static string[] GetPrimaryKeyNames(Type entityType)
        {
            return GetProperties(entityType).Select(key => key.Name).ToArray();
        }

        /// <summary>
        /// 获取 实体数据模型类型 中的所有主键的名称组成的一个数组。如果该 实体数据模型类型 没有定义主键，则返回一个空数组。
        /// </summary>
        /// <typeparam name="TEntity">表示一个实体数据模型类型。</typeparam>
        /// <returns></returns>
        public static string[] GetPrimaryKeyNames<TEntity>() where TEntity : EntityModel
        {
            return GetPrimaryKeyNames(typeof(TEntity));
        }



        /// <summary>
        /// 获取 实体数据模型类型 中的所有主键的值组成的一个数组。如果该 实体数据模型类型 没有定义主键，则返回一个空数组。
        /// </summary>
        /// <param name="entity">表示一个实体数据模型类型。</param>
        /// <returns></returns>
        public static object[] GetPrimaryKeyValues(object entity)
        {
            Check.NotNull(entity);
            return GetPrimaryKeys(entity.GetType()).Select(key => key.GetValue(entity)).ToArray();
        }

        /// <summary>
        /// 获取 实体数据模型类型 中的所有主键的值组成的一个数组。如果该 实体数据模型类型 没有定义主键，则返回一个空数组。
        /// </summary>
        /// <typeparam name="TEntity">表示一个实体数据模型类型。</typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object[] GetPrimaryKeyValues<TEntity>(TEntity entity) where TEntity : EntityModel
        {
            return GetPrimaryKeyValues(entity as object);
        }

    }
}
