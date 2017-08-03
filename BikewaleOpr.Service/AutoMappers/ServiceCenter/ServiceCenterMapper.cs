using AutoMapper;
using BikewaleOpr.DTO.City;
using BikewaleOpr.DTO.ServiceCenter;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ServiceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Service.AutoMappers.ServiceCenter
{
    /// <summary>
    /// Created BY : Snehal Dange on 29 July 2017
    /// Summary :  Class for auto mapping Service center data
    /// </summary>
    public class ServiceCenterMapper
    {
        /// <summary>
        /// Created BY : Snehal Dange on 29 July 2017
        /// Summary :  Class for auto mapping City entity to City Dto
        /// </summary>
        /// <param name="objCityList"></param>
        /// <returns></returns>
        internal static IEnumerable<CityBase> Convert(IEnumerable<CityEntityBase> objCityList)
        {
            Mapper.CreateMap<CityEntityBase, CityBase>();
            return Mapper.Map<IEnumerable<CityEntityBase>, IEnumerable<CityBase>>(objCityList);
        }

        /// <summary>
        /// Created BY : Snehal Dange on 29 July 2017
        /// Summary :  Class for auto mapping Service Center Entity to Dto.
        /// </summary>
        /// <param name="serviceCenterList"></param>
        /// <returns></returns>
        internal static IEnumerable<ServiceCenterBase> Convert(IEnumerable<ServiceCenterDetails> serviceCenterList)
        {
            Mapper.CreateMap<ServiceCenterDetails, ServiceCenterBase>();
            return Mapper.Map<IEnumerable<ServiceCenterDetails>, IEnumerable<ServiceCenterBase>>(serviceCenterList);

        }

    }
}