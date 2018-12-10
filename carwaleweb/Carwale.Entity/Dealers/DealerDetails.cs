using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Dealers
{
    [Serializable]
    //[JsonObject]
    public class DealerDetails
    {
        public int MakeId { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
        public string Address{ get; set; }
        public string Pincode{ get; set; }
        public string ContactNo{ get; set; }
        public string FaxNo{ get; set; }
        public string EMailId{ get; set; }
        public string WebSite{ get; set; }
        public string WorkingHours{ get; set; }
        public string HostURL{ get; set; }
        public string ProfilePhoto { get; set; }
        public string ContactPerson{ get; set; }
        public string DealerMobileNo{ get; set; }
        public string Mobile{ get; set; }
        public string ShowroomStartTime{ get; set; }
        public string ShowroomEndTime{ get; set; }
        public string PrimaryMobileNo{ get; set; }
        public string SecondaryMobileNo{ get; set; }
        public string LandLineNo{ get; set; }
        public string DealerArea{ get; set; }
        public double Latitude{ get; set; }
        public double Longitude { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public int IsPremium { get;set; }
        public int DealerId { get; set; }
        public int CampaignId { get; set; }
        public int StateId { get; set; }
        public bool IsLocatorActive { get; set; }
        public int Distance { get; set; }
    }
}
