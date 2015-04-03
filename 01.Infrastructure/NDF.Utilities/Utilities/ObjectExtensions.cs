using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 基础对象类型 <see cref="System.Object"/> 操作方法的扩展。
    /// </summary>
    public static partial class ObjectExtensions
    {

        /// <summary>
        /// 判断指定的对象是否为 Null 或者等于 DBNull.Value 值。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool IsNull(this Object _this)
        {
            return _this == null || Convert.IsDBNull(_this);
        }


        /// <summary>
        /// 将当前对象转换成一个包含键值对集合的 <see cref="NameValueCollection"/> 对象。键值对中的 Key 值表示对象中的属性名，Value 值表示该属性名所对应的值。
        /// 当前对象中所有具有 get 属性器的公共属性名及属性值都将包含在返回的键值对枚举列表中。
        /// </summary>
        /// <param name="_this">要被转换的对象。</param>
        /// <returns>返回一个 <see cref="NameValueCollection"/>。该对象键值对中的 Key 值表示对象中的属性名，Value 值表示该属性名所对应的值。</returns>
        public static NameValueCollection ToNameValueCollection(this Object _this)
        {
            Check.NotNull(_this);

            Dictionary<string, object> dictionary = _this.ToDictionary();
            NameValueCollection collection = new NameValueCollection();
            foreach (var item in dictionary)
            {
                collection.Add(item.Key, Convert.ToString(item.Value));
            }
            return collection;
        }


        /// <summary>
        /// 将当前对象转换成一个包含键值对集合的 Dictionary 对象。键值对中的 Key 值表示对象中的属性名，Value 值表示该属性名所对应的值。
        /// 当前对象中所有具有 get 属性器的公共属性名及属性值都将包含在返回的键值对枚举列表中。
        /// </summary>
        /// <param name="_this">要被转换的对象。</param>
        /// <returns>返回一个 Dictionary。该对象键值对中的 Key 值表示对象中的属性名，Value 值表示该属性名所对应的值。</returns>
        public static Dictionary<string, object> ToDictionary(this object _this)
        {
            Check.NotNull(_this);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            var properies = _this.GetType().GetProperties().Where(p => p.GetMethod != null);
            foreach (PropertyInfo property in properies)
            {
                dictionary.Add(property.Name, property.GetValue(_this));
            }
            return dictionary;
        }

        /// <summary>
        /// 将当前对象转换成一个包含键值对集合的 Dictionary 对象。键值对中的 Key 值表示对象中的属性名，Value 值表示该属性名所对应的值。
        /// 当前对象中所有指定作用域范围的属性名及属性值都将包含在返回的键值对枚举列表中。
        /// </summary>
        /// <param name="_this">要被转换的对象。</param>
        /// <param name="bindingAttr">指定搜索 <paramref name="_this"/> 中属性的反射搜索范围。</param>
        /// <returns>返回一个 Dictionary。该对象键值对中的 Key 值表示对象中的属性名，Value 值表示该属性名所对应的值。</returns>
        public static Dictionary<string, object> ToDictionary(this object _this, BindingFlags bindingAttr)
        {
            Check.NotNull(_this);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            PropertyInfo[] properies = _this.GetType().GetProperties(bindingAttr);
            foreach (PropertyInfo property in properies)
            {
                dictionary.Add(property.Name, property.GetValue(_this));
            }
            return dictionary;
        }


        /// <summary>
        /// 将当前对象中所有属性的值按照属性名称和类型定义的匹配关系复制到另一对象中。
        /// </summary>
        /// <param name="_this">
        /// 表示将要用于复制数据到另一对象的元数据对象。
        /// 如果该参数值为 Null，将不会执行复制操作。
        /// </param>
        /// <param name="element">表示一个目标对象，该对象中的相应属性值将会被更改。</param>
        /// <param name="throwError">一个布尔类型值，该值指示在复制数据过程中如果出现异常，是否立即停止并抛出异常，默认为 false。</param>
        public static void CopyTo(this object _this, object element, bool throwError = false)
        {
            if (_this == null)
                return;

            Check.NotNull(element);
            Type thisType = _this.GetType(), elemType = element.GetType();

            var thisProps = thisType.GetProperties().Where(p => p.GetMethod != null);
            var elemProps = elemType.GetProperties().Where(p => p.SetMethod != null);
            foreach (PropertyInfo thisProperty in thisProps)
            {
                PropertyInfo elemProperty = elemProps.FirstOrDefault(p => p.Name == thisProperty.Name);
                if (elemProperty != null && elemProperty.PropertyType.IsAssignableFrom(thisProperty.PropertyType))
                {
                    if (throwError)
                        elemProperty.SetValue(element, thisProperty.GetValue(_this));
                    else
                        Utility.TryCatchExecute(() => elemProperty.SetValue(element, thisProperty.GetValue(_this)));
                }
            }

            var thisFields = thisType.GetFields();
            var elemFields = elemType.GetFields();
            foreach (FieldInfo thisField in thisFields)
            {
                FieldInfo elemField = elemFields.FirstOrDefault(f => f.Name == thisField.Name);
                if (elemField != null && elemField.FieldType.IsAssignableFrom(thisField.FieldType))
                {
                    if (throwError)
                        elemField.SetValue(element, thisField.GetValue(_this));
                    else
                        Utility.TryCatchExecute(() => elemField.SetValue(element, thisField.GetValue(_this)));
                }
            }
        }


        /// <summary>
        /// 将当前对象转换成一个指定类型的新对象。
        /// 该方法首先判断 <paramref name="_this"/> 是否为 <paramref name="resultType"/> 指定的类型，如果是，则直接返回 <paramref name="_this"/>；
        /// 否则将通过 Activator.CreateInstance(resultType) 方法创建一个 <paramref name="resultType"/> 类型的对象，然后查找出该类型对象有哪些公共属性值，将 <paramref name="_this"/> 对象的对应属性值复制到新创建的对象中。
        /// </summary>
        /// <param name="_this">要转换的对象。</param>
        /// <param name="resultType">要转换的目标实体类型。</param>
        /// <param name="throwError">一个布尔类型值，该值表示在复制数据过程中如果出现异常，是否立即停止并抛出异常，默认为 false。</param>
        /// <returns>
        /// 该方法首先判断 <paramref name="_this"/> 是否为 <paramref name="resultType"/> 指定的类型，如果是，则直接返回 <paramref name="_this"/>；
        /// 否则将通过 Activator.CreateInstance(resultType) 方法创建一个 <paramref name="resultType"/> 类型的对象，然后查找出该类型对象有哪些公共属性值，将 <paramref name="_this"/> 对象的对应属性值复制到新创建的对象中。
        /// </returns>
        public static object CastTo(this object _this, Type resultType, bool throwError = false)
        {
            Check.NotNull(_this);
            Check.NotNull(resultType);
            if (resultType.IsInstanceOfType(_this))
            {
                return _this;
            }
            object ret = Activator.CreateInstance(resultType);
            _this.CopyTo(ret, throwError);
            return ret;
        }

        /// <summary>
        /// 将当前对象转换成一个指定类型的新对象。
        /// 该方法首先判断 <paramref name="_this"/> 是否为 <typeparamref name="TResult"/> 指定的类型，如果是，则直接返回 <paramref name="_this"/>；
        /// 否则将通过 Activator.CreateInstance(resultType) 方法创建一个 <typeparamref name="TResult"/> 类型的对象，然后查找出该类型对象有哪些公共属性值，将 <paramref name="_this"/> 对象的对应属性值复制到新创建的对象中。
        /// </summary>
        /// <typeparam name="TResult">要转换的目标实体类型。</typeparam>
        /// <param name="_this">要转换的对象。</param>
        /// <param name="throwError">一个布尔类型值，该值表示在复制数据过程中如果出现异常，是否立即停止并抛出异常，默认为 false。</param>
        /// <returns>
        /// 该方法首先判断 <paramref name="_this"/> 是否为 <typeparamref name="TResult"/> 指定的类型，如果是，则直接返回 <paramref name="_this"/>；
        /// 否则将通过 Activator.CreateInstance(resultType) 方法创建一个 <typeparamref name="TResult"/> 类型的对象，然后查找出该类型对象有哪些公共属性值，将 <paramref name="_this"/> 对象的对应属性值复制到新创建的对象中。
        /// </returns>
        public static TResult CastTo<TResult>(this object _this, bool throwError = false)
        {
            if (_this is TResult)
            {
                return (TResult)_this;
            }
            Check.NotNull(_this);
            TResult ret = (TResult)Activator.CreateInstance(typeof(TResult));
            _this.CopyTo(ret, throwError);
            return ret;
        }



        /// <summary>
        /// 创建一个当前对象的新副本，该副本和当前对象为同一类型，且其中各个属性的值均等同于原对象中各个属性的值。
        /// 相当于浅表复制操作 Object.MemberwiseClone，但和 Object.MemberwiseClone 不同的是该操作只对公共 Public 的可 set 属性进行复制，并且不会复制其他字段或私有成员的值。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public static object Duplicate(this object _this, bool throwError = false)
        {
            if (_this == null)
                return null;

            Type thisType = _this.GetType();
            if (thisType.IsValueType)
                return _this;

            if (_this is ICloneable)
                return ((ICloneable)_this).Clone();

            Object obj = Activator.CreateInstance(thisType);
            _this.CopyTo(obj, throwError);
            return obj;
        }

        /// <summary>
        /// 创建一个当前对象的新副本，该副本和当前对象为同一类型，且其中各个属性的值均等同于原对象中各个属性的值。
        /// 相当于浅表复制操作 Object.MemberwiseClone，但和 Object.MemberwiseClone 不同的是该操作只对公共 Public 的可 set 属性进行复制，并且不会复制其他字段或私有成员的值。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="_this"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public static TSource Duplicate<TSource>(this TSource _this, bool throwError = false) where TSource : class, new()
        {
            if (_this == null)
                return null;

            Type thisType = typeof(TSource);
            if (thisType.IsAbstract)
            {
                Object obj = Duplicate(_this as object, throwError);
                return obj == null ? null : obj as TSource;
            }

            if (thisType.IsValueType)
                return _this;

            if (_this is ICloneable)
                return ((ICloneable)_this).Clone() as TSource;

            TSource target = new TSource();
            _this.CopyTo(target, throwError);
            return target;
        }

        /// <summary>
        /// 创建当前 Object 的浅表副本。同 Object.MemberwiseClone 方法。
        /// 该方法创建一个浅表副本，方法是创建一个新对象，然后将当前对象的非静态字段复制到该新对象。
        /// 如果字段是值类型的，则对该字段执行逐位复制。 如果字段是引用类型，则复制引用但不复制引用的对象；因此，原始对象及其复本引用同一对象。
        /// </summary>
        /// <param name="_this">要执行复制操作的对象。</param>
        /// <returns></returns>
        public static TSource Clone<TSource>(this TSource _this)
        {
            if (_this == null)
                return default(TSource);

            if (_this.GetType().IsValueType)
                return _this;

            return (TSource)MemberwiseCloneMethod.Invoke(_this, null);
        }




        #region Internal CodeBlock

        private static MethodInfo _memberwiseClone;

        internal static MethodInfo MemberwiseCloneMethod
        {
            get
            {
                if (_memberwiseClone == null)
                    _memberwiseClone = typeof(Object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
                return _memberwiseClone;
            }
        }

        #endregion


    }
}
