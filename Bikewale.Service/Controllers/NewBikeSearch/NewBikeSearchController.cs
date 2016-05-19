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
        //private readonly ISearchQuery _searchQuery = null;
        private readonly ISearchResult _searchResult = null;
        private readonly IProcessFilter _processFilter = null;

        public NewBikeSearchController(ISearchResult searchResult, IProcessFilter processFilter)
        {
            //_searchQuery = searchQuery;
            _searchResult = searchResult;
            _processFilter = processFilter;
        }

        public IHttpActionResult Get([FromUri]InputBaseEntity input)
        {
            try
            {
                SearchOutput searchResult = new SearchOutput();
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.NewBikeSearch.NewBikeSearchController");
                objErr.SendMail();
                return InternalServerError();
            }
        }

    }
}
