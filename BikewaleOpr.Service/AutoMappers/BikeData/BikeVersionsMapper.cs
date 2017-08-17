using AutoMapper;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Service.AutoMappers.BikeData
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 7th Aug 2017
    /// </summary>
    public class BikeVersionsMapper
    {
        /// <summary>
        /// Created by: Vivek Singh Tomar on 7th Aug 2017
        /// Summary: Function to map Bike version list
        /// </summary>
        /// <param name="objVersions"></param>
        /// <returns></returns>
        internal static IEnumerable<VersionBase> Convert(IEnumerable<BikeVersionEntityBase> objVersions)
        {
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            return Mapper.Map<IEnumerable<BikeVersionEntityBase>, IEnumerable<VersionBase>>(objVersions);
        }
    }
}