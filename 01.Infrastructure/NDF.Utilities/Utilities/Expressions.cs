using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组快速生成或操作 LINQ 表达式/表达式树 的工具方法。
    /// </summary>
    public static class Expressions
    {
        /// <summary>
        /// 通过指定的 类型参数 和公共 属性/字段 名称创建一个获取该类型中指定属性值的 Lambda 表达式树。
        /// </summary>
        /// <param name="type">指定的类型。</param>
        /// <param name="propertyOrFieldName"><paramref name="type"/> 类型中的公共属性或者公共字段。</param>
        /// <returns>返回一个 Lambda 表达式树 <see cref="LambdaExpression"/> 对象，表示用于获取类型 <paramref name="type"/> 中指定名称 <paramref name="propertyOrFieldName"/> 的属性值。</returns>
        /// <exception cref="System.ArgumentNullException">type 或 propertyOrFieldName 为 null。</exception>
        /// <exception cref="System.ArgumentException">没有在 type 或其基类型中定义名为 propertyOrFieldName 的属性或字段。</exception>
        public static LambdaExpression Lambda(Type type, string propertyOrFieldName)
        {
            return Lambda(type, propertyOrFieldName);
        }

        /// <summary>
        /// 通过指定的 类型参数 和公共 属性/字段 名称创建一个获取该类型中指定属性值的 Lambda 表达式树。
        /// 并返回该公共 属性/字段 的数据类型。
        /// </summary>
        /// <param name="type">指定的类型。</param>
        /// <param name="propertyOrFieldName"><paramref name="type"/> 类型中的公共属性或者公共字段。</param>
        /// <param name="propertyOrFieldType">表示 <paramref name="propertyOrFieldName"/> 属性/字段 的数据类型。</param>
        /// <returns>返回一个 Lambda 表达式树 <see cref="LambdaExpression"/> 对象，表示用于获取类型 <paramref name="type"/> 中指定名称 <paramref name="propertyOrFieldName"/> 的属性值。</returns>
        /// <exception cref="System.ArgumentNullException">type 或 propertyOrFieldName 为 null。</exception>
        /// <exception cref="System.ArgumentException">没有在 type 或其基类型中定义名为 propertyOrFieldName 的属性或字段。</exception>
        public static LambdaExpression Lambda(Type type, string propertyOrFieldName, out Type propertyOrFieldType)
        {
            ParameterExpression paramExpr = Expression.Parameter(type, "t");
            MemberExpression bodyExpr = Expression.PropertyOrField(paramExpr, propertyOrFieldName);
            propertyOrFieldType = bodyExpr.Type;
            return Expression.Lambda(bodyExpr, paramExpr);
        }


        /// <summary>
        /// 通过指定的 类型参数 和公共 属性/字段 名称创建一个获取该类型中指定属性值的 Lambda 表达式树。
        /// </summary>
        /// <typeparam name="TSource">指定的类型。</typeparam>
        /// <param name="propertyOrFieldName"><typeparamref name="TSource"/> 类型中的公共属性或者公共字段。</param>
        /// <returns>返回一个 Lambda 表达式树 <see cref="LambdaExpression"/> 对象，表示用于获取类型 <typeparamref name="TSource"/> 中指定名称 <paramref name="propertyOrFieldName"/> 的属性值。</returns>
        /// <exception cref="System.ArgumentNullException">propertyOrFieldName 为 null。</exception>
        /// <exception cref="System.ArgumentException">没有在 <typeparamref name="TSource"/> 或其基类型中定义名为 propertyOrFieldName 的属性或字段。</exception>
        public static LambdaExpression Lambda<TSource>(string propertyOrFieldName)
        {
            return Lambda(typeof(TSource), propertyOrFieldName);
        }

        /// <summary>
        /// 通过指定的 类型参数 和公共 属性/字段 名称创建一个获取该类型中指定属性值的 Lambda 表达式树。
        /// 并返回该公共 属性/字段 的数据类型。
        /// </summary>
        /// <typeparam name="TSource">指定的类型。</typeparam>
        /// <param name="propertyOrFieldName"><typeparamref name="TSource"/> 类型中的公共属性或者公共字段。</param>
        /// <param name="propertyOrFieldType">表示 <paramref name="propertyOrFieldName"/> 属性/字段 的数据类型。</param>
        /// <returns>返回一个 Lambda 表达式树 <see cref="LambdaExpression"/> 对象，表示用于获取类型 <typeparamref name="TSource"/> 中指定名称 <paramref name="propertyOrFieldName"/> 的属性值。</returns>
        /// <exception cref="System.ArgumentNullException">propertyOrFieldName 为 null。</exception>
        /// <exception cref="System.ArgumentException">没有在 <typeparamref name="TSource"/> 或其基类型中定义名为 propertyOrFieldName 的属性或字段。</exception>
        public static LambdaExpression Lambda<TSource>(string propertyOrFieldName, out Type propertyOrFieldType)
        {
            return Lambda(typeof(TSource), propertyOrFieldName, out propertyOrFieldType);
        }

    }
}
