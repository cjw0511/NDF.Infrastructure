using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace NDF.Data.EntityFramework.MasterSlaves
{
    /// <summary>
    /// 表示包含 <see cref="SlaveServer"/> 对象的集合。
    /// </summary>
    public class SlaveServerCollection : Collection<SlaveServer>
    {

        /// <summary>
        /// 初始化类型 <see cref="SlaveServerCollection"/> 的新实例。
        /// </summary>
        internal SlaveServerCollection()
        {
        }


        /// <summary>
        /// 将元素插入 <see cref="SlaveServerCollection"/> 的指定索引处。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void InsertItem(int index, SlaveServer item)
        {
            Check.NotNull(item);
            ValidateSlaveConnectionString(item);
            base.InsertItem(index, item);
        }

        /// <summary>
        /// 替换指定索引处的元素。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void SetItem(int index, SlaveServer item)
        {
            Check.NotNull(item);
            ValidateSlaveConnectionString(item);
            base.SetItem(index, item);
        }


        private void ValidateSlaveConnectionString(SlaveServer slave)
        {
            SlaveServer server = this.FirstOrDefault(item => string.Equals(slave.ConnectionString, item.ConnectionString, StringComparison.InvariantCultureIgnoreCase));
            if (server != null)
                throw new InvalidOperationException(string.Format("当前集合中已存在 ConnectionString 值为 {0} 的 SlaveServer 元素。", slave.ConnectionString));
        }


    }
}
