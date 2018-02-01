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
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("notificationId")]
        public string NotificationId { get; set; }

        [JsonProperty("makeId")]
        public string MakeId { get; set; }

        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        [JsonProperty("entryDate")]
        public DateTime EntryDate { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("notificationTypeId")]
        public string NotificationTypeId { get; set; }

        [JsonProperty("notificationType")]
        public string NotificationType { get; set; }


    }
}
