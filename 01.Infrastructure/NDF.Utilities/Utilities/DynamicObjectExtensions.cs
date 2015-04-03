using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对运行时动态行为数据对象 <see cref="System.Dynamic.DynamicObject" /> 操作方法的扩展。
    /// </summary>
    public static class DynamicObjectExtensions
    {
        /// <summary>
        /// 获取当前 <see cref="System.Dynamic.DynamicObject"/> 对象的所有动态属性。
        /// </summary>
        /// <param name="_this">一个 <see cref="System.Dynamic.DynamicObject"/> 类型对象。</param>
        /// <returns>当前 <see cref="System.Dynamic.DynamicObject"/> 对象的所有动态属性。</returns>
        public static IEnumerable<string> GetProperties(this DynamicObject _this)
        {
            return _this.GetDynamicMemberNames();
        }
    }
}
