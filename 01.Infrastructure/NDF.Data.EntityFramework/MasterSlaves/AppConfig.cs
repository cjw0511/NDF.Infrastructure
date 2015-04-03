using NDF.Data.EntityFramework.MasterSlaves.ConfigFile;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves
{
    /// <summary>
    /// 表示 web.config/app.config 或自定义配置文件中的 EF 读写分离服务配置文件信息。
    /// </summary>
    public class AppConfig
    {
        public const string _EF_MASTER_SLAVE_CONFIG_FILE = "~/ef.masterslave.config";
        public const string _EF_MASTER_SLAVE_SECTION_NAME = "ef.masterslave";

        public const string _EF_MASTER_SLAVE_CONFIG_FILE_KEY = "ef:masterslave.config";
        public const string _EF_MASTER_SLAVE_USE_DEFAULT_CONFIG_FILE_KEY = "ef:masterslave.useDefaultConfigFile";

        private static object _shared_locker = new object();

        private static string _configFileFullPath;
        private static string _configFile;
        private static bool? _useDefaultConfigFile;

        private static AppConfig _defaultInstance = new AppConfig();
        private Configuration _config;
        private FileSystemWatcher _watcher;



        /// <summary>
        /// 初始化类型 <see cref="AppConfig"/> 的新实例。
        /// </summary>
        private AppConfig()
        {
            this.RefreshConfigCache();
            this.InitializeConfigWatcher();
        }



        /// <summary>
        /// 获取类型 <see cref="AppConfig"/> 的唯一默认实例。
        /// </summary>
        public static AppConfig DefaultInstance
        {
            get { return _defaultInstance; }
        }



        /// <summary>
        /// 获取当前 EF 读写分离服务的配置文件信息。
        /// </summary>
        public Configuration Configuration
        {
            get { return this._config; }
        }

        /// <summary>
        /// 判断当前 EF 读写分离服务所使用的配置文件是否为当前应用程序的默认配置文件。
        /// </summary>
        public bool IsDefaultConfig
        {
            get { return IsDefaultConfigFile(this.Configuration.FilePath); }
        }



        /// <summary>
        /// 获取当前 EF 读写分离服务的配置文件信息所定义的配置根节点。
        /// </summary>
        public EFMasterSlaveSection EFMasterSlaveSection
        {
            get { return this.Configuration.Sections[_EF_MASTER_SLAVE_SECTION_NAME] as EFMasterSlaveSection; }
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
                    throw new ConfigurationErrorsException(string.Format("当前应用程序的 EF 读写分离服务配置文件定义有误，不包含名称为 {0} 的相关节点，或没有为该节点在 'configuration/configSections' 节点中定义其绑定的配置类型。", _EF_MASTER_SLAVE_SECTION_NAME));
                }
                return section.ApplyItems;
            }
        }



        /// <summary>
        /// 根据 EF 数据库上下文类型获取当前应用程序的 EF 读写分离服务配置文件中其所对应的 <see cref="ApplyItemSection"/> 对象。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ApplyItemSection GetApplyItem(System.Data.Entity.DbContext context)
        {
            ApplyItemCollection applyItems = this.ApplyItems;
            this.ValidateApplyItemSection(applyItems);
            return applyItems.GetApplyItem(context);
        }

        /// <summary>
        /// 根据 EF 数据库上下文对象获取当前应用程序的 EF 读写分离服务配置文件中其所对应的 <see cref="ApplyItemSection"/> 对象。
        /// </summary>
        /// <param name="contextType"></param>
        /// <returns></returns>
        public ApplyItemSection GetApplyItem(Type contextType)
        {
            ApplyItemCollection applyItems = this.ApplyItems;
            this.ValidateApplyItemSection(applyItems);
            return applyItems.GetApplyItem(contextType);
        }



        /// <summary>
        /// 刷新当前 EF 读写分离服务所使用的配置文件信息。
        /// </summary>
        public void RefreshConfigCache()
        {
            this._config = GetCurrentConfig();
        }



        private void ValidateApplyItemSection(ApplyItemCollection applyItems)
        {
            if (applyItems == null || applyItems.Count == 0)
            {
                throw new ConfigurationErrorsException("当前应用程序的 EF 读写分离服务配置文件定义有误，不包含任何的读写分离数据库连接配置映射项。");
            }
        }



        #region EF 读写分离服务配置文件内容变更时引发的事件定义

        private void InitializeConfigWatcher()
        {
            if (this.IsDefaultConfig)
                return;

            FileInfo config = new FileInfo(this.Configuration.FilePath);
            string filename = config.Name;
            string path = config.Directory.FullName;

            this._watcher = new FileSystemWatcher(path, filename);
            this._watcher.Changed += _watcher_Changed;
            this._watcher.Created += _watcher_Created;
            this._watcher.EnableRaisingEvents = true;
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            this.RefreshConfigCache();
            this.OnConfigChanged();
        }
        
        private void _watcher_Created(object sender, FileSystemEventArgs e)
        {
            this.RefreshConfigCache();
            this.OnConfigChanged();
        }


        /// <summary>
        /// 触发当前对象的 <see cref="ConfigChanged"/> 事件。
        /// </summary>
        protected void OnConfigChanged()
        {
            if (this.ConfigChanged != null)
            {
                this.ConfigChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 当 EF 读写分离服务所使用的配置文件内容发生变更时将会触发该事件。
        /// </summary>
        public event EventHandler ConfigChanged;

        #endregion



        #region 读取配置文件相关操作

        /// <summary>
        /// 获取 Web.config/App.config 中 appSettings 配置集合下的 key: "ef:masterslave.config" 配置项（注意大小写）的值所表示的完整文件路径。
        /// 表示 EF 读写分离服务的配置文件名称；如果 appSettings 中未设定该参数，则取常量 <see cref="_EF_MASTER_SLAVE_CONFIG_FILE"/> 的值（值为 "~/ef.masterslave.config"）所表示的完整文件路径。
        /// </summary>
        public static string ConfigFilePhysicalPath
        {
            get
            {
                lock (_shared_locker)
                {
                    if (_configFileFullPath == null)
                    {
                        _configFileFullPath = GetPhysicalPath(ConfigFileRelativePath);
                    }
                    return _configFileFullPath;
                }
            }
        }

        /// <summary>
        /// 获取 Web.config/App.config 中 appSettings 配置集合下的 key: "ef:masterslave.config" 配置项（注意大小写）的值。
        /// 表示 EF 读写分离服务的配置文件的相对路径；如果 appSettings 中未设定该参数，则取常量 <see cref="_EF_MASTER_SLAVE_CONFIG_FILE"/> 的值（值为 "~/ef.masterslave.config"）。
        /// </summary>
        private static string ConfigFileRelativePath
        {
            get
            {
                if (_configFile == null)
                {
                    string value = ConfigurationManager.AppSettings[_EF_MASTER_SLAVE_CONFIG_FILE_KEY];
                    value = value != null ? value.Trim() : null;
                    _configFile = !string.IsNullOrEmpty(value) ? value : _EF_MASTER_SLAVE_CONFIG_FILE;
                }
                return _configFile;
            }
        }

        private static string GetPhysicalPath(string path)
        {
            path = Check.EmptyCheck(path);
            string physicalPath = path;
            if (path.StartsWith("~/") || path.StartsWith("~\\"))
            {
                string realPath = path.Substring(1);
                string directory = AppDomain.CurrentDomain.BaseDirectory;
                physicalPath = directory + realPath;
            }
            return Path.GetFullPath(physicalPath);
        }


        private static Configuration GetCurrentConfig()
        {
            bool useDefaultConfigFile = UseDefaultConfigFile;
            if (useDefaultConfigFile)
            {
                return GetConfig(DefaultConfigFile);
            }
            string path = ConfigFilePhysicalPath;
            if (IsDefaultConfigFile(path))
                throw new ConfigurationErrorsException("当前设定为不使用系统默认配置文件作为  EF 读写分离服务的配置文件，但是指定的配置文件路径却为系统默认配置文件地址。");

            return GetConfig(path);
        }

        private static Configuration GetConfig(string path)
        {
            path = Check.EmptyCheck(path);
            string fullpath = Path.GetFullPath(path);
            ConfigurationFileMap fileMap = new ConfigurationFileMap(fullpath);
            return ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
        }

        #endregion


        #region 获取系统默认配置文件定义相关操作

        /// <summary>
        /// 获取 Web.config/App.config 中 appSettings 配置集合下的 key: "ef:masterslave.useDefaultConfigFile" 配置项（注意大小写）的值。
        /// 表示是否使用当前应用程序的默认配置文件作为 EF 读写分离服务的配置文件；如果 appSettings 中未设定该参数，则取默认值 false。
        /// </summary>
        internal static bool UseDefaultConfigFile
        {
            get
            {
                if (!_useDefaultConfigFile.HasValue)
                {
                    string value = ConfigurationManager.AppSettings[_EF_MASTER_SLAVE_USE_DEFAULT_CONFIG_FILE_KEY];
                    _useDefaultConfigFile = value.ToBoolean(false);
                }
                return _useDefaultConfigFile.Value;
            }
        }

        /// <summary>
        /// 获取当前应用程序的默认配置文件完整路径。
        /// </summary>
        internal static string DefaultConfigFile
        {
            get { return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; }
        }

        /// <summary>
        /// 判断指定的文本路径值是否等效于当前应用程序的默认配置文件完整路径。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static bool IsDefaultConfigFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            string defaultConfig = DefaultConfigFile;
            return string.Equals(path, defaultConfig, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion


    }
}
