using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified By :- Subodh Jain 09 March 2017
    /// Summary :- added BodyStyleId
    /// Modified by Sajal Gupta added ExpectedLaunchedDate
    /// </summary>
    [Serializable, DataContract]
    public class UpcomingBikeEntity
    {
        [DataMember]
        public uint ExpectedLaunchId { get; set; }
        [DataMember]
        public string ExpectedLaunchDate { get; set; }
        [DataMember]
        public DateTime ExpectedLaunchedDate { get; set; }
        [DataMember]
        public ulong EstimatedPriceMin { get; set; }
        [DataMember]
        public ulong EstimatedPriceMax { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string LargePicImagePath { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }

        private BikeMakeEntityBase objmakeBase = new BikeMakeEntityBase();
        [DataMember]
        public BikeMakeEntityBase MakeBase { get { return objmakeBase; } set { objmakeBase = value; } }

        private BikeModelEntityBase objModelBase = new BikeModelEntityBase();
        [DataMember]
        public BikeModelEntityBase ModelBase { get { return objModelBase; } set { objModelBase = value; } }

        private BikeDescriptionEntity objDesc = new BikeDescriptionEntity();
        [DataMember]
        public BikeDescriptionEntity BikeDescription { get { return objDesc; } set { objDesc = value; } }

        [DataMember]
        public string BikeName { get; set; }
        [DataMember]
        public uint BodyStyleId { get; set; }
    }
}
