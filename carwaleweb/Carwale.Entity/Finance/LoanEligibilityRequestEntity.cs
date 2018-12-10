using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Finance
{
    public class LoanEligibilityRequestEntity
    {
        public int FinanceLeadId { get; set; }
        public int IncomeTypeId { get; set; }
        public int AnnualIncome { get; set; }
        public int VersionId { get; set; }
        public int CityId { get; set; }
        public DateTime? CustomerDOB { get; set; }
        public int CompanyId { get; set; }
        public bool IsExistingCustomer { get; set; }
        public int Tenor { get; set; }
        public int ResidenceTypeId { get; set; }
        public int StabilityTime { get; set; }
        public int? CustomerExp { get; set; }
        public long ExShowroomPrice { get; set; }
    }
}
