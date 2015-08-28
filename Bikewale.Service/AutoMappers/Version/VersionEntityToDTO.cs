using AutoMapper;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Version
{
    public class VersionEntityToDTO
    {
        internal static DTO.Version.VersionDetails ConvertVersionEntity(Entities.BikeData.BikeVersionEntity objVersion)
        {
            Mapper.CreateMap<BikeVersionEntity, VersionDetails>();
            return Mapper.Map<BikeVersionEntity, VersionDetails>(objVersion);
        }

        internal static VersionSpecifications ConvertSpecificationEntity(BikeSpecificationEntity objSpecs)
        {
            Mapper.CreateMap<BikeSpecificationEntity, VersionSpecifications>();
            return Mapper.Map<BikeSpecificationEntity, VersionSpecifications>(objSpecs);
        }

        internal static List<VersionMinSpecs> ConvertVersionMinSpecsList(List<BikeVersionMinSpecs> objMVSpecsList)
        {
            Mapper.CreateMap<BikeVersionsListEntity, ModelVersionList>();
            Mapper.CreateMap<BikeVersionMinSpecs, VersionMinSpecs>();
            return Mapper.Map<List<BikeVersionMinSpecs>, List<VersionMinSpecs>>(objMVSpecsList);
        }

        internal static IEnumerable<VersionBase> ConvertVersionsListEntityList(List<BikeVersionsListEntity> objVersionList)
        {
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            return Mapper.Map<List<BikeVersionsListEntity>, List<VersionBase>>(objVersionList);
        }
    }
}