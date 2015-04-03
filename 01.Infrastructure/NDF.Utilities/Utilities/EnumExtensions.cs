using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对枚举类型和枚举值 <see cref="System.Enum"/> 操作方法的扩展。
    /// </summary>
    public static class EnumExtensions
    {

        /// <summary>
        /// 将一个标记了 <see cref="FlagsAttribute "/> 特性的枚举值分解成其包含已合并的多个该类型的枚举值。
        /// 例如通过对枚举值 BindingFlags.GetField | BindingFlags.GetProperty 指定该方法，将会返回一个数组 { BindingFlags.GetField, BindingFlags.GetProperty }。
        /// 如果传入的枚举参数所属的枚举类型未标记 <see cref="FlagsAttribute "/> 特性，该方法将会抛出异常。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static Enum[] Decompose(this System.Enum _this)
        {
            Type thisType = _this.GetType();

            if (!_this.IsDefinedFlags())
                throw new ArgumentException(string.Format("传入的枚举值 {0} 其类型为 {1}，未定义标记特性 FlagsAttribute。", _this, thisType));

            Enum[] enums = Enums.GetFields(thisType);
            List<Enum> list = new List<Enum>();
            foreach (Enum item in enums)
            {
                if (_this.HasFlag(item))
                    list.Add(item);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 将一个标记了 <see cref="FlagsAttribute "/> 特性的枚举值分解成其包含已合并的多个该类型的枚举值。
        /// 例如通过对枚举值 BindingFlags.GetField | BindingFlags.GetProperty 指定该方法，将会返回一个数组 { BindingFlags.GetField, BindingFlags.GetProperty }。
        /// 如果传入的枚举参数所属的枚举类型未标记 <see cref="FlagsAttribute "/> 特性，该方法将会抛出异常。
        /// 注意，传入的泛型参数 <typeparamref name="T"/> 必须与枚举值 <paramref name="_this"/> 的相同。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static T[] Decompose<T>(this System.Enum _this) where T : struct
        {
            Type thisType = _this.GetType();
            Type enumType = typeof(T);
            if (thisType != enumType)
                throw new ArgumentException(string.Format("传入的枚举值 {0} 其类型为 {1}，与被转换的目标类型 {2} 不一致。", _this, thisType, enumType));

            return Decompose(_this).Select(item => (T)(object)item).ToArray();
        }


        /// <summary>
        /// 获取枚举值的字段显示名称。
        /// </summary>
        /// <param name="_this">枚举值。</param>
        /// <returns>枚举值的字段显示名称。</returns>
        public static string DisplayName(this System.Enum _this)
        {
            return System.Enum.GetName(_this.GetType(), _this);
        }



        /// <summary>
        /// 判断某个枚举值所属的枚举类型是否定义了 <see cref="FlagsAttribute"/> 属性。
        /// </summary>
        /// <param name="_this">要判断的类型的枚举值。</param>
        /// <returns>如果该枚举值所属的枚举类型定义了 <see cref="FlagsAttribute"/> 属性，则返回 true，否则返回 false。</returns>
        public static bool IsDefinedFlags(this System.Enum _this)
        {
            Type enumType = _this.GetType();
            return Enums.IsDefinedFlags(enumType);
        }



        /// <summary>
        /// 获取该枚举值字段所定义的所有自定义特性的数组。
        /// </summary>
        /// <param name="_this">要获取自定义特性的枚举值。</param>
        /// <returns>该枚举值字段所定义的所有自定义特性的数组。</returns>
        public static IEnumerable<Attribute> GetCustomeAttributes(this System.Enum _this)
        {
            return _this.GetType().GetField(_this.DisplayName()).GetCustomAttributes(true).OfType<Attribute>();
        }

        /// <summary>
        /// 获取该枚举值字段所定义的所有指定类型的自定义特性的数组。
        /// </summary>
        /// <param name="_this">要获取自定义特性的枚举值。</param>
        /// <param name="attributeType">指定的自定义特性类型。</param>
        /// <returns>该枚举值字段所定义的所有指定类型的自定义特性的数组。</returns>
        public static IEnumerable<Attribute> GetCustomeAttributes(this System.Enum _this, System.Type attributeType)
        {
            Check.NotNull(attributeType);
            return _this.GetType().GetField(_this.DisplayName()).GetCustomAttributes(attributeType, true).OfType<Attribute>();
        }

        /// <summary>
        /// 获取该枚举值字段所定义的所有指定类型的自定义特性的数组。
        /// </summary>
        /// <typeparam name="TResult">指定的自定义特性类型。</typeparam>
        /// <param name="_this">要获取自定义特性的枚举值。</param>
        /// <returns>该枚举值字段所定义的所有指定类型的自定义特性的数组。</returns>
        public static IEnumerable<TResult> GetCustomeAttributes<TResult>(this System.Enum _this) where TResult : Attribute
        {
            return _this.GetType().GetField(_this.DisplayName()).GetCustomAttributes<TResult>(true);
        }
    }
}
