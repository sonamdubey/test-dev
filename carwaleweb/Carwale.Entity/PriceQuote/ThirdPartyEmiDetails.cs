using Carwale.Entity.Enum;
using System;

namespace Carwale.Entity.PriceQuote
{
    [Serializable]
    public class ThirdPartyEmiDetails
    {
        public int CarVersionId { get; set; }
        public bool IsMetallic { get; set; }
        public EmiType EmiType { get; set; }
        public int LoanAmount { get; set; }
        public int Emi { get; set; }
        public int LumpsumAmount { get; set; }
        public Byte TenureInMonth { get; set; }
        public float InterestRate { get; set; }
    }
}
