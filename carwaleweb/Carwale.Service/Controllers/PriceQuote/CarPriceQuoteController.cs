using System;
using System.Web.Http;
using Carwale.Entity.PriceQuote;
using System.Net.Http;
using System.Net;
using Carwale.Notifications;
using System.Web;
using Carwale.DTOs.PriceQuote;
using Carwale.Interfaces.PriceQuote;
using Carwale.Service.Filters;
using System.Web.Http.Cors;
using log4net;
using Carwale.Interfaces.Prices;
using Carwale.Notifications.Logs;
using Carwale.Interfaces.Validations;
using Carwale.Service.Adapters.Validations;
using Carwale.Utility;
using AutoMapper;

namespace Carwale.Service.Controllers.PriceQuote
{
    public class CarPriceQuoteController : ApiController
    {
        private readonly ICarPriceQuoteAdapter _iPQAdapter;
        private readonly IPriceAdapter _iPriceAdapter;
        private readonly IValidateMmv _validateMMV;
        private readonly IValidateLocation _validateLocation;
        private readonly IModelSimilarPriceDetailsBL _modelSimilarPriceDetailBL;
        private readonly IModelSimilarPriceDetailsRepo _modelSimilarPriceDetailRepo;

        private const string origins = "*";

        private static ILog _logger = LogManager.GetLogger(typeof(CarPriceQuoteController));

        public CarPriceQuoteController(ICarPriceQuoteAdapter iPQAdapter, IPriceAdapter iPriceAdapter, IValidateMmv validateMMV,
            IValidateLocation validateLocation, IModelSimilarPriceDetailsBL modelSimilarPriceDetailBL, IModelSimilarPriceDetailsRepo modelSimilarPriceDetailRepo)
        {
            _iPQAdapter = iPQAdapter;
            _iPriceAdapter = iPriceAdapter;
            _validateMMV = validateMMV;
            _validateLocation = validateLocation;
            _modelSimilarPriceDetailBL = modelSimilarPriceDetailBL;
            _modelSimilarPriceDetailRepo = modelSimilarPriceDetailRepo;
        }

        [Route("api/prices/model/{id}/")]
        [EnableCors(origins: origins, headers: "*", methods: "GET")]
        [HttpGet]
        public IHttpActionResult GetModelPriceQuote([FromUri] int id, int cityId, bool isNew, bool isSpecsMandatory = false, bool? isCachedData = true)
        {
            try
            {
                if (id <= 0 || cityId <= 0)
                    return BadRequest();

                CarPriceQuoteDTO priceQuoteList = null;

                priceQuoteList = _iPQAdapter.GetModelPriceQuote(id, cityId, isNew, isSpecsMandatory, isCachedData ?? true);

                if (priceQuoteList != null)
                    return Ok(priceQuoteList);
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                var exception = new ExceptionHandler(e, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                return InternalServerError();
            }
        }

        [Route("api/prices/version/{id}/")]
        [HttpGet]
        public IHttpActionResult GetVersionPriceQuote([FromUri] int id, int cityId)
        {
            try
            {
                if (id <= 0 || cityId <= 0)
                    return BadRequest();

                var versionPriceQuote = _iPriceAdapter.GetVersionCompulsoryPrices(id, cityId);

                if (versionPriceQuote == null)
                {
                    return InternalServerError();
                }

                if (versionPriceQuote.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                return Ok(versionPriceQuote);
            }
            catch (Exception e)
            {
                var exception = new ExceptionHandler(e, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                return InternalServerError();
            }
        }

        [Route("api/prices/")]
        [EnableCors(origins: origins, headers: "*", methods: "POST")]
        [HttpPost]
        public HttpResponseMessage InsertPriceQuote([FromBody] CarPriceQuote pricesInput)
        {
            if (pricesInput.UpdatedBy == 0 || pricesInput.UpdatedBy == 14)
            {
                _logger.Error("API hit for insert price having updated by = " + pricesInput.UpdatedBy + " With ModelId = "
                    + pricesInput.ModelId + " And CityId = " + pricesInput.CityId);
            }

            var response = new HttpResponseMessage();

            ResponseDTO insertResult = null;

            try
            {
                insertResult = _iPQAdapter.InsertPriceQuote(pricesInput);

                response = Request.CreateResponse(insertResult);

                response.StatusCode = (HttpStatusCode)insertResult.StatusCode;

                return response;
            }
            catch (Exception e)
            {
                var exception = new ExceptionHandler(e, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                response = Request.CreateResponse(insertResult);
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }
        }

        [Route("api/prices/")]
        [EnableCors(origins: origins, headers: "*", methods: "DELETE")]
        [HttpDelete]
        public HttpResponseMessage DeletePriceQuote([FromBody] CarPriceQuote pricesInput)
        {
            if (pricesInput.UpdatedBy == 0 || pricesInput.UpdatedBy == 14)
            {
                _logger.Error("API hit for delete price having updated by = " + pricesInput.UpdatedBy + " With ModelId = "
                    + pricesInput.ModelId + " And CityId = " + pricesInput.CityId);
            }

            var response = new HttpResponseMessage();

            ResponseDTO deleteResult = null;

            try
            {
                deleteResult = _iPQAdapter.DeletePriceQuote(pricesInput);

                response = Request.CreateResponse(deleteResult);

                response.StatusCode = (HttpStatusCode)deleteResult.StatusCode;

                return response;
            }
            catch (Exception e)
            {
                var exception = new ExceptionHandler(e, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                response = Request.CreateResponse(deleteResult);
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }
        }

        [HttpGet, Route("api/v2/prices/version/{versionId}")]
        public IHttpActionResult GetVersionPriceAndEmi([FromUri]int versionId, int cityId)
        {
            try
            {
                if (_validateMMV.IsModelVersionValid(versionId) && (cityId < 1 || _validateLocation.IsCityValid(cityId)))
                {
                    var versionPrice = _iPriceAdapter.GetVersionPriceAndEmi(versionId, cityId);
                    return Ok(versionPrice);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [Route("api/prices/similar/")]
        [EnableCors(origins: origins, headers: "*", methods: "POST")]
        [HttpPost]
        public IHttpActionResult SaveModelSimilarPriceDetails([FromBody] ModelSimilarPriceDetail modelSimilarPriceDetail)
        {
            try
            {
                if (modelSimilarPriceDetail.ModelId < 1 || !_validateMMV.IsModelValid(modelSimilarPriceDetail.ModelId))
                {
                    return BadRequest();
                }

                _modelSimilarPriceDetailBL.CreateOrUpdate(modelSimilarPriceDetail);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return InternalServerError();
            }
        }

        [Route("api/prices/similar/")]
        [EnableCors(origins: origins, headers: "*", methods: "GET")]
        [HttpGet]
        public IHttpActionResult GetModelSimilarPriceDetails(int modelId)
        {
            try
            {
                if (modelId < 1 || !_validateMMV.IsModelValid(modelId))
                {
                    return BadRequest();
                }

                var modelSimilarPriceDetail = _modelSimilarPriceDetailRepo.Get(modelId);
                return Ok(modelSimilarPriceDetail);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return InternalServerError();
            }
        }
    }
}
