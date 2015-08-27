using AutoMapper;
using Bikewale.BAL.AutoComplete;
using Bikewale.DTO.AutoComplete;
using Bikewale.Interfaces.AutoComplete;
using Bikewale.Notifications;
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
    public class AutoSuggestController : ApiController
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 25 Aug 2015
        /// Summary : Auto Suggest Api for Make And Model
        /// </summary>
        /// <param name="inputText">String</param>
        /// <returns></returns>
        public HttpResponseMessage Get(string inputText, int? noOfRecords=null)
        {
            try
            {
                BikeList objBikes = new BikeList();
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IAutoSuggest, AutoSuggest>();
                    IAutoSuggest autoSuggest = container.Resolve<AutoSuggest>();

                    int noOfSuggestion = noOfRecords.HasValue ? noOfRecords.Value : 10;

                    List<SuggestOption> objSuggestion = autoSuggest.GetAutoSuggestResult(inputText,noOfSuggestion);

                    Mapper.CreateMap<SuggestOption, SuggestionList>();                    

                    objBikes.Bikes = Mapper.Map<List<SuggestOption>, List<SuggestionList>>(objSuggestion);  

                    if (objSuggestion != null && objSuggestion.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, objBikes);
                    else
                        return Request.CreateResponse(HttpStatusCode.NoContent, "no data present");
                }
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.AutoComplete.AutoSuggestController");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}