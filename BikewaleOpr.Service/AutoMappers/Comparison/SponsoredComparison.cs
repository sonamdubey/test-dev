using AutoMapper;
using Bikewale.Comparison.DTO;
using Bikewale.Comparison.Entities;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.DTO.Make;
using BikewaleOpr.DTO.UserReviews;
using BikewaleOpr.Entities;
using BikewaleOpr.Entities.UserReviews;
using System.Collections.Generic;

namespace BikewaleOpr.Service.AutoMappers
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Jul-2017
    /// Summary: Mapper class for sposnored comparison
    /// 
    /// </summary>
    public class SponsoredComparisonMapper
    {
        internal static IEnumerable<SponsoredComparisonDTO> Convert(IEnumerable<SponsoredComparison> objSponsored)
        {
            Mapper.CreateMap<SponsoredComparison, SponsoredComparisonDTO>();
            return Mapper.Map<IEnumerable<SponsoredComparison>, IEnumerable<SponsoredComparisonDTO>>(objSponsored);
        }

        internal static TargetSponsoredMappingDTO Convert(TargetSponsoredMapping objTargetSponsored)
        {
            Mapper.CreateMap<BikeModel, BikeModelDTO>();
            Mapper.CreateMap<BikeModelVersionMapping, BikeModelVersionMappingDTO>();
            Mapper.CreateMap<TargetSponsoredMapping, TargetSponsoredMappingDTO>();
            return Mapper.Map<TargetSponsoredMapping, TargetSponsoredMappingDTO>(objTargetSponsored);
        }
    }
}
