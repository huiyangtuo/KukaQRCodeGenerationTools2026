using KukaQRCodeGenerationTools2026.Api.KMres;
using KukaQRCodeGenerationTools2026.BLL;
using KukaQRCodeGenerationTools2026.Common;
using KukaQRCodeGenerationTools2026.Common.Wpf;
using KukaQRCodeGenerationTools2026.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KukaQRCodeGenerationTools2026.ViewModel
{
    public class MapSelectWindowViewModel : NotifyBase
    {
        public Action<string> ErrorMessageEvent;
        public Action<string> WarningMessageEvent;
        public Action<string> InfoMessageEvent;

        public Action CloseWindowEvent;
        public Action<MapItemModel> SelectedMapEvent;

        public MapSelectWindowViewModel()
        {
            bool isServerSettingReady = CheckServerAndUserInfo();
            if (isServerSettingReady == false)
            {
                ErrorMessageEvent?.Invoke("未填写服务器设置，请先填写服务器设置");
                CloseWindowEvent?.Invoke();
                return;
            }

            // 查询地图列表
            GetMapList();
        }

        #region 检测服务器及用户信息是否填写

        private bool CheckServerAndUserInfo()
        {
            try
            {
                var serverSetting = BaseSettingBLL.ReadServerSetting();
                if (serverSetting == null)
                    return false;

                if (string.IsNullOrEmpty(serverSetting.Ip) ||
                    string.IsNullOrEmpty(serverSetting.Username) ||
                    string.IsNullOrEmpty(serverSetting.Password))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region 查询地图列表

        public class MapItem : NotifyBase
        {
            private string _MapName;
            /// <summary>
            /// 所属地图
            /// </summary>
            public string MapName { get => _MapName; set => SetProperty(ref _MapName, value); }

            private string _MapCode;
            /// <summary>
            /// 地图编码
            /// </summary>
            public string MapCode { get => _MapCode; set => SetProperty(ref _MapCode, value); }

            private string _BuildingCode;
            /// <summary>
            /// 仓库编码
            /// </summary>
            public string BuildingCode { get => _BuildingCode; set => SetProperty(ref _BuildingCode, value); }

            private string _FloorName;
            /// <summary>
            /// 片区别名
            /// </summary>
            public string FloorName { get => _FloorName; set => SetProperty(ref _FloorName, value); }

            private string _FloorNumber;
            /// <summary>
            /// 片区编号
            /// </summary>
            public string FloorNumber { get => _FloorNumber; set => SetProperty(ref _FloorNumber, value); }


            private string _BussinessMapPath;
            /// <summary>
            /// 业务底图路径
            /// </summary>
            public string BussinessMapPath { get => _BussinessMapPath; set => SetProperty(ref _BussinessMapPath, value); }
        }

        private ObservableCollection<MapItem> _MapList;
        /// <summary>
        /// 地图列表
        /// </summary>
        public ObservableCollection<MapItem> MapList { get => _MapList; set => SetProperty(ref _MapList, value); }

        private int _SelectedMapIndex = -1;
        /// <summary>
        /// 选中的地图索引
        /// </summary>
        public int SelectedMapIndex { get => _SelectedMapIndex; set => SetProperty(ref _SelectedMapIndex, value); }

        private async Task GetMapList()
        {
            var serverSetting = BaseSettingBLL.ReadServerSetting();
            if (serverSetting == null)
                return;

            string ip = serverSetting.Ip;
            string userToken = serverSetting.Token;
            var resMapBuildings = await MapBuildingApi.GetMapBuildings(ip, userToken);
            if (resMapBuildings == null)
            {
                throw new ApplicationException(JsonConvert.SerializeObject(resMapBuildings));
            }

            List<string> mapCodeList = resMapBuildings.Data.Select(x => x.MapCode).ToList();
            var resMapFloors = await MapBuildingApi.GetMapFloors(ip, userToken, mapCodeList);
            if (resMapFloors == null)
            {
                throw new ApplicationException(JsonConvert.SerializeObject(resMapBuildings));
            }

            IEnumerable<MapItem> mapList = resMapFloors.Data.Select(floor =>
            {
                MapItem mapItem = new()
                {
                    MapCode = floor.MapCode,
                    FloorName = floor.FloorName,
                    FloorNumber = floor.FloorNumber,
                    BussinessMapPath = floor.BussinessMapPath,
                };

                var mapBuilding = resMapBuildings.Data.FirstOrDefault(x => x.MapCode == mapItem.MapCode);
                if (mapBuilding != null)
                {
                    mapItem.BuildingCode = mapBuilding.BuildingCode;
                    mapItem.MapName = mapBuilding.MapName;
                }

                return mapItem;
            });

            MapList = new(mapList);
        }

        #endregion

        #region 地图选择

        public ICommand SelectMapCommand => new CommandBase(param =>
        {
            try
            {
                if (SelectedMapIndex < 0)
                {
                    WarningMessageEvent?.Invoke("请选择地图");
                    return;
                }

                MapItem _selectedMap = MapList.ElementAt(SelectedMapIndex);
                MapItemModel selectedMap = new()
                {
                    MapCode = _selectedMap.MapCode,
                    BussinessMapPath = _selectedMap.BussinessMapPath,
                    MapName = _selectedMap.MapName,
                    BuildingCode = _selectedMap.BuildingCode,
                    FloorName = _selectedMap.FloorName,
                    FloorNumber = _selectedMap.FloorNumber,
                };

                SelectedMapEvent?.Invoke(selectedMap);
                CloseWindowEvent?.Invoke();
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
