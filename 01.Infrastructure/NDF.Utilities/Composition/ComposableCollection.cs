using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Composition
{
    /// <summary>
    /// 表示可自定用 MEF 组件进行内部的泛型集合 <see cref="System.Collections.ObjectModel.Collection&lt;T&gt;"/> 类型属性进行解析的集合定义。
    /// </summary>
    public class ComposableCollection
    {

        /// <summary>
        /// 初始化 <see cref="ComposableCollection"/> 类型的新实例。
        /// </summary>
        protected ComposableCollection()
        {
            Compositions.ComposeParts(this);
        }


    }
}
