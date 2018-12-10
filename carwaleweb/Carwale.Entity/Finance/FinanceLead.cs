
using Carwale.Entity.Enum;
using System;
namespace Carwale.Entity.Finance
{
    public class FinanceLead
    {
        public int FinanceLeadId { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Pan_No { get; set; }
        public string Res_address { get; set; }
        public string Res_address2 { get; set; }
        public string Res_address3 { get; set; }
        public string Resi_type { get; set; }
        public int CityId { get; set; }
        public string Mobile { get; set; }
        public int LeadClickSource { get; set; }
        public string Res_City { get; set; }
        public string Resi_City_other { get; set; }
        public string Resi_City_other1 { get; set; }
        public string Res_Pin { get; set; }
        public string Company_Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Designation { get; set; }
        public string Emp_Type { get; set; }
        public int Monthly_Income { get; set; }
        public string Card_Held { get; set; }
        public string Source_Code { get; set; }
        public string Promo_Code { get; set; }
        public string Lead_Date_Time { get; set; }
        public string Product_Applied_For { get; set; }
        public string ExistingCust { get; set; }
        public int YearsInEmp { get; set; }
        public string Emi_Paid { get; set; }
        public string Car_Make { get; set; }
        public string Car_Model { get; set; }
        public string TypeOfLoan { get; set; }
        public string IP_Address { get; set; }
        public string Indigo_UniqueKey { get; set; }
        public string Indigo_RequestFromYesNo { get; set; }
        public int VersionId { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal ROI { get; set; }
        public decimal LTV { get; set; }
        public int CarPrice { get; set; }
        public bool IsPermitted { get; set; }
        public bool IsPushSuccess { get; set; }
        public int BankTypeId { get; set; }
        public string ApiResponse { get; set; }
        public string BuyingPeriod { get; set; }
        public int IncomeTypeId { get; set; }
        public int YearsInRes { get; set; }
        public string FailureReason { get; set; }
        public Clients ClientId { get; set; }
        public Platform PlatformId { get; set; }
        public string ClientCustomerId { get; set; }
        public string UtmCode { get; set; }
    }
}
