﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    public class BikeVersionMinSpecs : BikeVersionsListEntity      {
       
        public string BrakeType { get; set; }
        public bool AlloyWheels { get; set; }
        public bool ElectricStart { get; set; }
        public bool AntilockBrakingSystem { get; set; }
    }
}
