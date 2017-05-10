﻿
using System;
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 11 May 2016
    /// Desc       : Entity to hold make, model and masking Name of the links shown under compare bike links
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Added modelid1 and modelid2 for sponsored comparions
    /// Modified by : Aditi Srivastava on 25 Apr 2017
    /// Summary     : Added additional parameters (ID, IsScooterOnly) for generic use
    /// Modified By :- Subodh Jain 10 may 2017
    /// summary :- Added bodystyle1 and bodystyle2
    /// </summary>
    [Serializable]
    public class SimilarCompareBikeEntity
    {
        public string Make1 { get; set; }
        public string Make2 { get; set; }
        public string MakeMasking1 { get; set; }
        public string MakeMasking2 { get; set; }
        public uint ModelId1 { get; set; }
        public uint ModelId2 { get; set; }
        public string Model1 { get; set; }
        public string Model2 { get; set; }
        public string Version1 { get; set; }
        public string ModelMasking1 { get; set; }
        public string ModelMasking2 { get; set; }
        public string VersionId1 { get; set; }
        public string VersionId2 { get; set; }
        public int Price1 { get; set; }
        public int Price2 { get; set; }
        public string OriginalImagePath1 { get; set; }
        public string OriginalImagePath2 { get; set; }
        public string HostUrl1 { get; set; }
        public string HostUrl2 { get; set; }
        public string City1 { get; set; }
        public string City2 { get; set; }
        public string Bike1 { get; set; }
        public string Bike2 { get; set; }
        public int ID { get; set; }
        public bool IsScooterOnly { get; set; }
        public uint BodyStyle1 { get; set; }
        public uint BodyStyle2 { get; set; }
    }
}
