using NDF.Data.EntityFramework.Annotations;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework
{
    /// <summary>
    /// 提供一组对 基础对象类型 <see cref="System.Object"/> 操作方法的扩展。
    /// 主要用于基于实体对象的操作进行扩展。
    /// </summary>
    public static class EntityObjectExtensions
    {

        /// <summary>
        /// 基于指定的实体数据库上下文创建一个当前对象的新副本，该副本和当前对象为同一类型，且其中各个属性的值均等同于原对象中各个属性的值，相当于浅表复制操作。
        /// <para>但是该操作在复制数据的过程中将不会复制实体对象的导航（外键引用等）属性。</para>
        /// </summary>
        /// <param name="_this">要执行复制操作的对象。</param>
        /// <param name="context">实体数据库上下文对象。</param>
        /// <returns>返回一个当前对象的副本，返回的对象中各个属性值均相当于源对象 <paramref name="_this"/> 的各个属性值，但导航（外键引用等）属性除外。</returns>
        public static object DuplicateWithNonNavigations(this object _this, System.Data.Entity.DbContext context)
        {
            if (_this == null)
                return null;

            return InternalDuplicateWithNonNavigations(_this, context, _this.GetType(), () => _this.Duplicate());
        }

        /// <summary>
        /// 基于指定的实体数据库上下文创建一个当前对象的新副本，该副本和当前对象为同一类型，且其中各个属性的值均等同于原对象中各个属性的值，相当于浅表复制操作。
        /// <para>但是该操作在复制数据的过程中将不会复制实体对象的导航（外键引用等）属性。</para>
        /// </summary>
        /// <typeparam name="TEntity">要执行复制操作的对象的类型。</typeparam>
        /// <param name="_this">要执行复制操作的对象。</param>
        /// <param name="context">实体数据库上下文对象。</param>
        /// <returns>返回一个当前对象的副本，返回的对象中各个属性值均相当于源对象 <paramref name="_this"/> 的各个属性值，但导航（外键引用等）属性除外。</returns>
        public static TEntity DuplicateWithNonNavigations<TEntity>(this TEntity _this, System.Data.Entity.DbContext context)
            where TEntity : class, new()
        {
            if (_this == null)
                return null;

            return InternalDuplicateWithNonNavigations(_this, context, typeof(TEntity), () => _this.Duplicate()) as TEntity;
        }



        /// <summary>
        /// 基于指定的实体数据库上下文创建一个当前对象的新副本，该副本和当前对象为同一类型，且其中各个属性的值均等同于原对象中各个属性的值，相当于浅表复制操作。
        /// <para>但是该操作在复制数据的过程中将不会复制实体对象的所有键属性值。</para>
        /// </summary>
        /// <param name="_this">要执行复制操作的对象。</param>
        /// <param name="context">实体数据库上下文对象。</param>
        /// <returns>返回一个当前对象的副本，返回的对象中各个属性值均相当于源对象 <paramref name="_this"/> 的各个属性值，但键属性除外。</returns>
        public static object DuplicateWithNonKeys(this object _this, System.Data.Entity.DbContext context)
        {
            if (_this == null)
                return null;

            return InternalDuplicateWithNonKeys(_this, context, _this.GetType(), () => _this.Duplicate());
        }

        /// <summary>
        /// 基于指定的实体数据库上下文创建一个当前对象的新副本，该副本和当前对象为同一类型，且其中各个属性的值均等同于原对象中各个属性的值，相当于浅表复制操作。
        /// <para>但是该操作在复制数据的过程中将不会复制实体对象的所有键属性值。</para>
        /// </summary>
        /// <typeparam name="TEntity">要执行复制操作的对象的类型。</typeparam>
        /// <param name="_this">要执行复制操作的对象。</param>
        /// <param name="context">实体数据库上下文对象。</param>
        /// <returns>返回一个当前对象的副本，返回的对象中各个属性值均相当于源对象 <paramref name="_this"/> 的各个属性值，但键属性除外。</returns>
        public static TEntity DuplicateWithNonKeys<TEntity>(this TEntity _this, System.Data.Entity.DbContext context)
            where TEntity : class, new()
        {
            if (_this == null)
                return null;

            return InternalDuplicateWithNonKeys(_this, context, typeof(TEntity), () => _this.Duplicate()) as TEntity;
        }



        /// <summary>
        /// 基于指定的实体数据库上下文获取实体对象在该上下文中定义的实体数据模型的所有主键成员的值所构成的一个数组。
        /// <para>如果该对象在实体数据模型中只定义了一个主键，则返回的数组中将仅包含一个元素。</para>
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object[] GetPrimaryKeyValues(this object _this, System.Data.Entity.DbContext context)
        {
            Check.NotNull(_this);
            Type thisType = _this.GetType();

            string[] primaryKeys = context.GetPrimaryKeyNames(thisType);
            return InternalGetPrimaryKeyValues(_this, thisType, primaryKeys);
        }

        /// <summary>
        /// 基于指定的实体数据库上下文获取实体对象在该上下文中定义的实体数据模型的所有主键成员的值所构成的一个数组。
        /// <para>如果该对象在实体数据模型中只定义了一个主键，则返回的数组中将仅包含一个元素。</para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_this"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object[] GetPrimaryKeyValues<TEntity>(this TEntity _this, System.Data.Entity.DbContext context)
            where TEntity : class, new()
        {
            Type thisType = typeof(TEntity);

            string[] primaryKeys = context.GetPrimaryKeyNames<TEntity>();
            return InternalGetPrimaryKeyValues(_this, thisType, primaryKeys);
        }


        /// <summary>
        /// 基于指定的实体数据库上下文更新该实体数据模型对象的主键值。
        /// </summary>
        /// <param name="_this">要执行更新主键值操作的对象。</param>
        /// <param name="context">实体数据库上下文对象。</param>
        /// <param name="values">被更新至 <paramref name="_this"/> 实体数据模型对象的一组新主键值。如果 <paramref name="_this"/> 对象的主键只有一个属性，则该属性应仅包含一个元素。</param>
        public static void UpdatePrimaryKeyValues(this object _this, System.Data.Entity.DbContext context, params object[] values)
        {
            Check.NotNull(_this);
            Type thisType = _this.GetType();

            string[] primaryKeys = context.GetPrimaryKeyNames(thisType);
            InternalUpdatePrimaryKeyValues(_this, context, thisType, primaryKeys, values);
        }

        /// <summary>
        /// 基于指定的实体数据库上下文更新该实体数据模型对象的主键值。
        /// </summary>
        /// <typeparam name="TEntity">实体数据模型对象 <paramref name="_this"/> 的类型。</typeparam>
        /// <param name="_this">要执行更新主键值操作的对象。</param>
        /// <param name="context">实体数据库上下文对象。</param>
        /// <param name="values">被更新至 <paramref name="_this"/> 实体数据模型对象的一组新主键值。如果 <paramref name="_this"/> 对象的主键只有一个属性，则该属性应仅包含一个元素。</param>
        public static void UpdatePrimaryKeyValues<TEntity>(this TEntity _this, System.Data.Entity.DbContext context, params object[] values)
            where TEntity : class, new()
        {
            Type thisType = typeof(TEntity);

            string[] primaryKeys = context.GetPrimaryKeyNames<TEntity>();
            InternalUpdatePrimaryKeyValues(_this, context, thisType, primaryKeys, values);
        }



        private static object InternalDuplicateWithNonNavigations(object _this, System.Data.Entity.DbContext context, Type entityType, Func<object> duplicater)
        {
            if (_this == null)
                return null;

            Check.NotNull(context);
            Check.NotNull(entityType);
            Check.NotNull(duplicater);

            if (entityType.IsAbstract)
                entityType = _this.GetType();

            EntityTable table = context.GetEntityTable(entityType);
            if (table == null || table.ModelType == null)
                throw new InvalidOperationException(string.Format("类型 {0} 不是实体数据库上下文 {1} 中定义的一个实体模型类型。", entityType, context));

            NavigationProperty[] nps = table.ModelType.DeclaredNavigationProperties.ToArray();
            if (nps.Length == 0)
                return _this;

            var properties = (from p in entityType.GetProperties()
                              where p.GetMethod != null && p.SetMethod != null && nps.Any(np => np.Name == p.Name)
                              select p).ToArray();
            var fields = (from f in entityType.GetFields()
                          where nps.Any(np => np.Name == f.Name)
                          select f).ToArray();

            if (properties.Length == 0 && fields.Length == 0)
                return _this;

            object obj = duplicater();

            // 将 NavigationProperties 设置为类型默认值，即去除 NavigationProperties 属性的值。
            foreach (PropertyInfo p in properties)
            {
                p.SetValue(obj, null);
            }
            foreach (FieldInfo f in fields)
            {
                f.SetValue(obj, null);
            }

            return obj;
        }

        private static object InternalDuplicateWithNonKeys(object _this, System.Data.Entity.DbContext context, Type entityType, Func<object> duplicater)
        {
            if (_this == null)
                return null;

            Check.NotNull(context);
            Check.NotNull(entityType);
            Check.NotNull(duplicater);

            if (entityType.IsAbstract)
                entityType = _this.GetType();

            EntityTable table = context.GetEntityTable(entityType);
            if (table == null || table.ModelType == null)
                throw new InvalidOperationException(string.Format("类型 {0} 不是实体数据库上下文 {1} 中定义的一个实体模型类型。", entityType, context));

            EdmMember[] keys = table.ModelType.KeyMembers.ToArray();
            if (keys.Length == 0)
                return _this;

            var properties = (from p in entityType.GetProperties()
                              where p.GetMethod != null && p.SetMethod != null && keys.Any(k => k.Name == p.Name)
                              select p).ToArray();
            var fields = (from f in entityType.GetFields()
                          where keys.Any(k => k.Name == f.Name)
                          select f).ToArray();

            if (properties.Length == 0 && fields.Length == 0)
                return _this;

            object obj = duplicater();

            // 将 KeyMembers 设置为类型默认值，即去除 KeyMembers 属性的值。
            foreach (PropertyInfo p in properties)
            {
                p.SetValue(obj, null);
            }
            foreach (FieldInfo f in fields)
            {
                f.SetValue(obj, null);
            }

            return obj;
        }


        private static void InternalUpdatePrimaryKeyValues(object _this, System.Data.Entity.DbContext context, Type entityType, string[] primaryKeys, object[] values)
        {
            Check.NotNull(entityType);
            Check.NotEmpty(primaryKeys);
            Check.NotEmpty(values);

            if (primaryKeys.Length != values.Length)
                throw new ArgumentException(string.Format("传入的新值列表 {0} 长度与参数 {1} 所示实体数据模型类型的主键列表长度不一致。", values, _this));

            Type thisType = entityType;
            List<MemberInfo> list = new List<MemberInfo>();

            for (int i = 0; i < primaryKeys.Length; i++)
            {
                string name = primaryKeys[i];
                object value = values[i];
                MemberInfo member = thisType.GetProperty(name);
                if (member == null)
                    member = thisType.GetField(name);

                if (member == null)
                    throw new InvalidOperationException(string.Format("在实体类型 {0} 中没有找到名称为 {1} 的主键成员，异常出现异常。", thisType, name));

                if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = member as PropertyInfo;
                    if (value != null && !property.PropertyType.IsAssignableFrom(value.GetType()))
                        throw new ArgumentException(string.Format(
                            "参数列表 {0} 中的第 {1} 项值 {2} 的类型与实体数据模型类型 {3} 中主键属性 {4} 的数据类型不一致，不能进行赋值操作。",
                            "values", i, value, thisType, name
                            ));
                }
                else
                {
                    FieldInfo field = member as FieldInfo;
                    if (value != null && !field.FieldType.IsAssignableFrom(value.GetType()))
                        throw new ArgumentException(string.Format(
                            "参数列表 {0} 中的第 {1} 项值 {2} 的类型与实体数据模型类型 {3} 中主键字段 {4} 的数据类型不一致，不能进行赋值操作。",
                            "values", i, value, thisType, name
                            ));
                }
                list.Add(member);
            }

            for (int i = 0; i < list.Count; i++)
            {
                MemberInfo member = list[i];
                object value = values[i];

                if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = member as PropertyInfo;
                    property.SetValue(_this, value);
                }
                else
                {
                    FieldInfo field = member as FieldInfo;
                    field.SetValue(_this, value);
                }
            }
        }

        private static object[] InternalGetPrimaryKeyValues(object _this, Type entityType, string[] primaryKeys)
        {
            Check.NotNull(_this);
            Check.NotNull(entityType);
            Check.NotEmpty(primaryKeys);

            object[] values = new object[primaryKeys.Length];
            for (int i = 0; i < values.Length; i++)
            {
                string name = primaryKeys[i];
                PropertyInfo property = entityType.GetProperty(name);
                if (property != null)
                {
                    if (property.GetMethod == null)
                        throw new InvalidOperationException(string.Format("实体数据模型对象 {0}:{1} 中中名称为 {2} 的主键成员未定义 get 访问器，不能读取该属性值，程序出现异常。", entityType, _this, name));

                    values[i] = property.GetValue(_this);
                }
                else
                {
                    FieldInfo field = entityType.GetField(name);
                    if (field == null)
                        throw new InvalidOperationException(string.Format("没有在实体数据模型对象 {0}:{1} 中找到名称为 {2} 的主键成员，程序出现异常。", entityType, _this, name));

                    values[i] = field.GetValue(_this);
                }
            }
            return values;
        }

    }
}
