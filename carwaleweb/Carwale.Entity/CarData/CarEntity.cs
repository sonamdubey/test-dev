using Carwale.Entity.Deals;
using Carwale.Entity.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using Carwale.Entity.Campaigns;
namespace Carwale.Entity
{
    /// <summary>
    /// This entity class includes MakeId, ModelId, VersionId, MakeName, ModelName, VersionName
    /// </summary>
    [Serializable]
    public class CarEntity
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }       
        public int VersionId { get; set; }
        public int RootId { get; set; }
        public string RootName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public string MaskingName { get; set; }
        public bool IsFuturistic { get; set; }
        public bool IsNew { get; set; }
        public string VersionMaskingName { get; set; }
    }

    /// <summary>
    /// This entity class includes MakeId, ModelId, VersionId
    /// </summary>
    public class CarIdEntity
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }       
        public int VersionId { get; set; }
    }

    /// <summary>
    /// This entity class includes MakeName, ModelName, VersionName
    /// </summary>
    public class CarNameEntity
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
    }

    /// <summary>
    /// This entity class includes MakeId, ModelId, MakeName, ModelName
    /// </summary>
    [Serializable]
    public class MakeModelEntity
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
       
        public string MakeName { get; set; }
        public string ModelName { get; set; }       
    }

    [Serializable,JsonObject]
    public class MakeModelEntityV2
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("modelMaskingName")]
        public string ModelMaskingName { get; set; }

        [JsonProperty("isNew")]
        public uint IsNew { get; set; }

        [JsonProperty("isFuturistic")]
        public uint IsFuturistic { get; set; }

        [JsonProperty("is360ExteriorAvailable")]
        public bool Is360ExteriorAvailable { get; set; }
        
        [JsonProperty("is360OpenAvailable")]
        public bool Is360OpenAvailable { get; set; }
        
        [JsonProperty("is360InteriorAvailable")]
        public bool Is360InteriorAvailable { get; set; }
    }
    /// <summary>
    /// This entity class includes MakeId, ModelId
    /// </summary>
    [Serializable]
    public class MakeModelIdsEntity
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
    }

    /// <summary>
    /// This entity class includes MakeName, ModelName
    /// </summary>
    public class MakeModelNameEntity
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }   
    }

    /// <summary>
    /// This entity class includes MakeId
    /// </summary>
    [Serializable]
    public class MakeEntity
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }  
    }

    /// <summary>
    /// This entity class includes MakeId
    /// </summary>
    [Serializable]
    public class ModelBase
    {
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
    }

    [Serializable]
    public class VersionBase
    {
        [JsonProperty("versionName")]
        public string VersionName { get; set; }

        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("exShowroomPrice")]
        public int ExShowroomPrice { get; set; }

        [JsonProperty("averagePrice")]
        public int AveragePrice { get; set; }
    }

    /// <summary>
    /// This entity class includes MakeId
    /// </summary>
    [Serializable]
    public class ModelEntity
    {
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
        [JsonProperty("rootId")]
        public string RootId { get; set; }
    }

    /// <summary>
    /// This entity class includes MakeId
    /// </summary>
    public class MakeIdEntity
    {
        public int MakeId { get; set; }
    }

    /// <summary>
    /// This entity class includes MakeName
    /// </summary>
    public class MakeNameEntity
    {
        public int MakeId { get; set; }
    }

    [Serializable]
    public class CarWithImageEntity : CarEntity
    {
        public string Image { get; set; }
        public string ImgPath { get; set; }
        public string HostURL { get; set; }
        public int Price { get; set; }
        public int MinAvgPrice { get; set; }
        public string ImageSmall { get; set; }
        public string  OriginalImgPath { get; set; }
        public DealsStock DiscountSummary { get; set; }
        public PriceOverview PriceOverview { get; set; }
        public string CloseUrl { get; set;}
        public bool ShowAllColorsLink { get; set; }
        public DealerAd SponsoredCampaign { get; set; }
        public bool ShowCampaignLink { get; set; }
    }

    /// <summary>
    /// This entity class includes MakeId, ModelId
    /// </summary>
    [Serializable]
    public class MakeLogoEntity
    {
        public int MakeId { get; set; }
        public string MakeName { get; set; }
        public string HostURL { get; set; }
        public string OriginalImgPath { get; set; }
    }

    [Serializable]
    public class ColorEntity
    {
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string HexCode { get; set; }
    }

    [Serializable]
    public class VersionEntity
    {
        public int VersionId { get; set; }
        public string VersionName { get; set; }

        [JsonIgnore]
        public string VersionMasking { get; set; }
    }

    [Serializable, JsonObject]
    public class ModelPhotos : MakeModelEntityV2
    {
        [JsonProperty("hostUrl")]
        public string HostURL { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("imageCount")]
        public int ImageCount { get; set; }

        [JsonProperty("galleryImagePath")]
        public string GalleryImagePath { get; set; }
    }

    [Serializable, JsonObject]
    public class ModelVideo : MakeModelEntityV2
    {
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("videoCount")]
        public int VideoCount { get; set; }

        [JsonProperty("subCategoryName")]
        public string SubCategoryName { get; set; }

        [JsonProperty("basicId")]
        public int BasicId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("videoUrl")]
        public string VideoUrl { get; set; }
        
        [JsonProperty("views")]
        public int Views { get; set; }
        
        [JsonProperty("duration")]
        public int Duration { get; set; }
        
        [JsonProperty("displayDate")]
        public DateTime DisplayDate { get; set; }
    }
}
