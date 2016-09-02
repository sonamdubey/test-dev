using Bikewale.Entities.Customer;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Create by   :   Sumit Kate on 01 Sep 2016
    /// Description :   Upload Photo Request Entity
    /// </summary>
    public class PhotoRequest
    {
        public CustomerEntityBase Buyer { get; set; }
        public string Message { get; set; }
        public string ProfileId { get; set; }
        public string BikeName { get; set; }
    }
}
