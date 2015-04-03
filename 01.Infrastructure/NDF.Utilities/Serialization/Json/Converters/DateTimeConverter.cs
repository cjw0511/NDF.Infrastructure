using NDF.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NDF.Serialization.Json.Converters
{
    /// <summary>
    /// 定义用于将 <see cref="System.DateTime"/> 日期时间对象转换为 JSON 字符串表示法转换器的非抽象基类。
    /// 该类型定义计算机的区域设置中指定的标准日期时间格式。为："yyyy/MM/dd hh:mm:ss.fff"。
    /// </summary>
    /// <remarks>
    /// 可通过继承该类同时重写属性 <seealso cref="DateTimeFormat"/> 来快速实现日期时间对象自定义转换为 JSON 字符串的转换器。
    /// </remarks>
    public class DateTimeConverter : DateTimeConverterBase
    {
        private const string DefaultDateTimeFormat = "yyyy/MM/dd hh:mm:ss.fff";

        private string _dateTimeFormat;
        private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
        private CultureInfo _culture;


        /// <summary>
        /// 初始化 <see cref="NDF.Serialization.Json.Converters.DateTimeConverter"/> 类型的新实例。
        /// </summary>
        public DateTimeConverter() { }

        /// <summary>
        /// 以指定的日期时间格式化字符串作为 <seealso cref="DateTimeFormat"/> 属性值初始化 <see cref="NDF.Serialization.Json.Converters.DateTimeConverter"/> 类型的新实例。
        /// </summary>
        /// <param name="dateTimeFormat">日期时间格式化字符串。</param>
        public DateTimeConverter(string dateTimeFormat)
        {
            this._dateTimeFormat = dateTimeFormat;
        }




        /// <summary>
        /// 定义用于将 <see cref="System.DateTime"/> 日期时间对象转换为字符串的格式。
        /// 该属性默认情况下返回 "yyyy/MM/dd hh:mm:ss.fff"。
        /// </summary>
        /// <remarks>
        /// 具体日期时间转字符串的格式化字符串语法规则参考：http://msdn.microsoft.com/ZH-CN/library/8kb3ddd4(v=VS.110,d=hv.2).aspx。
        /// </remarks>
        public virtual string DateTimeFormat
        {
            get
            {
                return string.IsNullOrWhiteSpace(this._dateTimeFormat) ? DefaultDateTimeFormat : this._dateTimeFormat;
            }
        }

        /// <summary>
        /// 获取或设置一个用于将字符串解析为日期格式的解析选项。
        /// </summary>
        public virtual DateTimeStyles DateTimeStyles
        {
            get { return this._dateTimeStyles; }
            set { this._dateTimeStyles = value; }
        }

        /// <summary>
        /// 获取或设置用于将 <see cref="System.DateTime"/> 日期时间格式数据解析为字符串的区域设置信息。
        /// 该属性默认情况下将返回当前服务器操作系统的区域信息 <seealso cref="CultureInfo.CurrentCulture"/> 设置。
        /// </summary>
        public virtual CultureInfo Culture
        {
            get { return this._culture ?? CultureInfo.CurrentCulture; }
            set { this._culture = value; }
        }




        /// <summary>
        /// 重写基类中的 ReadJson 方法。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            bool nullable = objectType.IsNullable();
            Type t = nullable ? Nullable.GetUnderlyingType(objectType) : objectType;

            if (reader.TokenType == JsonToken.Null)
            {
                if (!nullable)
                    throw new JsonSerializationException("不能转换空值 {0}.".Format(this.Culture, objectType));
                return null;
            }
            if (reader.TokenType == JsonToken.Date)
            {
                if (t == typeof(DateTimeOffset))
                    return reader.Value is DateTimeOffset ? reader.Value : new DateTimeOffset((DateTime)reader.Value);

                return reader.Value;
            }
            if (reader.TokenType != JsonToken.String)
                throw new JsonSerializationException("将对象转换成日期时传入了一个非预期的数据类型值，预期的数据类型为 System.String，而传入的是 {0}.".Format(CultureInfo.InvariantCulture, reader.TokenType));

            string dateText = reader.Value.ToString();

            if (string.IsNullOrEmpty(dateText) && nullable)
                return null;

            if (t == typeof(DateTimeOffset))
            {
                if (!string.IsNullOrEmpty(this.DateTimeFormat))
                    return DateTimeOffset.ParseExact(dateText, this.DateTimeFormat, this.Culture, this.DateTimeStyles);
                else
                    return DateTimeOffset.Parse(dateText, this.Culture, this.DateTimeStyles);
            }

            if (!string.IsNullOrEmpty(this.DateTimeFormat))
                return DateTime.ParseExact(dateText, this.DateTimeFormat, this.Culture, this.DateTimeStyles);
            else
                return DateTime.Parse(dateText, this.Culture, this.DateTimeStyles);
        }

        /// <summary>
        /// 重写基类中的 WriteJson 方法。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            string text;
            if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;
                if ((this.DateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (this.DateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                    dateTime = dateTime.ToUniversalTime();
                text = this.ToString(dateTime);
            }
            else if (value is DateTimeOffset)
            {
                DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
                if ((this.DateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (this.DateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                    dateTimeOffset = dateTimeOffset.ToUniversalTime();
                text = this.ToString(dateTimeOffset);
            }
            else
            {
                throw new JsonSerializationException(string.Format("进行类型转换时设置了一个非预期数据类型：{0}，允许的数据类型 System.DateTime 或 System.DateTimeOffset。", value != null ? value.GetType() : null));
            }
            writer.WriteValue(text);
        }


        /// <summary>
        /// 定义将时间对象 <see cref="System.DateTime"/> 转换为字符串的方法。
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        protected virtual string ToString(DateTime datetime)
        {
            return datetime.ToString(this.DateTimeFormat, this.Culture);
        }

        /// <summary>
        /// 定义将时间点对象 <see cref="System.DateTimeOffset"/> 转换为字符串的方法。
        /// </summary>
        /// <param name="datetimeOffset"></param>
        /// <returns></returns>
        protected virtual string ToString(DateTimeOffset datetimeOffset)
        {
            return datetimeOffset.ToString(this.DateTimeFormat, this.Culture);
        }

    }
}
