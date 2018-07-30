using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Service.AutoMappers.PriceQuote;
using Microsoft.Practices.Unity;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Detailed dealer quatation controller
    /// Author  :   Sumit Kate
    /// Created On  : 24 Aug 2015
    /// </summary>
    public class DetailedDealerQuotationController : ApiController
    {
        /// <summary>
        /// Gets Detailed Dealer Quotation
        /// </summary>
        /// <param name="versionId">Bike version</param>
        /// <param name="dealerId">Dealer Id</param>
        /// <param name="cityId">City Id</param>
        /// <returns></returns>
        [ResponseType(typeof(DDQDealerDetailBase))]
        public HttpResponseMessage Get(Int32 versionId, Int32 dealerId, Int32 cityId)
        {
            PQ_DealerDetailEntity dealerDetailEntity = null;
            DDQDealerDetailBase output = null;

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealers, Bikewale.DAL.AutoBiz.DealersRepository>();
                Bikewale.Interfaces.AutoBiz.IDealers objDealer = container.Resolve<Bikewale.DAL.AutoBiz.DealersRepository>();
                PQParameterEntity objParam = new PQParameterEntity();
                objParam.CityId = Convert.ToUInt32(cityId);
                objParam.DealerId = Convert.ToUInt32(dealerId);
                objParam.VersionId = Convert.ToUInt32(versionId);
                dealerDetailEntity = objDealer.GetDealerDetailsPQ(objParam);
            }

            if (dealerDetailEntity != null)
            {
                output = DDQDealerDetailBaseMapper.Convert(dealerDetailEntity);

                if (dealerDetailEntity.objFacilities != null)
                {
                    dealerDetailEntity.objFacilities.Clear();
                    dealerDetailEntity.objFacilities = null;
                }

                if (dealerDetailEntity.objOffers != null)
                {
                    dealerDetailEntity.objOffers.Clear();
                    dealerDetailEntity.objOffers = null;
                }

                if (dealerDetailEntity.objQuotation != null)
                {
                    if (dealerDetailEntity.objQuotation.Disclaimer != null)
                    {
                        dealerDetailEntity.objQuotation.Disclaimer.Clear();
                        dealerDetailEntity.objQuotation.Disclaimer = null;
                    }

                    if (dealerDetailEntity.objQuotation.objOffers != null)
                    {
                        dealerDetailEntity.objQuotation.objOffers.Clear();
                        dealerDetailEntity.objQuotation.objOffers = null;
                    }

                    if (dealerDetailEntity.objQuotation.PriceList != null)
                    {
                        dealerDetailEntity.objQuotation.PriceList.Clear();
                        dealerDetailEntity.objQuotation.PriceList = null;
                    }

                    dealerDetailEntity.objQuotation.Varients = null;
                }

                return Request.CreateResponse(HttpStatusCode.OK, output);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No dealer price quote found");
            }

        }
    }
}
