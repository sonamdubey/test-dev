using AutoMapper;
using Carwale.BL.Stock;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Search;
using Carwale.Entity.Stock;
using Carwale.Utility;

namespace Carwale.Service.Mappers
{
    public static class UsedSearchMappers
    {
        public static void CreateMaps()
        {

            Mapper.CreateMap<SearchParams, FilterInputs>()
                .ForMember(d => d.city, o => o.MapFrom(s => s.City <= 0 ? string.Empty : s.City.ToString()))
                .ForMember(d => d.Latitude, o => o.MapFrom(s => !RegExValidations.IsValidLatLong(s.Latitude, s.Longitude) ? 0.0 : s.Latitude))
                .ForMember(d => d.Longitude, o => o.MapFrom(s => !RegExValidations.IsValidLatLong(s.Latitude, s.Longitude) ? 0.0 : s.Longitude));

            Mapper.CreateMap<StockRecoParams, StockRecoParamsData>()
                .ForMember(pd => pd.MaxPrice, o => o.MapFrom(p => StockRecommendationsBL.GetMaxPriceForRecommendations(p.Price)))
                .ForMember(pd => pd.MinPrice, o => o.MapFrom(p => StockRecommendationsBL.GetMinPriceForRecommendations(p.Price)));
        }
    }
}
