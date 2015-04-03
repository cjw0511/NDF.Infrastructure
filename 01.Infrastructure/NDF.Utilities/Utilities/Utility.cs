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
    public static class Utility
    {
        /// <summary>
        /// 以 try...catch... 语句体方式执行一个没有返回值的操作方法。
        /// 该方法确保被执行的操作方法不会抛出异常（在方法体内以 try...catch... 语句体捕获了异常）。
        /// </summary>
        /// <param name="tryCode">要执行的操作方法。</param>
        public static void TryCatchExecute(Action tryCode)
        {
            try
            {
                tryCode();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 以 try...catch... 语句体方式执行一个没有返回值的操作方法；
        /// 如果 tryCode 代码块出现异常，则执行 catchCode 代码块。
        /// 参数 throwCatchError 指定是否再次屏蔽 catchCode 代码块中可能抛出的异常。
        /// </summary>
        /// <param name="tryCode">要执行的操作方法。</param>
        /// <param name="catchCode">如果 tryCode 代码块出现异常，则执行 catchCode 代码块。</param>
        /// <param name="throwCatchError">指定是否再次屏蔽 catchCode 代码块中可能抛出的异常。默认为 false 表示即使 catchCode 中出现异常也 try...catch...处理掉。</param>
        public static void TryCatchExecute(Action tryCode, Action catchCode, bool throwCatchError = false)
        {
            try
            {
                tryCode();
            }
            catch
            {
                if (throwCatchError)
                {
                    catchCode();
                }
                else
                {
                    TryCatchExecute(catchCode);
                }
            }
        }



        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 该方法确保被执行的操作方法不会抛出异常（在方法体内以 try...catch... 语句体捕获了异常）。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryCode">要执行的具有返回值的操作方法。</param>
        /// <returns>如果方法 <paramref name="tryCode"/> 顺利执行成功，则将其执行结果返回；否则返回类型 <typeparamref name="T"/> 的默认值（Null）。</returns>
        public static T TryCatchExecute<T>(Func<T> tryCode) where T : class
        {
            T ret = default(T);
            try
            {
                ret = tryCode();
            }
            catch
            {
            }
            return ret;
        }

        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 如果 tryCode 代码块出现异常，则执行 catchCode 代码块。
        /// 参数 throwCatchError 指定是否再次屏蔽 catchCode 代码块中可能抛出的异常。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryCode">要执行的操作方法。</param>
        /// <param name="catchCode">如果 tryCode 代码块出现异常，则执行 catchCode 代码块。</param>
        /// <param name="throwCatchError">指定是否再次屏蔽 catchCode 代码块中可能抛出的异常。默认为 false 表示即使 catchCode 中出现异常也 try...catch...处理掉。</param>
        /// <returns>如果方法 <paramref name="tryCode"/> 顺利执行成功，则将其执行结果返回；否则返回 catchCode 代码块的执行结果。</returns>
        public static T TryCatchExecute<T>(Func<T> tryCode, Func<T> catchCode, bool throwCatchError = false) where T : class
        {
            T ret = default(T);
            try
            {
                ret = tryCode();
            }
            catch
            {
                if (throwCatchError)
                {
                    ret = catchCode();
                }
                else
                {
                    ret = TryCatchExecute(catchCode);
                }
            }
            return ret;
        }

        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 如果 tryCode 代码块出现异常，则执行 catchCode 代码块。
        /// 参数 throwCatchError 指定是否再次屏蔽 catchCode 代码块中可能抛出的异常。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryCode">要执行的操作方法。</param>
        /// <param name="catchCode">如果 tryCode 代码块出现异常，则执行 catchCode 代码块。</param>
        /// <param name="throwCatchError">指定是否再次屏蔽 catchCode 代码块中可能抛出的异常。默认为 false 表示即使 catchCode 中出现异常也 try...catch...处理掉。</param>
        /// <returns>如果方法 <paramref name="tryCode"/> 顺利执行成功，则将其执行结果返回；否则返回 catchCode 代码块的执行结果。</returns>
        public static T TryCatchExecute<T>(Func<T> tryCode, Action catchCode, bool throwCatchError = false) where T : class
        {
            T ret = default(T);
            try
            {
                ret = tryCode();
            }
            catch
            {
                if (throwCatchError)
                {
                    catchCode();
                }
                else
                {
                    catchCode();
                }
            }
            return ret;
        }



        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 该方法确保被执行的操作方法不会抛出异常（在方法体内以 try...catch... 语句体捕获了异常）。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryCode">要执行的具有返回值的操作方法。</param>
        /// <returns>如果方法 <paramref name="tryCode"/> 顺利执行成功，则将其执行结果返回；否则返回类型 <typeparamref name="T"/> 的默认值（Null）。</returns>
        public static T? TryCatchExecute<T>(Func<T?> tryCode) where T : struct
        {
            T? ret = null;
            try
            {
                ret = tryCode();
            }
            catch
            {
            }
            if (ret == null)
            {
                return null;
            }
            else
            {
                return ret.Value;
            }
        }

        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 如果 tryCode 代码块出现异常，则执行 catchCode 代码块。
        /// 参数 throwCatchError 指定是否再次屏蔽 catchCode 代码块中可能抛出的异常。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryCode">要执行的操作方法。</param>
        /// <param name="catchCode">如果 tryCode 代码块出现异常，则执行 catchCode 代码块。</param>
        /// <param name="throwCatchError">指定是否再次屏蔽 catchCode 代码块中可能抛出的异常。默认为 false 表示即使 catchCode 中出现异常也 try...catch...处理掉。</param>
        /// <returns>如果方法 <paramref name="tryCode"/> 顺利执行成功，则将其执行结果返回；否则返回 catchCode 代码块的执行结果。</returns>
        public static T? TryCatchExecute<T>(Func<T?> tryCode, Func<T?> catchCode, bool throwCatchError = false) where T : struct
        {
            T? ret = null;
            try
            {
                ret = tryCode();
            }
            catch
            {
                if (throwCatchError)
                {
                    ret = catchCode();
                }
                else
                {
                    ret = TryCatchExecute(catchCode);
                }
            }
            if (ret == null)
            {
                return null;
            }
            else
            {
                return ret.Value;
            }
        }

        /// <summary>
        /// 以 try...catch... 语句体方式执行一个具有返回值的操作方法。
        /// 如果 tryCode 代码块出现异常，则执行 catchCode 代码块。
        /// 参数 throwCatchError 指定是否再次屏蔽 catchCode 代码块中可能抛出的异常。
        /// </summary>
        /// <typeparam name="T">操作方法的返回值类型。</typeparam>
        /// <param name="tryCode">要执行的操作方法。</param>
        /// <param name="catchCode">如果 tryCode 代码块出现异常，则执行 catchCode 代码块。</param>
        /// <param name="throwCatchError">指定是否再次屏蔽 catchCode 代码块中可能抛出的异常。默认为 false 表示即使 catchCode 中出现异常也 try...catch...处理掉。</param>
        /// <returns>如果方法 <paramref name="tryCode"/> 顺利执行成功，则将其执行结果返回；否则返回 catchCode 代码块的执行结果。</returns>
        public static T? TryCatchExecute<T>(Func<T?> tryCode, Action catchCode, bool throwCatchError = false) where T : struct
        {
            T? ret = null;
            try
            {
                ret = tryCode();
            }
            catch
            {
                if (throwCatchError)
                {
                    catchCode();
                }
                else
                {
                    TryCatchExecute(catchCode);
                }
            }
            if (ret == null)
            {
                return null;
            }
            else
            {
                return ret.Value;
            }
        }


    }
}
