using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 19 Aug 2015
    /// Summary : Entity for Featured Bike
    /// </summary>
    public class FeaturedBikeEntity
    {
        public string BikeName { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public string Discription { get; set; }
        public ushort Priority { get; set; }

        public BikeMakeEntityBase MakeBase { get; set; }
        public BikeModelEntityBase ModelBase { get; set; }
    }
}
