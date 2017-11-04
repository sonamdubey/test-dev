using AutoMapper;
using Bikewale.ManufacturerCampaign.Entities;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Service.AutoMappers.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 18 Apr 2017
    /// </summary>
    public class BikeModelsMapper
    {
        /// <summary>
        /// Writtten By : Ashish G. Kamble on 18 Apr 2017
        /// Summary : Method to map the bikemodels list
        /// </summary>
        /// <param name="objModels"></param>
        /// <returns></returns>
        internal static IEnumerable<ModelBase> Convert(IEnumerable<BikeModelEntityBase> objModels)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            return Mapper.Map<IEnumerable<BikeModelEntityBase>, IEnumerable<ModelBase>>(objModels);
        }
        internal static IEnumerable<BikeModelDTO> ConvertV2(IEnumerable<BikeModelEntity> objModels)
        {
            Mapper.CreateMap<BikeModelEntity, BikeModelDTO >();
            return Mapper.Map<IEnumerable<BikeModelEntity>, IEnumerable<BikeModelDTO>>(objModels);
        }
    }
}