using Bikewale.Entities.CMS.Photos;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified BY : Ashish G. Kamble on 8 Oct 2015
    /// Modified By : Rajan Chauhan on 7 Apr 2018
    /// Description : Added ModelVersionMinSpecs
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
        public IEnumerable<BikeSpecificationEntity> ModelVersionSpecsList { get; set; }
        [DataMember]
        public IEnumerable<TransposeModelSpecEntity> TransposeModelSpecs { get; set; }
        [DataMember]
        public IEnumerable<NewBikeModelColor> ModelColors { get; set; }
        [DataMember]
        public UpcomingBikeEntity UpcomingBike { get; set; }
        [DataMember]
        public IEnumerable<ModelImage> Photos { get; set; }
        [DataMember]
        public IEnumerable<ColorImageBaseEntity> AllPhotos { get; set; }
        [DataMember]
        public IEnumerable<ModelColorImage> colorPhotos { get; set; }
        [DataMember]
        public Overview objOverview { get; set; }
        [DataMember]
        public Features objFeatures { get; set; }
        [DataMember]
        public Specifications objSpecs { get; set; }
        [DataMember]
        public uint UsedListingsCnt { get; set; }
        public BikeVersionMinSpecs ModelVersionMinSpecs { get; set; }
        public IEnumerable<SpecsItem> SpecsSummaryList { get; set; }
        public SpecsFeaturesEntity VersionSpecsFeatures { get; set; }
    }
}
