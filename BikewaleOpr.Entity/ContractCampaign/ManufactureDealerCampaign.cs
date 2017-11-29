using System;

namespace BikewaleOpr.Entities.ContractCampaign
{
    public class ManufactureDealerCampaign
    {
        public int id { get; set; }
        public int dealerid { get; set; }
        public string description { get; set; }
        public int isactive { get; set; }
        public string maskingnumber { get; set; }
        public DateTime entryDate { get; set; }
    }
}
