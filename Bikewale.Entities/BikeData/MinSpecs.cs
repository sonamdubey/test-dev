using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bikewale.Entities.BikeData
{
    public class MinSpecsEntity
    {
        public float? Displacement { get; set; }
        public ushort? FuelEfficiencyOverall { get; set; }
        public float? MaxPower { get; set; }
        public float? MaximumTorque { get; set; }
    }
}
