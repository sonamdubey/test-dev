using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Carwale.DTOs.CarData
{
    /// <summary>
    /// Created By : Ajay Singh on 1 march 2016
    /// </summary>
    /// 
    public class CarVersionDTO
    { 
      
        [JsonProperty("versionId")]
        public int Id;

     
        [JsonProperty("name")]
        public string Version;

       
        [JsonIgnore]
        [JsonProperty("modelId")]
        public int ModelId;

        
        [JsonProperty("features")]
        public string SpecsSummary;

        [JsonIgnore]
        [JsonProperty("maskingName")]
        public string MaskingName;

      
        [JsonProperty("new")]
        public bool New;

        [JsonIgnore]
        [JsonProperty("reviewRateOld")]
        public float ReviewRate;

        [JsonIgnore]
        [JsonProperty("minPrice")]
        public double MinPrice;

        [JsonIgnore]
        [JsonProperty("reviewCount")]
        public int ReviewCount;

        [JsonIgnore]
        [JsonProperty("bodyStyleId")]
        public int BodyStyleId;

        [JsonIgnore]
        [JsonProperty("transmissionTypeId")]
        public int TransmissionTypeId;

     
        [JsonProperty("fuelType")]
        public string CarFuelType;

        [JsonIgnore]
        [JsonProperty("fuelTypeId")]
        public int FuelTypeId;

        
        [JsonProperty("exShowRoomPrice")]
        public string MinPriceNew;

        [JsonIgnore]
        [JsonProperty("reviewRate")]
        public string ReviewRateNew;

        [JsonProperty("exShowroomCityId")]
        public int ExShowRoomCityId;

        [JsonProperty("exShowroomCity")]
        public string ExShowRoomCityName;

     
    }
}
