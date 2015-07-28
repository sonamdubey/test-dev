using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Customer;

namespace Bikewale.Entities.PriceQuote
{
    public class PriceQuoteParametersEntity
    {
        public uint VersionId { get; set; }
        public uint CityId { get; set; }
        public uint AreaId { get; set; }    //Added By Sadhana Upadhyay on 24th Oct 2014
        public UInt16 BuyingPreference { get; set; }
        public ulong CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public string ClientIP { get; set; }
        public UInt16 SourceId { get; set; }
        //Added By : Sadhana Upadhyay on 20 July 2015
        public uint DealerId { get; set; }  
        public uint ModelId { get; set; }
    }
}
