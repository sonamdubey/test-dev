using AutoMapper;
using Bikewale.DTO.BikeBooking.Model;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Bikebooking.Model
{
    public class ModelEntityToDTO
    {
        internal static IEnumerable<DTO.BikeBooking.Model.BBModelBase> Convert(IEnumerable<Entities.BikeData.BikeModelEntityBase> objModelList)
        {
            Mapper.CreateMap<BikeModelEntityBase, BBModelBase>();
            return Mapper.Map<IEnumerable<BikeModelEntityBase>, IEnumerable<BBModelBase>>(objModelList);
        }
    }
}