using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    public class ApiLogData
    {
        public string RequestUri { get; set; }
        public string RequestMethod { get; set; }
        public string RequestContent { get; set; }
        public string RequestHeaders { get; set; }
        public string ResponseContent { get; set; }
        public int ResponseStatusCode { get; set; }
        public string ClientIp { get; set; }
    }
}
