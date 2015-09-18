using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Entities.NewBikeSearch
{
    public class SearchOutputEntityBase
    {
        public string BikeName { get; set; }
        public float Displacement { get; set; }
        public string FuelType { get; set; }
        public string Power { get; set; }
        public ushort FuelEfficiency { get; set; }
        public ushort KerbWeight { get; set; }
        public float MaximumTorque { get; set; }
        public string FinalPrice { get; set; }
        public string AvailableSpecs { get; set; }
        public BikeModelEntity BikeModel { get; set; }
    }
}
