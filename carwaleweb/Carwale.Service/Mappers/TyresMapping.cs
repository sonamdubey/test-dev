using AutoMapper;
using Carwale.DTOs.Accessories.Tyres;
using Carwale.Entity.Accessories.Tyres;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Service.Mappers
{
    public static class TyresMapping
    {
        public static void CreateMap()
        {
            Mapper.CreateMap<TyreSummary, TyreSummaryDTO>();
            Mapper.CreateMap<TyreList, TyreListDTO>();
            Mapper.CreateMap<VersionTyres, VersionTyresDTO>();
            Mapper.CreateMap<CarVersionEntity, IdName>()
                .ForMember(x => x.Id, o => o.MapFrom(s => s.ID))
                .ForMember(x => x.Name, o => o.MapFrom(s => s.Name));
            Mapper.CreateMap<CarVersionDetails, VersionTyresList>();
        }
    }
}
