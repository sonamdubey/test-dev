using Bikewale.Entities.NewBikeSearch;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.DTO.NewBikeSearch;
using Bikewale.Service.AutoMappers.NewBikeSearch;

namespace Bikewale.Service.Controllers.NewBikeSearch
{
    public class NewBikeSearchController : ApiController
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
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.NewBikeSearch.NewBikeSearchController");
                objErr.SendMail();
                return InternalServerError();
            }
        }

    }
}
