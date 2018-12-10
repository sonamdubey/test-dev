using AutoMapper;
using Carwale.DTOs.CarData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Carwale.Utility;

namespace Carwale.Service.Mappers
{
    class CarColor
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<Carwale.Entity.CompareCars.Color, VersionColorDto>()
                .ForMember(x => x.HexCodes, o => o.MapFrom(s => new List<string>(s.Value.ConvertStringToList<string>())));
        }
    }

}
