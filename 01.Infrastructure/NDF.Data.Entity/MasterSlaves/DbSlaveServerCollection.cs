using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace NDF.Data.Entity.MasterSlaves
{
    /// <summary>
    /// 表示包含 <see cref="DbSlaveServer"/> 对象的集合。
    /// </summary>
    public class DbSlaveServerCollection : Collection<DbSlaveServer>
    {

        /// <summary>
        /// 初始化类型 <see cref="DbSlaveServerCollection"/> 的新实例。
        /// </summary>
        public DbSlaveServerCollection()
        {
        }


        /// <summary>
        /// 将元素插入 <see cref="DbSlaveServerCollection"/> 的指定索引处。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void InsertItem(int index, DbSlaveServer item)
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
        protected override void SetItem(int index, DbSlaveServer item)
        {
            Check.NotNull(item);
            ValidateSlaveConnectionString(item);
            base.SetItem(index, item);
        }


        private void ValidateSlaveConnectionString(DbSlaveServer slave)
        {
            DbSlaveServer server = this.FirstOrDefault(item => string.Equals(slave.ConnectionString, item.ConnectionString, StringComparison.OrdinalIgnoreCase));
            if (server != null)
                throw new InvalidOperationException(string.Format("当前集合中已存在 ConnectionString 值为 {0} 的 SlaveServer 元素。", slave.ConnectionString));
        }


    }
}
