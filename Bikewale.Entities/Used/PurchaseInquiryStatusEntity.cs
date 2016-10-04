using System;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 23 Sep 2016
    /// Description :   Purchase Inquiry Status Entity.
    /// It describes the purchase inquiry result status
    /// </summary>
    public class PurchaseInquiryStatusEntity
    {
        public UInt16 Status
        {
            get
            {
                return Convert.ToUInt16(this.Code);
            }
        }
        public PurchaseInquiryStatusCode Code { get; set; }
        public string Message
        {
            get
            {
                switch (this.Code)
                {
                    case PurchaseInquiryStatusCode.InvalidRequest:
                        return "Invalid purchase inquiry request.";
                    case PurchaseInquiryStatusCode.Success:
                        return "Process completed successfully!!!";
                    case PurchaseInquiryStatusCode.InvalidCustomerInfo:
                        return "Information you provided was invalid. Please provide valid information.";
                    case PurchaseInquiryStatusCode.MobileNotVerified:
                        return "Buyer mobile is not verified.";
                    case PurchaseInquiryStatusCode.DuplicateUsedBikeInquiry:
                        return "Inquiry already submitted.";
                    case PurchaseInquiryStatusCode.MaxLimitReached:
                        return "Oops! You have reached the maximum limit for viewing inquiry details in a day.";
                    default:
                        return "";
                }
            }
        }
    }
}
