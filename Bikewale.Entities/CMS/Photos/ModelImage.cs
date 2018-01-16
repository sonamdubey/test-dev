using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.CMS.Photos
{
    [Serializable, DataContract]
    public class ModelImage
    {
        [DataMember]
        public uint ImageId { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string ImagePathThumbnail { get; set; }
        [DataMember]
        public string ImagePathLarge { get; set; }
        [DataMember]
        public short MainImgCategoryId { get; set; }
        [DataMember]
        public string ImageCategory { get; set; }
        [DataMember]
        public string Caption { get; set; }
        [DataMember]
        public string ImageName { get; set; }
        [DataMember]
        public string AltImageName { get; set; }
        [DataMember]
        public string ImageTitle { get; set; }
        [DataMember]
        public string ImageDescription { get; set; }
        [DataMember]
        public BikeMakeEntityBase MakeBase { get; set; }
        [DataMember]
        public BikeModelEntityBase ModelBase { get; set; }
        [DataMember]
        public string OriginalImgPath { get; set; }
    }

    /// <summary>
    /// Created by:Sangram Nandkhile on 02 Feb 2017
    /// To hold model images, color images and mainimage
    /// </summary>
    [Serializable, DataContract]
    public class ImageBaseEntity
    {
        [DataMember]
        public uint ImageId { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string OriginalImgPath { get; set; }
        [DataMember]
        public ImageBaseType ImageType { get; set; }
        [DataMember]
        public string ImageTitle { get; set; }
        [DataMember]
        public string ImageCategory { get; set; }
    }

    [Serializable, DataContract]
    public class ColorImageBaseEntity : ImageBaseEntity
    {
        [DataMember]
        public uint ColorId { get; set; }
        [DataMember]
        public IEnumerable<string> Colors { get; set; }

        public uint Index { get; set; }
    }
    [Serializable, DataContract]
    public enum ImageBaseType
    {
        ModelImage = 1,
        ModelGallaryImage = 2,
        ModelColorImage = 3
    }
}
