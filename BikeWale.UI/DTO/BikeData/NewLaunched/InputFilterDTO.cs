using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace Bikewale.DTO.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 13 Feb 2017
    /// Description :   InputFilter DTO
    /// </summary>
    public class InputFilterDTO
    {
        [Required, JsonProperty("pageNo"), RegularExpression(@"^[1-9][0-9]*$")]
        public int? PageNo { get; set; }
        [Required, JsonProperty("pageSize"), RegularExpression(@"^[1-9][0-9]*$")]
        public int? PageSize { get; set; }
        [JsonProperty("make"), RegularExpression(@"^[1-9][0-9]*$")]
        public uint? Make { get; set; }
        [JsonProperty("yearLaunch"), RegularExpression(@"\d{4}")]
        public uint? YearLaunch { get; set; }
        [JsonProperty("cityId"), RegularExpression(@"^[1-9][0-9]*$")]
        public uint? CityId { get; set; }
    }
}
