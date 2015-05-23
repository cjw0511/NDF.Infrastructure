using NDF.ConfigFile;
using NDF.Data.Entity.MasterSlaves.ConfigFile;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.MasterSlaves
{
    /// <summary>
    /// 表示应用程序默认配置文件（Web.config/App.config）或自定义配置文件中的 EF 读写分离服务配置文件信息。
    /// </summary>
    public class DbMasterSlaveConfig : AppConfig
    {
        private const bool _Default_UseDefaultConfigurationFile = false;
        private const bool _Default_EnableConfigChangedEvent = true;
        private const string _Default_ConfigFile = "~/ef.masterslave.config";
        private const string _Default_SectionName = "ef.masterslave";

        private Lazy<bool> _UseDefaultConfigurationFile = new Lazy<bool>(GetUseDefaultConfigurationFile);
        private Lazy<bool> _EnableConfigChangedEvent = new Lazy<bool>(GetEnableConfigChangedEvent);
        private Lazy<string> _ConfigFile = new Lazy<string>(GetConfigFile);
        private Lazy<string> _SectionName = new Lazy<string>(GetSectionName);

        private static readonly Lazy<DbMasterSlaveConfig> _instance = new Lazy<DbMasterSlaveConfig>(() => new DbMasterSlaveConfig());


        /// <summary>
        /// 初始化类型 <see cref="DbMasterSlaveConfig"/> 的新实例。
        /// </summary>
        private DbMasterSlaveConfig()
            : base()
        {
        }


        /// <summary>
        /// 获取类型 <see cref="DbMasterSlaveConfig"/> 的唯一默认实例。
        /// </summary>
        public static DbMasterSlaveConfig DefaultInstance
        {
            get { return _instance.Value; }
        }


        /// <summary>
        /// 获取一个字符串值，表示应用程序配置文件（Web.config/App.config 或自定义的配置文件）中的 EF 数据库主从读写分离服务配置节名称。
        /// </summary>
        protected string SectionName
        {
            get { return this._SectionName.Value; }
        }


        /// <summary>
        /// 获取当前 EF 读写分离服务的配置文件信息所定义的配置根节点。
        /// </summary>
        public EFMasterSlaveSection EFMasterSlaveSection
        {
            get
            {
                DbMasterSlaveConfig config = _instance.Value;
                return config.Configuration.Sections[config.SectionName] as EFMasterSlaveSection;
            }
        }

        /// <summary>
        ///  获取当前 EF 读写分离服务的配置文件信息所定义数据库读写分离连接信息项所构成的集合。
        /// </summary>
        public ApplyItemCollection ApplyItems
        {
            get
            {
                EFMasterSlaveSection section = this.EFMasterSlaveSection;
                if (section == null)
                {
                    string message = string.Format("当前应用程序的 EF 读写分离服务配置文件定义有误，不包含名称为 {0} 的相关节点，或没有为该节点在 'configuration/configSections' 节点中定义其绑定的配置类型。", _instance.Value.SectionName);
                    throw new ConfigurationErrorsException(message);
                }
                return section.ApplyItems;
            }
        }


        /// <summary>
        /// 根据 EF 数据库上下文类型获取当前应用程序的 EF 读写分离服务配置文件中其所对应的 <see cref="ApplyItemElement"/> 对象。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ApplyItemElement GetApplyItem(System.Data.Entity.DbContext context)
        {
            ApplyItemCollection applyItems = this.ApplyItems;
            this.ValidateApplyItemSection(applyItems);
            return applyItems.GetApplyItem(context);
        }

        /// <summary>
        /// 根据 EF 数据库上下文对象获取当前应用程序的 EF 读写分离服务配置文件中其所对应的 <see cref="ApplyItemElement"/> 对象。
        /// </summary>
        /// <param name="contextType"></param>
        /// <returns></returns>
        public ApplyItemElement GetApplyItem(Type contextType)
        {
            ApplyItemCollection applyItems = this.ApplyItems;
            this.ValidateApplyItemSection(applyItems);
            return applyItems.GetApplyItem(contextType);
        }


        private void ValidateApplyItemSection(ApplyItemCollection applyItems)
        {
            if (applyItems == null || applyItems.Count == 0)
            {
                throw new ConfigurationErrorsException("当前应用程序的 EF 读写分离服务配置文件定义有误，不包含任何的读写分离数据库连接配置映射项。");
            }
        }



        #region 抽象类型 AppConfigBase 中的抽象属性重写

        /// <summary>
        /// 定义一个抽象属性，表示是否使用当前应用程序的默认配置文件来获取 <see cref="Configuration"/> 配置对象。
        /// </summary>
        protected override bool UseDefaultConfigurationFile
        {
            get { return this._UseDefaultConfigurationFile.Value; }
        }

        /// <summary>
        /// 表示当不使用当前应用程序的默认配置文件来获取 <see cref="Configuration"/> 配置对象时（即 <see cref="UseDefaultConfigurationFile"/> 属性值为 false），使用的自定义
        /// <para>配置文件路径（相对路径或者绝对路径均可，如果返回相对于根目录的路径，请以 "~/" 作为前缀，例如 "~/ef.masterslave.config"）。</para>
        /// <para>注意：此属性仅当属性 <see cref="UseDefaultConfigurationFile"/> 返回 false 时才有效，如果属性 <see cref="UseDefaultConfigurationFile"/> 返回 true，则重写该属性时返回任意值均可（因为已被认为是无效值）。</para>
        /// </summary>
        protected override string ConfigurationFile
        {
            get { return this._ConfigFile.Value; }
        }

        /// <summary>
        /// 如果该属性值返回 true，则当所使用的配置文件（系统默认配置文件 Web.config/App.config 或由属性 <see cref="ConfigurationFile"/> 定义的配置文件，具体取决于属性 <see cref="UseDefaultConfigurationFile"/> 和属性 <see cref="ConfigurationFile"/> 的返回值）内容发生变更时：
        /// <para>1、将会重新设置当前对象的 <see cref="AppConfig.Configuration"/> 属性。</para>
        /// <para>2、将会同时引发当前对象的 <see cref="AppConfig.ConfigChanged"/> 事件。</para>
        /// <para>通过该属性的定义可以实现以热插拔的方式修改应用程序的配置文件。</para>
        /// </summary>
        protected override bool EnableConfigChangedEvent
        {
            get { return this._EnableConfigChangedEvent.Value; }
        }

        #endregion


        #region 初始化对象内部的延迟加载字段

        private static bool GetUseDefaultConfigurationFile()
        {
            string value = ConfigurationManager.AppSettings["ef.masterslave.useDefaultConfigurationFile"];
            return value.ToBoolean(_Default_UseDefaultConfigurationFile);
        }

        private static bool GetEnableConfigChangedEvent()
        {
            string value = ConfigurationManager.AppSettings["ef.masterslave.enableConfigChangedEvent"];
            return value.ToBoolean(_Default_EnableConfigChangedEvent);
        }

        private static string GetConfigFile()
        {
            string value = ConfigurationManager.AppSettings["ef.masterslave.configFile"];
            return string.IsNullOrEmpty(value) ? _Default_ConfigFile : value.Trim();
        }

        private static string GetSectionName()
        {
            string value = ConfigurationManager.AppSettings["ef.masterslave.sectionName"];
            return string.IsNullOrEmpty(value) ? _Default_SectionName : value.Trim();
        }

        #endregion

    }
}
