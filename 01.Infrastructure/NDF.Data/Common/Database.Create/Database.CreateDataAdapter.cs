using NDF.Data.EnterpriseLibrary;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Common
{
    public abstract partial class Database
    {
        /// <summary>
        /// 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。
        /// </summary>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbDataAdapter"/> 对象。</returns>
        public virtual DbDataAdapter CreateDataAdapter()
        {
            return DatabaseExtensions.CreateDataAdapter(this.PrimitiveDatabase);
        }
    }
}
