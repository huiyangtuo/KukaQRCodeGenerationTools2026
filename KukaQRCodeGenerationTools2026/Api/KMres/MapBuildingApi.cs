using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KukaQRCodeGenerationTools2026.Api.KMres
{
    internal class MapBuildingApi
    {
        #region 查询地图列表

        public class GetMaps_ResponseData
        {
            [JsonProperty("buildingCode")]
            public string BuildingCode { get; set; }

            [JsonProperty("createApp")]
            public string CreateApp { get; set; }

            [JsonProperty("createBy")]
            public string CreateBy { get; set; }

            [JsonProperty("createTime")]
            public DateTime CreateTime { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("lastUpdateApp")]
            public string LastUpdateApp { get; set; }

            [JsonProperty("lastUpdateBy")]
            public string LastUpdateBy { get; set; }

            [JsonProperty("lastUpdateTime")]
            public DateTime LastUpdateTime { get; set; }

            [JsonProperty("mapCode")]
            public string MapCode { get; set; }

            [JsonProperty("mapFloors")]
            public int MapFloors { get; set; }

            [JsonProperty("mapName")]
            public string MapName { get; set; }

            [JsonProperty("remarks")]
            public string Remarks { get; set; }

            [JsonProperty("status")]
            public int Status { get; set; }

        }

        /// <summary>
        /// 查询仓库地图列表
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<KukaResponseModel<GetMaps_ResponseData[]>> GetMapBuildings(string ip, string token)
        {
            if (token.StartsWith("Bearer "))
                token = token.Remove(0, 7);

            string url = $"http://{ip}:5000/apibd/api/v1/data/map-building/select/all/maps";

            return await HytHttpClient.GetWithTokenAsync<KukaResponseModel<GetMaps_ResponseData[]>>(url, token);
        }

        public class GetMapFloors_ResponseData
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("mapCode")]
            public string MapCode { get; set; }

            [JsonProperty("floorNumber")]
            public string FloorNumber { get; set; }

            [JsonProperty("floorName")]
            public string FloorName { get; set; }

            [JsonProperty("floorLength")]
            public double FloorLength { get; set; }

            [JsonProperty("floorWidth")]
            public double FloorWidth { get; set; }

            [JsonProperty("floorLevel")]
            public int FloorLevel { get; set; }

            [JsonProperty("floorMapVersion")]
            public string FloorMapVersion { get; set; }

            [JsonProperty("status")]
            public object Status { get; set; }

            [JsonProperty("laserMapId")]
            public int LaserMapId { get; set; }

            [JsonProperty("laserMapPath")]
            public string LaserMapPath { get; set; }

            [JsonProperty("bussinessMapPath")]
            public string BussinessMapPath { get; set; }

            [JsonProperty("remarks")]
            public string Remarks { get; set; }

            [JsonProperty("lastUpdateTime")]
            public DateTime LastUpdateTime { get; set; }
        }

        /// <summary>
        /// 查询地图楼层列表
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="token"></param>
        /// <param name="mapCodeList"></param>
        /// <returns></returns>
        public static async Task<KukaResponseModel<GetMapFloors_ResponseData[]>> GetMapFloors(string ip, string token, List<string> mapCodeList)
        {
            if (token.StartsWith("Bearer "))
                token = token.Remove(0, 7);

            string url = $"http://{ip}:5000/apibd/api/v1/data/map-floor/select/all/map";

            return await HytHttpClient.PostWithTokenAsync<KukaResponseModel<GetMapFloors_ResponseData[]>>(url, mapCodeList, token);
        }

        #endregion

        #region 地图中的点位信息

        public class GetMapFloorInfo_RequestData
        {
            public string MapCode { get; set; }

            public string FloorNumber { get; set; }
        }

        public class GetMapFloorInfo_ResponseData
        {
            [JsonProperty("nodes")]
            public List<GetMapFloorInfo_ResponseData_Node> Nodes { get; set; }
        }

        public class GetMapFloorInfo_ResponseData_Node
        {
            [JsonProperty("customerUi")]
            public string CustomerUi { get; set; }

            /// <summary>
            /// 外部编码
            /// </summary>
            [JsonProperty("foreginCode")]
            public string ForeginCode { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("nodeLabel")]
            public string NodeLabel { get; set; }

            [JsonProperty("nodeNumber")]
            public int NodeNumber { get; set; }

            /// <summary>
            /// 点位类型<br/>
            /// 1 = 普通拓扑点<br/>
            /// 2 = 二维码点
            /// </summary>
            [JsonProperty("nodeType")]
            public int NodeType { get; set; }

            [JsonProperty("nodeUuid")]
            public string NodeUuid { get; set; }

            [JsonProperty("opportunityCharging")]
            public int OpportunityCharging { get; set; }

            [JsonProperty("xCooridinate")]
            public double XCooridinate { get; set; }

            [JsonProperty("yCooridinate")]
            public double YCooridinate { get; set; }

        }

        /// <summary>
        /// 查询地图中的详细信息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="token"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<KukaResponseModel<GetMapFloorInfo_ResponseData>> GetMapFloorInfo(string ip, string token, GetMapFloorInfo_RequestData param)
        {
            if (param == null)
                throw new ApplicationException("请填写 param 参数");
            if (string.IsNullOrEmpty(param.MapCode))
                throw new ApplicationException("请填写 param.MapCode 参数");
            if (string.IsNullOrEmpty(param.FloorNumber))
                throw new ApplicationException("请填写 param.FloorNumber 参数");

            if (token.StartsWith("Bearer "))
                token = token.Remove(0, 7);

            string mapCode = param.MapCode;
            string floorNumber = param.FloorNumber;

            string url = $"http://{ip}:5100/apibd/api/v1/data/open-monitor/map-floor/?mapCode={mapCode}&floorNumber={floorNumber}";

            return await HytHttpClient.GetWithTokenAsync<KukaResponseModel<GetMapFloorInfo_ResponseData>>(url, token);
        }
        #endregion
    }
}
