using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Search.Model
{

    public class NewCarSearchModelBaseDto
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }

        [JsonProperty("carRating")]
        public string CarRating { get; set; } 

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("matchingVersionText")]
        public string MatchingVersionText { get; set; }
    }

    public class NewCarSearchBaseDto
    {
        [JsonProperty("nextPageUrl")]
        public string NextPageUrl { get; set; }
        [JsonProperty("totalModels")]
        public int TotalModels { get; set; }
        [JsonProperty("totalVersions")]
        public int TotalVersions { get; set; }
    }

    public class NewCarSearchModelDTO : NewCarSearchModelBaseDto
    {
        
        [JsonProperty("priceText")]
        public string PriceText { get; set; }

        [JsonProperty("minPrice")]
        public string MinPrice { get; set; }       
    }

    public class NewCarSearchModelDtoV2 : NewCarSearchModelBaseDto
    {
        [JsonProperty("priceOverview")]
        public  PriceOverviewDTO PriceOverView { get; set; }
    }

    public class NewCarSearchDTO : NewCarSearchBaseDto
    {
        [JsonProperty("carModels")]
        public List<NewCarSearchModelDTO> CarModels { get; set; }
    }

    public class NewCarSearchDtoV2 : NewCarSearchBaseDto
    {
        [JsonProperty("carModels")]
        public List<NewCarSearchModelDtoV2> CarModels { get; set; }
        [JsonProperty("orpText")]
        public string OrpText { get; set; }
    }

}
