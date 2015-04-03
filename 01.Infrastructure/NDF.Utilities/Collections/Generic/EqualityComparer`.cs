using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Collections.Generic
{
    /// <summary>
    /// 为 <see cref="System.Collections.Generic.IEqualityComparer&lt;T&gt;"/> 泛型接口的实现提供基类。
    /// 该类型与 <see cref="System.Collections.Generic.EqualityComparer&lt;T&gt;"/> 的不同之处在于：
    ///     <see cref="System.Collections.Generic.EqualityComparer&lt;T&gt;"/> 为一个抽象类；
    ///     而该类型为一个已实现了的类型定义。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    public class EqualityComparer<T> : System.Collections.Generic.EqualityComparer<T>
    {
        private Func<T, int> _proxyGetHashCode;
        private Func<T, T, bool> _proxyEquals;


        /// <summary>
        /// 初始化 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例。
        /// 该对象比较器实例的 <seealso cref="GetHashCode"/> 方法始终返回 0。
        /// </summary>
        public EqualityComparer()
        {
            this.ProxyGetHashCode = obj => 0;
        }

        /// <summary>
        /// 以指定的方法委托作为 哈希算法函数 初始化 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例。
        /// </summary>
        /// <param name="getHashCode">一个表示 哈希算法函数 的方法委托。</param>
        public EqualityComparer(Func<T, int> getHashCode)
        {
            this.ProxyGetHashCode = getHashCode;
        }

        /// <summary>
        /// 以指定的方法委托作为 对象比较函数 初始化 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例。
        /// 该对象比较器实例的 <seealso cref="GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        public EqualityComparer(Func<T, T, bool> equals)
        {
            this.ProxyEquals = equals;
            this.ProxyGetHashCode = obj => 0;
        }

        /// <summary>
        /// 以指定的方法委托作为 对象比较函数 初始化 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例。
        /// 该对象比较器实例的 <seealso cref="GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <param name="comparer">一个表示 对象比较函数 的 <see cref="Comparer&lt;T&gt;"/> 实例。</param>
        public EqualityComparer(IComparer<T> comparer)
        {
            this.ProxyEquals = (x, y) => comparer.Compare(x, y) == 0;
            this.ProxyGetHashCode = obj => 0;
        }

        /// <summary>
        /// 以指定的 对象比较函数 和 哈希算法函数 初始化 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例。
        /// </summary>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        /// <param name="getHashCode">一个表示 哈希算法函数 的方法委托。</param>
        public EqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            this.ProxyGetHashCode = getHashCode;
            this.ProxyEquals = equals;
        }

        /// <summary>
        /// 以指定的 哈希算法函数 和 对象比较函数 初始化 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例。
        /// </summary>
        /// <param name="getHashCode">一个表示 哈希算法函数 的方法委托。</param>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        public EqualityComparer(Func<T, int> getHashCode, Func<T, T, bool> equals)
        {
            this.ProxyGetHashCode = getHashCode;
            this.ProxyEquals = equals;
        }


        /// <summary>
        /// 获取或设置一个方法委托属性，该属性表示当前对象相等比较器的 哈希算法函数。
        /// </summary>
        public Func<T, int> ProxyGetHashCode
        {
            get { return this._proxyGetHashCode; }
            set { this._proxyGetHashCode = value; }
        }

        /// <summary>
        /// 获取或设置一个方法委托属性，该属性表示当前对象相等比较器的 对象比较函数。
        /// </summary>
        public Func<T, T, bool> ProxyEquals
        {
            get { return this._proxyEquals; }
            set { this._proxyEquals = value; }
        }


        /// <summary>
        /// 确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个类型为 <typeparamref name="T"/> 的对象。</param>
        /// <param name="y">要比较的第二个类型为 <typeparamref name="T"/> 的对象。</param>
        /// <returns>如果指定的对象相等，则为 true；否则为 false。</returns>
        public override bool Equals(T x, T y)
        {
            var equals = this.ProxyEquals ?? Default.ProxyEquals;
            return equals(x, y);
        }

        /// <summary>
        /// 返回指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">类型为 <typeparamref name="T"/> 的一个对象，将为其返回哈希代码。 </param>
        /// <returns>指定对象的哈希代码。 </returns>
        public override int GetHashCode(T obj)
        {
            var getHashCode = this.ProxyGetHashCode ?? Default.ProxyGetHashCode;
            return getHashCode(obj);
        }



        private static EqualityComparer<T> _default;

        /// <summary>
        /// 返回一个默认的相等比较器，用于比较此泛型参数指定的类型。
        /// </summary>
        public static new EqualityComparer<T> Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new EqualityComparer<T>(
                        System.Collections.Generic.EqualityComparer<T>.Default.GetHashCode,
                        System.Collections.Generic.EqualityComparer<T>.Default.Equals);
                }
                return _default;
            }
        }





        /// <summary>
        /// 创建一个默认的相等比较器。
        /// 该对象比较器实例的 <seealso cref="GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <returns>一个 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer<T> Create()
        {
            return new EqualityComparer<T>();
        }

        /// <summary>
        /// 以指定的方法委托作为 哈希算法函数 创建一个 <see cref="EqualityComparer&lt;T&gt;"/> 相等比较器。
        /// </summary>
        /// <param name="getHashCode">一个表示 对象比较函数 的方法委托。</param>
        /// <returns>一个 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer<T> Create(Func<T, int> getHashCode)
        {
            return new EqualityComparer<T>(getHashCode);
        }

        /// <summary>
        /// 以指定的方法委托作为 对象比较函数 创建一个 <see cref="EqualityComparer&lt;T&gt;"/> 相等比较器。
        /// 该对象比较器实例的 <seealso cref="GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        /// <returns>一个 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer<T> Create(Func<T, T, bool> equals)
        {
            return new EqualityComparer<T>(equals);
        }

        /// <summary>
        /// 以指定的方法委托作为 对象比较函数 创建一个 <see cref="EqualityComparer&lt;T&gt;"/> 相等比较器。
        /// 该对象比较器实例的 <seealso cref="GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <param name="comparer">一个表示 对象比较函数 的 <see cref="Comparer&lt;T&gt;"/> 实例。</param>
        /// <returns>一个 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer<T> Create(IComparer<T> comparer)
        {
            return new EqualityComparer<T>(comparer);
        }

        /// <summary>
        /// 以指定的 对象比较函数 和 哈希算法函数 创建一个 <see cref="EqualityComparer&lt;T&gt;"/> 相等比较器。
        /// </summary>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        /// <param name="getHashCode">一个表示 对象比较函数 的方法委托。</param>
        /// <returns>一个 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer<T> Create(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            return new EqualityComparer<T>(equals, getHashCode);
        }

        /// <summary>
        /// 以指定的 哈希算法函数 和 对象比较函数 创建一个 <see cref="EqualityComparer&lt;T&gt;"/> 相等比较器。
        /// </summary>
        /// <param name="getHashCode">一个表示 对象比较函数 的方法委托。</param>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        /// <returns>一个 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer<T> Create(Func<T, int> getHashCode, Func<T, T, bool> equals)
        {
            return new EqualityComparer<T>(getHashCode, equals);
        }
    }
}
