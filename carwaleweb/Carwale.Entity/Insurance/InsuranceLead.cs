using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Insurance
{
    public class InsuranceLead 
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string ZoneName { get; set; }
        public string CustomerDOB { get; set; }
        public string CustomerAge { get; set; }
        public string CustomerGender { get; set; }
        public int CustomerId { get; set; }
        public string Pincode { get; set; }

        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }

        public int InsuranceLeadId { get; set; }
        public Clients clientId { get; set; }

        public int Price { get; set; }
        public int ExpectedIDV { get; set; }
        public string Product { get; set; }
        public string Agency { get; set; }

        public string CarRegDate { get; set; }
        public string CarPurchaseDate { get; set; }
        public int CarManufactureYear { get; set; }

        public string InsuranceCompany { get; set; }

        public bool InsuranceNew { get; set; }//true for new insurance and false in case of existing insurance i.e. renewal
        public byte NCBPercent { get; set; }
        public bool IsNCBApplicable { get; set; }
        public string InsuranceExpDate { get; set; }
        public string ApiResponse { get; set; }

        public int LeadSource { get; set; }
        public Application Application { get; set; }
        public Platform Platform { get; set; }

        public int StateId { get; set; }
        public string UtmCode { get; set; }
    }
}
