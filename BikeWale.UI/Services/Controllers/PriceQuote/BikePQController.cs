using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
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
        private readonly IPQByCityArea _objPQByCityArea = null;
        public BikePQController(IPriceQuote objPriceQuote, IPQByCityArea objPQByCityArea)
        {
            _objPriceQuote = objPriceQuote;
            _objPQByCityArea = objPQByCityArea;
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

                    if (input.IsPersistance)
                    {
                        pqOut = new Bikewale.Entities.PriceQuote.v2.PQByCityAreaEntity();
                        pqOut.PQCitites = _objPQByCityArea.FetchCityByModelId(Convert.ToInt32(objPQEntity.ModelId));

                        var selectedCity = pqOut.PQCitites.FirstOrDefault(p => p.CityId == objPQEntity.CityId);
                        if (selectedCity != null && selectedCity.HasAreas)
                            pqOut.PQAreas = _objPQByCityArea.GetAreaForCityAndModel(Convert.ToInt32(objPQEntity.ModelId), Convert.ToInt32(objPQEntity.CityId));
                    }
                    else
                    {
                        pqOut = _objPQByCityArea.GetPriceQuoteByCityArea(objPQEntity, input.IsReload);
                    }


                    if (pqOut != null)
                    {
                        pqOutput = PQBikePriceQuoteOutputMapper.Convert(pqOut);

                        if (input.IsPersistance)
                        {
                            pqOutput.QueryString = string.Empty;
                            pqOutput.Action = false;
                        }
                        else if (pqOutput.PriceQuote != null)
                        {
                            pqOutput.QueryString = EncodingDecodingHelper.EncodeTo64(string.Format("CityId={0}&AreaId={1}&PQId={2}&VersionId={3}&DealerId={4}", objPQEntity.CityId, pqOutput.PriceQuote.IsDealerAvailable ? objPQEntity.AreaId : 0, pqOutput.PriceQuote.PQId, pqOutput.PriceQuote.VersionId, pqOutput.PriceQuote.DealerId));
                        }


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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.BikePQController.Get");

                return InternalServerError();
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 1st August 2016
        /// Description :  To take action for pq generation based on different paramters
        /// Modifier    : Kartik on 20 jun 2018 for price quote changes modified
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, ResponseType(typeof(Bikewale.DTO.PriceQuote.v2.BikePQOutput)), Route("api/v2/generatepq/")]
        public IHttpActionResult GeneratePQByCityArea(Bikewale.DTO.PriceQuote.v3.PQInput input)
        {
            Entities.PriceQuote.v2.PriceQuoteParametersEntity objPQEntity = null;
            Bikewale.DTO.PriceQuote.v2.BikePQOutput pqOutput = null;
            Bikewale.Entities.PriceQuote.v4.PQByCityAreaEntity pqOut = null;
            try
            {
                if (input != null && input.ModelId > 0)
                {
                    objPQEntity = new Entities.PriceQuote.v2.PriceQuoteParametersEntity();
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
                    objPQEntity.ManufacturerCampaignPageId = input.SourceType == PQSources.Desktop ? ManufacturerCampaignServingPages.Desktop_DealerPriceQuote : ManufacturerCampaignServingPages.Mobile_DealerPriceQuote;

                    if (input.IsPersistance)
                    {
                        pqOut = new Bikewale.Entities.PriceQuote.v4.PQByCityAreaEntity();
                        pqOut.PQCitites = _objPQByCityArea.FetchCityByModelId(Convert.ToInt32(objPQEntity.ModelId));

                        var selectedCity = pqOut.PQCitites.FirstOrDefault(p => p.CityId == objPQEntity.CityId);
                        if (selectedCity != null && selectedCity.HasAreas)
                            pqOut.PQAreas = _objPQByCityArea.GetAreaForCityAndModel(Convert.ToInt32(objPQEntity.ModelId), Convert.ToInt32(objPQEntity.CityId));
                    }
                    else
                    {
                        pqOut = _objPQByCityArea.GetPriceQuoteByCityAreaV2(objPQEntity, input.IsReload);
                    }


                    if (pqOut != null)
                    {
                        pqOutput = PQBikePriceQuoteOutputMapper.Convert(pqOut);

                        if (input.IsPersistance)
                        {
                            pqOutput.QueryString = string.Empty;
                            pqOutput.Action = false;
                        }
                        else if (pqOutput.PriceQuote != null)
                        {
                            pqOutput.QueryString = EncodingDecodingHelper.EncodeTo64(string.Format("CityId={0}&AreaId={1}&PQId={2}&VersionId={3}&DealerId={4}", objPQEntity.CityId, pqOutput.PriceQuote.IsDealerAvailable ? objPQEntity.AreaId : 0, pqOutput.PriceQuote.PQId, pqOutput.PriceQuote.VersionId, pqOutput.PriceQuote.DealerId));
                        }


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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.BikePQController.Get");

                return InternalServerError();
            }
        }
    }
}
