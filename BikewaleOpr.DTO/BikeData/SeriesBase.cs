using Newtonsoft.Json;

namespace BikewaleOpr.DTO.BikeData
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 12th Sep 2017
    /// Summary : DTO for bike series base
    /// </summary>
    public class SeriesBase
    {
        [JsonProperty("seriesId")]
        public uint SeriesId { get; set; }
        [JsonProperty("seriesName")]
        public string SeriesName { get; set; }
        [JsonProperty("seriesMaskingName")]
        public string SeriesMaskingName { get; set; }
    }
}
