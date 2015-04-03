using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves.ConfigFile
{
    /// <summary>
    /// 表示一个包含 <see cref="SlaveConnectionStringElement"/> 集合的配置元素。
    /// </summary>
    public class SlaveConnectionStringCollection : ConfigurationElementCollection
    {
        private const string _Element_Name = "add";


        /// <summary>
        /// 初始化类型 <see cref="SlaveConnectionStringCollection"/> 的新实例。
        /// </summary>
        public SlaveConnectionStringCollection()
        {
        }


        /// <summary>
        /// 获取 <see cref="ConfigurationElementCollection"/> 的类型。
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// 获取用于标识配置文件中此元素集合的名称。
        /// </summary>
        protected override string ElementName
        {
            get { return _Element_Name; }
        }


        /// <summary>
        /// 向 <see cref="SlaveConnectionStringCollection"/> 添加配置元素。
        /// </summary>
        /// <param name="element"></param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            if (!this.ValidateSlaveConnectionStringElement(element))
            {
                base.BaseAdd(element);
            }
        }

        /// <summary>
        /// 向配置元素集合添加配置元素。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="element"></param>
        protected override void BaseAdd(int index, ConfigurationElement element)
        {
            if (!this.ValidateSlaveConnectionStringElement(element))
            {
                base.BaseAdd(index, element);
            }
        }


        private bool ValidateSlaveConnectionStringElement(ConfigurationElement element)
        {
            var key = this.GetElementKey(element);
            var slave = (SlaveConnectionStringElement)BaseGet(key);
            if (slave != null
                && slave.ConnectionString != ((SlaveConnectionStringElement)element).ConnectionString)
            {
                throw new InvalidOperationException("EF 读写分离服务配置节点下的 slaves 节点下已经存在表示相同数据库连接的配置项。");
            }
            return slave != null;
        }

        /// <summary>
        /// 创建一个新的 <see cref="SlaveConnectionStringElement"/>。
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new SlaveConnectionStringElement();
        }

        /// <summary>
        /// 获取指定配置元素的元素键。
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SlaveConnectionStringElement)element).ConnectionString;
        }


    }
}
