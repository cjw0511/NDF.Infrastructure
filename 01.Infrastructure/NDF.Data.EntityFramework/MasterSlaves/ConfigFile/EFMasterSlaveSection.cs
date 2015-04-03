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
    /// 表示 EF 读写分离服务配置文件信息中的配置根节点。
    /// </summary>
    public class EFMasterSlaveSection : ConfigurationSection
    {
        private const string _Section_Name = "ef.masterslave";

        private static readonly ConfigurationProperty _applyItems;
        private static readonly ConfigurationPropertyCollection _properties;


        /// <summary>
        /// 初始化类型 <see cref="EFMasterSlaveSection"/> 的新实例。
        /// </summary>
        public EFMasterSlaveSection()
        {
        }


        /// <summary>
        /// 获取配置集合中为特定的 EF 数据库上下文类型 <paramref name="dbContextType"/> 定义的 <see cref="ApplyItemCollection"/> 对象。
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <returns></returns>
        public ApplyItemSection this[Type dbContextType]
        {
            get { return this.ApplyItems[dbContextType]; }
        }

        /// <summary>
        /// 获取配置集合中为特定的 EF 数据库上下文对象 <paramref name="context"/> 定义的 <see cref="ApplyItemCollection"/> 对象。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ApplyItemSection this[System.Data.Entity.DbContext context]
        {
            get { return this.ApplyItems[context]; }
        }


        /// <summary>
        /// 获取 <see cref="EFMasterSlaveSection"/> 对象的 <see cref="ApplyItemCollection"/> 集合。
        /// </summary>
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ApplyItemCollection ApplyItems
        {
            get { return base[_applyItems] as ApplyItemCollection; }
        }


        /// <summary>
        /// 获取 <see cref="EFMasterSlaveSection"/> 对象的属性集合 <see cref="ConfigurationPropertyCollection"/> 对象。
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }


        static EFMasterSlaveSection()
        {
            _properties = new ConfigurationPropertyCollection();
            _applyItems = new ConfigurationProperty("", typeof(ApplyItemCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
            _properties.Add(_applyItems);
        }
        

    }
}
