using Bikewale.Entities.CMS.Photos;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified BY : Ashish G. Kamble on 8 Oct 2015
    /// </summary>
    [Serializable, DataContract]
    public class BikeModelPageEntity
    {
        [DataMember]
        public BikeDescriptionEntity ModelDesc { get; set; }
        [DataMember]
        public BikeModelEntity ModelDetails { get; set; }
        [DataMember]
        public List<BikeVersionMinSpecs> ModelVersions { get; set; }
        [DataMember]
        public BikeSpecificationEntity ModelVersionSpecs { get; set; }
        [DataMember]
        public IEnumerable<NewBikeModelColor> ModelColors { get; set; }
        [DataMember]
        public UpcomingBikeEntity UpcomingBike { get; set; }
        [DataMember]
        public List<ModelImage> Photos { get; set; }
        [DataMember]
        public int PhotosCount { get; set; } //Added by Aditi Srivastava
        [DataMember]
        public int VideosCount { get; set; } //Added by Aditi Srivastava
        [DataMember]
        public Overview objOverview { get; set; }
        [DataMember]
        public Features objFeatures { get; set; }
        [DataMember]
        public Specifications objSpecs { get; set; }
    }
}
