using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.Edm
{
    /// <summary>
    /// 提供一组对 <see cref="System.Data.Entity.Core.Metadata.Edm.EntitySet"/> 对象的扩展方法。
    /// </summary>
    public static class EntitySetExtensions
    {
        /// <summary>
        /// 获取 <see cref="System.Data.Entity.Core.Metadata.Edm.EntitySet"/> 对象所表示的数据库表的名称。
        /// </summary>
        /// <param name="set"><see cref="System.Data.Entity.Core.Metadata.Edm.EntitySet"/> 对象。</param>
        /// <returns>返回 <see cref="System.Data.Entity.Core.Metadata.Edm.EntitySet"/> 对象所表示的数据库表的名称。</returns>
        public static string GetTableName(this EntitySet set)
        {
            return set.MetadataProperties.Contains("Table") && set.MetadataProperties["Table"].Value != null
                ? set.MetadataProperties["Table"].Value.ToString()
                : set.Name;
        }

    }
}
