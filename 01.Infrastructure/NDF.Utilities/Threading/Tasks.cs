using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Threading
{
    /// <summary>
    /// 提供一组用于多线程和任务操作的工具 API。
    /// </summary>
    public static class Tasks
    {
        private static readonly Task _defaultCompleted = Task.FromResult<AsyncVoid>(default(AsyncVoid));
        private static readonly Task _completedTaskReturningNull = Task.FromResult<object>(null);


        /// <summary>
        /// 返回一个被取消执行的任务；
        /// 返回的 <see cref="Task"/> 对象表示已经被取消执行，其 IsCanceled = true，IsFaulted = False。
        /// </summary>
        /// <returns></returns>
        public static Task Canceled()
        {
            return Canceled<AsyncVoid>();
        }

        /// <summary>
        /// 返回一个被取消执行的任务，该任务具有给定的返回类型；
        /// 返回的 <see cref="Task"/> 对象表示已经被取消执行，其 IsCanceled = true，IsFaulted = False。
        /// </summary>
        /// <returns></returns>
        public static Task<TResult> Canceled<TResult>()
        {
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
            tcs.SetCanceled();
            return tcs.Task;
        }



        /// <summary>
        /// 返回一个表示执行完成的任务；
        /// 返回的 <see cref="Task"/> 对象表示已经被执行完成，其 IsCompleted = true，IsFaulted = False。
        /// </summary>
        /// <returns></returns>
        public static Task Completed()
        {
            return _defaultCompleted;
        }

        /// <summary>
        /// 返回一个表示执行完成的任务，该任务具有给定的返回类型；
        /// 返回的 <see cref="Task"/> 对象表示已经被执行完成，其 IsCompleted = true，IsFaulted = False。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static Task<TResult> Completed<TResult>()
        {
            return Task.FromResult<TResult>(default(TResult));
        }



        /// <summary>
        /// 返回一个表示执行异常的任务；
        /// 返回的 <see cref="Task"/> 对象表示执行过程中发生异常，其 IsCanceled = False, IsFaulted = True。
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static Task FromError(Exception exception)
        {
            return FromError<AsyncVoid>(exception);
        }

        /// <summary>
        /// 返回一个表示执行异常的任务，该任务具有给定的返回类型；
        /// 返回的 <see cref="Task"/> 对象表示执行过程中发生异常，其 IsCanceled = False, IsFaulted = True。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static Task<TResult> FromError<TResult>(Exception exception)
        {
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(exception);
            return tcs.Task;
        }



        /// <summary>
        /// 返回一个表示没有返回结果的任务；
        /// </summary>
        /// <returns></returns>
        public static Task NullResult()
        {
            return _completedTaskReturningNull;
        }

        /// <summary>
        /// 返回一个表示虽然定义了返回类型但是返回结果为 Null 值的任务。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static Task<TResult> NullResult<TResult>() where TResult : class
        {
            return Task.FromResult<TResult>(null);
        }




        //public static Task AttachWith(Task task, Action completation, Action faulttation, Action cancellation)
        //{

        //}

    }
}
