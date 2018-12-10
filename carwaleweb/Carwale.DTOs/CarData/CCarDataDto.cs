using Carwale.Entity.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.CarData
{
	public class CCarDataDto
    {
        [JsonProperty("specs")]
        public List<SubCategory> Specs = new List<SubCategory>();
        [JsonProperty("features")]
        public List<SubCategory> Features = new List<SubCategory>();
        [JsonProperty("overview")]
        public List<Item> OverView = new List<Item>();
        [JsonProperty("colors")]
        public List<List<Color>> Colors = new List<List<Color>>();
        [JsonProperty("validVersionIds")]
        public List<int> ValidVersionIds = new List<int>();
        [JsonProperty("carDetails")]
        public List<CarWithImageEntityDTO> CarDetails = new List<CarWithImageEntityDTO>();
        [JsonProperty("featuredVersionId")]
        public int FeaturedVersionId = -1;
        [JsonProperty("campaignTemplates")]
        public Dictionary<int, IdName> CampaignTemplates { get; set; }
    }

    public class CarEntityDTO
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
    }

    public class CarWithImageEntityDTO : CarEntityDTO
    {
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("hostURL")]
        public string HostURL { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("imageSmall")]
        public string ImageSmall { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
        [JsonProperty("versions")]
        public List<Versions> Versions = new List<Versions>();
    }

    public class Color
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }


     public class ColorDTO
    {
      [JsonProperty("carColors")]
      public List<List<Carwale.DTOs.CarData.Color>> CarColors { get; set; }

   }

     public class ColorDto_V1
     {
         [JsonProperty("carColors")]
         public List<CarColorsDto> CarColors { get; set; }
     }
    
    public class SubCategoryData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("nodeCode")]
        public string NodeCode { get; set; }
        [JsonProperty("sortOrder")]
        public string SortOrder { get; set; }
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

    public class ItemData
    {
        [JsonProperty("itemMasterId")]
        public string ItemMasterId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("nodeCode")]
        public string NodeCode { get; set; }
        [JsonProperty("sortOrder")]
        public string SortOrder { get; set; }
        [JsonProperty("overviewSortOrder")]
        public string OverviewSortOrder { get; set; }
        [JsonProperty("unitType")]
        public string UnitType { get; set; }
        [JsonProperty("isOverviewable")]
        public bool IsOverviewable { get; set; }
        [JsonProperty("values")]
        public string Value { get; set; }
    }

    public class ValueData
    {
        [JsonProperty("itemMasterId")]
        public string ItemMasterId { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Versions
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
