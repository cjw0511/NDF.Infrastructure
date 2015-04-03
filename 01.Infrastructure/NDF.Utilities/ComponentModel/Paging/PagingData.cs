using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.ComponentModel.Paging
{
    /// <summary>
    /// 表示分页结果显示界面中的单页数据内容。
    /// </summary>
    public class PagingData
    {
        private int _pageSize;
        private int _pageIndex;
        private int _rowCount;
        private IEnumerable _data;

        /// <summary>
        /// 初始化一个空数据的 <see cref="NDF.ComponentModel.Paging.PagingData"/> 对象。
        /// </summary>
        public PagingData() { }

        /// <summary>
        /// 以指定的页面数据内容作为参数初始化 <see cref="NDF.ComponentModel.Paging.PagingData"/> 对象。
        /// </summary>
        /// <param name="data">表示页面数据内容。</param>
        public PagingData(IEnumerable data)
            : this()
        {
            this.Data = data;
        }

        /// <summary>
        /// 以指定的页面数据总行数和页面数据内容作为参数初始化 <see cref="NDF.ComponentModel.Paging.PagingData"/> 对象。
        /// </summary>
        /// <param name="rowCount">表示页面数据总行数。</param>
        /// <param name="data">表示页面数据内容。</param>
        public PagingData(int rowCount, IEnumerable data)
            : this(data)
        {
            this.RowCount = rowCount;
        }

        /// <summary>
        /// 以指定的页面尺寸、页面数据总行数和页面数据内容作为参数初始化 <see cref="NDF.ComponentModel.Paging.PagingData"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示页面索引号，从 0 开始计数。</param>
        /// <param name="rowCount">表示页面数据总行数。</param>
        /// <param name="data">表示页面数据内容。</param>
        public PagingData(int pageIndex, int rowCount, IEnumerable data)
            : this(rowCount, data)
        {
            this.PageIndex = pageIndex;
        }



        /// <summary>
        /// 以指定的页面数据总行数作为参数初始化 <see cref="NDF.ComponentModel.Paging.PagingData"/> 对象。
        /// </summary>
        /// <param name="rowCount">表示页面数据总行数。</param>
        public PagingData(int rowCount)
            : this()
        {
            this.RowCount = rowCount;
        }

        /// <summary>
        /// 以指定的页面尺寸、页面数据总该行数作为参数初始化 <see cref="NDF.ComponentModel.Paging.PagingData"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示页面索引号，从 0 开始计数。</param>
        /// <param name="rowCount">表示页面数据总行数。</param>
        public PagingData(int pageIndex, int rowCount)
            : this(rowCount)
        {
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// 以指定的页面尺寸、页面索引号、页面数据总行数作为参数初始化 <see cref="NDF.ComponentModel.Paging.PagingData"/> 对象。
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
        /// 以指定的页面尺寸、页面索引号、页面数据总行数和页面数据内容作为参数初始化 <see cref="NDF.ComponentModel.Paging.PagingData"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示页面索引号，从 0 开始计数。</param>
        /// <param name="pageSize">表示页面的尺寸，即一页有多少行数据。</param>
        /// <param name="rowCount">表示页面数据总行数。</param>
        /// <param name="data">表示页面数据内容。</param>
        public PagingData(int pageIndex, int pageSize, int rowCount, IEnumerable data)
            : this(pageIndex, rowCount, data)
        {
            this.PageSize = pageSize;
        }



        /// <summary>
        /// 获取或设置分页数据对象 <see cref="NDF.ComponentModel.Paging.PagingData"/> 所表示的单页数据尺寸，表示一页有多少行数据。
        /// 该值应大于 0，如果传入一个小于等于 0 的值或者不设置该值，该值会被设置为 <seealso cref="DataPager.DefaultPageSize"/>。
        /// </summary>
        public virtual int PageSize
        {
            get { return this._pageSize < 1 ? DataPager.DefaultPageSize : this._pageSize; }
            set { this._pageSize = value > 0 ? value : DataPager.DefaultPageSize; }
        }

        /// <summary>
        /// 获取或设置分页数据对象 <see cref="NDF.ComponentModel.Paging.PagingData"/> 所表示的当前页面索引号，从 0 开始计数，默认为 0。
        /// 如果传入一个小于 0 的值，该值会被设置为默认值 0。
        /// </summary>
        public virtual int PageIndex
        {
            get { return this._pageIndex < 0 ? 0 : this._pageIndex; }
            set { this._pageIndex = value > -1 ? value : 0; }
        }


        /// <summary>
        /// 获取或设置分页数据对象 <see cref="NDF.ComponentModel.Paging.PagingData"/> 所表示的总行数，默认为 0。
        /// 如果传入一个小于 0 的值，该值会被设置为默认值 0。
        /// </summary>
        public virtual int RowCount
        {
            get { return this._rowCount < 0 ? 0 : this._rowCount; }
            set { this._rowCount = value > -1 ? value : 0; }
        }



        /// <summary>
        /// 获取分页数据对象 <see cref="NDF.ComponentModel.Paging.PagingData"/> 所表示的总页数。
        /// 该值是根据 <seealso cref="RowCount"/> 和 <seealso cref="PageSize"/> 属性分页计算后所得。
        /// </summary>
        public virtual int PageCount
        {
            get { return this.RowCount <= 0 ? 0 : Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.RowCount) / this.PageSize)); }
        }



        /// <summary>
        /// 获取分页数据对象 <see cref="NDF.ComponentModel.Paging.PagingData"/> 所表示的页面数据内容。
        /// </summary>
        public virtual IEnumerable Data
        {
            get
            {
                return this._data == null ? Enumerable.Empty<object>() : this._data;
            }
            set { this._data = value; }
        }
    }
}
