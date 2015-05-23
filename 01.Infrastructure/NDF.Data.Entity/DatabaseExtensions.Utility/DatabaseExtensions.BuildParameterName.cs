using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NDF.Data.Entity
{
    public static partial class DatabaseExtensions
    {

        /// <summary>
        /// 根据指明的变量名称，创建一个适用于当前数据库类型（依据当前对象的 <see cref="DbProviderFactory"/> 属性所创建的 <see cref="DbConnection"/>）的变量参数名称。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <param name="name">指明的用于包装变量参数名称。</param>
        /// <returns>一个表示可用于表示当前数据库类型（依据当前对象的 <see cref="DbProviderFactory"/> 属性所创建的 <see cref="DbConnection"/>）的变量参数名称。</returns>
        public static string BuildParameterName(this Database database, string name)
        {
            return GetGeneralDatabase(database).BuildParameterName(name);
        }

    }
}
