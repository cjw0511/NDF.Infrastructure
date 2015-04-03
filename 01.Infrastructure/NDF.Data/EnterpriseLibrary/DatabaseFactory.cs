using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EnterpriseLibrary
{
    /// <summary>
    /// 提供一组工具方法，用于快速创建或操作 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。
    /// </summary>
    public static class DatabaseFactory
    {
        /// <summary>
        /// 使用 App.Config 或 Web.Config 中 root/ConnectionStrings 节点内定义的配置项名称创建一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。
        /// </summary>
        /// <param name="connectionStringName">App.Config 或 Web.Config 中 ConnectionStrings 节点内定义的数据库链接配置项名称（注意，配置的 ConnectionString 不能被加密，并且需定义 ProviderName，否则默认为 System.Data.SqlClient）。</param>
        /// <returns>返回一个新创建的 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象</returns>
        public static Microsoft.Practices.EnterpriseLibrary.Data.Database CreateDatabase(string connectionStringName)
        {
            return Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase(connectionStringName);
        }

        /// <summary>
        /// 创建一个默认的 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。
        /// </summary>
        /// <returns>返回一个新创建的 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</returns>
        /// <remarks>
        /// 该方法将使用 App.Config 或 Web.Config 中 root/dataConfiguration 节点内 defaultDatabase 属性定义的值作为 ConnectioStringName。
        /// 如需在 App.Config 或 Web.Config 中的 root 节点内定义 dataConfiguration 节点，需先在 root/configSections 内定义：
        ///     &lt;section name="dataConfiguration" type="Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings, Microsoft.Practices.EnterpriseLibrary.Data" requirePermission="true" /&gt;。
        /// </remarks>
        public static Microsoft.Practices.EnterpriseLibrary.Data.Database CreateDatabase()
        {
            return Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase();
        }
    }
}
