using NDF.IO;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.ConfigFile
{
    /// <summary>
    /// 定义用于读取应用程序默认配置文件（Web.config/App.config）或自定义配置文件中配置信息的基类。
    /// </summary>
    public abstract class AppConfig : Disposable
    {
        private Lazy<string> _physicalPath;
        private Configuration _config;
        private FileSystemWatcher _watcher;


        /// <summary>
        /// 初始化类型 <see cref="AppConfig"/> 的新实例。
        /// </summary>
        protected AppConfig()
        {
            this.InitializeConfig();
            this.InitializeFileWatcher();
            this.Disposing += AppConfig_Disposing;
        }



        /// <summary>
        /// 获取当前对象用于获取 <see cref="Configuration"/> 配置对象的配置文件完整物理路径。
        /// <para></para>
        /// </summary>
        public string ConfigurationFilePhysicalPath
        {
            get { return this._physicalPath.Value; }
        }

        /// <summary>
        /// 获取当前实例包含的基础配置对象定义。
        /// </summary>
        public Configuration Configuration
        {
            get { return this._config; }
        }


        /// <summary>
        /// 定义一个抽象属性，表示是否使用当前应用程序的默认配置文件来获取 <see cref="Configuration"/> 配置对象。
        /// </summary>
        protected abstract bool UseDefaultConfigurationFile
        {
            get;
        }

        /// <summary>
        /// 定义一个抽象方法，表示当不使用当前应用程序的默认配置文件来获取 <see cref="Configuration"/> 配置对象时（即 <see cref="UseDefaultConfigurationFile"/> 属性值为 false），使用的自定义
        /// <para>配置文件路径（相对路径或者绝对路径均可，如果返回相对于根目录的路径，请以 "~/" 作为前缀，例如 "~/custome.config"）。</para>
        /// <para>注意：此属性仅当属性 <see cref="UseDefaultConfigurationFile"/> 返回 false 时才有效，如果属性 <see cref="UseDefaultConfigurationFile"/> 返回 true，则重写该属性时返回任意值均可（因为已被认为是无效值）。</para>
        /// </summary>
        protected abstract string ConfigurationFile
        {
            get;
        }

        /// <summary>
        /// 定义一个抽象属性，如果该属性值返回 true，则当所使用的配置文件（系统默认配置文件 Web.config/App.config 或由属性 <see cref="ConfigurationFile"/> 定义的配置文件，具体取决于属性 <see cref="UseDefaultConfigurationFile"/> 和属性 <see cref="ConfigurationFile"/> 的返回值）内容发生变更时：
        /// <para>1、将会重新设置当前对象的 <see cref="AppConfig.Configuration"/> 属性。</para>
        /// <para>2、将会同时引发当前对象的 <see cref="AppConfig.ConfigChanged"/> 事件。</para>
        /// <para>通过该属性的定义可以实现以热插拔的方式修改应用程序的配置文件。</para>
        /// </summary>
        protected abstract bool EnableConfigChangedEvent
        {
            get;
        }



        #region 定义当配置文件更改时引发的事件

        private void InitializeFileWatcher()
        {
            FileInfo config = new FileInfo(this.Configuration.FilePath);
            string filename = config.Name;
            string path = config.Directory.FullName;

            this._watcher = new FileSystemWatcher(path, filename);
            this._watcher.Created += _watcher_Created;
            this._watcher.Changed += _watcher_Changed;
            this._watcher.EnableRaisingEvents = true;
        }

        private void _watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (this.EnableConfigChangedEvent)
            {
                this.ResetConfiguration();
                this.OnConfigChanged();
            }
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (this.EnableConfigChangedEvent)
            {
                this.ResetConfiguration();
                this.OnConfigChanged();
            }
        }


        /// <summary>
        /// 触发当前对象的 <see cref="ConfigChanged"/> 事件。
        /// <para>该操作同时会导致重新设置当前对象的 <see cref="AppConfig.Configuration"/> 属性。</para>
        /// </summary>
        protected void OnConfigChanged()
        {
            if (this.ConfigChanged != null)
            {
                this.ConfigChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 当前对象所使用的配置文件（系统默认配置文件 Web.config/App.config 或由属性 <see cref="ConfigurationFile"/> 定义的配置文件，具体取决于属性 <see cref="UseDefaultConfigurationFile"/> 和属性 <see cref="ConfigurationFile"/> 的返回值）内容发生变更时将会触发该事件。
        /// <para>注意：此事件属性定义仅当属性 <see cref="EnableConfigChangedEvent"/> 返回 true 时才有效。</para>
        /// </summary>
        public event EventHandler ConfigChanged;

        #endregion


        #region 初始化默认配置对象

        private void InitializeConfig()
        {
            this._physicalPath = new Lazy<string>(this.GetCurrentConfigurationPhysicalPath);
            this.ResetConfiguration();
        }

        private string GetCurrentConfigurationPhysicalPath()
        {
            return Paths.GetPhysicalPath(this.ConfigurationFile);
        }


        private void ResetConfiguration()
        {
            this._config = this.GetCurrentConfiguration();
        }

        private Configuration GetCurrentConfiguration()
        {
            if (this.UseDefaultConfigurationFile)
            {
                return GetConfiguration(DefaultConfigurationFile);
            }
            string path = this.ConfigurationFilePhysicalPath;
            if (IsDefaultConfigurationFile(path))
            {
                throw new ConfigurationErrorsException("当前设定为不使用系统默认配置文件来获取 System.Configuration.Configuration 对象，但是使用的配置文件路径（ConfigurationFilePhysicalPath）却为系统默认配置文件地址。");
            }
            return GetConfiguration(path);
        }

        #endregion


        #region 定义一组公共静态工具方法

        /// <summary>
        /// 获取当前应用程序的默认配置文件完整路径。
        /// </summary>
        public static string DefaultConfigurationFile
        {
            get { return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; }
        }

        /// <summary>
        /// 判断指定的文本路径值是否等效于当前应用程序的默认配置文件完整路径。
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static bool IsDefaultConfigurationFile(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
                return false;

            string path1 = Path.GetFullPath(DefaultConfigurationFile);
            string path2 = Path.GetFullPath(filepath);
            return string.Equals(path1, path2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 获取指定路径所表示的计算机配置文件 <see cref="Configuration"/> 对象。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Configuration GetConfiguration(string path)
        {
            string physicalPath = Paths.GetPhysicalPath(path);
            ConfigurationFileMap fileMap = new ConfigurationFileMap(physicalPath);
            return ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
        }

        #endregion


        #region 定义一组方法以释放占用资源

        private void AppConfig_Disposing(object sender, EventArgs e)
        {
            if (this._watcher != null)
            {
                this._watcher.Dispose();
            }
        }

        #endregion

    }
}
