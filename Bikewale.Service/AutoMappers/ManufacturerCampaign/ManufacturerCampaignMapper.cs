using Bikewale.DTO.Campaign;
using Bikewale.DTO.DealerLocator;
using Bikewale.DTO.PriceQuote.v2;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.Utility;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.pqEntity.ManufacturerCampaignManufacturerCampaign
{
    public class ManufacturerCampaignMapper
    {
        /// <summary>
        /// Converts the specified dealers.
        /// </summary>
        /// <param name="dealers">The dealers.</param>
        /// <param name="manufacturerCampaign">The manufacturer campaign.</param>
        /// <returns></returns>
        internal static CampaignBaseDto Convert(Entities.PriceQuote.v2.DetailedDealerQuotationEntity dealers, ManufacturerCampaignEntity manufacturerCampaign)
        {
            CampaignBaseDto campaignResponse = null;
            if (dealers != null)
            {
                if (dealers.PrimaryDealer != null)
                {
                    var dealerOffer = new List<DPQOffer>();
                    foreach (var offer in dealers.PrimaryDealer.OfferList)
                    {
                        var addOffer = new DPQOffer()
                        {
                            Id = (int)offer.OfferId,
                            OfferCategoryId = (int)offer.OfferCategoryId,
                            Text = offer.OfferText
                        };
                        dealerOffer.Add(addOffer);
                    }
                    campaignResponse = new CampaignBaseDto();
                    campaignResponse.DetailsCampaign = new DetailsDto();
                    campaignResponse.DetailsCampaign.Dealer = new DealerCampaignBase();
                    campaignResponse.DetailsCampaign.Dealer.Offers = dealerOffer;
                    if (dealers.PrimaryDealer.DealerDetails != null)
                    {
                        campaignResponse.CampaignType = CampaignType.DS;
                        campaignResponse.DetailsCampaign.Dealer.PrimaryDealer = new DealerBase();
                        campaignResponse.DetailsCampaign.Dealer.PrimaryDealer.Name = dealers.PrimaryDealer.DealerDetails.Organization;
                        campaignResponse.DetailsCampaign.Dealer.PrimaryDealer.MaskingNumber = dealers.PrimaryDealer.DealerDetails.MaskingNumber;
                        campaignResponse.DetailsCampaign.Dealer.PrimaryDealer.Area = dealers.PrimaryDealer.DealerDetails.objArea.AreaName;
                        campaignResponse.DetailsCampaign.Dealer.PrimaryDealer.DealerId = dealers.PrimaryDealer.DealerDetails.DealerId;
                        campaignResponse.DetailsCampaign.Dealer.PrimaryDealer.DealerPkgType = (DTO.PriceQuote.DealerPackageType)dealers.PrimaryDealer.DealerDetails.DealerPackageType;
                        campaignResponse.DetailsCampaign.Dealer.IsPremium = dealers.PrimaryDealer.IsPremiumDealer;
                    }

                }
                campaignResponse.DetailsCampaign.Dealer.SecondaryDealerCount = (ushort)dealers.SecondaryDealerCount;

            }
            else if (manufacturerCampaign != null && manufacturerCampaign.LeadCampaign != null)
            {
                ManufactureCampaignLeadEntity LeadCampaign = new ManufactureCampaignLeadEntity()
                {
                    Area = GlobalCityArea.GetGlobalCityArea().Area,
                    CampaignId = manufacturerCampaign.LeadCampaign.CampaignId,
                    DealerId = manufacturerCampaign.LeadCampaign.DealerId,
                    DealerRequired = manufacturerCampaign.LeadCampaign.DealerRequired,
                    EmailRequired = manufacturerCampaign.LeadCampaign.EmailRequired,
                    LeadsButtonTextDesktop = manufacturerCampaign.LeadCampaign.LeadsButtonTextDesktop,
                    LeadsButtonTextMobile = manufacturerCampaign.LeadCampaign.LeadsButtonTextMobile,
                    LeadSourceId = (int)LeadSourceEnum.Model_Mobile,
                    PqSourceId = (int)PQSourceEnum.Mobile_ModelPage,
                    LeadsHtmlDesktop = manufacturerCampaign.LeadCampaign.LeadsHtmlDesktop,
                    LeadsHtmlMobile = manufacturerCampaign.LeadCampaign.LeadsHtmlMobile,
                    LeadsPropertyTextDesktop = manufacturerCampaign.LeadCampaign.LeadsPropertyTextDesktop,
                    LeadsPropertyTextMobile = manufacturerCampaign.LeadCampaign.LeadsPropertyTextMobile,
                    //MakeName = objModelPage.ModelDetails.MakeBase.MakeName,
                    Organization = manufacturerCampaign.LeadCampaign.Organization,
                    MaskingNumber = manufacturerCampaign.LeadCampaign.MaskingNumber,
                    PincodeRequired = manufacturerCampaign.LeadCampaign.PincodeRequired,
                    PopupDescription = manufacturerCampaign.LeadCampaign.PopupDescription,
                    PopupHeading = manufacturerCampaign.LeadCampaign.PopupHeading,
                    PopupSuccessMessage = manufacturerCampaign.LeadCampaign.PopupSuccessMessage,
                    ShowOnExshowroom = manufacturerCampaign.LeadCampaign.ShowOnExshowroom,
                    //PQId = (uint)pqEntity.PqId,
                   // VersionId = objModelPage.ModelVersionSpecs.BikeVersionId,
                    PlatformId = 3,
                    //BikeName = string.Format("{0} {1}", objModelPage.ModelDetails.MakeBase.MakeName, objModelPage.ModelDetails.ModelName),
                };
                campaignResponse = new DTO.Campaign.CampaignBaseDto();
                campaignResponse.DetailsCampaign = new DTO.Campaign.DetailsDto();
                campaignResponse.DetailsCampaign.EsCamapign = new DTO.Campaign.PreRenderCampaignBase();
                campaignResponse.CampaignLeadSource = new DTO.Campaign.ESCampaignBase();
                campaignResponse.DetailsCampaign.EsCamapign.TemplateHtml = Format.GetRenderedContent(string.Format("LeadCampaign_{0}", LeadCampaign.CampaignId), LeadCampaign.LeadsHtmlDesktop, LeadCampaign);
                campaignResponse.CampaignLeadSource.FloatingBtnText = LeadCampaign.LeadsButtonTextMobile;
                campaignResponse.CampaignLeadSource.CaptionText = LeadCampaign.LeadsPropertyTextMobile;
                campaignResponse.CampaignLeadSource.LeadSourceId = (int)LeadSourceEnum.Model_Mobile;
                campaignResponse.CampaignType = CampaignType.ES;
            }

            return campaignResponse;
        }
    }
}