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
        /// 找出 <paramref name="command"/> 脚本中定义的参数列表，并将 <paramref name="parameterValues"/> 作为参数值分配给该参数列表。
        /// </summary>
        /// <param name="command">用于查找参数列表的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="parameterValues">用于作为参数值分配给 <paramref name="command"/> 参数列表的值集合。</param>
        public virtual void AssignParameters(DbCommand command, params object[] parameterValues)
        {
            DatabaseExtensions.AssignParameters(this.PrimitiveDatabase, command, parameterValues);
        }
    }
}
