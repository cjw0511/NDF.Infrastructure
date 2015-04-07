using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves.ConfigFile
{
    /// <summary>
    /// 表示 EF 读写分离服务配置文件信息中为某个 EF 实体数据库上下文配置的读写分离连接信息。
    /// </summary>
    public class ApplyItemSection : ConfigurationSection
    {
        private const string _TargetContext_Key = "targetContext";
        private const string _TargetContext_Default_Value = "";

        private const string _MasterConnectionString_Key = "master";
        private const string _SlaveConnectionStrings_Key = "slaves";

        private const string _AutoSwitchSlaveOnMasterFauled_Key = "autoSwitchSlaveOnMasterFauled";
        private const string _AutoSwitchMasterOnSlavesFauled_Key = "autoSwitchMasterOnSlavesFauled";
        private const string _SlaveRandomization_Key = "slaveRandomization";
        private const string _ServerStateScanInterval_Key = "serverStateScanInterval";
        private const string _ServerStateScanWithNoOffline_Key = "serverStateScanWithNoOffline";

        private Type _targetContextType;
        private readonly object _locker = new object();



        /// <summary>
        /// 初始化类型 <see cref="ApplyItemSection"/> 的新实例。
        /// </summary>
        public ApplyItemSection()
        {
        }

        /// <summary>
        /// 以指定的 EF 实体数据库上下文类型完整名称初始化类型 <see cref="ApplyItemSection"/> 的新实例。
        /// </summary>
        /// <param name="targetContextFullName"></param>
        public ApplyItemSection(string targetContextFullName)
            : this()
        {
            targetContextFullName = Check.EmptyCheck(targetContextFullName);
            if (!this.ValidateTargetContextType(targetContextFullName))
            {
                throw new ArgumentException(string.Format("指定的参数 {0} 不能表示一个有效的 {1} 类型。",
                    targetContextFullName,
                    typeof(System.Data.Entity.DbContext)));
            }
            this.TargetContext = targetContextFullName;
        }

        /// <summary>
        /// 以指定的 EF 实体数据库上下文类型初始化类型 <see cref="ApplyItemSection"/> 的新实例。
        /// </summary>
        /// <param name="targetContextType"></param>
        public ApplyItemSection(Type targetContextType)
            : this(targetContextType.FullName)
        {
        }



        /// <summary>
        /// 获取或设置该 <see cref="ApplyItemSection"/> 对象所表示的目标 EF 实体数据库上下文类型的（完整）名称。
        /// </summary>
        [ConfigurationProperty(_TargetContext_Key, DefaultValue = _TargetContext_Default_Value, Options = ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey)]
        public string TargetContext
        {
            get { return (string)base[_TargetContext_Key]; }
            set { base[_TargetContext_Key] = value; }
        }



        /// <summary>
        /// 读取或设置 <see cref="ApplyItemSection"/> 配置节中表示 EF 读写分离服务配置文件信息中的 增删改（写入）操作 数据库服务连接参数配置信息。
        /// </summary>
        [ConfigurationProperty(_MasterConnectionString_Key, IsRequired = true)]
        public MasterConnectionStringElement MasterConnectionString
        {
            get { return this[_MasterConnectionString_Key] as MasterConnectionStringElement; }
            set { this[_MasterConnectionString_Key] = value; }
        }

        /// <summary>
        /// 读取 <see cref="ApplyItemSection"/> 配置节中表示 EF 读写分离服务配置文件信息中的 查询（读取）操作（Slave 服务器节点） 数据库服务连接参数配置信息集合。
        /// </summary>
        [ConfigurationProperty(_SlaveConnectionStrings_Key, IsRequired = true)]
        public SlaveConnectionStringCollection SlaveConnectionStrings
        {
            get { return base[_SlaveConnectionStrings_Key] as SlaveConnectionStringCollection; }
        }



        /// <summary>
        /// 获取或设置一个布尔值属性，表示 EF 读写分离服务配置环境中是否允许当 Master 服务器离线后自动将 Slave 服务器节点作为 Master 服务器使用。
        /// <para>备注：Master 服务器提供数据写入功能，而 Slave 服务器提供数据只读功能。</para>
        /// <para>在 Master 服务器离线后，将 Slave 服务器作为 Master 服务器使用能使在 Master 故障后应用程序不离线，但是同样也会带来在 Slave 服务器节点之间的数据一致性问题。</para>
        /// <para>该属性默认值为 false。</para>
        /// </summary>
        [ConfigurationProperty(_AutoSwitchSlaveOnMasterFauled_Key, DefaultValue = false)]
        public bool AutoSwitchSlaveOnMasterFauled
        {
            get { return (bool)this[_AutoSwitchSlaveOnMasterFauled_Key]; }
            set { this[_AutoSwitchSlaveOnMasterFauled_Key] = value; }
        }

        /// <summary>
        /// 获取或设置一个布尔值属性，表示 EF 读写分离服务配置环境中是否允许当所有的 Slave 服务器离线后自动将 Master 服务器节点作为 Slave 服务器使用。
        /// <para>备注：Master 服务器提供数据写入功能，而 Slave 服务器提供数据只读功能。</para>
        /// <para>在 Slave 服务器离线后，将 Master 服务器作为 Slave 服务器使用虽然一定程度上会增大 Master 服务器的查询压力，但是可以避免应用程序离线。</para>
        /// <para>该属性默认值为 true。</para>
        /// </summary>
        [ConfigurationProperty(_AutoSwitchMasterOnSlavesFauled_Key, DefaultValue = true)]
        public bool AutoSwitchMasterOnSlavesFauled
        {
            get { return (bool)this[_AutoSwitchMasterOnSlavesFauled_Key]; }
            set { this[_AutoSwitchMasterOnSlavesFauled_Key] = value; }
        }

        /// <summary>
        /// 获取或设置一个布尔值属性，表示 EF 读写分离服务配置环境中是否在每次执行 Query 请求时随机选择 <see cref="SlaveConnectionStrings"/> 集合中的任意一台可用的 Slave 服务器节点作为查询服务器使用。
        /// <para>如果该属性定位为 false，EF 读写分离服务将在每次执行 Query 请求时按照 <see cref="SlaveConnectionStringElement.Order"/> 属性定义的值（如果未定义该属性则按配置节的定义顺序）的（按最小值）优先顺序选择  <see cref="SlaveConnectionStrings"/> 集合中的 <see cref="SlaveConnectionStringElement"/> 元素。</para>
        /// <para>该属性默认值为 true。</para>
        /// </summary>
        [ConfigurationProperty(_SlaveRandomization_Key, DefaultValue = true)]
        public bool SlaveRandomization
        {
            get { return (bool)this[_SlaveRandomization_Key]; }
            set { this[_SlaveRandomization_Key] = value; }
        }


        /// <summary>
        /// 获取或设置一个 int 数值属性，表示当 EF 读写分离服务配置环境中定义了多个数据库服务器节点时，系统轮询扫描检测每个服务器节点可用状态的时间间隔。
        /// <para>该属性值单位为 秒。</para>
        /// <para>如果该属性值定义为 0，则表示不轮询扫描每服务器节点的在线状态。</para>
        /// <para>该属性默认值为 0，即表示不开启服务器节点的在线状态扫描功能。</para>
        /// <para>该属性值不能小于 30，如果被设定为一个小于 30 的值，将会自动取值 30。</para>
        /// </summary>
        [ConfigurationProperty(_ServerStateScanInterval_Key, DefaultValue = 0)]
        public int ServerStateScanInterval
        {
            get { return Math.Max((int)this[_ServerStateScanInterval_Key], 30); }
            set { this[_ServerStateScanInterval_Key] = value; }
        }

        /// <summary>
        /// 获取或设置一个布尔值属性，表示 EF 读写分离服务配置环境中是否在每次轮询扫描检测每个服务器节点可用状态时，是否排除检测已经标记为离线的数据库服务器节点。
        /// <para>该属性默认值为 false，即每次检测时都检测已配置的所有数据库服务器节点。</para>
        /// </summary>
        [ConfigurationProperty(_ServerStateScanWithNoOffline_Key, DefaultValue = true)]
        public bool ServerStateScanWithNoOffline
        {
            get { return (bool)this[_ServerStateScanWithNoOffline_Key]; }
            set { this[_ServerStateScanWithNoOffline_Key] = value; }
        }



        /// <summary>
        /// 获取该 <see cref="ApplyItemSection"/> 对象所表示的目标 EF 实体数据库上下文类型。
        /// <para>如果该对象的 <see cref="TargetContext"/> 属性未定义，则该属性将返回 null。</para>
        /// <para>如果该对象的 <see cref="TargetContext"/> 属性定义不正确，则该读取属性可能会导致异常。</para>
        /// </summary>
        public Type TargetContextType
        {
            get
            {
                lock (this._locker)
                {
                    if (this._targetContextType == null)
                    {
                        if (string.IsNullOrEmpty(this.TargetContext))
                        {
                            this._targetContextType = null;
                            return this._targetContextType;
                        }

                        Type targetContextType = Type.GetType(this.TargetContext);
                        if (!this.ValidateTargetContextType(targetContextType))
                        {
                            throw new InvalidOperationException(string.Format("设定的类型 {0} 不能表示一个有效的 {1} 类型。",
                                targetContextType,
                                typeof(System.Data.Entity.DbContext)));
                        }
                        _targetContextType = targetContextType;
                    }
                    return this._targetContextType;
                }
            }
        }

        /// <summary>
        /// 获取该 <see cref="ApplyItemSection"/> 对象所表示的目标 EF 实体数据库上下文类型的完整名称。
        /// <para>如果该对象的 <see cref="TargetContext"/> 属性未定义，则该属性将返回 null。</para>
        /// <para>如果该对象的 <see cref="TargetContext"/> 属性定义不正确，则该读取属性可能会导致异常。</para>
        /// </summary>
        internal string TargetContextFullName
        {
            get
            {
                Type type = this.TargetContextType;
                return type != null ? type.FullName : null;
            }
        }



        /// <summary>
        /// 确定当前对象的 <see cref="TargetContextType"/> 所示类型能否应用于指定的 EF 实体数据库上下文对象。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool CanApplyTo(System.Data.Entity.DbContext context)
        {
            if (context == null)
                return false;

            return this.CanApplyTo(context.GetType());
        }

        /// <summary>
        /// 确定当前对象的 <see cref="TargetContextType"/> 所示类型能否应用于指定的 EF 实体数据库上下文。
        /// </summary>
        /// <param name="contextType"></param>
        /// <returns></returns>
        public virtual bool CanApplyTo(Type contextType)
        {
            Type targetContextType = this.TargetContextType;
            return this.ValidateTargetContextType(contextType) &&
                (contextType == targetContextType || contextType.IsSubclassOf(targetContextType));
        }

        /// <summary>
        /// 确定当前对象的 <see cref="TargetContextType"/> 所示类型能否应用于指定的 EF 实体数据库上下文。
        /// </summary>
        /// <param name="contextTypeFullName"></param>
        /// <returns></returns>
        public virtual bool CanApplyTo(string contextTypeFullName)
        {
            Type contextType = Type.GetType(contextTypeFullName);
            return this.CanApplyTo(contextType);
        }


        /// <summary>
        /// 确认 <paramref name="targetContextFullName"/> 参数所示的 <see cref="Type"/> 对象是否等效或继承于类型 <see cref="System.Data.Entity.DbContext"/>。
        /// </summary>
        /// <param name="targetContextFullName"></param>
        /// <returns></returns>
        private bool ValidateTargetContextType(string targetContextFullName)
        {
            targetContextFullName = Check.EmptyCheck(targetContextFullName);
            Type targetContextType = Type.GetType(targetContextFullName);
            return this.ValidateTargetContextType(targetContextType);
        }

        /// <summary>
        /// 确认 <paramref name="targetContextType"/> 参数所示的 <see cref="Type"/> 对象是否等效或继承于类型 <see cref="System.Data.Entity.DbContext"/>。
        /// </summary>
        /// <param name="targetContextType"></param>
        /// <returns></returns>
        private bool ValidateTargetContextType(Type targetContextType)
        {
            if (targetContextType == null)
                return false;

            Type contextType = typeof(System.Data.Entity.DbContext);
            return targetContextType == contextType || targetContextType.IsSubclassOf(contextType);
        }


    }
}
