using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockCondition
    {
        public bool? IsAccidental { get; set; }
        public string AccidentDetails { get; set; }
        public bool? IsPartReplaced { get; set; }
        public List<int> PartsReplaced { get; set; }
        public string PartReplacedText { get; set; }
        public bool? IsInsuranceClaimed { get; set; }
        public int? TotalInsuranceClaimed { get; set; }
        public bool? IsRegularlyServiced { get; set; }
        public bool? IsLoanPending { get; set; }
        public int? OutstandingLoanAmt { get; set; }
        public bool? IsTyreOriginal { get; set; }
        public int? TyresChangedAtKm { get; set; }
        public bool? IsWearTear { get; set; }
        public List<int> MinorScratchedParts { get; set; }
        public string MinorScratchText { get; set; }
        public List<int> DentedParts { get; set; }
        public string DentedPartText { get; set; }
        public bool? IsCarRepainted { get; set; }
        public bool? IsMechanicalIssue { get; set; }
        public List<int> EngineIssues { get; set; }
        public string EngineIssueText { get; set; }
        public List<int> ElectricIssues { get; set; }
        public string ElectricIssueText { get; set; }
        public bool? IsSuspensionIssue { get; set; }
        public bool? IsAcWorking { get; set; }
    }
}
