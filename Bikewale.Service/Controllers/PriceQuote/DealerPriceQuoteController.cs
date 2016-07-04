using Bikewale.DAL.AutoBiz;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote;
using Microsoft.Practices.Unity;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Dealer Price Quote Controller
    /// Author  :   Sumit Kate
    /// Created on  :   24 Aug 2015
    /// </summary>
    public class DealerPriceQuoteController : ApiController
    {
        private readonly IDealerPriceQuote _objDealer = null;
        public DealerPriceQuoteController(IDealerPriceQuote objDealer)
        {
            _objDealer = objDealer;
        }
        /// <summary>
        /// Gets the Dealer price Quote availability for given version and city
        /// </summary>
        /// <param name="versionId">bike version id</param>
        /// <param name="cityId">city id</param>
        /// <returns></returns>
        [ResponseType(typeof(bool))]
        public IHttpActionResult Get(uint versionId, uint cityId)
        {
            bool isDealerPricesAvailable = false;
            try
            {
                isDealerPricesAvailable = _objDealer.IsDealerPriceAvailable(versionId, cityId);
                if (isDealerPricesAvailable)
                {
                    return Ok(isDealerPricesAvailable);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.DealerPriceQuoteController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        /// <summary>
        /// Generates the dealer price quote
        /// </summary>
        /// <param name="input">Required parameters to generate the dealer price quote</param>
        /// <returns>Dealer Price Quotation</returns>
        [ResponseType(typeof(DPQuotationOutput))]
        public IHttpActionResult Post([FromBody]DPQuotationInput input)
        {
            PQ_QuotationEntity objPrice = null;
            DPQuotationOutput output = null;
            try
            {
                //string api = String.Format("/api/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", input.CityId, input.VersionId, input.DealerId);

                //using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                //{
                //    objPrice = objClient.GetApiResponseSync<PQ_QuotationEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, api, objPrice);
                //}

                //PQ_QuotationEntity objDealerPrice = default(PQ_QuotationEntity);
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, DealerPriceQuoteRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                    PQParameterEntity objParam = new PQParameterEntity();
                    objParam.CityId = input.CityId;
                    objParam.DealerId = input.DealerId;
                    objParam.VersionId = input.VersionId;
                    objPrice = objPriceQuote.GetDealerPriceQuote(objParam);
                }

                if (objPrice != null)
                {
                    output = DPQuotationOutputMapper.Convert(objPrice);

                    if (objPrice.Disclaimer != null)
                    {
                        objPrice.Disclaimer.Clear();
                        objPrice.Disclaimer = null;
                    }

                    if (objPrice.objOffers != null)
                    {
                        objPrice.objOffers.Clear();
                        objPrice.objOffers = null;
                    }

                    if (objPrice.PriceList != null)
                    {
                        objPrice.PriceList.Clear();
                        objPrice.PriceList = null;
                    }

                    objPrice.Varients = null;

                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.DealerPriceQuoteController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
