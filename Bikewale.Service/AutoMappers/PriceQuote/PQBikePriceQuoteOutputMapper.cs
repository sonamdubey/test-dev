using AutoMapper;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class PQBikePriceQuoteOutputMapper
    {
        internal static DTO.PriceQuote.BikeQuotation.PQBikePriceQuoteOutput Convert(Entities.PriceQuote.BikeQuotationEntity quotation)
        {
            Mapper.CreateMap<OtherVersionInfoEntity, OtherVersionInfoDTO>();
            Mapper.CreateMap<BikeQuotationEntity, PQBikePriceQuoteOutput>();
            return Mapper.Map<BikeQuotationEntity, PQBikePriceQuoteOutput>(quotation);
        }
    }
}