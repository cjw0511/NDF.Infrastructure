using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF
{
    /// <summary>
    /// 提供用于指定运行时的动态行为的数据类型。
    /// </summary>
    public class DynamicObject : System.Dynamic.DynamicObject
    {
        /// <summary>
        /// 初始化一个 <see cref="DynamicObject"/> 对象。
        /// </summary>
        public DynamicObject()
        {
            this.Data = new Dictionary<string, object>();
        }


        /// <summary>
        /// 获取运行时动态解析对象的内部字典数据。
        /// </summary>
        public Dictionary<string, object> Data
        {
            get;
            private set;
        }



        /// <summary>
        /// 返回所有动态成员名称的枚举。
        /// </summary>
        /// <returns>一个包含动态成员名称的序列。</returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Data.Keys;
        }

        /// <summary>
        /// 获取当前 <see cref="DynamicObject"/> 对象的所有动态属性。
        /// </summary>
        /// <returns>当前 <see cref="DynamicObject"/> 对象的所有动态属性。</returns>
        public virtual IEnumerable<string> GetProperties()
        {
            return this.GetDynamicMemberNames();
        }



        /// <summary>
        /// 为获取成员值的操作提供实现。
        /// </summary>
        /// <param name="binder">
        /// 提供有关调用了动态操作的对象的信息。 binder.Name 属性提供针对其执行动态操作的成员的名称。 例如，对于 Console.WriteLine(sampleObject.SampleProperty)
        /// 语句（其中 sampleObject 是派生自 System.Dynamic.DynamicObject 类的类的一个实例），binder.Name
        /// 将返回“SampleProperty”。 binder.IgnoreCase 属性指定成员名称是否区分大小写。
        /// </param>
        /// <param name="result">获取操作的结果。 例如，如果为某个属性调用该方法，则可以为 result 指派该属性值。</param>
        /// <returns>如果此运算成功，则为 true；否则为 false。 如果此方法返回 false，则该语言的运行时联编程序将决定行为。（大多数情况下，将引发运行时异常。）</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.Data.TryGetValue(binder.Name, out result);
        }

        /// <summary>
        /// 为设置成员值的操作提供实现。 
        /// </summary>
        /// <param name="binder">
        /// 提供有关调用了动态操作的对象的信息。 binder.Name 属性提供将该值分配到的成员的名称。 例如，对于语句 sampleObject.SampleProperty
        /// = "Test"（其中 sampleObject 是派生自 System.Dynamic.DynamicObject 类的类的一个实例），binder.Name
        /// 将返回“SampleProperty”。 binder.IgnoreCase 属性指定成员名称是否区分大小写。
        /// </param>
        /// <param name="value">
        /// 要为成员设置的值。 例如，对于 sampleObject.SampleProperty = "Test"（其中 sampleObject 是派生自
        /// System.Dynamic.DynamicObject 类的类的一个实例），value 为“Test”。
        /// </param>
        /// <returns>如果此运算成功，则为 true；否则为 false。 如果此方法返回 false，则该语言的运行时联编程序将决定行为。（大多数情况下，将引发语言特定的运行时异常。）</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.Data[binder.Name] = value;
            return true;
        }


        /// <summary>
        /// 为按索引获取值的操作提供实现。
        /// </summary>
        /// <param name="binder">提供有关该操作的信息。</param>
        /// <param name="indexes">
        /// 该操作中使用的索引。 例如，对于 C# 中的 sampleObject[3] 操作（Visual Basic 中为 sampleObject(3)）（其中
        /// sampleObject 派生自 DynamicObject 类），indexes[0] 等于 3。
        /// </param>
        /// <param name="result">索引操作的结果。</param>
        /// <returns>如果此运算成功，则为 true；否则为 false。 如果此方法返回 false，则该语言的运行时联编程序将决定行为。（大多数情况下，将引发运行时异常。）</returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            string property = indexes[0] as string;
            if (string.IsNullOrWhiteSpace(property))
            {
                result = null;
                return false;
            }
            else
            {
                return this.Data.TryGetValue(property, out result);
            }
        }

        /// <summary>
        /// 为按索引设置值的操作提供实现。
        /// </summary>
        /// <param name="binder">提供有关该操作的信息。</param>
        /// <param name="indexes">
        /// 该操作中使用的索引。 例如，对于 C# 中的 sampleObject[3] = 10 操作（Visual Basic 中为 sampleObject(3)
        /// = 10）（其中 sampleObject 派生自 System.Dynamic.DynamicObject 类），indexes[0] 等于 3。
        /// </param>
        /// <param name="value">
        /// 要为具有指定索引的对象设置的值。 例如，对于 C# 中的 sampleObject[3] = 10 操作（Visual Basic 中为 sampleObject(3)
        /// = 10）（其中 sampleObject 派生自 System.Dynamic.DynamicObject 类），value 等于 10。
        /// </param>
        /// <returns>如果此运算成功，则为 true；否则为 false。 如果此方法返回 false，则该语言的运行时联编程序将决定行为。（大多数情况下，将引发语言特定的运行时异常。）</returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            string property = indexes[0] as string;
            if (string.IsNullOrWhiteSpace(property))
            {
                return false;
            }
            this.Data[property] = value;
            return true;
        }

    }
}
