using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Entities.UserReviews
{
    public class ReviewTaggedBikeEntity
    {

        //public BikeMakeEntityBase MakeEntity { get; set; }
        //public BikeModelEntityBase ModelEntity { get; set; }
        //public BikeVersionEntityBase VersionEntity { get; set; }

        private BikeMakeEntityBase objmakeBase = new BikeMakeEntityBase();
        public BikeMakeEntityBase MakeEntity { get { return objmakeBase; } set { objmakeBase = value; } }

        private BikeModelEntityBase objModelBase = new BikeModelEntityBase();
        public BikeModelEntityBase ModelEntity { get { return objModelBase; } set { objModelBase = value; } }

        private BikeVersionEntityBase objVersionBase = new BikeVersionEntityBase();
        public BikeVersionEntityBase VersionEntity { get { return objVersionBase; } set { objVersionBase = value; } }

        public uint ReviewsCount { get; set; }
        public uint Price { get; set; }
    }
}
