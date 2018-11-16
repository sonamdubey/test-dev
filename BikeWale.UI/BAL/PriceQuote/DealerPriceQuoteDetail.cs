using Bikewale.DAL.AutoBiz;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IPriceQuote _objPQ = null;
        private readonly Bikewale.Interfaces.BikeBooking.IDealerPriceQuote _objDPQ = null;
        private readonly IPriceQuoteCache _objPqCache = null;
        private readonly IDealer _dealer = null;
        public DealerPriceQuoteDetail(IPriceQuote objPQ, Bikewale.Interfaces.BikeBooking.IDealerPriceQuote objDPQ, IPriceQuoteCache objPqCache, IDealer objdealer)
        {
            _objPQ = objPQ;
            _objDPQ = objDPQ;
            _objPqCache = objPqCache;
            _dealer = objdealer;
        }
        /// <summary>
        /// Created By : Lucky Rathore
        /// Created on : 15 March 2016
        /// Description : call API to get reponse for DealerPriceQuote Page.
        /// Modified by :   Sumit Kate on 27 July 2016
        /// Description :   If dealer type is premium and emi is not present send the default values for EMI
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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, DealerPriceQuoteRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                    PQParameterEntity objParam = new PQParameterEntity();
                    objParam.CityId = cityId;
                    objParam.DealerId = dealerId > 0 ? Convert.ToUInt32(dealerId) : default(UInt32);
                    objParam.VersionId = versionID;
                    dealerQuotation = objPriceQuote.GetDealerPriceQuoteByPackage(objParam);

                    if (dealerQuotation != null)
                    {
                        if (dealerQuotation.PrimaryDealer != null && dealerQuotation.PrimaryDealer.DealerDetails != null)
                        {
                            if (dealerQuotation.PrimaryDealer.EMIDetails == null && (dealerQuotation.PrimaryDealer.IsPremiumDealer))
                            {
                                dealerQuotation.PrimaryDealer.EMIDetails = new EMI();
                                dealerQuotation.PrimaryDealer.EMIDetails.MaxDownPayment = 40;
                                dealerQuotation.PrimaryDealer.EMIDetails.MinDownPayment = 10;
                                dealerQuotation.PrimaryDealer.EMIDetails.MaxTenure = 48;
                                dealerQuotation.PrimaryDealer.EMIDetails.MinTenure = 12;
                                dealerQuotation.PrimaryDealer.EMIDetails.MaxRateOfInterest = 15;
                                dealerQuotation.PrimaryDealer.EMIDetails.MinRateOfInterest = 10;
                                dealerQuotation.PrimaryDealer.EMIDetails.ProcessingFee = 2000;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + "DealerPriceQuoteDetail.GetDealerQuotation");

            }
            return dealerQuotation;
        }


        /// <summary>
        /// Created By : Sushil Kumar
        /// Created on : 17th June 2016
        /// Description : call Autobiz API to get reponse for DealerPriceQuote deatils along with secondary dealers having version prices.
        /// Modified By : Prabhu puredla on 18 july 2018
        /// Description : Added logic to get mla dealers
        /// Modified by : Pratibha Verma on 14 August 2018
        /// Description : Added method call to filter out multioutlet dealers
        /// Modifier    : Kartik rathod on 8 nov 2018
        /// Desc        : Handled refernce error for dealerQuotation.PrimaryDealer.DealerDetails.MasterDealerId. added uint masterDealerId 
        /// </summary>
        /// <param name="cityId">e.g. 1</param>
        /// <param name="versionID">e.g. 806</param>
        /// <param name="dealerId">e.g. 12527</param>
        /// <returns>DetailedDealerQuotationEntity entity</returns>
        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity GetDealerQuotationV2(UInt32 cityId, UInt32 versionID, UInt32 dealerId, uint areaId)
        {
            Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity dealerQuotation = null;
            try
            {

                PQParameterEntity objParam = new PQParameterEntity();
                objParam.CityId = cityId;
                objParam.DealerId = dealerId > 0 ? Convert.ToUInt32(dealerId) : default(UInt32);
                objParam.VersionId = versionID;
                objParam.AreaId = areaId;
                dealerQuotation = _objPqCache.GetDealerPriceQuoteByPackageV2(objParam);

                if (dealerQuotation != null)
                {
                    uint masterDealerId = 0;
                    if (dealerQuotation.PrimaryDealer != null && dealerQuotation.PrimaryDealer.DealerDetails != null)
                    {
                        if (dealerQuotation.PrimaryDealer.EMIDetails == null && (dealerQuotation.PrimaryDealer.IsPremiumDealer || dealerQuotation.PrimaryDealer.IsDeluxDealer))
                        {
                            dealerQuotation.PrimaryDealer.EMIDetails = new EMI();
                            dealerQuotation.PrimaryDealer.EMIDetails.MaxDownPayment = 40;
                            dealerQuotation.PrimaryDealer.EMIDetails.MinDownPayment = 10;
                            dealerQuotation.PrimaryDealer.EMIDetails.MaxTenure = 48;
                            dealerQuotation.PrimaryDealer.EMIDetails.MinTenure = 12;
                            dealerQuotation.PrimaryDealer.EMIDetails.MaxRateOfInterest = 15;
                            dealerQuotation.PrimaryDealer.EMIDetails.MinRateOfInterest = 10;
                            dealerQuotation.PrimaryDealer.EMIDetails.ProcessingFee = 2000;

                            masterDealerId = dealerQuotation.PrimaryDealer.DealerDetails.MasterDealerId;
                        }
                    }
                    if (dealerQuotation.objMake != null)
                    {
                        if (dealerQuotation.SecondaryDealers != null && dealerQuotation.SecondaryDealers.Any())
                        {
                            IList<SecondaryDealerBase> MultioutletDealers = new List<SecondaryDealerBase>();
                            foreach (var secondaryDealer in dealerQuotation.SecondaryDealers)
                            {
                                if (areaId == 0 || (areaId > 0 && secondaryDealer.Distance != 0))
                                {
                                    SecondaryDealerBase objDealer = new SecondaryDealerBase()
                                    {
                                        DealerId = secondaryDealer.DealerId,
                                        Area = secondaryDealer.Area,
                                        Name = secondaryDealer.Name,
                                        Distance = secondaryDealer.Distance,
                                        MasterDealerId = secondaryDealer.MasterDealerId
                                    };
                                    MultioutletDealers.Add(objDealer);
                                }
                            }
                            dealerQuotation.MLADealers = _dealer.FilterMultioutletDealers(MultioutletDealers);
                            if (dealerQuotation.MLADealers != null && masterDealerId > 0)
                            {
                                dealerQuotation.MLADealers.RemoveAll(x => x.MasterDealerId == dealerQuotation.PrimaryDealer.DealerDetails.MasterDealerId);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + "DealerPriceQuoteDetail.GetDealerQuotationV2");

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
        public PQ_QuotationEntity Quotation(uint cityId, UInt16 sourceType, string deviceId, uint dealerId, uint modelId, ref ulong pqId, bool isPQRegistered, uint? areaId = null, uint? versionId = null)
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
                    pqId = _objPQ.RegisterPriceQuote(objPQEntity);
                }

                if (dealerId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, DealerPriceQuoteRepository>();
                        Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                        PQParameterEntity objParam = new PQParameterEntity();
                        objParam.CityId = cityId;
                        objParam.DealerId = dealerId;
                        objParam.VersionId = Convert.ToUInt32(versionId.Value);
                        objDealerPQ = objPriceQuote.GetDealerPriceQuote(objParam);
                    }

                }

                IEnumerable<OtherVersionInfoEntity> versions = _objPQ.GetOtherVersionsPrices(pqId);
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
                ErrorClass.LogError(ex, "DealerPriceQuoteDetail: " + "Quotation");

            }
            return objDealerPQ;
        }
    }
}
