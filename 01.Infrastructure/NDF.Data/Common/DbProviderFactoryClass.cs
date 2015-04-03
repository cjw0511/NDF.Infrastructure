using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Common
{
    /// <summary>
    /// 用于描述数据库提供程序的信息。
    /// </summary>
    public class DbProviderFactoryClass
    {
        /// <summary>
        /// 表示数据提供程序的可识别名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表示数据提供程序的可识别描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 表示可以以编程方式用于引用数据提供程序的名称。
        /// </summary>
        public string InvariantName { get; set; }

        /// <summary>
        /// 表示工厂类的完全限定名，它包含用于实例化该对象的足够的信息。
        /// </summary>
        public string AssemblyQualifiedName { get; set; }
    }
}
