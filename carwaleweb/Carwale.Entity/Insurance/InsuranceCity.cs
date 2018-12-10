using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Insurance
{
    [Serializable]
    public class InsuranceCity
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
    }
}

