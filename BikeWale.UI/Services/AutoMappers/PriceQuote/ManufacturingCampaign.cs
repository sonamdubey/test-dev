using AutoMapper;
using Bikewale.DTO;
using Bikewale.Entities;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 10-May-2017
    /// Automapper for Manufacturer campaign
    /// </summary>
    public class ManufacturingCampaign
    {
        internal static IEnumerable<ManufactureDealerDTO> Convert(IEnumerable<ManufacturerDealer> objDealers)
        {
           return Mapper.Map<IEnumerable<ManufacturerDealer>, IEnumerable<ManufactureDealerDTO>>(objDealers);
        }
    }
}