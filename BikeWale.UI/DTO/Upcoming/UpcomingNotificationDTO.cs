using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Upcoming
{
    /// <summary>
    /// Created by: Dhruv Joshi 
    /// Dated: 8th Feb 2018
    /// Description: DTO for Upcoming Bikes Notification
    /// </summary>
    public class UpcomingNotificationDTO
    {
        [JsonProperty("emailId")]
        [JsonRequired]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        public string EmailId { get; set; }

        [JsonProperty("makeId")]
        [JsonRequired]
        [Range(1, ushort.MaxValue)]
        public ushort MakeId { get; set; }

        [JsonProperty("modelId")]
        [JsonRequired]
        [Range(1, ushort.MaxValue)]
        public ushort ModelId { get; set; }

        [JsonProperty("bikeName")]
        [JsonRequired]
        public string BikeName { get; set; }
    
    }
}
