using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.ComponentModel.Paging
{
    /// <summary>
    /// 表示可用于将序列数据集合进行分页处理的数据分页器，该类型可以用于对泛型集合数据进行分页处理。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataPager<T> : DataPager
    {
        /// <summary>
        /// 初始化 <see cref="DataPager&lt;T&gt;"/> 对象。
        /// </summary>
        public DataPager() { }

        /// <summary>
        /// 以指定的所包含的所有数据内容初始化 <see cref="DataPager&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="fullData">表示分页对象所包含的所有数据内容。</param>
        public DataPager(IEnumerable<T> fullData)
            : this()
        {
            this.FullData = FullData;
        }

        /// <summary>
        /// 以指定的页面索引号初始化 <see cref="DataPager&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示分页对象的当前索引号，从 0 开始计数。</param>
        public DataPager(int pageIndex)
            : this()
        {
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// 以指定的页面索引号、页面尺寸初始化 <see cref="DataPager&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示分页对象的当前索引号，从 0 开始计数。</param>
        /// <param name="pageSize">表示分页对象的页面尺寸，即一页有多少行数据。</param>
        public DataPager(int pageIndex, int pageSize)
            : this(pageIndex)
        {
            this.PageSize = pageSize;
        }

        /// <summary>
        /// 以指定的页面索引号和所包含的所有数据内容初始化 <see cref="DataPager&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示分页对象的当前索引号，从 0 开始计数。</param>
        /// <param name="fullData">表示分页对象所包含的所有数据内容。</param>
        public DataPager(int pageIndex, IEnumerable<T> fullData)
            : this(fullData)
        {
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// 以指定的页面索引号、页面尺寸和所包含的所有数据内容初始化 <see cref="DataPager&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="pageIndex">表示分页对象的当前索引号，从 0 开始计数。</param>
        /// <param name="pageSize">表示分页对象的页面尺寸，即一页有多少行数据。</param>
        /// <param name="fullData">表示分页对象所包含的所有数据内容。</param>
        public DataPager(int pageIndex, int pageSize, IEnumerable<T> fullData)
            : this(pageIndex, fullData)
        {
            this.PageSize = pageSize;
        }




        /// <summary>
        /// 获取或设置分页对象 <see cref="DataPager&lt;T&gt;"/> 所包含的所有数据内容。
        /// </summary>
        public new IEnumerable<T> FullData
        {
            get
            {
                return base.FullData == null ? System.Linq.Enumerable.Empty<T>() : base.FullData as IEnumerable<T>;
            }
            set { base.FullData = value; }
        }

        /// <summary>
        /// 获取分页对象 <see cref="DataPager&lt;T&gt;"/> 当前所示页面的数据内容。
        /// 该值是根据 PageIndex 和 PageSize 属性分页计算后所得。
        /// </summary>
        public new IEnumerable<T> CurrentPage
        {
            get
            {
                return this.FullData.GetRange(this.PageIndex * this.PageSize, this.PageSize);
            }
        }



        /// <summary>
        /// 将当前分页对象 <see cref="DataPager&lt;T&gt;"/> 的当前索引页跳至上一页。
        /// 该方法会改变当前对象 PageIndex 属性的值，并直接将当前对象返回。
        /// </summary>
        /// <returns>返回索引号跳至上一页后的当前分页对象 <see cref="DataPager&lt;T&gt;"/>。</returns>
        public new DataPager<T> PrevPage()
        {
            this.PageIndex--;
            return this;
        }

        /// <summary>
        /// 将当前分页对象 <see cref="DataPager&lt;T&gt;"/> 的当前索引页跳至下一页。
        /// 该方法会改变当前对象 PageIndex 属性的值，并直接将当前对象返回。
        /// </summary>
        /// <returns>返回索引号跳至下一页后的当前分页对象 <see cref="DataPager&lt;T&gt;"/>。</returns>
        public new DataPager<T> NextPage()
        {
            this.PageIndex++;
            return this;
        }

        /// <summary>
        /// 将当前分页对象 <see cref="DataPager&lt;T&gt;"/> 的当前索引页跳至第一页。
        /// 该方法会改变当前对象 PageIndex 属性的值，并直接将当前对象返回。
        /// </summary>
        /// <returns>返回索引号跳至第一页后的当前分页对象 <see cref="DataPager&lt;T&gt;"/>。</returns>
        public new DataPager<T> FirstPage()
        {
            this.PageIndex = 0;
            return this;
        }

        /// <summary>
        /// 将当前分页对象 <see cref="DataPager&lt;T&gt;"/> 的当前索引页跳至最后一页。
        /// 该方法会改变当前对象 PageIndex 属性的值，并直接将当前对象返回。
        /// </summary>
        /// <returns>返回索引号跳至最后一页后的当前分页对象 <see cref="DataPager&lt;T&gt;"/>。</returns>
        public new DataPager<T> LastPage()
        {
            this.PageIndex = this.PageCount - 1;
            return this;
        }
    }
}
