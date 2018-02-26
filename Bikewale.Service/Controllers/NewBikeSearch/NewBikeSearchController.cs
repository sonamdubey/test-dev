using Bikewale.DTO.NewBikeSearch;
using Bikewale.ElasticSearch.Entities;
using Bikewale.Entities.Location;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.NewBikeSearch;
using Bikewale.Service.Model.NewBikeSearch;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
namespace Bikewale.Service.Controllers.NewBikeSearch
{
    /// <summary>
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class NewBikeSearchController : CompressionApiController//ApiController
    {
        //private readonly ISearchQuery _searchQuery = null;
        private readonly ISearchResult _searchResult = null;
        private readonly IProcessFilter _processFilter = null;
        private readonly IBikeSearch _bikeSearch = null;

        public NewBikeSearchController(ISearchResult searchResult, IProcessFilter processFilter, IBikeSearch bikeSearch)
        {
            //_searchQuery = searchQuery;
            _searchResult = searchResult;
            _processFilter = processFilter;
            _bikeSearch = bikeSearch;
        }
        /// <summary>
        /// Modified By :- Subodh Jain 29 March 2017
        /// Summary :- if platform  is android then use dispalcement clause 8
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IHttpActionResult Get([FromUri]InputBaseEntity input)
        {
            try
            {
                SearchOutput searchResult;
                FilterInput filterInputs = _processFilter.ProcessFilters(input);

                SearchOutputEntity objSearchList = _searchResult.GetSearchResult(filterInputs, input);

                searchResult = SearchOutputMapper.Convert(objSearchList);
                if (objSearchList != null && objSearchList.SearchResult != null && objSearchList.SearchResult.Count > 0)
                {
                    return Ok(searchResult);
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.NewBikeSearch.NewBikeSearchController");

                return InternalServerError();
            }
        }

        public IHttpActionResult BikeList([FromBody]/*SearchFilterDTO*/SearchFilters input)
        {

            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            input.CityId = currentCityArea.CityId;

            IEnumerable<BikeModelDocument> objBikeList = null;
            IEnumerable<BikeModelDocument> objBikeListWithCityPrice = null;
            SearchOutput searchResult = null;

            if (input.CityId > 0)
            {
                var bikeList = Task.Factory.StartNew(() => objBikeList = _bikeSearch.GetBikeSearch(input, BikeSearchEnum.BikeList));
                var bikeListWithCityPrice = Task.Factory.StartNew(() => objBikeListWithCityPrice = _bikeSearch.GetBikeSearch(input, BikeSearchEnum.PriceList));
                Task.WaitAll(bikeList, bikeListWithCityPrice);

                BikeSearchModel objData = new BikeSearchModel();
                objData.GetData(objBikeList, objBikeListWithCityPrice);

            }
            else
            {
                objBikeList = _bikeSearch.GetBikeSearch(input, BikeSearchEnum.BikeList);
            }


            searchResult = SearchOutputMapper.Convert(objBikeList);
            return Ok(searchResult);
        }

        public IHttpActionResult BikeListOtherMake([FromBody]/*SearchFilterDTO*/SearchFilters input)
        {
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            input.CityId = currentCityArea.CityId;

            IEnumerable<BikeModelDocument> objBikeList = null;
            IEnumerable<BikeModelDocument> objBikeListWithCityPrice = null;
            SearchOutput searchResult = null;

            if (input.CityId > 0)
            {
                var bikeList = Task.Factory.StartNew(() => objBikeList = _bikeSearch.GetBikeSearch(input, BikeSearchEnum.BikeList));
                var bikeListWithCityPrice = Task.Factory.StartNew(() => objBikeListWithCityPrice = _bikeSearch.GetBikeSearch(input, BikeSearchEnum.PriceList));
                Task.WaitAll(bikeList, bikeListWithCityPrice);

                BikeSearchModel objData = new BikeSearchModel();
                objBikeList = objData.GetData(objBikeList, objBikeListWithCityPrice);

            }
            else
            {
                objBikeList = _bikeSearch.GetBikeSearch(input, BikeSearchEnum.BikeList);
            }

            searchResult = SearchOutputMapper.Convert(objBikeList);
            return Ok(searchResult);
        }

    }
}
