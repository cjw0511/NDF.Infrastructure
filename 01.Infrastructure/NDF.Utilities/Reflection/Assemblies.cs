using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Reflection
{
    /// <summary>
    /// 提供一组对 程序集 <see cref="System.Reflection.Assembly"/> 的工具操作方法。
    /// </summary>
    public static class Assemblies
    {
        /// <summary>
        /// 获取已加载至当前应用程序中整个应用程序域下所有已经加载的程序集 <see cref="Assembly"/> 集合。
        /// </summary>
        /// <returns>已加载至当前应用程序中整个应用程序域下所有已经加载的程序集 <see cref="Assembly"/> 集合所构成的一个数组。</returns>
        public static Assembly[] GetAssemblies()
        {
            return GetAssemblies(AssemblyScope.Global);
        }

        /// <summary>
        /// 获取已加载至当前应用程序中指定程序集作用范围内的程序集 <see cref="Assembly"/> 集合。
        /// </summary>
        /// <param name="scope">一个 <see cref="AssemblyScope"/> 枚举值，表示指定的程序集作用范围。</param>
        /// <returns>已加载至当前应用程序中指定程序集作用范围内的程序集 <see cref="Assembly"/> 集合所构成的一个数组。</returns>
        public static Assembly[] GetAssemblies(AssemblyScope scope)
        {
            List<Assembly> list = new List<Assembly>();
            if (scope.HasFlag(AssemblyScope.Calling))
            {
                list.Add(Assembly.GetCallingAssembly());
            }
            if (scope.HasFlag(AssemblyScope.Entry))
            {
                list.Add(Assembly.GetEntryAssembly());
            }
            if (scope.HasFlag(AssemblyScope.Executing))
            {
                list.Add(Assembly.GetExecutingAssembly());
            }
            if (scope.HasFlag(AssemblyScope.Global))
            {
                list.AddRange(AppDomain.CurrentDomain.GetAssemblies());
            }
            return list.Distinct().ToArray();
        }



        /// <summary>
        /// 获取已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <returns>已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetTypes()
        {
            return GetTypes(AssemblyScope.Global);
        }

        /// <summary>
        /// 获取已加载至当前应用程序中指定程序集作用范围内的程序集中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <param name="scope">一个 <see cref="AssemblyScope"/> 枚举值，表示指定的程序集作用范围。</param>
        /// <returns>已加载至当前应用程序中指定程序集作用范围内的程序集中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetTypes(AssemblyScope scope)
        {
            return GetAssemblies(scope).SelectMany(assembly => GetTypes(assembly)).Distinct().ToArray();
        }

        /// <summary>
        /// 获取程序集中定义的类型。
        /// 同 <seealso cref="Assembly.GetTypes"/>；但在 <seealso cref="Assembly.GetTypes"/> 基础上屏蔽了 <see cref="System.Reflection.ReflectionTypeLoadException"/> 异常。
        /// </summary>
        /// <param name="assembly">应用程序集。</param>
        /// <returns>一个数组，包含此程序集中定义的所有类型。</returns>
        public static Type[] GetTypes(Assembly assembly)
        {
            return assembly.GetTypes();
        }



        /// <summary>
        /// 获取已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <returns>已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetPublicTypes()
        {
            return GetPublicTypes(AssemblyScope.Global);
        }

        /// <summary>
        /// 获取已加载至当前应用程序中指定程序集作用范围内的程序集中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <param name="scope">一个 <see cref="AssemblyScope"/> 枚举值，表示指定的程序集作用范围。</param>
        /// <returns>已加载至当前应用程序中指定程序集作用范围内的程序集中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetPublicTypes(AssemblyScope scope)
        {
            return GetAssemblies(scope).SelectMany(assembly => assembly.ExportedTypes).Distinct().ToArray();
        }

        /// <summary>
        /// 获取此程序集中定义的公共类型的集合，这些公共类型在程序集外可见。
        /// 同 <seealso cref="Assembly.ExportedTypes"/>；但在 <seealso cref="Assembly.ExportedTypes"/> 基础上屏蔽了 <see cref="System.Reflection.ReflectionTypeLoadException"/> 异常。
        /// </summary>
        /// <param name="assembly">应用程序集。</param>
        /// <returns>此程序集中定义的公共类型的集合，这些公共类型在程序集外可见。</returns>
        public static Type[] GetPublicTypes(Assembly assembly)
        {
            return assembly.ExportedTypes.ToArray();
        }




        /// <summary>
        /// 获取已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中定义的所有类型 <see cref="System.Type"/> 的命名空间所构成的一个数组。
        /// </summary>
        /// <returns>已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中定义的所有类型 <see cref="System.Type"/> 的命名空间所构成的一个数组。</returns>
        public static string[] GetNamespaces()
        {
            return GetNamespaces(AssemblyScope.Global);
        }

        /// <summary>
        /// 获取已加载至当前应用程序中指定程序集作用范围内的程序集中定义的所有类型 <see cref="System.Type"/> 的命名空间所构成的一个数组。
        /// </summary>
        /// <param name="scope">一个 <see cref="AssemblyScope"/> 枚举值，表示指定的程序集作用范围。</param>
        /// <returns>已加载至当前应用程序中指定程序集作用范围内的程序集中定义的所有类型 <see cref="System.Type"/> 的命名空间所构成的一个数组。</returns>
        public static string[] GetNamespaces(AssemblyScope scope)
        {
            return GetAssemblies(scope).SelectMany(assembly => GetNamespaces(assembly)).Distinct().ToArray();
        }

        /// <summary>
        /// 获取当前应用程序集中已定义的所有类型 <see cref="System.Type"/> 的命名空间所构成的一个数组。
        /// </summary>
        /// <param name="assembly">应用程序集。</param>
        /// <returns>当前应用程序集中已定义的所有类型 <see cref="System.Type"/> 的命名空间所构成的一个数组。</returns>
        public static string[] GetNamespaces(Assembly assembly)
        {
            return GetTypes(assembly).Select(t => t.Namespace).Distinct().ToArray();
        }
    }
}
