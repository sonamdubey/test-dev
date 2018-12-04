using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.Common;
using Bikewale.DTO.PriceQuote;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AdSlot;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Models.Price;
using Bikewale.Utility;
using ManufacturingCampaign.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;
        private readonly IAdSlot _adSlot = null;
        private readonly IPriceQuoteCache _objPQCache;
        private readonly IManufacturerFinanceCampaign _objManufacturerFinanceCampaign;
        public string RedirectUrl { get; set; }
        public uint OtherTopCount { get; set; }
        public StatusCodes Status { get; set; }
        public string CurrentPageUrl { get; set; }
        public LeadSourceEnum LeadSource { get; set; }
        public PQSources Platform { get; set; }
        public PQSourceEnum PQSource { get; set; }
        public ManufacturerCampaignServingPages ManufacturerCampaignPageId { get; set; }
        private uint _modelId, _versionId, _cityId, _areaId, _dealerId, _makeId;
        private string pageUrl, mpqQueryString, currentCity = string.Empty, currentArea = string.Empty, _pqId = string.Empty, exitUrl;


        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : Resolve unity containers
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added IAdSlot.
        /// </summary>
        public DealerPriceQuotePage(IDealerPriceQuoteDetail objDealerPQDetails, IDealerPriceQuote objDealerPQ, IBikeVersions<BikeVersionEntity, uint> objVersion, IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ, IDealerCacheRepository objDealerCache, IManufacturerCampaign objManufacturerCampaign, IAdSlot adSlot, IPriceQuoteCache objPQCache, IManufacturerFinanceCampaign objManufacturerFinanceCampaign)
        {
            _objDealerPQDetails = objDealerPQDetails;
            _objDealerPQ = objDealerPQ;
            _objVersion = objVersion;
            _objAreaCache = objAreaCache;
            _objCityCache = objCityCache;
            _objPQ = objPQ;
            _objDealerCache = objDealerCache;
            _objManufacturerCampaign = objManufacturerCampaign;
            _adSlot = adSlot;
            _objPQCache = objPQCache;
            _objManufacturerFinanceCampaign = objManufacturerFinanceCampaign;
            ProcessQueryString();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : To get dealerpricequote page data 
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added call to BindAdSlotTags.
        /// Modified by : Rajan Chauhan on 27 June 2018
        /// Description : Moved SetModelVariables above 
        ///		GetDealerVersionsPriceByModelCity to correctly
        ///		set CityId in objData
        /// Modified By : Prabhu Puredla on 10 oct 2018
        /// Description : Get the exitUrl from the querystring
        /// Modified by : Kartik Rathod on 19 oct 2018
        /// Desc        : fetch offerlist in leadcapture to show offers on lead popup
        /// Modified by : Pratibha Verma on 29 November 2018
        /// Description : Added call to bind manufacturer finance campaign
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
                    SetModelVariables(objData);
                    _objPQ.GetDealerVersionsPriceByModelCity(objData.VersionSpecs ,_cityId, _modelId, _dealerId);
                    SetDealerPriceQuoteDetail(objData);
                    BindPageWidgets(objData);
                    SetPageMetas(objData);
                    GetManufacturerCampaign(objData);
                    if (!objData.IsPrimaryDealerAvailable && !objData.IsManufacturerLeadAdShown)
                    {
                        objData.ManufacturerFinanceCampaign = _objManufacturerFinanceCampaign.GetFinanaceCampaigns(_modelId, _cityId);
                    }
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
                            BikeName = objData.BikeName,
                            IsMLAActive = true,
                            MLADealers = objData.DetailedDealer.MLADealers,
                            PlatformId = Convert.ToUInt16(Platform),
                            MlaLeadSourceId = (Platform == PQSources.Desktop) ? (UInt16)LeadSourceEnum.DPQ_MLA_Desktop : (UInt16)LeadSourceEnum.DPQ_MLA_Mobile,
                            PageId = Convert.ToUInt16(PQSource),
                            OfferList = (objData.IsPrimaryDealerAvailable &&  objData.DetailedDealer.PrimaryDealer.HasOffers) 
                                        ? objData.DetailedDealer.PrimaryDealer.OfferList.Select(x => x.OfferText) : (objData.LeadCampaign != null ? objData.LeadCampaign.OffersList : null)
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
                    objData.BhriguPage = BhriguPages.BWPQPage;

                    ShowInnovationBanner(objData, _modelId);
                    BindAdSlotTags(objData);
                    SetTestFlags(objData);
                    if (objData != null && objData.IsOffersShownOnLeadPopup && objData.LeadCapture != null)
                    {
                        objData.LeadCapture.ShowOffersOnLeadPage = objData.LeadCapture.OfferCount > 0;
                    }

                }
                objData.ExitUrl = exitUrl;

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Models.DealerPriceQuotePage.GetData()");
            }

            return objData;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Bind ad slot to adtags.
        /// </summary>
        /// <param name="_objData"></param>
        private void BindAdSlotTags(DealerPriceQuotePageVM _objData)
        {
            try
            {
                if (_objData.AdTags != null)
                {
                    _objData.AdTags.Ad_292x359 = _adSlot.CheckAdSlotStatus("Ad_292x359"); //For similar bikes widget desktop
                    _objData.AdTags.Ad_200x216 = _adSlot.CheckAdSlotStatus("Ad_200x216");  //For similar bikes widget mobile
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "ModelPage.BindAdSlotTags");
            }
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
                Bikewale.Notifications.ErrorClass.LogError(ex, String.Format("ShowInnovationBanner({0})", _modelId));
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
                    objData.AdTags.TargetedCity = currentCity;
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
            var objSimilarBikes = new SimilarBikesWidget(_objVersion, _versionId , PQSourceEnum.Desktop_DPQ_Alternative);
            objSimilarBikes.TopCount = 9;
            objSimilarBikes.CityId = _cityId;
            objData.SimilarBikesVM = objSimilarBikes.GetData();
            if (objData.SimilarBikesVM != null)
            {
                objData.SimilarBikesVM.Page = Entities.Pages.GAPages.DealerPriceQuote_Page;
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
                        objData.Quotation = _objPQ.GetPriceQuote(objData.CityId, objData.VersionId, LeadSource);
                        objData.Quotation.PriceQuoteId = _pqId;
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
        /// Modified By : Rajan Chauhan on 28 Mar 2018
        /// Description : Bind data via Specs Service
        /// </summary>
        private void GetBikeVersions(DealerPriceQuotePageVM objData)
        {
            try
            {
                objData.SelectedVersion = _objVersion.GetById(_versionId);
                if (objData.SelectedVersion != null && objData.SelectedVersion.MakeBase != null && objData.SelectedVersion.ModelBase != null)
                {
                    objData.BikeName = String.Format("{0} {1}", objData.SelectedVersion.MakeBase.MakeName, objData.SelectedVersion.ModelBase.ModelName);
                    _modelId = (uint)objData.SelectedVersion.ModelBase.ModelId;
                    _makeId = (uint)objData.SelectedVersion.MakeBase.MakeId;

                    objData.VersionsList = _objVersion.GetVersionsByType(EnumBikeType.PriceQuote, objData.SelectedVersion.ModelBase.ModelId, (int)_cityId);
                    objData.VersionSpecs = _objVersion.GetVersionMinSpecs(_modelId, true);
                    BikeVersionMinSpecs selectedBikeVersion = objData.VersionSpecs.FirstOrDefault(x => x.VersionId == _versionId);
                    if (selectedBikeVersion != null)
                    {
                        objData.BodyStyle = selectedBikeVersion.BodyStyle;
                        objData.SelectedVersionMinSpecs = selectedBikeVersion.MinSpecsList;
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
        /// Created By : Sushil Kumar
        /// Created On : 16th March 2016
        /// Description : Private Method to proceess mpq queryString and set the for queried parameters viz. versionId,dealerId,cityId,pqId and areaId
        /// Modified By : Lucky Rathore
        /// Description : DealerId Assingment moved in "if" condition
        /// Modified By : Prabhu Puredla on 10 oct 2018
        /// Description : Get the exitUrl from the querystring
        /// </summary>
        private void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            try
            {
                _pqId = PriceQuoteQueryString.PQId;
                if (PriceQuoteQueryString.IsPQQueryStringExists() && UInt32.TryParse(PriceQuoteQueryString.VersionId, out _versionId) && UInt32.TryParse(PriceQuoteQueryString.CityId, out _cityId) && !string.IsNullOrEmpty(_pqId) && _versionId > 0 && _cityId > 0)
                {
                    UInt32.TryParse(PriceQuoteQueryString.DealerId, out _dealerId);
                    UInt32.TryParse(PriceQuoteQueryString.AreaId, out _areaId);
                    pageUrl = request.ServerVariables["URL"];
                    mpqQueryString = request.QueryString["MPQ"];
                    exitUrl = request.QueryString["exitUrl"];
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
        /// Modified by : Ashutosh Sharma on 28 Jun 2018
        /// Description : Removed update of dealer id in pq table. Dealer id is inserted in pq table from the Referrer page of Price Quote page, No need to update here.
        /// Modified by : Rajan Chauhan on 14 Nov 2018
        /// Description : Added ABTest check on Offers
        /// Modified by : Pratibha Verma on 22 November 2018
        /// Description : set pageId in LeadCampaign entity
        /// </summary>
        private void GetManufacturerCampaign(DealerPriceQuotePageVM objData)
        {
            try
            {
                if (_objManufacturerCampaign != null && !(objData.IsPrimaryDealerAvailable))
                {
                    ManufacturerCampaignEntity campaigns = _objManufacturerCampaign.GetCampaigns(_modelId, _cityId, ManufacturerCampaignPageId);
                    if (campaigns!=null)
                    {
                        if (campaigns.LeadCampaign != null)
                        {
                            string campaignTemplate = string.Empty;
                            IEnumerable<string> manufacturerOffersList = null;
                            ushort cookieValue;
                            if (HttpContext.Current.Request.Cookies["_bwtest"] != null && ushort.TryParse(HttpContext.Current.Request.Cookies["_bwtest"].Value, out cookieValue) && cookieValue <= 90 && Platform != PQSources.Desktop)
                                manufacturerOffersList = _objPQCache.GetManufacturerOffers(campaigns.LeadCampaign.CampaignId);
                            if (manufacturerOffersList != null && manufacturerOffersList.Any())
                            {
                                campaignTemplate = _objPQCache.GetManufactuerDefaultCampaignOfferTemplate((ushort)Platform);
                            }
                            else
                            {
                                campaignTemplate = campaigns.LeadCampaign.LeadsHtmlMobile;
                            }
                            objData.LeadCampaign = new Bikewale.Entities.manufacturecampaign.v2.ManufactureCampaignLeadEntity()
                            {
                                Area = GlobalCityArea.GetGlobalCityArea().Area,
                                CampaignId = campaigns.LeadCampaign.CampaignId,
                                DealerId = campaigns.LeadCampaign.DealerId,
                                Organization = campaigns.LeadCampaign.Organization,
                                DealerRequired = campaigns.LeadCampaign.DealerRequired,
                                EmailRequired = campaigns.LeadCampaign.EmailRequired,
                                LeadsButtonTextDesktop = campaigns.LeadCampaign.LeadsButtonTextDesktop,
                                LeadsButtonTextMobile = campaigns.LeadCampaign.LeadsButtonTextMobile,
                                LeadSourceId = (Platform == PQSources.Desktop) ? (int)LeadSourceEnum.DPQ_TopCard_Desktop : (int)LeadSourceEnum.DPQ_TopCard_Mobile,
                                PqSourceId = (int)objData.PQSourcePage,
                                GACategory = "Dealer_PQ",
                                GALabel = string.Format("{0}_{1}", objData.BikeName, currentCity),
                                LeadsHtmlDesktop = campaigns.LeadCampaign.LeadsHtmlDesktop,
                                LeadsHtmlMobile = campaignTemplate,
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
                                LoanAmount = Convert.ToUInt32((objData.TotalPrice) * 0.8),
                                SendLeadSMSCustomer = campaigns.LeadCampaign.SendLeadSMSCustomer,
                                FloatingBtnLeadSourceId = LeadSourceEnum.DPQ_Floating_Mobile,
                                OffersList = manufacturerOffersList,
                                PageId = ManufacturerCampaignPageId
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
                                LeadSourceId = (Platform == PQSources.Desktop) ? (int)LeadSourceEnum.DPQ_EmiCalculator_Desktop : (int)LeadSourceEnum.DPQ_EmiCalculator_Mobile,
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
                                LoanAmount = Convert.ToUInt32((objData.TotalPrice) * 0.8),
                                SendLeadSMSCustomer = campaigns.EMICampaign.SendLeadSMSCustomer
                            };
                            objData.IsManufacturerEMIAdShown = true;
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ModelPage.GetManufacturerCampaign({0},{1},{2})", _modelId, _cityId, ManufacturerCampaignPageId));
            }
        }

        /// <summary>
        /// Author  :   Kartik Rathod on 19 oct 2018
        /// Desc    :   Set Test experiment flags
        /// </summary>
        private void SetTestFlags(DealerPriceQuotePageVM objData)
        {
            ushort cookieValue;
            if (HttpContext.Current.Request.Cookies["_bwtest"] != null && ushort.TryParse(HttpContext.Current.Request.Cookies["_bwtest"].Value, out cookieValue))
            {
                objData.IsOffersShownOnLeadPopup = cookieValue <= 90;
            }
        }
    }
}