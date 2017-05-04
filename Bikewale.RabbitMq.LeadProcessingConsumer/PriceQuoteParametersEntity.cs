﻿using System;
namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// Modified By : Sushil Kumar On 11th Nov 2015
    /// Summary : Added colorId to Update colorId in PQ_NewBikeDealerPriceQuotes
    /// Modified by :   Lucky Rathore on 20 April 2016
    /// Description :   Added RefPQId as new property
    /// Modified by :   Sumit Kate on 02 May 2016
    /// Description :   Added CampaignId
    public class PriceQuoteParametersEntity
    {
        public uint VersionId { get; set; }
        public uint CityId { get; set; }
        public uint AreaId { get; set; }    //Added By Sadhana Upadhyay on 24th Oct 2014
        public ulong CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public string ClientIP { get; set; }
        public UInt16 SourceId { get; set; }
        //Added By : Sadhana Upadhyay on 20 July 2015
        public uint DealerId { get; set; }
        public uint ModelId { get; set; }
        public uint ColorId { get; set; }
        public uint? CampaignId { get; set; }
    }
}

