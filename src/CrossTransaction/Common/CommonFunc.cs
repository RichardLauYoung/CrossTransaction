using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.International.Converters.PinYinConverter;

namespace CrossTransaction.Common
{
    public static class CommonFuncs
    {
        #region Hash

        public static string GetHash(this string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            var byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            var byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        #endregion

        #region<<拼音>>



        /// <summary>
        /// 转首字母大写
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string Convert2FirstSpell(this string words)
        {
            if (string.IsNullOrWhiteSpace(words)) return string.Empty;
            try
            {
                var pinyin = string.Empty;
                //逐字进行转换
                for (var i = 0; i < words.Length; i++)
                {
                    var ch = Convert.ToChar(words[i]);
                    //识别给出的字符串是否是一个有效的汉字字符。 
                    if (ChineseChar.IsValidChar(ch))
                    {
                        var chineseChar = new ChineseChar(ch);
                        var readOnlyDinosaurs = chineseChar.Pinyins;
                        pinyin += readOnlyDinosaurs[chineseChar.PinyinCount - 1].Substring(0, 1).ToUpper();
                    }
                    else pinyin += ch;
                }
                return pinyin;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 转中文拼音
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string Convert2ChineseSpell(this string words)
        {
            if (string.IsNullOrWhiteSpace(words)) return string.Empty;
            var r = new Regex(@"\D*");
            try
            {
                var pinyin = string.Empty;
                //逐字进行转换
                for (var i = 0; i < words.Length; i++)
                {
                    var ch = Convert.ToChar(words[i]);
                    //识别给出的字符串是否是一个有效的汉字字符。 
                    if (ChineseChar.IsValidChar(ch))
                    {
                        var chineseChar = new ChineseChar(ch);
                        var readOnlyDinosaurs = chineseChar.Pinyins;
                        var py = readOnlyDinosaurs[chineseChar.PinyinCount - 1].ToUpper();
                        py = r.Match(py).ToString();
                        pinyin += py;
                    }
                    else pinyin += ch;
                }
                return pinyin;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #endregion

        #region<<数字计算>>

        /// <summary>
        /// 四舍五入到int
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static int DecimalToInt(this decimal dec)
        {
            var mr = Math.Round(dec);
            return (int)mr;
        }

        #endregion

        #region<<枚举方法>>

        /// <summary>
        /// 获取枚举名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="sender">枚举</param>
        /// <returns>名称</returns>
        public static string GetEnumName<T>(this T sender)
        {
            return Enum.GetName(typeof(T), sender);
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="sender">枚举</param>
        /// <returns>对应属性</returns>
        public static string ToDescription<T>(this T sender)
        {
            var type = typeof(T);
            var info = type.GetField(sender.ToString());
            var descriptionAttribute = info.GetCustomAttributes(typeof(DescriptionAttribute), true)[0] as DescriptionAttribute;

            return descriptionAttribute != null ? descriptionAttribute.Description : type.ToString();
        }

        /// <summary>
        /// 对应的字符转换为枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="valueName">名称</param>
        /// <returns>T</returns>
        public static T FromNameToEnum<T>(this string valueName)
        {
            var type = typeof(T);
            var info = type.GetField(valueName);
            if (info == null) return default(T);

            var value = (T)info.GetRawConstantValue();

            return value;
        }

        /// <summary>
        /// 根据描述定位枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="desp"></param>
        /// <returns></returns>
        public static T FromDespToEnum<T>(this string desp)
        {
            var t = typeof(T);
            var infos = t.GetFields();
            if (infos.All(x => (x.GetCustomAttributes(typeof(DescriptionAttribute), true)[0] as DescriptionAttribute) == null)) return default(T);
            var info =
                infos.First(
                    x => (x.GetCustomAttributes(typeof(DescriptionAttribute), true)[0] as DescriptionAttribute) != null);

            return (T)info.GetRawConstantValue();
        }

        /// <summary>
        /// 获取枚举列表字典
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <returns>枚举列表字典</returns>
        public static IList<EnumList> GetEnumList<T>()
        {
            var t = typeof(T);
            var infos = t.GetFields();
            if (!infos.Any()) return null;

            // 获取枚举属性赋值到列表并返回
            IList<EnumList> list = new List<EnumList>();
            for (var i = 1; i < infos.Count(); i++)
            {
                var el = new EnumList();
                var field = infos[i];
                el.Key = field.Name;
                try
                {
                    el.KeyValue = (int)Enum.Parse(t, field.Name);
                    var descriptionAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), true)[0] as DescriptionAttribute;
                    el.KeyDesc = descriptionAttribute != null ? descriptionAttribute.Description : t.ToString();
                }
                catch (Exception ex)
                {
                    el.KeyValue = 0;
                }
                list.Add(el);
            }
            return list;
        }


        /// <summary>
        /// 获取枚举列表字典
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <returns>枚举列表字典</returns>
        public static IList<EnumList> GetWeekDayList()
        {
            IList<EnumList> listWeekDay = new List<EnumList>();
            listWeekDay.Add(new EnumList
            {
                Key = "Monday",
                KeyValue = 1,
                KeyDesc = "周一"
            });
            listWeekDay.Add(new EnumList
            {
                Key = "Tuesday",
                KeyValue = 2,
                KeyDesc = "周二"
            });
            listWeekDay.Add(new EnumList
            {
                Key = "Wednesday",
                KeyValue = 4,
                KeyDesc = "周三"
            });
            listWeekDay.Add(new EnumList
            {
                Key = "Thursday",
                KeyValue = 8,
                KeyDesc = "周四"
            });
            listWeekDay.Add(new EnumList
            {
                Key = "Friday",
                KeyValue = 16,
                KeyDesc = "周五"
            });
            listWeekDay.Add(new EnumList
            {
                Key = "Saturday",
                KeyValue = 32,
                KeyDesc = "周六"
            });
            listWeekDay.Add(new EnumList
            {
                Key = "Sunday",
                KeyValue = 64,
                KeyDesc = "周日"
            });

            return listWeekDay;
        }
        /// <summary>
        /// 枚举列表类
        /// </summary>
        [Serializable]
        public class EnumList
        {
            private string key;
            private int keyValue;
            private string keyDesc;

            /// <summary>
            /// 枚举key
            /// </summary>
            public string Key
            {
                get { return key; }
                set { key = value; }
            }

            /// <summary>
            /// 枚举value
            /// </summary>
            public int KeyValue
            {
                get { return keyValue; }
                set { keyValue = value; }
            }

            /// <summary>
            /// 枚举对应enum值描述
            /// </summary>
            public string KeyDesc
            {
                get { return keyDesc; }
                set { keyDesc = value; }
            }
        }

        #endregion

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <returns>int</returns>
        public static int GetRandomInt()
        {
            var tick = DateTime.Now.Ticks;
            var ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return ran.Next();
        }

        /// <summary>
        /// 获取指定最大值的随机数
        /// </summary>
        /// <param name="up">随机数的最大值</param>
        /// <returns>int</returns>
        public static int GetRandomInt(int up)
        {
            var tick = DateTime.Now.Ticks;
            var ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return ran.Next(up);
        }

        /// <summary>
        /// 获取指定范围内的随机数
        /// </summary>
        /// <param name="down">最小值</param>
        /// <param name="up">最大值</param>
        /// <returns>int</returns>
        public static int GetRandomInt(int down, int up)
        {
            var tick = DateTime.Now.Ticks;
            var ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return ran.Next(down, up);
        }

        /// <summary>
        /// 生成20位编号(yyyyMMddHHmmss+6位随机数)
        /// </summary>
        /// <returns></returns>
        public static string GenerateCode()
        {
            var code = DateTime.Now.ToString("yyyyMMddHHmmss");
            var end = new Object().GetHashCode().ToString();
            end = end.Length > 6 ? end.Substring(end.Length - 6) : end.PadLeft(6, '0');

            return code + end;
        }


        /// <summary>
        /// “四舍五入”，若取舍部分大于a，则进一
        /// </summary>
        /// <param name="v"></param>
        /// <param name="a">取舍阀值</param>
        /// <returns></returns>
        public static int Round(this double v, double a = 0.5d)
        {
            int value = (int)Math.Ceiling(v - a);
            return value;
        }

        /// <summary>
        /// “四舍五入”，若取舍部分大于a，则进一
        /// </summary>
        /// <param name="v"></param>
        /// <param name="a">取舍阀值</param>
        /// <returns></returns>
        public static int Round(this float v, float a = 0.5f)
        {
            int value = (int)Math.Ceiling(v - a);
            return value;
        }

        /// <summary>
        /// 将数字字符转换成整型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this object obj, int defaultValue = 0)
        {
            if (obj == null) return defaultValue;
            int tmp;
            var rs = int.TryParse(obj.ToString(), out tmp);
            return rs ? tmp : defaultValue;
        }

        /// <summary>
        /// 将数字字符串转换成长整型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToLong(this object obj, long defaultValue = 0)
        {
            if (obj == null) return defaultValue;
            long tmp;
            var rs = long.TryParse(obj.ToString(), out tmp);
            return rs ? tmp : defaultValue;
        }

        /// <summary>
        /// 转换成double类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this object obj, double defaultValue = 0)
        {
            if (obj == null) return defaultValue;
            double tmp;
            var rs = double.TryParse(obj.ToString(), out tmp);
            return rs ? tmp : defaultValue;
        }

        /// <summary>
        /// 将数字字符转换成decimal类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object obj, decimal defaultValue = 0)
        {
            if (obj == null) return defaultValue;
            decimal tmp;
            var rs = decimal.TryParse(obj.ToString(), out tmp);
            return rs ? tmp : defaultValue;
        }

        /// <summary>
        /// 将数字字符转换成float类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(this object obj, float defaultValue = 0)
        {
            if (obj == null) return defaultValue;
            float tmp;
            var rs = float.TryParse(obj.ToString(), out tmp);
            return rs ? tmp : defaultValue;
        }

        /// <summary>
        /// 将时间字符串转化为DateTime，时间字符串格式非法则返回null
        /// </summary>
        /// <param name="strTime">时间字符串</param>
        /// <param name="format">字符串时间格式，如：MM/dd/yyyy</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string strTime, string format)
        {
            DateTime dt;
            if (DateTime.TryParseExact(strTime, format, null, System.Globalization.DateTimeStyles.None, out dt)) return dt;
            return null;
        }

        /// <summary>
        /// <para>对于null, "", string.Empty，是相等的，返回true</para>
        /// <para>其他情况返回obj.Equals(obj2)</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj2">要比较的字符串</param>
        /// <returns></returns>
        public static bool IsGeneralEqual(this object obj, object obj2)
        {
            if (obj == null && obj2 == null) return true;
            if (obj != null && obj.Equals(obj2)) return true;
            if (obj is string)
            {
                if ((obj as string).IsNullOrEmpty() && (obj2 == null || (obj2 is string && (obj2 as string).IsNullOrEmpty()))) return true;
            }
            else if (obj2 is string)
            {
                if ((obj2 as string).IsNullOrEmpty() && (obj == null || (obj is string && (obj as string).IsNullOrEmpty()))) return true;
            }
            return false;
        }

        /// <summary>
        /// 验证字符串是否匹配pattern表示的正则表达式
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern">正则表达式</param>
        /// <returns></returns>
        public static bool IsMatch(this string s, string pattern)
        {
            if (s == null) return false;
            var r = new Regex(pattern);
            return r.IsMatch(s);
        }

        /// <summary>
        /// 判断列表是否为null或元素个数是否为0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> lst)
        {
            return (lst == null || !lst.Any());
        }

        /// <summary>
        /// 移除重复的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public static T[] Distinct<T>(this IEnumerable<T> lst, Func<T, object> fieldValue)
        {
            if (lst.IsNullOrEmpty()) return new T[0];

            var tmpList = new List<T>();
            foreach (var a in lst)
            {
                if (!tmpList.Any(b =>
                {
                    var t1 = fieldValue(b);
                    var t2 = fieldValue(a);
                    if (t1 == null && t2 == null || t1 == t2) return true;
                    return t1 != null && t1.Equals(t2);
                })) tmpList.Add(a);
            }

            return tmpList.ToArray();
        }

        /// <summary>
        /// 判断列表中是否有相同的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static bool HasSameElement<T>(this IEnumerable<T> lst)
        {
            return lst.Distinct().Count() != lst.Count();
        }

        /// <summary>
        /// 从列表中移除元素为空的项
        /// </summary>
        /// <param name="lst"></param>
        public static void RemoveEmptyEntries(this List<string> lst)
        {
            if (lst.IsNullOrEmpty()) return;
            lst.RemoveAll(a => a.IsNullOrEmpty());
        }

        /// <summary>
        /// 将列表中元素组合成字符串
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="combineStr"></param>
        /// <param name="removeEmptyElement"></param>
        /// <returns></returns>
        public static string CombineElements(this IEnumerable<string> lst, string combineStr, bool removeEmptyElement = true)
        {
            if (lst.IsNullOrEmpty()) return string.Empty;

            List<string> tmpList = lst.ToList();
            if (removeEmptyElement) tmpList.RemoveEmptyEntries();
            if (tmpList.IsNullOrEmpty()) return string.Empty;
            if (tmpList.Count() == 1) return lst.ElementAt(0);
            return tmpList.Aggregate((a, b) => string.Format("{0}{1}{2}", a, combineStr, b));
        }

        /// <summary>
        /// 验证列表中的元素是否有序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static bool ElementsInOrder(this IEnumerable<int> arr)
        {
            if (arr == null) return false;
            if (arr.Count() <= 2) return true;
            for (int i = 0; i < arr.Count() - 2; i++)
            {
                if (arr.ElementAt(i) >= arr.ElementAt(i + 1) && arr.ElementAt(i + 1) <= arr.ElementAt(i + 2)) return false;
                if (arr.ElementAt(i) <= arr.ElementAt(i + 1) && arr.ElementAt(i + 1) >= arr.ElementAt(i + 2)) return false;
            }
            return true;
        }

        /// <summary>
        /// 获取列表中元素所在的索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static int GetIndex<T>(this IList<T> lst, Func<T, bool> predicate)
        {
            if (lst.IsNullOrEmpty()) return -1;
            for (int i = 0; i < lst.Count; i++)
            {
                if (predicate(lst[i])) return i;
            }
            return -1;
        }

        /// <summary>
        /// 判断字符串是否为中文字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsChineseChar(this string s)
        {
            return s.IsMatch("^[\u4e00-\u9fa5]$");
        }

        /// <summary>
        /// 将中文字符串转换成拼音
        /// </summary>
        /// <param name="s"></param>
        /// <param name="splitString"></param>
        /// <returns></returns>
        public static string GetChineseSpell(this string s, bool lowerCase = true, string splitString = null)
        {
            string tmp = ChineseSpell.GetSpell(s, splitString).ToLower();
            if (!lowerCase) tmp = tmp.ToUpper();
            return tmp;
        }

        /// <summary>
        /// 将中文字符串转换成拼音首字母
        /// </summary>
        /// <param name="s"></param>
        /// <param name="lowerCase"></param>
        /// <param name="splitString"></param>
        /// <returns></returns>
        public static string GetChineseFirstSpells(this string s, bool lowerCase = true, string splitString = null)
        {
            if (lowerCase) return ChineseSpell.GetFirstSpellLower(s, splitString).ToLower();
            return ChineseSpell.GetFirstSpellSupper(s, splitString).ToUpper();
        }

        /// <summary>
        /// UTCToDateTime
        /// </summary>
        /// <param name="utcSecond"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long utcSecond)
        {
            DateTime dtZone = new DateTime(1970, 1, 1, 0, 0, 0);
            dtZone = dtZone.AddSeconds(utcSecond);
            return dtZone.ToLocalTime();
        }

        /// <summary>
        /// DateTimeToUTC
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUTC(this DateTime dateTime)
        {
            dateTime = dateTime.ToUniversalTime();
            DateTime dtZone = new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)dateTime.Subtract(dtZone).TotalSeconds;
        }


        /// <summary>
        /// 位运算加密
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static int BitOperationEncryption(int[] nums)
        {
            int sum = 0;
            foreach (var item in nums)
            {
                sum = sum | item;
            }
            return sum;
        }


        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encryption(string code)
        {
            string encode = string.Empty;
            md5_digui(code, 0, 100, ref encode);
            return encode;
        }

        /// <summary>
        /// 递归加密
        /// </summary>
        /// <param name="code"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="encode"></param>
        private static void md5_digui(string code, int start, int end, ref string encode)
        {
            MD5CryptoServiceProvider md5_csp = new MD5CryptoServiceProvider();
            byte[] data = md5_csp.ComputeHash(Encoding.Default.GetBytes(code));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            if (start <= end)
            {
                start++;
                encode = sBuilder.ToString();
                md5_digui(sBuilder.ToString(), start, end, ref encode);
            }
        }

        public static List<string> GetTrimString(string code, string trim_code)
        {
            List<string> ls = new List<string>();
            if (trim_code.Length > 1)
            {
                while (code.IndexOf(trim_code) >= 0)
                {
                    ls.Add(code.Substring(0, code.IndexOf(trim_code)));
                    code = code.Substring(code.IndexOf(trim_code) + trim_code.Length);
                }
                if (code.IndexOf(trim_code) < 0 && code.Length >= 1)
                {
                    ls.Add(code);
                }
            }
            else
            {
                ls = code.Split(trim_code.ToArray()).ToList();
            }
            return ls;
        }

        /// <summary>
        /// 弹屏电话号码过滤
        /// </summary>
        /// <param name="number">来电号码</param>
        /// <param name="Areacode">区号</param>
        /// <param name="type">类型:1固话，2手机</param>
        /// <returns>来电号码</returns>
        public static string CallNumberFormat(string number, ref string Areacode, ref int type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(number))
                    return "";

                if (number.Length == 12)//如果是12位数
                {

                    Areacode = "0";
                    //如果是手机开头号码
                    if (number.StartsWith("013") || number.StartsWith("015") || number.StartsWith("018"))
                    {
                        type = 2;
                        number = number.Remove(0, 1);
                    }
                    else
                    {
                        type = 1;
                        Areacode += number.Substring(0, number.Length - 8);
                        number = number.Substring(number.Length - 8, 8);
                    }
                    return number;
                }
                else if (number.Length == 11) //如果是11位数
                {
                    //如果开始数是0,就是带区号的固话
                    if (number.StartsWith("0"))
                    {
                        type = 1;
                        if (number.StartsWith("010") || number.StartsWith("021") || number.StartsWith("022") || number.StartsWith("023") || number.StartsWith("024") || number.StartsWith("027") || number.StartsWith("025") || number.StartsWith("020") || number.StartsWith("028") || number.StartsWith("029"))
                        {

                            Areacode += number.Substring(0, number.Length - 8);
                            number = number.Substring(number.Length - 8, 8);
                        }
                        else
                        {
                            Areacode += number.Substring(0, number.Length - 7);
                            number = number.Substring(number.Length - 7, 7);
                        }
                    }
                    else
                    {
                        type = 2;
                    }
                    return number;
                }
                else if (number.Length == 10) //如果是10位数
                {
                    if (number.StartsWith("0"))
                    {
                        type = 1;
                        Areacode += number.Substring(0, number.Length - 7);
                        number = number.Substring(number.Length - 7, 7);
                    }
                    return number;
                }
                else
                {
                    return number;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public sealed class Base64
    {
        /// <summary> 
        /// Base64加密
        /// /// </summary> 
        /// <param name="codeName">加密采用的编码方式</param> 
        /// <param name="source">待加密的明文</param> 
        /// <returns></returns> 
        public static string EncodeBase64(Encoding encode, string source)
        {

            byte[] bytes = encode.GetBytes(source);
            string code = string.Empty;
            try
            {

                code = Convert.ToBase64String(bytes);

            }

            catch
            {

                code = source;

            }

            return code;

        }

        /// <summary> 
        /// Base64加密，采用utf8编码方式加密
        /// </summary> 
        /// <param name="source">待加密的明文</param> 
        /// <returns>加密后的字符串</returns> 
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary> 
        /// Base64解密
        /// </summary> 
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param> 
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns> 
        public static string DecodeBase64(Encoding encode, string result)
        {

            string decode = "";

            byte[] bytes = Convert.FromBase64String(result);

            try
            {

                decode = encode.GetString(bytes);

            }

            catch
            {

                decode = result;

            }

            return decode;

        }

        /// <summary> 
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }

    }

    /// <summary>
    /// 汉字转拼音静态类,包括功能全拼和缩写，方法全部是静态的
    /// </summary>
    public class ChineseSpell
    {
        #region 属性数据定义
        /// <summary>
        /// 汉字的机内码数组
        /// </summary>
        private static int[] pyValue = new int[]
        {
            -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
            -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
            -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
            -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
            -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
            -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
            -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
            -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
            -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
            -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
            -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
            -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
            -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
            -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
            -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
            -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
            -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
            -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
            -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
            -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
            -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
            -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
            -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
            -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
            -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
            -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
            -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
            -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
            -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
            -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
            -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
            -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
            -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
        };
        /// <summary>
        /// 机内码对应的拼音数组
        /// </summary>
        private static string[] pyName = new string[]
        {
            "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
            "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
            "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
            "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
            "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
            "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
            "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
            "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
            "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
            "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
            "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
            "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
            "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
            "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
            "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
            "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
            "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
            "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
            "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
            "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
            "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
            "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
            "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
            "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
            "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
            "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
            "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
            "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
            "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
            "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
            "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
            "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
            "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
        };
        #endregion

        #region 把汉字转换成拼音(全拼) 用特定的字符间隔
        /// <summary>
        /// 把汉字转换成拼音(全拼)
        /// </summary>
        /// <param name="hzString">汉字字符串</param>
        /// <returns>转换后的拼音(全拼)字符串</returns>
        public static string GetSpell(string hzString, string splitChar = null)
        {
            if (splitChar == null) splitChar = string.Empty;
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = hzString.ToCharArray();
            for (int j = 0; j < noWChar.Length; j++)
            {
                // 中文字符
                if (noWChar[j].ToString().IsChineseChar())
                {
                    array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrAsc = i1 * 256 + i2 - 65536;
                    if (chrAsc > 0 && chrAsc < 160)
                    {
                        pyString = pyString + splitChar + noWChar[j];
                    }
                    else
                    {
                        // 修正部分文字
                        if (chrAsc == -9254)  // 修正“圳”字
                            pyString = pyString + splitChar + "Zhen";
                        else
                        {
                            for (int i = (pyValue.Length - 1); i >= 0; i--)
                            {
                                if (pyValue[i] <= chrAsc)
                                {
                                    pyString = pyString + splitChar + pyName[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                // 非中文字符
                else
                {
                    pyString = pyString + splitChar + noWChar[j].ToString();
                }
            }
            char[] trimAChar = splitChar.ToCharArray();
            return pyString.TrimStart(trimAChar);
        }
        #endregion

        #region 汉字转拼音缩写 (字符串) (小写)
        /// <summary>
        /// 汉字转拼音缩写 (字符串) (小写) (空格间隔)
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static string GetFirstSpellLower(string str, string splitString = null)
        {
            if (str.IsNullOrEmpty()) return "";
            if (splitString == null) splitString = string.Empty;

            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {
                    //字母和符号原样保留
                    tempStr = tempStr + c.ToString();
                }
                else
                {
                    //累加拼音声母
                    tempStr = tempStr + GetSpellCharLower(c.ToString());
                }
            }
            return tempStr.Trim();
        }
        #endregion

        #region 汉字转拼音缩写 (字符串)(大写)
        /// <summary>
        /// 汉字转拼音缩写  (字符串)(大写)(空格间隔)
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static string GetFirstSpellSupper(string str, string splitString = null)
        {
            return GetFirstSpellLower(str, splitString).ToUpper();
        }
        #endregion

        #region 取单个字符的拼音声母(字符)(大写)
        /// <summary>
        /// 取单个字符的拼音声母
        /// </summary>
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母</returns>
        private static string GetSpellCharSupper(string c)
        {
            return GetSpellCharLower(c).ToUpper();
        }
        #endregion

        #region 取单个字符的拼音声母(字符)(小写)
        /// <summary>
        /// 取单个字符的拼音声母
        /// </summary>
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母</returns>
        private static string GetSpellCharLower(string c)
        {
            if (!c.IsChineseChar()) return c;

            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return c;
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "j";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";
            return c;
        }
        #endregion
    }

    public class TreeTableThead
    {
        public int id;
        public string name;
        public string value;
    }
}

