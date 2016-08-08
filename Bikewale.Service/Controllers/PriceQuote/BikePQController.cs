using Bikewale.BAL.PriceQuote;
using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Service.Utilities;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Bikewale Price Quote Controller
    /// Author  :   Sumit Kate
    /// Created On  :   25 Aug 2015
    /// </summary>
    public class BikePQController : CompressionApiController
    {
        private readonly IPriceQuote _objPriceQuote = null;
        public BikePQController(IPriceQuote objPriceQuote)
        {
            _objPriceQuote = objPriceQuote;
        }
        /// <summary>
        /// Gets the BikeWale Price Quote from the Price Quote Id
        /// </summary>
        /// <param name="pqId">Price Quote Id</param>
        /// <returns>BikeWale Price Quote</returns>
        [ResponseType(typeof(PQBikePriceQuoteOutput))]
        public IHttpActionResult Get(UInt64 pqId)
        {
            BikeQuotationEntity quotation = null;
            PQBikePriceQuoteOutput bwPriceQuote = null;
            try
            {
                quotation = _objPriceQuote.GetPriceQuoteById(pqId);
                if (quotation != null)
                {
                    bwPriceQuote = PQBikePriceQuoteOutputMapper.Convert(quotation);

                    quotation.Varients = null;

                    return Ok(bwPriceQuote);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.BikePQController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 1st August 2016
        /// Description :  To take action for pq generation based on different paramters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, ResponseType(typeof(BikePQOutput)), Route("api/generatepq/")]
        public IHttpActionResult GeneratePQByCityArea(Bikewale.DTO.PriceQuote.v2.PQInput input)
        {
            PriceQuoteParametersEntity objPQEntity = null;
            BikePQOutput pqOutput = null;
            Bikewale.Entities.PriceQuote.v2.PQByCityAreaEntity pqOut = null;
            try
            {
                if (input != null && input.ModelId > 0)
                {
                    objPQEntity = new PriceQuoteParametersEntity();
                    objPQEntity.CityId = input.CityId;
                    objPQEntity.AreaId = input.AreaId > 0 ? input.AreaId : 0;
                    objPQEntity.ClientIP = input.ClientIP;
                    objPQEntity.SourceId = Convert.ToUInt16(input.SourceType);
                    objPQEntity.ModelId = input.ModelId;
                    objPQEntity.UTMA = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : string.Empty;
                    objPQEntity.UTMZ = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : string.Empty;
                    objPQEntity.DeviceId = input.DeviceId;
                    objPQEntity.PQLeadId = input.PQLeadId;
                    objPQEntity.RefPQId = input.RefPQId;

                    PQByCityArea pqbyCityArea = new PQByCityArea();
                    pqOut = pqbyCityArea.GetPriceQuoteByCityArea(objPQEntity);

                    if (input.IsPersistance)
                    {
                        pqOut = new Bikewale.Entities.PriceQuote.v2.PQByCityAreaEntity();
                        pqOut.PQCitites = pqbyCityArea.FetchCityByModelId(Convert.ToInt32(objPQEntity.ModelId));

                        var selectedCity = pqOut.PQCitites.FirstOrDefault(p => p.CityId == objPQEntity.CityId);
                        pqOut.IsCityExists = selectedCity != null;

                        if (pqOut.IsCityExists && selectedCity.HasAreas)
                            pqOut.PQAreas = pqbyCityArea.GetAreaForCityAndModel(Convert.ToInt32(objPQEntity.ModelId), Convert.ToInt32(objPQEntity.CityId));
                    }
                    else
                    {
                        pqOut = pqbyCityArea.GetPriceQuoteByCityArea(objPQEntity);
                    }


                    if (pqOut != null)
                    {
                        return Ok(pqOutput);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.BikePQController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }



    }
}
