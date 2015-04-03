using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.ComponentModel.Paging
{
    /// <summary>
    /// 表示分页结果显示界面中的单页数据内容，该类型可以用于对泛型集合数据进行分页处理。
    /// </summary>
    /// <typeparam name="T">分页数据对象 <see cref="PagingData&lt;T&gt;"/> 存放的数据的类型。</typeparam>
    public class PagingData<T> : PagingData
    {
        /// <summary>
        /// 初始化一个空数据的 <see cref="PagingData&lt;T&gt;"/> 对象。
        /// </summary>
        public PagingData() : base() { }

        /// <summary>
        /// 以指定的页面数据内容作为参数初始化 <see cref="PagingData&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="data">表示页面数据内容。</param>
        public PagingData(IEnumerable<T> data)
            : this()
        {
            this.Data = data;
        }

        /// <summary>
        /// 以指定的页面数据总行数和页面数据内容作为参数初始化 <see cref="PagingData&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="rowCount">表示页面数据总行数。</param>
        /// <param name="data">表示页面数据内容。</param>
        public PagingData(int rowCount, IEnumerable<T> data)
            : this(data)
        {
            this.RowCount = rowCount;
        }

        /// <summary>
        /// 以指定的页面尺寸、页面数据总行数和页面数据内容作为参数初始化 <see cref="PagingData&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示页面索引号，从 0 开始计数。</param>
        /// <param name="rowCount">表示页面数据总行数。</param>
        /// <param name="data">表示页面数据内容。</param>
        public PagingData(int pageIndex, int rowCount, IEnumerable<T> data)
            : this(rowCount, data)
        {
            this.PageIndex = pageIndex;
        }



        /// <summary>
        /// 以指定的页面数据总行数作为参数初始化 <see cref="PagingData&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="rowCount">表示页面数据总行数。</param>
        public PagingData(int rowCount)
            : this()
        {
            this.RowCount = rowCount;
        }

        /// <summary>
        /// 以指定的页面尺寸、页面数据总该行数作为参数初始化 <see cref="PagingData&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示页面索引号，从 0 开始计数。</param>
        /// <param name="rowCount">表示页面数据总行数。</param>
        public PagingData(int pageIndex, int rowCount)
            : this(rowCount)
        {
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// 以指定的页面尺寸、页面索引号、页面数据总行数作为参数初始化 <see cref="PagingData&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示页面索引号，从 0 开始计数。</param>
        /// <param name="pageSize">表示页面的尺寸，即一页有多少行数据。</param>
        /// <param name="rowCount">表示页面数据总行数。</param>
        public PagingData(int pageIndex, int pageSize, int rowCount)
            : this(pageIndex, rowCount)
        {
            this.PageSize = pageSize;
        }

        /// <summary>
        /// 以指定的页面尺寸、页面索引号、页面数据总行数和页面数据内容作为参数初始化 <see cref="PagingData&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示页面索引号，从 0 开始计数。</param>
        /// <param name="pageSize">表示页面的尺寸，即一页有多少行数据。</param>
        /// <param name="rowCount">表示页面数据总行数。</param>
        /// <param name="data">表示页面数据内容。</param>
        public PagingData(int pageIndex, int pageSize, int rowCount, IEnumerable<T> data)
            : this(pageIndex, rowCount, data)
        {
            this.PageSize = pageSize;
        }



        /// <summary>
        /// 获取分页数据对象 <see cref="PagingData"/> 所表示的页面数据内容。
        /// </summary>
        public virtual new IEnumerable<T> Data
        {
            get
            {
                return base.Data == null ? Enumerable.Empty<T>() : base.Data as IEnumerable<T>;
            }
            set { base.Data = value; }
        }

    }
}
