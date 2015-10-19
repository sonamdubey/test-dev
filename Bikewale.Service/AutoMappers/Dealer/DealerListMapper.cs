using AutoMapper;
using Bikewale.DAL.Dealer;
using Bikewale.DTO.Dealer;
using Bikewale.DTO.Make;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Dealer;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            Mapper.CreateMap<NewBikeDealerEntityBase, NewBikeDealerBase>();
            return Mapper.Map<IEnumerable<NewBikeDealerEntityBase>, IEnumerable<NewBikeDealerBase>>(objDealers);
        }
    }
}



        