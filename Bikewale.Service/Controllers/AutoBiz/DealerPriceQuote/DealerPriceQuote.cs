﻿using Bikewale.DAL.AutoBiz;
using Bikewale.DTO.BikeBooking.Make;
using Bikewale.DTO.Location;
using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.AutoBiz;
using BikeWale.Entities.AutoBiz;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
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
                        //Convert entity to dto

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
        /// Written By : Ashish G. Kamble o 10 May 2015
        /// Summary : Function to get the list of cities where bike booking option is available.
        /// </summary>
        /// <returns>If success returns list of cities.</returns>
        [HttpGet]
        public IHttpActionResult GetBikeBookingCities(uint? modelId = null)
        {
            IEnumerable<CityEntityBase> cities = null;
            IEnumerable<CityEntityBaseDTO> objCities = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                    cities = objPriceQuote.GetBikeBookingCities(modelId);

                    objCities = DealerPriceQuoteMapper.Convert(cities);
                }
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Trace.Warn("GetBikeBookingCities ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                return InternalServerError();
            }

            if (objCities != null)
                return Ok(objCities);
            else
                return NotFound();
        }   // End of GetBikeBookingCities

        /// <summary>
        /// Written By : Ashish G. Kamble on 10 May 2015
        /// Summary : Function to get the list of bike makes in the particular city where booking option is available.
        /// </summary>
        /// <param name="cityId">Should be greater than 0.</param>
        /// <returns>Returns list of makes.</returns>
        public IHttpActionResult GetBikeMakesInCity(uint cityId)
        {
            IEnumerable<BikeMakeEntityBase> makes = null;
            IEnumerable<BBMakeBase> objMakes = null;

            if (cityId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                        makes = objPriceQuote.GetBikeMakesInCity(cityId);
                        objMakes = DealerPriceQuoteMapper.Convert(makes);
                    }
                }
                catch (Exception ex)
                {
                    //HttpContext.Current.Trace.Warn("GetBikeBookingCities ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                    return InternalServerError();
                }

                if (objMakes != null)
                    return Ok(objMakes);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }   // End of GetBikeMakesInCity

        [HttpGet]
        public IHttpActionResult GetOfferTerms(string offerMaskingName, int offerId)
        {
            OfferHtmlEntity objTerm = null;
            OfferHtmlDTO objOffers = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objCategoryNames = container.Resolve<DealerPriceQuoteRepository>();
                    objTerm = objCategoryNames.GetOfferTerms(offerMaskingName, offerId);
                    objOffers = DealerPriceQuoteMapper.Convert(objTerm);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                return InternalServerError();
            }

            if (objOffers != null)
                return Ok(objOffers);
            else
                return NotFound();
        }

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