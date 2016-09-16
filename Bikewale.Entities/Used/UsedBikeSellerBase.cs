
namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Used Bike Seller Entity
    /// </summary>
    public class UsedBikeSellerBase
    {
        public Customer.CustomerEntityBase Details { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
    }
}
