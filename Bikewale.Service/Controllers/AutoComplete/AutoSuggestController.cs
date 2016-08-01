using Bikewale.DTO.AutoComplete;
using Bikewale.Entities.AutoComplete;
using Bikewale.Interfaces.AutoComplete;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.AutoComplete;
using Bikewale.Service.Utilities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
namespace Bikewale.Service.Controllers.AutoComplete
{
    /// <summary>
    /// Auto Suggest Controller
    /// Created By : Sadhana Upadhyay on 25 Aug 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class AutoSuggestController : CompressionApiController//ApiController
    {
        private readonly IAutoSuggest _autoSuggest = null;
        public AutoSuggestController(IAutoSuggest autoSuggest)
        {
            _autoSuggest = autoSuggest;
        }
        /// <summary>        
        /// Summary : Auto Suggest Api for Make And Model
        /// Modified by :   Sumit Kate on 06 May 2016
        /// Description :   For APP, Handle the city name if it contains state name
        /// Modified by :   Sangram Nandkhile on 20 May 2016
        /// Description :   For APP version 11 and +, send state name along with city name
        /// </summary>
        /// <param name="inputText">user entered input in search box</param>
        /// <returns></returns>
        public IHttpActionResult Get(string inputText, AutoSuggestEnum source, int? noOfRecords = null)
        {
            string platformId = string.Empty;
            uint appVersion = 0;
            CityPayload city = null;
            try
            {
                BikeList objBikes = new BikeList();
                int noOfSuggestion = noOfRecords.HasValue ? noOfRecords.Value : 10;

                IEnumerable<SuggestOption> objSuggestion = _autoSuggest.GetAutoSuggestResult(HttpContext.Current.Server.UrlDecode(inputText), noOfSuggestion, source);

                objBikes.Bikes = SuggestionListMapper.Convert(objSuggestion);

                objSuggestion = null;

                if (objBikes != null && objBikes.Bikes != null)
                {
                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }
                    if (Request.Headers.Contains("version_code"))
                    {
                        string t_appVersion = Request.Headers.GetValues("version_code").First().ToString();
                        if (!string.IsNullOrEmpty(t_appVersion))
                        {
                            appVersion = Convert.ToUInt32(t_appVersion);
                        }
                    }

                    if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4") && source == AutoSuggestEnum.AllCity && appVersion < 12)
                    {
                        foreach (var item in objBikes.Bikes)
                        {
                            city = Newtonsoft.Json.JsonConvert.DeserializeObject<CityPayload>(item.Payload.ToString());
                            if (city != null && !city.isDuplicate)
                            {
                                item.Text = item.Text.Split(',')[0].Trim();
                            }
                        }
                    }
                    return Ok(objBikes);
                }
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

        /// <summary>
        /// Created By : Sajal Gupta
        /// Created On : 01/08/2016
        /// Description : Optimised version by sending only makeId and modelId in the payload.
        /// </summary>
        [Route("api/v2/autosuggest/")]
        public IHttpActionResult GetV2(string inputText, AutoSuggestEnum source, int? noOfRecords = null)
        {
            string platformId = string.Empty;
            uint appVersion = 0;
            CityPayload city = null;

            try
            {
                

                BikeList objBikes = new BikeList();
                int noOfSuggestion = noOfRecords.HasValue ? noOfRecords.Value : 10;

                IEnumerable<SuggestOption> objSuggestion = _autoSuggest.GetAutoSuggestResult(HttpContext.Current.Server.UrlDecode(inputText), noOfSuggestion, source);

                objBikes.Bikes = SuggestionListMapper.Convert(objSuggestion);

                objSuggestion = null;

                if (objBikes != null && objBikes.Bikes != null)
                {
                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }
                    if (Request.Headers.Contains("version_code"))
                    {
                        string t_appVersion = Request.Headers.GetValues("version_code").First().ToString();
                        if (!string.IsNullOrEmpty(t_appVersion))
                        {
                            appVersion = Convert.ToUInt32(t_appVersion);
                        }
                    }

                    if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4") && source == AutoSuggestEnum.AllCity && appVersion < 12)
                    {
                        foreach (var item in objBikes.Bikes)
                        {
                            city = Newtonsoft.Json.JsonConvert.DeserializeObject<CityPayload>(item.Payload.ToString());
                            if (city != null && !city.isDuplicate)
                            {
                                item.Text = item.Text.Split(',')[0].Trim();
                            }
                        }
                    }
  
                    Bikewale.DTO.AutoComplete.V2.Payload obj = new Bikewale.DTO.AutoComplete.V2.Payload();
                    Bikewale.DTO.AutoComplete.V2.BikeList objBikesV2 = new Bikewale.DTO.AutoComplete.V2.BikeList();
                    objBikesV2.Bikes = new List<Bikewale.DTO.AutoComplete.V2.SuggestionList>();

                    foreach (var item in objBikes.Bikes)
                    {
                        Bikewale.DTO.AutoComplete.V2.SuggestionList bike = new Bikewale.DTO.AutoComplete.V2.SuggestionList();
                        bike.Payload = Newtonsoft.Json.JsonConvert.DeserializeObject<Bikewale.DTO.AutoComplete.V2.Payload>(item.Payload.ToString());
                        bike.Text = item.Text;
                        objBikesV2.Bikes.Add(bike);
                    }

                    return Ok(objBikesV2);
                }
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