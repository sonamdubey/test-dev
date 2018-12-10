using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.CarDetails
{
    [Serializable]
    public class NonAbsureCarCondition
    {
        public string AC { get; set; }
        public string Battery { get; set; }
        public string Brakes { get; set; }
        public string Electricals { get; set; }
        public string Engine { get; set; }
        public string Exterior { get; set; }
        public string Interior { get; set; }
        public string Seats { get; set; }
        public string Suspensions { get; set; }
        public string Tyres { get; set; }
        public string OverAll { get; set; }        
    }
}
