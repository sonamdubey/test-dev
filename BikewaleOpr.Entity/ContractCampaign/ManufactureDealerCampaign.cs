using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.ContractCampaign
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
