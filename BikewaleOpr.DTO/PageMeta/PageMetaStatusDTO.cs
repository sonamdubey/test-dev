using Newtonsoft.Json;
namespace BikewaleOpr.DTO.PageMeta
{
    /// <summary>
    /// Created by : Snehal Dange on 31st Jan 2018
    /// Description: DTO created to modify status of page metas for api 
    /// </summary>


    public class PageMetaStatusDTO
    {
        [JsonProperty("pageMetaId")]
        public string PageMetaId { get; set; }

        [JsonProperty("status")]
        public ushort Status { get; set; }

        [JsonProperty("modelId")]
        public uint? ModelId { get; set; }

        [JsonProperty("makeId")]
        public uint? MakeId { get; set; }
    }
}