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

        [JsonProperty("updatedBy")]
        public uint UpdatedBy { get; set; }

        [JsonProperty("makeIdList")]
        public string MakeIdList { get; set; }

        [JsonProperty("modelIdList")]
        public string ModelIdList { get; set; }
    }
}