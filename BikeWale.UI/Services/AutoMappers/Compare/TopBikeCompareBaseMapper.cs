using AutoMapper;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Compare
{
    public class TopBikeCompareBaseMapper
    {
        internal static IEnumerable<DTO.Compare.TopBikeCompareDTO> Convert(IEnumerable<Entities.Compare.TopBikeCompareBase> topBikeComapreList)
        {
            if (topBikeComapreList != null)
            {
                Mapper.CreateMap<Entities.Compare.TopBikeCompareBase, DTO.Compare.TopBikeCompareDTO>();
                return Mapper.Map<IEnumerable<Entities.Compare.TopBikeCompareBase>, IEnumerable<DTO.Compare.TopBikeCompareDTO>>(topBikeComapreList);
            }
            return null;
        }
    }
}