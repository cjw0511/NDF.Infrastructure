using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.DB2.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// 表示 IBM DB2 数据库配置信息。
    /// </summary>
    public class DB2DatabaseData : DatabaseData
    {
        /// <summary>
        /// 以指定的连接字符串命名连接配置和配置源回调函数初始化一个 <see cref="DB2DatabaseData"/> 实例。
        /// </summary>
        /// <param name="connectionStringSettings">连接字符串命名连接配置。</param>
        /// <param name="configurationSource">配置源回调函数。</param>
        public DB2DatabaseData(ConnectionStringSettings connectionStringSettings, Func<string, ConfigurationSection> configurationSource)
            : base(connectionStringSettings, configurationSource)
        {
        }


        /// <summary>
        /// 基于当前的数据库配置信息创建一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 实例。
        /// </summary>
        /// <returns>基于当前数据库配置的 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 实例。</returns>
        public override Microsoft.Practices.EnterpriseLibrary.Data.Database BuildDatabase()
        {
            return new DB2Database(this.ConnectionString);
        }
    }
}
