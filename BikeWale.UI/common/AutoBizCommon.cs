
using Bikewale.DAL.AutoBiz;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.AutoBiz;
using Microsoft.Practices.Unity;
namespace Bikewale.common
{
    public class AutoBizCommon
    {
        /// <summary>
        /// Created by: Sangram Nandkhile on 01-Jul-2016
        /// Summary: Moving Autobiz dealerPQ API call to Code
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public PQ_QuotationEntity GetDealePQEntity(uint cityId, uint dealerId, uint versionId)
        {
            PQ_QuotationEntity objDealerPrice = default(PQ_QuotationEntity);
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
            return objDealerPrice;
        }
    }
}