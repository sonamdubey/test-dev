using Newtonsoft.Json;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 4th Oct 2017
    /// Summary : DTO to represent a gallery component
    /// </summary>
    public class ModelGalleryComponent
    {
        [JsonProperty("componentType")]
        public ushort ComponentType { get; set; }
        [JsonProperty("displayText")]
        public string DisplayText { get; set; }
        [JsonProperty("dataUrl")]
        public string DataUrl { get; set; }
    }
}
