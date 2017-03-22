using Newtonsoft.Json;

namespace BikewaleOpr.DTO.BikeData
{
    /// <summary>
    /// Created by : Sajal Gupta on 10-03-2017
    /// Description : Synopsis data for make bikes+ scooters 
    /// </summary>
    public class SynopsisDataDto
    {
        [JsonProperty("scooterDescription")]
        public string ScooterDescription { get; set; }
        [JsonProperty("bikeDescription")]
        public string BikeDescription { get; set; }
    }
}
