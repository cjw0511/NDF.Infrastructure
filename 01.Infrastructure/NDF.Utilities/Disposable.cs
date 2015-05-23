using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF
{
    /// <summary>
    /// 定义一种释放分配的资源的方法。该类型是对 <see cref="System.IDisposable"/> 接口简单实现的基类。
    /// <para>如果需要定义当调用完成后可自动释放资源的类型（例如支持 using () {...} 代码块自动释放资源的类型），可继承该抽象类。</para>
    /// <para>如果需要使用该基类创建可自动释放内部资源的对象类型，有如下三种方式：</para>
    /// <para>1、在具体实现子类的 默认/非默认 构造函数中，将其中要随 <see cref="NDF.Disposable"/> 基类/及其子类对象销毁而自动销毁释放资源的内部对象添加至 <seealso cref="Container"/> 容器中；</para>
    /// <para>2、（推荐）在子类的 默认/非默认 构造函数中，注册事件 <see cref="Disposable.Disposing"/> 或 <see cref="Disposable.Disposed"/> 以便进行资源释放；</para>
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        private bool _disposed = false;
        private Lazy<Container> _container = new Lazy<Container>(() => new Container());
        private readonly object _locker = new object();


        /// <summary>
        /// 初始化类型 <see cref="Disposable"/> 的新实例。
        /// </summary>
        protected Disposable()
        {
        }


        /// <summary>
        /// 获取 <see cref="System.ComponentModel.IContainer"/>，它包含 <see cref="System.ComponentModel.Component"/>。
        /// </summary>
        protected virtual IContainer Container
        {
            get { return this._container.Value; }
        }



        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            // 防止该对象被两次或多次执行 Dispose 方法。
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                lock (this._locker)
                {
                    this.OnDisposing();
                    if (disposing)
                    {
                        this._container.Value.Dispose();
                        this._container = null;
                    }
                    this._disposed = true;
                    this.OnDisposed();
                }
            }
        }


        /// <summary>
        /// 允许在子类中重写该方法，以触发 <see cref="Disposing"/> 事件。
        /// </summary>
        protected void OnDisposing()
        {
            if (this.Disposing != null)
            {
                this.Disposing(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 允许在子类中重写该方法，以触发 <see cref="Disposed"/> 事件。
        /// </summary>
        protected void OnDisposed()
        {
            if (this.Disposed != null)
            {
                this.Disposed(this, EventArgs.Empty);
            }
        }


        /// <summary>
        /// 在 <see cref="NDF.Disposable"/> 对象调用 Dispose 方法释放对象资源时 (在 Dispose 方法的开始位置) 发生。
        /// </summary>
        public event EventHandler Disposing;

        /// <summary>
        /// 在 <see cref="NDF.Disposable"/> 对象调用 Dispose 方法释放对象资源时 (在 Dispose 方法的结束位置) 发生。
        /// </summary>
        public event EventHandler Disposed;


        /// <summary>
        /// 析构函数，用于销毁对象释放资源。
        /// </summary>
        ~Disposable()
        {
            this.Dispose(false);
        }

    }
}
