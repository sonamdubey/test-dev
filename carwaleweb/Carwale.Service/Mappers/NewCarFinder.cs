using Carwale.DTOs.NewCarFinder;
using Carwale.DTOs.CarData;
using Carwale.Entity.NewCarFinder;
using Carwale.Entity.CarData;
using AutoMapper;
using Carwale.Entity.Classification;
using Carwale.DTOs.Search.Model;

namespace Carwale.Service.Mappers
{
    static class NewCarFinder
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<NewCarFinderBudget, NewCarFinderBudgetDTO>();
            Mapper.CreateMap<NewCarFinderBudget, BudgetBaseDTO>();
            Mapper.CreateMap<EmiBase, EmiBaseDTO>();
            Mapper.CreateMap<CarMakeEntityBase, CarMakesDTO>();
            Mapper.CreateMap<BodyType, BodyTypeBaseDTO>();
            Mapper.CreateMap<FuelTypes, FuelTypeBaseDTO>();
            Mapper.CreateMap<TransmissionTypeBase, TransmissionBaseDTO>();
            Mapper.CreateMap<Entity.NewCarFinder.MakeFilter, MakeFilterDto>();
            Mapper.CreateMap<NcfMake, NcfMakeDto>();
            Mapper.CreateMap<NcfScreen, NcfScreenDto>();
        }
    }
}
