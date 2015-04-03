using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 字符串键值对 对象 <see cref="NameValueCollection"/> 操作方法的扩展。
    /// </summary>
    public static class NameValueCollectionExtensions
    {

        /// <summary>
        /// 将一个 字符串键值对 <see cref="NameValueCollection"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string ToJson(this NameValueCollection _this)
        {
            dynamic item = ToObject(_this);
            return Serialization.ToJson(item);
        }

        /// <summary>
        /// 将一个 字符串键值对 <see cref="NameValueCollection"/> 对象 转换为一个动态解析对象。
        /// 动态解析对象中的各个属性和值对应于 字符串键值对 中的每个数据项。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static dynamic ToObject(this NameValueCollection _this)
        {
            dynamic item = new DynamicObject();
            foreach (string key in _this.AllKeys)
            {
                item[key] = _this[key];
            }
            return item;
        }


        /// <summary>
        /// 将一个 字符串键值对 <see cref="NameValueCollection"/> 对象 转换为一个 Dictionary&lt;string, string&gt; 类型的键值对对象。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this NameValueCollection _this)
        {
            Check.NotNull(_this);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string key in _this.AllKeys)
            {
                dict.Add(key, _this[key]);
            }
            return dict;
        }

    }
}
