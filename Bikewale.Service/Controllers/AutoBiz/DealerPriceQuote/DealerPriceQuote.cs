
using Bikewale.DAL.AutoBiz;
using Bikewale.Entities.AutoBiz;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Notifications;
using BikeWale.Entities.AutoBiz;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Bikewale.Service.Controllers.AutoBiz
{
    public class DealerPriceQuoteController : ApiController
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 28th Oct 2014
        /// Summary : api to get dealer price quote
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDealerPriceQuote(uint cityId, uint versionId, uint dealerId)
        {
            if ((cityId > 0) && (versionId > 0) && (dealerId > 0))
            {
                PQ_QuotationEntity objDealerPrice = null;
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                        PQParameterEntity objParam = new PQParameterEntity();
                        objParam.CityId = cityId;
                        objParam.DealerId = dealerId;
                        objParam.VersionId = versionId;

                        objDealerPrice = objPriceQuote.GetDealerPriceQuote(objParam);

                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetDealerPriceQuote ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                if (objDealerPrice != null && objDealerPrice.PriceList.Count > 0)
                    return Request.CreateResponse<PQ_QuotationEntity>(HttpStatusCode.OK, objDealerPrice);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
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
                    //objPrices = objPQ.GetDealerPrices(cityId, modelId, dealerId);
                    ds = objPQ.GetDealerPrices(cityId, makeId, dealerId);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeCategoryNames ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (ds.Tables.Count > 0)
                return Request.CreateResponse<DataSet>(HttpStatusCode.OK, ds);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");

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
                HttpContext.Current.Trace.Warn("SaveDealerPrices ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (isSuccess)
                return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Not Modified");
        }

        //[HttpPost]
        //public HttpResponseMessage SaveDealerPrices([FromBody] DataTable dt)
        //{
        //    bool isSuccess = false;

        //    try
        //    {
        //        using (IUnityContainer container = new UnityContainer())
        //        {
        //            container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
        //            IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();

        //            isSuccess = objPQ.SaveDealerPrice(dt);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpContext.Current.Trace.Warn("SaveDealerPrices ex : " + ex.Message + ex.Source);
        //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    if (isSuccess)
        //        return Request.CreateResponse<bool>(HttpStatusCode.OK, isSuccess);
        //    else
        //        return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Not Modified");       
        //}

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

        /// Created By : Sadhana Upadhyay on 4 Nov 2014
        /// Summary : to check dealer exists or not w.r.t. areaId and version id 
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage IsDealerExists(uint versionId, uint areaId)
        {
            if (areaId > 0 && versionId > 0)
            {
                uint dealerId = 0;
                try
                {

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealer, Bikewale.BAL.AutoBiz.Dealers>();
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();

                        IDealer objDealer = container.Resolve<IDealer>();

                        dealerId = objDealer.IsDealerExists(Convert.ToUInt32(versionId), Convert.ToUInt32(areaId));

                        //container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                        //IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();

                        //dealerId = objPriceQuote.IsDealerExists(Convert.ToUInt32(versionId), Convert.ToUInt32(areaId));
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetDealerPriceQuote ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                if (dealerId > 0)

                    return Request.CreateResponse<uint>(HttpStatusCode.OK, dealerId);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, "Content not found");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }

        /// <summary>
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

        /// <summary>
        /// Written By : Ashish G. Kamble o 10 May 2015
        /// Summary : Function to get the list of cities where bike booking option is available.
        /// </summary>
        /// <returns>If success returns list of cities.</returns>
        [HttpGet]
        public HttpResponseMessage GetBikeBookingCities(uint? modelId = null)
        {
            List<CityEntityBase> cities = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();

                    cities = objPriceQuote.GetBikeBookingCities(modelId);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeBookingCities ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal server error");
            }

            if (cities != null)
                return Request.CreateResponse<List<CityEntityBase>>(HttpStatusCode.OK, cities, "text/json");
            else
                return Request.CreateResponse(HttpStatusCode.NoContent, "Content not found");
        }   // End of GetBikeBookingCities

        /// <summary>
        /// Written By : Ashish G. Kamble on 10 May 2015
        /// Summary : Function to get the list of bike makes in the particular city where booking option is available.
        /// </summary>
        /// <param name="cityId">Should be greater than 0.</param>
        /// <returns>Returns list of makes.</returns>
        public HttpResponseMessage GetBikeMakesInCity(uint cityId)
        {
            List<MakeEntityBase> makes = null;

            if (cityId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();

                        makes = objPriceQuote.GetBikeMakesInCity(cityId);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetBikeBookingCities ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal server error");
                }

                if (makes != null)
                    return Request.CreateResponse<List<MakeEntityBase>>(HttpStatusCode.OK, makes);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, "Content not found");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
        }   // End of GetBikeMakesInCity

        [HttpGet]
        public HttpResponseMessage GetOfferTerms(string offerMaskingName, int offerId)
        {
            OfferHtmlEntity objTerm = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objCategoryNames = container.Resolve<DealerPriceQuoteRepository>();
                    objTerm = objCategoryNames.GetOfferTerms(offerMaskingName, offerId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            if (objTerm != null)
                return Request.CreateResponse<OfferHtmlEntity>(HttpStatusCode.OK, objTerm);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Content not found");
        }

        #region GetAllDealerPriceQuotes Method
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
            if ((cityId > 0) && (versionId > 0) && areaId > 0)
            {
                IEnumerable<DealerPriceQuoteDetailed> dealerPriceQuotes = null;
                IEnumerable<uint> dealerIdList = null;
                string dealerIds = string.Empty;
                DealerPriceQuoteList objDealersDetails = new DealerPriceQuoteList();
                try
                {

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealer, Bikewale.BAL.AutoBiz.Dealers>();
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();

                        IDealer objDealer = container.Resolve<IDealer>();

                        dealerIdList = objDealer.GetAllAvailableDealer((versionId),(areaId));

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
        #endregion

        /// <summary>
        /// Created By : Sumit Kate on 14 Mar 2016
        /// Summary : api to get dealer price quote based on subscription model.
        /// Modified by :   Sumit Kate on 22 Mar 2016
        /// Description :   Changed the return type to IHttpActionResult
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/v2/DealerPriceQuote/GetDealerPriceQuote/")]
        public IHttpActionResult GetDealerPriceQuoteV2(uint cityId, uint versionId, string dealerId = null)
        {
            if (cityId > 0 && versionId > 0)
            {
                DetailedDealerQuotationEntity objDealerPrice = null;
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                        PQParameterEntity objParam = new PQParameterEntity();
                        objParam.CityId = cityId;
                        objParam.DealerId = !String.IsNullOrEmpty(dealerId) ? Convert.ToUInt32(dealerId) : default(UInt32);
                        objParam.VersionId = versionId;

                        objDealerPrice = objPriceQuote.GetDealerPriceQuoteByPackage(objParam);

                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "GetDealerPriceQuoteV2");
                    objErr.SendMail();
                }
                if (objDealerPrice != null)
                    return Ok(objDealerPrice);
                else
                    return NotFound();
            }
            else
                return BadRequest("Bad request");
        }

        /// <summary>
        /// Created By : Sumit Kate on 21 Mar 2016
        /// Summary : to check dealer exists for areaId and version id 
        /// Modified by :   Sumit Kate on 22 Mar 2016
        /// Description :   Changed the return type to IHttpActionResult
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/v2/DealerPriceQuote/IsDealerExists/")]
        public IHttpActionResult IsDealerExistsV2(uint versionId, uint areaId)
        {
            if (areaId > 0 && versionId > 0)
            {
                uint dealerId = 0;
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealer, Bikewale.BAL.AutoBiz.Dealers>();
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealer objDealer = container.Resolve<IDealer>();

                        dealerId = objDealer.IsSubscribedDealerExists(Convert.ToUInt32(versionId), Convert.ToUInt32(areaId));
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetDealerPriceQuote ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                if (dealerId > 0)

                    return Ok(dealerId);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }


        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 28-04-2016
        /// Desc : to check dealer exists for areaId and version id and isSecondaryDealers availble
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/v3/DealerPriceQuote/IsDealerExists/")]
        public IHttpActionResult IsDealerExistsV3(uint versionId, uint areaId)
        {
            if (areaId > 0 && versionId > 0)
            {
                DealerInfo objDealerInfo = null;
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealer, Bikewale.BAL.AutoBiz.Dealers>();
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealer objDealer = container.Resolve<IDealer>();

                        objDealerInfo = objDealer.IsSubscribedDealerExistsV3(Convert.ToUInt32(versionId), Convert.ToUInt32(areaId));
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("GetDealerPriceQuote ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                if (objDealerInfo != null)

                    return Ok(objDealerInfo);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }

    }   //End of class
}   //End of namespace