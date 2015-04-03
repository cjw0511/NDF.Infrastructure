using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Reflection
{
    /// <summary>
    /// 表示元数据（类型、属性、字段、方法、事件、委托等）的附加描述性信息。
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class MemberInfoDescriptionAttribute : Attribute
    {
        /// <summary>
        /// 初始化 <see cref="MemberInfoDescriptionAttribute"/> 类的新实例。
        /// </summary>
        public MemberInfoDescriptionAttribute() { }

        /// <summary>
        /// 用指定的 <paramref name="text"/> 参数初始化 <see cref="MemberInfoDescriptionAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="text">一个字符串，用于表示 <see cref="MemberInfoDescriptionAttribute"/> 类的新实例的 <seealso cref="Text"/> 属性值。</param>
        public MemberInfoDescriptionAttribute(string text) : this()
        {
            this.Text = text;
        }

        /// <summary>
        /// 用指定的 <paramref name="value"/> 参数初始化 <see cref="MemberInfoDescriptionAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="value">一个 <see cref="System.Object"/> 对象值，用于表示 <see cref="MemberInfoDescriptionAttribute"/> 类的新实例的 <seealso cref="Value"/> 属性值。</param>
        public MemberInfoDescriptionAttribute(object value) : this()
        {
            this.Value = value;
        }

        /// <summary>
        /// 用指定的 <paramref name="text"/> 和 <paramref name="value"/> 参数初始化 <see cref="MemberInfoDescriptionAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="text">一个字符串，用于表示 <see cref="MemberInfoDescriptionAttribute"/> 类的新实例的 <seealso cref="Text"/> 属性值。</param>
        /// <param name="value">一个 <see cref="System.Object"/> 对象值，用于表示 <see cref="MemberInfoDescriptionAttribute"/> 类的新实例的 <seealso cref="Value"/> 属性值。</param>
        public MemberInfoDescriptionAttribute(string text, object value) : this(text)
        {
            this.Value = value;
        }



        /// <summary>
        /// 获取或设置一个字符串，该字符串表示元数据的描述性文本内容。
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 获取或设置一个 <see cref="System.Object"/> 值，该值表示元数据的描述性值内容。
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 获取或设置一个字符串，该字符串表示元数据的附加描述性文本内容。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置一个字符串，该字符串表示元数据的描述性备注信息。
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 获取或设置一个字符串，该字符串表示元数据的描述性简介信息。
        /// </summary>
        public string Summary { get; set; }

    }
}
