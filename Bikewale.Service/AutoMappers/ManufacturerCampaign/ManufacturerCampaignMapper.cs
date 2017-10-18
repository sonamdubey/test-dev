﻿using Bikewale.DTO.Campaign;
using Bikewale.DTO.DealerLocator;
using Bikewale.DTO.PriceQuote.v2;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.Utility;
using System;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.ManufacturerCampaign
{
    internal class ManufacturerCampaignMapper
    {
        /// <summary>
        /// Converts the specified platform identifier.
        /// </summary>
        /// <param name="platformId">The platform identifier.</param>
        /// <param name="pqId">The pq identifier.</param>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="versionId">The version identifier.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="dealers">The dealers.</param>
        /// <param name="manufacturerCampaign">The manufacturer campaign.</param>
        /// <param name="price">The price.</param>
        /// <param name="makeName">Name of the make.</param>
        /// <param name="modelName">Name of the model.</param>
        /// <returns></returns>
        internal static CampaignBaseDto Convert(
            ushort platformId, ulong pqId, uint modelId, uint versionId, uint cityId,
            Entities.PriceQuote.v2.DetailedDealerQuotationEntity dealers, ManufacturerCampaignEntity manufacturerCampaign,
            uint price, string makeName, string modelName)
        {
            CampaignBaseDto campaignResponse = null;
            if (dealers != null)
            {
                campaignResponse = new CampaignBaseDto();
                campaignResponse.DetailsCampaign = new DetailsDto();
                campaignResponse.DetailsCampaign.Dealer = new DealerCampaignBase();
                if (dealers.PrimaryDealer != null && dealers.PrimaryDealer.DealerDetails != null)
                {
                    var dealerOffer = new List<DPQOffer>();
                    foreach (var offer in dealers.PrimaryDealer.OfferList)
                    {
                        var addOffer = new DPQOffer
                        {
                            Id = (int)offer.OfferId,
                            OfferCategoryId = (int)offer.OfferCategoryId,
                            Text = offer.OfferText
                        };
                        dealerOffer.Add(addOffer);
                    }
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
                        campaignResponse.DetailsCampaign.Dealer.CaptionText = String.Format("Authorized dealer in {0}", dealers.PrimaryDealer.DealerDetails.objArea.AreaName);
                    }

                }
                campaignResponse.DetailsCampaign.Dealer.SecondaryDealerCount = (ushort)dealers.SecondaryDealerCount;

            }


            if (manufacturerCampaign != null && manufacturerCampaign.LeadCampaign != null && (dealers == null || dealers.PrimaryDealer == null || dealers.PrimaryDealer.DealerDetails == null))
            {
                ManufactureCampaignLeadEntity LeadCampaign = new ManufactureCampaignLeadEntity
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
                    Organization = manufacturerCampaign.LeadCampaign.Organization,
                    MaskingNumber = manufacturerCampaign.LeadCampaign.MaskingNumber,
                    PincodeRequired = manufacturerCampaign.LeadCampaign.PincodeRequired,
                    PopupDescription = manufacturerCampaign.LeadCampaign.PopupDescription,
                    PopupHeading = manufacturerCampaign.LeadCampaign.PopupHeading,
                    PopupSuccessMessage = manufacturerCampaign.LeadCampaign.PopupSuccessMessage,
                    ShowOnExshowroom = manufacturerCampaign.LeadCampaign.ShowOnExshowroom,
                    PQId = (uint)pqId,
                    VersionId = versionId,
                    PlatformId = 3,
                    BikeName = string.Format("{0} {1}", makeName, modelName),
                };
                campaignResponse = new DTO.Campaign.CampaignBaseDto();
                campaignResponse.DetailsCampaign = new DTO.Campaign.DetailsDto();
                campaignResponse.DetailsCampaign.EsCamapign = new DTO.Campaign.PreRenderCampaignBase();
                campaignResponse.CampaignLeadSource = new DTO.Campaign.ESCampaignBase();
                campaignResponse.DetailsCampaign.EsCamapign.TemplateHtml = MvcHelper.GetRenderedContent(string.Format("LeadCampaign_{0}", LeadCampaign.CampaignId), LeadCampaign.LeadsHtmlDesktop, LeadCampaign);
                campaignResponse.CampaignLeadSource.FloatingBtnText = LeadCampaign.LeadsButtonTextMobile;
                campaignResponse.CampaignLeadSource.CaptionText = LeadCampaign.LeadsPropertyTextMobile;
                campaignResponse.CampaignLeadSource.LeadSourceId = (int)LeadSourceEnum.Model_Mobile;
                campaignResponse.CampaignType = CampaignType.ES;

                if (LeadCampaign.DealerId == Bikewale.Utility.BWConfiguration.Instance.CapitalFirstDealerId)
                {
                    LeadCampaign.LoanAmount = (uint)(System.Convert.ToUInt32(price) * 0.8);

                    LeadCampaign.PageUrl = string.Format("{8}/m/finance/capitalfirst/?campaingid={0}&amp;dealerid={1}&amp;pqid={2}&amp;leadsourceid={3}&amp;versionid={4}&amp;url=&amp;platformid={5}&amp;bike={6}&amp;loanamount={7}", LeadCampaign.CampaignId, LeadCampaign.DealerId, pqId, LeadCampaign.LeadSourceId, versionId, 3, LeadCampaign.BikeName, LeadCampaign.LoanAmount, BWConfiguration.Instance.BwHostUrl);
                }
                else
                {

                    string strDES = string.Format("modelid={0}&cityid={1}&areaid={2}&bikename={3}&location={4}&city={5}&area={6}&ismanufacturer={7}&dealerid={8}&dealername={9}&dealerarea={10}&versionid={11}&leadsourceid={12}&pqsourceid={13}&mfgcampid={14}&pqid={15}&pageurl={16}&clientip={17}&dealerheading={18}&dealermessage={19}&dealerdescription={20}&pincoderequired={21}&emailrequired={22}&dealersrequired={23}", modelId, cityId, string.Empty, string.Format(LeadCampaign.BikeName), string.Empty, string.Empty, string.Empty, true, LeadCampaign.DealerId, String.Format(LeadCampaign.LeadsPropertyTextMobile, LeadCampaign.Organization), LeadCampaign.Area, versionId, LeadCampaign.LeadSourceId, LeadCampaign.PqSourceId, LeadCampaign.CampaignId, LeadCampaign.PQId, string.Empty, string.Empty, LeadCampaign.PopupHeading, String.Format(LeadCampaign.PopupSuccessMessage, LeadCampaign.Organization), LeadCampaign.PopupDescription, LeadCampaign.PincodeRequired, LeadCampaign.EmailRequired, LeadCampaign.DealerRequired);
                    LeadCampaign.PageUrl = string.Format("{0}/m/popup/leadcapture/?q={1}&amp;platformid=3", BWConfiguration.Instance.BwHostUrl, Utils.Utils.EncryptTripleDES(strDES));
                }
                campaignResponse.CampaignLeadSource.LinkUrl = LeadCampaign.PageUrl;
            }

            return campaignResponse;
        }
    }
}