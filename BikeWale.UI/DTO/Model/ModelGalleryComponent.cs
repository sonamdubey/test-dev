using Newtonsoft.Json;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 4th Oct 2017
    /// Summary : DTO to represent a gallery component
    /// </summary>
    public class ModelGalleryComponent
    {
        [JsonProperty("categoryId")]
        public ushort CategoryId { get; set; }
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }
        [JsonProperty("categoryCount")]
        public int CategoryCount { get; set; }
        [JsonProperty("dataUrl")]
        public string DataUrl { get; set; }
    }
}
