using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Finance
{

    public class FinanceCity
    {
        public int CWCityId { get; set; }
        public int ClientCityId { get; set; }
        public string CityName { get; set; }        
    }
}
