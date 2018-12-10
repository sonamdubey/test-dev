using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class CarCondition
    {
        public bool? IsAccidental { get; set; }
        public string AccidentDetails { get; set; }
        
        public bool? IsInsuranceClaimed { get; set; }
        public int TotalInsuranceClaimed { get; set; }
        public bool? IsRegularlyServiced { get; set; }
        public bool? IsLoanPending { get; set; }
        public bool? IsTyreChanged { get; set; }
        public int TyresChangedAtKm { get; set; }
        public bool? IsWearTear { get; set; }

        public bool? IsPartReplaced { get; set; }
        public List<int> PartsReplaced { get; set; }
        public string PartReplacedText { get; set; }

        public List<int> MinorScratchedParts { get; set; }
        public string MinorScratchesPartsText { get; set; }

        public List<int> DentedParts { get; set; }
        public string DentedPartsText { get; set; }

        public List<int> ElectricalSystemParts { get; set; }
        public string ElectricalSystemPartsText { get; set; }
        public bool? IsCarRepainted { get; set; }

        public List<int> EngineParts { get; set; }
        public string EnginePartsText { get; set; }
    }
}
