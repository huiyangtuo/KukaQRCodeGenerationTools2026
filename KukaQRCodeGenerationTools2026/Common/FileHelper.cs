using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KukaQRCodeGenerationTools2026.Common
{
    internal class FileHelper
    {
        /// <summary>
        /// 将内容写入文件中
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileShare"></param>
        public static void Write(string filePath, string content, FileMode fileMode = FileMode.Create, FileShare fileShare = FileShare.Read)
        {
            using (FileStream fs = new FileInfo(filePath).Open(fileMode, FileAccess.Write, fileShare))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                fs.Write(buffer, 0, buffer.Length);
            }
        }
        /// <summary>
        /// 将内容写入文件中（异步）
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileShare"></param>
        public static async Task WriteAsync(string filePath, string content, FileMode fileMode = FileMode.Create, FileShare fileShare = FileShare.Read)
        {
            using (FileStream fs = new FileInfo(filePath).Open(fileMode, FileAccess.Write, fileShare))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                await fs.WriteAsync(buffer.AsMemory(0, buffer.Length));
            }
        }
        /// <summary>
        /// 读取文件的内容，并返回字符串
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileShare"></param>
        /// <returns></returns>
        public static string Read(string filePath, FileMode fileMode = FileMode.Open, FileShare fileShare = FileShare.Read)
        {
            using (FileStream fs = new FileInfo(filePath).Open(fileMode, FileAccess.Read, fileShare))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);

                return Encoding.UTF8.GetString(buffer);
            }
        }
        /// <summary>
        /// 读取文件的内容，并返回字符串
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileShare"></param>
        /// <returns></returns>
        public static async Task<string> ReadAsync(string filePath, FileMode fileMode = FileMode.Open, FileShare fileShare = FileShare.Read)
        {
            using (FileStream fs = new FileInfo(filePath).Open(fileMode, FileAccess.Read, fileShare))
            {
                byte[] buffer = new byte[fs.Length];
                await fs.ReadAsync(buffer.AsMemory(0, buffer.Length));

                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}
