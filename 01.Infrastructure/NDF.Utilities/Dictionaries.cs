using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF
{
    /// <summary>
    /// 提供一组常用的静态数据字典。
    /// </summary>
    public static class Dictionaries
    {
        private static Dictionary<string, string> idCardAreas;

        /// <summary>
        /// 获取表示身份证属地区域编码(中国)的集合。
        /// 该集合中每个 KeyValuePair&lt;string, string&gt; 对象的 Key 属性表示编码号、Value 属性表示身份证属地区域名称，如 { Key: "11", Value: "北京" }。
        /// 该字典收录总计 35 个区域。
        /// </summary>
        public static Dictionary<string, string> IDCardAreas
        {
            get
            {
                if (idCardAreas == null)
                {
                    idCardAreas = new Dictionary<string, string>();
                    idCardAreas.Add("11", "北京");
                    idCardAreas.Add("12", "天津");
                    idCardAreas.Add("13", "河北");
                    idCardAreas.Add("14", "山西");
                    idCardAreas.Add("15", "内蒙古");
                    idCardAreas.Add("21", "辽宁");
                    idCardAreas.Add("22", "吉林");
                    idCardAreas.Add("23", "黑龙江");
                    idCardAreas.Add("31", "上海");
                    idCardAreas.Add("32", "江苏");
                    idCardAreas.Add("33", "浙江");
                    idCardAreas.Add("34", "安徽");
                    idCardAreas.Add("35", "福建");
                    idCardAreas.Add("36", "江西");
                    idCardAreas.Add("37", "山东");
                    idCardAreas.Add("41", "河南");
                    idCardAreas.Add("42", "湖北");
                    idCardAreas.Add("43", "湖南");
                    idCardAreas.Add("44", "广东");
                    idCardAreas.Add("45", "广西");
                    idCardAreas.Add("46", "海南");
                    idCardAreas.Add("50", "重庆");
                    idCardAreas.Add("51", "四川");
                    idCardAreas.Add("52", "贵州");
                    idCardAreas.Add("53", "云南");
                    idCardAreas.Add("54", "西藏");
                    idCardAreas.Add("61", "陕西");
                    idCardAreas.Add("62", "甘肃");
                    idCardAreas.Add("63", "青海");
                    idCardAreas.Add("64", "宁夏");
                    idCardAreas.Add("65", "新疆");
                    idCardAreas.Add("71", "台湾");
                    idCardAreas.Add("81", "香港");
                    idCardAreas.Add("82", "澳门");
                    idCardAreas.Add("91", "国外");
                }
                return idCardAreas;
            }
        }
    }
}
