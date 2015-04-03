using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework
{
    public static partial class DatabaseExtensions
    {

        /// <summary>
        /// 找出 <paramref name="command"/> 脚本中定义的参数列表，并将参数列表加入其属性 Parameters 中。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <param name="command">用于查找参数列表的 <see cref="DbCommand"/> 对象。</param>
        public static void DiscoverParameters(this Database database, DbCommand command)
        {
            GetGeneralDatabase(database).DiscoverParameters(command);
        }

    }
}
