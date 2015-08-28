using AutoMapper;
using Bikewale.DTO.State;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.State
{
    public class StateEntityToDTO
    {
        internal static IEnumerable<DTO.State.StateBase> ConvertStateEntityBase(List<Entities.Location.StateEntityBase> objStateList)
        {
            Mapper.CreateMap<StateEntityBase, StateBase>();
            return Mapper.Map<List<StateEntityBase>, List<StateBase>>(objStateList);
        }
    }
}