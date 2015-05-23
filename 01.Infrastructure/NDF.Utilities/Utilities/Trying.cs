using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 定义一组系统级别的工具方法。
    /// </summary>
    public static class Trying
    {
        /// <summary>
        /// 以 try...catch... 语句体方式执行一个没有返回值的操作方法。
        /// 该方法确保被执行的操作方法不会抛出异常（在方法体内以 try...catch... 语句体捕获了异常）。
        /// </summary>
        /// <param name="tryAction">要执行的操作方法。</param>
        public static void Try(Action tryAction)
        {
            try
            {
                tryAction();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 以 try...catch... 语句体方式执行一个没有返回值的操作方法；
        /// 如果 tryAction 代码块出现异常，则执行 catchAction 代码块。
        /// 参数 abortOnCatchActionFailed 指定是否再次屏蔽 catchAction 代码块中可能抛出的异常。
        /// </summary>
        /// <param name="tryAction">要执行的操作方法。</param>
        /// <param name="catchAction">如果 tryAction 代码块出现异常，则执行 catchAction 代码块。</param>
        /// <param name="abortOnCatchActionFailed">指定是否再次屏蔽 catchAction 代码块中可能抛出的异常。默认为 false 表示即使 catchAction 中出现异常也 try...catch...处理掉。</param>
        public static void Try(Action tryAction, Action<Exception> catchAction, bool abortOnCatchActionFailed = false)
        {
            try
            {
                tryAction();
            }
            catch (Exception exception)
            {
                Action action = () => catchAction(exception);
                if (abortOnCatchActionFailed)
                {
                    action();
                }
                else
                {
                    Try(action);
                }
            }
        }



        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 该方法确保被执行的操作方法不会抛出异常（在方法体内以 try...catch... 语句体捕获了异常）。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryAction">要执行的具有返回值的操作方法。</param>
        /// <returns>如果方法 <paramref name="tryAction"/> 顺利执行成功，则将其执行结果返回；否则返回类型 <typeparamref name="T"/> 的默认值（Null）。</returns>
        public static T Try<T>(Func<T> tryAction) where T : class
        {
            T ret = default(T);
            try
            {
                ret = tryAction();
            }
            catch
            {
            }
            return ret;
        }

        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 如果 tryAction 代码块出现异常，则执行 catchAction 代码块。
        /// 参数 abortOnCatchActionFailed 指定是否再次屏蔽 catchAction 代码块中可能抛出的异常。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryAction">要执行的操作方法。</param>
        /// <param name="catchAction">如果 tryAction 代码块出现异常，则执行 catchAction 代码块。</param>
        /// <param name="abortOnCatchActionFailed">指定是否再次屏蔽 catchAction 代码块中可能抛出的异常。默认为 false 表示即使 catchAction 中出现异常也 try...catch...处理掉。</param>
        /// <returns>如果方法 <paramref name="tryAction"/> 顺利执行成功，则将其执行结果返回；否则返回 catchAction 代码块的执行结果。</returns>
        public static T Try<T>(Func<T> tryAction, Func<Exception, T> catchAction, bool abortOnCatchActionFailed = false) where T : class
        {
            T ret = default(T);
            try
            {
                ret = tryAction();
            }
            catch (Exception exception)
            {
                Func<T> action = () => catchAction(exception);
                if (abortOnCatchActionFailed)
                {
                    ret = action();
                }
                else
                {
                    ret = Try(action);
                }
            }
            return ret;
        }

        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 如果 tryAction 代码块出现异常，则执行 catchAction 代码块。
        /// 参数 abortOnCatchActionFailed 指定是否再次屏蔽 catchAction 代码块中可能抛出的异常。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryAction">要执行的操作方法。</param>
        /// <param name="catchAction">如果 tryAction 代码块出现异常，则执行 catchAction 代码块。</param>
        /// <param name="abortOnCatchActionFailed">指定是否再次屏蔽 catchAction 代码块中可能抛出的异常。默认为 false 表示即使 catchAction 中出现异常也 try...catch...处理掉。</param>
        /// <returns>如果方法 <paramref name="tryAction"/> 顺利执行成功，则将其执行结果返回；否则返回 catchAction 代码块的执行结果。</returns>
        public static T Try<T>(Func<T> tryAction, Action<Exception> catchAction, bool abortOnCatchActionFailed = false) where T : class
        {
            T ret = default(T);
            try
            {
                ret = tryAction();
            }
            catch (Exception exception)
            {
                Action action = () => catchAction(exception);
                if (abortOnCatchActionFailed)
                {
                    action();
                }
                else
                {
                    Try(action);
                }
            }
            return ret;
        }



        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 该方法确保被执行的操作方法不会抛出异常（在方法体内以 try...catch... 语句体捕获了异常）。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryAction">要执行的具有返回值的操作方法。</param>
        /// <returns>如果方法 <paramref name="tryAction"/> 顺利执行成功，则将其执行结果返回；否则返回类型 <typeparamref name="T"/> 的默认值（Null）。</returns>
        public static T? Try<T>(Func<T?> tryAction) where T : struct
        {
            T? ret = null;
            try
            {
                ret = tryAction();
            }
            catch
            {
            }
            return ret;
        }

        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 如果 tryAction 代码块出现异常，则执行 catchAction 代码块。
        /// 参数 abortOnCatchActionFailed 指定是否再次屏蔽 catchAction 代码块中可能抛出的异常。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryAction">要执行的操作方法。</param>
        /// <param name="catchAction">如果 tryAction 代码块出现异常，则执行 catchAction 代码块。</param>
        /// <param name="abortOnCatchActionFailed">指定是否再次屏蔽 catchAction 代码块中可能抛出的异常。默认为 false 表示即使 catchAction 中出现异常也 try...catch...处理掉。</param>
        /// <returns>如果方法 <paramref name="tryAction"/> 顺利执行成功，则将其执行结果返回；否则返回 catchAction 代码块的执行结果。</returns>
        public static T? Try<T>(Func<T?> tryAction, Func<Exception, T?> catchAction, bool abortOnCatchActionFailed = false) where T : struct
        {
            T? ret = null;
            try
            {
                ret = tryAction();
            }
            catch (Exception exception)
            {
                Func<T?> action = () => catchAction(exception);
                if (abortOnCatchActionFailed)
                {
                    ret = action();
                }
                else
                {
                    ret = Try(action);
                }
            }
            return ret;
        }

        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 如果 tryAction 代码块出现异常，则执行 catchAction 代码块。
        /// 参数 abortOnCatchActionFailed 指定是否再次屏蔽 catchAction 代码块中可能抛出的异常。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryAction">要执行的操作方法。</param>
        /// <param name="catchAction">如果 tryAction 代码块出现异常，则执行 catchAction 代码块。</param>
        /// <param name="abortOnCatchActionFailed">指定是否再次屏蔽 catchAction 代码块中可能抛出的异常。默认为 false 表示即使 catchAction 中出现异常也 try...catch...处理掉。</param>
        /// <returns>如果方法 <paramref name="tryAction"/> 顺利执行成功，则将其执行结果返回；否则返回 catchAction 代码块的执行结果。</returns>
        public static T? Try<T>(Func<T?> tryAction, Action<Exception> catchAction, bool abortOnCatchActionFailed = false) where T : struct
        {
            T? ret = null;
            try
            {
                ret = tryAction();
            }
            catch (Exception exception)
            {
                Action action = () => catchAction(exception);
                if (abortOnCatchActionFailed)
                {
                    action();
                }
                else
                {
                    Try(action);
                }
            }
            return ret;
        }


    }
}
