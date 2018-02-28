using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.NewBikeSearch;
using Bikewale.DTO.Series;
using Bikewale.ElasticSearch.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.NewBikeSearch;
using System.Collections.Generic;
namespace Bikewale.Service.AutoMappers.NewBikeSearch
{
    public class SearchOutputMapper
    {
        internal static SearchOutput Convert(SearchOutputEntity objSuggestion)
        {
            Mapper.CreateMap<SearchOutputEntity, SearchOutput>();
            Mapper.CreateMap<PagingUrl, Pager>();
            Mapper.CreateMap<SearchOutputEntityBase, SearchOutputBase>();
            Mapper.CreateMap<BikeModelEntity, ModelDetail>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();

            return Mapper.Map<SearchOutputEntity, SearchOutput>(objSuggestion);
        }

        internal static SearchOutput Convert(IEnumerable<BikeModelDocument> obj)
        {
            SearchOutput objData = new SearchOutput();

            Mapper.CreateMap<BikeModelDocument, SearchOutputBase>().ForMember(d => d.BikeModel, opt => opt.MapFrom(s => s.BikeModel));
            Mapper.CreateMap<BikeModelDocument, SearchOutputBase>().ForMember(d => d.BikeModel.MakeBase, opt => opt.MapFrom(s => s.BikeMake));

            Mapper.CreateMap<BikeModelDocument, SearchOutputBase>().ForMember(d => d.Displacement, opt => opt.MapFrom(s => s.TopVersion.Displacement));
            Mapper.CreateMap<BikeModelDocument, SearchOutputBase>().ForMember(d => d.KerbWeight, opt => opt.MapFrom(s => s.TopVersion.KerbWeight));
            Mapper.CreateMap<BikeModelDocument, SearchOutputBase>().ForMember(d => d.Power, opt => opt.MapFrom(s => s.TopVersion.Power));
            Mapper.CreateMap<BikeModelDocument, SearchOutputBase>().ForMember(d => d.FuelEfficiency, opt => opt.MapFrom(s => s.TopVersion.Mileage));
            Mapper.CreateMap<BikeModelDocument, SearchOutputBase>().ForMember(d => d.FinalPrice, opt => opt.MapFrom(s => s.TopVersion.Exshowroom));
            Mapper.CreateMap<BikeModelDocument, SearchOutputBase>().ForMember(d => d.BikeModel.ReviewCount, opt => opt.MapFrom(s => s.UserReviewsCount));
            Mapper.CreateMap<BikeModelDocument, SearchOutputBase>().ForMember(d => d.BikeModel.RatingCount, opt => opt.MapFrom(s => s.RatingsCount));
            Mapper.CreateMap<BikeModelDocument, SearchOutputBase>().ForMember(d => d.BikeModel.ReviewRate, opt => opt.MapFrom(s => s.ReviewRatings));
            Mapper.CreateMap<BikeModelDocument, SearchOutputBase>().ForMember(d => d.BikeModel.ReviewRate, opt => opt.MapFrom(s => s.ReviewRatings));



            objData.SearchResult = Mapper.Map<IEnumerable<BikeModelDocument>, List<SearchOutputBase>>(obj);
            return objData;

        }

        internal static SearchFilters Convert(SearchFilterDTO input)
        {

            Mapper.CreateMap<SearchFilterDTO, SearchFilters>();
            return Mapper.Map<SearchFilterDTO, SearchFilters>(input);



        }
    }
}