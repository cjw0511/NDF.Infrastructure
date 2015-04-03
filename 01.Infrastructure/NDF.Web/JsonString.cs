using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NDF.Web
{
    /// <summary>
    /// 表示不应再次进行 JSON 解析的 JSON 字符串。
    /// </summary>
    public class JsonString : IJsonString
    {
        private static readonly JsonString _empty = Create(string.Empty);
        private static readonly string[] _nullableToken = new string[] { "null", "undefined" };
        private readonly string _value;


        /// <summary>
        /// 以指定的字符串内容作为 JSON 对象文本内容初始化 <see cref="JsonString"/> 类型实例。
        /// </summary>
        /// <param name="value">作为 JSON 对象文本内容的字符串。</param>
        public JsonString(string value)
        {
            _value = value ?? string.Empty;
        }


        /// <summary>
        /// 表示一个空 JSON 字符串值。这是一个只读属性。
        /// </summary>
        public static JsonString Empty
        {
            get { return _empty; }
        }

        /// <summary>
        /// 表示 JSON 空值对象的标记。这是一个只读属性。
        /// </summary>
        public static string[] NullableToken
        {
            get { return _nullableToken; }
        }




        /// <summary>
        /// 返回 JSON 解析的字符串。
        /// </summary>
        /// <returns>经过 JSON 格式解析的字符串。</returns>
        public string ToJsonString()
        {
            return IsNullOrEmpty(this) ? "null" : HttpUtility.HtmlDecode(_value);
        }

        /// <summary>
        /// 返回表示当前对象的字符串。
        /// </summary>
        /// <returns>返回表示当前对象的字符串。</returns>
        public override string ToString()
        {
            return this.ToJsonString();
        }



        /// <summary>
        /// 使用指定的文本内容创建 JSON 解析字符串。
        /// </summary>
        /// <param name="value">要创建的字符串的值。</param>
        /// <returns>返回一个新创建的 <see cref="JsonString"/> 对象，该对象表示的字符串为传入的 value 参数值。</returns>
        public static JsonString Create(string value)
        {
            return new JsonString(value);
        }


        /// <summary>
        /// 确定指定的字符串包含内容还是为 null、空字符串、"null" 或 "undefined"。
        /// </summary>
        /// <param name="value">字符串。</param>
        /// <returns>如果该字符串为 null 或为空或这为 "null" 或 "undefined" 字符，则为 true；否则为 false。</returns>
        public static bool IsNullOrEmpty(JsonString value)
        {
            return value == null || string.IsNullOrEmpty(value._value) || NullableToken.Contains(value._value);
        }
    }
}
