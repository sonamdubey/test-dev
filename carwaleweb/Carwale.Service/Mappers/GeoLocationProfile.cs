using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Elastic.Autocomplete.Area;
using Carwale.DTOs.Geolocation;
using Carwale.Entity.Geolocation;
using Carwale.Utility;

namespace Carwale.Service.Mappers
{
    public class GeoLocationProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<City, CustLocationDTO>();
            CreateMap<CustLocation, CustLocationDTO>();
            CreateMap<City, DTOs.City>();
            CreateMap<States, State>();
            CreateMap<Entity.Geolocation.StateAndAllCities, DTOs.Geolocation.StateAndAllCities>();
            CreateMap<City, CityDTO>().ForMember(x => x.Id, o => o.MapFrom(s => s.CityId))
               .ForMember(x => x.Name, o => o.MapFrom(s => s.CityName));
            CreateMap<Entity.Geolocation.Zone, ZoneDTO>().ForMember(x => x.Id, o => o.MapFrom(s => s.ZoneId))
                .ForMember(x => x.Name, o => o.MapFrom(s => s.ZoneName));
            CreateMap<PQGroupCity, PQGroupCityDTO>();
            CreateMap<Entity.Geolocation.Zone, DTOs.Geolocation.Zone>();
            CreateMap<CityZones, CityZonesDTO>();
            CreateMap<Carwale.Entity.Geolocation.City, PriceQuoteCityDTO>();
            CreateMap<CityZonesV2, CityZonesDTOV2>().ForMember(x => x.Id, o => o.MapFrom(s => s.CityId))
                .ForMember(x => x.Name, o => o.MapFrom(s => s.CityName))
                .ForMember(x => x.Zones, o => o.MapFrom(s => s.Zones));
            CreateMap<PQGroupCityV2, PQGroupCityDTOV2>();
            CreateMap<City, CitiesDTO>();
            CreateMap<GroupCity, GroupCityDTO>();
            CreateMap<City, CityZonesDTOV2>().ForMember(x => x.Id, o => o.MapFrom(s => s.CityId))
                 .ForMember(x => x.Name, o => o.MapFrom(s => s.CityName));
            CreateMap<CustLocation, PQCustLocationDTO>()
                .ForMember(d => d.ZoneId, o => o.MapFrom(s => CustomParser.parseIntObject(s.ZoneId)));
            CreateMap<LocationV2, PQCustLocationDTO>()
                .ForMember(d => d.ZoneId, o => o.MapFrom(s => CustomParser.parseIntObject(s.ZoneId)));
            CreateMap<City, Cities>();
            CreateMap<Cities, CitiesDTO>();
            CreateMap<City, CitiesDTO>();
            CreateMap<City, CityBaseDTO>().ForMember(x => x.Id, o => o.MapFrom(s => s.CityId))
                 .ForMember(x => x.Name, o => o.MapFrom(s => s.CityName));
            CreateMap<Area, Location>()
                .ForMember(x => x.AreaName, o => o.MapFrom(s => s.name));

            CreateMap<Area, LocationV2>()
                .ForMember(x => x.AreaName, o => o.MapFrom(s => s.name));
            CreateMap<CustLocation, LocationV2>().ReverseMap();

            CreateMap<CustLocation, Location>().ReverseMap();
            CreateMap<PuneThaneZones, PuneThaneZonesDTO>();
            CreateMap<City, CityDTO>()
               .ForMember(d => d.Id, o => o.MapFrom(s => s.CityId))
               .ForMember(d => d.Name, o => o.MapFrom(s => s.CityName));
            CreateMap<AreaPayLoad, Area>()
                .ForMember(d => d.areaid, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.zoneid, o => o.MapFrom(s => s.ZoneId))
                .ForMember(d => d.zonename, o => o.MapFrom(s => s.ZoneName))
                .ForMember(d => d.cityid, o => o.MapFrom(s => s.CityId))
                .ForMember(d => d.cityname, o => o.MapFrom(s => s.CityName))
                .ForMember(d => d.pincode, o => o.MapFrom(s => s.PinCode));
            CreateMap<Location, CityAreaDTO>();
            CreateMap<Cities, CityAreaDTO>();
            CreateMap<Location, PQCustLocationDTO>();
            CreateMap<PQCustLocationDTO, CityAreaDTO>();
            CreateMap<AreaPayLoad, LocationsDTO>()
                .ForMember(d => d.AreaId, o => o.MapFrom(s => s.Id > 0 ? s.Id : -1))
                .ForMember(d => d.AreaName, o => o.MapFrom(s => s.Name));
            CreateMap<DTOs.CityResultsDTO, LocationsDTO>()
                .ForMember(d => d.AreaId, o => o.UseValue(-1))
                .ForMember(d => d.CityId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CityName, o => o.MapFrom(s => s.Name));
        }
    }
}
