using AutoMapper;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified.CarDetails;

namespace Carwale.Service.Mappers
{
    public static class UsedFeatureListMapper
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<CategoryItem, FeatureListItems>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ItemValue, opt => opt.MapFrom(src => src.Value));
            Mapper.CreateMap<CarData, FeatureList>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
