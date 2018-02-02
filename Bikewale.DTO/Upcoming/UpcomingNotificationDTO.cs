using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Upcoming
{
    public class UpcomingNotificationDTO
    {
        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("notificationId")]
        public int NotificationId { get; set; }

        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("entryDate")]
        public DateTime EntryDate { get; set; }
        
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("notificationTypeId")]
        public int NotificationTypeId { get; set; }

    }
}
