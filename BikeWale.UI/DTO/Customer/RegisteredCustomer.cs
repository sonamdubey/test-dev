using Newtonsoft.Json;

namespace Bikewale.DTO.Customer
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 8 Sept 2015
    /// Summary : Class hold the values for the resitered customer
    /// </summary>
    public class RegisteredCustomer : AuthenticatedCustomer
    {
        [JsonProperty("isNewCustomer")]
        public bool IsNewCustomer { get; set; }
    }
}
