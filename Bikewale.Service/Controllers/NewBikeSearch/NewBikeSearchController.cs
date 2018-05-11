using Bikewale.DTO.NewBikeSearch;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.NewBikeSearch;
using Bikewale.Service.Utilities;
using System;
using System.Web.Http;
namespace Bikewale.Service.Controllers.NewBikeSearch
{
    /// <summary>
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class NewBikeSearchController : CompressionApiController//ApiController
    {
        private readonly IBikeSearchResult _searchResult = null;
        private readonly IProcessFilter _processFilter = null;
        private readonly IBikeSearch _bikeSearch = null;

        public NewBikeSearchController(IBikeSearchResult searchResult, IProcessFilter processFilter, IBikeSearch bikeSearch)
        {
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
                searchResult = _searchResult.GetSearchResult(filterInputs, input);
                if (searchResult != null && searchResult.SearchResult != null && searchResult.SearchResult.Count > 0)
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

        [Route("api/v2/bikesearch/")]
        public IHttpActionResult BikeList([FromBody]SearchFilterDTO input)
        {
            try
            {
                BikeSearchOutputEntity objBikeList = null;
                if (input != null)
                {
                    SearchFilters Filters = SearchOutputMapper.Convert(input);

                    if (Filters != null)
                    {
                        objBikeList = _bikeSearch.GetBikeSearch(Filters);
                    }


                    if (objBikeList != null)
                    {
                        return Ok(objBikeList);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.NewBikeSearch.BikeList");

                return InternalServerError();
            }
        }
    }
}
