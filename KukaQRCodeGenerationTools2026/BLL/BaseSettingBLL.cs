using KukaQRCodeGenerationTools2026.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KukaQRCodeGenerationTools2026.Common;
using Newtonsoft.Json;

namespace KukaQRCodeGenerationTools2026.BLL
{
    internal class BaseSettingBLL
    {
        /// <summary>
        /// 读取服务器设置
        /// </summary>
        /// <returns></returns>
        public static ServerSettingModel? ReadServerSetting()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Setting", "ServerSetting.json");
            if (!File.Exists(filePath))
                return null;

            string content = FileHelper.Read(filePath);
            if (string.IsNullOrEmpty(content))
                return null;

            ServerSettingModel? serverSetting = JsonConvert.DeserializeObject<ServerSettingModel>(content);
            return serverSetting;
        }

        /// <summary>
        /// 保存服务器设置
        /// </summary>
        /// <param name="serverSetting"></param>
        /// <returns></returns>
        public static bool SaveServerSetting(ServerSettingModel serverSetting)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Setting");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            filePath = Path.Combine(filePath, "ServerSetting.json");

            string content = JsonConvert.SerializeObject(serverSetting, Formatting.Indented);
            FileHelper.Write(filePath, content);

            return true;
        }
    }
}
