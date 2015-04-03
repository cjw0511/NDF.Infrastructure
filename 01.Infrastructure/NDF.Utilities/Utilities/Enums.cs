using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对枚举类型和枚举值 <see cref="System.Enum"/> 的工具操作方法。
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// 获取指定枚举类型 <typeparamref name="T"/> 中定义的所有枚举字段的值所组成的一个数组。
        /// </summary>
        /// <typeparam name="T">表示一个枚举类型。</typeparam>
        /// <returns>返回指定枚举类型 <typeparamref name="T"/> 中定义的所有枚举字段的值所组成的一个数组。</returns>
        public static T[] GetFields<T>() where T : struct
        {
            return GetFields(typeof(T)).Select(item => (T)(object)item).ToArray();
        }

        /// <summary>
        /// 获取指定枚举类型 <paramref name="enumType"/> 中定义的所有枚举字段的值所组成的一个数组。
        /// </summary>
        /// <param name="enumType">表示一个枚举类型。</param>
        /// <returns>返回指定枚举类型 <paramref name="enumType"/> 中定义的所有枚举字段的值所组成的一个数组。</returns>
        public static Enum[] GetFields(Type enumType)
        {
            Check.SubclassOf(enumType, typeof(System.Enum));
            return System.Enum.GetNames(enumType).Select(name => (Enum)System.Enum.Parse(enumType, name)).ToArray();
        }


        /// <summary>
        /// 判断某个枚举类型是否定义了 <see cref="FlagsAttribute"/> 属性。
        /// </summary>
        /// <param name="enumType">要判断的枚举类型。</param>
        /// <returns>如果该枚举类型定义了 <see cref="FlagsAttribute"/> 属性，则返回 true，否则返回 false。</returns>
        public static bool IsDefinedFlags(Type enumType)
        {
            Check.SubclassOf(enumType, typeof(System.Enum));
            return enumType.GetCustomAttribute<FlagsAttribute>() != null;
        }


    }
}
