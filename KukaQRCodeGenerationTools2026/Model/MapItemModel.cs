using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KukaQRCodeGenerationTools2026.Model
{
    public class MapItemModel
    {
        public string MapCode { get; set; }

        public string MapName { get; set; }

        public string FloorNumber { get; set; }

        public string FloorName { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string BuildingCode { get; set; }

        /// <summary>
        /// 地图的底图地址
        /// </summary>
        public string BussinessMapPath { get; set; }
    }
}
