using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Videos
{
    [DataContract, Serializable]
    public class BikeVideoEntity
    {
        [DataMember]
        public string VideoTitle { get; set; }
        [DataMember]
        public string VideoUrl { get; set; }
        [DataMember]
        public string VideoId { get; set; }
        [DataMember]
        public uint Views { get; set; }
        [DataMember]
        public uint Likes { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public uint BasicId { get; set; }
        [DataMember]
        public string Tags { get; set; }
        [DataMember]
        public uint Duration { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string MaskingName { get; set; }
        [DataMember]
        public string SubCatId { get; set; }
        [DataMember]
        public string SubCatName { get; set; }
        [DataMember]
        public string VideoTitleUrl { get; set; }
        [DataMember]
        public string ImgHost { get; set; }
        [DataMember]
        public string ThumbnailPath { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public string DisplayDate { get; set; }
        public uint ModelId { get; set; }
    }

    public class VideosListWrapper
    {
        public IEnumerable<BikeVideoEntity> Videos { get; set; }
    }
}
