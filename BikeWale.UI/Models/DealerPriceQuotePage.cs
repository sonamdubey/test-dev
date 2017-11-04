﻿using Bikewale.Common;
using Bikewale.DTO.PriceQuote;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Models.Price;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Sushil Kumar on 23rd March 2017
    /// Description  :To manage dealerpricequote mobile and desktop page
    /// </summary>
    public class DealerPriceQuotePage
    {

        private readonly IDealerPriceQuoteDetail _objDealerPQDetails = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionCache = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;
        public string RedirectUrl { get; set; }
        public uint OtherTopCount { get; set; }
        public StatusCodes Status { get; set; }
        public string CurrentPageUrl { get; set; }
        public LeadSourceEnum LeadSource { get; set; }
        public PQSources Platform { get; set; }
        public ManufacturerCampaignServingPages ManufacturerCampaignPageId { get; set; }
        private uint _modelId, _versionId, _cityId, _areaId, _pqId, _dealerId, _makeId;
        private string pageUrl, mpqQueryString, currentCity = string.Empty, currentArea = string.Empty;


        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : Resolve unity containers
        /// </summary>
        public DealerPriceQuotePage(IDealerPriceQuoteDetail objDealerPQDetails, IDealerPriceQuote objDealerPQ, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ, IDealerCacheRepository objDealerCache, IManufacturerCampaign objManufacturerCampaign)
        {
            _objDealerPQDetails = objDealerPQDetails;
            _objDealerPQ = objDealerPQ;
            _objVersionCache = objVersionCache;
            _objAreaCache = objAreaCache;
            _objCityCache = objCityCache;
            _objPQ = objPQ;
            _objDealerCache = objDealerCache;
            _objManufacturerCampaign = objManufacturerCampaign;
            ProcessQueryString();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : To get dealerpricequote page data 
        /// </summary>
        /// <returns></returns>
        public DealerPriceQuotePageVM GetData()
        {
            DealerPriceQuotePageVM objData = null;
            try
            {
                objData = new DealerPriceQuotePageVM();
                if (_versionId > 0)
                {
                    GetBikeVersions(objData);
                    SetDealerPriceQuoteDetail(objData);
                    SetModelVariables(objData);
                    BindPageWidgets(objData);
                    SetPageMetas(objData);
                    GetManufacturerCampaign(objData);
                    if (objData.SelectedVersion.New)
                    {
                        objData.LeadCapture = new LeadCaptureEntity()
                        {
                            ModelId = _modelId,
                            CityId = _cityId,
                            AreaId = _areaId,
                            Area = currentArea,
                            City = currentCity,
                            Location = objData.Location,
                            BikeName = objData.BikeName

                        };
                    }
                    if (objData.SimilarBikesVM != null)
                    {
                        objData.SimilarBikesVM.Model = objData.SelectedVersion.ModelBase;
                        objData.SimilarBikesVM.Make = objData.SelectedVersion.MakeBase;
                        objData.SimilarBikesVM.BodyStyle = objData.BodyStyle;
                    }
                    objData.BodyStyleText = objData.BodyStyle.Equals(Entities.GenericBikes.EnumBikeBodyStyles.Scooter) ? "Scooters" : "Bikes";
                    objData.Page = Entities.Pages.GAPages.DealerPriceQuote_Page;

                    ShowInnovationBanner(objData, _modelId);
                }


            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Models.DealerPriceQuotePage.GetData()");
            }

            return objData;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Sep 2017
        /// Description :   To Show Innovation Banner
        /// Enable the innovation banner only for Desktop
        /// </summary>
        /// <param name="_modelId"></param>
        private void ShowInnovationBanner(DealerPriceQuotePageVM objData, uint _modelId)
        {
            try
            {
                if (!String.IsNullOrEmpty
                    (BWConfiguration.Instance.InnovationBannerModels))
                {
                    objData.AdTags.ShowInnovationBannerDesktop = BWConfiguration.Instance.InnovationBannerModels.Split(',').Contains(_modelId.ToString());
                    objData.AdTags.InnovationBannerGALabel = String.Format("{0}_{1}", objData.BikeName.Replace(" ", "_"), "PQ_Page");
                }
            }
            catch (Exception ex)
            {
                var err = new Notifications.ErrorClass(ex, String.Format("ShowInnovationBanner({0})", _modelId));
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : To set Page Meta tags
        /// Modified by :- Subodh Jain 19 june 2017
        /// Summary :- Added TargetModels and Target Make
        /// </summary>
        /// <param name="objData"></param>
        private void SetPageMetas(DealerPriceQuotePageVM objData)
        {
            try
            {
                if (objData != null && objData.SelectedVersion != null && objData.SelectedVersion.MakeBase != null && objData.SelectedVersion.ModelBase != null)
                {
                    objData.PageMetaTags.Title = String.Format("{0} {1} {2} Price Quote", objData.SelectedVersion.MakeBase.MakeName, objData.SelectedVersion.ModelBase.ModelName, objData.SelectedVersion.VersionName);
                    objData.PageMetaTags.ShareImage = Image.GetPathToShowImages(objData.SelectedVersion.OriginalImagePath, objData.SelectedVersion.HostUrl, Bikewale.Utility.ImageSize._360x202);
                    objData.PageMetaTags.Description = String.Format("{0} {1} {2} price quote", objData.SelectedVersion.MakeBase.MakeName, objData.SelectedVersion.ModelBase.ModelName, objData.SelectedVersion.VersionName);
                    objData.AdTags.TargetedMakes = objData.SelectedVersion.MakeBase.MakeName;
                    objData.AdTags.TargetedModel = objData.SelectedVersion.ModelBase.ModelName;
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.DealerPriceQuotePage.SetMetaTags");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : To set dealerpricequote page variables 
        /// </summary>
        /// <param name="objData"></param>
        private void SetModelVariables(DealerPriceQuotePageVM objData)
        {
            try
            {
                objData.DealerId = _dealerId;
                objData.CityId = _cityId;
                objData.AreaId = _areaId;
                objData.PQId = _pqId;
                objData.ClientIP = CommonOpn.GetClientIP();
                objData.Location = GetLocationCookie();
                objData.CiyName = currentCity;
                objData.MPQQueryString = mpqQueryString;
                objData.PageUrl = pageUrl;

                if (objData.SelectedVersion != null && objData.SelectedVersion.MakeBase != null && objData.SelectedVersion.ModelBase != null)
                    objData.BhriguTrackingLabel = string.Format("make={0}|model={1}|version={2}|city={3}", Uri.EscapeDataString(objData.SelectedVersion.MakeBase.MakeName), Uri.EscapeDataString(objData.SelectedVersion.ModelBase.ModelName), Uri.EscapeDataString(objData.SelectedVersion.VersionName), Uri.EscapeDataString(currentCity));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.DealerPriceQuotePage.SetModelVariables()");
            }

        }

        /// <summary>
        /// Created By  : Sushil Kumar on 11th Jan 2016
        /// Description : Bind page related widgets
        /// Modified By :- Subodh Jain 01 march 2017
        /// Summary :- lead capture pop up
        /// Modified by: Vivek Singh Tomar on 23 Aug 2017
        /// Summary: Added page enum to similar bikes widget
        /// </summary>
        private void BindPageWidgets(DealerPriceQuotePageVM objData)
        {

            try
            {
                if (objData != null)
                {

                    DealerCardWidget objDealer = new DealerCardWidget(_objDealerCache, _cityId, _makeId);
                    objDealer.TopCount = OtherTopCount;
                    objData.OtherDealers = objDealer.GetData();

                    BindSimilarBikes(objData);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.DealerPriceQuotePage.BindPageWidgets");
            }

        }

        private void BindSimilarBikes(DealerPriceQuotePageVM objData)
        {
            var objSimilarBikes = new SimilarBikesWidget(_objVersionCache, _versionId, PQSourceEnum.Desktop_DPQ_Alternative);
            if (objSimilarBikes != null)
            {
                objSimilarBikes.TopCount = 9;
                objSimilarBikes.CityId = _cityId;
                objData.SimilarBikesVM = objSimilarBikes.GetData();
                if (objData.SimilarBikesVM != null)
                {
                    objData.SimilarBikesVM.Page = Entities.Pages.GAPages.DealerPriceQuote_Page;
                }
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 15th March 2016
        /// Description : To set user location
        /// Modified By : Aditi srivastava on 17 Nov 2016
        /// Description : get city area name from global city
        /// Modified By :Sushil Kumar on 12th Jan 2017
        /// Description : To set city area from bikewale common utility function
        /// </summary>
        /// <returns></returns>
        private string GetLocationCookie()
        {
            string location = String.Empty;

            try
            {
                var cities = _objCityCache.GetPriceQuoteCities(_modelId);

                if (cities != null)
                {
                    Entities.Location.CityEntityBase city = cities.FirstOrDefault(m => m.CityId == _cityId);
                    currentCity = city != null ? city.CityName : String.Empty;


                    IEnumerable<Entities.Location.AreaEntityBase> areas = _objAreaCache.GetAreaList(_modelId, _cityId);
                    if (areas != null)
                    {
                        Entities.Location.AreaEntityBase area = areas.FirstOrDefault(m => m.AreaId == _areaId);
                        if (area != null)
                        {
                            currentArea = area != null ? area.AreaName : String.Empty;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(currentCity))
                {
                    if (!string.IsNullOrEmpty(currentArea))
                    {
                        location = String.Format("<span>{0}</span>, <span>{1}</span>", currentArea, currentCity);
                    }
                    else
                    {
                        location = String.Format("<span>{0}</span>", currentCity);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.DealerPriceQuotePage.GetLocationCookie");
            }

            return location;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created on : 15 March 2016
        /// Description : for Dealer Basics details.
        /// Modified By : Sushil Kumar on 17th March 2016
        /// Description  : Added default values for emi if no emi details is available
        /// Modifide By :- Subodh jain on 02 March 2017
        /// Summary:- added manufacturer campaign leadpopup changes
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        /// <param name="objData"></param>
        private void SetDealerPriceQuoteDetail(DealerPriceQuotePageVM objData)
        {
            Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity detailedDealer = null;
            bool isBikewalePQ = false;
            try
            {
                detailedDealer = _objDealerPQDetails.GetDealerQuotationV2(_cityId, _versionId, _dealerId, _areaId);
                if (detailedDealer != null)
                {
                    if (detailedDealer.PrimaryDealer != null && detailedDealer.PrimaryDealer.DealerDetails != null)
                    {

                        #region Set Dealer Details
                        if (string.IsNullOrEmpty(detailedDealer.PrimaryDealer.DealerDetails.DisplayTextLarge))
                            detailedDealer.PrimaryDealer.DealerDetails.DisplayTextLarge = objData.LeadBtnLongText;
                        if (string.IsNullOrEmpty(detailedDealer.PrimaryDealer.DealerDetails.DisplayTextSmall))
                            detailedDealer.PrimaryDealer.DealerDetails.DisplayTextSmall = objData.LeadBtnShortText;

                        //EMI details
                        #region Set EMI Details
                        var objEMI = detailedDealer.PrimaryDealer.EMIDetails;
                        if (objEMI != null)
                        {
                            var _objEMI = setDefaultEMIDetails();
                            if (objEMI.MinDownPayment < 1 || objEMI.MaxDownPayment < 1)
                            {
                                objEMI.MinDownPayment = _objEMI.MinDownPayment;
                                objEMI.MaxDownPayment = _objEMI.MaxDownPayment;
                            }

                            if (objEMI.MinTenure < 1 || objEMI.MaxTenure < 1)
                            {
                                objEMI.MinTenure = _objEMI.MinTenure;
                                objEMI.MaxTenure = _objEMI.MaxTenure;
                            }

                            if (objEMI.MinRateOfInterest < 1 || objEMI.MaxRateOfInterest < 1)
                            {
                                objEMI.MinRateOfInterest = _objEMI.MinRateOfInterest;
                                objEMI.MaxRateOfInterest = _objEMI.MaxRateOfInterest;
                            }
                        }
                        else
                        {
                            objEMI = setDefaultEMIDetails();
                        }

                        detailedDealer.PrimaryDealer.EMIDetails = objEMI;

                        #endregion
                        #endregion
                        if (detailedDealer.PrimaryDealer.PriceList != null && detailedDealer.PrimaryDealer.PriceList.Any())
                        {
                            #region Dealer PriceQuote
                            objData.TotalPrice = (uint)detailedDealer.PrimaryDealer.TotalPrice;
                            #endregion
                        }
                        else { isBikewalePQ = true; }
                    }
                    else
                    {
                        isBikewalePQ = true;
                    }

                    if (isBikewalePQ)
                    {
                        #region Bikewale PriceQuote
                        objData.Quotation = _objPQ.GetPriceQuoteById(Convert.ToUInt64(_pqId), LeadSource);
                        if (objData.Quotation != null)
                        {
                            objData.TotalPrice = (uint)objData.Quotation.OnRoadPrice;
                        }
                        #endregion
                    }

                }
                else
                {
                    RedirectUrl = "/pricequote/";
                    Status = StatusCodes.RedirectPermanent;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.SetDealerPriceQuoteDetail(): versionId {0}", _versionId));
            }

            objData.DetailedDealer = detailedDealer;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 2 Dec 2014
        /// Summary : To Fill version dropdownlist
        /// Created By : Sangram Nandkhile on 16 Dec 2016
        /// Summary : Fetch minspecs and show on DPQ
        /// </summary>
        private void GetBikeVersions(DealerPriceQuotePageVM objData)
        {
            IEnumerable<BikeVersionMinSpecs> minSpecs = null;
            try
            {

                objData.SelectedVersion = _objVersionCache.GetById(_versionId);
                if (objData.SelectedVersion != null && objData.SelectedVersion.MakeBase != null && objData.SelectedVersion.ModelBase != null)
                {
                    objData.BikeName = String.Format("{0} {1}", objData.SelectedVersion.MakeBase.MakeName, objData.SelectedVersion.ModelBase.ModelName);
                    _modelId = (uint)objData.SelectedVersion.ModelBase.ModelId;
                    _makeId = (uint)objData.SelectedVersion.MakeBase.MakeId;

                    objData.VersionsList = _objVersionCache.GetVersionsByType(EnumBikeType.PriceQuote, objData.SelectedVersion.ModelBase.ModelId, (int)_cityId);

                    if (objData.VersionsList != null)
                    {
                        minSpecs = _objVersionCache.GetVersionMinSpecs(_modelId, true);
                        var objMin = minSpecs.FirstOrDefault(x => x.VersionId == _versionId);
                        if (objMin != null)
                        {
                            objData.MinSpecsHtml = FormatVarientMinSpec(objMin);
                            objData.BodyStyle = objMin.BodyStyle;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.GetBikeVersions() versionId {0}", _versionId));
            }
        }

        /// <summary>
        /// Created BY : Sushil Kumar on 14th March 2015
        /// Summary : To set EMI details for the dealer if no EMI Details available for the dealer
        /// </summary>
        private EMI setDefaultEMIDetails()
        {
            EMI _objEMI = null;
            try
            {
                _objEMI = new EMI();
                _objEMI.MaxDownPayment = 40;
                _objEMI.MinDownPayment = 10;
                _objEMI.MaxTenure = 48;
                _objEMI.MinTenure = 12;
                _objEMI.MaxRateOfInterest = 15;
                _objEMI.MinRateOfInterest = 10;
                _objEMI.ProcessingFee = 2000;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.SetEMIDetails(): versionId {0}", _versionId));
            }
            return _objEMI;
        }

        /// <summary>
        /// Createdby: Sangram Nandkhile on 15 Dec 2016
        /// Summary: Format Minspecs for each version
        /// </summary>
        /// <returns></returns>
        private string FormatVarientMinSpec(BikeVersionMinSpecs objVersion)
        {
            string minSpecsStr = string.Empty;

            try
            {
                minSpecsStr = string.Format("{0}<li>{1} Wheels</li>", minSpecsStr, objVersion.AlloyWheels ? "Alloy" : "Spoke");
                minSpecsStr = string.Format("{0}<li>{1} Start</li>", minSpecsStr, objVersion.ElectricStart ? "Electric" : "Kick");

                if (objVersion.AntilockBrakingSystem)
                {
                    minSpecsStr = string.Format("{0}<li>ABS</li>", minSpecsStr);
                }

                if (!String.IsNullOrEmpty(objVersion.BrakeType))
                {
                    minSpecsStr = string.Format("{0}<li>{1} Brake</li>", minSpecsStr, objVersion.BrakeType);
                }


                if (!string.IsNullOrEmpty(minSpecsStr))
                {
                    minSpecsStr = string.Format("<ul id='version-specs-list'>{0}</ul>", minSpecsStr);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.FormatVarientMinSpec(): versionId {0}", _versionId));
            }

            return minSpecsStr;

        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 16th March 2016
        /// Description : Private Method to proceess mpq queryString and set the for queried parameters viz. versionId,dealerId,cityId,pqId and areaId
        /// Modified By : Lucky Rathore
        /// Description : DealerId Assingment moved in "if" condition
        /// </summary>
        private void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            try
            {

                if (PriceQuoteQueryString.IsPQQueryStringExists() && UInt32.TryParse(PriceQuoteQueryString.PQId, out _pqId) && UInt32.TryParse(PriceQuoteQueryString.VersionId, out _versionId) && UInt32.TryParse(PriceQuoteQueryString.CityId, out _cityId) && _pqId > 0 && _versionId > 0 && _cityId > 0)
                {
                    UInt32.TryParse(PriceQuoteQueryString.DealerId, out _dealerId);
                    UInt32.TryParse(PriceQuoteQueryString.AreaId, out _areaId);
                    pageUrl = request.ServerVariables["URL"];
                    mpqQueryString = request.QueryString["MPQ"];
                    Status = StatusCodes.ContentFound;
                }
                else
                {
                    RedirectUrl = "/pricequote/";
                    Status = StatusCodes.RedirectPermanent;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.ParseQueryString() versionid {0}, CityId {1}, PQId {2}", _versionId, _cityId, _pqId));
                Status = StatusCodes.ContentNotFound;
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Jun 2017
        /// Description :   Fetches Manufacturer Campaigns
        /// Modified by  :  Sushil Kumar on 11th Aug 2017
        /// Description :   Store dealerid for manufacturer campaigns for impressions tracking
        /// </summary>
        private void GetManufacturerCampaign(DealerPriceQuotePageVM objData)
        {
            try
            {
                if (_objManufacturerCampaign != null && !(objData.IsPrimaryDealerAvailable))
                {
                    ManufacturerCampaignEntity campaigns = _objManufacturerCampaign.GetCampaigns(_modelId, _cityId, ManufacturerCampaignPageId);
                    if (campaigns.LeadCampaign != null)
                    {
                        objData.LeadCampaign = new ManufactureCampaignLeadEntity()
                        {
                            Area = GlobalCityArea.GetGlobalCityArea().Area,
                            CampaignId = campaigns.LeadCampaign.CampaignId,
                            DealerId = campaigns.LeadCampaign.DealerId,
                            Organization = campaigns.LeadCampaign.Organization,
                            DealerRequired = campaigns.LeadCampaign.DealerRequired,
                            EmailRequired = campaigns.LeadCampaign.EmailRequired,
                            LeadsButtonTextDesktop = campaigns.LeadCampaign.LeadsButtonTextDesktop,
                            LeadsButtonTextMobile = campaigns.LeadCampaign.LeadsButtonTextMobile,
                            LeadSourceId = (int)LeadSource,
                            PqSourceId = (int)objData.PQSourcePage,
                            GACategory = "Dealer_PQ",
                            GALabel = string.Format("{0}_{1}", objData.BikeName, currentCity),
                            LeadsHtmlDesktop = campaigns.LeadCampaign.LeadsHtmlDesktop,
                            LeadsHtmlMobile = campaigns.LeadCampaign.LeadsHtmlMobile,
                            LeadsPropertyTextDesktop = campaigns.LeadCampaign.LeadsPropertyTextDesktop,
                            LeadsPropertyTextMobile = campaigns.LeadCampaign.LeadsPropertyTextMobile,
                            PriceBreakUpLinkDesktop = campaigns.LeadCampaign.PriceBreakUpLinkDesktop,
                            PriceBreakUpLinkMobile = campaigns.LeadCampaign.PriceBreakUpLinkMobile,
                            PriceBreakUpLinkTextDesktop = campaigns.LeadCampaign.PriceBreakUpLinkTextDesktop,
                            PriceBreakUpLinkTextMobile = campaigns.LeadCampaign.PriceBreakUpLinkTextMobile,

                            MakeName = objData.SelectedVersion.MakeBase.MakeName,
                            MaskingNumber = campaigns.LeadCampaign.MaskingNumber,
                            PincodeRequired = campaigns.LeadCampaign.PincodeRequired,
                            PopupDescription = campaigns.LeadCampaign.PopupDescription,
                            PopupHeading = campaigns.LeadCampaign.PopupHeading,
                            PopupSuccessMessage = campaigns.LeadCampaign.PopupSuccessMessage,
                            PQId = objData.PQId,
                            VersionId = objData.VersionId,
                            CurrentPageUrl = CurrentPageUrl,
                            PlatformId = (ushort)Platform,
                            BikeName = objData.BikeName,
                            LoanAmount = Convert.ToUInt32((objData.TotalPrice) * 0.8)
                        };
                        objData.IsManufacturerLeadAdShown = true;
                    }
                    if (campaigns.EMICampaign != null)
                    {
                        objData.EMICampaign = new ManufactureCampaignEMIEntity()
                        {
                            Area = GlobalCityArea.GetGlobalCityArea().Area,
                            CampaignId = campaigns.EMICampaign.CampaignId,
                            DealerId = campaigns.EMICampaign.DealerId,
                            Organization = campaigns.EMICampaign.Organization,
                            DealerRequired = campaigns.EMICampaign.DealerRequired,
                            EmailRequired = campaigns.EMICampaign.EmailRequired,
                            EMIButtonTextDesktop = campaigns.EMICampaign.EMIButtonTextDesktop,
                            EMIButtonTextMobile = campaigns.EMICampaign.EMIButtonTextMobile,
                            LeadSourceId = (int)LeadSource,
                            PqSourceId = (int)objData.PQSourcePage,
                            EMIPropertyTextDesktop = campaigns.EMICampaign.EMIPropertyTextDesktop,
                            EMIPropertyTextMobile = campaigns.EMICampaign.EMIPropertyTextMobile,
                            MakeName = objData.SelectedVersion.MakeBase.MakeName,
                            MaskingNumber = campaigns.EMICampaign.MaskingNumber,
                            PincodeRequired = campaigns.EMICampaign.PincodeRequired,
                            PopupDescription = campaigns.EMICampaign.PopupDescription,
                            PopupHeading = campaigns.EMICampaign.PopupHeading,
                            PopupSuccessMessage = campaigns.EMICampaign.PopupSuccessMessage,
                            VersionId = objData.VersionId,
                            CurrentPageUrl = CurrentPageUrl,
                            PlatformId = (ushort)Platform,
                            LoanAmount = Convert.ToUInt32((objData.TotalPrice) * 0.8)
                        };
                        objData.IsManufacturerEMIAdShown = true;
                    }

                    if (objData.IsManufacturerLeadAdShown)
                    {
                        _objManufacturerCampaign.SaveManufacturerIdInPricequotes(objData.PQId, campaigns.LeadCampaign.DealerId);
                    }
                    else if (objData.IsManufacturerEMIAdShown)
                    {
                        _objManufacturerCampaign.SaveManufacturerIdInPricequotes(objData.PQId, campaigns.EMICampaign.DealerId);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ModelPage.GetManufacturerCampaign({0},{1},{2})", _modelId, _cityId, ManufacturerCampaignPageId));
            }
        }
    }
}