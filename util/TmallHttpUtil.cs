
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace util
{
    public class TmallHttpUtil
    {
        public static String genSign(JObject jsonParam, string secretKey) {
            // treeMap 默认安名字的升序排
            StringBuilder builder = new StringBuilder();
            foreach (var entry in jsonParam) {
                if ("sign"==entry.Key || entry.Value == null) {
                    continue;
                }
                builder.Append(entry.Key)
                        .Append("=")
                        .Append(entry.Value)
                        .Append("&");
            }
            builder.Append("key=").Append(secretKey);
            return null;// DigestUtils.md5Hex(builder.toString()).toUpperCase();
        }

        public static IDictionary<string, string> buildParamMap(IDictionary<string, string> jsonParam, string method, string appKey, string appSecret, string accessToken) {
            jsonParam.Add("method", method);
            jsonParam.Add("app_key", appKey);
            jsonParam.Add("session", accessToken);
            jsonParam.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            jsonParam.Add("format", "json");
            jsonParam.Add("v", "2.0");
            jsonParam.Add("sign_method", "md5");

            string sign = generateSign(jsonParam, appSecret, "md5");
            jsonParam.Add("sign", sign);
            return jsonParam;
        }

        public static string generateSign(IDictionary<string, string> parameters, string secret, string signMethod)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            if (Constants.SIGN_METHOD_MD5.Equals(signMethod))
            {
                query.Append(secret);
            }
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }

            // 第三步：使用MD5/HMAC加密
            byte[] bytes;
            if (Constants.SIGN_METHOD_HMAC.Equals(signMethod))
            {
                HMACMD5 hmac = new HMACMD5(Encoding.UTF8.GetBytes(secret));
                bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }
            else
            {
                query.Append(secret);
                MD5 md5 = MD5.Create();
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }

    }
}