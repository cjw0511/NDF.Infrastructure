using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.MasterSlaves.ConfigFile
{
    /// <summary>
    /// 表示 EF 读写分离服务配置文件信息中的 查询（读取）操作（即 Slave 服务器节点） 数据库服务连接参数配置信息。
    /// </summary>
    public class SlaveConnectionStringElement : ConnectionStringElement
    {
        private const string _Order_Key = "order";


        /// <summary>
        /// 初始化类型 <see cref="SlaveConnectionStringElement"/> 的新实例。
        /// </summary>
        public SlaveConnectionStringElement()
        {
        }


        /// <summary>
        /// 获取该配置对象所定义的排序号。
        /// </summary>
        [ConfigurationProperty(_Order_Key, DefaultValue = 0)]
        public int Order
        {
            get { return (int)this[_Order_Key]; }
            set { this[_Order_Key] = value; }
        }


    }
}
