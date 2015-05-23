using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.ServiceProcess
{
    /// <summary>
    /// 表示 Windows 服务应用程序中服务工作单元的基类。
    /// <para>可以理解为一个服务任务，包含 开始、暂停、继续、停止 等动作的调度项。</para>
    /// <para>
    /// 在定义该类型子类时同时为其添加特性类 <see cref="System.ComponentModel.Composition.ExportMetadataAttribute"/>（并指定 Name 为 "ServiceKey" 和对应的 Value 值），可使之仅能
    /// 被指定的 <see cref="ServiceMainBase"/> 类型子类实例（初始化实例时指定了相对应的 <see cref="ServiceMainBase.ServiceKey"/> 参数）使用。
    /// </para>
    /// </summary>
    public abstract class ServiceUnitBase : Disposable, IServiceUnit
    {

        /// <summary>
        /// 初始化类型 <see cref="ServiceUnitBase"/> 的新实例。
        /// </summary>
        public ServiceUnitBase()
        {
        }


        /// <summary>
        /// 返回服务工作单元的服务键值。该属性作为该类型实例被 MEF 组件自动加载时的查找匹配键使用时，请在子类实现时重写该属性以返回
        /// 与 <see cref="ServiceMainBase"/> 子类宿主实例的 <see cref="ServiceMainBase.ServiceKey"/> 属性相同的值。
        /// <para>
        /// 当该抽象类的子类声明时定义了特性 <see cref="System.ComponentModel.Composition.ExportAttribute"/> 时，该实现类的实例
        /// 将会被类型 <see cref="ServiceMainBase"/> 的子类实例按照 <see cref="ServiceMainBase.ServiceKey"/> 与该属性以相同匹配的方式自动查找并加载。
        /// </para>
        /// </summary>
        public virtual string ServiceKey
        {
            get { return string.Empty; }
        }


        /// <summary>
        /// 指定服务启动时采取的操作。
        /// <para>该动作一般在下列情况执行：在下列情况下执行：在“服务控制管理器”(SCM) 向服务发送“开始”命令时，或者在操作系统启动时（对于自动启动的服务）。</para>
        /// </summary>
        /// <param name="args"></param>
        public virtual void Start(string[] args)
        {
        }

        /// <summary>
        /// 指定要在服务暂停时采取的操作。
        /// <para>该动作一般在下列情况执行：“服务控制管理器”(SCM) 将“暂停”命令发送到服务时。</para>
        /// </summary>
        public virtual void Pause()
        {
        }

        /// <summary>
        /// 指定要在服务暂停后恢复正常功能时采取的操作。
        /// <para>该动作一般在下列情况执行：“服务控制管理器”(SCM) 将“继续”命令发送到服务时。</para>
        /// </summary>
        public virtual void Continue()
        {
        }

        /// <summary>
        /// 指定服务停止运行时采取的操作。
        /// <para>该动作一般在下列情况执行：“服务控制管理器”(SCM) 将“停止”命令发送到服务时。</para>
        /// </summary>
        public virtual void Stop()
        {
        }

        /// <summary>
        /// 该方法指定应在系统即将关闭前执行的处理。
        /// <para>该动作一般在下列情况执行：操作系统即将关闭时（如果执行操作系统关闭时该任务已被手动停止，则不执行）。</para>
        /// </summary>
        public virtual void Shutdown()
        {
        }

    }
}
