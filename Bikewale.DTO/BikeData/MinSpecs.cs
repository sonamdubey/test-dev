﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeData
{
    public class MinSpecs
    {
        public float Displacement { get; set; }
        public ushort FuelEfficiencyOverall { get; set; }
        public float MaxPower { get; set; }
        public float MaximumTorque { get; set; }
        public float KerbWeight { get; set; }
    }
}
