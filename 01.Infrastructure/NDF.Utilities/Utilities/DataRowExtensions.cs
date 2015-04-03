using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 数据行对象 <see cref="System.Data.DataRow"/> 操作方法的扩展。
    /// </summary>
    public static class DataRowExtensions
    {

        /// <summary>
        /// 将一个数据行对象转换为一个动态运行时对象。
        /// <paramref name="row"/> 数据行对象中的所有数据项将作为属性值反映在返回的动态对象中。
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static dynamic ToObject(this DataRow row)
        {
            Check.NotNull(row);
            dynamic obj = new DynamicObject();
            foreach (DataColumn column in row.Table.Columns)
            {
                obj[column.ColumnName] = row[column];
            }
            return obj;
        }

        /// <summary>
        /// 将一个数据行对象转换成一个指定类型的 CLR 对象。
        /// <paramref name="row"/> 数据行对象中的所有数据项将作为属性值反映在返回的 CLR 对象中。
        /// 对于不能转换的数据项（例如指定的 <typeparamref name="TResult"/> 类型中没有该数据项所示名称的属性、或者数据类型不匹配），将不会包含在返回的 CLR 对象中。
        /// </summary>
        /// <typeparam name="TResult">指定的 CLR 类型。</typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static TResult ToObject<TResult>(this DataRow row) where TResult : class, new()
        {
            Check.NotNull(row);

            Type type = typeof(TResult);
            TResult ret = new TResult();
            IEnumerable<PropertyInfo> properties = type.GetProperties().Where(p => p.SetMethod != null);
            foreach (string name in row.Table.GetColumnNames())
            {
                PropertyInfo property = properties.FirstOrDefault(p => p.Name == name);
                if (property == null)
                    continue;

                object value = row[name];
                if (value.IsNull() && !property.PropertyType.IsValueType)
                {
                    Utility.TryCatchExecute(() => property.SetValue(ret, property.PropertyType == typeof(DBNull) ? DBNull.Value : value));
                }
                else
                {
                    if (property.PropertyType.IsAssignableFrom(value.GetType()))
                    {
                        Utility.TryCatchExecute(() => property.SetValue(ret, value));
                    }
                    else
                    {
                        if (value is IConvertible && property.PropertyType.IsImplementOf(typeof(IConvertible)))
                        {
                            object val = Convert.ChangeType(value, property.PropertyType);
                            Utility.TryCatchExecute(() => property.SetValue(ret, val));
                        }
                    }
                }
            }
            return ret;
        }
        

    }
}
