using NDF.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 类型 <see cref="System.Type"/> 的工具操作方法。
    /// </summary>
    public static class Types
    {
        private static BindingFlags _bindingFlagsAll = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// 指定控制绑定和由反射执行的成员和类型搜索方法标志的所有搜索范围。
        /// 包含
        ///     公共成员 <seealso cref="BindingFlags.Public"/>、
        ///     非公共成员 <seealso cref="BindingFlags.NonPublic"/>、
        ///     实例成员 <seealso cref="BindingFlags.Instance"/>、
        ///     静态成员 <seealso cref="BindingFlags.Static"/>。
        /// </summary>
        public static BindingFlags BindingFlagsAll
        {
            get { return _bindingFlagsAll; }
        }


        /// <summary>
        /// 获取指定对象的 类型<see cref="System.Type"/>。
        /// </summary>
        /// <param name="value">要获取类型的对象。</param>
        /// <returns><paramref name="value"/> 对象的准确运行时类型 <see cref="System.Type"/>。</returns>
        public static Type GetType(object value)
        {
            Check.NotNull(value);
            return value.GetType();
        }



        /// <summary>
        /// 将一个结构（struct）类型定义转换成该结构的可空泛型类型 <see cref="System.Nullable&lt;T&gt;"/> 定义。
        /// </summary>
        /// <param name="_this">一个 <see cref="System.Type"/> 类型对象，必须是一个 struct 定义。</param>
        /// <returns>返回 <paramref name="_this"/> 转换后的可空泛型类型 <see cref="System.Nullable&lt;T&gt;"/> 定义。</returns>
        public static Type ToNullableType(this Type _this)
        {
            Check.IsValueType(_this);
            Type nullable = typeof(Nullable<>);
            return nullable.MakeGenericType(_this);
        }

        /// <summary>
        /// 将一个结构（struct）类型定义转换成该结构的可空泛型类型 <see cref="System.Nullable&lt;T&gt;"/> 定义。
        /// </summary>
        /// <typeparam name="TSource">指定的结构（struct）类型定义。</typeparam>
        /// <returns>返回 <typeparamref name="TSource"/> 转换后的可空泛型类型 <see cref="System.Nullable&lt;T&gt;"/> 定义。</returns>
        public static Type ToNullableType<TSource>() where TSource : struct
        {
            Type nullable = typeof(Nullable<>);
            return nullable.MakeGenericType(typeof(TSource));
        }



        /// <summary>
        /// 尝试获取具有指定名称的 System.Type，执行区分大小写的搜索。
        /// </summary>
        /// <param name="typeName">要获取的类型的程序集限定名称。 请参见 System.Type.AssemblyQualifiedName。 如果该类型位于当前正在执行的程序集中或者 Mscorlib.dll 中，则提供由命名空间限定的类型名称就足够了。</param>
        /// <param name="result">当此方法返回值时，如果找到该类型，便会返回获取到的类型；否则返回 null。</param>
        /// <returns>如果找到具有指定名称的类型，则返回 true，否则返回 false。</returns>
        public static bool TryGetType(string typeName, out Type result)
        {
            try
            {
                result = Type.GetType(typeName);
            }
            catch
            {
                result = null;
            }
            return result != null;
        }

        /// <summary>
        /// 尝试获取具有指定名称的 System.Type，同时执行大小写敏感的搜索，并指定是否要在没有找到该类型时引发异常。
        /// </summary>
        /// <param name="typeName">要获取的类型的程序集限定名称。 请参见 System.Type.AssemblyQualifiedName。 如果该类型位于当前正在执行的程序集中或者 Mscorlib.dll 中，则提供由命名空间限定的类型名称就足够了。</param>
        /// <param name="throwOnError">如果为 true，则在找不到该类型时引发异常；如果为 false，则返回 null。 指定 false 还会取消某些其他异常条件，但并不取消所有条件。</param>
        /// <param name="result">当此方法返回值时，如果找到该类型，便会返回获取到的类型；否则返回 null。</param>
        /// <returns>如果找到与查找条件相匹配的的类型，则返回 true，否则返回 false。</returns>
        public static bool TryGetType(string typeName, bool throwOnError, out Type result)
        {
            try
            {
                result = Type.GetType(typeName, throwOnError);
            }
            catch
            {
                result = null;
            }
            return result != null;
        }

        /// <summary>
        /// 尝试获取具有指定名称的 System.Type，指定是否执行区分大小写的搜索，以及在找不到类型时是否引发异常。
        /// </summary>
        /// <param name="typeName">要获取的类型的程序集限定名称。 请参见 System.Type.AssemblyQualifiedName。 如果该类型位于当前正在执行的程序集中或者 Mscorlib.dll 中，则提供由命名空间限定的类型名称就足够了。</param>
        /// <param name="throwOnError">true 则引发异常（如果找不到类型）；false 则返回null.Specifyingfalse，也抑制了其他一些异常情况，但不是所有异常。</param>
        /// <param name="ignoreCase">为 typeName 执行的搜索不区分大小写则为 true，为 typeName 执行的搜索区分大小写则为 false。</param>
        /// <param name="result">当此方法返回值时，如果找到该类型，便会返回获取到的类型；否则返回 null。</param>
        /// <returns>如果找到与查找条件相匹配的的类型，则返回 true，否则返回 false。</returns>
        public static bool TryGetType(string typeName, bool throwOnError, bool ignoreCase, out Type result)
        {
            try
            {
                result = Type.GetType(typeName, throwOnError, ignoreCase);
            }
            catch
            {
                result = null;
            }
            return result != null;
        }

        /// <summary>
        /// 尝试获取具有指定名称的类型，（可选）提供自定义方法以解析程序集和该类型。
        /// </summary>
        /// <param name="typeName">
        /// 要获取的类型的名称。 如果提供了 typeResolver 参数，则类型名称可以为 typeResolver 能够解析的任何字符串。 如果提供了
        ///     assemblyResolver 参数，或者使用了标准类型解析，则除非该类型位于当前正在执行的程序集或 Mscorlib.dll 中（在这种情况下足以提供其命名空间所限定的类型名称），否则
        ///     typeName 必须为程序集限定的名称（请参见 System.Type.AssemblyQualifiedName）。
        /// </param>
        /// <param name="assemblyResolver">
        /// 一个方法，它定位并返回 typeName 中指定的程序集。 以 System.Reflection.AssemblyName 对象形式传递给 assemblyResolver
        ///     的程序集名称。 如果 typeName 不包含程序集的名称，则不调用 assemblyResolver。 如果未提供 assemblyResolver，则执行标准程序集解析。
        ///     警告 不要通过未知的或不受信任的调用方传递方法。 此操作可能会导致恶意代码特权提升。 仅使用您提供或者熟悉的方法。
        /// </param>
        /// <param name="typeResolver">
        /// 一个方法，它在由 assemblyResolver 或标准程序集解析返回的程序集中定位并返回 typeName 所指定的类型。 如果未提供任何程序集，则
        ///     typeResolver 方法可以提供一个程序集。 该方法还采用一个参数以指定是否执行不区分大小写的搜索；false 传递给该参数。 警告 不要通过未知的或不受信任的调用方传递方法。
        /// </param>
        /// <param name="result">当此方法返回值时，如果找到该类型，便会返回获取到的类型；否则返回 null。</param>
        /// <returns>如果找到与查找条件相匹配的的类型，则返回 true，否则返回 false。</returns>
        public static bool TryGetType(string typeName, Func<AssemblyName, Assembly> assemblyResolver, Func<Assembly, string, bool, Type> typeResolver, out Type result)
        {
            try
            {
                result = Type.GetType(typeName, assemblyResolver, typeResolver);
            }
            catch
            {
                result = null;
            }
            return result != null;
        }

        /// <summary>
        /// 尝试获取具有指定名称的类型，指定在找不到该类型时是否引发异常，（可选）提供自定义方法以解析程序集和该类型。
        /// </summary>
        /// <param name="typeName">
        /// 要获取的类型的名称。 如果提供了 typeResolver 参数，则类型名称可以为 typeResolver 能够解析的任何字符串。 如果提供了
        ///     assemblyResolver 参数，或者使用了标准类型解析，则除非该类型位于当前正在执行的程序集或 Mscorlib.dll 中（在这种情况下足以提供其命名空间所限定的类型名称），否则
        ///     typeName 必须为程序集限定的名称（请参见 System.Type.AssemblyQualifiedName）。
        /// </param>
        /// <param name="assemblyResolver"></param>
        /// <param name="typeResolver">
        /// 一个方法，它定位并返回 typeName 中指定的程序集。 以 System.Reflection.AssemblyName 对象形式传递给 assemblyResolver
        ///     的程序集名称。 如果 typeName 不包含程序集的名称，则不调用 assemblyResolver。 如果未提供 assemblyResolver，则执行标准程序集解析。
        ///     警告 不要通过未知的或不受信任的调用方传递方法。 此操作可能会导致恶意代码特权提升。 仅使用您提供或者熟悉的方法。
        /// </param>
        /// <param name="throwOnError">
        /// 一个方法，它在由 assemblyResolver 或标准程序集解析返回的程序集中定位并返回 typeName 所指定的类型。 如果未提供任何程序集，则该方法可以提供一个程序集。
        ///     该方法还采用一个参数以指定是否执行不区分大小写的搜索；false 传递给该参数。 警告 不要通过未知的或不受信任的调用方传递方法。
        /// </param>
        /// <param name="result">当此方法返回值时，如果找到该类型，便会返回获取到的类型；否则返回 null。</param>
        /// <returns>如果找到与查找条件相匹配的的类型，则返回 true，否则返回 false。</returns>
        public static bool TryGetType(string typeName, Func<AssemblyName, Assembly> assemblyResolver, Func<Assembly, string, bool, Type> typeResolver, bool throwOnError, out Type result)
        {
            try
            {
                result = Type.GetType(typeName, assemblyResolver, typeResolver, throwOnError);
            }
            catch
            {
                result = null;
            }
            return result != null;
        }

        /// <summary>
        /// 尝试获取具有指定名称的类型，指定是否执行区分大小写的搜索，在找不到类型时是否引发异常，（可选）提供自定义方法以解析程序集和该类型。
        /// </summary>
        /// <param name="typeName">
        /// 要获取的类型的名称。 如果提供了 typeResolver 参数，则类型名称可以为 typeResolver 能够解析的任何字符串。 如果提供了
        ///     assemblyResolver 参数，或者使用了标准类型解析，则除非该类型位于当前正在执行的程序集或 Mscorlib.dll 中（在这种情况下足以提供其命名空间所限定的类型名称），否则
        ///     typeName 必须为程序集限定的名称（请参见 System.Type.AssemblyQualifiedName）。
        /// </param>
        /// <param name="assemblyResolver">
        /// 一个方法，它定位并返回 typeName 中指定的程序集。 以 System.Reflection.AssemblyName 对象形式传递给 assemblyResolver
        ///     的程序集名称。 如果 typeName 不包含程序集的名称，则不调用 assemblyResolver。 如果未提供 assemblyResolver，则执行标准程序集解析。
        ///     警告 不要通过未知的或不受信任的调用方传递方法。 此操作可能会导致恶意代码特权提升。 仅使用您提供或者熟悉的方法。
        /// </param>
        /// <param name="typeResolver">
        /// 一个方法，它在由 assemblyResolver 或标准程序集解析返回的程序集中定位并返回 typeName 所指定的类型。 如果未提供任何程序集，则该方法可以提供一个程序集。
        ///     该方法还采用一个参数以指定是否执行不区分大小写的搜索；ignoreCase 的值传递给该参数。 警告 不要通过未知的或不受信任的调用方传递方法。
        /// </param>
        /// <param name="throwOnError">如果为 true，则在找不到该类型时引发异常；如果为 false，则返回 null。 指定 false 还会取消某些其他异常条件，但并不取消所有条件。</param>
        /// <param name="ignoreCase">为 typeName 执行的搜索不区分大小写则为 true，为 typeName 执行的搜索区分大小写则为 false。</param>
        /// <param name="result">当此方法返回值时，如果找到该类型，便会返回获取到的类型；否则返回 null。</param>
        /// <returns>如果找到与查找条件相匹配的的类型，则返回 true，否则返回 false。</returns>
        public static bool TryGetType(string typeName, Func<AssemblyName, Assembly> assemblyResolver, Func<Assembly, string, bool, Type> typeResolver, bool throwOnError, bool ignoreCase, out Type result)
        {
            try
            {
                result = Type.GetType(typeName, assemblyResolver, typeResolver, throwOnError, ignoreCase);
            }
            catch
            {
                result = null;
            }
            return result != null;
        }




        /// <summary>
        /// 获取已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <returns>已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetTypes()
        {
            return Assemblies.GetTypes();
        }

        /// <summary>
        /// 获取已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中指定的命名空间中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <param name="nameSpace">指定的命名空间。</param>
        /// <returns>已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中指定的命名空间中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetTypes(string nameSpace)
        {
            return GetTypes(nameSpace, AssemblyScope.Global);
        }

        /// <summary>
        /// 获取已加载至当前应用程序中指定程序集作用范围内的程序集中指定的命名空间中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <param name="nameSpace">指定的命名空间。</param>
        /// <param name="scope">一个 <see cref="AssemblyScope"/> 枚举值，表示指定的程序集作用范围。</param>
        /// <returns>已加载至当前应用程序中指定程序集作用范围内的程序集中指定的命名空间中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetTypes(string nameSpace, AssemblyScope scope)
        {
            return GetTypes(scope).Where(type => type.Namespace.Equals(nameSpace, StringComparison.CurrentCulture)).ToArray();
        }

        /// <summary>
        /// 获取已加载至当前应用程序中指定程序集作用范围内的程序集中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <param name="scope">一个 <see cref="AssemblyScope"/> 枚举值，表示指定的程序集作用范围。</param>
        /// <returns>已加载至当前应用程序中指定程序集作用范围内的程序集中定义的所有类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetTypes(AssemblyScope scope)
        {
            return Assemblies.GetTypes(scope);
        }

        /// <summary>
        /// 获取程序集中定义的类型。
        /// 同 <seealso cref="Assembly.GetTypes"/>；但在 <seealso cref="Assembly.GetTypes"/> 基础上屏蔽了 <see cref="System.Reflection.ReflectionTypeLoadException"/> 异常。
        /// </summary>
        /// <param name="assembly">应用程序集。</param>
        /// <returns>一个数组，包含此程序集中定义的所有类型。</returns>
        public static Type[] GetTypes(Assembly assembly)
        {
            return Assemblies.GetTypes(assembly);
        }



        /// <summary>
        /// 获取已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <returns>已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetPublicTypes()
        {
            return Assemblies.GetPublicTypes();
        }

        /// <summary>
        /// 获取已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中指定的命名空间中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <param name="nameSpace">指定的命名空间。</param>
        /// <returns>已加载至当前应用程序中整个应用程序域下所有已经加载的程序集中指定的命名空间中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetPublicTypes(string nameSpace)
        {
            return GetPublicTypes(nameSpace, AssemblyScope.Global);
        }

        /// <summary>
        /// 获取已加载至当前应用程序中指定程序集作用范围内的程序集中指定的命名空间中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <param name="nameSpace">指定的命名空间。</param>
        /// <param name="scope">一个 <see cref="AssemblyScope"/> 枚举值，表示指定的程序集作用范围。</param>
        /// <returns>已加载至当前应用程序中指定程序集作用范围内的程序集中指定的命名空间中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetPublicTypes(string nameSpace, AssemblyScope scope)
        {
            return GetPublicTypes(scope).Where(type => type.Namespace.Equals(nameSpace, StringComparison.CurrentCulture)).ToArray();
        }

        /// <summary>
        /// 获取已加载至当前应用程序中指定程序集作用范围内的程序集中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。
        /// </summary>
        /// <param name="scope">一个 <see cref="AssemblyScope"/> 枚举值，表示指定的程序集作用范围。</param>
        /// <returns>已加载至当前应用程序中指定程序集作用范围内的程序集中定义的所有公共类型 <see cref="System.Type"/> 集合所构成的一个数组。</returns>
        public static Type[] GetPublicTypes(AssemblyScope scope)
        {
            return Assemblies.GetPublicTypes(scope);
        }

        /// <summary>
        /// 获取此程序集中定义的公共类型的集合，这些公共类型在程序集外可见。
        /// 同 <seealso cref="Assembly.ExportedTypes"/>；但在 <seealso cref="Assembly.ExportedTypes"/> 基础上屏蔽了 <see cref="System.Reflection.ReflectionTypeLoadException"/> 异常。
        /// </summary>
        /// <param name="assembly">应用程序集。</param>
        /// <returns>此程序集中定义的公共类型的集合，这些公共类型在程序集外可见。</returns>
        public static Type[] GetPublicTypes(Assembly assembly)
        {
            return Assemblies.GetPublicTypes(assembly);
        }
    }
}
