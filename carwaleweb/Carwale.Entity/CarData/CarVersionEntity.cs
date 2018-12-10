using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarVersionEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public long OnRoadPrice { get; set; }
        public long ExshowroomPrice { get; set; }
        public string FuelType { get; set; }
        public string Transmission { get; set; }
        public string MaskingName { get; set; }
        public string OriginalImgPath { get; set; }
        public string Displacement { get; set; }
    }
}
