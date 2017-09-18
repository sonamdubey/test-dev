using BikewaleOpr.DTO.Make;
using Newtonsoft.Json;
using System;

namespace BikewaleOpr.DTO.BikeData
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 12th Sep 2017
    /// Summary : DTO for bike series entity
    /// </summary>
    public class BikeSeriesDTO: SeriesBase
    {
        [JsonProperty("createdOn")]
        public string CreatedOn { get; set; }
        [JsonProperty("updatedOn")]
        public string UpdatedOn { get; set; }
        [JsonProperty("updatedBy")]
        public string UpdatedBy { get; set; }
        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty("make")]
        public MakeBase BikeMake { get; set; }
        [JsonProperty("isSeriesPageUrl")]
        public bool IsSeriesPageUrl { get; set; }
    }
}
