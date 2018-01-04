
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.AdOperations;
using Newtonsoft.Json;
using System;
namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd Jan 2018
    /// Description: Entity created for promoted bike 
    /// </summary>
    public class PromotedBike
    {
        [JsonProperty("promotedBikeId")]
        public uint PromotedBikeId { get; set; }
        [JsonProperty("make")]
        public BikeMakeEntityBase Make { get; set; }
        [JsonProperty("model")]
        public BikeModelEntityBase Model { get; set; }
        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }
        [JsonProperty("adOperationType")]
        public AdOperationEnum AdOperationType { get; set; }
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
        [JsonProperty("lastUpdateBy")]
        public string LastUpdatedBy { get; set; }
        [JsonProperty("lastUpdatedById")]
        public uint LastUpdatedById { get; set; }
        [JsonProperty("lastUpdateOn")]
        public DateTime LastUpdatedOn { get; set; }
        [JsonProperty("contractStatus")]
        public ContractStatusEnum ContractStatus { get; set; }
    }
}
