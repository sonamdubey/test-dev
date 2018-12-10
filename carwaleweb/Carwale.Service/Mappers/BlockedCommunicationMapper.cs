using AutoMapper;
using Carwale.DTOs.Blocking;
using Carwale.Entity.Blocking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Service.Mappers
{
    public static class BlockedCommunicationMapper
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<BlockedCommunicationDto, BlockedCommunication>();
            Mapper.CreateMap<BlockedCommunication, BlockedCommunicationDto>()
                .ForMember(x => x.Reason, opt => opt.Ignore())
                .ForMember(x => x.ActionBy, opt => opt.Ignore());
        }
    }
}
