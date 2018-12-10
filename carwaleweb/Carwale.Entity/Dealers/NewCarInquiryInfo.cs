using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Dealers
{
    public class NewCarInquiryInfo
    {
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }

        public string CustomerEmail { get; set; }

        public string BuyingTime { get; set; }

        public string VersionId { get; set; }

        public string CityId { get; set; }

        public string InquirySourceId { get; set; }

        public string Eagerness { get; set; }

        public string ModelId { get; set; }

        public string FuelType { get; set; }

        public string Transmission { get; set; }

        public string RequestType { get; set; }

        public string CWInquiryId { get; set; }

        public string CWCustomerId { get; set; }


        //Fields specifically for inquiries like ServiceRequest and Loan request added from dealer website 

        public string Comments { get; set; }

        public string RegistrationNo { get; set; }

        public string PreferedDate { get; set; }

        public string TypeOfService { get; set; }

        //Inquiry type 5:Service Request, 8:Finance Inquiry(Loan), 10:Grievance Redressal from Dealer Website

        public string InquiryTypeId { get; set; }

        public string IsCorporate { get; set; }

        public string CompanyName { get; set; }

        //added by ashish verma

        public int CwOfferId { get; set; }

        public string CouponCode { get; set; }

        public string DealsStockId { get; set; }

        public string IsPaymentSuccess { get; set; }

    }
}
