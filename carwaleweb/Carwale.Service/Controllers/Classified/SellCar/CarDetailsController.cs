using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Service.Filters.ExceptionFilters;
using Carwale.Service.Filters.ExceptionFilters.Classified;
using Carwale.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http;
using FluentValidation;

namespace Carwale.Service.Controllers.Classified.SellCar
{
    [ApiLogExceptionFilter, SellCarApiExceptionFilter]
    public class CarDetailsController : ApiController
    {
        private readonly ICarDetailsRepository _carDetailsRepository;
        private readonly ICarDetailsBL _carDetailsBL;

        private static readonly JsonMediaTypeFormatter _jsonFormatter = new JsonMediaTypeFormatter
        {
            SerializerSettings = { NullValueHandling = NullValueHandling.Ignore,
                                   ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                  }
        };

        public CarDetailsController(
            ICarDetailsRepository carDetailsRepository, 
            ICarDetailsBL carDetailsBL)
        {
            _carDetailsRepository = carDetailsRepository;
            _carDetailsBL = carDetailsBL;
        }

        [HttpPost]
        [Route("api/used/sell/cardetails/")]
        public IHttpActionResult SaveSellCarDetails(SellCarInfo sellCarInfo, string tempId)
        {
            if (sellCarInfo == null || string.IsNullOrEmpty(tempId))
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp
                {
                    Heading = "Data Invalid",
                    Description = (sellCarInfo == null) ? "SellCar info can not be null/empty" : "Invalid TempInquiryId"
                }, _jsonFormatter);
            }

            sellCarInfo.TempInquiryId = Convert.ToInt32(CarwaleSecurity.Decrypt(tempId, true));

            if (sellCarInfo.TempInquiryId <= 0)
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp
                {
                    Heading = "Data Invalid",
                    Description = "Invalid TempInquiryId"
                }, _jsonFormatter);
            }

            sellCarInfo.IPAddress = UserTracker.GetUserIp();
            return Ok(_carDetailsBL.ProcessCarDetails(sellCarInfo));
        }

        [HttpPut, Route("api/used/sell/cardetails/")]
        public IHttpActionResult UpdateCarDetails([FromBody]SellCarInfo sellCarInfo, string encryptedId)
        {
            if (sellCarInfo == null || string.IsNullOrEmpty(encryptedId) || (!new SellCarInfoValidator().Validate(sellCarInfo, ruleSet: "address,carmake,carmodel,carversion,cardetails,carregistration,carinsurance").IsValid))
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp
                {
                    Heading = "Data Invalid",
                    Description = "Data entered is incorrect",
                }, _jsonFormatter);
            }
            int inquiryId = Convert.ToInt32(CarwaleSecurity.Decrypt(encryptedId, true));
            if (inquiryId <= 0)
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp()
                {
                    Heading = "Data Invalid",
                    Description = "Data entered is incorrect"
                }, _jsonFormatter);
            }
            bool isCarDetailsUpdated = _carDetailsRepository.UpdateCarDetails(sellCarInfo, inquiryId);
            return Ok(isCarDetailsUpdated);
        }

        [HttpPut, Route("api/v1/used/sell/cardetails/")]
        public IHttpActionResult UpdateCarDetailsV1([FromBody]SellCarInfo sellCarInfo, string encryptedId)
        {
            if (sellCarInfo == null || string.IsNullOrEmpty(encryptedId) || (!new SellCarInfoValidator().Validate(sellCarInfo, ruleSet: "address,carmake,carmodel,carversion,cardetails,carregistration,carregistrationtype,carinsurance").IsValid))
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp
                {
                    Heading = "Data Invalid",
                    Description = "Data entered is incorrect",
                }, _jsonFormatter);
            }
            int inquiryId = Convert.ToInt32(CarwaleSecurity.Decrypt(encryptedId, true));
            if (inquiryId <= 0)
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp()
                {
                    Heading = "Data Invalid",
                    Description = "Data entered is incorrect"
                }, _jsonFormatter);
            }
            bool isCarDetailsUpdated = _carDetailsRepository.UpdateCarDetailsV1(sellCarInfo, inquiryId);
            return Ok(isCarDetailsUpdated);
        }

    }
}
