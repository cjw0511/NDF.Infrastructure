using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.ComponentModel.Paging
{
    /// <summary>
    /// 提供一组针对 分页数据对象 <see cref="PagingTable"/>、<see cref="PagingData"/> 或 <see cref="PagingData&lt;T&gt;"/> 的快速操作 API。
    /// </summary>
    public static class PagingExtensions
    {

        /// <summary>
        /// 将 <see cref="PagingData&lt;TSource&gt;"/> 分页数据对象转换为 <see cref="PagingTable"/>。
        /// </summary>
        /// <typeparam name="TSource">分页数据对象中数据项的类型。</typeparam>
        /// <param name="_this">源分页数据对象。</param>
        /// <returns>返回一个  <see cref="PagingTable"/>，返回的分页数据对象中的分页数据等效于源对象 <paramref name="_this"/> 。</returns>
        public static PagingTable ToPagingTable<TSource>(this PagingData<TSource> _this)
        {
            Check.NotNull(_this);
            DataTable data = _this.Data == null ? null : _this.Data.ToDataTable();
            return new PagingTable(_this.PageIndex, _this.PageSize, _this.RowCount, data);
        }


        /// <summary>
        /// 将 <see cref="PagingTable"/> 分页数据表格对象转换为 <see cref="PagingData&lt;TResult&gt;"/> 分页数据对象。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static PagingData<TResult> ToPagingData<TResult>(this PagingTable _this) where TResult : class, new()
        {
            Check.NotNull(_this);
            IEnumerable<TResult> data = _this.Data == null ? null : _this.Data.ToList<TResult>();
            return new PagingData<TResult>(_this.PageIndex, _this.PageSize, _this.RowCount, data);
        }

        /// <summary>
        /// 根据指定的转换函数将泛型分页数据对象 <see cref="PagingData{TSource}"/> 转换成另一种格式的泛型分页数据对象 <see cref="PagingData{TResult}"/>。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="_this"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static PagingData<TResult> ToPagingData<TSource, TResult>(this PagingData<TSource> _this, Converter<TSource, TResult> converter)
        {
            Check.NotNull(_this);
            Check.NotNull(converter);
            IEnumerable<TResult> data = _this.Data.Select(item => converter(item));
            return new PagingData<TResult>(_this.PageIndex, _this.PageSize, _this.RowCount, data);
        }


    }
}
