using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Collections
{
    /// <summary>
    /// 为 <see cref="System.Collections.IEqualityComparer"/> 接口的实现提供基类。
    /// </summary>
    [Serializable]
    public class EqualityComparer : System.Collections.IEqualityComparer
    {
        private Func<object, int> _proxyGetHashCode;
        private Func<object, object, bool> _proxyEquals;


        /// <summary>
        /// 初始化 <see cref="EqualityComparer&lt;T&gt;"/> 类的新实例。
        /// 该对象比较器实例的 <seealso cref="Object.GetHashCode"/> 方法始终返回 0。
        /// </summary>
        public EqualityComparer()
        {
            this.ProxyGetHashCode = obj => 0;
        }

        /// <summary>
        /// 以指定的方法委托作为 哈希算法函数 初始化 <see cref="EqualityComparer"/> 类的新实例。
        /// </summary>
        /// <param name="getHashCode">一个表示 哈希算法函数 的方法委托。</param>
        public EqualityComparer(Func<object, int> getHashCode)
        {
            this.ProxyGetHashCode = getHashCode;
        }

        /// <summary>
        /// 以指定的方法委托作为 对象比较函数 初始化 <see cref="EqualityComparer"/> 类的新实例。
        /// 该对象比较器实例的 <seealso cref="Object.GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        public EqualityComparer(Func<object, object, bool> equals)
        {
            this.ProxyEquals = equals;
            this.ProxyGetHashCode = obj => 0;
        }

        /// <summary>
        /// 以指定的方法委托作为 对象比较函数 初始化 <see cref="EqualityComparer"/> 类的新实例。
        /// 该对象比较器实例的 <seealso cref="Object.GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <param name="comparer">一个表示 对象比较函数 的 <see cref="IComparer"/> 实例。</param>
        public EqualityComparer(IComparer comparer)
        {
            this.ProxyEquals = (x, y) => comparer.Compare(x, y) == 0;
            this.ProxyGetHashCode = obj => 0;
        }

        /// <summary>
        /// 以指定的方法委托作为 对象比较函数 初始化 <see cref="EqualityComparer"/> 类的新实例。
        /// 该对象比较器实例的 <seealso cref="Object.GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <param name="comparer">一个表示 对象比较函数 的 IComparer&lt;Object&gt; 实例。</param>
        public EqualityComparer(IComparer<object> comparer)
        {
            this.ProxyEquals = (x, y) => comparer.Compare(x, y) == 0;
            this.ProxyGetHashCode = obj => 0;
        }

        /// <summary>
        /// 以指定的 对象比较函数 和 哈希算法函数 初始化 <see cref="EqualityComparer"/> 类的新实例。
        /// </summary>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        /// <param name="getHashCode">一个表示 哈希算法函数 的方法委托。</param>
        public EqualityComparer(Func<object, object, bool> equals, Func<object, int> getHashCode)
        {
            this.ProxyGetHashCode = getHashCode;
            this.ProxyEquals = equals;
        }

        /// <summary>
        /// 以指定的 哈希算法函数 和 对象比较函数 初始化 <see cref="EqualityComparer"/> 类的新实例。
        /// </summary>
        /// <param name="getHashCode">一个表示 哈希算法函数 的方法委托。</param>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        public EqualityComparer(Func<object, int> getHashCode, Func<object, object, bool> equals)
        {
            this.ProxyGetHashCode = getHashCode;
            this.ProxyEquals = equals;
        }



        /// <summary>
        /// 获取或设置一个方法委托属性，该属性表示当前对象相等比较器的 哈希算法函数。
        /// </summary>
        public Func<object, int> ProxyGetHashCode
        {
            get { return this._proxyGetHashCode; }
            set { this._proxyGetHashCode = value; }
        }

        /// <summary>
        /// 获取或设置一个方法委托属性，该属性表示当前对象相等比较器的 对象比较函数。
        /// </summary>
        public Func<object, object, bool> ProxyEquals
        {
            get { return this._proxyEquals; }
            set { this._proxyEquals = value; }
        }




        /// <summary>
        /// 确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>如果指定的对象相等，则为 true；否则为 false。</returns>
        bool IEqualityComparer.Equals(object x, object y)
        {
            var equals = this.ProxyEquals ?? Default.ProxyEquals;
            return equals(x, y);
        }

        /// <summary>
        /// 返回指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">一个对象，将为其返回哈希代码。 </param>
        /// <returns>指定对象的哈希代码。 </returns>
        int IEqualityComparer.GetHashCode(object obj)
        {
            var getHashCode = this.ProxyGetHashCode ?? Default.ProxyGetHashCode;
            return getHashCode(obj);
        }




        private static EqualityComparer _default;

        /// <summary>
        /// 返回一个默认的相等比较器，用于比较此泛型参数指定的类型。
        /// </summary>
        public static EqualityComparer Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new EqualityComparer(
                        System.Collections.Generic.EqualityComparer<object>.Default.GetHashCode,
                        System.Collections.Generic.EqualityComparer<object>.Default.Equals);
                }
                return _default;
            }
        }





        /// <summary>
        /// 创建一个默认的相等比较器。
        /// 该对象比较器实例的 <seealso cref="Object.GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <returns>一个 <see cref="EqualityComparer"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer Create()
        {
            return new EqualityComparer();
        }

        /// <summary>
        /// 以指定的方法委托作为 哈希算法函数 创建一个 <see cref="EqualityComparer"/> 相等比较器。
        /// </summary>
        /// <param name="getHashCode">一个表示 对象比较函数 的方法委托。</param>
        /// <returns>一个 <see cref="EqualityComparer"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer Create(Func<object, int> getHashCode)
        {
            return new EqualityComparer(getHashCode);
        }

        /// <summary>
        /// 以指定的方法委托作为 对象比较函数 创建一个 <see cref="EqualityComparer"/> 相等比较器。
        /// 该对象比较器实例的 <seealso cref="Object.GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        /// <returns>一个 <see cref="EqualityComparer"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer Create(Func<object, object, bool> equals)
        {
            return new EqualityComparer(equals);
        }

        /// <summary>
        /// 以指定的方法委托作为 对象比较函数 创建一个 <see cref="EqualityComparer"/> 相等比较器。
        /// 该对象比较器实例的 <seealso cref="Object.GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <param name="comparer">一个表示 对象比较函数 的 <see cref="Comparer"/> 实例。</param>
        /// <returns>一个 <see cref="EqualityComparer"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer Create(IComparer comparer)
        {
            return new EqualityComparer(comparer);
        }

        /// <summary>
        /// 以指定的方法委托作为 对象比较函数 创建一个 <see cref="EqualityComparer"/> 相等比较器。
        /// 该对象比较器实例的 <seealso cref="Object.GetHashCode"/> 方法始终返回 0。
        /// </summary>
        /// <param name="comparer">一个表示 对象比较函数 的 IComparer&lt;Object&gt; 实例。</param>
        /// <returns>一个 <see cref="EqualityComparer"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer Create(IComparer<object> comparer)
        {
            return new EqualityComparer(comparer);
        }

        /// <summary>
        /// 以指定的 对象比较函数 和 哈希算法函数 创建一个 <see cref="EqualityComparer"/> 相等比较器。
        /// </summary>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        /// <param name="getHashCode">一个表示 对象比较函数 的方法委托。</param>
        /// <returns>一个 <see cref="EqualityComparer"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer Create(Func<object, object, bool> equals, Func<object, int> getHashCode)
        {
            return new EqualityComparer(equals, getHashCode);
        }

        /// <summary>
        /// 以指定的 哈希算法函数 和 对象比较函数 创建一个 <see cref="EqualityComparer"/> 相等比较器。
        /// </summary>
        /// <param name="getHashCode">一个表示 对象比较函数 的方法委托。</param>
        /// <param name="equals">一个表示 对象比较函数 的方法委托。</param>
        /// <returns>一个 <see cref="EqualityComparer"/> 类的新实例，用于比较此泛型参数指定的类型。</returns>
        public static EqualityComparer Create(Func<object, int> getHashCode, Func<object, object, bool> equals)
        {
            return new EqualityComparer(getHashCode, equals);
        }
    }
}
