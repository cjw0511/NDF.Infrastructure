using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.IO
{
    /// <summary>
    /// 提供一组用于操作文件或者目录路径信息的 API，该类型可作为类型 <see cref="System.IO.Path"/> 的功能补充使用。
    /// </summary>
    public static class Paths
    {

        /// <summary>
        /// 返回指定路径字符串的完整物理路径。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">如果 <paramref name="path"/> 值为 Null、空或者空白字符串组成，则抛出该异常。</exception>
        public static string GetPhysicalPath(string path)
        {
            Check.NotEmpty(path);
            string temp = path.Trim();
            string physicalPath = path;

            if (temp.StartsWith("~/") || temp.StartsWith("~\\"))
            {
                string realPath = path.Substring(1);
                string directory = AppDomain.CurrentDomain.BaseDirectory;
                physicalPath = directory + realPath;
            }
            return Path.GetFullPath(physicalPath);
        }

    }
}
