using NDF.Composition;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NDF.ServiceProcess
{
    /// <summary>
    /// 定义一个 Windows 服务基类。
    /// <para>该类型将自动承载应用程序中所有通过 MEF 组件管理和定义的 <see cref="IServiceUnit"/> 组件作为其工作任务项。</para>
    /// 
    /// <para>该类型通过 MEF 组件从应用程序中加载所有指定的 <see cref="IServiceUnit"/> 组件作为其工作任务项：</para>
    /// <para>
    /// 1、该类型的子类需实现抽象方法 <see cref="ServiceMainBase.ServiceKey"/> 以返回相应的服务对象键值；
    /// </para>
    /// <para>
    /// 2、所有被加载作为服务工作项的 <see cref="IServiceUnit"/> 接口实现类应定义特性类 <see cref="System.ComponentModel.Composition.ExportAttribute"/>，并且实例应
    /// 定义属性 <see cref="IServiceUnit.ServiceKey"/> 的返回值，该返回值与指定的 <see cref="ServiceMainBase"/> 实例的 <see cref="ServiceMainBase.ServiceKey"/> 属性值相等
    /// 时，该 <see cref="IServiceUnit"/> 接口实例将会被对应的 <see cref="ServiceMainBase"/> 实例所加载并使用。
    /// </para>
    /// </summary>
    public abstract class ServiceMainBase : ServiceBase
    {
        private System.ComponentModel.IContainer _components;
        private Lazy<IEnumerable<IServiceUnit>> _serviceUnits;


        #region 构造函数定义

        /// <summary>
        /// 初始化类型 <see cref="ServiceMainBase"/> 的新实例。
        /// </summary>
        public ServiceMainBase()
        {
            this.InitializeComponent();
            this.InitializeServiceUnits();
        }

        #endregion



        #region 获取当前服务对象中承载的工作任务项和其他相关属性定义

        /// <summary>
        /// 定义一个抽象属性，表示当前服务类型的键值。
        /// <para>该键值用于 MEF 组件加载服务工作任务项时进行匹配。</para>
        /// <para>
        /// 当在子类实现中重写了该属性返回值时，该类型实例能够自动通过 MEF 组件加载所有的 <see cref="IServiceUnit"/> 接口实现类，并找出
        /// 其中 <see cref="IServiceUnit.ServiceKey"/> 属性值与当前类型的 <see cref="ServiceMainBase.ServiceKey"/> 属性值相同的实例作为
        /// 服务工作任务项加载并运行。
        /// </para>
        /// <para>注意：如果该属性值返回 Null、空字符串或者纯空格字符，则该类型实例将不会加载任何的 <see cref="IServiceUnit"/> 工作任务项。</para>
        /// </summary>
        public abstract string ServiceKey
        {
            get;
        }

        /// <summary>
        /// 获取应用程序中为当前服务类设定的所有工作任务项。
        /// <para>该属性中的所有元素实际是基于 MEF 组件自动从应用程序中引入的所有 DLL 中加载而来。</para>
        /// <para>MEF 组件加载匹配的规则见当前类型的说明。</para>
        /// </summary>
        protected IEnumerable<IServiceUnit> ServiceUnits
        {
            get { return this._serviceUnits.Value; }
        }


        /// <summary>
        /// 获取当前服务的名称。
        /// <para>该方法返回的值必须与相应的安装程序类的 <see cref="ServiceInstaller.ServiceName"/> 属性中为该服务记录的名称相同。</para>
        /// </summary>
        /// <returns></returns>
        protected abstract string GetServiceName();

        #endregion


        #region 服务基类 ServiceBase 中的调度方法重写

        /// <summary>
        /// 指定服务启动时采取的操作。
        /// <para>该动作一般在下列情况执行：在下列情况下执行：在“服务控制管理器”(SCM) 向服务发送“开始”命令时，或者在操作系统启动时（对于自动启动的服务）。</para>
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            this.ServiceUnitsEach(unit => unit.Start(args));
        }

        /// <summary>
        /// 指定要在服务暂停时采取的操作。
        /// <para>该动作一般在下列情况执行：“服务控制管理器”(SCM) 将“暂停”命令发送到服务时。</para>
        /// </summary>
        protected override void OnPause()
        {
            this.ServiceUnitsEach(unit => unit.Pause());
        }

        /// <summary>
        /// 指定要在服务暂停后恢复正常功能时采取的操作。
        /// <para>该动作一般在下列情况执行：“服务控制管理器”(SCM) 将“继续”命令发送到服务时。</para>
        /// </summary>
        protected override void OnContinue()
        {
            this.ServiceUnitsEach(unit => unit.Continue());
        }

        /// <summary>
        /// 指定服务停止运行时采取的操作。
        /// <para>该动作一般在下列情况执行：“服务控制管理器”(SCM) 将“停止”命令发送到服务时。</para>
        /// </summary>
        protected override void OnStop()
        {
            this.ServiceUnitsEach(unit => unit.Stop());
        }

        /// <summary>
        /// 该方法指定应在系统即将关闭前执行的处理。
        /// <para>该动作一般在下列情况执行：操作系统即将关闭时（如果执行操作系统关闭时该任务已被手动停止，则不执行）。</para>
        /// </summary>
        protected override void OnShutdown()
        {
            this.ServiceUnitsEach(unit => unit.Shutdown());
        }


        private void ServiceUnitsEach(Action<IServiceUnit> action)
        {
            Check.NotNull(action);
            foreach (IServiceUnit unit in this.ServiceUnits)
            {
                action(unit);
            }
        }

        #endregion


        #region 当前服务对象中承载的工作任务项初始化定义

        [ImportMany]
        private IEnumerable<Lazy<IServiceUnit>> ServiceUnitsWithNonServiceKey
        {
            get;
            set;
        }


        private void InitializeServiceUnits()
        {
            Compositions.ComposeParts(this);
            this.ResetServiceUnits();
        }

        private void ResetServiceUnits()
        {
            this._serviceUnits = new Lazy<IEnumerable<IServiceUnit>>(this.GetServiceUnits);
        }

        private IEnumerable<IServiceUnit> GetServiceUnits()
        {
            if (string.IsNullOrWhiteSpace(this.ServiceKey))
                return Enumerable.Empty<IServiceUnit>();

            var items = from item in this.ServiceUnitsWithNonServiceKey ?? Enumerable.Empty<Lazy<IServiceUnit>>()
                        where item != null && item.Value != null &&
                            string.Equals(item.Value.ServiceKey ?? string.Empty, this.ServiceKey, StringComparison.OrdinalIgnoreCase)
                        select item.Value;

            return items.ToArray();
        }

        #endregion


        #region 内部方法定义

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this._components != null))
            {
                this._components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._components = new Container();
            this.ServiceName = this.GetServiceName();
        }

        #endregion

    }
}
