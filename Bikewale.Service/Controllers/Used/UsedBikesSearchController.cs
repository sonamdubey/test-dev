using Bikewale.Entities.Used.Search;
using Bikewale.Interfaces.Used.Search;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Used;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Used
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 11 Sept 2016
    /// Summary : Controller to get the used bikes search result
    /// </summary>
    public class UsedBikesSearchController : ApiController
    {
        private readonly ISearchFilters _searchFilters = null;
        private readonly ISearchQuery _searchQuery = null;
        private readonly ISearchRepository _searchRepo = null;
        private readonly ISearch _search = null;


        public UsedBikesSearchController(ISearchFilters searchFilters, ISearchQuery searchQuery, ISearchRepository searchRepo, ISearch search)
        {
            _searchFilters = searchFilters;
            _searchQuery = searchQuery;
            _searchRepo = searchRepo;
            _search = search;
        }

        [ResponseType(typeof(Bikewale.DTO.Used.Search.SearchResult)), Route("api/used/search/")]
        public IHttpActionResult Get([FromUri]InputFilters inputFilters)
        {
            try
            {
                Bikewale.Entities.Used.Search.SearchResult objSearchList = _search.GetUsedBikesList(inputFilters);

                Bikewale.DTO.Used.Search.SearchResult searchResult = UsedBikeSearchResult.Convert(objSearchList);

                if (searchResult != null && searchResult.Result != null && searchResult.Result.Count() > 0)
                {
                    return Ok(searchResult);
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.Used.UsedBikesSearchController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }



    }   // class
}   // namespace
