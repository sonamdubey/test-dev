using Bikewale.Notifications;
using Bikewale.Utility.Terms;
using BikewaleOpr.DAL;
using BikewaleOpr.DTO.Dealers;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Entity.Dealers;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Dealers;
using BikewaleOpr.Service.AutoMappers.Dealer;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BikewaleOpr.Service
{
    /// <summary>
    /// Created By : Sangram Nandkhile  05 July 2016
    /// </summary>
    public class DealersController : ApiController
    {
        private readonly IDealerPriceRepository dealerPriceRepository = null;
        private readonly IDealerPrice dealerPrice = null;
        private readonly IDealers dealersRepository = null;
        private readonly IVersionAvailability versionAvailability = null;
        public DealersController(IDealerPriceRepository dealerPriceRepositoryObject, IDealerPrice dealerPriceObject,
            IDealers dealersRepositoryObject, IVersionAvailability versionAvailabilityObject)
        {
            dealerPriceRepository = dealerPriceRepositoryObject;
            dealerPrice = dealerPriceObject;
            dealersRepository = dealersRepositoryObject;
            versionAvailability = versionAvailabilityObject;

        }

        /// <summary>
        /// Created by : Aditi Srivastava on 9 feb 2017
        /// Summary    : To get makes in a city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpGet, ResponseType(typeof(BikeMakeBase)), Route("api/makes/city/{cityId}/")]
        public IHttpActionResult GetDealerMakesByCity(int cityId)
        {
            if (cityId > 0)
            {
                IEnumerable<BikewaleOpr.Entities.BikeData.BikeMakeEntityBase> objMakes = null;
                IEnumerable<BikeMakeBase> objMakesDTO = null;
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objAllDealer = container.Resolve<DealersRepository>();
                        objMakes = objAllDealer.GetDealerMakesByCity(cityId);
                    }

                    if (objMakes != null && objMakes.Any())
                    {
                        objMakesDTO = new Collection<BikeMakeBase>();
                        objMakesDTO = DealerListMapper.Convert(objMakes);

                        objMakes = null;

                        return Ok(objMakesDTO);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, string.Format("Exception : BikewaleOpr.Service.Controllers.DealersController.GetDealerMakesByCity: CityId:{0}", cityId));
                    return InternalServerError();
                }

            }
            else
            {
                return BadRequest();
            }

        }
        /// <summary>
        /// Created by : Aditi Srivastava on 9 feb 2017
        /// Summary    : To get dealers of a make in a city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpGet, ResponseType(typeof(DealerBase)), Route("api/dealers/make/{makeid}/city/{cityId}/")]
        public IHttpActionResult GetDealersByMake(uint makeId, uint cityId)
        {
            if (cityId > 0 && makeId > 0)
            {
                IEnumerable<DealerEntityBase> objDealers = null;
                IEnumerable<DealerBase> objDealersDTO = null;
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objAllDealer = container.Resolve<DealersRepository>();
                        objDealers = objAllDealer.GetDealersByMake(makeId, cityId);
                    }

                    if (objDealers != null && objDealers.Any())
                    {
                        objDealersDTO = new Collection<DealerBase>();
                        objDealersDTO = DealerListMapper.Convert(objDealers);

                        objDealers = null;

                        return Ok(objDealersDTO);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, string.Format("Exception : BikewaleOpr.Service.Controllers.DealersController.GetDealersByMake: MakeId:{0}, CityId:{1}", makeId, cityId));
                    return InternalServerError();
                }

            }
            else
            {
                return BadRequest();
            }

        }
        /// <summary>
        /// Created By : Suresh Prajapati on 28th Oct, 2014.
        /// Modified By :   Vishnu Teja Yalakuntla on 01 Aug 2017.
        /// Description : To Get Dealer's name by cityId.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>Dealer's Name</returns>
        [HttpGet, ResponseType(typeof(DealerMakeDTO)), Route("api/dealers/city/{cityId}/")]
        public IHttpActionResult GetDealersByCity(UInt32 cityId)
        {
            if (cityId > 0)
            {
                IEnumerable<DealerMakeEntity> dealerEntities = null;
                IEnumerable<DealerMakeDTO> dealerDtos = null;

                try
                {
                    dealerEntities = dealersRepository.GetDealersByCity(cityId);
                    dealerDtos = DealerListMapper.Convert(dealerEntities);
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                }
                if (dealerDtos != null)
                    return Ok(dealerDtos);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }

        /// <summary>
        ///  Created By  : Suresh Prajapati on 04th Nov, 2014.
        ///  Description : To Delete an Offer specified by "offerId".
        ///  Modified By : Sadhana Upadhyay on 6Th Oct 2015
        ///  Summary : To delete Multiple offers
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult DeleteDealerOffer(string offerId)
        {
            bool isdeleteSucess = false;
            if (!String.IsNullOrEmpty(offerId))
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objCity = container.Resolve<DealersRepository>();

                        isdeleteSucess = objCity.DeleteDealerOffer(offerId);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("DeleteDealerOffer ex : " + ex.Message + ex.Source);
                    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

                }
                if (isdeleteSucess)
                    return Ok(isdeleteSucess);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }

        /// <summary>
        ///  Created By  : Suresh Prajapati on 07th Jan, 2015.
        ///  Description : To Update Dealer Bike Offers".
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="offerId"></param>
        /// <param name="userId"></param>
        /// <param name="offerCstegoryId"></param>
        /// <param name="offerText"></param>
        /// <param name="offerValue"></param>
        /// <param name="offerValidTill"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateDealerBikeOffers(DealerOffersEntity dealerOffer)
        {
            try
            {

                bool isUpdated = false;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objDealer = container.Resolve<DealersRepository>();
                    TermsHtmlFormatting htmlFormat = new TermsHtmlFormatting();
                    dealerOffer.Terms = htmlFormat.MakeHtmlList(dealerOffer.Terms);

                    isUpdated = objDealer.UpdateDealerBikeOffers(dealerOffer);
                    if (isUpdated)
                    {
                        return Ok("Dealer Bike Offers Updated.");
                    }
                    else
                    {
                        return InternalServerError();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);


                return InternalServerError();
            }
        }

        /// <summary>
        /// Created By  : Suresh Prajapati On 12th Nov, 2014.
        /// Description : Function To Edit Availability Limit Days of Bike By Specified Dealer.
        /// </summary>
        /// <param name="availabilityId"></param>
        /// <param name="days"></param>
        /// <returns></returns>

        [HttpGet]
        public IHttpActionResult EditAvailabilityDays(int availabilityId, int days)
        {
            bool iseditSuccess = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();

                    iseditSuccess = objCity.EditAvailabilityDays(Convert.ToInt32(availabilityId), Convert.ToInt32(days));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            if (iseditSuccess)
                return Ok(iseditSuccess);
            else
                return NotFound();
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Delete Disclaimer of a Dealer.
        /// </summary>
        /// <param name="disclaimerId"></param>
        /// <returns></returns>

        [HttpPost]
        public IHttpActionResult DeleteDealerDisclaimer(uint disclaimerId)
        {
            bool isDeleteSuccess = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objDisclaimer = container.Resolve<DealersRepository>();
                    isDeleteSuccess = objDisclaimer.DeleteDealerDisclaimer(disclaimerId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            if (isDeleteSuccess)
                return Ok("Content deleted successfully.");
            else
                return InternalServerError();
        }
        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Edit Disclaimer of a Dealer.
        /// </summary>
        /// <param name="disclaimerId"></param>
        /// <param name="newDisclaimerText"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult EditDisclaimer(uint disclaimerId, string newDisclaimerText)
        {
            bool iseditSuccess = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    iseditSuccess = objCity.EditDisclaimer(disclaimerId, newDisclaimerText);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            if (iseditSuccess)
                return Ok("Content updated.");
            else
                return InternalServerError();
        }

        //Written By : Ashwini Todkar on 17 Dec 2014
        #region Bike Booking Amount actions(post and get)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult UpdateBookingAmount(uint bookingId, uint amount)
        {
            bool isSuccess = false;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objBookingAmt = container.Resolve<DealersRepository>();
                    BookingAmountEntityBase objBookingAmtBase = new BookingAmountEntityBase() { Amount = amount, Id = bookingId };
                    isSuccess = objBookingAmt.UpdateBookingAmount(objBookingAmtBase);
                }
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Trace.Warn("UpdateBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            if (isSuccess)
                return Ok(isSuccess);
            else
                return InternalServerError();
        }

        #endregion

        /// <summary>
        ///s Written By : Suresh Prajapati on 02nd Jan, 2015
        /// Summary    : To Delete a bike booking amount.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult DeleteBookingAmount(uint bookingId)
        {
            bool isDeleteSuccess = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objBooking = container.Resolve<DealersRepository>();
                    isDeleteSuccess = objBooking.DeleteBookingAmount(bookingId);
                }
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Trace.Warn("DeleteBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            if (isDeleteSuccess)
                return Ok("Content deleted successfully.");
            else
                return InternalServerError();
        }

        /// <summary>
        /// Created by  :    Sumit Kate on 10 Mar 2016
        /// Description :   Saves the Dealer benefit
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="catId"></param>
        /// <param name="benefitText"></param>
        /// <param name="userId"></param>
        /// <param name="benefitId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveDealerBenefit(uint dealerId, uint cityId, uint catId, string benefitText, uint userId, uint benefitId = 0)
        {
            bool isSuccess = false;
            if (dealerId > 0 && cityId > 0 && catId > 0 && userId > 0 && benefitId > 0 && !String.IsNullOrEmpty(benefitText))
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objCity = container.Resolve<DealersRepository>();
                        isSuccess = objCity.SaveDealerBenefit(dealerId, cityId, catId, benefitText, userId, benefitId);
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "SaveDealerBenefit");

                    return InternalServerError();
                }
                if (isSuccess)
                    return Ok("isSuccess");
                else
                    return InternalServerError();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Created By  :   Sumit Kate on 10 Mar 2016
        /// Description :   Deletes Dealer Benefits
        /// </summary>
        /// <param name="benefitIds">Comma seperated benefit ids</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult DeleteDealerBenefits(string benefitIds)
        {
            bool isDeleteSuccess = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objDisclaimer = container.Resolve<DealersRepository>();

                    isDeleteSuccess = objDisclaimer.DeleteDealerBenefits(benefitIds);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DeleteDealerBenefits");

            }
            if (isDeleteSuccess)
                return Ok("Content deleted successfully.");
            else
                return InternalServerError();
        }

        /// <summary>
        /// Created By  :   Sumit Kate on 10 Mar 2016
        /// Description :   Deletes Dealer EMI
        /// </summary>
        /// <param name="benefitIds">Comma seperated benefit ids</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult DeleteDealerEMI(uint id)
        {
            bool isDeleteSuccess = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objDealer = container.Resolve<DealersRepository>();
                    isDeleteSuccess = objDealer.DeleteDealerEMI(id);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DeleteDealerEMI");

            }
            if (isDeleteSuccess)
                return Ok("Content deleted successfully.");
            else
                return InternalServerError();
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 02 Aug 2017
        /// Description :   Fetches all dealer pricing for specified city, state and dealer.
        /// </summary>
        /// <param name="benefitIds">Comma seperated benefit ids</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetDealerPrices(uint cityId, uint makeId, uint dealerId)
        {
            IEnumerable<DealerVersionPriceDTO> dealerPricesDtos = null;
            IEnumerable<DealerVersionPriceEntity> dealerPricesEntities = null;
            if (cityId > 0 && makeId > 0 && dealerId > 0)
            {
                try
                {
                    dealerPricesEntities = dealerPrice.GetDealerPriceQuotes(cityId, makeId, dealerId);
                    dealerPricesDtos = DealerListMapper.Convert(dealerPricesEntities);

                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, string.Format(
                        "GetDealerPrices cityId={0} makeId={1} dealerId={2}", cityId, makeId, dealerId));

                }
                if (dealerPricesDtos != null)
                    return Ok(dealerPricesDtos);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Saves pricing to list of dealers cross joined with list of cities
        /// </summary>
        /// <param name="dealerPricesAvaialabilities"></param>
        /// <returns></returns>
        [HttpPost, Route("api/dealers/savepricesandavailability/")]
        public IHttpActionResult SaveDealerPricesAndAvailability(DealerVersionPricesAvailabilityDTO dealerPricesAvaialabilities)
        {
            SaveDealerPricingResponseDTO apiResponse = new SaveDealerPricingResponseDTO();
            UpdatePricingRulesResponseEntity savePriceResponse = new UpdatePricingRulesResponseEntity();

            apiResponse.IsPriceSaved = false;
            apiResponse.IsAvailabilitySaved = false;
            bool isAvailabilitySaved = false;

            if (dealerPricesAvaialabilities != null)
            {
                try
                {
                    if (dealerPricesAvaialabilities.DealerVersionPrices.ItemIds != null && dealerPricesAvaialabilities.DealerVersionPrices.ItemIds.Any() && dealerPricesAvaialabilities.DealerVersionPrices.ItemValues != null && dealerPricesAvaialabilities.DealerVersionPrices.ItemValues.Any() && dealerPricesAvaialabilities.DealerVersionPrices.VersionIds != null && dealerPricesAvaialabilities.DealerVersionPrices.VersionIds.Any() && dealerPricesAvaialabilities.DealerVersionPrices.CityIds != null && dealerPricesAvaialabilities.DealerVersionPrices.CityIds.Any() && dealerPricesAvaialabilities.DealerVersionPrices.DealerIds.Any())
                        savePriceResponse = dealerPrice.SaveVersionPriceQuotes(
                            dealerPricesAvaialabilities.DealerVersionPrices.DealerIds,
                            dealerPricesAvaialabilities.DealerVersionPrices.CityIds,
                            dealerPricesAvaialabilities.DealerVersionPrices.VersionIds,
                            dealerPricesAvaialabilities.DealerVersionPrices.ItemIds,
                            dealerPricesAvaialabilities.DealerVersionPrices.ItemValues,
                            dealerPricesAvaialabilities.BikeModelIds,
                            dealerPricesAvaialabilities.BikeModelNames,
                            dealerPricesAvaialabilities.DealerVersionPrices.EnteredBy,
                            dealerPricesAvaialabilities.MakeId
                        );

                    if (dealerPricesAvaialabilities.DealerVersionAvailabilities.DealerId > 0 && dealerPricesAvaialabilities.DealerVersionAvailabilities.BikeVersionIds != null && dealerPricesAvaialabilities.DealerVersionAvailabilities.BikeVersionIds.Any() && dealerPricesAvaialabilities.DealerVersionAvailabilities.NumberOfDays != null && dealerPricesAvaialabilities.DealerVersionAvailabilities.NumberOfDays.Any())
                        isAvailabilitySaved = versionAvailability.SaveVersionAvailability(
                            dealerPricesAvaialabilities.DealerVersionAvailabilities.DealerId,
                            dealerPricesAvaialabilities.DealerVersionAvailabilities.BikeVersionIds,
                            dealerPricesAvaialabilities.DealerVersionAvailabilities.NumberOfDays
                        );
                    apiResponse = SaveDealerPricingResponseMapper.Convert(savePriceResponse, isAvailabilitySaved);
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "SaveDealerPrice");
                }
                if (apiResponse.IsPriceSaved || apiResponse.IsAvailabilitySaved)
                    return Ok(apiResponse);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 02 Aug 2017
        /// Description :   Saves dealer pricing.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/dealers/saveprices/")]
        public IHttpActionResult SaveDealerPrices(DealerPriceListDTO dealerPrices)
        {
            bool isSaved = false;
            if (dealerPrices != null && dealerPrices.VersionIds != null && dealerPrices.VersionIds.Any() && dealerPrices.CityIds != null && dealerPrices.DealerIds != null && dealerPrices.CityIds.Any() && dealerPrices.DealerIds.Any())
            {
                try
                {
                    isSaved = dealerPrice.SaveVersionPriceQuotes(dealerPrices.DealerIds, dealerPrices.CityIds, dealerPrices.VersionIds,
                        dealerPrices.ItemIds, dealerPrices.ItemValues, dealerPrices.EnteredBy, dealerPrices.BikeModelIds);
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "SaveDealerPrices");
                }
                if (isSaved)
                    return Ok(isSaved);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 02 Aug 2017
        /// Description :   Saves dealer pricing.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/dealers/copyprices/")]
        public IHttpActionResult CopyDealerPrices(DealerPriceListDTO dealerPrices)
        {
            bool isSaved = false;
            if (dealerPrices != null && dealerPrices.VersionIds != null && dealerPrices.VersionIds.Any() && dealerPrices.CityIds != null && dealerPrices.DealerIds != null && dealerPrices.CityIds.Any() && dealerPrices.DealerIds.Any())
            {
                try
                {
                    isSaved = dealerPrice.CopyDealerPriceToOtherDealer(dealerPrices.DealerIds, dealerPrices.CityIds, dealerPrices.VersionIds,
                        dealerPrices.ItemIds, dealerPrices.ItemValues, dealerPrices.EnteredBy, dealerPrices.BikeModelIds);
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "SaveDealerPrices");
                }
                if (isSaved)
                    return Ok(isSaved);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 08 Aug 2017
        /// Description :   Deletes dealer pricing and version availability.
        /// </summary>
        /// <param name="dealerCityVersions"></param>
        /// <returns></returns>
        [HttpPost, Route("api/dealers/deletepricesandavailability/")]
        public IHttpActionResult DeleteDealerPricesAndAvailability(DealerCityVersionsDTO dealerCityVersions)
        {
            DeleteDealerPricingResponseDTO apiResponse = new DeleteDealerPricingResponseDTO();
            apiResponse.IsPriceDeleted = false;
            apiResponse.IsAvailabilityDeleted = false;

            if (dealerCityVersions.CityId > 0 && dealerCityVersions.DealerId > 0)
            {
                try
                {
                    apiResponse.IsPriceDeleted = dealerPrice.DeleteVersionPriceQuotes(
                        dealerCityVersions.DealerId,
                        dealerCityVersions.CityId,
                        dealerCityVersions.BikeVersionIds,
                        dealerCityVersions.BikeModelIds
                    );

                    apiResponse.IsAvailabilityDeleted = versionAvailability.DeleteVersionAvailability(dealerCityVersions.DealerId, dealerCityVersions.BikeVersionIds);
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "DeleteDealerPricesAndAvailability");
                }
                if (apiResponse.IsPriceDeleted || apiResponse.IsAvailabilityDeleted)
                    return Ok(apiResponse);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 03 Aug 2017
        /// Description :   Saves the version availability.
        /// </summary>
        /// <param name="versionDays"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveVersionAvailability(VersionDaysDTO versionDays)
        {
            bool isSaved = false;
            if (versionDays.DealerId > 0)
            {
                try
                {
                    isSaved = versionAvailability.SaveVersionAvailability(versionDays.DealerId, versionDays.BikeVersionIds, versionDays.NumberOfDays);
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "SaveVersionAvailability");
                }
                if (isSaved)
                    return Ok(isSaved);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 03 Aug 2017
        /// Description :   Deletes the availability of a version.
        /// </summary>
        /// <param name="dealerVersions"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteVersionAvailability(DealerCityVersionsDTO dealerVersions)
        {
            bool isDeleted = false;
            if (dealerVersions.DealerId > 0)
            {
                try
                {
                    isDeleted = versionAvailability.DeleteVersionAvailability(dealerVersions.DealerId, dealerVersions.BikeVersionIds);
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "DeleteVersionAvailability");
                }
                if (isDeleted)
                    return Ok(isDeleted);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }

    } // class
} // namespace