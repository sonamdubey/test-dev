using Bikewale.Notifications;
using BikewaleOpr.BAL;
using BikewaleOpr.DALs;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers
{
    public class DealerPriceQuoteController : ApiController
    {

        /// <summary>
        /// Created By : Sadhana Upadhayay on 7 Nov 2014
        /// Summary : To unmap dealer with area
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areaIdList"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UnmapDealerWithArea(uint dealerId, string areaIdList)
        {
            if (dealerId > 0)
            {
                bool isSuccess = false;
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                        isSuccess = objPriceQuote.UnmapDealer(dealerId, areaIdList);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetDealerPriceQuote ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal server error");
                }
                if (isSuccess)

                    return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, "Content not found");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 26 Oct 2015
        /// Summary : To get all Dealer PriceQuote Details like booking amount, availability, dealer details, offers and prices
        ///         which fulfills d area dealer constraints
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <param name="dealerIds"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllDealerPriceQuotes(uint versionId, uint cityId, uint areaId)
        {
            if (cityId > 0 && versionId > 0 && areaId > 0)
            {
                IEnumerable<DealerPriceQuoteDetailed> dealerPriceQuotes = null;
                IEnumerable<uint> dealerIdList = null;
                string dealerIds = string.Empty;
                DealerPriceQuoteList objDealersDetails = new DealerPriceQuoteList();
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealer, Dealers>();
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealer objDealer = container.Resolve<IDealer>();
                        dealerIdList = objDealer.GetAllAvailableDealer((versionId), (areaId));
                        foreach (uint dealerId in dealerIdList)
                        {
                            dealerIds += dealerId.ToString() + ",";
                        }
                        dealerIds = dealerIds.Substring(0, dealerIds.Length - 1);
                        dealerPriceQuotes = objDealer.GetDealerPriceQuoteDetail(versionId, cityId, dealerIds);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetAllDealerPriceQuotes ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                if (dealerPriceQuotes != null)
                {
                    objDealersDetails.DealersDetails = dealerPriceQuotes;
                    return Request.CreateResponse<DealerPriceQuoteList>(HttpStatusCode.OK, objDealersDetails);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, "Content not found");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }   //End of GetAllDealerPriceQuotes
    }
}