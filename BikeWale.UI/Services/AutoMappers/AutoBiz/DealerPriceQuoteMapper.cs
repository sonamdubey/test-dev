using AutoMapper;
using Bikewale.DTO.BikeBooking.Make;
using Bikewale.DTO.Location;
using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.AutoBiz
{
    public class DealerPriceQuoteMapper
    {
        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 06-07-2016
        /// </summary>
        /// <param name="objCityList"></param>
        /// <returns></returns>
        internal static IEnumerable<CityEntityBaseDTO> Convert(IEnumerable<CityEntityBase> objCityList)
        {
          
            return Mapper.Map<IEnumerable<CityEntityBase>, IEnumerable<CityEntityBaseDTO>>(objCityList);
        }

        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 06-07-2016
        /// </summary>
        /// <param name="objMakesList"></param>
        /// <returns></returns>
        internal static IEnumerable<BBMakeBase> Convert(IEnumerable<BikeMakeEntityBase> objMakesList)
        {
            
            return Mapper.Map<IEnumerable<BikeMakeEntityBase>, IEnumerable<BBMakeBase>>(objMakesList);
        }

        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 06-07-2016
        /// </summary>
        /// <param name="objTerm"></param>
        /// <returns></returns>
        internal static OfferHtmlDTO Convert(OfferHtmlEntity objTerm)
        {
            
            return Mapper.Map<OfferHtmlEntity, OfferHtmlDTO>(objTerm);
        }
    }
}