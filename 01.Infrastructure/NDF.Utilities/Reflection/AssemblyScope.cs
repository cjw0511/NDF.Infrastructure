using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Reflection
{
    /// <summary>
    /// 表示应用程序集的作用范围。
    /// </summary>
    [Flags]
    public enum AssemblyScope
    {
        /// <summary>
        /// 表示调用当前正在执行的方法的方法的程序集。
        /// </summary>
        Calling = 1,

        /// <summary>
        /// 表示默认应用程序域中的进程可执行文件。 在其他的应用程序域中，这是由 AppDomain.ExecuteAssembly 执行的第一个可执行文件
        /// </summary>
        Entry = 2,

        /// <summary>
        /// 表示包含当前执行的代码的程序集。
        /// </summary>
        Executing = 4,

        /// <summary>
        /// 表示整个应用程序域下所有已经加载的程序集。
        /// </summary>
        Global = 8,
    }
}
