using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Reflection
{
    /// <summary>
    /// 提供一组对 应用程序集的作用范围枚举(<see cref="AssemblyScope"/>) 操作方法的扩展。
    /// </summary>
    public static class AssemblyScopeExtensions
    {
        /// <summary>
        /// 以当前枚举值作为应用程序集作用范围内参数获取已加载至当前应用程序中的程序集 <see cref="Assembly"/> 集合。
        /// </summary>
        /// <param name="_this">表示应用程序集的作用范围枚举的 <see cref="AssemblyScope"/> 枚举值。</param>
        /// <returns>返回以当前枚举值作为应用程序集作用范围内参数获取已加载至当前应用程序中的程序集 <see cref="Assembly"/> 集合所构成的一个数组。</returns>
        public static Assembly[] GetAssemblies(this AssemblyScope _this)
        {
            return Assemblies.GetAssemblies(_this);
        }


        /// <summary>
        /// 以当前枚举值作为应用程序集作用范围内参数获取已加载至当前应用程序中的程序集中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <param name="_this">表示应用程序集的作用范围枚举的 <see cref="AssemblyScope"/> 枚举值。</param>
        /// <returns>以当前枚举值作为应用程序集作用范围内参数获取已加载至当前应用程序中的程序集中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetTypes(AssemblyScope _this)
        {
            return Assemblies.GetTypes(_this);
        }


        /// <summary>
        /// 以当前枚举值作为应用程序集作用范围内参数获取已加载至当前应用程序中的程序集中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <param name="_this">表示应用程序集的作用范围枚举的 <see cref="AssemblyScope"/> 枚举值。</param>
        /// <returns>以当前枚举值作为应用程序集作用范围内参数获取已加载至当前应用程序中的程序集中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetPublicTypes(AssemblyScope _this)
        {
            return Assemblies.GetPublicTypes(_this);
        }


        /// <summary>
        /// 以当前枚举值作为应用程序集作用范围内参数获取已加载至当前应用程序中的程序集中定义的所有类型 <see cref="System.Type"/> 的命名空间所构成的一个数组。
        /// </summary>
        /// <param name="_this">表示应用程序集的作用范围枚举的 <see cref="AssemblyScope"/> 枚举值。</param>
        /// <returns>以当前枚举值作为应用程序集作用范围内参数获取已加载至当前应用程序中的程序集中定义的所有类型 <see cref="System.Type"/> 的命名空间所构成的一个数组。</returns>
        public static string[] GetNamespaces(AssemblyScope _this)
        {
            return Assemblies.GetNamespaces(_this);
        }
    }
}
