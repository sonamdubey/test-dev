using System;
using System.Web.Http;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Utility;
using Carwale.Interfaces.Classified.SellCar;
using AEPLCore.Logging;
using Carwale.Service.Filters;
using Carwale.DTOs.Classified.SellCar;
using AutoMapper;
using Carwale.Interfaces.CarData;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using Carwale.Interfaces.Classified.MyListings;
using FluentValidation;
using FluentValidation.Results;
using Carwale.Interfaces.Blocking;
using Carwale.Entity.Blocking;
using Carwale.Entity.Blocking.Enums;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using Carwale.Interfaces.Geolocation;
using Carwale.Utility.Classified;

namespace Carwale.Service.Controllers.Classified
{
	
    public class ListingController : ApiController
    {
		private static Logger Logger = LoggerFactory.GetLogger();
        private readonly IListingsBL _listingsBL;
        private readonly ICarDetailsRepository _carDetailsRepository;
        private readonly IListingRepository _listingRepo;
        private readonly ICarVersionCacheRepository _carVersionsCacheRepo;
        private readonly IMyListings _myListings;
        private readonly IBlockedCommunicationsRepository _blockedCommunicationRepo;
        private readonly ISellCarBL _sellCarBL;
        private readonly IGeoCitiesCacheRepository _geoCityCacheRepo;
        
        private static readonly JsonMediaTypeFormatter _jsonFormatter = new JsonMediaTypeFormatter
        {
            SerializerSettings =
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            }
        };

        public ListingController
            (
                IListingsBL listingsBL,
                ICarDetailsRepository carDetailsRepository,
                IListingRepository listingRepo,
                ICarVersionCacheRepository carVersionsCacheRepo,
                IMyListings myListings,
                IBlockedCommunicationsRepository blockedCommunicationRepo,
                ISellCarBL sellCarBL,
                IGeoCitiesCacheRepository geoCityCacheRepo
            )
        {
            _listingsBL = listingsBL;
            _carDetailsRepository = carDetailsRepository;
            _listingRepo = listingRepo;
            _carVersionsCacheRepo = carVersionsCacheRepo;
            _myListings = myListings;
            _blockedCommunicationRepo = blockedCommunicationRepo;
            _sellCarBL = sellCarBL;
            _geoCityCacheRepo = geoCityCacheRepo;
        }

        [HttpDelete, Route("api/listings/"), Carwale.Service.Filters.LogApi]
        [ApiAuthorization]
        public IHttpActionResult DeleteListing([FromBody] Listing listingDelete)
        {
            try
            {
                ValidateStatusUpdateRequest(listingDelete, StatusValidationType.Delete);
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _listingsBL.PushToQueue(listingDelete);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
            return Ok();
        }

        [ApiAuthorization]
        [HttpPost, Route("api/listings/"), Carwale.Service.Filters.LogApi]
        public IHttpActionResult Create([FromBody]TempCustomerSellInquiry customerSellInquiry)
        {
            
            if(customerSellInquiry == null)
            {
                return BadRequest("request object can not be null");
            }
            var carInfoValidation = new SellCarInfoValidator().Validate(customerSellInquiry.sellCarInfo, ruleSet: "address,carversion,cardetails,source,carregistration,carregistrationtype,carinsurance");
            AddValidationsToModelState(carInfoValidation);
            var customerInfoValidation = new SellCarCustomerValidator().Validate(customerSellInquiry.sellCarCustomer);
            AddValidationsToModelState(customerInfoValidation);
            ValidateCity(customerSellInquiry.sellCarCustomer.CityId);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_blockedCommunicationRepo.IsCommunicationBlocked(new BlockedCommunication { Value = customerSellInquiry.sellCarCustomer.Mobile, Type = CommunicationType.Mobile, Module = CommunicationModule.SellCar }))
            {
                return Content(HttpStatusCode.Forbidden, new ModalPopUp
                {
                    Heading = "Number Blocked",
                    Description = "Your number entered is blocked. Please contact CarWale."
                }, _jsonFormatter);
            }

            if (!_sellCarBL.CheckFreeListingAvailability(customerSellInquiry.sellCarCustomer.Mobile))
            {
                return Content(HttpStatusCode.Forbidden, new ModalPopUp
                {
                    Heading = "Listing Limit Reached",
                    Description = Constants.IndividualListingLimitMessage
                }, _jsonFormatter);
            }

            _sellCarBL.InsertVerifiedMobile(customerSellInquiry.sellCarCustomer.Mobile, customerSellInquiry.sellCarInfo.SourceId);
            _sellCarBL.CreateCustomer(customerSellInquiry.sellCarCustomer);
            if (customerSellInquiry.sellCarCustomer.Id <= 0) //unable to create customer
            {
                return Content(HttpStatusCode.InternalServerError, new ModalPopUp()
                {
                    Heading = "Error!",
                    Description = "Something went wrong"
                }, _jsonFormatter);
            }
            customerSellInquiry.sellCarInfo.IPAddress = UserTracker.GetUserIp();
            int inquiryId = _sellCarBL.CreateSellCarInquiryV1(customerSellInquiry);
            if (inquiryId <= 0)
            {
                return Content(HttpStatusCode.InternalServerError, new ModalPopUp()
                {
                    Heading = "Error!",
                    Description = "Something went wrong"
                }, _jsonFormatter);
            }
            return Ok(new { inquiryId});
        }

        private void ValidateCity(int cityId)
        {
            var city = _geoCityCacheRepo.GetCityDetailsById(cityId);
            if (city == null || string.IsNullOrWhiteSpace(city.CityName))
            {
                ModelState.AddModelError("city", "invalid city id");
            }
        }

        [HttpPut, Route("api/listings/"), Carwale.Service.Filters.LogApi]
        [ApiAuthorization]
        public IHttpActionResult UpdateListingStatus([FromBody] Listing listing)
        {
            try
            {
                ValidateStatusUpdateRequest(listing, StatusValidationType.Update);
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _listingsBL.PushToQueue(listing);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
            return Ok();
        }

        [HttpPut, Route("api/listings/{inquiryId}/"), Carwale.Service.Filters.LogApi]
        [ApiAuthorization]
        public IHttpActionResult UpdateListing([FromBody] UpdateListing updateListingData, int inquiryId)
        {
            bool isUpdated = false;
            try
            {
                if (updateListingData == null)
                {
                    ModelState.AddModelError("sellCarInfo", "object can not be null");
                    return BadRequest(ModelState);
                }
                if (inquiryId <= 0)
                {
                    ModelState.AddModelError("inquiryId", "Inquiry Id cannot be less than or equal to zero");
                    return BadRequest(ModelState);
                }
                var validation = new UpdateListingValidator().Validate(updateListingData);
                if (!validation.IsValid && validation.Errors != null)
                {
                    FluentValidationError.LogValidationErrorsAsync(validation.Errors, "C2bUpdateListingErrors ");
                    foreach (var error in validation.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return BadRequest(ModelState);
                }
                SellCarInfo sellCarInfo = Mapper.Map<UpdateListing, SellCarInfo>(updateListingData);
                isUpdated = _carDetailsRepository.UpdateCarDetails(sellCarInfo, inquiryId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
            if (isUpdated)
            {
                return Ok();
            }
            return BadRequest("inquiryId is not valid");
        }

        [HttpPatch, Route("api/listings/{inquiryId}/"), Carwale.Service.Filters.LogApi, ApiAuthorization]
        public IHttpActionResult PatchListing([FromBody] UpdateListing updateListingData, int inquiryId)
        {
            try
            {
                string ruleSet = "cardetails,premium,maskingnumber,carinsurance";
                ValidatePatchRequest(updateListingData, inquiryId, ruleSet);
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SellCarInfo sellCarInfo = Mapper.Map<UpdateListing, SellCarInfo>(updateListingData);
                if (_listingRepo.PatchListingsV1(inquiryId, sellCarInfo))
                {
                    _myListings.RefreshCacheWithCriticalRead(inquiryId);
                    return Ok("Record updated");
                }
                return ResponseMessage(new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Unable to patch request against listingid: " + inquiryId + "and request: " + JsonConvert.SerializeObject(updateListingData));
                return InternalServerError();
            }
        }

        [HttpPatch, Route("api/v1/listings/{inquiryId}/"), Carwale.Service.Filters.LogApi, ApiAuthorization]
        public IHttpActionResult PatchListingV1([FromBody] UpdateListing updateListingData, int inquiryId)
        {
            try
            {
                string ruleSet = "cardetails,premium,maskingnumber,carinsurance,carregistrationtype";
                ValidatePatchRequest(updateListingData, inquiryId, ruleSet);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SellCarInfo sellCarInfo = Mapper.Map<UpdateListing, SellCarInfo>(updateListingData);
                if (_listingRepo.PatchListingsV1(inquiryId, sellCarInfo))
                {
                    _myListings.RefreshCacheWithCriticalRead(inquiryId);
                    return Ok("Record updated");
                }
                return ResponseMessage(new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Unable to patch request against listingid: " + inquiryId + "and request: " + JsonConvert.SerializeObject(updateListingData));
                return InternalServerError();
            }
        }

        [HttpPut,Route("api/listings/scheduledexpiry/")]
        public IHttpActionResult ScheduledExpiry()
        {
            return Ok(_listingsBL.UpdateExpiredListings());
        }

        private void ValidateStatusUpdateRequest(Listing listing,StatusValidationType validationType)
        {
            if (listing == null)
            {
                ModelState.AddModelError("listing", "Listing id can not be null");
            }
            var validation = new ListingValidator(validationType).Validate(listing);
            if (!validation.IsValid && validation.Errors != null)
            {
                FluentValidationError.LogValidationErrorsAsync(validation.Errors, "C2bDeleteListingErrors ");
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
        }

        private void AddValidationsToModelState(ValidationResult validation)
        {
            foreach (var error in validation.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        private void ValidatePatchRequest(UpdateListing updateListingData, int inquiryId,string ruleSet)
        {
            if (updateListingData == null)
            {
                ModelState.AddModelError("updateListingData", "object can not be null");
                return;
            }
            if (inquiryId <= 0)
            {
                ModelState.AddModelError("inquiryId", "Inquiry Id cannot be less than or equal to zero");
                return;
            }
            var validation = new PatchListingValidator().Validate(updateListingData, ruleSet: ruleSet);
            if (!validation.IsValid)
            {
                FluentValidationError.LogValidationErrorsAsync(validation.Errors, "C2bPatchListingErrors ");
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return;
            }
        }
    }
}
