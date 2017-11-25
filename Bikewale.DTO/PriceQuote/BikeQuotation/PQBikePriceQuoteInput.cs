using System;

namespace Bikewale.DTO.PriceQuote.BikeQuotation
{
    /// <summary>
    /// BikeWale Price Quote Input
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class PQBikePriceQuoteInput
    {
        public UInt32 CityId { get; set; }
        public UInt32 AreaId { get; set; }
        public UInt32 VersionId { get; set; }
    }
}
