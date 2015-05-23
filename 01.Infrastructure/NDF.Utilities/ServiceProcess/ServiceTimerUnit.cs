using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NDF.ServiceProcess
{
    /// <summary>
    /// 表示一个包含按固定时间间隔自动执行任务的 Windows 服务工作项。
    /// <para>该类型封装一个类型为 <see cref="System.Timers.Timer"/> 的定时器工作任务项。</para>
    /// <para>
    /// 在定义该类型子类时同时为其添加特性类 <see cref="System.ComponentModel.Composition.ExportAttribute"/>，并且重写了属性 <see cref="ServiceMainBase.ServiceKey"/> 时，可使之仅能
    /// 被指定的 <see cref="ServiceMainBase"/> 类型子类实例（该 <see cref="ServiceMainBase"/> 实例时同时定义了相对应的 <see cref="ServiceMainBase.ServiceKey"/> 属性）使用。
    /// </para>
    /// </summary>
    public abstract class ServiceTimerUnit : ServiceUnitBase
    {
        private Timer _timer;


        #region 构造函数定义

        /// <summary>
        /// 初始化类型 <see cref="ServiceTimerUnit"/> 的实例，并将所有属性设置为初始值。
        /// </summary>
        public ServiceTimerUnit()
        {
            this._timer = new Timer();
            this.InitializeTimer();
            this.Disposing += ServiceTimerUnit_Disposing;
        }

        /// <summary>
        /// 初始化类型 <see cref="ServiceTimerUnit"/> 的实例，并将 <see cref="Interval"/> 属性设置为指定的毫秒数。
        /// </summary>
        /// <param name="interval"></param>
        public ServiceTimerUnit(double interval)
        {
            this._timer = new Timer(interval);
            this.InitializeTimer();
            this.Disposing += ServiceTimerUnit_Disposing;
        }

        #endregion



        #region 组件内 Timer 定时器相关属性定义

        /// <summary>
        /// 获取当前组件所使用的 <see cref="System.Timers.Timer"/> 定时器对象。
        /// </summary>
        public Timer Timer
        {
            get { return this._timer; }
        }

        /// <summary>
        /// 获取或设置当前工作任务的定时器任务执行间隔时间（毫秒）。
        /// <para>如果当前对象的 <see cref="Enabled"/> 属性值为 true，则设置该参数将会抛出 <see cref="System.ArgumentException"/> 异常。</para>
        /// </summary>
        public double Interval
        {
            get { return this._timer.Interval; }
            set { this._timer.Interval = value; }
        }


        /// <summary>
        /// 获取一个布尔值，表示该工作任务是否处于启用状态。
        /// <para>启用状态的动作任务将会在指定的时间间隔执行相应动作。</para>
        /// </summary>
        public bool Enabled
        {
            get { return this._timer.Enabled; }
        }

        #endregion


        #region 类型 ServiceUnitBase 的方法体重写

        /// <summary>
        /// 启动定时器任务执行。
        /// </summary>
        /// <param name="args"></param>
        public override void Start(string[] args)
        {
            this._timer.Start();
        }

        /// <summary>
        /// 暂停定时器任务执行。
        /// </summary>
        public override void Pause()
        {
            this._timer.Stop();
        }

        /// <summary>
        /// 继续定时器任务执行。
        /// </summary>
        public override void Continue()
        {
            this._timer.Start();
        }

        /// <summary>
        /// 结束定时器任务执行。
        /// </summary>
        public override void Stop()
        {
            this._timer.Stop();
        }

        /// <summary>
        /// 结束定时器任务执行。
        /// </summary>
        public override void Shutdown()
        {
            this._timer.Stop();
        }

        #endregion


        #region 内部方法定义

        /// <summary>
        /// 定义当前工作任务中定时器对象 <see cref="Timer"/> 中在指定时间间隔中需要执行的 任务/动作。
        /// </summary>
        /// <param name="singalTime"></param>
        protected abstract void ServiceWork(DateTime singalTime);


        private void InitializeTimer()
        {
            this._timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.ServiceWork(e.SignalTime);
        }


        private void ServiceTimerUnit_Disposing(object sender, EventArgs e)
        {
            if (this._timer != null)
            {
                this._timer.Dispose();
            }
        }

        #endregion


    }
}
