﻿
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 11 May 2016
    /// Desc       : Entity to hold make, model and masking Name of the links shown under compare bike links
    /// </summary>
    public class SimilarCompareBikeEntity
    {
        public string Make1 { get; set; }
        public string Make2 { get; set; }
        public string MakeMasking1 { get; set; }
        public string MakeMasking2 { get; set; }
        public string Model1 { get; set; }
        public string Model2 { get; set; }
        public string Version1 { get; set; }
        public string ModelMasking1 { get; set; }
        public string ModelMasking2 { get; set; }
        public string VersionId1 { get; set; }
        public string VersionId2 { get; set; }
        public string ModelMaskingName1 { get; set; }
        public string ModelMaskingName2 { get; set; }
        public int Price1 { get; set; }
        public int Price2 { get; set; }
        public string HostURL { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl1 { get; set; }
        public string HostUrl2 { get; set; }
        public string VersionImgUrl1 { get; set; }
        public string VersionImgUrl2 { get; set; }
    }
}
