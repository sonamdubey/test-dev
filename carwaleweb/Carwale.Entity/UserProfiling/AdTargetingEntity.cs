using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.UserProfiling
{
    public class AdTargetingEntity
    {
        public string CarType { get; set; }
        public int NumberOfLeads { get; set; }
        public string ModelList { get; set; }
        public string BodyStyleList { get; set; }
        public string BudgetSegmentList { get; set; }
        public int NumberOfCars { get; set; }
    }
}
