using Bikewale.Notifications;
using BikewaleOpr.BAL;
using BikewaleOpr.DALs;
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

namespace BikewaleOpr.Service.Controllers
{
    public class DealerPriceQuoteController : ApiController
    {
        [HttpDelete]
        public HttpResponseMessage DeletePrices(uint dealerId, uint cityId, string versionIdList)
        {
            bool isSuccess = false;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();
                    isSuccess = objPQ.DeleteVersionPrices(dealerId, cityId, versionIdList);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DeletePrices ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isSuccess)
                return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Not Modified");
        }

        [HttpPost]
        public HttpResponseMessage SaveDealerPrices([FromBody] string dt)
        {
            bool isSuccess = false;
            DataTable dtValue = (DataTable)JsonConvert.DeserializeObject(dt, (typeof(DataTable)));
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();
                    isSuccess = objPQ.SaveDealerPrice(dtValue);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isSuccess)
                return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Not Modified");
        }

        /// <summary>
        /// Created By : Sanjay Soni On 29/10/2014
        /// Summary : api to get bike Category Names
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetBikeCategoryNames(string categoryList)
        {

            List<PQ_Price> objCategory = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objCategoryNames = container.Resolve<DealerPriceQuoteRepository>();
                    objCategory = objCategoryNames.GetBikeCategoryItems(categoryList);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeCategoryNames ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            if (objCategory != null)
                return Request.CreateResponse<List<PQ_Price>>(HttpStatusCode.OK, objCategory);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
        }


        /// <summary>
        /// Modified By : Suresh Prajapati on 20th Jan, 2015
        /// Description : To get Price with bike make information.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDealerPrices(uint cityId, uint makeId, uint dealerId)
        {
            DataSet ds = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();
                    ds = objPQ.GetDealerPrices(cityId, makeId, dealerId);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerPrices ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (ds.Tables.Count > 0)
                return Request.CreateResponse<DataSet>(HttpStatusCode.OK, ds);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");

        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 7 Nov 2014
        /// Summary : To map dealer with area
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areaIdList"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage MapDealerWithArea(uint dealerId, string areaIdList)
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
                        isSuccess = objPriceQuote.MapDealerWithArea(dealerId, areaIdList);
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

        // <summary>
        /// Created By : Sadhana Upadhyay on 6 Nov 2014
        /// Summary : To get dealer area mapping detail.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDealerAreaDetail(uint cityId)
        {
            if (cityId > 0)
            {
                List<DealerAreaDetails> objMappingDetail = null;
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                        objMappingDetail = objPriceQuote.GetDealerAreaDetails(cityId);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetDealerPriceQuote ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal server error");
                }
                if (objMappingDetail != null)

                    return Request.CreateResponse<List<DealerAreaDetails>>(HttpStatusCode.OK, objMappingDetail);
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