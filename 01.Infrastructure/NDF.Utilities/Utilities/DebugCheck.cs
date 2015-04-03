using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 静态辅助类，基于 .NET 编译器调试特性提供一组用于检查输入参数是否合规的工具方法。
    /// </summary>
    public class DebugCheck
    {
        /// <summary>
        /// 测试输入的参数 <paramref name="value"/> 是否为 Null。如果是，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <typeparam name="T"><paramref name="value"/> 参数的类型。</typeparam>
        /// <param name="value">被检查的参数值。</param>
        [Conditional("DEBUG")]
        public static void NotNull<T>(T value) where T : class
        {
            Debug.Assert(value != null);
        }

        /// <summary>
        /// 测试输入的参数 <paramref name="value"/> 是否为 Null。如果是，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <typeparam name="T"><paramref name="value"/> 参数的类型。</typeparam>
        /// <param name="value">被检查的参数值。</param>
        [Conditional("DEBUG")]
        public static void NotNull<T>(T? value) where T : struct
        {
            Debug.Assert(value != null);
        }

        /// <summary>
        /// 检查输入的参数是否为 Null、空或者空白字符串组成。如果是，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="value">被检查的参数值。</param>
        [Conditional("DEBUG")]
        public static void NotEmpty(string value)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(value));
        }

        /// <summary>
        /// 检查输入的数组类型参数是否为 Null 或者空数组。如果是，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <typeparam name="TSource">表示被检查的数组参数中元素的类型。</typeparam>
        /// <param name="values">被检查的数组参数值。</param>
        [Conditional("DEBUG")]
        public static void NotEmpty<TSource>(IEnumerable<TSource> values)
        {
            Debug.Assert(!EnumerableExtensions.IsNullOrEmpty(values));
        }


        /// <summary>
        /// 检查条件；如果条件为 false，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="condition">要计算的条件表达式。 如果条件为 true，则不发送失败消息，并且不显示消息框。</param>
        [Conditional("DEBUG")]
        public static void NotTrue(bool condition)
        {
            Debug.Assert(condition);
        }

        /// <summary>
        /// 检查条件；如果条件为 false，则输出指定消息，并显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="condition">要计算的条件表达式。 如果条件为 true，则不发送指定消息，并且不显示消息框。</param>
        /// <param name="message">要发送给 System.Diagnostics.Trace.Listeners 集合的消息。</param>
        [Conditional("DEBUG")]
        public static void NotTrue(bool condition, string message)
        {
            Debug.Assert(condition, message);
        }

        /// <summary>
        /// 检查条件；如果条件为 false，则输出两条指定消息，并显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="condition">要计算的条件表达式。 如果条件为 true，则不发送指定消息，并且不显示消息框。</param>
        /// <param name="message">要发送给 <see cref="System.Diagnostics.Trace.Listeners"/> 集合的消息。</param>
        /// <param name="detailMessage">要发送给 <see cref="System.Diagnostics.Trace.Listeners"/> 集合的详细消息。</param>
        [Conditional("DEBUG")]
        public static void NotTrue(bool condition, string message, string detailMessage)
        {
            Debug.Assert(condition, message, detailMessage);
        }

        /// <summary>
        /// 检查条件；如果条件为 false，则输出两条指定消息（简单消息和格式化消息），并显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="condition">要计算的条件表达式。 如果条件为 true，则不发送指定消息，并且不显示消息框。</param>
        /// <param name="message">要发送给 <see cref="System.Diagnostics.Trace.Listeners"/> 集合的消息。</param>
        /// <param name="detailMessage">
        /// 要发送到 <see cref="System.Diagnostics.Trace.Listeners"/> 集合的复合格式字符串（见“备注”）。 该消息包含与零个或多个格式项混合的文本，它与
        ///     args 数组中的对象相对应。
        /// </param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        [Conditional("DEBUG")]
        public static void NotTrue(bool condition, string message, string detailMessage, params object[] args)
        {
            Debug.Assert(condition, message, detailMessage, args);
        }



        /// <summary>
        /// 检查条件函数的运算结果；如果条件函数运算结果为 false，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="condition">要计算的条件表达式运算函数。 如果条件为 true，则不发送失败消息，并且不显示消息框。</param>
        /// <param name="throwError">指定在指定函数 <paramref name="condition"/> 时，是否屏蔽其执行过程中可能会抛出的异常。</param>
        [Conditional("DEBUG")]
        public static void NotTrue(Func<bool?> condition, bool throwError = false)
        {
            bool? b = throwError ? condition() : Utility.TryCatchExecute(condition);
            Debug.Assert(b.HasValue ? b.Value : false);
        }

        /// <summary>
        /// 检查条件函数的运算结果；如果条件函数运算结果为 false，则输出指定消息，并显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="condition">要计算的条件表达式运算函数。 如果条件为 true，则不发送指定消息，并且不显示消息框。</param>
        /// <param name="message">要发送给 System.Diagnostics.Trace.Listeners 集合的消息。</param>
        /// <param name="throwError">指定在指定函数 <paramref name="condition"/> 时，是否屏蔽其执行过程中可能会抛出的异常。</param>
        [Conditional("DEBUG")]
        public static void NotTrue(Func<bool?> condition, string message, bool throwError = false)
        {
            bool? b = throwError ? condition() : Utility.TryCatchExecute(condition);
            Debug.Assert(b.HasValue ? b.Value : false, message);
        }

        /// <summary>
        /// 检查条件函数的运算结果；如果条件函数运算结果为 false，则输出两条指定消息，并显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="condition">要计算的条件表达式运算函数。 如果条件为 true，则不发送指定消息，并且不显示消息框。</param>
        /// <param name="message">要发送给 <see cref="System.Diagnostics.Trace.Listeners"/> 集合的消息。</param>
        /// <param name="detailMessage">要发送给 <see cref="System.Diagnostics.Trace.Listeners"/> 集合的详细消息。</param>
        /// <param name="throwError">指定在指定函数 <paramref name="condition"/> 时，是否屏蔽其执行过程中可能会抛出的异常。</param>
        [Conditional("DEBUG")]
        public static void NotTrue(Func<bool?> condition, string message, string detailMessage, bool throwError = false)
        {
            bool? b = throwError ? condition() : Utility.TryCatchExecute(condition);
            Debug.Assert(b.HasValue ? b.Value : false, message, detailMessage);
        }

        /// <summary>
        /// 检查条件函数的运算结果；如果条件函数运算结果为 false，则输出两条指定消息（简单消息和格式化消息），并显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="condition">要计算的条件表达式运算函数。 如果条件为 true，则不发送指定消息，并且不显示消息框。</param>
        /// <param name="message">要发送给 <see cref="System.Diagnostics.Trace.Listeners"/> 集合的消息。</param>
        /// <param name="detailMessage">
        /// 要发送到 <see cref="System.Diagnostics.Trace.Listeners"/> 集合的复合格式字符串（见“备注”）。 该消息包含与零个或多个格式项混合的文本，它与
        ///     args 数组中的对象相对应。
        /// </param>
        /// <param name="throwError">指定在指定函数 <paramref name="condition"/> 时，是否屏蔽其执行过程中可能会抛出的异常。</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        [Conditional("DEBUG")]
        public static void NotTrue(Func<bool?> condition, string message, string detailMessage, bool throwError = false, params object[] args)
        {
            bool? b = throwError ? condition() : Utility.TryCatchExecute(condition);
            Debug.Assert(b.HasValue ? b.Value : false, message, detailMessage, args);
        }





        /// <summary>
        /// 测试输入参数 <paramref name="value1"/> 的值是否等同于参数 <paramref name="value2"/> 的值；
        /// 如果不是，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <typeparam name="T1">表示参数 <paramref name="value1"/> 的类型。</typeparam>
        /// <typeparam name="T2">表示参数 <paramref name="value2"/> 的类型。</typeparam>
        /// <param name="value1">被检查的输入参数。</param>
        /// <param name="value2">作为与 <paramref name="value1"/> 值对比的另一个参数。</param>
        [Conditional("DEBUG")]
        public static void Equals<T1, T2>(T1 value1, T2 value2)
        {
            Debug.Assert(value1.Equals(value2));
        }

        /// <summary>
        /// 以指定的委托函数作为谓词测试输入参数 <paramref name="value1"/> 的值是否等同于参数 <paramref name="value2"/> 的值；
        /// 如果不是，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <typeparam name="T1">表示参数 <paramref name="value1"/> 的类型。</typeparam>
        /// <typeparam name="T2">表示参数 <paramref name="value2"/> 的类型。</typeparam>
        /// <param name="value1">被检查的输入参数。</param>
        /// <param name="value2">作为与 <paramref name="value1"/> 值对比的另一个参数。</param>
        /// <param name="predicate">一个谓词函数，用于比较参数 <paramref name="value1"/> 和参数 <paramref name="value2"/> 是否相等。</param>
        [Conditional("DEBUG")]
        public static void Equals<T1, T2>(T1 value1, T2 value2, Func<T1, T2, bool> predicate)
        {
            Debug.Assert(predicate(value1, value2));
        }

        /// <summary>
        /// 测试输入参数 <paramref name="value1"/> 的值是否为 Null、空字符串或者值等同于参数 <paramref name="value2"/> 的值；
        /// 如果不是，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="value1">被检查的输入参数。</param>
        /// <param name="value2">作为与 <paramref name="value1"/> 值对比的另一个参数。</param>
        [Conditional("DEBUG")]
        public static void EmptyOrEquals(string value1, string value2)
        {
            Debug.Assert(string.IsNullOrWhiteSpace(value1) || value1 == value2);
        }



        /// <summary>
        /// 测试输入的数组参数 <paramref name="values"/> 的值不为 Null，且不是空数组，且存在至少一个元素的值不为 Null;
        /// 如果不是，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <typeparam name="TSource">被检查的数组参数中元素的类型。</typeparam>
        /// <param name="values">被检查的数组参数，将会判断该数组中每个元素是否为 Null。</param>
        [Conditional("DEBUG")]
        public static void AnyNotNull<TSource>(TSource[] values)
        {
            if (values.IsNullOrEmpty())
            {
                Debug.Assert(false);
                return;
            }
            Debug.Assert(values.Any(item => item != null));
        }




        /// <summary>
        /// 测试输入的参数是否为其所代表的类型的默认值。如果是，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <typeparam name="T"><paramref name="value"/> 参数的类型。</typeparam>
        /// <param name="value">被检查的参数值。</param>
        public static void IsDefault<T>(T value)
        {
            Debug.Assert(object.Equals(value, default(T)));
        }


        /// <summary>
        /// 测试参数 <paramref name="a"/> 所表示的类型是否为从参数 <paramref name="b"/> 所表示的类型派生的。
        /// 如果不是，或者 <paramref name="a"/> 、 <paramref name="b"/> 中任意一个为 Null，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="a">表示一个 <see cref="System.Type"/> 类型，其作为待检查项的子类。</param>
        /// <param name="b">表示一个 <see cref="System.Type"/> 类型，其作为待检查项的父类。</param>
        [Conditional("DEBUG")]
        public static void SubclassOf(Type a, Type b)
        {
            Debug.Assert(a != null && b != null && a.IsSubclassOf(b));
        }

        /// <summary>
        /// 确认参数 <paramref name="a"/> 所表示的类型是否可以从参数 <paramref name="b"/> 所表示的类型的实例分配。
        /// 如果不可以，或者 <paramref name="a"/> 、 <paramref name="b"/> 中任意一个为 Null，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="a">表示一个 <see cref="System.Type"/> 类型。</param>
        /// <param name="b">表示一个 <see cref="System.Type"/> 类型。</param>
        /// <remarks>
        /// 如果满足下列任一条件，则表示参数 <paramref name="a"/> 所表示的类型可以从参数 <paramref name="b"/> 所表示的类型的实例分配：
        ///     1、<paramref name="a"/> 和当前 <paramref name="b"/> 表示同一类型；
        ///     2、当前 <paramref name="a"/> 位于 <paramref name="b"/> 的继承层次结构中；
        ///     3、当前 <paramref name="a"/> 是 <paramref name="b"/> 实现的接口；
        ///     4、<paramref name="b"/> 是泛型类型参数且 <paramref name="a"/> 表示 <paramref name="b"/> 的约束之一。
        /// </remarks>
        [Conditional("DEBUG")]
        public static void AssignableFrom(Type a, Type b)
        {
            Debug.Assert(a != null && b != null && a.IsAssignableFrom(b));
        }

        /// <summary>
        /// 确认参数 <paramref name="a"/> 所表示的类型是否继承或等价于参数 <paramref name="b"/> 所表示的类型。
        /// 如果不可以，或者 <paramref name="a"/> 、 <paramref name="b"/> 中任意一个为 Null，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="a">表示一个 <see cref="System.Type"/> 类型。</param>
        /// <param name="b">表示一个 <see cref="System.Type"/> 类型。</param>
        [Conditional("DEBUG")]
        public static void InhertOf(Type a, Type b)
        {
            Debug.Assert(a != null && b != null && a.IsInhertOf(b));
        }


        /// <summary>
        /// 确认输入参数 <paramref name="obj"/> 的类型是否是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现。
        /// 如果不是，或者 <paramref name="obj"/> 、<paramref name="types"/> 中任意一个值为 Null，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <typeparam name="TSource">表示被检查的输入参数 <paramref name="obj"/> 的类型。</typeparam>
        /// <param name="obj">被检查的输入参数。</param>
        /// <param name="types">被检查的类型数组，数组中的每个元素都用于判断 <paramref name="obj"/> 是否为其实现。</param>
        [Conditional("DEBUG")]
        public static void IsRangeTypes<TSource>(TSource obj, params Type[] types)
        {
            if (types.IsNullOrEmpty())
            {
                Debug.Assert(false);
                return;
            }
            Type c = typeof(TSource);
            Debug.Assert(types.Any(type => c.IsInhertOrImplementOf(type)));
        }

        /// <summary>
        /// 确认输入参数 <paramref name="c"/> 所表示的类型是否是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现。
        /// 如果不是，或者 <paramref name="c"/> 、<paramref name="types"/> 中任意一个值为 Null，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="c">被检查的类型参数。</param>
        /// <param name="types">被检查的类型数组，数组中的每个元素都用于判断 <paramref name="c"/> 是否为其实现。</param>
        [Conditional("DEBUG")]
        public static void IsRangeTypes(Type c, params Type[] types)
        {

            Debug.Assert(c != null && !types.IsNullOrEmpty() && types.Any(type => c.IsInhertOrImplementOf(type)));
        }

        /// <summary>
        /// 确认输入参数 <paramref name="array"/> 中存在人一个元素的类型都必须是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现。
        /// 如果不是，或者 <paramref name="array"/> 、<paramref name="types"/> 中任意一个值为 Null 或空数组，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="array">被检查的数组类型输入参数，用于验证其中每个元素的类型都必须为指定的范围。</param>
        /// <param name="types">被检查的类型数组，数组中的每个元素都用于判断 <paramref name="array"/> 中每个元素所表示的类型都必须为其实现。</param>
        [Conditional("DEBUG")]
        public static void ArrayIsRangeTypes(object[] array, params Type[] types)
        {
            if (array.IsNullOrEmpty() || types.IsNullOrEmpty())
            {
                Debug.Assert(false);
                return;
            }
            var temps = array.Select(item => item.GetType());
            Debug.Assert(temps.All(item => types.Any(type => item.IsInhertOrImplementOf(type))));
        }

        /// <summary>
        /// 确认输入参数 <paramref name="array"/> 中存在任意一个元素所表示的类型都必须是另一个输入的数组参数 <paramref name="types"/> 中任意一个元素所表示的类型的实现。
        /// 如果不是，或者 <paramref name="array"/> 、<paramref name="types"/> 中任意一个值为 Null 或空数组，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="array">被检查的数组类型输入参数，用于验证其中每个元素所表示的类型都必须为指定的范围。</param>
        /// <param name="types">被检查的类型数组，数组中的每个元素都用于判断 <paramref name="array"/> 中每个元素所表示的类型都必须为其实现。</param>
        [Conditional("DEBUG")]
        public static void ArrayIsRangeTypes(Type[] array, params Type[] types)
        {
            Debug.Assert(!array.IsNullOrEmpty() && !types.IsNullOrEmpty() &&
                array.All(item => types.Any(type => item.IsInhertOrImplementOf(type))));
        }



        /// <summary>
        /// 测试参数 <paramref name="c"/> 所表示的类型是否为一个值类型（即，不是类或接口）。
        /// 如果不是，或 <paramref name="c"/> 为 Null，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="c">表示一个 <see cref="System.Type"/> 类型。</param>
        [Conditional("DEBUG")]
        public static void IsValueType(Type c)
        {
            Debug.Assert(c != null && c.IsValueType);
        }

        /// <summary>
        /// 测试参数 <paramref name="c"/> 所表示的类型是否为一个接口定义（即，不是类或值类型）。
        /// 如果不是，或 <paramref name="c"/> 为 Null，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="c">表示一个 <see cref="System.Type"/> 类型。</param>
        [Conditional("DEBUG")]
        public static void IsInterface(Type c)
        {
            Debug.Assert(c != null && c.IsInterface);
        }

        /// <summary>
        /// 测试参数 <paramref name="c"/> 所表示的类型是否为一个类（即，不是值类型或接口定义）。
        /// 如果不是，或 <paramref name="c"/> 为 Null，则显示一个消息框，其中会显示调用堆栈。
        /// </summary>
        /// <param name="c">表示一个 <see cref="System.Type"/> 类型。</param>
        [Conditional("DEBUG")]
        public static void IsClass(Type c)
        {
            Debug.Assert(c != null && c.IsClass);
        }
    }
}
