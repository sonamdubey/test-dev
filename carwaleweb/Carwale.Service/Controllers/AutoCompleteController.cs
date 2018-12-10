using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web;
using System.Collections.Specialized;
using Carwale.Entity;
using Carwale.Service.Filters;
using Carwale.BL.AutoComplete;
using Carwale.DTOs;
using Carwale.Entity.AutoComplete;
using Carwale.Entity.Enum;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Collections;
using AutoMapper;
using System.Linq;
using Carwale.DTOs.Autocomplete;
using Carwale.Utility;
using Carwale.Interfaces.AutoComplete;
using Carwale.DTOs.Elastic;
using Carwale.DTOs.Elastic.Autocomplete.Area;
using System.Text.RegularExpressions;
using Carwale.DTOs.Suggestion;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers
{
    public class AutoCompleteController : ApiController
    {
        private readonly IAutoComplete_v1 _iautoComplete_v1;

        public AutoCompleteController(IAutoComplete_v1 iautoComplete_v1) 
        {
            _iautoComplete_v1 = iautoComplete_v1;
        }

        [AutocompleteQSFilter]
        //[Route("api/v1/autocomplete/suggest")]
        [HttpGet]
        public IHttpActionResult GetResults()
        {
            AutoComplete _autoComplete = new AutoComplete();

           var result = _autoComplete.GetResults(HttpUtility.ParseQueryString(Request.RequestUri.Query));

            return Ok(result);
        }


        [AutocompleteQSFilter]
        public IHttpActionResult GetTopResults() //android and ios users uses it
        {
            List<Suggest> carSuggestions = null;

            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            int sourceId; IEnumerable<string> headerValues;
            if (Request.Headers.TryGetValues("sourceID", out headerValues))
            { 
                int.TryParse(headerValues.First(), out sourceId);
                nvc["sourceIDHeader"] = sourceId.ToNullSafeString();
            }


            AutoComplete _autoComplete = new AutoComplete();
            string source = nvc["source"];

            if (source.Equals("1") || source.Equals("2"))
                carSuggestions = _autoComplete.GetSuggestObject<List<Suggest>>(nvc);
            else
            {
                var cities = _autoComplete.GetSuggestObject<List<LabelValueDTO>>(nvc);
                return Ok(cities);
            }
            return Ok(carSuggestions);
        }

        [HttpGet, AutocompleteQSFilter, Route("api/v1/autocomplete/areas/")]
        public IHttpActionResult GetAreaResults(int cityId)
        {
            AutoComplete autoComplete = new AutoComplete();
            var result = autoComplete.GetResults(HttpUtility.ParseQueryString(Request.RequestUri.Query));
            return Ok(result);
        }

        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082,http://localhost:8082,http://localhost:8081", headers: "*", methods: "GET")]
        [HttpGet, Route("api/v2/autocomplete/areas/")]
        public IHttpActionResult GetAreaSuggestions()
        {

            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            var areaSuggestion = _iautoComplete_v1.GetAreaSuggestion(nvc);

            return Ok(areaSuggestion);
        }

        [HttpGet, Route("api/v2/autocomplete/city/"), EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
        public IHttpActionResult GetCitySuggestions()
        {

            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            var citySuggestion = _iautoComplete_v1.GetCitySuggestion(nvc);

            return Ok(citySuggestion);
        }

        [HttpGet, Route("api/v1/autocomplete-amp/city/"), EnableCors("https://www-carwale-com.cdn.ampproject.org,https://cdn.ampproject.org,https://www-carwale-com.amp.cloudflare.com", "*","GET")]
        public IHttpActionResult GetAMPCitySuggestions()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            if (nvc["__amp_source_origin"] == null) return BadRequest();
            if(nvc["__amp_source_origin"].Contains("carwale.com") || nvc["__amp_source_origin"].Contains("localhost"))
            {
                HttpContextUtils.AddAmpHeaders(nvc["__amp_source_origin"], false);
                var citySuggestion = _iautoComplete_v1.GetCitySuggestion(nvc);

                if (citySuggestion == null)
                {
                    return InternalServerError();
                }

                var citySuggestionDto = Mapper.Map<List<Suggest>, List<AmpSuggest>>(citySuggestion);
                citySuggestionDto.ForEach(x => x.Url = nvc["url"]);

                return Ok(citySuggestionDto);
            }
            else
            {
                return BadRequest();
            }
        }

		[HttpGet, Route("api/v3/autocomplete/"), EnableCors("https://www-carwale-com.cdn.ampproject.org,https://cdn.ampproject.org,https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
		[AutocompleteQSFilter]
		[AmpFilter]
		public IHttpActionResult GetSearchResults([FromUri]SuggestionInput input)
        {
            List<SuggestionTypeEnum> types = input.source.Split(',').Select(a => (SuggestionTypeEnum)Enum.Parse(typeof(SuggestionTypeEnum), a)).Distinct().OrderBy(x => (int)x).ToList();
            int size = input.size > 0 ? input.size : 10;
            bool isAmp = input.IsAmp;
            //added new parameter isamp to support search on amp page
            var result = new List<Base>();
            input.value = Regex.Replace(HttpUtility.UrlDecode(input.value).Trim().Replace("-", " "), "[^/\\-0-9a-zA-Z\\s]*", "");
            result = _iautoComplete_v1.GetSearchSuggestion(input.value, types, size, isAmp);
            return Ok(result);
        }
    }
}
