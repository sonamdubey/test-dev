using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{

    public class BikeVersionEntity : BikeVersionEntityBase
    {
        public bool New { get; set; }
        public bool Used { get; set; }
        public bool Futuristic { get; set; }
        public string BikeName { get; set; }
        public string HostUrl { get; set; }
        public string SmallPicUrl { get; set; }
        public string LargePicUrl { get; set; }
        public Int64 Price { get; set; }
        public string OriginalImagePath { get; set; }
        private BikeMakeEntityBase objmakeBase = new BikeMakeEntityBase();
        public BikeMakeEntityBase MakeBase { get { return objmakeBase; } set { objmakeBase = value; } }

        private BikeModelEntityBase objmodelBase = new BikeModelEntityBase();
        public BikeModelEntityBase ModelBase { get { return objmodelBase; } set { objmodelBase = value; } }
    }
}
