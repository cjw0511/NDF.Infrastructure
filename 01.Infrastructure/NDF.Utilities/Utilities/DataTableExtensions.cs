using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 数据表格对象 <see cref="System.Data.DataTable"/> 操作方法的扩展。
    /// </summary>
    public static class DataTableExtensions
    {


        /// <summary>
        /// 获取数据表格中所有列的名称所构成的一个数组。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string[] GetColumnNames(this DataTable _this)
        {
            Check.NotNull(_this);

            DataColumnCollection columns = _this.Columns;
            string[] names = new string[columns.Count];

            for (int i = 0; i < columns.Count; i++)
            {
                names[i] = columns[i].ColumnName;
            }
            return names;
        }



        /// <summary>
        /// 将数据表格对象 <see cref="DataTable"/> 转换成一个包含元素类型为 dynamic(动态解析对象) 的列表。
        /// 列表中的每个元素都是一个 dynamic(动态解析对象) 对象，该对象中所有的动态属性名称和值对应于表格中行的数据项。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static List<dynamic> ToList(this DataTable _this)
        {
            Check.NotNull(_this);
            List<dynamic> list = new List<dynamic>();
            foreach (DataRow row in _this.Rows)
            {
                list.Add(row.ToObject());
            }
            return list;
        }

        /// <summary>
        /// 将数据表格对象 <see cref="DataTable"/> 转换成一个包含元素类型为 <typeparamref name="TResult"/> 的列表。
        /// 列表中的每个元素都是一个 <typeparamref name="TResult"/> 类型对象，该对象中所有的动态属性值对应于表格中行的数据项。
        /// 对于不能转换的数据项（例如指定的 <typeparamref name="TResult"/> 类型中没有该数据项所示名称的属性、或者数据类型不匹配），将不会包含在返回的结果中。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static List<TResult> ToList<TResult>(this DataTable _this) where TResult : class, new()
        {
            Check.NotNull(_this);
            List<TResult> list = new List<TResult>();
            foreach (DataRow row in _this.Rows)
            {
                list.Add(row.ToObject<TResult>());
            }
            return list;
        }


    }
}
