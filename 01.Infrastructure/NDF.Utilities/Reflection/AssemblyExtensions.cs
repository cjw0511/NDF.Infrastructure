using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Reflection
{
    /// <summary>
    /// 提供一组对 程序集 <see cref="System.Reflection.Assembly"/> 操作方法的扩展。
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// 获取当前应用程序集中已定义的所有类型 <see cref="System.Type"/> 的命名空间所构成的一个数组。
        /// </summary>
        /// <param name="_this">应用程序集。</param>
        /// <returns>当前应用程序集中已定义的所有类型 <see cref="System.Type"/> 的命名空间所构成的一个数组。</returns>
        public static string[] GetNamespaces(Assembly _this)
        {
            return Assemblies.GetNamespaces(_this);
        }
    }
}
