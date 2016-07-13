using System;

namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by  :   Sumit Kate on 12 July 2016
    /// Description :   Dealer Contract Entity
    /// </summary>
    public class DealerContractEntity
    {
        public int ContractId { get; set; }
        public int DealerId { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public int ContractStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
