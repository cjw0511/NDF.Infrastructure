using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Web.Http
{
    /// <summary>
    /// 提供一组对 <see cref="System.Net.Http.HttpContent"/> 对象操作方法的扩展。
    /// </summary>
    public static class HttpContentExtensions
    {

        /// <summary>
        /// 以异步方式将 HTTP 请求内容写入流。
        /// <para>该方法与 ReadAsStreamAsync 方法的差异在于：ReadAsStreamAsync 方法只能返回一次有效可读取内容的流对象，而该方法可返回多次。</para>
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static Task<Stream> ReadContentAsStreamAsync(this HttpContent _this)
        {
            Check.NotNull(_this);
            return _this.ReadAsStreamAsync().ContinueWith(task =>
                {
                    Stream stream = new BufferedStream(task.Result);
                    return stream;
                });
        }

        /// <summary>
        /// 将 HTTP 请求内容写入流。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static Stream ReadContentAsStream(this HttpContent _this)
        {
            return ReadContentAsStreamAsync(_this).Result;
        }


        /// <summary>
        /// 以异步方式将 HTTP 请求内容写入字节数组。
        /// <para>该方法与 ReadAsByteArrayAsync 方法的差异在于：ReadAsByteArrayAsync 方法只能返回一次有效可读取内容的字节数组，而该方法可返回多次。</para>
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static Task<byte[]> ReadContentAsByteArrayAsync(this HttpContent _this)
        {
            return ReadContentAsStreamAsync(_this).ContinueWith(task =>
                {
                    Stream stream = task.Result;
                    long position = stream.Position;
                    if (stream.CanSeek)
                        stream.Position = 0;

                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);

                    if (stream.CanSeek)
                        stream.Position = position;

                    return buffer;
                });
        }

        /// <summary>
        /// 将 HTTP 请求内容写入字节数组。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static byte[] ReadContentAsByteArray(this HttpContent _this)
        {
            return ReadContentAsByteArrayAsync(_this).Result;
        }


        /// <summary>
        /// 以异步方式获取 HTTP 请求内容中的完整字符串文本。
        /// <para>该方法与 ReadAsStringAsync 方法的差异在于：ReadAsStringAsync 方法只能返回一次有效可读取内容的字符串，而该方法可返回多次。</para>
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static Task<string> ReadContentAsStringAsync(this HttpContent _this)
        {
            return ReadContentAsStreamAsync(_this).ContinueWith(task =>
                {
                    Stream stream = task.Result;
                    long position = stream.Position;
                    if (stream.CanSeek)
                        stream.Position = 0;

                    StreamReader reader = new StreamReader(stream);
                    string text = reader.ReadToEnd();
                    
                    if (stream.CanSeek)
                        stream.Position = position;

                    return text;
                });
        }

        /// <summary>
        /// 获取 HTTP 请求内容中的完整字符串文本。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string ReadContentAsString(this HttpContent _this)
        {
            return ReadContentAsStringAsync(_this).Result;
        }


    }
}
