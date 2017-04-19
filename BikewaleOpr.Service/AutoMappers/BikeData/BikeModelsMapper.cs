using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.Entities.BikeData;

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
        internal static IEnumerable<BikeModelBaseDTO> Convert(IEnumerable<BikeModelEntityBase> objModels)
        {
            Mapper.CreateMap<BikeModelEntityBase, BikeModelBaseDTO>();            
            return Mapper.Map<IEnumerable<BikeModelEntityBase>, IEnumerable<BikeModelBaseDTO>>(objModels);
        }
    }
}