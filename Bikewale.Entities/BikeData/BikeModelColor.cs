﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    public class BikeModelColor
    {
        public uint Id { get; set; }
        public uint ModelId { get; set; }
        public string ColorName { get; set; }
        public string HexCode { get; set; }
    }
}
