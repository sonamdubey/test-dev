using Bikewale.DAL.AutoBiz;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
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
        public IHttpActionResult GetDealerDetailsPQ(uint versionId, uint dealerId, uint cityId)
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
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                if (objDealerDetail != null)
                    return Ok(objDealerDetail);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Written By : Ashwini Todkar on  7 Nov 2014
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAvailabilityDays(uint dealerId, uint versionId)
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
                return Ok(numOfDays);
            else
                return NotFound();
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 18 Dec 2014
        /// Summary : to get Dealer booking amount by version id
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IHttpActionResult GetDealerBookingAmount(uint versionId, uint dealerId)
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
                    //HttpContext.Current.Trace.Warn("SaveDealerDisclaimer ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                    return InternalServerError();
                }
                if (objAmount != null)
                    return Ok(objAmount);
                else
                    return NotFound();
            }
            else
                return BadRequest();
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
        public IHttpActionResult CopyOffersToCities(uint dealerId, string lstOfferIds, string lstCityId)
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
                //HttpContext.Current.Trace.Warn("CopyOffersToCities ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isSuccess)
                return Ok(isSuccess);
            else
                return NotFound();
        }
        #endregion
    }
}