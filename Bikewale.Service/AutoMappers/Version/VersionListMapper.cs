using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Version
{
    public class VersionListMapper
    {
        /// <summary>
        /// Modified by :   Sumit Kate on 12 Apr 2016
        /// Description :   Mapping for MakeBase and ModelBase
        /// </summary>
        /// <param name="objVersion"></param>
        /// <returns></returns>
        internal static DTO.Version.VersionDetails Convert(Entities.BikeData.BikeVersionEntity objVersion)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeVersionEntity, VersionDetails>();
            return Mapper.Map<BikeVersionEntity, VersionDetails>(objVersion);
        }

        internal static VersionSpecifications Convert(BikeSpecificationEntity objSpecs)
        {
            Mapper.CreateMap<BikeSpecificationEntity, VersionSpecifications>();
            return Mapper.Map<BikeSpecificationEntity, VersionSpecifications>(objSpecs);
        }

        internal static List<VersionMinSpecs> Convert(List<BikeVersionMinSpecs> objMVSpecsList)
        {
            Mapper.CreateMap<BikeVersionsListEntity, ModelVersionList>();
            Mapper.CreateMap<BikeVersionMinSpecs, VersionMinSpecs>();
            return Mapper.Map<List<BikeVersionMinSpecs>, List<VersionMinSpecs>>(objMVSpecsList);
        }

        internal static IEnumerable<VersionBase> Convert(List<BikeVersionsListEntity> objVersionList)
        {
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            return Mapper.Map<List<BikeVersionsListEntity>, List<VersionBase>>(objVersionList);
        }

        internal static DTO.Version.v2.VersionDetails ConvertV2(BikeVersionEntity objVersion)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeVersionEntity, DTO.Version.v2.VersionDetails>();
            return Mapper.Map<BikeVersionEntity, DTO.Version.v2.VersionDetails>(objVersion);
        }
    }
}