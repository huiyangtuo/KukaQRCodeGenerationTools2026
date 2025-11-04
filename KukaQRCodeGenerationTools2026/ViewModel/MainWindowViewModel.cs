using KukaQRCodeGenerationTools2026.Common.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KukaQRCodeGenerationTools2026.ViewModel
{
    public class MainWindowViewModel : NotifyBase
    {
        public Action<string> ErrorMessageEvent;
        public Action<string> InfoMessageEvent;
        public Action<string> WarningMessageEvent;

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

        private string _CurrentBussinessMapPath;
        /// <summary>
        /// 当前地图业务底图路径
        /// </summary>
        public string CurrentBussinessMapPath { get => _CurrentBussinessMapPath; set => SetProperty(ref _CurrentBussinessMapPath, value); }

        #endregion
    }
}
