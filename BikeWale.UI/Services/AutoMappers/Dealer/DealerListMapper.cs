using AutoMapper;
using Bikewale.DTO.Dealer;
using Bikewale.Entities.Dealer;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Dealer
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 7th October 2015
    /// dealers Entity - DTO Mapping
    /// </summary>
    public class DealerListMapper
    {
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 7th October 2015
        /// Get list of all dealers with details for a given make and city
        /// </summary>

        internal static IEnumerable<NewBikeDealerBase> Convert(IEnumerable<NewBikeDealerEntityBase> objDealers)
        {
           return Mapper.Map<IEnumerable<NewBikeDealerEntityBase>, IEnumerable<NewBikeDealerBase>>(objDealers);
        }

        /// <summary>
        /// Craeted by  :   Sumit Kate on 20 May 2016
        /// Description :   AutoMapper for Dealer List api v2 DTO
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        internal static IEnumerable<DTO.Dealer.v2.NewBikeDealerBase> ConvertV2(IEnumerable<Entities.DealerLocator.DealersList> enumerable)
        {
            return Mapper.Map<IEnumerable<Entities.DealerLocator.DealersList>, IEnumerable<DTO.Dealer.v2.NewBikeDealerBase>>(enumerable);
        }
    }
}



