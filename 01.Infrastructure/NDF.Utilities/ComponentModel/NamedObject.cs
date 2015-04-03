using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.ComponentModel
{
    /// <summary>
    /// 表示一个由 <see cref="System.String"/> 和 <see cref="System.Object"/> 值组合而成的键值对。
    /// 用于表示类似于 Dictionary&lt;string, object&gt; 集合中单个项的数据结构，即类似于 KeyValuePair&lt;string, object&gt; 结构。
    /// </summary>
    public class NamedObject
    {

        /// <summary>
        /// 初始化类型 <see cref="NamedObject"/> 的新实例。
        /// </summary>
        public NamedObject() { }

        /// <summary>
        /// 以指定的名称初始化类型 <see cref="NamedObject"/> 的新实例。
        /// </summary>
        /// <param name="name">指定的名称。</param>
        public NamedObject(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 以指定的名称和值初始化类型 <see cref="NamedObject"/> 的新实例。
        /// </summary>
        /// <param name="name">指定的名称。</param>
        /// <param name="value">与 Name 关联的值定义。</param>
        public NamedObject(string name, object value) : this(name)
        {
            this.Value = value;
        }



        /// <summary>
        /// 获取 名称/值 中的 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取 名称/值 中的 值。
        /// </summary>
        public object Value { get; set; }



        /// <summary>
        /// 将该 名称/值 对象转换成一个 KeyValuePair&lt;string, object&gt; 对象。
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, object> ToPair()
        {
            return new KeyValuePair<string, object>(this.Name, this.Value);
        }


        /// <summary>
        /// 使用名称和值的字符串表示形式返回 <see cref="NDF.ComponentModel.NamedObject"/> 的字符串表示形式。
        /// </summary>
        /// <returns><see cref="NDF.ComponentModel.NamedObject"/> 的字符串表示形式，它包括名称和值的字符串表示形式。</returns>
        public override string ToString()
        {
            return this.ToPair().ToString();
        }


    }
}
