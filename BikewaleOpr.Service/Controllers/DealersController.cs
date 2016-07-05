using Bikewale.Notifications;
using BikewaleOpr.DAL;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BikewaleOpr.Service
{
    /// <summary>
    /// Created By : Sangram Nandkhile  05 July 2016
    /// </summary>
    public class DealersController : ApiController
    {
        /// <summary>
        /// Created By : Suresh Prajapati on 28th Oct, 2014.
        /// Description : To Get Dealer's name by cityId.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>Dealer's Name</returns>

        [HttpGet]
        public HttpResponseMessage GetAllDealers(UInt32 cityId)
        {
            if (cityId > 0)
            {
                DataTable objDealer = null;

                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objAllDealer = container.Resolve<DealersRepository>();
                        objDealer = objAllDealer.GetAllDealers(cityId);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetAllDealers ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                if (objDealer != null)
                    return Request.CreateResponse<DataTable>(HttpStatusCode.OK, objDealer);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }

#if unused
                    /// <summary>
                    /// Written By : Ashwini Todkar on  28th Oct 2014
                    /// Description : Method to get dealer details like pricelist, dealer, offers, emi, booking amount & facility
                    /// Modified By : Suresh Prajapati on 20th Oct, 2015
                    /// Description : Added "0" check for versionId, dealerId and cityId
                    /// </summary>
                    /// <param name="versionId"></param>
                    /// <param name="dealerId"></param>
                    /// <param name="cityId"></param>
                    /// <returns></returns>

                    [HttpGet]
                    public HttpResponseMessage GetDealerDetailsPQ(uint versionId, uint dealerId, uint cityId)
                    {
                        if (versionId > 0 && dealerId > 0 && cityId > 0)
                        {
                            PQ_DealerDetailEntity objDealerDetail = null;
                            try
                            {
                                using (IUnityContainer container = new UnityContainer())
                                {
                                    container.RegisterType<IDealers, DealersRepository>();
                                    IDealers objDealer = container.Resolve<DealersRepository>();
                                    PQParameterEntity objParam = new PQParameterEntity();
                                    objParam.CityId = cityId;
                                    objParam.DealerId = dealerId;
                                    objParam.VersionId = versionId;
                                    objDealerDetail = objDealer.GetDealerDetailsPQ(objParam);
                                }
                            }
                            catch (Exception ex)
                            {
                                HttpContext.Current.Trace.Warn("GetDealerDetailsPQ ex : " + ex.Message + ex.Source);
                                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                                objErr.SendMail();
                            }
                            if (objDealerDetail != null)
                                return Request.CreateResponse<PQ_DealerDetailEntity>(HttpStatusCode.OK, objDealerDetail);
                            else
                                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
                        }
                        else
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
                    }
        /// <summary>
        /// Created By : Suresh Prajapati on 29th Oct 2014
        /// Summary : To Get Dealer Cities for which Bike Dealer exists
        /// </summary>
        /// <returns>Dealer's Cities</returns>

        [HttpGet]
        public HttpResponseMessage GetDealerCities()
        {
            DataTable objCities = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    objCities = objCity.GetDealerCities();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerCities ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (objCities != null)
                return Request.CreateResponse<DataTable>(HttpStatusCode.OK, objCities);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
        }


        /// <summary>
        /// Created By : Suresh Prajapati on 03rd Nov, 2014
        /// Description : To Get Offer types for drop down.
        /// </summary>
        /// <returns>Offer Types</returns>

        [HttpGet]
        public HttpResponseMessage GetOfferTypes()
        {
            DataTable objOffers = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    objOffers = objCity.GetOfferTypes();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetOfferTypes ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (objOffers != null)
                return Request.CreateResponse<DataTable>(HttpStatusCode.OK, objOffers);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
        }

        /// <summary>
        /// Created By : Suresh Prajapati on 03rd Nov, 2014.
        /// Description : To Get Dealer Offer Details for particular Dealer Name and City.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>

        [HttpGet]
        public HttpResponseMessage GetDealerOffers(int dealerId)
        {
            List<OfferEntity> objOffers = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();

                    objOffers = objCity.GetDealerOffers(dealerId);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerOffers ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (objOffers != null)
                return Request.CreateResponse<List<OfferEntity>>(HttpStatusCode.OK, objOffers);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 05th Nov, 2014.
        /// Description : To save New Dealer offer.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="offercategoryId"></param>
        /// <param name="offerText"></param>
        /// <param name="offerValue"></param>
        /// <param name="offervalidTill"></param>
        /// <returns></returns>

        [HttpPost]
        public HttpResponseMessage SaveDealerOffer(int dealerId, uint userId, int cityId, string modelId, int offercategoryId, string offerText, int? offerValue, DateTime offervalidTill, bool isPriceImpact)
        {
            bool isSuccess = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    isSuccess = objCity.SaveDealerOffer(Convert.ToInt32(dealerId), userId, Convert.ToInt32(cityId), modelId, Convert.ToInt32(offercategoryId), offerText, Convert.ToInt32(offerValue), Convert.ToDateTime(offervalidTill), isPriceImpact);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveDealerOffer ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isSuccess)
                return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
        }
#endif
        /// <summary>
        ///  Created By  : Suresh Prajapati on 04th Nov, 2014.
        ///  Description : To Delete an Offer specified by "offerId".
        ///  Modified By : Sadhana Upadhyay on 6Th Oct 2015
        ///  Summary : To delete Multiple offers
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DeleteDealerOffer(string offerId)
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
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                if (isdeleteSucess)
                    return Request.CreateResponse<bool>(HttpStatusCode.OK, isdeleteSucess);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
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
        public HttpResponseMessage UpdateDealerBikeOffers(uint offerId, uint userId, uint offerCategoryId, string offerText, uint? offerValue, DateTime offerValidTill, bool isPriceImpact)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objDealer = container.Resolve<DealersRepository>();

                    objDealer.UpdateDealerBikeOffers(offerId, userId, offerCategoryId, offerText, offerValue, offerValidTill, isPriceImpact);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UpdateDealerBikeOffers ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Some error occured.");
            }

            return Request.CreateErrorResponse(HttpStatusCode.Created, "Dealer Bike Offers Updated.");
        }

#if unused
        /// <summary>
        /// Written By : Ashish G. Kamble on 7 Nov 2014
        /// Summary : Function to get the dealer facilities.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDealerFacilities(uint dealerId)
        {
            if (dealerId > 0)
            {
                List<FacilityEntity> objFacility = null;

                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();

                        objFacility = objDealer.GetDealerFacilities(dealerId);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetDealerFacilities ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }

                if (objFacility != null)
                    return Request.CreateResponse<List<FacilityEntity>>(HttpStatusCode.OK, objFacility);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }   // End of GetDealerFacilities

        /// <summary>
        /// Written By : Ashish G. Kamble on 7 Nov 2014
        /// Summary : Function to save the dealer facility.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="facility"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SaveDealerFacilities(uint dealerId, string facility, bool isActive)
        {
            if (dealerId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();
                        objDealer.SaveDealerFacility(dealerId, facility, isActive);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("SaveDealerFacilities ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Some error occured.");
                }

                return Request.CreateErrorResponse(HttpStatusCode.Created, "Facility Saved.");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }   // End of SaveDealerFacilities


        /// <summary>
        /// Written By : Ashish G. Kamble on 7 Nov 2014
        /// Summary : Function to update the dealer facility.
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="facility"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateDealerFacilities(uint facilityId, string facility, bool isActive)
        {
            if (facilityId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();
                        objDealer.UpdateDealerFacility(facilityId, facility, isActive);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("UpdateDealerFacilities ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Some error occured.");
                }

                return Request.CreateErrorResponse(HttpStatusCode.Created, "Facility Updated.");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }   // End of UpdateDealerFacilities

        /// <summary>
        /// Written By : Ashish G. Kamble on 9 Nov 2014
        /// Summary : Function to save the dealer loan amounts for the emi calculations.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="tenure"></param>
        /// <param name="rateOfInterest"></param>
        /// <param name="ltv"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SaveDealerLoanAmounts(uint dealerId, ushort tenure, float rateOfInterest, ushort ltv, string loanProvider)
        {
            if (dealerId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();

                        objDealer.SaveDealerLoanAmounts(dealerId, tenure, rateOfInterest, ltv, loanProvider);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("SaveDealerLoanAmounts ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Some error occured.");
                }

                return Request.CreateErrorResponse(HttpStatusCode.Created, "Dealer Loan Amount Saved.");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }   // End of SaveDealerLoanAmounts

        /// <summary>
        /// Written By : Ashish G. Kamble on 9 Nov 2014
        /// Summary : Function to udpate the dealer loan amounts for the emi calculations.
        /// </summary>
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="tenure"></param>
        /// <param name="rateOfInterest"></param>
        /// <param name="ltv"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateDealerLoanAmounts(uint dealerId, ushort tenure, float rateOfInterest, ushort ltv, string loanProvider)
        {
            if (dealerId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();

                        objDealer.UpdateDealerLoanAmounts(dealerId, tenure, rateOfInterest, ltv, loanProvider);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("UpdateDealerLoanAmounts ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Some error occured.");
                }

                return Request.CreateErrorResponse(HttpStatusCode.Created, "Dealer Loan Amount Updated.");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }   // End of UpdateDealerLoanAmounts


        /// <summary>
        /// Written By : Ashwini Todkar on  28th Oct 2014
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDealerLoanAmounts(uint dealerId)
        {
            EMI objEmi = null;

            if (dealerId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();
                        objEmi = objDealer.GetDealerLoanAmounts(dealerId);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetDealerLoanAmounts ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Some error occured.");
                }

                if (objEmi != null)
                    return Request.CreateResponse<EMI>(HttpStatusCode.OK, objEmi);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }   // End of GetDealerLoanAmounts

        /// <summary>
        /// Created By  : Suresh Prajapati on 11th Nov, 2014.
        /// Description : Function to save New Bike Availability.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="bikemodelId"></param>
        /// <param name="bikeversionId"></param>
        /// <param name="numOfDays"></param>
        /// <returns></returns>

        [HttpPost]
        public HttpResponseMessage SaveBikeAvailability(uint dealerId, uint bikemodelId, uint? bikeversionId, ushort numOfDays)
        {
            bool isSuccess = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    isSuccess = objCity.SaveBikeAvailability(dealerId, bikemodelId, Convert.ToUInt32(bikeversionId), Convert.ToUInt16(numOfDays));
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveBikeAvailability ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isSuccess)
                return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 27th Jan, 2015.
        /// Description : Function to save New Bike Availability.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SaveBikeAvailability([FromBody] string dt)
        {
            bool isSuccess = false;

            DataTable dtValue = (DataTable)JsonConvert.DeserializeObject(dt, (typeof(DataTable)));

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objDays = container.Resolve<DealersRepository>();

                    isSuccess = objDays.SaveBikeAvailability(dtValue);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveBikeAvailability ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isSuccess)
                return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Not Modified");
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 28th Jan, 2015.
        /// Description : To delete Bike Availability days.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteBikeAvailability([FromBody] string dt)
        {
            bool isDeleted = false;
            DataTable dtValue = (DataTable)JsonConvert.DeserializeObject(dt, (typeof(DataTable)));

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objDays = container.Resolve<DealersRepository>();
                    isDeleted = objDays.DeleteBikeAvailabilityDays(dtValue);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DeleteBikeAvailability ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isDeleted)
                return Request.CreateResponse<bool>(HttpStatusCode.OK, isDeleted);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Not Modified");
        }

#endif
        /// <summary>
        /// Created By  : Suresh Prajapati On 11th Nov, 2014.
        /// Description : Function To Get Added Bikes Availability by specific Dealer.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>

        [HttpGet]
        public HttpResponseMessage GetBikeAvailability(uint dealerId)
        {
            List<OfferEntity> objAvailability = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    objAvailability = objCity.GetBikeAvailability(dealerId);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeAvailability ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (objAvailability != null)
                return Request.CreateResponse<List<OfferEntity>>(HttpStatusCode.OK, objAvailability);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
        }

        /// <summary>
        /// Created By  : Suresh Prajapati On 12th Nov, 2014.
        /// Description : Function To Edit Availability Limit Days of Bike By Specified Dealer.
        /// </summary>
        /// <param name="availabilityId"></param>
        /// <param name="days"></param>
        /// <returns></returns>

        [HttpGet]
        public HttpResponseMessage EditAvailabilityDays(int availabilityId, int days)
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
                HttpContext.Current.Trace.Warn("EditAvailabilityDays ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (iseditSuccess)
                return Request.CreateResponse<bool>(HttpStatusCode.OK, iseditSuccess);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
        }

        /// <summary>
        /// Written By : Ashwini Todkar on  7 Nov 2014
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAvailabilityDays(uint dealerId, uint versionId)
        {
            uint numOfDays = 0;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objDays = container.Resolve<DealersRepository>();

                    numOfDays = objDays.GetAvailabilityDays(dealerId, versionId);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetAvailabilityDays ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (numOfDays > 0)
                return Request.CreateResponse<uint>(HttpStatusCode.OK, numOfDays);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Get Disclaimer By Specified Dealer ID.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>

        [HttpGet]
        public HttpResponseMessage GetDealerDisclaimer(uint dealerId)
        {
            List<DealerDisclaimerEntity> objDisclaimer = null;
            if (dealerId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();

                        objDisclaimer = objDealer.GetDealerDisclaimer(dealerId);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetDealerLoanAmounts ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Some error occured.");
                }

                if (objDisclaimer != null)
                    return Request.CreateResponse<List<DealerDisclaimerEntity>>(HttpStatusCode.OK, objDisclaimer);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");

        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Save New Disclaimer for a Dealers.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="disclaimer"></param>
        /// <returns></returns>

        [HttpPost]
        public HttpResponseMessage SaveDealerDisclaimer(uint dealerId, uint makeId, uint? modelId, uint? versionId, string disclaimer)
        {
            if (dealerId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();

                        objDealer.SaveDealerDisclaimer(dealerId
                                                      , makeId
                                                      , Convert.ToUInt32(modelId)
                                                      , Convert.ToUInt32(versionId)
                                                      , Convert.ToString(disclaimer)
                                                     );
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("SaveDealerDisclaimer ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Some error occured.");
                }

                return Request.CreateErrorResponse(HttpStatusCode.Created, "Dealer Disclaimer Saved.");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Update Disclaimer of a Dealer.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="disclaimer"></param>
        /// <returns></returns>

        [HttpPost]
        public HttpResponseMessage UpdateDealerDisclaimer(uint dealerId, uint versionId, string disclaimer)
        {
            if (dealerId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();

                        objDealer.UpdateDealerDisclaimer(dealerId, versionId, disclaimer);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("UpdateDealerDisclaimer ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Some error occured.");
                }

                return Request.CreateErrorResponse(HttpStatusCode.Created, "Dealer Disclaimer Updated.");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Delete Disclaimer of a Dealer.
        /// </summary>
        /// <param name="disclaimerId"></param>
        /// <returns></returns>

        [HttpPost]
        public HttpResponseMessage DeleteDealerDisclaimer(uint disclaimerId)
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
                HttpContext.Current.Trace.Warn("DeleteDealerDisclaimer ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isDeleteSuccess)
                return Request.CreateResponse(HttpStatusCode.OK, "Content deleted successfully.");
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Content not deleted.");
        }
        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Edit Disclaimer of a Dealer.
        /// </summary>
        /// <param name="disclaimerId"></param>
        /// <param name="newDisclaimerText"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage EditDisclaimer(uint disclaimerId, string newDisclaimerText)
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
                HttpContext.Current.Trace.Warn("EditDisclaimer ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (iseditSuccess)
                return Request.CreateResponse(HttpStatusCode.OK, "Content updated.");
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Content not updated.");
        }

        //Written By : Ashwini Todkar on 17 Dec 2014
        #region Bike Booking Amount actions(post and get)

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Dec 2014
        /// </summary>
        /// <param name="objBookingAmt">dealerid,amount,modelid,versionid</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SaveBookingAmount([FromBody] string strBookingAmt)
        {
            bool isSuccess = false;
            BookingAmountEntity objBookingAmt = (BookingAmountEntity)JsonConvert.DeserializeObject(strBookingAmt, (typeof(BookingAmountEntity)));
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objPQ = container.Resolve<DealersRepository>();

                    isSuccess = objPQ.SaveBookingAmount(objBookingAmt);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveDealerPrices ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isSuccess)
                return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Not Modified");
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Dec 2014
        /// Summary    : Method to get booking amount details(bikename,dealer,amount)
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetBikeBookingAmount(uint dealerId)
        {
            List<BookingAmountEntity> objBookingAmt = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objIBooking = container.Resolve<DealersRepository>();

                    objBookingAmt = objIBooking.GetBikeBookingAmount(dealerId);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (objBookingAmt != null)
                return Request.CreateResponse<List<BookingAmountEntity>>(HttpStatusCode.OK, objBookingAmt);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Dec 2014
        /// Summary    : method to update bike booking amount of a dealer
        /// </summary>
        /// <param name="bookingAmtId"></param>
        /// <param name="amount">booking amount</param>
        /// <returns></returns>
        //[HttpPost]
        //public HttpResponseMessage UpdateBookingAmount([FromBody] string strObjBookingAmt)
        //{
        //    bool isSuccess = false;

        //    try
        //    {
        //        using (IUnityContainer container = new UnityContainer())
        //        {
        //            container.RegisterType<IDealers, DealersRepository>();
        //            IDealers objBookingAmt = container.Resolve<DealersRepository>();
        //            BookingAmountEntityBase objBookingAmtBase = (BookingAmountEntityBase)JsonConvert.DeserializeObject(strObjBookingAmt, (typeof(BookingAmountEntityBase)));
        //            isSuccess = objBookingAmt.UpdateBookingAmount(objBookingAmtBase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpContext.Current.Trace.Warn("UpdateBookingAmount ex : " + ex.Message + ex.Source);
        //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    if (isSuccess)
        //        return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
        //    else
        //        return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Content not modified");
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage UpdateBookingAmount(uint bookingId, uint amount)
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
                HttpContext.Current.Trace.Warn("UpdateBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isSuccess)
                return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Content not modified");
        }


        #endregion

        /// <summary>
        /// Created By : Sadhana Upadhyay on 18 Dec 2014
        /// Summary : to get Dealer booking amount by version id
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetDealerBookingAmount(uint versionId, uint dealerId)
        {
            if (dealerId > 0 && versionId > 0)
            {
                BookingAmountEntity objAmount = null;
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();

                        objAmount = objDealer.GetDealerBookingAmount(versionId, dealerId);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("SaveDealerDisclaimer ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Some error occured.");
                }
                if (objAmount != null)
                    return Request.CreateResponse<BookingAmountEntity>(HttpStatusCode.OK, objAmount);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found.");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }

        /// <summary>
        ///s Written By : Suresh Prajapati on 02nd Jan, 2015
        /// Summary    : To Delete a bike booking amount.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DeleteBookingAmount(uint bookingId)
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
                HttpContext.Current.Trace.Warn("DeleteBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isDeleteSuccess)
                return Request.CreateResponse(HttpStatusCode.OK, "Content deleted successfully.");
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Content not deleted.");
        }

        #region Pivotal Tracker #95410582
        /// <summary>
        /// Copies the Offers to Cities
        /// </summary>
        /// <param name="dealerId">Dealer Id</param>
        /// <param name="lstOfferIds">Comma Delimited Offer Ids</param>
        /// <param name="lstCityId">Comma Delimited City Ids</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CopyOffersToCities(uint dealerId, string lstOfferIds, string lstCityId)
        {
            bool isSuccess = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    isSuccess = objCity.CopyOffersToCities(dealerId, lstOfferIds, lstCityId);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CopyOffersToCities ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isSuccess)
                return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Not Modified");
        }
        #endregion

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Mar 2016
        /// Description :   Get Dealer Benefits
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDealerBenefits(uint dealerId)
        {
            IEnumerable<DealerBenefitEntity> dealerBenefits = null;

            if (dealerId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();

                        dealerBenefits = objDealer.GetDealerBenefits(dealerId);
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "GetDealerBenefits");
                    objErr.SendMail();
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Some error occured.");
                }

                if (dealerBenefits != null)
                    return Request.CreateResponse<IEnumerable<DealerBenefitEntity>>(HttpStatusCode.OK, dealerBenefits);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
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
        public HttpResponseMessage SaveDealerBenefit(uint dealerId, uint cityId, uint catId, string benefitText, uint userId, uint benefitId = 0)
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
                    ErrorClass objErr = new ErrorClass(ex, "SaveDealerBenefit");
                    objErr.SendMail();
                }
                if (isSuccess)
                    return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Not Modified");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }

        /// <summary>
        /// Created By  :   Sumit Kate on 10 Mar 2016
        /// Description :   Deletes Dealer Benefits
        /// </summary>
        /// <param name="benefitIds">Comma seperated benefit ids</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DeleteDealerBenefits(string benefitIds)
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
                ErrorClass objErr = new ErrorClass(ex, "DeleteDealerBenefits");
                objErr.SendMail();
            }
            if (isDeleteSuccess)
                return Request.CreateResponse(HttpStatusCode.OK, "Content deleted successfully.");
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Content not deleted.");
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 11 Mar 2016
        /// Description :   Saves the Dealer EMI vallues
        /// Modified by :   Sangram Nandkhile on 15 Mar 2016
        /// Description :   Removed parameters and added new parameters
        /// </summary>
        /// <param name="dealerId">mandatory</param>
        /// <param name="tenure">mandatory</param>
        /// <param name="rateOfInterest">mandatory</param>
        /// <param name="ltv">mandatory</param>
        /// <param name="loanProvider">mandatory</param>
        /// <param name="userID">mandatory</param>
        /// <param name="minDownPayment">Optional</param>
        /// <param name="maxDownPayment">Optional</param>
        /// <param name="minTenure">Optional</param>
        /// <param name="maxTenure">Optional</param>
        /// <param name="minRateOfInterest">Optional</param>
        /// <param name="maxRateOfInterest">Optional</param>
        /// <param name="processingFee">Optional</param>
        /// <param name="id">Optional</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SaveDealerEMI(
            uint dealerId,
            string loanProvider,
            UInt32 userID,
            float? minDownPayment = null,
            float? maxDownPayment = null,
            ushort? minTenure = null,
            ushort? maxTenure = null,
            float? minRateOfInterest = null,
            float? maxRateOfInterest = null,
            float? minLtv = null,
            float? maxLtv = null,
            float? processingFee = null,
            uint? id = null)
        {
            bool isSuccess = false;
            if (dealerId > 0 && minTenure > 0 && (minRateOfInterest > 0.0f) && minLtv > 0.0f && userID > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();
                        isSuccess = objDealer.SaveDealerEMI(
                            dealerId,
                            minDownPayment,
                            maxDownPayment,
                            minTenure,
                            maxTenure,
                            minRateOfInterest,
                            maxRateOfInterest,
                            minLtv,
                            maxLtv,
                            loanProvider,
                            processingFee,
                            id,
                            userID);
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "SaveDealerEMI");
                    objErr.SendMail();
                }
                if (isSuccess)
                    return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Not Modified");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }

        /// <summary>
        /// Created By  :   Sumit Kate on 10 Mar 2016
        /// Description :   Deletes Dealer EMI
        /// </summary>
        /// <param name="benefitIds">Comma seperated benefit ids</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DeleteDealerEMI(uint id)
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
                ErrorClass objErr = new ErrorClass(ex, "DeleteDealerEMI");
                objErr.SendMail();
            }
            if (isDeleteSuccess)
                return Request.CreateResponse(HttpStatusCode.OK, "Content deleted successfully.");
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Content not deleted.");
        }
    }
}