using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 5th Aug 2014
    /// </summary>
    public class SimilarBikeEntity
    {
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int VersionPrice { get; set; }
        public string HostUrl { get; set; }
        public string SmallPicUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string OriginalImagePath { get; set; }
        private BikeMakeEntityBase objmakeBase = new BikeMakeEntityBase();
        public BikeMakeEntityBase MakeBase { get { return objmakeBase; } set { objmakeBase = value; } }

        private BikeModelEntityBase objModelBase = new BikeModelEntityBase();
        public BikeModelEntityBase ModelBase { get { return objModelBase; } set { objModelBase = value; } }

        private BikeVersionEntityBase objDesc = new BikeVersionEntityBase();
        public BikeVersionEntityBase VersionBase { get { return objDesc; } set { objDesc = value; } }
    }
}
