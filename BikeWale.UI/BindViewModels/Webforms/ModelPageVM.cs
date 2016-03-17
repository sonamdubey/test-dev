using Bikewale.BAL.PriceQuote;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public int SecondaryDealerCount { get; set; }
        public string MaskingNumber {get;set;}
        
        public ModelPageVM(uint cityId, uint versionId, uint dealerId)
        {
            try
            {
                DealerCampaign = GetDetailedDealer(cityId, versionId, dealerId);
                if (DealerCampaign != null && DealerCampaign.PrimaryDealer!=null && DealerCampaign.PrimaryDealer.DealerDetails != null && DealerCampaign.PrimaryDealer.DealerDetails.DealerPackageType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
                {
                    IsPremiumDealer = true;
                    Organization = DealerCampaign.PrimaryDealer.DealerDetails.Organization;
                    if (DealerCampaign.PrimaryDealer.DealerDetails.objArea != null)
                        AreaName = DealerCampaign.PrimaryDealer.DealerDetails.objArea.AreaName;
                    SecondaryDealerCount = DealerCampaign.SecondaryDealerCount;
                    MaskingNumber = DealerCampaign.PrimaryDealer.DealerDetails.MaskingNumber;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Common.ErrorClass objErr = new Bikewale.Common.ErrorClass(ex, "ModelPageVM constructor");
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
                Bikewale.Common.ErrorClass objErr = new Bikewale.Common.ErrorClass(ex, "GetDetailedDealer");
                objErr.SendMail();
            }
            return detailedDealer;
        }
    }
}