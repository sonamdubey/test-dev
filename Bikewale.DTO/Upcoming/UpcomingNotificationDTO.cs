using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Upcoming
{
    /// <summary>
    /// Created by: Dhruv Joshi on 7th Feb 2018
    /// Description: DTO for Upcoming Bikes Notification 
    /// </summary>
    public class UpcomingNotificationDTO
    {
        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("notificationId")]
        public ushort NotificationId { get; set; }

        [JsonProperty("makeId")]
        public ushort MakeId { get; set; }

        [JsonProperty("modelId")]
        public ushort ModelId { get; set; }

        [JsonProperty("bikeName")]
        public string BikeName { get; set; }

        [JsonProperty("entryDate")]
        public DateTime EntryDate { get; set; }
        
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("notificationTypeId")]
        public ushort NotificationTypeId { get; set; }

    }
}
