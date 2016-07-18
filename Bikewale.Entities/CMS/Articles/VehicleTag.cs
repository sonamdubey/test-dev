using Bikewale.Entities.BikeData;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary>    
    [Serializable, DataContract]
    public class VehicleTag
    {
        [DataMember]
        public BikeMakeEntityBase MakeBase { get; set; }
        [DataMember]
        public BikeModelEntityBase ModelBase { get; set; }
        [DataMember]
        public BikeVersionEntityBase VersionBase { get; set; }
    }
}
