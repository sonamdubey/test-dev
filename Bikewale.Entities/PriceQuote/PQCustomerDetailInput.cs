
namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Created by :Snehal Dange on 2nd May 2018
    /// Desc :  PQCustomerDetailInput entity created to map PQCustomerDetailInput dto
    /// </summary>
    public class PQCustomerDetailInput
    {
        public uint DealerId { get; set; }

        public uint PQId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerMobile { get; set; }

        public string CustomerEmail { get; set; }

        public string ClientIP { get; set; }

        public string PageUrl { get; set; }

        public string VersionId { get; set; }

        public string CityId { get; set; }

        //Added By  : Sadhana Upadhyay on 29 Dec 2015
        public ushort? LeadSourceId { get; set; }

        public string DeviceId { get; set; }
    }
}
