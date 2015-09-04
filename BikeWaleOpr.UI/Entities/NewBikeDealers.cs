using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BikeWaleOpr.Entities
{
    [Serializable,DataContract]
    public class NewBikeDealers
    {
        [JsonProperty("dealerId"),DataMember]
        public UInt32 DealerId { get; set; }

        [JsonProperty("areaId"),DataMember]
        public UInt32 AreaId { get; set; }

        [JsonProperty("dealerName"),DataMember]
        public string Name { get; set; }

        [JsonProperty("emailId"),DataMember]
        public string EmailId { get; set; }

        [JsonProperty("mobileNo"),DataMember]
        public string MobileNo { get; set; }

        [JsonProperty("website"),DataMember]
        public string Website { get; set; }

        [JsonProperty("workingTime"),DataMember]
        public string WorkingTime { get; set; }


        [JsonProperty("address"),DataMember]
        public string Address { get; set; }

        public StateEntityBase objState { get; set; }

        public CityEntityBase objCity { get; set; }

        public AreaEntityBase objArea { get; set; }
    }   //End of Class
}   //End of namespace
