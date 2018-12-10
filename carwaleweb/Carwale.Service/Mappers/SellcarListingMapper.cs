using AutoMapper;
using Carwale.DTOs.Classified.SellCar;
using Carwale.Entity.Classified.SellCarUsed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Service.Mappers
{
    public static class SellcarListingMapper
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<UpdateListing, SellCarInfo>();
        }
    }
}
