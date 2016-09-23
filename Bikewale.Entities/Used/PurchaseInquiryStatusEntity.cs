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
        public string Message { get; set; }
    }
}
