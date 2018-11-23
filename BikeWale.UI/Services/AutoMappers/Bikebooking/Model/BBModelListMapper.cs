using AutoMapper;
using Bikewale.DTO.BikeBooking.Model;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Bikebooking.Model
{
    public class BBModelListMapper
    {
        internal static IEnumerable<DTO.BikeBooking.Model.BBModelBase> Convert(IEnumerable<Entities.BikeData.BikeModelEntityBase> objModelList)
        {
            return Mapper.Map<IEnumerable<BikeModelEntityBase>, IEnumerable<BBModelBase>>(objModelList);
        }
    }
}