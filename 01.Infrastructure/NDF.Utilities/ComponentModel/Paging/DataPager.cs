using NDF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.ComponentModel.Paging
{
    /// <summary>
    /// 表示可用于将序列数据集合进行分页处理的数据分页器。
    /// </summary>
    public class DataPager
    {
        private int _pageSize;
        private int _pageIndex;
        private IEnumerable _fullData;

        /// <summary>
        /// 初始化 <see cref="DataPager"/> 对象。
        /// </summary>
        public DataPager() { }

        /// <summary>
        /// 以指定的所包含的所有数据内容初始化 <see cref="DataPager"/> 对象。
        /// </summary>
        /// <param name="fullData">表示分页对象所包含的所有数据内容。</param>
        public DataPager(IEnumerable fullData)
            : this()
        {
            this.FullData = FullData;
        }

        /// <summary>
        /// 以指定的页面索引号初始化 <see cref="DataPager"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示分页对象的当前索引号，从 0 开始计数。</param>
        public DataPager(int pageIndex)
            : this()
        {
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// 以指定的页面索引号、页面尺寸初始化 <see cref="DataPager"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示分页对象的当前索引号，从 0 开始计数。</param>
        /// <param name="pageSize">表示分页对象的页面尺寸，即一页有多少行数据。</param>
        public DataPager(int pageIndex, int pageSize)
            : this(pageIndex)
        {
            this.PageSize = pageSize;
        }

        /// <summary>
        /// 以指定的页面索引号和所包含的所有数据内容初始化 <see cref="DataPager"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示分页对象的当前索引号，从 0 开始计数。</param>
        /// <param name="fullData">表示分页对象所包含的所有数据内容。</param>
        public DataPager(int pageIndex, IEnumerable fullData)
            : this(fullData)
        {
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// 以指定的页面索引号、页面尺寸和所包含的所有数据内容初始化 <see cref="DataPager"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示分页对象的当前索引号，从 0 开始计数。</param>
        /// <param name="pageSize">表示分页对象的页面尺寸，即一页有多少行数据。</param>
        /// <param name="fullData">表示分页对象所包含的所有数据内容。</param>
        public DataPager(int pageIndex, int pageSize, IEnumerable fullData)
            : this(pageIndex, fullData)
        {
            this.PageSize = pageSize;
        }




        /// <summary>
        /// 获取或设置分页对象 <see cref="DataPager"/> 的单页尺寸大小，表示一页有多少行数据。
        /// 该值应大于 0，如果传入一个小于等于 0 的值或者不设置该值，该值会被设置为 <seealso cref="DataPager.DefaultPageSize"/>。
        /// </summary>
        public int PageSize
        {
            get { return this._pageSize < 1 ? DefaultPageSize : this._pageSize; }
            set { this._pageSize = value > 0 ? value : DefaultPageSize; }
        }

        /// <summary>
        /// 获取或设置分页对象 <see cref="DataPager"/> 的当前页索引号，从 0 开始计数，默认为 0。
        /// 该索引号标识 <seealso cref="CurrentPage"/> 属于数据 <seealso cref="FullData"/> 的第几页。
        /// 如果传入一个小于 0 的值，该值会被设置为默认值 0。
        /// </summary>
        public int PageIndex
        {
            get { return this._pageIndex < 0 ? 0 : this._pageIndex; }
            set { this._pageIndex = value > -1 ? (value < this.PageCount ? value : (this.PageCount - 1)) : 0; }
        }

        /// <summary>
        /// 获取或设置分页对象 <see cref="DataPager"/> 所包含的所有数据内容。
        /// </summary>
        public IEnumerable FullData
        {
            get
            {
                return this._fullData == null ? System.Linq.Enumerable.Empty<object>() : this._fullData;
            }
            set { this._fullData = value; }
        }


        /// <summary>
        /// 获取分页对象 <see cref="DataPager"/> 所包含的所有数据内容 <seealso cref="FullData"/> 的总行数。
        /// </summary>
        public int RowCount
        {
            get { return this.FullData == null ? 0 : this.FullData.ToGeneric().Count(); }
        }

        /// <summary>
        /// 获取分页对象 <see cref="DataPager"/> 所包含的所有数据内容 <seealso cref="FullData"/> 的总页数。
        /// 该值是根据 <seealso cref="RowCount"/> 和 <seealso cref="PageSize"/> 属性分页计算后所得。
        /// </summary>
        public int PageCount
        {
            get { return this.RowCount <= 0 ? 0 : Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.RowCount) / this.PageSize)); }
        }

        /// <summary>
        /// 获取分页对象 <see cref="DataPager"/> 当前所示页面的数据内容。
        /// 该值是根据 <seealso cref="PageIndex"/> 和 <seealso cref="PageSize"/> 属性分页计算后所得。
        /// </summary>
        public IEnumerable CurrentPage
        {
            get
            {
                return this.FullData.ToGeneric().GetRange(this.PageIndex * this.PageSize, this.PageSize);
            }
        }



        /// <summary>
        /// 将当前分页对象 <see cref="DataPager"/> 的当前索引页跳至上一页。
        /// 该方法会改变当前对象 <seealso cref="PageIndex"/> 属性的值，并直接将当前对象返回。
        /// </summary>
        /// <returns>返回索引号跳至上一页后的当前分页对象 <see cref="DataPager"/>。</returns>
        public DataPager PrevPage()
        {
            this.PageIndex--;
            return this;
        }

        /// <summary>
        /// 将当前分页对象 <see cref="DataPager"/> 的当前索引页跳至下一页。
        /// 该方法会改变当前对象 <seealso cref="PageIndex"/> 属性的值，并直接将当前对象返回。
        /// </summary>
        /// <returns>返回索引号跳至下一页后的当前分页对象 <see cref="DataPager"/>。</returns>
        public DataPager NextPage()
        {
            this.PageIndex++;
            return this;
        }

        /// <summary>
        /// 将当前分页对象 <see cref="DataPager"/> 的当前索引页跳至第一页。
        /// 该方法会改变当前对象 <seealso cref="PageIndex"/> 属性的值，并直接将当前对象返回。
        /// </summary>
        /// <returns>返回索引号跳至第一页后的当前分页对象 <see cref="DataPager"/>。</returns>
        public DataPager FirstPage()
        {
            this.PageIndex = 0;
            return this;
        }

        /// <summary>
        /// 将当前分页对象 <see cref="DataPager"/> 的当前索引页跳至最后一页。
        /// 该方法会改变当前对象 <seealso cref="PageIndex"/> 属性的值，并直接将当前对象返回。
        /// </summary>
        /// <returns>返回索引号跳至最后一页后的当前分页对象 <see cref="DataPager"/>。</returns>
        public DataPager LastPage()
        {
            this.PageIndex = this.PageCount - 1;
            return this;
        }




        private static int _defaultPageSize = 10;

        /// <summary>
        /// 获取或设置分页数据的默认单页数据尺寸，即默认不设置 <seealso cref="PageSize"/> 属性的情况一页有多少行数据，初始默认值为 10。
        /// 该值应大于 0，如果传入一个小于等于 0 的值，该值会被设置为初始默认值 10。
        /// </summary>
        public static int DefaultPageSize
        {
            get { return _defaultPageSize < 1 ? 10 : _defaultPageSize; }
            set { _defaultPageSize = value > 0 ? value : 10; }
        }

    }
}
