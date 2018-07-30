using AutoMapper;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;
using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bikewale.Service.AutoMappers.PriceQuote.Version
{
    public class PQVersionListMapper
    {
        internal static IEnumerable<PQVersionBase> Convert(IEnumerable<Entities.BikeData.BikeVersionsListEntity> objVersionList)
        {
            Mapper.CreateMap<BikeVersionsListEntity, PQVersionBase>();
            return Mapper.Map<IEnumerable<BikeVersionsListEntity>, IEnumerable<PQVersionBase>>(objVersionList);
        }

        internal static IEnumerable<PQVersionBase> Convert(IEnumerable<OtherVersionInfoEntity> otherVersionInfoEntity)
        {
            ICollection<PQVersionBase> versionList = null;
            if (otherVersionInfoEntity != null)
            {
                otherVersionInfoEntity = otherVersionInfoEntity.OrderBy(v => v.OnRoadPrice);
                versionList = new Collection<PQVersionBase>();
                foreach (var version in otherVersionInfoEntity)
                {
                    if (version.OnRoadPrice > 0)
                    {
                        versionList.Add(new PQVersionBase
                        {
                            HostUrl = version.HostUrl,
                            OriginalImagePath = version.OriginalImagePath,
                            Price = version.OnRoadPrice,
                            VersionId = version.VersionId,
                            VersionName = version.VersionName
                        }); 
                    }
                }
                return versionList;
            }
            return null;
            
        }
    }
}