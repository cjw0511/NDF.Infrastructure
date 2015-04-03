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
        /// 找出 <paramref name="command"/> 脚本中定义的参数列表，并将参数列表加入其属性 Parameters 中。
        /// </summary>
        /// <param name="command">用于查找参数列表的 <see cref="DbCommand"/> 对象。</param>
        public virtual void DiscoverParameters(DbCommand command)
        {
            DatabaseExtensions.DiscoverParameters(this.PrimitiveDatabase, command);
        }
    }
}
