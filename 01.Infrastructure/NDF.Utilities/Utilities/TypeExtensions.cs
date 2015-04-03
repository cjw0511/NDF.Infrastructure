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
    /// 提供一组对 类型 <see cref="System.Type"/> 操作方法的扩展。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 判断当前 <see cref="System.Type"/> 指示的类型是否继承于（或等价于）某个 <see cref="System.Type"/> 类型。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <param name="c">用于比较的 <see cref="System.Type"/> 类型对象。</param>
        /// <returns>如果当前类型等价或继承与指定的类型，则返回 true，否则返回 false。</returns>
        public static bool IsInhertOf(this System.Type _this, System.Type c)
        {
            return _this == c || _this.IsSubclassOf(c);
        }

        /// <summary>
        /// 判断当前 <see cref="System.Type"/> 指示的类型是否实现或(等价于)某个 <see cref="System.Type"/> 指示的接口。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <param name="c">用于比较的 <see cref="System.Type"/> 类型对象，表示一个接口类型。</param>
        /// <returns>如果当前类型等价或继承与指定的接口类型，则返回 true，否则返回 false。</returns>
        public static bool IsImplementOf(this System.Type _this, System.Type c)
        {
            return _this == c || _this.GetInterfaces().Contains(c);
        }

        /// <summary>
        /// 判断当前 <see cref="System.Type"/> 指示的类型是否实现、继承等价于某个 <see cref="System.Type"/> 指示的类型或接口。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <param name="c">用于比较的 <see cref="System.Type"/> 类型对象。</param>
        /// <returns>如果当前类型实现、继承等价于指定的类型或接口，则返回 true，否则返回 false。</returns>
        public static bool IsInhertOrImplementOf(this System.Type _this, System.Type c)
        {
            return _this.IsInhertOf(c) || _this.IsImplementOf(c);
        }


        /// <summary>
        /// 判断传入的类型定义是否为类型 <see cref="System.Nullable&lt;T&gt;"/> 分配的可空值类型。
        /// </summary>
        /// <param name="_this">被判断的类型。</param>
        /// <returns>如果传入的类型定义 <paramref name="_this"/> 是类型 <see cref="System.Nullable&lt;T&gt;"/> 分配的可空值类型，则返回 true；否则返回 false。</returns>
        public static bool IsNullableType(this System.Type _this)
        {
            Check.NotNull(_this);
            return _this.IsGenericType && _this.GetGenericTypeDefinition() == typeof(Nullable<>);
        }


        /// <summary>
        /// 判断传入的类型定义的对象值是否可以为 NULL。
        /// </summary>
        /// <param name="_this">被判断的类型。</param>
        /// <returns>如果传入的类型定义 <paramref name="_this"/> 是一个值类型且派生至 <see cref="System.Nullable&lt;T&gt;"/>，或者是一个引用类型，则返回 true；否则返回 false。</returns>
        public static bool IsNullable(this System.Type _this)
        {
            Check.NotNull(_this);
            return _this.IsValueType ? IsNullableType(_this) : true;
        }

        /// <summary>
        /// 判断传入的类型定义是否为数值类型。
        /// </summary>
        /// <param name="_this">被判断的类型定义。</param>
        /// <returns>
        /// 如果传入的类型定义 <paramref name="_this"/> 是一个数值类型：
        ///     Byte、SByte、Int16、Int32、Int64、UInt16、UInt32、UInt64、Decimal、Single、Double、IntPtr、UIntPtr 其中
        ///     一种，则返回 true；否则返回 false。
        /// </returns>
        public static bool IsNumeric(this System.Type _this)
        {
            Check.NotNull(_this);
            return _this == typeof(Byte)
                || _this == typeof(SByte)
                || _this == typeof(Int16)
                || _this == typeof(Int32)
                || _this == typeof(Int64)
                || _this == typeof(UInt16)
                || _this == typeof(UInt32)
                || _this == typeof(UInt64)
                || _this == typeof(Decimal)
                || _this == typeof(Single)
                || _this == typeof(Double)
                || _this == typeof(IntPtr)
                || _this == typeof(UIntPtr);
        }

        /// <summary>
        /// 判断传入的类型定义是否为数值类型，或者是可空的泛型数据类型 <see cref="System.Nullable&lt;T&gt;"/> 并且其第一个泛型参数类型是数值类型。
        /// </summary>
        /// <param name="_this">被判断的类型定义。</param>
        /// <returns>如果传入的类型定义是数值类型，或者其是一个泛型数据类型 <see cref="System.Nullable&lt;T&gt;"/> 并且其第一个泛型参数类型是数值类型，则返回 true；否则返回 false。</returns>
        public static bool IsNumericWithNullable(this System.Type _this)
        {
            Check.NotNull(_this);
            if (IsNumeric(_this))
            {
                return true;
            }
            else if (_this.IsGenericType)
            {
                Type genericType = _this.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    Type[] genericParameters = _this.GetGenericArguments();
                    return genericParameters.Length == 1 && IsNumeric(genericParameters[0]);
                }
            }
            return false;
        }




        /// <summary>
        /// 获取当前类型的所有子类或接口实现类。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <returns>当前类型的所有子类或接口实现类。</returns>
        public static Type[] GetSubClass(this Type _this)
        {
            return GetSubClass(_this, AssemblyScope.Global);
        }

        /// <summary>
        /// 获取当前类型在指定应用程序集范围的所有子类或接口实现类。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <param name="scope">指定的应用程序集范围</param>
        /// <returns>当前类型在指定应用程序集范围的所有子类或接口实现类。</returns>
        public static Type[] GetSubClass(this Type _this, AssemblyScope scope)
        {
            return Types.GetTypes(scope).Where(type => type != _this && type.IsInhertOrImplementOf(_this)).ToArray();
        }

        /// <summary>
        /// 获取当前类型在所有已经加载的应用程序集中的所有父类。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <returns>
        /// 当前类型在所有已经加载的应用程序集中的所有父类 <see cref="System.Type"/> 所构成的一个数组；
        /// 如果当前类型没有任何父类(当前类型为 <see cref="System.Object"/>)，则返回一个长度为 0 的空数组。
        /// </returns>
        public static Type[] GetParentClass(this Type _this)
        {
            List<Type> list = new List<Type>();
            Type type = _this;
            while (type.BaseType != null)
            {
                list.Add(type.BaseType);
                type = type.BaseType;
            }
            return list.ToArray();
        }




        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的所有属性，包括其所有内部成员和继承的属性。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <returns>当前 <see cref="System.Type"/> 定义的所有 <see cref="System.Reflection.PropertyInfo"/> 所构成的一个数组（如果找到的话）；否则为空数组。</returns>
        public static PropertyInfo[] GetAllProperties(this Type _this)
        {
            Check.NotNull(_this);
            return _this.GetRuntimeProperties().ToArray();
        }

        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的所有指定名称的属性，包括其所有内部成员和继承的属性。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <param name="name">指定的属性名称。</param>
        /// <returns>当前 <see cref="System.Type"/> 定义的所有指定名称的 <see cref="System.Reflection.PropertyInfo"/> 所构成的一个数组（如果找到的话）；否则为空数组。</returns>
        public static PropertyInfo[] GetAllProperties(this Type _this, string name)
        {
            Check.NotEmpty(name, "name");
            return GetAllProperties(_this).Where(p => p.Name == name).ToArray();
        }



        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的所有方法，包括其所有内部成员和继承的方法。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <returns>当前 <see cref="System.Type"/> 定义的所有方法 <see cref="System.Reflection.MethodInfo"/> 所构成的一个数组（如果找到的话）；否则为空数组。</returns>
        public static MethodInfo[] GetAllMethods(this Type _this)
        {
            Check.NotNull(_this);
            return _this.GetRuntimeMethods().ToArray();
        }

        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的所有指定名称的方法，包括其所有内部成员和继承的方法。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <param name="name">指定的方法名称。</param>
        /// <returns>当前 <see cref="System.Type"/> 定义的所有指定名称的方法 <see cref="System.Reflection.MethodInfo"/> 所构成的一个数组（如果找到的话）；否则为空数组。</returns>
        public static MethodInfo[] GetAllMethods(this Type _this, string name)
        {
            Check.NotEmpty(name, "name");
            return GetAllMethods(_this).Where(m => m.Name == name).ToArray();
        }

        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的所有指定名称和参数列表的方法，包括其所有内部成员和继承的方法。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <param name="name">指定的方法名称。</param>
        /// <param name="parameterTypes">指定的方法参数列表及类型。</param>
        /// <returns>当前 <see cref="System.Type"/> 定义的所有指定名称和参数列表的方法 <see cref="System.Reflection.MethodInfo"/> 所构成的一个数组（如果找到的话）；否则为空数组。</returns>
        public static MethodInfo[] GetAllMethods(this Type _this, string name, params Type[] parameterTypes)
        {
            var methods = GetAllMethods(_this, name);
            return (parameterTypes == null || parameterTypes.Length == 0) ? methods : methods.Where(method =>
            {
                var parameters = method.GetParameters();
                return parameterTypes.Length == parameters.Length && parameters.All((p, i) => p.ParameterType == parameterTypes[i]);
            }).ToArray();
        }



        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的所有事件，包括其所有内部成员和继承的事件。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <returns>当前 <see cref="System.Type"/> 定义的所有事件 <see cref="System.Reflection.EventInfo"/> 所构成的一个数组（如果找到的话）；否则为空数组。</returns>
        public static EventInfo[] GetAllEvents(this Type _this)
        {
            Check.NotNull(_this);
            return _this.GetRuntimeEvents().ToArray();
        }

        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的所有指定名称事件，包括其所有内部成员和继承的事件。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <param name="name">指定的事件名称。</param>
        /// <returns>当前 <see cref="System.Type"/> 定义的所有指定名称事件 <see cref="System.Reflection.EventInfo"/> 所构成的一个数组（如果找到的话）；否则为空数组。</returns>
        public static EventInfo[] GetAllEvents(this Type _this, string name)
        {
            Check.NotEmpty(name, "name");
            return GetAllEvents(_this).Where(e => e.Name == name).ToArray();
        }



        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的所有字段，包括其所有内部成员和继承的字段。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <returns>当前 <see cref="System.Type"/> 定义的所有字段 <see cref="System.Reflection.FieldInfo"/> 所构成的一个数组（如果找到的话）；否则为空数组。</returns>
        public static FieldInfo[] GetAllFields(this Type _this)
        {
            Check.NotNull(_this);
            return _this.GetRuntimeFields().ToArray();
        }

        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的所有指定名称字段，包括其所有内部成员和继承的字段。
        /// </summary>
        /// <param name="_this">当前 <see cref="System.Type"/> 对象。</param>
        /// <param name="name">指定的字段名称。</param>
        /// <returns>当前 <see cref="System.Type"/> 定义的所有指定名称字段 <see cref="System.Reflection.FieldInfo"/> 所构成的一个数组（如果找到的话）；否则为空数组。</returns>
        public static FieldInfo[] GetAllFields(this Type _this, string name)
        {
            Check.NotEmpty(name, "name");
            return GetAllFields(_this).Where(field => field.Name == name).ToArray();
        }




        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的所有成员，包括其所有内部成员和继承的成员。
        /// </summary>
        /// <param name="_this">表示待查找成员的 System.Type 对象。</param>
        /// <returns>返回指定 System.Type 对象的所有成员(包括其所有内部成员和继承的成员)。</returns>
        public static MemberInfo[] GetAllMembers(this Type _this)
        {
            Check.NotNull(_this);
            return _this.GetRuntimeMethods().ToArray();
        }

        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的所有指定名称成员，包括其所有内部成员和继承的成员。
        /// </summary>
        /// <param name="_this">表示待查找成员的 System.Type 对象。</param>
        /// <param name="name">指定的 MemberInfo 名称。</param>
        /// <returns>返回 System.Type 对象中符合过滤条件的所有 MemberInfo 成员对象。</returns>
        public static MemberInfo[] GetAllMembers(this Type _this, string name)
        {
            Check.NotEmpty(name, "name");
            return GetAllMembers(_this).Where(m => m.Name == name).ToArray();
        }

        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的指定类型的所有成员，包括其所有内部成员和继承的成员。
        /// </summary>
        /// <param name="_this">表示待查找成员的 System.Type 对象。</param>
        /// <param name="memberType">指定的 MemberTypes 成员类型；如果该参数值为 MemberTypes.All，则不按成员类型过滤返回列表。</param>
        /// <returns>返回 System.Type 对象中符合过滤条件的所有 MemberInfo 成员对象。</returns>
        public static MemberInfo[] GetAllMembers(this Type _this, MemberTypes memberType)
        {
            return GetAllMembers(_this).Where(m => memberType.HasFlag(m.MemberType)).ToArray();
        }

        /// <summary>
        /// 获取当前 <see cref="System.Type"/> 定义的指定名称和类型的所有成员，包括其所有内部成员和继承的成员。
        /// </summary>
        /// <param name="_this">表示待查找成员的 System.Type 对象。</param>
        /// <param name="name">指定的 MemberInfo 名称。</param>
        /// <param name="memberType">指定的 MemberTypes 成员类型；如果该参数值为 MemberTypes.All，则不按成员类型过滤返回列表。</param>
        /// <returns>一个表示具有指定名称的公共成员的 <see cref="System.Reflection.MemberInfo"/> 对象数组（如果找到的话）；否则为空数组。</returns>
        public static MemberInfo[] GetAllMembers(this Type _this, string name, MemberTypes memberType)
        {
            Check.NotEmpty(name, "name");
            return GetAllMembers(_this).Where(m => m.Name == name && memberType.HasFlag(m.MemberType)).ToArray();
        }
    }
}
