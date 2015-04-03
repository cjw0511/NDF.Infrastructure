using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Redis
{
    /// <summary>
    /// 为 StackExchange.Redis 组件提供一组扩展 API。
    /// </summary>
    public static class StackExchangeRedisExtensions
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this IDatabase cache, string key)
        {
            T ret = Deserialize<T>(cache.StringGet(key));
            return ret;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(this IDatabase cache, string key)
        {
            return Deserialize<object>(cache.StringGet(key));
        }


        /// <summary>
        /// Set
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(this IDatabase cache, string key, object value)
        {
            var data = Serialize(value);
            cache.StringSet(key, data);
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(this IDatabase cache, string key, T value)
        {
            var data = Serialize(value);
            cache.StringSet(key, data);
        }


        /// <summary>
        /// Serialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        static byte[] Serialize<T>(T o)
        {
            if (o == null)
                return null;

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, o);
                byte[] data = stream.ToArray();
                return data;
            }
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        static T Deserialize<T>(byte[] data)
        {
            if (data == null || data.Length == 0)
                return default(T);

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(data))
            {
                T result = (T)formatter.Deserialize(stream);
                return result;
            }
        }


    }
}
