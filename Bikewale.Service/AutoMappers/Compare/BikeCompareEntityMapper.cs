using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Bike Compare Entity Mapper
    /// </summary>
    public class BikeCompareEntityMapper
    {
        /// <summary>
        /// Converts Entity to DTO
        /// </summary>
        /// <param name="compareEntity"></param>
        /// <returns></returns>
        internal static DTO.Compare.BikeCompareDTO Convert(Entities.Compare.BikeCompareEntity compareEntity)
        {
            if (compareEntity != null)
            {
                Mapper.CreateMap<Bikewale.Entities.Compare.BikeEntityBase, Bikewale.DTO.Compare.BikeDTOBase>();
                Mapper.CreateMap<Bikewale.Entities.Compare.BikeColor, Bikewale.DTO.Compare.BikeColorDTO>();
                Mapper.CreateMap<Bikewale.Entities.Compare.BikeCompareEntity, Bikewale.DTO.Compare.BikeCompareDTO>();                
                Mapper.CreateMap<Bikewale.Entities.Compare.BikeEntityBase, Bikewale.DTO.Compare.BikeDTOBase>();
                Mapper.CreateMap<Bikewale.Entities.Compare.BikeFeature, Bikewale.DTO.Compare.BikeFeatureDTO>();
                Mapper.CreateMap<Bikewale.Entities.Compare.BikeModelColor, Bikewale.DTO.Compare.BikeModelColorDTO>();
                Mapper.CreateMap<Bikewale.Entities.Compare.BikeSpecification, Bikewale.DTO.Compare.BikeSpecificationDTO>();
                Mapper.CreateMap<Bikewale.Entities.Compare.CompareBikeColor, Bikewale.DTO.Compare.CompareBikeColorDTO>();
                Mapper.CreateMap<Bikewale.Entities.Compare.CompareBikeColorCategory, Bikewale.DTO.Compare.CompareBikeColorCategoryDTO>();
                Mapper.CreateMap<Bikewale.Entities.Compare.CompareBikeData, Bikewale.DTO.Compare.CompareBikeDataDTO>();
                Mapper.CreateMap<Bikewale.Entities.Compare.CompareMainCategory, Bikewale.DTO.Compare.CompareMainCategoryDTO>();
                Mapper.CreateMap<Bikewale.Entities.Compare.CompareSubCategory, Bikewale.DTO.Compare.CompareSubCategoryDTO>();
                Mapper.CreateMap<Bikewale.Entities.Compare.CompareSubMainCategory, Bikewale.DTO.Compare.CompareSubMainCategoryDTO>();
                return Mapper.Map<Bikewale.Entities.Compare.BikeCompareEntity, Bikewale.DTO.Compare.BikeCompareDTO>(compareEntity);
            }
            return null;
        }
    }
}