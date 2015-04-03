using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.Annotations
{
    /// <summary>
    /// 表示 EntityFramework 环境中的 实体对象模型类型 与 数据库中对应表的信息。
    /// </summary>
    public class EntityTable
    {
        /// <summary>
        /// 表示 <see cref="EntityTable"/> 实体表对象映射至 EF 中的 ModelSet 对象。
        /// </summary>
        public EntitySet ModelSet { get; set; }

        /// <summary>
        /// 表示 <see cref="EntityTable"/> 实体表对象映射至 EF 中的 StoreSet 对象。
        /// </summary>
        public EntitySet StoreSet { get; set; }

        /// <summary>
        /// 表示 <see cref="EntityTable"/> 实体表对象映射至 EF 中的 ModelType 对象。
        /// </summary>
        public EntityType ModelType { get; set; }

        /// <summary>
        /// 表示 <see cref="EntityTable"/> 实体表对象映射至 EF 中的 StoreType 对象。
        /// </summary>
        public EntityType StoreType { get; set; }

        /// <summary>
        /// 表示 <see cref="EntityTable"/> 实体表对象映射至 EF 中的 EntityType 对象。
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 表示 <see cref="EntityTable"/> 实体表对象映射至 EF 中的数据库表名。
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表示 <see cref="EntityTable"/> 实体表对象映射至 EF 中的数据库架构名。
        /// </summary>
        public string Schema { get; set; }


    }
}
