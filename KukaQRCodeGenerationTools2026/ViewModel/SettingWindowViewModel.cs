using KukaQRCodeGenerationTools2026.Api.KMres;
using KukaQRCodeGenerationTools2026.BLL;
using KukaQRCodeGenerationTools2026.Common;
using KukaQRCodeGenerationTools2026.Common.Wpf;
using KukaQRCodeGenerationTools2026.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KukaQRCodeGenerationTools2026.ViewModel
{
    internal class SettingWindowViewModel : NotifyBase
    {
        public Action<string> ErrorMessageEvent;
        public Action<string> WarningMessageEvent;
        public Action<string> InfoMessageEvent;

        public SettingWindowViewModel()
        {
            ReadServerSetting();     // 读取服务器设置
        }

        #region 服务器配置

        private string _ServerIp;
        /// <summary>
        /// 服务器 IP 地址
        /// </summary>
        public string ServerIp { get => _ServerIp; set => SetProperty(ref _ServerIp, value); }

        private string _ServerIpMsg;
        /// <summary>
        /// 服务器 IP 地址 提示信息
        /// </summary>
        public string ServerIpMsg { get => _ServerIpMsg; set => SetProperty(ref _ServerIpMsg, value); }

        private string _ServerUsername;
        /// <summary>
        /// 服务器用户名
        /// </summary>
        public string ServerUsername { get => _ServerUsername; set => SetProperty(ref _ServerUsername, value); }

        private string _ServerUsernameMsg;
        /// <summary>
        /// 服务器用户名 提示信息
        /// </summary>
        public string ServerUsernameMsg { get => _ServerUsernameMsg; set => SetProperty(ref _ServerUsernameMsg, value); }

        private string _ServerPassword;
        /// <summary>
        /// 服务器用户密码
        /// </summary>
        public string ServerPassword { get => _ServerPassword; set => SetProperty(ref _ServerPassword, value); }

        private string _ServerPasswordMsg;
        /// <summary>
        /// 服务器用户密码 提示信息
        /// </summary>
        public string ServerPasswordMsg { get => _ServerPasswordMsg; set => SetProperty(ref _ServerPasswordMsg, value); }

        /// <summary>
        /// 测试连接 Command
        /// </summary>
        public ICommand TestConnectCommand => new CommandBase(param =>
        {
            try
            {
                bool valid = true;
                ServerIpMsg = string.Empty;
                if (string.IsNullOrEmpty(ServerIp))
                {
                    ServerIpMsg = "请填写服务器IP";
                    valid = false;
                }
                if (valid == false)
                    return;

                string ipAddress = ServerIp;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        bool isIpReacgable = await IsIpReachableAsync(ipAddress);
                        if (isIpReacgable)
                        {
                            InfoMessageEvent?.Invoke("连接成功！");
                        }
                        else
                        {
                            ErrorMessageEvent?.Invoke("连接失败，请检查网络是否连接");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                        ErrorMessageEvent?.Invoke(ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                ErrorMessageEvent?.Invoke(ex.Message);
            }
        });

        /// <summary>
        /// IP 是否可以 Ping 通（异步）
        /// </summary>
        private async Task<bool> IsIpReachableAsync(string ip, int timeout = 1000)
        {
            using Ping ping = new();
            // 发送 ping 请求，超时时间（毫秒）
            PingReply reply = await ping.SendPingAsync(ip, timeout);
            return reply.Status == IPStatus.Success;
        }

        /// <summary>
        /// 服务器表单验证
        /// </summary>
        private bool ValidateServerForm()
        {
            ResetServerFormValid();     // 重置服务器表单验证

            bool valid = true;

            if (string.IsNullOrEmpty(ServerIp))
            {
                ServerIpMsg = "请填写服务器 IP";
                valid = false;
            }
            if (string.IsNullOrEmpty(ServerUsername))
            {
                ServerUsernameMsg = "请填写用户名";
                valid = false;
            }
            if (string.IsNullOrEmpty(ServerPassword))
            {
                ServerPasswordMsg = "请填写密码";
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// 重置服务器表单验证
        /// </summary>
        private void ResetServerFormValid()
        {
            ServerIpMsg = string.Empty;
            ServerUsernameMsg = string.Empty;
            ServerPasswordMsg = string.Empty;
        }

        private string _ServerUserToken;
        /// <summary>
        /// KUKA 服务器的 用户 Token
        /// </summary>
        public string ServerUserToken { get => _ServerUserToken; set => SetProperty(ref _ServerUserToken, value); }

        /// <summary>
        /// 验证用户 Command
        /// </summary>
        public ICommand ValidateUserCommand => new CommandBase(param =>
        {
            bool formValid = ValidateServerForm();
            if (formValid == false)
                return;

            string serverIp = ServerIp;
            string username = ServerUsername;
            string password = ServerPassword;

            _ = Task.Run(async () =>
            {
                try
                {
                    string token = await ValidateUser(serverIp, username, password);
                    if (string.IsNullOrEmpty(token))
                    {
                        ErrorMessageEvent?.Invoke("用户验证失败");
                        return;
                    }

                    ServerUserToken = token.StartsWith("Bearer ") ? token.Remove(0, 7) : token;
                    InfoMessageEvent?.Invoke("验证通过！");
                }
                catch (ApplicationException ex)
                {
                    ErrorMessageEvent?.Invoke(ex.Message);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    ErrorMessageEvent?.Invoke(ex.Message);
                }
            });
        });

        /// <summary>
        /// 验证用户，并返回用户 Token
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>用户 Token</returns>
        private async Task<string> ValidateUser(string ipAddress, string username, string password)
        {
            var resLogin = await LoginApi.Login(ipAddress, new LoginApi.Login_RequestData()
            {
                Username = username,
                Password = password,
            });
            if (resLogin == null || resLogin.Success == false)
            {
                throw new ApplicationException($"发生异常，返回内容:{JsonConvert.SerializeObject(resLogin)}");
            }

            string userToken = resLogin.Data.Token;
            return userToken;
        }

        /// <summary>
        /// 保存服务器设置
        /// </summary>
        /// <returns></returns>
        private bool SaveServerSetting()
        {
            try
            {
                bool result = BaseSettingBLL.SaveServerSetting(new()
                {
                    Ip = ServerIp,
                    Username = ServerUsername,
                    Password = ServerPassword,
                    Token = ServerUserToken,
                });

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                ErrorMessageEvent?.Invoke(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 读取服务器设置
        /// </summary>
        /// <returns></returns>
        private bool ReadServerSetting()
        {
            try
            {
                ServerSettingModel? serverSetting = BaseSettingBLL.ReadServerSetting();
                if (serverSetting == null)
                    return true;

                ServerIp = serverSetting.Ip;
                ServerUsername = serverSetting.Username;
                ServerPassword = serverSetting.Password;
                ServerUserToken = serverSetting.Token;

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                ErrorMessageEvent?.Invoke(ex.Message);
                return false;
            }
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存 Command
        /// </summary>
        public ICommand SaveCommand => new CommandBase(param =>
        {
            try
            {
                SaveServerSetting();        // 保存服务器设置

                InfoMessageEvent?.Invoke("保存成功！");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                ErrorMessageEvent?.Invoke(ex.Message);
            }
        });


        #endregion
    }
}
