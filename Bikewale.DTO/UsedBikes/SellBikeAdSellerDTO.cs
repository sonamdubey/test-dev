
using System.ComponentModel.DataAnnotations;
namespace Bikewale.DTO.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 14 Oct 2016
    /// Description :   Sell Bike Ad Seller DTO
    /// </summary>
    public class SellBikeAdSellerDTO : Customer.CustomerBase
    {
        [Required]
        public SellerType SellerType { get; set; }
    }

    public enum SellerType { Dealer = 1, Individual = 2 }
}
