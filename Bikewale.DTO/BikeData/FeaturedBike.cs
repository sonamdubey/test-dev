using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeData
{
    public class FeaturedBike
    {
        public string BikeName { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public string Discription { get; set; }
        public ushort Priority { get; set; }

        public MakeBase MakeBase { get; set; }
        public ModelBase ModelBase { get; set; }
    }
}
