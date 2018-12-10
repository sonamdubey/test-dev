using Carwale.DTOs.NewCarFinder;
using Microsoft.Practices.Unity;
using System;
using System.Web.Http;
using Carwale.Interfaces.NewCarFinder;
using Carwale.Entity.NewCarFinder;
using AutoMapper;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using Carwale.BL.Elastic.NewCarSearch;
using System.Web;
using Carwale.Interfaces.Elastic;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.Service.Controllers.NewCarFinder
{
    public class NewCarFinderController : ApiController
    {
        private readonly INewCarSearchAppAdapter _adapter;
        private readonly INewCarFinderCacheRepository _newCarFinderCacheRepository;
        public NewCarFinderController(INewCarSearchAppAdapter adapter, INewCarFinderCacheRepository newCarFinderCacheRepository)
        {
            _adapter = adapter;
            _newCarFinderCacheRepository = newCarFinderCacheRepository;
        }

        [HttpGet, Route("api/newcarfinder/budget/")]
        public IHttpActionResult Get(int cityId)
        {
            NewCarFinderBudgetDTO newCarFinderBudgetDTO = null;
            NewCarFinderBudget newCarFinderBudget = null;
            try
            {
                if (cityId <= 0)
                {
                    cityId = -1;
                }
                newCarFinderBudget = _newCarFinderCacheRepository.GetBudgetDetails(cityId);

                if (newCarFinderBudget == null)
                {
                    return NotFound();
                }

                newCarFinderBudgetDTO = Mapper.Map<NewCarFinderBudget, NewCarFinderBudgetDTO>(newCarFinderBudget);
                return Ok(newCarFinderBudgetDTO);

            }
            catch (Exception ex)
            {

                Logger.LogException(ex);
                return InternalServerError();

            }


        }
        [BodyTypesQSFilter]
        [HttpGet, Route("api/newcarfinder/bodytype/")]
        public IHttpActionResult GetBodyTypes()
        {
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
                var bodyTypesDTO = _adapter.GetBodyTypes(nvc);
                if (bodyTypesDTO == null)
                {
                    return NotFound();
                }
                return Ok(bodyTypesDTO);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }
        [FuelTypesQSFilter]
        [HttpGet, Route("api/newcarfinder/fueltype/")]
        public IHttpActionResult GetFuelTypes()
        {
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
                var fuelTypesDTO = _adapter.GetFuelTypes(nvc);
                return Ok(fuelTypesDTO);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is NullReferenceException)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [NewCarFinderQSFilter]
        [HttpGet, Route("api/newcarfinder/search/")]
        public IHttpActionResult GetModels()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
            try
            {
                var DTO = _adapter.GetNCFModels(nvc);
                var pageNo = nvc["pageNo"];
                var pageSize = nvc["pageSize"];

                if (pageNo != null && pageSize != null && DTO.TotalModels > Convert.ToInt32(nvc["pageSize"]) * Convert.ToInt32(pageNo))
                {
                    string domainName = string.Format("{0}{1}{2}", this.Request.RequestUri.Scheme, Uri.SchemeDelimiter, this.Request.RequestUri.Host);
                    nvc["pageNo"] = (Convert.ToInt32(pageNo) + 1).ToString();
                    DTO.NextPageUrl = string.Format("{0}/api/newcarfinder/search/?{1}", domainName, HttpUtility.UrlDecode(Convert.ToString(nvc)));
                }
                return Ok(DTO);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "NewCarFinderController.GetModels()");
            }
            return InternalServerError();
        }
        [HttpGet, Route("api/filters/all")]
        public IHttpActionResult GetAllFilters()
        {
            try
            {
                int sourceID = 0;
                IEnumerable<string> headerValues;
                if (Request.Headers.TryGetValues("sourceID", out headerValues))
                {
                    int.TryParse(headerValues.First(), out sourceID);
                }

                var allFiltersDto = _adapter.GetAllFilters(sourceID);
                if (allFiltersDto == null)
                {
                    return NotFound();
                }

                return Ok(allFiltersDto);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }
        [NewCarFinderQSFilter]
        [HttpGet, Route("api/newcarfinder/filters/make/")]
        public IHttpActionResult GetMakes()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
            try
            {
                if (nvc != null)
                {
                    var list = Mapper.Map<MakeFilter, Carwale.DTOs.Search.Model.MakeFilterDto>(_adapter.GetMakeList(nvc));
                    return Ok(list);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "NewCarFinderController.GetModels()");
                return BadRequest();
            }
        }

        [HttpGet, Route("api/newcarfinder/screen/")]
        public IHttpActionResult GetScreens()
        {
            try
            {
                NcfScreensDto ncfScreenList = new NcfScreensDto();
                ncfScreenList.Screens = Mapper.Map<List<NcfScreenDto>>(_newCarFinderCacheRepository.GetScreens());
                return Ok(ncfScreenList);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "NewCarFinderController.GetScreens()");
                return InternalServerError();
            }
        }

    }
}
