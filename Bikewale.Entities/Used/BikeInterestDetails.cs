
namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 02 Sep 2016
    /// Description :   BikeInterest Details
    /// </summary>
    public class BikeInterestDetails
    {
        public UsedBikeSellerBase Seller { get; set; }
        public Customer.CustomerEntityBase Buyer { get; set; }
        public bool ShownInterest { get; set; }
    }
}
