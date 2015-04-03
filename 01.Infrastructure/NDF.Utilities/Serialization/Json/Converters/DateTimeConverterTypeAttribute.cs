using NDF.Utilities;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDF.Serialization.Json.Converters
{
    /// <summary>
    /// 表示日期 JSON 序列化格式转换类型。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DateTimeConverterTypeAttribute : Attribute
    {
        private Type _converterType;

        /// <summary>
        /// 初始化 <see cref="DateTimeConverterTypeAttribute"/> 类型的新实例。
        /// </summary>
        public DateTimeConverterTypeAttribute() : this(DefaultConverterType)
        {
        }

        /// <summary>
        /// 以指定的 日期 JSON 序列化格式转换类型初始化 <see cref="DateTimeConverterTypeAttribute"/> 类型的新实例。
        /// </summary>
        /// <param name="dateTimeConverterType">
        /// 表示日期 JSON 序列化格式转换类型。
        ///     该参数的值必须是表示类型 <see cref="Newtonsoft.Json.Converters.DateTimeConverterBase"/> 的子类，否则将抛出异常 <see cref="System.ArgumentNullException"/>。
        /// </param>
        public DateTimeConverterTypeAttribute(Type dateTimeConverterType)
        {
            this.ConverterType = dateTimeConverterType;
        }


        /// <summary>
        /// 获取或设置该属性表示的日期 JSON 序列化格式转换类型。
        /// 如果对该属性进行赋值操作，新设置的值必须是类型 <see cref="Newtonsoft.Json.Converters.DateTimeConverterBase"/> 的子类，否则将抛出异常 <see cref="System.ArgumentNullException"/>。
        /// </summary>
        public Type ConverterType
        {
            get
            {
                if (this._converterType == null)
                {
                    this._converterType = DefaultConverterType;
                }
                return this._converterType;
            }
            set
            {
                Check.SubclassOf(value, typeof(DateTimeConverterBase));
                this._converterType = value;
            }
        }



        /// <summary>
        /// 在当前属性标记类型对象未指定 <seealso cref="ConverterType"/> 属性时，默认所使用的日期 JSON 序列化格式转换类型。
        /// 该值为 <see cref="NDF.Serialization.Json.Converters.GeneralDateConverter"/> 所表示的类型定义。
        /// </summary>
        public static Type DefaultConverterType
        {
            get { return typeof(GeneralDateConverter); }
        }
    }
}
