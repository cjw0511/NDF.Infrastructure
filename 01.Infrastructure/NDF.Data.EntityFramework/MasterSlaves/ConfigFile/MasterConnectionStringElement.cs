using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework.MasterSlaves.ConfigFile
{
    /// <summary>
    /// 表示 EF 读写分离服务配置文件信息中的 增删改（写入）操作（即 Master 服务器节点） 数据库服务连接参数配置信息。
    /// </summary>
    public class MasterConnectionStringElement : ConnectionStringElement
    {

        /// <summary>
        /// 初始化类型 <see cref="MasterConnectionStringElement"/> 的新实例。
        /// </summary>
        public MasterConnectionStringElement()
        {
        }


    }
}
