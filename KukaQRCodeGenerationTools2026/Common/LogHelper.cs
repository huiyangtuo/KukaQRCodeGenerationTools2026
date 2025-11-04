using System;
using System.IO;
using System.Text;

namespace KukaQRCodeGenerationTools2026.Common
{
    internal class LogHelper
    {
        private static event Action<string> LogEvent;
        private static event Action<string, DateTime> LogWithTimeEvent;
        private static event Action<string[]> LogsEvent;
        private static event Action<Exception> ExceptionEvent;
        private static event Action<string> ErrorEvent;

        static LogHelper()
        {
            LogEvent += message => AddLog("Info", message);
            LogWithTimeEvent += (message, time) => AddLogWithTime("Info", time, message);
            ErrorEvent += message => AddLog("Error", message);
            LogsEvent += messages =>
            {
                DeleteThreeMonths();

                //string path = $"{Environment.CurrentDirectory}\\Logs\\{DateTime.Now:yyyy-MM}";
                string path = Path.Combine(new string[] {
                    Environment.CurrentDirectory,
                    "Logs",
                    $"{DateTime.Now:yyyy-MM}",
                });
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                //path += $"\\{DateTime.Now:yyyy-MM-dd HH}.log";
                path = Path.Combine(path, $"{DateTime.Now:yyyy-MM-dd HH}.log");

                string content = "";
                foreach (string message in messages)
                {
                    content += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][Info]{message}\r\n";
                }
                using (FileStream fs = new FileInfo(path).Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(content);
                    fs.Write(buffer, 0, buffer.Length);
                }
            };
            ExceptionEvent += ex =>
            {
                DeleteThreeMonths();

                //string path = $"{Environment.CurrentDirectory}\\Logs\\{DateTime.Now:yyyy-MM}";
                string path = Path.Combine(new string[] {
                    Environment.CurrentDirectory,
                    "Logs",
                    $"{DateTime.Now:yyyy-MM}",
                });
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                //path += $"\\{DateTime.Now.ToString("yyyy-MM-dd HH")}.log";
                path = Path.Combine(path, $"{DateTime.Now:yyyy-MM-dd HH}.log");

                Guid errorId = Guid.NewGuid();
                DateTime now = DateTime.Now;

                string message =
                    $"[{now:yyyy-MM-dd HH:mm:ss}][Error][{errorId}]Exception Message: {ex.Message}\r\n" +
                    $"[{now:yyyy-MM-dd HH:mm:ss}][Error][{errorId}]Exception StackTrace: {ex.StackTrace}\r\n";
                using (FileStream fs = new FileInfo(path).Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    fs.Write(buffer, 0, buffer.Length);
                }
            };
        }
        private static void AddLog(string logType, string message)
        {
            DeleteThreeMonths();

            string baseUrl = Environment.CurrentDirectory;

            //string path = $"{Environment.CurrentDirectory}\\Logs\\{DateTime.Now:yyyy-MM}";
            string path = Path.Combine(new string[] {
                    baseUrl,
                    "Logs",
                    $"{DateTime.Now:yyyy-MM}",
                });
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            //path += $"\\{DateTime.Now:yyyy-MM-dd HH}.log";
            path = Path.Combine(path, $"{DateTime.Now:yyyy-MM-dd HH}.log");

            message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][{logType}]{message}\r\n";
            using (FileStream fs = new FileInfo(path).Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                fs.Write(buffer, 0, buffer.Length);
            }
        }
        private static void AddLogWithTime(string logType, DateTime time, string message)
        {
            DeleteThreeMonths();

            string baseUrl = Environment.CurrentDirectory;

            //string path = $"{Environment.CurrentDirectory}\\Logs\\{DateTime.Now:yyyy-MM}";
            string path = Path.Combine(new string[] {
                    baseUrl,
                    "Logs",
                    $"{DateTime.Now:yyyy-MM}",
                });
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            //path += $"\\{DateTime.Now:yyyy-MM-dd HH}.log";
            path = Path.Combine(path, $"{DateTime.Now:yyyy-MM-dd HH}.log");

            //message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][{logType}]{message}\r\n";
            message = $"[{time:yyyy-MM-dd HH:mm:ss}][{logType}]{message}\r\n";
            using (FileStream fs = new FileInfo(path).Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                fs.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志信息</param>
        public static void Log(string message)
        {
            LogEvent?.Invoke(message);
        }

        public static void Log(string[] messages)
        {
            LogsEvent?.Invoke(messages);
        }

        /// <summary>
        /// 记录日志并带有时间
        /// </summary>
        /// <param name="message"></param>
        /// <param name="time"></param>
        public static void LogWithTime(string message, DateTime time)
        {
            LogWithTimeEvent?.Invoke(message, time);
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            ExceptionEvent?.Invoke(ex);
        }

        public static void Error(string message)
        {
            ErrorEvent?.Invoke(message);
        }

        private static void DeleteThreeMonths()
        {
            string directoryPath = $"{Environment.CurrentDirectory}\\Logs\\{DateTime.Now.AddMonths(-3):yyyy-MM}";
            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);
        }
    }
}
