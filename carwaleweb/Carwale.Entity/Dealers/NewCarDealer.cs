using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.Entity.Dealers
{
    /// <summary>
    /// This entity class is for contents on new car dealers page
    /// </summary>
    [Serializable]
    public class NewCarDealer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("pincode")]
        public string PinCode { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("workingTime")]
        public string WorkingTime { get; set; }

        [JsonProperty("mobileNo")]
        public string MobileNo { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("isPremium")]
        public bool IsPremium { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("dealerId")]
        public int DealerId { get; set; }

        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonProperty("campaignId")]
        public int CampaignId { get; set; }

        [JsonProperty("showEmail")]
        public bool ShowEmail { get; set; }

        [JsonProperty("leadPanel")]
        public short LeadPanel { get; set; }

        [JsonProperty("businessType")]
        public short BusinessType { get; set; }

        [JsonProperty("serverTime")]
        public long ServerTime { get; set; }

        [JsonProperty("about")]
        public string DealerContent { get; set; }

        [JsonProperty("showroomImage")]
        public string ShowroomImage { get; set; }

        [JsonProperty("faxNo")]
        public string FaxNo { get; set; }

        [JsonProperty("landlineNo")]
        public string LandlineNo { get; set; }

        [JsonProperty("landlineCode")]
        public string LandlineCode { get; set; }

        [JsonProperty("newCarDealerId")]
        public int NewCarDealerId { get; set; }

        [JsonProperty("stateId")]
        public int StateId { get; set; }

        [JsonProperty("profilePhotoHostUrl")]
        public string ProfilePhotoHostUrl { get; set; }

        [JsonProperty("profilePhotoUrl")]
        public string ProfilePhotoUrl { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("showroomStartTime")]
        public string ShowroomStartTime { get; set; }
        [JsonProperty("showroomEndTime")]
        public string ShowroomEndTime { get; set; }

    }
}
