
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
namespace Bikewale.DTO.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 14 Oct 2016
    /// Description :   Sell Bike Ad Seller DTO
    /// Modified By : Aditi Srivastava on 10 Nov 2016
    /// Description : Added inquiryid
    /// </summary>
    public class SellerDTO : Customer.CustomerBase
    {
        [Required]
        [JsonProperty("sellerType")]
        public SellerType SellerType { get; set; }
        [JsonProperty("otp")]
        public string Otp { get; set; }
        [JsonProperty("inquiryId")]
        public uint InquiryId { get; set; }
        [JsonProperty("isEdit")]
        public bool IsEdit { get; set; }

    }

    public enum SellerType { Dealer = 1, Individual = 2 }
    public enum SellAdStatus
    {
        Approved = 1, // live and approved
        Fake = 2,
        Sold = 3,
        MobileUnverified = 4,
        MobileVerified = 5 // Mobile is verified but listing is yet to be approved
    }

    /// <summary>
    /// Created by  :   Sangram Nandkhile on 14 oct 2016
    /// Description :   Sell bike Status Entity.
    /// It describes the seller ad result status
    /// </summary>
    public class SellBikeAdStatusDTO
    {
        public UInt16 Status
        {
            get
            {
                return Convert.ToUInt16(this.Code);
            }
        }
        public SellAdStatus Code { get; set; }
        public string Message
        {
            get
            {
                switch (Code)
                {
                    case SellAdStatus.Approved:
                        return "Approved";
                    case SellAdStatus.Fake:
                        return "User has been marked fake";
                    case SellAdStatus.Sold:
                        return "Sold";
                    case SellAdStatus.MobileUnverified:
                        return "OTP generated";
                    case SellAdStatus.MobileVerified:
                        return "Mobile has been verified";
                    default:
                        return string.Empty;
                }
            }
        }
    }

    public class SellBikeInquiryResultDTO
    {
        public SellBikeAdStatusDTO Status { get; set; }
        public uint InquiryId { get; set; }
        public ulong CustomerId { get; set; }
    }
}
