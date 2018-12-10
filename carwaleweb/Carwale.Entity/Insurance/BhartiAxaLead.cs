using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Insurance
{
    public class BhartiAxaLead
    {
        public int BhartiAxaLeadId { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerCity { get; set; }
        public string Product { get; set; }
        public string Agency { get; set; }
        public string CustomerDOB { get; set; }
        public string CustomerAge { get; set; }
        public string CustomerGender { get; set; }
        public string CustomerState { get; set; }
        public string Pincode { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string CarVersion { get; set; }
        public string CarRegDate { get; set; }
        public string InsuranceCompany { get; set; }
        public bool InsuranceType { get; set; }//true for new insurance and false in case of existing insurance i.e. renewal
        public string InsuranceExpDate { get; set; }
        public string ApiResponse { get; set; }
    }
}
