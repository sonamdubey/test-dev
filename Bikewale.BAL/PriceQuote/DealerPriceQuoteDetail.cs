using Bikewale.DAL.PriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.BAL.PriceQuote
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 March 2016
    /// Description : Implement IDealerPriceQuote interface.
    /// </summary>
    public class DealerPriceQuoteDetail : IDealerPriceQuoteDetail
    {
        private readonly IPriceQuote objPQ = null;
        private readonly IDealerPriceQuote objDPQ = null;
        public DealerPriceQuoteDetail()
        {
            using (IUnityContainer objPQCont = new UnityContainer())
            {
                objPQCont.RegisterType<IPriceQuote, PriceQuoteRepository>();
                objPQCont.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                objPQ = objPQCont.Resolve<IPriceQuote>();
                objDPQ = objPQCont.Resolve<IDealerPriceQuote>();
            }
        }
        /// <summary>
        /// Created By : Lucky Rathore
        /// Created on : 15 March 2016
        /// Description : call API to get reponse for DealerPriceQuote Page.
        /// </summary>
        /// <param name="cityId">e.g. 1</param>
        /// <param name="versionID">e.g. 806</param>
        /// <param name="dealerId">e.g. 12527</param>
        /// <returns>DetailedDealerQuotationEntity entity</returns>
        public DetailedDealerQuotationEntity GetDealerQuotation(UInt32 cityId, UInt32 versionID, UInt32 dealerId)
        {
            DetailedDealerQuotationEntity dealerQuotation = null;
            try
            {
                string _apiUrl = String.Format("/api/v2/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", cityId, versionID, dealerId);
                using (BWHttpClient objClient = new BWHttpClient())
                {
                    dealerQuotation = objClient.GetApiResponseSync<DetailedDealerQuotationEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, dealerQuotation);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "DealerPriceQuoteDetail.GetDealerQuotation");
                objErr.SendMail();
            }
            return dealerQuotation;
        }


        /// <summary>
        /// Created By : Sushil Kumar
        /// Created on : 17th June 2016
        /// Description : call Autobiz API to get reponse for DealerPriceQuote deatils along with secondary dealers having version prices.
        /// </summary>
        /// <param name="cityId">e.g. 1</param>
        /// <param name="versionID">e.g. 806</param>
        /// <param name="dealerId">e.g. 12527</param>
        /// <returns>DetailedDealerQuotationEntity entity</returns>
        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity GetDealerQuotationV2(UInt32 cityId, UInt32 versionID, UInt32 dealerId)
        {
            Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity dealerQuotation = null;
            try
            {
                string _apiUrl = String.Format("/api/v3/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", cityId, versionID, dealerId);
                using (BWHttpClient objClient = new BWHttpClient())
                {
                    dealerQuotation = objClient.GetApiResponseSync<Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, dealerQuotation);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "DealerPriceQuoteDetail.GetDealerQuotationV2");
                objErr.SendMail();
            }
            return dealerQuotation;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 03 Jun 2016
        /// Description :   Quotation
        /// </summary>
        /// <param name="cityId">e.g. 1</param>
        /// <param name="versionID">e.g. 112</param>
        /// <param name="dealerId">e.g. 19886</param>
        /// <returns></returns>
        public PQ_QuotationEntity Quotation(uint cityId, UInt16 sourceType, string deviceId, uint dealerId, uint modelId,ref ulong pqId, bool isPQRegistered, uint? areaId = null, uint? versionId = null)
        {
            PQ_QuotationEntity objDealerPQ = null;
            IList<PQ_BikeVarient> pqVersion = null;
            try
            {
                if (!isPQRegistered)
                {
                    PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                    objPQEntity.CityId = cityId;
                    objPQEntity.AreaId = areaId.HasValue ? areaId.Value : 0;
                    objPQEntity.SourceId = Convert.ToUInt16(sourceType);
                    objPQEntity.ModelId = modelId;
                    objPQEntity.DeviceId = deviceId;
                    objPQEntity.VersionId = versionId.HasValue ? versionId.Value : default(uint);
                    objPQEntity.RefPQId = pqId;
                    objPQEntity.DealerId = dealerId;
                    pqId = objPQ.RegisterPriceQuote(objPQEntity);
                }

                if (dealerId > 0)
                {
                    string api = String.Format("/api/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", cityId, versionId.Value, dealerId);
                    using (Utility.BWHttpClient objDealerPqClient = new Utility.BWHttpClient())
                    {
                        objDealerPQ = objDealerPqClient.GetApiResponseSync<PQ_QuotationEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, api, objDealerPQ);
                    }
                }

                IEnumerable<OtherVersionInfoEntity> versions = objPQ.GetOtherVersionsPrices(pqId);
                pqVersion = new List<PQ_BikeVarient>();

                if (objDealerPQ == null)
                {
                    objDealerPQ = new PQ_QuotationEntity();
                    objDealerPQ.Varients = new List<PQ_BikeVarient>();
                }

                foreach (var version in objDealerPQ.Varients)
                {
                    pqVersion.Add(version);
                }

                foreach (var bwVersion in versions)
                {
                    if (pqVersion.FirstOrDefault(m => m.objVersion.VersionId == bwVersion.VersionId) == null)
                    {
                        List<PQ_Price> pricelist = new List<PQ_Price>();
                        pricelist.Add(new PQ_Price() { CategoryId = 3, CategoryName = "Ex-Showroom", DealerId = 0, Price = bwVersion.Price });
                        pricelist.Add(new PQ_Price() { CategoryId = 5, CategoryName = "RTO", DealerId = 0, Price = bwVersion.RTO });
                        pricelist.Add(new PQ_Price() { CategoryId = 11, CategoryName = "Insurance", DealerId = 0, Price = bwVersion.Insurance });
                        pqVersion.Add(
                            new PQ_BikeVarient()
                            {
                                OnRoadPrice = Convert.ToUInt32(bwVersion.OnRoadPrice),
                                objVersion = new Entities.BikeData.BikeVersionEntityBase() { VersionId = Convert.ToInt32(bwVersion.VersionId), VersionName = bwVersion.VersionName },
                                PriceList = pricelist
                            }
                            );
                    }
                }

                objDealerPQ.Varients = pqVersion;

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerPriceQuoteDetail: " + "Quotation");
                objErr.SendMail();
            }
            return objDealerPQ;
        }
    }
}
