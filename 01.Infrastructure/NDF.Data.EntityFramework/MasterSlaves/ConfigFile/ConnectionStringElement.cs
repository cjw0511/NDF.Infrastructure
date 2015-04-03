using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves.ConfigFile
{
    /// <summary>
    /// 表示 EF 读写分离服务配置文件信息中的数据库服务连接参数配置信息。
    /// </summary>
    public abstract class ConnectionStringElement : ConfigurationElement
    {
        private const string _ConnectionString_Key = "connectionString";


        /// <summary>
        /// 初始化类型 <see cref="ConnectionStringElement"/> 的新实例。
        /// </summary>
        public ConnectionStringElement()
        {
        }


        /// <summary>
        /// 获取该配置对象所定义的数据库连接字符串。
        /// </summary>
        [ConfigurationProperty(_ConnectionString_Key, IsRequired = true)]
        public virtual string ConnectionString
        {
            get { return this[_ConnectionString_Key] as string; }
            set { this[_ConnectionString_Key] = value; }
        }


    }
}
