using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bikewale.DTO.BikeData.Upcoming
{
    public class InputFilterDTO
    {
        [Required, JsonProperty("pageNo"), RegularExpression(@"^[1-9][0-9]*$")]
        public int? PageNo { get; set; }
        [Required, JsonProperty("pageSize"), RegularExpression(@"^[1-9][0-9]*$")]
        public int? PageSize { get; set; }
        [JsonProperty("makeId"), RegularExpression(@"^[1-9][0-9]*$")]
        public uint? MakeId { get; set; }
        [JsonProperty("year"), RegularExpression(@"\d{4}")]
        public uint? Year { get; set; }
    }
}
