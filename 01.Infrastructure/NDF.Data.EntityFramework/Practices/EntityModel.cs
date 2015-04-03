using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.Practices
{
    /// <summary>
    /// 表示实体数据对象模型的基类。
    /// </summary>
    [Serializable]
    public abstract class EntityModel
    {
        private Type _entityType;
        private PropertyInfo[] _properties;
        private Dictionary<string, PropertyInfo> _propertyCache;
        private Dictionary<string, Delegate> _keySelectorCache;

        /// <summary>
        /// 初始化类型 <see cref="EntityModel"/> 的新实例。
        /// </summary>
        protected EntityModel()
        {
            this._entityType = this.GetType();
            this._properties = EntityUtility.GetProperties(this._entityType).Where(p => p.GetMethod != null).ToArray();
            this._propertyCache = new Dictionary<string, PropertyInfo>();
            this._keySelectorCache = new Dictionary<string, Delegate>();
        }



        /// <summary>
        /// 获取或设置当前实体数据模型对象中特定名称属性的值。
        /// </summary>
        /// <param name="propertyName">当前实体数据模型对象中指定的属性名称。</param>
        /// <returns>当执行 Get 操作时，返回特定名称属性的值。</returns>
        /// <exception cref="System.ArgumentException">
        /// 当传入的属性名称错误，不存在该属性时、或者赋值时新值为 Null 但是属性类型为非 Nullable 的值类型时、或者赋值时新值的类型与属性类型不匹配时，抛出该异常。
        /// </exception>
        public virtual object this[string propertyName]
        {
            get { return this.GetProperty(propertyName).GetValue(this); }
            set
            {
                PropertyInfo property = this.GetProperty(propertyName);
                if (property == null)
                    throw new ArgumentException("传入的属性名称 {0} 错误，不存在该属性。".Format(propertyName as object));
                if (property.PropertyType.IsValueType && !property.PropertyType.IsGenericType && value == null)
                    throw new ArgumentException("给属性 {0} 赋值时出现一个错误，该属性的类型是一个值类型 {1}，所以赋的新值不能为 Null。".Format(propertyName as object, property.PropertyType));
                if (value != null && !property.PropertyType.IsAssignableFrom(value.GetType()))
                    throw new ArgumentException("属性 {0} 不能由传入的新值 {1} 来分配。".Format(propertyName as object, value));
                property.SetValue(this, value);
            }
        }


        /// <summary>
        /// 获取当前实体数据模型的类型。
        /// </summary>
        protected internal Type EntityType
        {
            get { return this._entityType; }
        }



        /// <summary>
        /// 获取或设置当前 实体数据模型 对象在数据库中存储 的行版本标识。
        /// </summary>
        /// <remarks>在用于并发处理环境中时，该版本标识非常有用。</remarks>
        [ConcurrencyCheck]
        [Timestamp]
        public virtual byte[] Timestamp { get; set; } 



        /// <summary>
        /// 获取当前 实体数据模型类型 中的所有公共属性。
        /// </summary>
        /// <returns></returns>
        protected virtual PropertyInfo[] GetProperties()
        {
            return this._properties;
        }

        /// <summary>
        /// 获取 实体数据模型类型 中的所有主键组成的一个数组。如果该 实体数据模型类型 没有定义主键，则返回一个空数组。
        /// </summary>
        /// <returns></returns>
        public virtual PropertyInfo[] GetPrimaryKeys()
        {
            return EntityUtility.GetPrimaryKeys(this.GetType());
        }

        /// <summary>
        /// 获取 实体数据模型类型 中的所有主键的名称组成的一个数组。如果该 实体数据模型类型 没有定义主键，则返回一个空数组。
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetPrimaryKeyNames()
        {
            return EntityUtility.GetPrimaryKeyNames(this.GetType());
        }

        /// <summary>
        /// 获取 实体数据模型类型 中的所有主键的值组成的一个数组。如果该 实体数据模型类型 没有定义主键，则返回一个空数组。
        /// </summary>
        /// <returns></returns>
        public virtual object[] GetPrimaryKeyValues()
        {
            return EntityUtility.GetPrimaryKeyValues(this);
        }




        /// <summary>
        /// 获取当前实体数据模型类型中指定名称的属性信息。
        /// </summary>
        /// <param name="name">指定的属性名称。</param>
        /// <returns>如果当前实体数据模型类型中存在指定名称的属性信息，则返回该属性信息；否则返回 Null。</returns>
        public virtual PropertyInfo GetProperty(string name)
        {
            Check.NotEmpty(name);
            PropertyInfo property = null;
            if (!this._propertyCache.TryGetValue(name, out property))
            {
                property = this._properties.FirstOrDefault(p => p.Name == name);
                if (property != null)
                {
                    this._propertyCache.Add(name, property);
                }
            }
            return property;
        }

        /// <summary>
        /// 判断当前实体数据模型类型中是否存在指定名称的属性。
        /// </summary>
        /// <param name="name">指定的属性名称。</param>
        /// <returns>如果当前实体数据模型类型中存在 <paramref name="name"/> 参数指定的属性，则返回 true；否则返回 false。</returns>
        public virtual bool HasProperty(string name)
        {
            Check.NotEmpty(name);
            return this._properties.Any(p => p.Name == name);
        }


        /// <summary>
        /// 根据指定的属性名称获取该属性的 Get 属性器所表示的委托方法。
        /// </summary>
        /// <param name="name">指定的属性名称。</param>
        /// <returns>返回 <paramref name="name"/> 属性的 Get 属性器所表示的委托方法，类型为 Func(TEntity, TKey)。</returns>
        public Delegate GetKeySelector(string name)
        {
            Delegate keySelector = null;
            if (!this._keySelectorCache.TryGetValue(name, out keySelector))
            {
                PropertyInfo property = this.GetProperty(name);
                if (property == null)
                    throw new ArgumentException("指定名称 {0} 所表示的属性不存在，不能获取该值的选择器。".Format(name as object));

                MethodInfo getMethod = property.GetMethod;
                keySelector = getMethod.CreateDelegate(typeof(Func<>).MakeGenericType(property.PropertyType));
                this._keySelectorCache.Add(name, keySelector);
            }
            return keySelector;
        }

    }
}
