using AutoMapper;
using Bikewale.DTO.State;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.State
{
    public class StateListMapper
    {
        internal static IEnumerable<DTO.State.StateBase> Convert(List<Entities.Location.StateEntityBase> objStateList)
        {
          return Mapper.Map<List<StateEntityBase>, List<StateBase>>(objStateList);
        }
    }
}