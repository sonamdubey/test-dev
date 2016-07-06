using Bikewale.BAL.PriceQuote;
using Bikewale.DAL.AutoBiz;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BindViewModels.Webforms
{
    /// <summary>
    /// Created by  : Sangram Nandkhile on 16 mar 2016
    /// Summary     : A ViewModel to bind data with Model page view
    /// </summary>
    public class ModelPageVM
    {
        public bool IsPremiumDealer { get; set; }
        public DetailedDealerQuotationEntity DealerCampaign { get; set; }
        public string Organization { get; set; }
        public string AreaName { get; set; }
        public short SecondaryDealerCount { get; set; }
        public string MaskingNumber { get; set; }
        public IEnumerable<OfferEntityBase> Offers { get; set; }
        public ushort OfferCount { get; set; }
        public IEnumerable<Bikewale.Entities.PriceQuote.NewBikeDealerBase> SecondaryDealers { get; set; }
        public string MobileNo { get; set; }

        public ModelPageVM(uint cityId, uint versionId, uint dealerId)
        {
            try
            {
                DealerCampaign = GetDetailedDealer(cityId, versionId, dealerId);
                if (DealerCampaign != null && DealerCampaign.PrimaryDealer != null && DealerCampaign.PrimaryDealer.DealerDetails != null && DealerCampaign.PrimaryDealer.DealerDetails.DealerPackageType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
                {
                    IsPremiumDealer = true;
                    Organization = DealerCampaign.PrimaryDealer.DealerDetails.Organization;
                    if (DealerCampaign.PrimaryDealer.DealerDetails.objArea != null)
                        AreaName = DealerCampaign.PrimaryDealer.DealerDetails.objArea.AreaName;
                    SecondaryDealerCount = Convert.ToInt16(DealerCampaign.SecondaryDealerCount);
                    MaskingNumber = DealerCampaign.PrimaryDealer.DealerDetails.MaskingNumber;
                    Offers = DealerCampaign.PrimaryDealer.OfferList;
                    MobileNo = DealerCampaign.PrimaryDealer.DealerDetails.MobileNo;
                    SecondaryDealers = DealerCampaign.SecondaryDealers;
                    if (DealerCampaign.PrimaryDealer.OfferList != null)
                        OfferCount = Convert.ToUInt16(DealerCampaign.PrimaryDealer.OfferList.Count());
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.ModelPageVM constructor");
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Created by: Sangram Nandkhile on 16 mar 2016
        /// Summary     : API to fetch detailed dealer entity
        /// </summary>
        private DetailedDealerQuotationEntity GetDetailedDealer(uint cityId, uint versionId, uint dealerId)
        {
            DetailedDealerQuotationEntity detailedDealer = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuoteDetail, DealerPriceQuoteDetail>();
                    IDealerPriceQuoteDetail objIPQ = container.Resolve<IDealerPriceQuoteDetail>();
                    detailedDealer = objIPQ.GetDealerQuotation(cityId, versionId, dealerId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.GetDetailedDealer");
                objErr.SendMail();
            }
            return detailedDealer;
        }

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