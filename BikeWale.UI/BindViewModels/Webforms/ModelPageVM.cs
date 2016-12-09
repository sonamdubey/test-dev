using Bikewale.BAL.PriceQuote;
using Bikewale.Entities;
using Bikewale.Interfaces.PriceQuote;
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
        public string Organization { get; set; }
        public string AreaName { get; set; }
        public short SecondaryDealerCount { get; set; }
        public string MaskingNumber { get; set; }
        public IEnumerable<OfferEntityBase> Offers { get; set; }
        public ushort OfferCount { get; set; }
        public IEnumerable<Bikewale.Entities.PriceQuote.v2.NewBikeDealerBase> SecondaryDealersV2 { get; set; }
        public IEnumerable<Bikewale.Entities.PriceQuote.NewBikeDealerBase> SecondaryDealers { get; set; }
        public string MobileNo { get; set; }
        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity DealerCampaignV2 { get; set; }
        public string primaryDealerDistance { get; set; }

        public ModelPageVM(uint cityId, uint versionId, uint dealerId, uint areaId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuoteDetail, DealerPriceQuoteDetail>();
                    IDealerPriceQuoteDetail objIPQ = container.Resolve<IDealerPriceQuoteDetail>();
                    DealerCampaignV2 = objIPQ.GetDealerQuotationV2(cityId, versionId, dealerId, Convert.ToUInt32(areaId));
                    if (DealerCampaignV2 != null)
                    {
                        var dealeDetails = DealerCampaignV2.PrimaryDealer.DealerDetails;
                        if (DealerCampaignV2.PrimaryDealer != null && dealeDetails != null)
                        {
                            if (dealeDetails.DealerPackageType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
                            {
                                IsPremiumDealer = true;
                                Organization = dealeDetails.Organization;
                                if (dealeDetails.objArea != null)
                                    AreaName = dealeDetails.objArea.AreaName;
                                SecondaryDealerCount = Convert.ToInt16(DealerCampaignV2.SecondaryDealerCount);
                                MaskingNumber = dealeDetails.MaskingNumber;
                                Offers = DealerCampaignV2.PrimaryDealer.OfferList;
                                MobileNo = dealeDetails.MobileNo;
                                if (DealerCampaignV2.PrimaryDealer.OfferList != null)
                                    OfferCount = Convert.ToUInt16(DealerCampaignV2.PrimaryDealer.OfferList.Count());
                            }
                            else
                                primaryDealerDistance = dealeDetails.Distance;
                        }

                        if (DealerCampaignV2.SecondaryDealers != null)
                        {
                            SecondaryDealersV2 = DealerCampaignV2.SecondaryDealers;
                            SecondaryDealerCount = Convert.ToInt16(DealerCampaignV2.SecondaryDealerCount);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("Bikewale.BindViewModels.Webforms.ModelPageVM constructor-> cityId:{0}, versionId:{1}, dealerId:{2}, areaId{3}", cityId, versionId, dealerId, areaId));
                objErr.SendMail();
            }
        }
    }
}