
using Newtonsoft.Json;
namespace BikewaleOpr.Entity.Users
{
    /// <summary>
    /// Author  : Kartik Rathod on 28 mar 2018
    /// Desc    : Opr users details entiy to save opr users data and respective task ids
    /// </summary>
    public class UserDetailsEntity
    {
        [JsonProperty("userId")]
        public uint UserId { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("taskIds")]
        public string TaskIds { get; set; }
    }
}
