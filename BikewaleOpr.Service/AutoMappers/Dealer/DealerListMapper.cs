using AutoMapper;
using BikewaleOpr.DTO.Dealers;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Service.AutoMappers.Dealer
{
    /// <summary>
    /// Created by : Aditi Srivastava on 9 Feb 2017
    /// Summary    : Dealer and make list DTO mapping
    /// </summary>
    public class DealerListMapper
    {
        /// <summary>
        /// Created by : Aditi Srivastava on 9 Feb 2017
        /// Summary    : Get list of all makes in city
        /// </summary>
        /// <param name="objMakes"></param>
        /// <returns></returns>
        internal static IEnumerable<BikeMakeBase> Convert(IEnumerable<BikeMakeEntityBase> objMakes)
        {
            Mapper.CreateMap<BikeMakeEntityBase, BikeMakeBase>();
            return Mapper.Map<IEnumerable<BikeMakeEntityBase>, IEnumerable<BikeMakeBase>>(objMakes);
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 9 Feb 2017
        /// Summary    : Get list of dealers by make and city
        /// </summary>
        /// <param name="objDealers"></param>
        /// <returns></returns>
        internal static IEnumerable<DealerBase> Convert(IEnumerable<DealerEntityBase> objDealers)
        {
            Mapper.CreateMap<DealerEntityBase, DealerBase>();
            return Mapper.Map<IEnumerable<DealerEntityBase>, IEnumerable<DealerBase>>(objDealers);
        }
                
    }
}