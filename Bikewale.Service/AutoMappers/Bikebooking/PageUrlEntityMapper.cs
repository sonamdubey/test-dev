﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Bikebooking
{
    /// <summary>
    /// 
    /// </summary>
    public class PageUrlEntityMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageUrlEntity"></param>
        /// <returns></returns>
        internal static DTO.BikeBooking.PagingUrl Convert(Entities.BikeBooking.PagingUrl PageUrlEntity)
        {
            Mapper.CreateMap<Entities.BikeBooking.PagingUrl, DTO.BikeBooking.PagingUrl>();
            return Mapper.Map<Entities.BikeBooking.PagingUrl, DTO.BikeBooking.PagingUrl>(PageUrlEntity);
        }
    }
}