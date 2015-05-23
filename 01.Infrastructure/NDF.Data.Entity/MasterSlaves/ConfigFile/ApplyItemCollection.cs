using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.MasterSlaves.ConfigFile
{
    /// <summary>
    /// 表示一个包含 <see cref="ApplyItemElement"/> 配置元素的集合。
    /// </summary>
    public class ApplyItemCollection : ConfigurationElementCollection
    {
        private const string _Element_Name = "applyItem";


        /// <summary>
        /// 初始化类型 <see cref="ApplyItemCollection"/> 的新实例。
        /// </summary>
        public ApplyItemCollection()
        {
        }


        /// <summary>
        /// 获取或设置位于集合中指定索引处的 <see cref="ApplyItemElement"/> 对象。
        /// </summary>
        /// <param name="index">集合中 <see cref="ApplyItemElement"/> 对象的索引。</param>
        /// <returns></returns>
        public ApplyItemElement this[int index]
        {
            get { return (ApplyItemElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// 根据 EF 数据库上下文类型获取其所对应的 <see cref="ApplyItemElement"/> 对象。
        /// <para>如果没找到 <paramref name="targetContextType"/> 对应的 <see cref="ApplyItemElement"/> 对象，该索引器属性将返回 null。</para>
        /// </summary>
        /// <param name="targetContextType"></param>
        /// <returns></returns>
        public ApplyItemElement this[Type targetContextType]
        {
            get
            {
                Check.NotNull(targetContextType);
                foreach (ConfigurationElement c in this)
                {
                    var section = c as ApplyItemElement;
                    if (section == null)
                        continue;

                    if (section.CanApplyTo(targetContextType))
                        return section;
                }
                return null;
            }
        }

        /// <summary>
        /// 根据 EF 数据库上下文对象获取其所对应的 <see cref="ApplyItemElement"/> 对象。
        /// <para>如果没找到 <paramref name="context"/> 对应的 <see cref="ApplyItemElement"/> 对象，该索引器属性将返回 null。</para>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ApplyItemElement this[System.Data.Entity.DbContext context]
        {
            get
            {
                Check.NotNull(context);
                Type contextType = context.GetType();
                return this[contextType];
            }
        }



        /// <summary>
        /// 获取 <see cref="ApplyItemCollection"/> 的类型。
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
        /// 根据 EF 数据库上下文类型获取其所对应的 <see cref="ApplyItemElement"/> 对象。
        /// </summary>
        /// <param name="targetContextType"></param>
        /// <returns></returns>
        public ApplyItemElement GetApplyItem(Type targetContextType)
        {
            return this[targetContextType];
        }

        /// <summary>
        /// 根据 EF 数据库上下文对象获取其所对应的 <see cref="ApplyItemElement"/> 对象。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ApplyItemElement GetApplyItem(System.Data.Entity.DbContext context)
        {
            return this[context];
        }



        /// <summary>
        /// 向 ApplyItemCollection 添加配置元素。
        /// </summary>
        /// <param name="element"></param>
        protected override void BaseAdd(System.Configuration.ConfigurationElement element)
        {
            if (!this.ValidateApplyItemSection(element))
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
            if (!this.ValidateApplyItemSection(element))
            {
                base.BaseAdd(index, element);
            }
        }


        private bool ValidateApplyItemSection(ConfigurationElement element)
        {
            var key = this.GetElementKey(element);
            ApplyItemElement applyItem = (ApplyItemElement)BaseGet(key);
            if (applyItem != null && applyItem.TargetContextFullName != ((ApplyItemElement)element).TargetContextFullName)
            {
                throw new InvalidOperationException(string.Format("EF 读写分离服务配置中已经存在表示相同 {0} 的配置项。", applyItem.TargetContextFullName));
            }
            return applyItem != null;
        }


        /// <summary>
        /// 创建一个新的 <see cref="ApplyItemElement"/>。
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ApplyItemElement();
        }

        /// <summary>
        /// 获取指定配置元素的元素键。
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ApplyItemElement)element).TargetContextFullName;
        }


    }
}
