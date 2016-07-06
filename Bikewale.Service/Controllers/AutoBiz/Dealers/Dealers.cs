using Bikewale.DAL.AutoBiz;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Bikewale.Service.Controllers.AutoBiz
{
    /// <summary>
    /// Created By : Ashwini Todkar on  28th Oct 2014
    /// </summary>
    public class DealersController : ApiController
    {
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
    }
}