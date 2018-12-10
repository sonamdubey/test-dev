using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.EmailAlerts
{
    public class RecommendationEmailData
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerKey { get; set; }
        public string ProfileId { get; set; }
        public string CarName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string RootName { get; set; }
        public string City { get; set; }
    }
}
