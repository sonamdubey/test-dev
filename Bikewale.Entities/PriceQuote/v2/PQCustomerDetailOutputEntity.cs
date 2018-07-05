
using System;
namespace Bikewale.Entities.PriceQuote.v2
{
    /// <summary>
    /// Created by : Pratibha Verma on 26 June 2018
    /// Description: Entity created to store leadId
    /// </summary>
    public class PQCustomerDetailOutputEntity
    {
        public UInt64 PQId { get; set; }

        public bool IsSuccess { get; set; }

        public DealerDetails Dealer { get; set; }

        public sbyte NoOfAttempts { get; set; }

        public uint LeadId { get; set; }
    }
}
