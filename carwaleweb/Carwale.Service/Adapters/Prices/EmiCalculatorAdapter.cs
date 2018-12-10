using AutoMapper;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.PageProperty;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.Template;
using Carwale.Entity.Enum;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Prices;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.Service.Adapters.NewCars
{
    public class EmiCalculatorAdapter : IEmiCalculatorAdapter
    {
        private readonly ICarPriceQuoteAdapter _iPrices;
        private readonly IEmiCalculatorBl _emiCalculatorBl;
        private readonly ITemplate _campaignTemplate;
        private readonly IThirdPartyEmiDetailsCache _thirdPartyEmiDetailsCache;
        private readonly ICarVersionCacheRepository _versionCache;
        private readonly IPrices _prices;
        int[] _vwfsMakeIds = Array.ConvertAll(System.Configuration.ConfigurationManager.AppSettings["VWFSMakeIds"].Split(','), int.Parse);
        private readonly string _imgHostUrl = System.Configuration.ConfigurationManager.AppSettings["CDNHostURL"];

        public EmiCalculatorAdapter(ICarPriceQuoteAdapter iPrices, IEmiCalculatorBl emiCalculatorBl, ITemplate campaignTemplate, IThirdPartyEmiDetailsCache thirdPartyEmiDetailsCache,
            ICarVersionCacheRepository versionCache, IPrices prices)
        {
            _iPrices = iPrices;
            _emiCalculatorBl = emiCalculatorBl;
            _campaignTemplate = campaignTemplate;
            _thirdPartyEmiDetailsCache = thirdPartyEmiDetailsCache;
            _versionCache = versionCache;
            _prices = prices;
        }
        public EmiCalculatorModelData GetEmiData(CarOverviewDTOV2 overview, DealerAdDTO dealerAd, LeadSourceDTO leadSource, int versionORP, int cityId)
        {
            try
            {
                if (overview == null || cityId < 1 || versionORP < 1)
                {
                    return null;
                }

                var versionExShowroom = _iPrices.GetVersionExShowroomPrice(overview.ModelId, overview.VersionId, cityId);
                var emiCalculatorModelData = new EmiCalculatorModelData();
                emiCalculatorModelData.VersionId = overview.VersionId;
                emiCalculatorModelData.MakeName = overview.MakeName;
                emiCalculatorModelData.ModelName = overview.ModelName;
                emiCalculatorModelData.VersionName = overview.VersionName;
                if (versionExShowroom > 0)
                {
                    emiCalculatorModelData.DownPaymentMinValue = versionORP - versionExShowroom;
                    emiCalculatorModelData.DownPaymentMaxValue = versionORP;
                    emiCalculatorModelData.DownPaymentDefaultValue = _emiCalculatorBl.CalculateDownPaymentDefaultValue(emiCalculatorModelData.DownPaymentMinValue, versionExShowroom);
                }
                emiCalculatorModelData.UniqueKey = overview.VersionId.ToString();
                emiCalculatorModelData.DealerAd = dealerAd;

                if (dealerAd != null && leadSource != null)
                {
                    var template = _campaignTemplate.GetEmiCalculatorTemplate();
                    emiCalculatorModelData.DealerAd.PageProperty = new List<PagePropertyDTO> { 
                        new PagePropertyDTO { 
                            Template = Mapper.Map<TemplateDTO>(template) 
                            }
                        };

                    //TODO: This is a temporary fix. It will be removed when DealerAd.PageProperties is used
                    emiCalculatorModelData.CtaDetails = new LeadSourceDTO
                    {
                        LeadClickSourceId = leadSource.LeadClickSourceId,
                        InquirySourceId = leadSource.InquirySourceId
                    };
                }
                emiCalculatorModelData.MakeId = overview.MakeId;
                GetThirdPartyEmiData(ref emiCalculatorModelData, overview.VersionId, emiCalculatorModelData.IsMetallic, overview.MakeId, versionORP);
                return emiCalculatorModelData;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        private void GetThirdPartyEmiData(ref EmiCalculatorModelData emiCalculatorModelData, int versionId, bool isMetallic, int makeId, int versionORP)
        {
            emiCalculatorModelData.IsEligibleForThirdPartyEmi = _vwfsMakeIds.Contains(makeId);
            if (!emiCalculatorModelData.IsEligibleForThirdPartyEmi)
            {
                return;
            }

            var thirdPartyEmiData = _thirdPartyEmiDetailsCache.Get(versionId, isMetallic);
            if (thirdPartyEmiData == null)
            {
                return;
            }

            emiCalculatorModelData.ThirdPartyEmiDetails = new ThirdPartyEmiDetailsDto
            {
                DownPayment = Format.FormatNumericCommaSep((versionORP - thirdPartyEmiData.LoanAmount).ToString()),
                Emi = Format.FormatNumericCommaSep(thirdPartyEmiData.Emi.ToString()),
                LoanAmount = Format.FormatNumericCommaSep(thirdPartyEmiData.LoanAmount.ToString()),
                LumpsumAmount = Format.FormatNumericCommaSep(thirdPartyEmiData.LumpsumAmount.ToString()),
                Tenure = (Math.Round((decimal)(thirdPartyEmiData.TenureInMonth / 12.0) * 10) / 10).ToString(),
                InterestRate = thirdPartyEmiData.InterestRate,
                EmiType = thirdPartyEmiData.EmiType,
                EmiTypeLabel = thirdPartyEmiData.EmiType.ToString()
            };

            if (makeId == 20)
            {
                emiCalculatorModelData.ThirdPartyEmiDetails.ImageUrl = ImageSizes.CreateImageUrl(_imgHostUrl, ImageSizes._0X0, "/cw/es/specials/volkswagen/finance-logos/d/vw-finance-logo.png");
                emiCalculatorModelData.ThirdPartyEmiDetails.WebsiteUrl = "https://www.volkswagen.co.in/app/site/finance-calculator/";
            }
            else if (makeId == 18)
            {
                emiCalculatorModelData.ThirdPartyEmiDetails.ImageUrl = ImageSizes.CreateImageUrl(_imgHostUrl, ImageSizes._0X0, "/cw/es/specials/volkswagen/finance-logos/d/audi-finance-logo.png");
                emiCalculatorModelData.ThirdPartyEmiDetails.WebsiteUrl = "http://emi-calculator.volkswagen-finance.co.in/audi/finance";
            }
            else if (makeId == 15)
            {
                emiCalculatorModelData.ThirdPartyEmiDetails.ImageUrl = ImageSizes.CreateImageUrl(_imgHostUrl, ImageSizes._0X0, "/cw/es/specials/volkswagen/finance-logos/d/skoda-finance-logo.png");
                emiCalculatorModelData.ThirdPartyEmiDetails.WebsiteUrl = "http://www.skoda-auto.co.in/aid/skoda-finance-calculator";
            }

            EmiType emiType = emiCalculatorModelData.ThirdPartyEmiDetails.EmiType;
            emiCalculatorModelData.ThirdPartyEmiDetails.EmiTypeDescription = "Special plan offered by";
            if (emiType == EmiType.Balloon)
            {
                emiCalculatorModelData.ThirdPartyEmiDetails.HeaderText = "Balloon payment of &#x20b9; " + emiCalculatorModelData.ThirdPartyEmiDetails.LumpsumAmount + " at the end of EMI tenure.";
            }
            else if (emiType == EmiType.Bullet)
            {
                emiCalculatorModelData.ThirdPartyEmiDetails.HeaderText = "Bullet payment of &#x20b9; " + emiCalculatorModelData.ThirdPartyEmiDetails.LumpsumAmount + " at the end of each year.";
            }
        }

        public EmiCalculatorModelData GetEmiSummary(int versionId, bool isMetallic, int orp, EmiCalculatorModelData emiCalculatorModelData = null)
        {
            if (emiCalculatorModelData == null)
            {
                emiCalculatorModelData = Mapper.Map<EmiCalculatorModelData>(_versionCache.GetVersionDetailsById(versionId));
            }
            GetThirdPartyEmiData(ref emiCalculatorModelData, versionId, isMetallic, emiCalculatorModelData.MakeId, orp);

            return emiCalculatorModelData;
        }

        public EmiCalculatorDto GetEmiCalculatorData(int versionId, int cityId)
        {
            if (versionId <= 0 || cityId <= 0)
            {
                return null;
            }

            var emiCalculator = new EmiCalculatorDto();
            var carVersionDetails = _versionCache.GetVersionDetailsById(versionId);

            if (carVersionDetails == null)
            {
                return null;
            }

            var versionCompulsoryPrices = _prices.GetVersionCompulsoryPrices(carVersionDetails.ModelId, versionId, cityId, true);

            if (!versionCompulsoryPrices.IsNotNullOrEmpty())
            {
                return null;
            }

            var carOverview = Mapper.Map<CarOverviewDTOV2>(carVersionDetails);

            int solidOrp = (int)versionCompulsoryPrices.Where(c => !c.IsMetallic).Sum(x => x.OnRoadPrice);
            int metallicOrp = (int)versionCompulsoryPrices.Where(c => c.IsMetallic).Sum(x => x.OnRoadPrice);

            var emiCalculatorSolidModelData = GetEmiData(carOverview, null, null, solidOrp, cityId);
            var emiCalculatorMetallicModelData = GetEmiData(carOverview, null, null, metallicOrp, cityId);

            var standardEmiWidgetData = GetStandardEmiWidgetData(emiCalculatorSolidModelData, emiCalculatorMetallicModelData);

            var emiCalculatorSolidData = Mapper.Map<EmiCalculatorModelDataDto>(emiCalculatorSolidModelData);
            var emiCalculatorMetallicData = Mapper.Map<EmiCalculatorModelDataDto>(emiCalculatorMetallicModelData);

            GetThirdPartyEmiTexts(ref emiCalculatorSolidData, carOverview.MakeName);
            GetThirdPartyEmiTexts(ref emiCalculatorMetallicData, carOverview.MakeName);

            if (standardEmiWidgetData != null)
            {
                emiCalculator.EmiWidgetCommonData = standardEmiWidgetData;
            }

            if (emiCalculatorSolidData != null)
            {
                emiCalculator.EmiCalculatorData.Add(emiCalculatorSolidData);
            }
            if (emiCalculatorMetallicData != null)
            {
                emiCalculatorMetallicData.IsMetallic = true;
                emiCalculator.EmiCalculatorData.Add(emiCalculatorMetallicData);
            }
            return emiCalculator;
        }

        private EmiWidgetCommonDataDto GetStandardEmiWidgetData(EmiCalculatorModelData emiCalculatorMetallicModelData, EmiCalculatorModelData emiCalculatorSolidModelData)
        {
            if (emiCalculatorMetallicModelData == null && emiCalculatorSolidModelData == null)
            {
                return null;
            }
            var emiCalculatorModelData = emiCalculatorSolidModelData != null ? emiCalculatorSolidModelData : emiCalculatorMetallicModelData;
            var emiWidgetCommonData = new EmiWidgetCommonDataDto();

            emiWidgetCommonData.InterestMaxValue = 15;
            emiWidgetCommonData.InterestMinValue = 1;
            emiWidgetCommonData.TenureMaxValue = 7;
            emiWidgetCommonData.TenureMinValue = 1;
            emiWidgetCommonData.DownPaymentTooltipText = "This is the minimum upfront payment that you need to make.";
            emiWidgetCommonData.InterestTooltipText = "This is the interest rate at which you can avail your loan.";
            return emiWidgetCommonData;
        }

        private void GetThirdPartyEmiTexts(ref EmiCalculatorModelDataDto emiCalculatorDataDto, string makeName)
        {
            if(emiCalculatorDataDto?.ThirdPartyEmiDetails == null || !makeName.IsNotNullOrEmpty())
            {
                return;
            }

            emiCalculatorDataDto.ThirdPartyEmiDetails.VisitWebsiteText = "Visit Website";
            emiCalculatorDataDto.ThirdPartyEmiDetails.InterestToolTipText = string.Format("Special interest rate as offered by {0} Financial Services", makeName);
            emiCalculatorDataDto.ThirdPartyEmiDetails.TotalDownPaymentTooltipText = "This is the minimum upfront payment that you need to make.";

            if (emiCalculatorDataDto.ThirdPartyEmiDetails.EmiType == EmiType.Balloon)
            {
                emiCalculatorDataDto.ThirdPartyEmiDetails.AmountToolTipText = @"This is the amount that you have to pay at the end of the" +
                                                                                " EMI tenure in addition to monthly payments.";
            }
            else if (emiCalculatorDataDto.ThirdPartyEmiDetails.EmiType == EmiType.Bullet)
            {
                emiCalculatorDataDto.ThirdPartyEmiDetails.AmountToolTipText = @"This is the amount that you have to pay at the end of the" +
                                                                                " every year in addition to monthly payments";
            }
            emiCalculatorDataDto.ThirdPartyEmiDetails.EmiTypeDescription = string.Format("Special plan offered by {0} Financial Services", makeName);

            var headerText = emiCalculatorDataDto.ThirdPartyEmiDetails.HeaderText;

            //Replace rupee symbol code for apps
            emiCalculatorDataDto.ThirdPartyEmiDetails.HeaderText = String.IsNullOrEmpty(headerText) ? "" : headerText.Replace("&#x20b9;", "\u20B9");
            return;
        }
    }
}
