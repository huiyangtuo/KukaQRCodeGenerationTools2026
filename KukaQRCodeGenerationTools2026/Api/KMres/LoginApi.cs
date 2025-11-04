using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KukaQRCodeGenerationTools2026.Api.KMres
{
    internal class LoginApi
    {
        public class Login_RequestData
        {
            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        }

        public class Login_ResponseData
        {
            [JsonProperty("token")]
            public string Token { get; set; }
        }

        /// <summary>
        /// 登录 KMRes
        /// </summary>
        /// <param name="serverIp">服务器 IP</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<KukaResponseModel<Login_ResponseData>> Login(string serverIp, Login_RequestData param = null)
        {
            if (param != null)
            {
                param.Username = !string.IsNullOrEmpty(param.Username) ? param.Username : "huayitao";
                //param.Password = !string.IsNullOrEmpty(param.Password) ? param.Password : "112f36e857dd7943092c6eeaaf190973";
                param.Password = !string.IsNullOrEmpty(param.Password) ? Md5Encrypt(param.Password) : Md5Encrypt("huayitao");
            }
            else
            {
                param = new()
                {
                    Username = "huayitao",
                    //Password = "112f36e857dd7943092c6eeaaf190973",
                    Password = Md5Encrypt("huayitao"),
                };
            }

            return await HytHttpClient.PostAsync<KukaResponseModel<Login_ResponseData>>($"http://{serverIp}:5000/apibd/api/v1/data/sys-user/login", param);
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static string Md5Encrypt(string input, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // 默认为 UTF8 编码
            encoding ??= Encoding.UTF8;

            // 将字符串转换为字节数组
            byte[] inputBytes = encoding.GetBytes(input);

            // 计算 MD5 哈希值
            using MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // 将字节数组转换为十六进制字符串
            StringBuilder sb = new();
            foreach (byte b in hashBytes)
            {
                // 格式化为两位十六进制，不足两位补0（小写）
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
           
        }
    }
}
