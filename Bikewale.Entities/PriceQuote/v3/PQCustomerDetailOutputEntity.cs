
namespace Bikewale.Entities.PriceQuote.v3
{
    /// <summary>
    /// Created by : Pratibha Verma on 12 October 2018
    /// Description: new version for PQId changes(api/v1/PQCustomerDetailWithOutPQ/)
    /// </summary>
    public class PQCustomerDetailOutputEntity
    {
        public string PQId { get; set; }

        public bool IsSuccess { get; set; }

        public DealerDetails Dealer { get; set; }

        public sbyte NoOfAttempts { get; set; }

        public uint LeadId { get; set; }
    }
}
