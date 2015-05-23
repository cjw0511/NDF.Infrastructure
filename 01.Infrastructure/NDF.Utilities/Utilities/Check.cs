using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 静态辅助类，提供一组用于检查输入参数是否合规的工具方法。
    /// </summary>
    public class Check
    {
        /// <summary>
        /// 检查输入的参数 <paramref name="value"/> 是否为 Null。
        /// 如果 <paramref name="value"/> 值为 Null，则抛出 <see cref="ArgumentNullException"/> 异常；否则返回 <paramref name="value"/> 值本身。
        /// </summary>
        /// <typeparam name="T"><paramref name="value"/> 参数的类型。</typeparam>
        /// <param name="value">被检查的参数值。</param>
        /// <returns>如果 <paramref name="value"/> 值不为 Null ，则返回 <paramref name="value"/> 值本身。</returns>
        /// <exception cref="ArgumentNullException">如果 <paramref name="value"/> 值为 Null，则抛出该异常。</exception>
        public static T NotNull<T>(T value) where T : class
        {
            return NotNull<T>(value, "value");
        }

        /// <summary>
        /// 检查输入的参数 <paramref name="value"/> 是否为 Null。
        /// 如果 <paramref name="value"/> 值为 Null，则抛出 <see cref="ArgumentNullException"/> 异常；否则返回 <paramref name="value"/> 值本身。
        /// </summary>
        /// <typeparam name="T"><paramref name="value"/> 参数的类型。</typeparam>
        /// <param name="value">被检查的参数值。</param>
        /// <param name="parameterName">被检查的参数名称。</param>
        /// <returns>如果 <paramref name="value"/> 值不为 Null ，则返回 <paramref name="value"/> 值本身。</returns>
        /// <exception cref="ArgumentNullException">如果 <paramref name="value"/> 值为 Null，则抛出该异常。</exception>
        public static T NotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        /// <summary>
        /// 检查输入的参数 <paramref name="value"/> 是否为 Null。
        /// 如果 <paramref name="value"/> 值为 Null，则抛出 <see cref="ArgumentNullException"/> 异常；否则返回 <paramref name="value"/> 值本身。
        /// </summary>
        /// <typeparam name="T"><paramref name="value"/> 参数的类型。</typeparam>
        /// <param name="value">被检查的参数值。</param>
        /// <returns>如果 <paramref name="value"/> 值不为 Null ，则返回 <paramref name="value"/> 值本身。</returns>
        /// <exception cref="ArgumentNullException">如果 <paramref name="value"/> 值为 Null，则抛出该异常。</exception>
        public static T? NotNull<T>(T? value) where T : struct
        {
            return NotNull<T>(value, "value");
        }

        /// <summary>
        /// 检查输入的参数 <paramref name="value"/> 是否为 Null。
        /// 如果 <paramref name="value"/> 值为 Null，则抛出 <see cref="ArgumentNullException"/> 异常；否则返回 <paramref name="value"/> 值本身。
        /// </summary>
        /// <typeparam name="T"><paramref name="value"/> 参数的类型。</typeparam>
        /// <param name="value">被检查的参数值。</param>
        /// <param name="parameterName">被检查的参数名称。</param>
        /// <returns>如果 <paramref name="value"/> 值不为 Null ，则返回 <paramref name="value"/> 值本身。</returns>
        /// <exception cref="ArgumentNullException">如果 <paramref name="value"/> 值为 Null，则抛出该异常。</exception>
        public static T? NotNull<T>(T? value, string parameterName) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }


        /// <summary>
        /// 检查输入的条件参数 <paramref name="condition"/> 是否为真(true)。
        /// 如果条件参数 <paramref name="condition"/> 值为 false，则抛出 <see cref="System.InvalidOperationException"/> 异常；否则返回 <paramref name="condition"/> 值本身。
        /// </summary>
        /// <param name="condition">被检查的条件参数。</param>
        /// <param name="message">当被检查的条件参数 <paramref name="condition"/> 值不为真(true)时，抛出的 <see cref="System.InvalidOperationException"/> 异常中所带的消息。</param>
        /// <returns>如果条件参数 <paramref name="condition"/> 值为真(true)，则返回 <paramref name="condition"/> 值本身。</returns>
        /// <exception cref="System.InvalidOperationException">如果被检查的条件参数 <paramref name="condition"/> 值不为真(true)，则抛出该异常。</exception>
        public static bool NotTrue(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
            return condition;
        }

        /// <summary>
        /// 检查输入的条件参数 <paramref name="condition"/> 函数运算结果是否为真(true)。
        /// 如果条件参数 <paramref name="condition"/> 函数运算结果值为 false，则抛出 <see cref="System.InvalidOperationException"/> 异常；否则返回 true。
        /// </summary>
        /// <param name="condition">被检查的条件参数，该参数是一个运算结果为 System.Nullable&gt;bool?&lt; 类型值的委托。</param>
        /// <param name="message">当被检查的条件参数 <paramref name="condition"/> 函数运算结果不为真(true)时，抛出的 <see cref="System.InvalidOperationException"/> 异常中所带的消息。</param>
        /// <param name="abortOnFailed">指定在指定函数 <paramref name="condition"/> 时，是否屏蔽其执行过程中可能会抛出的异常。</param>
        /// <returns>如果条件参数 <paramref name="condition"/> 运算结果为真(true)，则返回 true。</returns>
        /// <exception cref="System.ArgumentNullException">如果被检查的条件参数为 Null 时，则抛出该异常。</exception>
        /// <exception cref="System.InvalidOperationException">如果被检查的条件参数 <paramref name="condition"/> 运算结果不为真(true)，则抛出该异常。</exception>
        public static bool NotTrue(Func<bool?> condition, string message, bool abortOnFailed = false)
        {
            Check.NotNull(condition);
            bool? b = abortOnFailed ? condition() : Trying.Try(condition);
            bool ret = b.HasValue ? b.HasValue : false;
            return NotTrue(ret, message);
        }



        /// <summary>
        /// 检查输入的参数是否为 Null、空或者空白字符串组成。
        /// 如果 <paramref name="value"/> 值为 Null、空或者空白字符串组成，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value"/> 值本身。
        /// </summary>
        /// <param name="value">被检查的参数值。</param>
        /// <returns>如果 <paramref name="value"/> 值不为 Null、空或者空白字符串组成 ，则返回 <paramref name="value"/> 值本身。</returns>
        /// <exception cref="System.ArgumentNullException">如果 <paramref name="value"/> 值为 Null、空或者空白字符串组成，则抛出该异常。</exception>
        public static string NotEmpty(string value)
        {
            return NotEmpty(value, "value");
        }

        /// <summary>
        /// 检查输入的参数是否为 Null、空或者空白字符串组成。
        /// 如果 <paramref name="value"/> 值为 Null、空或者空白字符串组成，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value"/> 值本身。
        /// </summary>
        /// <param name="value">被检查的参数值。</param>
        /// <param name="parameterName">被检查的参数名称。</param>
        /// <returns>如果 <paramref name="value"/> 值不为 Null、空或者空白字符串组成 ，则返回 <paramref name="value"/> 值本身。</returns>
        /// <exception cref="System.ArgumentNullException">如果 <paramref name="value"/> 值为 Null、空或者空白字符串组成，则抛出该异常。</exception>
        public static string NotEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(string.Format(Resources.Check_NotEmpty, parameterName), parameterName);
            }
            return value;
        }


        /// <summary>
        /// 检查输入的参数是否为 Null、空或者空白字符串组成。
        /// 如果 <paramref name="value"/> 值为 Null、空或者空白字符串组成，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value"/> 值本身经过去首尾空格处理后的文本内容。
        /// </summary>
        /// <param name="value">被检查的参数值。</param>
        /// <returns>如果 <paramref name="value"/> 值不为 Null、空或者空白字符串组成 ，则返回 <paramref name="value"/> 参数去除首尾空格后的结果。</returns>
        /// <exception cref="System.ArgumentNullException">如果 <paramref name="value"/> 值为 Null、空或者空白字符串组成，则抛出该异常。</exception>
        public static string EmptyCheck(string value)
        {
            return EmptyCheck(value, "value");
        }

        /// <summary>
        /// 检查输入的参数是否为 Null、空或者空白字符串组成。
        /// 如果 <paramref name="value"/> 值为 Null、空或者空白字符串组成，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value"/> 值本身经过去首尾空格处理后的文本内容。
        /// </summary>
        /// <param name="value">被检查的参数值。</param>
        /// <param name="parameterName">被检查的参数名称。</param>
        /// <returns>如果 <paramref name="value"/> 值不为 Null、空或者空白字符串组成 ，则返回 <paramref name="value"/> 参数去除首尾空格后的结果。</returns>
        /// <exception cref="System.ArgumentNullException">如果 <paramref name="value"/> 值为 Null、空或者空白字符串组成，则抛出该异常。</exception>
        public static string EmptyCheck(string value, string parameterName)
        {
            value = value != null ? value.Trim() : value;
            NotEmpty(value, parameterName);
            return value;
        }



        /// <summary>
        /// 检查输入的参数是否为 Null 或者空序列。
        /// 如果 <paramref name="values"/> 值为 Null 或者空序列，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="values"/> 值本身。
        /// </summary>
        /// <typeparam name="TSource">被检查的数组参数中元素的类型。</typeparam>
        /// <param name="values">被检查的数组参数。</param>
        /// <returns>如果 <paramref name="values"/> 值不为 Null 或者空序列，则返回 <paramref name="values"/> 值本身。</returns>
        /// <exception cref="System.ArgumentException">如果 <paramref name="values"/> 值为 Null 或者空序列，则抛出该异常。</exception>
        public static IEnumerable<TSource> NotEmpty<TSource>(IEnumerable<TSource> values)
        {
            return NotEmpty(values, "value");
        }

        /// <summary>
        /// 检查输入的参数是否为 Null 或者空序列。
        /// 如果 <paramref name="values"/> 值为 Null 或者空序列，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="values"/> 值本身。
        /// </summary>
        /// <typeparam name="TSource">被检查的数组参数中元素的类型。</typeparam>
        /// <param name="values">被检查的数组参数。</param>
        /// <param name="parameterName">被检查的数组参数名称。</param>
        /// <returns>如果 <paramref name="values"/> 值不为 Null 或者空序列，则返回 <paramref name="values"/> 值本身。</returns>
        /// <exception cref="System.ArgumentException">如果 <paramref name="values"/> 值为 Null 或者空序列，则抛出该异常。</exception>
        public static IEnumerable<TSource> NotEmpty<TSource>(IEnumerable<TSource> values, string parameterName)
        {
            if (EnumerableExtensions.IsNullOrEmpty(values))
            {
                throw new ArgumentException(string.Format(Resources.Check_ArrayNotEmpty, parameterName), parameterName);
            }
            return values;
        }



        /// <summary>
        /// 检查输入参数 <paramref name="value1"/> 的值是否等同于参数 <paramref name="value2"/> 的值；
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value1"/> 值本身。
        /// </summary>
        /// <typeparam name="T1">表示参数 <paramref name="value1"/> 的类型。</typeparam>
        /// <typeparam name="T2">表示参数 <paramref name="value2"/> 的类型。</typeparam>
        /// <param name="value1">被检查的输入参数。</param>
        /// <param name="value2">作为与 <paramref name="value1"/> 值对比的另一个参数。</param>
        /// <returns>如果输入参数 <paramref name="value1"/> 的值等同于参数 <paramref name="value2"/>，则返回 <paramref name="value1"/> 值本身。</returns>
        /// <exception cref="System.ArgumentException">
        /// 如果输入参数 <paramref name="value1"/> 的值不等同于参数 <paramref name="value2"/>，则抛出该异常。
        /// </exception>
        public static T1 Equals<T1, T2>(T1 value1, T2 value2)
        {
            return Equals(value1, value2, (a, b) => a.Equals(b));
        }

        /// <summary>
        /// 以指定的委托函数作为谓词检查输入参数 <paramref name="value1"/> 的值是否等同于参数 <paramref name="value2"/> 的值；
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value1"/> 值本身。
        /// </summary>
        /// <typeparam name="T1">表示参数 <paramref name="value1"/> 的类型。</typeparam>
        /// <typeparam name="T2">表示参数 <paramref name="value2"/> 的类型。</typeparam>
        /// <param name="value1">被检查的输入参数。</param>
        /// <param name="value2">作为与 <paramref name="value1"/> 值对比的另一个参数。</param>
        /// <param name="predicate">一个谓词函数，用于比较参数 <paramref name="value1"/> 和参数 <paramref name="value2"/> 是否相等。</param>
        /// <returns>如果输入参数 <paramref name="value1"/> 的值等同于参数 <paramref name="value2"/>，则返回 <paramref name="value1"/> 值本身。</returns>
        /// <exception cref="System.ArgumentException">
        /// 如果输入参数 <paramref name="value1"/> 的值不等同于参数 <paramref name="value2"/>，则抛出该异常。
        /// </exception>
        public static T1 Equals<T1, T2>(T1 value1, T2 value2, Func<T1, T2, bool> predicate)
        {
            return Equals(value1, value2, "value", predicate);
        }

        /// <summary>
        /// 检查输入参数 <paramref name="value1"/> 的值是否等同于参数 <paramref name="value2"/> 的值；
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value1"/> 值本身。
        /// </summary>
        /// <typeparam name="T1">表示参数 <paramref name="value1"/> 的类型。</typeparam>
        /// <typeparam name="T2">表示参数 <paramref name="value2"/> 的类型。</typeparam>
        /// <param name="value1">被检查的输入参数。</param>
        /// <param name="value2">作为与 <paramref name="value1"/> 值对比的另一个参数。</param>
        /// <param name="parameterName">表示参数 <paramref name="value1"/> 的名称。</param>
        /// <returns>如果输入参数 <paramref name="value1"/> 的值等同于参数 <paramref name="value2"/>，则返回 <paramref name="value1"/> 值本身。</returns>
        /// <exception cref="System.ArgumentException">
        /// 如果输入参数 <paramref name="value1"/> 的值不等同于参数 <paramref name="value2"/>，则抛出该异常。
        /// </exception>
        public static T1 Equals<T1, T2>(T1 value1, T2 value2, string parameterName)
        {
            return Equals(value1, value2, parameterName, (a, b) => a.Equals(b));
        }

        /// <summary>
        /// 以指定的委托函数作为谓词检查输入参数 <paramref name="value1"/> 的值是否等同于参数 <paramref name="value2"/> 的值；
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value1"/> 值本身。
        /// </summary>
        /// <typeparam name="T1">表示参数 <paramref name="value1"/> 的类型。</typeparam>
        /// <typeparam name="T2">表示参数 <paramref name="value2"/> 的类型。</typeparam>
        /// <param name="value1">被检查的输入参数。</param>
        /// <param name="value2">作为与 <paramref name="value1"/> 值对比的另一个参数。</param>
        /// <param name="parameterName">表示参数 <paramref name="value1"/> 的名称。</param>
        /// <param name="predicate">一个谓词函数，用于比较参数 <paramref name="value1"/> 和参数 <paramref name="value2"/> 是否相等。</param>
        /// <returns>如果输入参数 <paramref name="value1"/> 的值等同于参数 <paramref name="value2"/>，则返回 <paramref name="value1"/> 值本身。</returns>
        /// <exception cref="System.ArgumentException">
        /// 如果输入参数 <paramref name="value1"/> 的值不等同于参数 <paramref name="value2"/>，则抛出该异常。
        /// </exception>
        public static T1 Equals<T1, T2>(T1 value1, T2 value2, string parameterName, Func<T1, T2, bool> predicate)
        {
            if (!predicate(value1, value2))
            {
                throw new ArgumentException(string.Format(Resources.Check_Equals, parameterName, value2), parameterName);
            }
            return value1;
        }



        /// <summary>
        /// 检查输入参数 <paramref name="value1"/> 的值是否为 Null、空字符串或者值等同于参数 <paramref name="value2"/> 的值；
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value1"/> 值本身。
        /// </summary>
        /// <param name="value1">被检查的输入参数。</param>
        /// <param name="value2">作为与 <paramref name="value1"/> 值对比的另一个参数。</param>
        /// <returns>如果输入参数 <paramref name="value1"/> 的值为 Null、空字符串或者值等同于参数 <paramref name="value2"/>，则返回 <paramref name="value1"/> 值本身。</returns>
        /// <exception cref="System.ArgumentException">
        /// 如果输入参数 <paramref name="value1"/> 的值不为 Null、空字符串且不等同于参数 <paramref name="value2"/> 的值，则抛出该异常。
        /// </exception>
        public static string EmptyOrEquals(string value1, string value2)
        {
            return EmptyOrEquals(value1, value2, "value");
        }

        /// <summary>
        /// 检查输入参数 <paramref name="value1"/> 的值是否为 Null、空字符串或者值等同于参数 <paramref name="value2"/> 的值；
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value1"/> 值本身。
        /// </summary>
        /// <param name="value1">被检查的输入参数。</param>
        /// <param name="value2">作为与 <paramref name="value1"/> 值对比的另一个参数。</param>
        /// <param name="parameterName">表示参数 <paramref name="value1"/> 的名称。</param>
        /// <returns>如果输入参数 <paramref name="value1"/> 的值为 Null、空字符串或者值等同于参数 <paramref name="value2"/>，则返回 <paramref name="value1"/> 值本身。</returns>
        /// <exception cref="System.ArgumentException">
        /// 如果输入参数 <paramref name="value1"/> 的值不为 Null、空字符串且不等同于参数 <paramref name="value2"/> 的值，则抛出该异常。
        /// </exception>
        public static string EmptyOrEquals(string value1, string value2, string parameterName)
        {
            if (!string.IsNullOrWhiteSpace(value1) && value1 != value2)
            {
                throw new ArgumentException(string.Format(Resources.Check_EmptyOrEquals, parameterName, value2));
            }
            return value1;
        }



        /// <summary>
        /// 检查输入的数组参数 <paramref name="values"/> 中应存在至少一个元素的值不能为 Null；
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="values"/> 值本身。
        /// </summary>
        /// <typeparam name="TSource">被检查的数组参数中元素的类型。</typeparam>
        /// <param name="values">被检查的数组参数，将会判断该数组中每个元素是否为 Null。</param>
        /// <returns>如果输入的数组参数 <paramref name="values"/> 不为 Null，则包含 1 个或多个元素，则存在至少一个元素的值不为 Null，则返回参数 <paramref name="values"/> 本身。</returns>
        /// <exception cref="System.ArgumentException">如果数组参数 <paramref name="values"/> 中不存在任何元素(空数组)，或其中所有的元素值均为 Null，则抛出该异常。</exception>
        /// <exception cref="System.ArgumentNullException">如果数组参数 <paramref name="values"/> 为 Null，则抛出该异常。</exception>
        public static TSource[] AnyNotNull<TSource>(TSource[] values)
        {
            return AnyNotNull(values, "values");
        }

        /// <summary>
        /// 检查输入的数组参数 <paramref name="values"/> 中应存在至少一个元素的值不能为 Null；
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="values"/> 值本身。
        /// </summary>
        /// <typeparam name="TSource">被检查的数组参数中元素的类型。</typeparam>
        /// <param name="values">被检查的数组参数，将会判断该数组中每个元素是否为 Null。</param>
        /// <param name="parameterName">表示数组参数 <paramref name="values"/> 的参数名称。</param>
        /// <returns>如果输入的数组参数 <paramref name="values"/> 不为 Null，则包含 1 个或多个元素，则存在至少一个元素的值不为 Null，则返回参数 <paramref name="values"/> 本身。</returns>
        /// <exception cref="System.ArgumentException">如果数组参数 <paramref name="values"/> 中不存在任何元素(空数组)，或其中所有的元素值均为 Null，则抛出该异常。</exception>
        /// <exception cref="System.ArgumentNullException">如果数组参数 <paramref name="values"/> 为 Null，则抛出该异常。</exception>
        public static TSource[] AnyNotNull<TSource>(TSource[] values, string parameterName)
        {
            Check.NotEmpty(values, parameterName);
            if (!values.Any(item => item != null))
            {
                throw new ArgumentException(string.Format(Resources.Check_AnyNotNull, parameterName));
            }
            return values;
        }



        /// <summary>
        /// 检查输入的参数是否为其所代表的类型的默认值(class 类型为 null，struct 类型为初始化为零或 null 的每个结构成员)。
        /// 如果 <paramref name="value"/> 值为其所代表的类型的默认值，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value"/> 值本身。
        /// </summary>
        /// <typeparam name="T"><paramref name="value"/> 参数的类型。</typeparam>
        /// <param name="value">被检查的参数值。</param>
        /// <returns>如果 <paramref name="value"/> 值不为其所代表的类型的默认值，则返回 <paramref name="value"/> 值本身。</returns>
        /// <exception cref="System.ArgumentException">如果 <paramref name="value"/> 值为其所代表的类型的默认值(class 类型为 null，struct 类型为初始化为零或 null 的每个结构成员)，则抛出该异常。</exception>
        public static T IsDefault<T>(T value)
        {
            return IsDefault<T>(value, "value");
        }

        /// <summary>
        /// 检查输入的参数是否为其所代表的类型的默认值(class 类型为 null，struct 类型为初始化为零或 null 的每个结构成员)。
        /// 如果 <paramref name="value"/> 值为其所代表的类型的默认值，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="value"/> 值本身。
        /// </summary>
        /// <typeparam name="T"><paramref name="value"/> 参数的类型。</typeparam>
        /// <param name="value">被检查的参数值。</param>
        /// <param name="parameterName">被检查的参数名称。</param>
        /// <returns>如果 <paramref name="value"/> 值不为其所代表的类型的默认值，则返回 <paramref name="value"/> 值本身。</returns>
        /// <exception cref="System.ArgumentException">如果 <paramref name="value"/> 值为其所代表的类型的默认值(class 类型为 null，struct 类型为初始化为零或 null 的每个结构成员)，则抛出该异常。</exception>
        public static T IsDefault<T>(T value, string parameterName)
        {
            if (object.Equals(value, default(T)))
            {
                throw new ArgumentException(string.Format(Resources.Check_IsDefault, parameterName), parameterName);
            }
            return value;
        }



        /// <summary>
        /// 确认参数 <paramref name="a"/> 所表示的类型是否为从参数 <paramref name="b"/> 所表示的类型派生的。
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="a"/> 值本身。
        /// </summary>
        /// <param name="a">表示一个 <see cref="System.Type"/> 类型，其作为待检查项的子类。</param>
        /// <param name="b">表示一个 <see cref="System.Type"/> 类型，其作为待检查项的父类。</param>
        /// <returns>参数 <paramref name="a"/> 所表示的类型是否为从参数 <paramref name="b"/> 所表示的类型派生的，则返回 <paramref name="a"/> 值本身。</returns>
        /// <exception cref="System.ArgumentNullException">如果参数 <paramref name="a"/> 或参数 <paramref name="b"/> 其中任意一个为 Null，则抛出该异常。</exception>
        /// <exception cref="System.ArgumentException">如果参数 <paramref name="a"/> 所表示的类型不是从参数 <paramref name="b"/> 所表示的类型派生的，则抛出该异常。</exception>
        public static Type SubclassOf(Type a, Type b)
        {
            NotNull(a);
            NotNull(b);
            if (!a.IsSubclassOf(b))
            {
                throw new ArgumentException(string.Format(Resources.Check_SubclassOf, a, b));
            }
            return a;
        }

        /// <summary>
        /// 确认参数 <paramref name="a"/> 所表示的类型是否可以从参数 <paramref name="b"/> 所表示的类型的实例分配。
        /// 如果不可以，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="a"/> 值本身。
        /// </summary>
        /// <param name="a">表示一个 <see cref="System.Type"/> 类型。</param>
        /// <param name="b">表示一个 <see cref="System.Type"/> 类型。</param>
        /// <returns>参数 <paramref name="a"/> 所表示的类型可以从参数 <paramref name="b"/> 所表示的类型的实例分配，则返回 <paramref name="a"/> 值本身。</returns>
        /// <exception cref="System.ArgumentNullException">如果参数 <paramref name="a"/> 或参数 <paramref name="b"/> 其中任意一个为 Null，则抛出该异常。</exception>
        /// <exception cref="System.ArgumentException">如果参数 <paramref name="a"/> 所表示的类型不可以从参数 <paramref name="b"/> 所表示的类型的实例分配，则抛出该异常。</exception>
        /// <remarks>
        /// 如果满足下列任一条件，则表示参数 <paramref name="a"/> 所表示的类型可以从参数 <paramref name="b"/> 所表示的类型的实例分配：
        ///     1、<paramref name="a"/> 和当前 <paramref name="b"/> 表示同一类型；
        ///     2、当前 <paramref name="a"/> 位于 <paramref name="b"/> 的继承层次结构中；
        ///     3、当前 <paramref name="a"/> 是 <paramref name="b"/> 实现的接口；
        ///     4、<paramref name="b"/> 是泛型类型参数且 <paramref name="a"/> 表示 <paramref name="b"/> 的约束之一。
        /// </remarks>
        public static Type AssignableFrom(Type a, Type b)
        {
            NotNull(a);
            NotNull(b);
            if (!a.IsAssignableFrom(b))
            {
                throw new ArgumentException(string.Format(Resources.Check_AssignableFrom, a, b));
            }
            return a;
        }

        /// <summary>
        /// 确认参数 <paramref name="a"/> 所表示的类型是否继承或等价于参数 <paramref name="b"/> 所表示的类型。
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="a"/> 值本身。
        /// </summary>
        /// <param name="a">表示一个 <see cref="System.Type"/> 类型。</param>
        /// <param name="b">表示一个 <see cref="System.Type"/> 类型。</param>
        /// <returns>参数 <paramref name="a"/> 所表示的类型继承或等价于参数 <paramref name="b"/> 所表示的类型，则返回 <paramref name="a"/> 值本身。</returns>
        /// <exception cref="System.ArgumentNullException">如果参数 <paramref name="a"/> 或参数 <paramref name="b"/> 其中任意一个为 Null，则抛出该异常。</exception>
        /// <exception cref="System.ArgumentException">如果参数 <paramref name="a"/> 所表示的类型不是继承或等价于参数 <paramref name="b"/> 所表示的类型，则抛出该异常。</exception>
        public static Type InhertOf(Type a, Type b)
        {
            NotNull(a);
            NotNull(b);
            if (!a.IsInhertOf(b))
            {
                throw new ArgumentException(string.Format(Resources.Check_InhertOf, a, b));
            }
            return a;
        }


        /// <summary>
        /// 检查输入参数 <paramref name="obj"/> 的类型是否是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现。
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="obj"/> 值本身。
        /// </summary>
        /// <typeparam name="TSource">被检查的输入参数的类型。</typeparam>
        /// <param name="obj">被检查的输入参数，用于验证其类型是否为指定的范围。</param>
        /// <param name="types">被检查的类型数组，数组中的每个元素都用于判断 <paramref name="obj"/> 是否为其实现。</param>
        /// <returns>如果输入参数 <paramref name="obj"/> 的类型是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现，则返回参数 <paramref name="obj"/> 本身。</returns>
        /// <exception cref="System.ArgumentException">
        /// 如果输入参数 <paramref name="obj"/> 的类型不是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现，则抛出该异常。
        /// </exception>
        public static TSource IsRangeTypes<TSource>(TSource obj, params Type[] types)
        {
            IsRangeTypes(typeof(TSource), types);
            return obj;
        }

        /// <summary>
        /// 检查输入参数 <paramref name="c"/> 所表示的类型是否是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现。
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="c"/> 值本身。
        /// </summary>
        /// <param name="c">被检查的输入参数，用于验证其所表示的类型是否为指定的范围。</param>
        /// <param name="types">被检查的类型数组，数组中的每个元素都用于判断 <paramref name="c"/> 所表示的类型是否为其实现。</param>
        /// <returns>如果输入参数 <paramref name="c"/> 所表示的类型不是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现，则抛出该异常。</returns>
        /// <exception cref="System.ArgumentException">
        /// 如果输入参数 <paramref name="c"/> 所表示的类型不是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现，则抛出该异常。
        /// </exception>
        public static Type IsRangeTypes(Type c, params Type[] types)
        {
            Check.NotNull(c);
            Check.NotEmpty(types);
            if (!types.Any(type => c.IsInhertOrImplementOf(type)))
            {
                throw new ArgumentException(string.Format(Resources.Check_IsRangeTypes, c, types));
            }
            return c;
        }

        /// <summary>
        /// 检查输入参数 <paramref name="array"/> 中存在任意一个元素的类型都必须是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现。
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="array"/> 值本身。
        /// </summary>
        /// <param name="array">被检查的数组类型输入参数，用于验证其中每个元素的类型都必须为指定的范围。</param>
        /// <param name="types">检查的类型数组，数组中的每个元素都用于判断 <paramref name="array"/> 中每个元素的类型都必须为其实现。</param>
        /// <returns>如果输入参数 <paramref name="array"/> 中存在任意一个元素的类型不是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现，则抛出该异常。</returns>
        /// <exception cref="System.ArgumentException">
        /// 如果输入参数 <paramref name="array"/> 中存在任意一个元素的类型不是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现，则抛出该异常。
        /// </exception>
        public static object[] ArrayIsRangeTypes(object[] array, params Type[] types)
        {
            Check.NotEmpty(array);
            ArrayIsRangeTypes(array.Select(item => item.GetType()).ToArray(), types);
            return array;
        }

        /// <summary>
        /// 检查输入参数 <paramref name="array"/> 中存在任意一个元素所表示的类型都必须是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现。
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="array"/> 值本身。
        /// </summary>
        /// <param name="array">被检查的数组类型输入参数，用于验证其中每个元素所表示的类型都必须为指定的范围。</param>
        /// <param name="types">被检查的类型数组，数组中的每个元素都用于判断 <paramref name="array"/> 中每个元素所表示的类型都必须为其实现。</param>
        /// <returns>如果输入参数 <paramref name="array"/> 中存在任意一个元素所表示的类型不是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现，则抛出该异常。</returns>
        /// <exception cref="System.ArgumentException">
        /// 如果输入参数 <paramref name="array"/> 中存在任意一个元素所表示的类型不是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现，则抛出该异常。
        /// </exception>
        public static Type[] ArrayIsRangeTypes(Type[] array, params Type[] types)
        {
            Check.NotEmpty(array);
            Check.NotEmpty(types);
            if (!array.All(item => types.Any(type => item.IsInhertOrImplementOf(type))))
            {
                throw new ArgumentException(string.Format(Resources.Check_ArrayIsRangeTypes, array, types));
            }
            return array;
        }



        /// <summary>
        /// 确认参数 <paramref name="c"/> 所表示的类型是否为值类型（即，不是类或接口）。
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="c"/> 值本身。
        /// </summary>
        /// <param name="c">表示一个 <see cref="System.Type"/> 类型。</param>
        /// <returns>如果参数 <paramref name="c"/> 所表示的类型是否为值类型，则返回 <paramref name="c"/> 值本身。</returns>
        /// <exception cref="System.ArgumentNullException">如果参数 <paramref name="c"/> 或为 Null，则抛出该异常。</exception>
        /// <exception cref="System.ArgumentException">如果参数 <paramref name="c"/> 所表示的类型不是一个值类型，则抛出该异常。</exception>
        public static Type IsValueType(Type c)
        {
            NotNull(c);
            if (!c.IsValueType)
            {
                throw new ArgumentException(string.Format(Resources.Check_IsValueType, c));
            }
            return c;
        }

        /// <summary>
        /// 确认参数 <paramref name="c"/> 所表示的类型是否为接口定义（即，不是类或值类型）。
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="c"/> 值本身。
        /// </summary>
        /// <param name="c">表示一个 <see cref="System.Type"/> 类型。</param>
        /// <returns>如果参数 <paramref name="c"/> 所表示的类型是否为接口定义，则返回 <paramref name="c"/> 值本身。</returns>
        /// <exception cref="System.ArgumentNullException">如果参数 <paramref name="c"/> 或为 Null，则抛出该异常。</exception>
        /// <exception cref="System.ArgumentException">如果参数 <paramref name="c"/> 所表示的类型不是一个接口定义，则抛出该异常。</exception>
        public static Type IsInterface(Type c)
        {
            NotNull(c);
            if (!c.IsInterface)
            {
                throw new ArgumentException(string.Format(Resources.Check_IsInterface, c));
            }
            return c;
        }

        /// <summary>
        /// 确认参数 <paramref name="c"/> 所表示的类型是否为类（即，不是值类型或接口）。
        /// 如果不是，则抛出 <see cref="System.ArgumentException"/> 异常；否则返回 <paramref name="c"/> 值本身。
        /// </summary>
        /// <param name="c">表示一个 <see cref="System.Type"/> 类型。</param>
        /// <returns>如果参数 <paramref name="c"/> 所表示的类型是否为类型定义，则返回 <paramref name="c"/> 值本身。</returns>
        /// <exception cref="System.ArgumentNullException">如果参数 <paramref name="c"/> 或为 Null，则抛出该异常。</exception>
        /// <exception cref="System.ArgumentException">如果参数 <paramref name="c"/> 所表示的类型不是一个类型定义，则抛出该异常。</exception>
        public static Type IsClass(Type c)
        {
            NotNull(c);
            if (!c.IsClass)
            {
                throw new ArgumentException(string.Format(Resources.Check_IsClass, c));
            }
            return c;
        }
    }
}
