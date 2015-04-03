using NDF.Serialization.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 基础对象类型 <see cref="System.Object"/> 生成 JSON 格式字符串操作方法的扩展。
    /// </summary>
    public static partial class Serialization
    {
        private static NDF.Serialization.Json.JsonSerializer _serializer;
        private static Func<NDF.Serialization.Json.JsonSerializer> _serializerGenerator;


        /// <summary>
        /// 获取当前类型用于序列化 Json 对象的序列化器 <see cref="NDF.Serialization.Json.JsonSerializer"/>。
        /// </summary>
        /// <remarks>在初始情况下，该序列化器由属性 <seealso cref="JsonSerializerGenerator"/> 所表示的委托函数所创建。</remarks>
        public static NDF.Serialization.Json.JsonSerializer JsonSerializer
        {
            get
            {
                if (_serializer == null)
                {
                    InitializeJsonSerializer();
                }
                return _serializer;
            }
        }

        /// <summary>
        /// 初始化/重置当前类型的静态属性 <seealso cref="JsonSerializer"/> 的值。
        /// </summary>
        /// <remarks>该操作将会丢弃对当前类型的静态属性 <seealso cref="JsonSerializer"/> 值的更改。</remarks>
        public static void InitializeJsonSerializer()
        {
            _serializer = JsonSerializerGenerator();
        }


        /// <summary>
        /// 获取或设置一个具有返回值的委托函数，该函数用于获取一个 Json 序列化器 <see cref="NDF.Serialization.Json.JsonSerializer"/> 对象。
        /// </summary>
        public static Func<NDF.Serialization.Json.JsonSerializer> JsonSerializerGenerator
        {
            get
            {
                if (_serializerGenerator == null)
                {
                    _serializerGenerator = () => new NDF.Serialization.Json.JsonSerializer();
                }
                return _serializerGenerator;
            }
            set { _serializerGenerator = value; }
        }



        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        /// <remarks>该方法调用 Newtonsoft.Json.dll 组件的 JsonConvert.SerializeObject 方法来执行序列化操作。</remarks>
        public static string ToJson(this object _this)
        {
            return JsonSerializer.Serialize(_this);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="formatting">定义序列化结果的输出格式。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        /// <remarks>该方法调用 Newtonsoft.Json.dll 组件的 JsonConvert.SerializeObject 方法来执行序列化操作。</remarks>
        public static string ToJson(this object _this, Formatting formatting)
        {
            return JsonSerializer.Serialize(_this, formatting);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="settings">定义序列化对象时采用的序列化设置。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        /// <remarks>该方法调用 Newtonsoft.Json.dll 组件的 JsonConvert.SerializeObject 方法来执行序列化操作。</remarks>
        public static string ToJson(this object _this, JsonSerializerSettings settings)
        {
            return JsonSerializer.Serialize(_this, settings);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="converters">定义序列化对象时的转换器集合。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        /// <remarks>该方法调用 Newtonsoft.Json.dll 组件的 JsonConvert.SerializeObject 方法来执行序列化操作。</remarks>
        public static string ToJson(this object _this, params JsonConverter[] converters)
        {
            return JsonSerializer.Serialize(_this, converters);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="formatting">定义序列化结果的输出格式。</param>
        /// <param name="settings">定义序列化对象时采用的序列化设置。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        /// <remarks>该方法调用 Newtonsoft.Json.dll 组件的 JsonConvert.SerializeObject 方法来执行序列化操作。</remarks>
        public static string ToJson(this object _this, Formatting formatting, JsonSerializerSettings settings)
        {
            return JsonSerializer.Serialize(_this, formatting, settings);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="formatting">定义序列化结果的输出格式。</param>
        /// <param name="converters">定义序列化对象时的转换器集合。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        /// <remarks>该方法调用 Newtonsoft.Json.dll 组件的 JsonConvert.SerializeObject 方法来执行序列化操作。</remarks>
        public static string ToJson(this object _this, Formatting formatting, params JsonConverter[] converters)
        {
            return JsonSerializer.Serialize(_this, formatting, converters);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="type">定义序列化对象的转换类型。</param>
        /// <param name="settings">定义序列化对象时采用的序列化设置。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        /// <remarks>该方法调用 Newtonsoft.Json.dll 组件的 JsonConvert.SerializeObject 方法来执行序列化操作。</remarks>
        public static string ToJson(this object _this, Type type, JsonSerializerSettings settings)
        {
            return JsonSerializer.Serialize(_this, type, settings);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="type">定义序列化对象的转换类型。</param>
        /// <param name="formatting">定义序列化结果的输出格式。</param>
        /// <param name="settings">定义序列化对象时采用的序列化设置。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        /// <remarks>该方法调用 Newtonsoft.Json.dll 组件的 JsonConvert.SerializeObject 方法来执行序列化操作。</remarks>
        public static string ToJson(this object _this, Type type, Formatting formatting, JsonSerializerSettings settings)
        {
            return JsonSerializer.Serialize(_this, type, formatting, settings);
        }


    }
}
