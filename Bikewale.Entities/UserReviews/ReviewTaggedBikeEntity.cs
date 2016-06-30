using Bikewale.Entities.BikeData;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    [Serializable, DataContract]
    public class ReviewTaggedBikeEntity
    {
        [DataMember]
        private BikeMakeEntityBase objmakeBase = new BikeMakeEntityBase();
        [DataMember]
        public BikeMakeEntityBase MakeEntity { get { return objmakeBase; } set { objmakeBase = value; } }
        [DataMember]
        private BikeModelEntityBase objModelBase = new BikeModelEntityBase();
        [DataMember]
        public BikeModelEntityBase ModelEntity { get { return objModelBase; } set { objModelBase = value; } }
        [DataMember]
        private BikeVersionEntityBase objVersionBase = new BikeVersionEntityBase();
        [DataMember]
        public BikeVersionEntityBase VersionEntity { get { return objVersionBase; } set { objVersionBase = value; } }
        [DataMember]
        public uint ReviewsCount { get; set; }
        [DataMember]
        public uint Price { get; set; }
    }
}
