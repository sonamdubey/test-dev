using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class UpcomingBikeEntity
    {
        public uint ExpectedLaunchId { get; set; }
        public string ExpectedLaunchDate { get; set; }
        public ulong EstimatedPriceMin { get; set; }
        public ulong EstimatedPriceMax { get; set; }
        public string HostUrl { get; set; }
        public string LargePicImagePath { get; set; }
        public string OriginalImagePath { get; set; }
        private BikeMakeEntityBase objmakeBase = new BikeMakeEntityBase();
        public BikeMakeEntityBase MakeBase { get { return objmakeBase; } set { objmakeBase = value; } }

        private BikeModelEntityBase objModelBase = new BikeModelEntityBase();
        public BikeModelEntityBase ModelBase { get { return objModelBase; } set { objModelBase = value; } }

        private BikeDescriptionEntity objDesc = new BikeDescriptionEntity();
        public BikeDescriptionEntity BikeDescription { get { return objDesc; } set { objDesc = value; } }
    }
}
