
namespace Bikewale.Entities.PriceQuote.v2
{
    /// <summary>
    /// Created by  : Pratibha Verma on 26 June 2018
    /// Description : PriceQuote.v2.PQCustomerDetailInput entity created to map PriceQuote.v3.PQCustomerDetailInput dto
    /// </summary>
    public class PQCustomerDetailInput
    {
        public uint DealerId { get; set; }

        public string PQId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerMobile { get; set; }

        public string CustomerEmail { get; set; }

        public string ClientIP { get; set; }

        public string PageUrl { get; set; }

        public uint VersionId { get; set; }

        public uint CityId { get; set; }

        public ushort? LeadSourceId { get; set; }

        public string DeviceId { get; set; }

        public uint LeadId { get; set; }
    }
}
