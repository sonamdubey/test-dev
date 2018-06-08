
using System;
namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Created by : Snehal Dange on 4th May 2018
    /// Description: Entity created to store success values from notify dealer and customer in dealer lead notification
    /// </summary>
    public class PQCustomerDetailOutputEntity
    {
        public UInt64 PQId { get; set; }

        public bool IsSuccess { get; set; }

        public DealerDetails Dealer { get; set; }

        public sbyte NoOfAttempts { get; set; }
    }
}
