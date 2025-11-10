using KukaQRCodeGenerationTools2026.Common;
using KukaQRCodeGenerationTools2026.Common.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;

namespace KukaQRCodeGenerationTools2026.ViewModel
{
    public class MainWindowViewModel : NotifyBase
    {
        public Action<string> ErrorMessageEvent;
        public Action<string> InfoMessageEvent;
        public Action<string> WarningMessageEvent;

        public MainWindowViewModel()
        {
            GetPrinterList();   // 获取打印机列表
        }

        #region 当前地图信息

        private string _CurrentMapName;
        /// <summary>
        /// 当前地图名称
        /// </summary>
        public string CurrentMapName { get => _CurrentMapName; set => SetProperty(ref _CurrentMapName, value); }

        private string _CurrentFloorNumber;
        /// <summary>
        /// 当前地图片区编号
        /// </summary>
        public string CurrentFloorNumber { get => _CurrentFloorNumber; set => SetProperty(ref _CurrentFloorNumber, value); }

        private string _CurrentMapCode;
        /// <summary>
        /// 当前地图编码
        /// </summary>
        public string CurrentMapCode { get => _CurrentMapCode; set => SetProperty(ref _CurrentMapCode, value); }

        /// <summary>
        /// 当前地图业务底图路径
        /// </summary>
        public string CurrentBussinessMapPath { get; set; }

        #endregion

        #region 页面设置

        private class MainWindowSettingModel
        {
            public string CurrentMapName { get; set; }

            public string CurrentFloorNumber { get; set; }

            public string CurrentMapCode { get; set; }

            public string CurrentBussinessMapPath { get; set; }

            public string SelectedPrinter { get; set; }
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        public void SaveSetting()
        {
            try
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, "Setting");
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                filePath = Path.Combine(filePath, "MainWindowSetting.json");

                MainWindowSettingModel setting = new()
                {
                    CurrentMapName = CurrentMapName,
                    CurrentMapCode = CurrentMapCode,
                    CurrentFloorNumber = CurrentFloorNumber,
                    CurrentBussinessMapPath = CurrentBussinessMapPath,
                    SelectedPrinter = SelectedPrinter,
                };
                string content = JsonConvert.SerializeObject(setting, Formatting.Indented);
                FileHelper.Write(filePath, content);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                ErrorMessageEvent?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// 读取设置
        /// </summary>
        public void ReadSetting()
        {
            try
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, "Setting", "MainWindowSetting.json");
                if (!File.Exists(filePath))
                    return;

                string content = FileHelper.Read(filePath);
                if (string.IsNullOrEmpty(content))
                    return;

                MainWindowSettingModel? setting = JsonConvert.DeserializeObject<MainWindowSettingModel>(content);
                if (setting == null)
                    return;

                CurrentMapCode = setting.CurrentMapCode;
                CurrentMapName = setting.CurrentMapName;
                CurrentFloorNumber = setting.CurrentFloorNumber;
                CurrentBussinessMapPath = setting.CurrentBussinessMapPath;

                if (!string.IsNullOrEmpty(setting.SelectedPrinter))
                    SelectedPrinter = setting.SelectedPrinter;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                ErrorMessageEvent?.Invoke(ex.Message);
            }
        }

        #endregion

        #region 打印机

        private ObservableCollection<string> _PrinterList = new();
        /// <summary>
        /// 打印机列表
        /// </summary>
        public ObservableCollection<string> PrinterList { get => _PrinterList; set => SetProperty(ref _PrinterList, value); }

        private string _SelectedPrinter;
        /// <summary>
        /// 选中的打印机
        /// </summary>
        public string SelectedPrinter { get => _SelectedPrinter; set => SetProperty(ref _SelectedPrinter, value); }

        /// <summary>
        /// 获取打印机列表
        /// </summary>
        private void GetPrinterList()
        {
            try
            {
                string[] _printerList = new string[PrinterSettings.InstalledPrinters.Count];
                PrinterSettings.InstalledPrinters.CopyTo(_printerList, 0);

                PrinterList = new(_printerList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                ErrorMessageEvent?.Invoke(ex.Message);
            }
        }

        #endregion
    }
}
