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
    /// 如果需要定义当调用完成后可自动释放资源的类型（例如支持 using () {...} 代码块自动释放资源的类型），可继承该抽象类。
    /// 如果需要使用该基类创建可自动释放资源的对象类型，请在具体实现子类的默认/非默认构造函数中，将其中要随 <see cref="NDF.Disposable"/> 基类/及其子类对象销毁而自动销毁释放资源的内部对象添加至 <seealso cref="Container"/> 容器中。
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        private bool _disposed = false;
        private Container container;

        /// <summary>
        /// 初始化 <see cref="NDF.Disposable"/> 类的新实例。
        /// </summary>
        protected Disposable()
        {
        }


        /// <summary>
        /// 获取 System.ComponentModel.IContainer，它包含 System.ComponentModel.Component。
        /// </summary>
        protected virtual IContainer Container
        {
            get
            {
                if (this.container == null)
                {
                    this.container = new Container();
                }
                return this.container;
            }
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
                this.eventHandler(this.Disposing);
                if (disposing)
                {
                    this.Container.Dispose();
                }
                this._disposed = true;
                this.eventHandler(this.Disposed);
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



        private void eventHandler(EventHandler handler)
        {
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }



        /// <summary>
        /// 析构函数，用于销毁对象释放资源。
        /// </summary>
        ~Disposable()
        {
            this.Dispose(false);
        }

    }
}
