using AutoMapper;
using Bikewale.BAL.AutoComplete;
using Bikewale.DTO.AutoComplete;
using Bikewale.Entities.AutoComplete;
using Bikewale.Interfaces.AutoComplete;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.AutoComplete;
using Microsoft.Practices.Unity;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Bikewale.Service.Controllers.AutoComplete
{
    /// <summary>
    /// Auto Suggest Controller
    /// Created By : Sadhana Upadhyay on 25 Aug 2015
    /// </summary>
    public class AutoSuggestController : ApiController
    {
        private readonly IAutoSuggest _autoSuggest = null;
        public AutoSuggestController(IAutoSuggest autoSuggest)
        {
            _autoSuggest = autoSuggest;
        }
        /// <summary>        
        /// Summary : Auto Suggest Api for Make And Model
        /// </summary>
        /// <param name="inputText">String</param>
        /// <returns></returns>
        public IHttpActionResult Get(string inputText, AutoSuggestEnum source, int? noOfRecords = null)
        {
            try
            {
                BikeList objBikes = new BikeList();
                int noOfSuggestion = noOfRecords.HasValue ? noOfRecords.Value : 10;

                List<SuggestOption> objSuggestion = _autoSuggest.GetAutoSuggestResult(HttpContext.Current.Server.UrlDecode(inputText), noOfSuggestion, source);

                objBikes.Bikes = SuggestionListMapper.Convert(objSuggestion);
                if (objSuggestion != null && objSuggestion.Count > 0)
                    return Ok(objBikes);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.AutoComplete.AutoSuggestController");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}