using AutoMapper;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CompareCars;
using Carwale.Entity.ViewModels.CarData;

namespace Carwale.Service.Mappers
{
    public static class CarDataMicroserviceMapper
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<VehicleData.Service.ProtoClass.ValueData, ItemValueData>();

            Mapper.CreateMap<CarData, SubCategory>()
               .ForMember(x => x.Name, o => o.MapFrom(y => y.CategoryName));

            Mapper.CreateMap<CarData, Carwale.DTOs.CarData.SubCategory>()
               .ForMember(x => x.Name, o => o.MapFrom(y => y.CategoryName));

            Mapper.CreateMap<CategoryItem, Item>()
                .ForMember(x => x.ItemMasterId, o => o.MapFrom(y => y.Id))
                .ForMember(x => x.ItemValue, o => o.MapFrom(y => y.Value))
                .ForMember(x => x.UnitType, o => o.MapFrom(y => y.UnitTypeName));

            Mapper.CreateMap<CategoryItem, Carwale.DTOs.CarData.Item>()
               .ForMember(x => x.ItemMasterId, o => o.MapFrom(y => y.Id))
               .ForMember(x => x.UnitType, o => o.MapFrom(y => y.UnitTypeName));

            Mapper.CreateMap<CarDataPresentation, CCarData>()
                .ForMember(x => x.Specs, o => o.MapFrom(y => y.Specifications))
                .ForMember(x => x.Features, o => o.MapFrom(y => y.Features))
                .ForMember(x => x.OverView, o => o.MapFrom(y => y.Overview));

            Mapper.CreateMap<CarDataPresentation, DTOs.CarData.CCarDataDto>()
                .ForMember(x => x.Specs, o => o.MapFrom(y => y.Specifications))
                .ForMember(x => x.Features, o => o.MapFrom(y => y.Features))
                .ForMember(x => x.OverView, o => o.MapFrom(y => y.Overview));

            Mapper.CreateMap<Color, DTOs.CarData.Color>();

            Mapper.CreateMap<VehicleData.Service.ProtoClass.SpecsSummary, CarModelSpecs>()
                .ForMember(x => x.ItemValue, o => o.MapFrom(s => s.Value))
                .ForMember(x => x.Item, o => o.MapFrom(s => s.ItemName));

            Mapper.CreateMap<VehicleData.Service.ProtoClass.VehicleDataValue, CarDataPresentation>()
                .ForMember(x => x.Specifications, o => o.MapFrom(s => s.Specifications))
                .ForMember(x => x.Features, o => o.MapFrom(s => s.Features))
                .ForMember(x => x.Overview, o => o.MapFrom(s => s.Overview));

            Mapper.CreateMap<VehicleData.Service.ProtoClass.Item, CategoryItem>()
               .ForMember(x => x.Id, o => o.MapFrom(s => s.Id))
               .ForMember(x => x.SortOrder, o => o.MapFrom(s => s.SortOrder))
               .ForMember(x => x.Name, o => o.MapFrom(s => s.Name))
               .ForMember(x => x.UnitTypeName, o => o.MapFrom(s => s.UnitTypeName))
               .ForMember(x => x.Value, o => o.MapFrom(s => s.ItemValue));

            Mapper.CreateMap<VehicleData.Service.ProtoClass.Category, CarData>()
                .ForMember(x => x.CategoryName, o => o.MapFrom(s => s.Name))
                .ForMember(x => x.SortOrder, o => o.MapFrom(s => s.PriorityOrder))
                .ForMember(x => x.Items, o => o.MapFrom(s => s.Items));

            Mapper.CreateMap<CarVersionDetails, CarWithImageEntity>()
                 .ForMember(x => x.VersionMaskingName, o => o.MapFrom(s => s.VersionMasking))
                 .ForMember(x => x.IsNew, o => o.MapFrom(s => s.New))
                 .ForMember(x => x.IsFuturistic, o => o.MapFrom(s => s.Futuristic));
            Mapper.CreateMap<CarDataPresentation, ComparisonData>();
            Mapper.CreateMap<CarDataPresentation, Carwale.DTOs.CarData.ComparisonDataDto>()
                .ForMember(x => x.Specs, o => o.MapFrom(s => s.Specifications))
                .ForMember(x => x.Features, o => o.MapFrom(s => s.Features))
                .ForMember(x => x.Overview, o => o.MapFrom(s => s.Overview));
            Mapper.CreateMap<VehicleData.Service.ProtoClass.ItemOldApp, CategoryItem>()
              .ForMember(x => x.Id, o => o.MapFrom(s => s.Id))
              .ForMember(x => x.SortOrder, o => o.MapFrom(s => s.SortOrder))
              .ForMember(x => x.Name, o => o.MapFrom(s => s.Name))
              .ForMember(x => x.UnitTypeName, o => o.MapFrom(s => s.UnitTypeName))
              .ForMember(x => x.Values, o => o.MapFrom(s => s.Values));
            Mapper.CreateMap<VehicleData.Service.ProtoClass.CategoryOldApp, Entity.CarData.CarData>()
                .ForMember(x => x.CategoryName, o => o.MapFrom(s => s.Name))
                .ForMember(x => x.SortOrder, o => o.MapFrom(s => s.PriorityOrder))
                .ForMember(x => x.Items, o => o.MapFrom(s => s.Items));
            Mapper.CreateMap<VehicleData.Service.ProtoClass.ModelFeature, ModelFeatures>();
            Mapper.CreateMap<VehicleData.Service.ProtoClass.ModelSpecsSummary, ModelDataSummary>();
            Mapper.CreateMap<VehicleData.Service.ProtoClass.SpecsInfo, SpecsInfo>();
            Mapper.CreateMap<SpecsImageDetailRequest,VehicleData.Service.ProtoClass.SpecsInfoRequest>();
        }
    }
}
