using AutoMapper;
using BikewaleOpr.DTO.Make;
using BikewaleOpr.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Service.AutoMappers.Dealer
{
    public class DealerCampaignMapper
    {

        internal static IEnumerable<DTO.Make.MakeBase> Convert(IEnumerable<Entities.BikeMakeEntityBase> makes)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            return Mapper.Map<IEnumerable<BikeMakeEntityBase>, IEnumerable<MakeBase>>(makes);
        }
    }
}