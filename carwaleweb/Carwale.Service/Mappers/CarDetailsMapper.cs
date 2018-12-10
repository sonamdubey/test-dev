using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Classified.CarDetails;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified.CarDetails;

namespace Carwale.Service.Mappers
{
    public static class CarDetailsMapper
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<Finance, FinanceWeb>();
            Mapper.CreateMap<CarDetailsEntity, CarDetailsWeb>();
            Mapper.CreateMap<CarIdEntity, CarIdDTO>();
            Mapper.CreateMap<CarWithImageEntity, CarOverviewDTOV2>();
            Mapper.CreateMap<CarIdEntity, CarOverviewDTOV2>(); 
        }
    }
}
