using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;
using System;

namespace Bikewale.Service.AutoMappers.Version
{
    public class VersionListMapper
    {
        /// <summary>
        /// Modified by :   Sumit Kate on 12 Apr 2016
        /// Description :   Mapping for MakeBase and ModelBase
        /// Modified by :   Aditi srivastava on 17 Oct 2016
        /// Description :   Mapping for Version Colors
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

        /// <summary>
        /// Created by  : Aditi srivastava on 20 Oct 2016
        /// Description : Mapping for Version Colors
        /// </summary>
        /// <param name="objVersionColors"></param>
        /// <returns></returns>
        internal static IEnumerable<DTO.BikeData.BikeColorsbyVersionsDTO> Convert(IEnumerable<Bikewale.Entities.BikeData.BikeColorsbyVersion> objVersionColors)
        {
            Mapper.CreateMap<Bikewale.Entities.BikeData.BikeColorsbyVersion, Bikewale.DTO.BikeData.BikeColorsbyVersionsDTO>();
            //Mapper.CreateMap<IEnumerable<Bikewale.Entities.BikeData.BikeColorsbyVersion>, BikeColorsbyVersion>();
            return Mapper.Map<IEnumerable<Bikewale.Entities.BikeData.BikeColorsbyVersion>, IEnumerable<BikeColorsbyVersionsDTO>>(objVersionColors);

        }
		/// <summary>
		/// Created by : Ashutosh Sharma on 26 Dec 2017
		/// Description : Method to map specs and features of a version.
		/// </summary>
		/// <param name="transposeModelSpecEntity">Entity with specs and features of a version.</param>
		/// <returns>DTO with specs and features of a version.</returns>
		internal static VersionSpecs Convert(TransposeModelSpecEntity transposeModelSpecEntity)
		{
			Mapper.CreateMap<Entities.BikeData.Specs, DTO.Model.Specs>();
			Mapper.CreateMap<Entities.BikeData.SpecsCategory, DTO.Model.SpecsCategory>();
			Mapper.CreateMap<Entities.BikeData.Specifications, DTO.Model.Specifications>();
			Mapper.CreateMap<Entities.BikeData.Features, DTO.Model.Features>();
			Mapper.CreateMap<TransposeModelSpecEntity, VersionSpecs>();
			return Mapper.Map<TransposeModelSpecEntity, VersionSpecs>(transposeModelSpecEntity);
		}
	}
}