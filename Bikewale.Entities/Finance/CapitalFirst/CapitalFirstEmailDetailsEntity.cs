using System;

namespace Bikewale.Entities.Finance.CapitalFirst
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13-Sep-2017
    /// Summary: Capiutal first email entity
    /// 
    /// </summary>
    public class CapitalFirstLeadEntity
    {
        public string CTLeadId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string BikeName { get; set; }
        public uint Exshowroom { get; set; }
        public uint RTO { get; set; }
        public uint Insurance { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime VoucherExpiryDate { get; set; }
        public string AgentName { get; set; }
        public string AgentNumber { get; set; }

        public uint OnRoadPrice { get; set; }
        public uint LoanAmount { get; set; }
        public string LoanAmountStr { get; set; }
        public string VersionName { get; set; }
        public bool Status { get; set; }
        public string Emi { get; set; }
        public string Downpayment { get; set; }
    }
}
