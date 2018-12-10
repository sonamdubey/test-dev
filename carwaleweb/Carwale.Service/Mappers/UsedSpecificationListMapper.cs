using AutoMapper;
using Carwale.DTOs.Stock.Details;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified.CarDetails;

namespace Carwale.Service.Mappers
{
    public static class UsedSpecificationListMapper
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<CategoryItem, SpecificationListItems>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ItemValue, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.ItemUnit, opt => opt.MapFrom(src => src.UnitTypeName));
            Mapper.CreateMap<CarData, SpecificationList>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            Mapper.CreateMap<CarData, SpecificationApp>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName));
            Mapper.CreateMap<CategoryItem, SpecificationItemApp>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
            Mapper.CreateMap<CarData, FeatureApp>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName));
            Mapper.CreateMap<CategoryItem, FeatureItemApp>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => (IsFeatureMissing(src) ? $"No {src.Name}" : src.Name)))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => !IsFeatureMissing(src)));
        }

        private static bool IsFeatureMissing(CategoryItem src)
        {
            return (src.DataTypeId == 2 && src.Value.Equals("0")) //for dataType=2, 0 means missing
                || (src.DataTypeId != 2 && (string.IsNullOrWhiteSpace(src.Value) || src.Value.Equals("No", System.StringComparison.OrdinalIgnoreCase)));  //for other cases, empty string or No means missing
        }
    }
}
