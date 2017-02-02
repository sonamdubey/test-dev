using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Models.Shared
{
    public class BikeInfo
    {
        //public BikeMakeEntityBase Make { get; set; }
        //public BikeModelEntityBase Model { get; set; }
        //public string HostUrl { get; set; }
        //public string OriginalImagePath { get; set; }
        //public uint VideosCount { get; set; }
        //public uint PhotosCount { get; set; }
        //public uint NewsCount { get; set; }
        //public uint ExpertReviewsCount { get; set; }
        //public uint FeaturesCount { get; set; }
        //public bool IsSpecsAvailable { get; set; }
        //public MinSpecsEntity MinSpecs { get; set; }
        //public uint BikePrice { get; set; }
        //public string BikePriceCity { get; set; }
        public string Url { get; set; }        
        public string Bike { get; set; }
        public int PQSource { get; set; }
        public uint ModelId { get; set; }
        public Bikewale.Entities.GenericBikes.GenericBikeInfo Info { get; set; }
    }
}
