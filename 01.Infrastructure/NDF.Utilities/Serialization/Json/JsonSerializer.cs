using NDF.Serialization.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NDF.Serialization.Json
{
    /// <summary>
    /// 提供一组工具方法，用于将基础对象类型 <see cref="System.Object"/> 生成 JSON 格式字符串。
    /// </summary>
    /// <remarks>该类型中将调用 Newtonsoft.Json.dll 组件来执行序列化和反序列化操作。</remarks>
    public class JsonSerializer
    {
        private JsonSerializerSettings _settings;

        /// <summary>
        /// 以默认的序列化设置初始化一个 <see cref="JsonSerializer"/> 对象。
        /// </summary>
        public JsonSerializer() : this(DefaultSettingsGenerator())
        {
        }

        /// <summary>
        /// 以指定的序列化设置初始化一个 <see cref="JsonSerializer"/> 对象。
        /// </summary>
        /// <param name="settings">指定的序列化设置。</param>
        public JsonSerializer(JsonSerializerSettings settings)
        {
            this.Settings = settings;
        }



        /// <summary>
        /// 获取或设置当前 JSON 格式字符串序列化器的默认序列化设置。
        /// </summary>
        public JsonSerializerSettings Settings
        {
            get
            {
                if (this._settings == null)
                {
                    this._settings = DefaultSettingsGenerator();
                }
                return this._settings;
            }
            set { this._settings = value; }
        }



        /// <summary>
        /// 获取一个用于返回 JSON 序列化生成字符串设置对象的委托。
        /// </summary>
        public static Func<JsonSerializerSettings> DefaultSettingsGenerator
        {
            get
            {
                return JsonConvert.DefaultSettings ?? GetDefaultSettings;
            }
        }

        private static JsonSerializerSettings GetDefaultSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(DateTimeConverters.GeneralDate);
            return settings;
        }





        /// <summary>
        /// Deserializes the JSON to the given anonymous type.
        /// </summary>
        /// <typeparam name="T">
        /// The anonymous type to deserialize to. This can't be specified traditionally
        ///     and must be infered from the anonymous type passed as a parameter.
        /// </typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <returns>The deserialized anonymous type from the JSON string.</returns>
        public T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
        {
            return this.DeserializeAnonymousType(value, anonymousTypeObject, this.Settings);
        }

        /// <summary>
        /// Deserializes the JSON to the given anonymous type using Newtonsoft.Json.JsonSerializerSettings.
        /// </summary>
        /// <typeparam name="T">
        /// The anonymous type to deserialize to. This can't be specified traditionally
        ///     and must be infered from the anonymous type passed as a parameter.
        /// </typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <param name="settings">
        /// The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object.
        ///     If this is null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized anonymous type from the JSON string.</returns>
        public T DeserializeAnonymousType<T>(string value, T anonymousTypeObject, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject, settings);
        }




        /// <summary>
        /// Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public object Deserialize(string value)
        {
            return this.Deserialize(value, this.Settings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public T Deserialize<T>(string value)
        {
            return this.Deserialize<T>(value, this.Settings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type using Newtonsoft.Json.JsonSerializerSettings.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The object to deserialize.</param>
        /// <param name="settings">
        /// The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object.
        ///     If this is null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public T Deserialize<T>(string value, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(value, settings);
        }

        /// <summary>
        /// Deserializes the JSON to a .NET object using Newtonsoft.Json.JsonSerializerSettings.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="settings">
        /// The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object.
        ///     If this is null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public object Deserialize(string value, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject(value, settings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type using a collection of Newtonsoft.Json.JsonConverter.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public T Deserialize<T>(string value, params JsonConverter[] converters)
        {
            return JsonConvert.DeserializeObject<T>(value, converters);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="type">The System.Type of object being deserialized.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public object Deserialize(string value, Type type)
        {
            return this.Deserialize(value, type, this.Settings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type using Newtonsoft.Json.JsonSerializerSettings.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="type">The type of the object to deserialize to.</param>
        /// <param name="settings">
        /// The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object.
        ///     If this is null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public object Deserialize(string value, Type type, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject(value, settings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type using a collection of Newtonsoft.Json.JsonConverter.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public object Deserialize(string value, Type type, params JsonConverter[] converters)
        {
            return JsonConvert.DeserializeObject(value, type, converters);
        }




        /// <summary>
        /// Deserializes the XmlNode from a JSON string.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <returns>The deserialized XmlNode</returns>
        public XmlDocument DeserializeXml(string value)
        {
            return JsonConvert.DeserializeXmlNode(value);
        }

        /// <summary>
        /// Deserializes the XmlNode from a JSON string nested in a root elment specified
        ///     by deserializeRootElementName.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <param name="deserializeRootElementName">The name of the root element to append when deserializing.</param>
        /// <returns>The deserialized XmlNode</returns>
        public XmlDocument DeserializeXml(string value, string deserializeRootElementName)
        {
            return JsonConvert.DeserializeXmlNode(value, deserializeRootElementName);
        }

        /// <summary>
        /// Deserializes the XmlNode from a JSON string nested in a root elment specified
        ///     by deserializeRootElementName and writes a .NET array attribute for collections.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <param name="deserializeRootElementName">The name of the root element to append when deserializing.</param>
        /// <param name="writeArrayAttribute">
        /// A flag to indicate whether to write the Json.NET array attribute.  This attribute
        ///     helps preserve arrays when converting the written XML back to JSON.
        /// </param>
        /// <returns>The deserialized XmlNode</returns>
        public XmlDocument DeserializeXml(string value, string deserializeRootElementName, bool writeArrayAttribute)
        {
            return JsonConvert.DeserializeXmlNode(value, deserializeRootElementName, writeArrayAttribute);
        }



        /// <summary>
        /// Deserializes the System.Xml.Linq.XNode from a JSON string.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <returns>The deserialized XNode</returns>
        public XDocument DeserializeXDoc(string value)
        {
            return JsonConvert.DeserializeXNode(value);
        }

        /// <summary>
        /// Deserializes the System.Xml.Linq.XNode from a JSON string nested in a root
        ///     elment specified by deserializeRootElementName.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <param name="deserializeRootElementName">The name of the root element to append when deserializing.</param>
        /// <returns>The deserialized XNode</returns>
        public XDocument DeserializeXDoc(string value, string deserializeRootElementName)
        {
            return JsonConvert.DeserializeXNode(value, deserializeRootElementName);
        }

        /// <summary>
        /// Deserializes the System.Xml.Linq.XNode from a JSON string nested in a root
        ///     elment specified by deserializeRootElementName and writes a .NET array attribute
        ///     for collections.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <param name="deserializeRootElementName">The name of the root element to append when deserializing.</param>
        /// <param name="writeArrayAttribute">
        /// A flag to indicate whether to write the Json.NET array attribute.  This attribute
        ///     helps preserve arrays when converting the written XML back to JSON.
        /// </param>
        /// <returns>The deserialized XNode</returns>
        public XDocument DeserializeXDoc(string value, string deserializeRootElementName, bool writeArrayAttribute)
        {
            return JsonConvert.DeserializeXNode(value, deserializeRootElementName, writeArrayAttribute);
        }




        /// <summary>
        /// Populates the object with values from the JSON string.
        /// </summary>
        /// <param name="value">The JSON to populate values from.</param>
        /// <param name="target">The target object to populate values onto.</param>
        public void Populate(string value, object target)
        {
            this.Populate(value, target, this.Settings);
        }

        /// <summary>
        /// Populates the object with values from the JSON string using Newtonsoft.Json.JsonSerializerSettings.
        /// </summary>
        /// <param name="value">The JSON to populate values from.</param>
        /// <param name="target">The target object to populate values onto.</param>
        /// <param name="settings">
        /// The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object.
        ///     If this is null, default serialization settings will be used.
        /// </param>
        public void Populate(string value, object target, JsonSerializerSettings settings)
        {
            JsonConvert.PopulateObject(value, target, settings);
        }




        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        public string Serialize(object _this)
        {
            return this.Serialize(_this, this.Settings);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="formatting">定义序列化结果的输出格式。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        public string Serialize(object _this, Newtonsoft.Json.Formatting formatting)
        {
            return this.Serialize(_this, formatting, this.Settings);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="settings">定义序列化对象时采用的序列化设置。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        public string Serialize(object _this, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(_this, settings);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="converters">定义序列化对象时的转换器集合。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        public string Serialize(object _this, params JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(_this, converters);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="formatting">定义序列化结果的输出格式。</param>
        /// <param name="settings">定义序列化对象时采用的序列化设置。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        public string Serialize(object _this, Newtonsoft.Json.Formatting formatting, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(_this, formatting, settings);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="formatting">定义序列化结果的输出格式。</param>
        /// <param name="converters">定义序列化对象时的转换器集合。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        public string Serialize(object _this, Newtonsoft.Json.Formatting formatting, params JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(_this, formatting, converters);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="type">定义序列化对象的转换类型。</param>
        /// <param name="settings">定义序列化对象时采用的序列化设置。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        public string Serialize(object _this, Type type, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(_this, type, settings);
        }

        /// <summary>
        /// 将一个 <see cref="System.Object"/> 对象序列化转换为 JSON 格式字符串。
        /// </summary>
        /// <param name="_this">要序列化的对象。</param>
        /// <param name="type">定义序列化对象的转换类型。</param>
        /// <param name="formatting">定义序列化结果的输出格式。</param>
        /// <param name="settings">定义序列化对象时采用的序列化设置。</param>
        /// <returns>序列化的 JSON 字符串。</returns>
        public string Serialize(object _this, Type type, Newtonsoft.Json.Formatting formatting, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(_this, type, formatting, settings);
        }
    }
}
