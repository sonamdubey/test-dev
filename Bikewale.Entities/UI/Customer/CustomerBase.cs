namespace Bikewale.UI.Customer.Entities
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 5 Sept 2015
    /// </summary>
    public class CustomerBase
    {        
        public ulong CustomerId { get; set; }        
        public string CustomerName { get; set; }        
        public string CustomerEmail { get; set; }        
        public string CustomerMobile { get; set; }
    }
}
