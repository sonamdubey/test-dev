using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Used;
using Bikewale.DAL.Used;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Webforms.Used
{
    /// <summary>
    /// Created by  : Sushil Kumar on 30th August 2016
    /// Description : Used bike details page for the used bike
    /// </summary>
    public class UsedBikeDetailsPage
    {
        public uint InquiryId { get; set; }
        public string BikeName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string CanonicalUrl { get; set; }
        public string AlternateUrl { get; set; }
        public BikePhoto FirstImage { get; set; }
        public string ModelYear { get; set; }
        public ClassifiedInquiryDetails InquiryDetails { get; set; }
        public string MoreBikeSpecsUrl { get; set; }
        public string MoreBikeFeaturesUrl { get; set; }
        public bool IsPageNotFoundRedirection { get; set; }
        public bool IsBikeSold { get; set; }
        public bool IsAdUserLoggedIn { get; set; }
        public string ProfileId { get; set; }
        public bool IsFakeListing { get; set; }

        private string _customerId = string.Empty;
        private IApiGatewayCaller _apiGatewayCaller;
        static readonly IUnityContainer _container;

        static UsedBikeDetailsPage()
        {
            _container = new UnityContainer();
            _container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                     .RegisterType<ICacheManager, MemcacheManager>()
                     .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                     .RegisterType<IApiGatewayCaller, ApiGatewayCaller>()
                    ;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 17th August 2016
        /// Description : Fetch CustomerId from cookie on initialization
        /// </summary>
        public UsedBikeDetailsPage()
        {
            _customerId = Bikewale.Common.CurrentUser.Id;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 29th August 2016
        /// Description : Bind used bike details page for the used bike
        /// </summary>
        public void BindUsedBikeDetailsPage()
        {
            try
            {
                GetProfileDetails();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.Used.UsedBikeDetailsPage.BindUsedBikeDetailsPage");
                
            }

        }

        /// <summary>
        /// Created by  : Sushil Kumar on 29th August 2016
        /// Description : Bind profile details for the used bike
        /// Modified By : Sushil Kumar on 17th August 2016
        /// Description : Redirect to pageNOt found if listing is not approved and not sold and not an ad user (IsPageNotFoundRedirection = (InquiryDetails.AdStatus != 1 && !IsBikeSold) && !IsAdUserLoggedIn)
        /// Modified By : Rajan Chauhan added TyreType in place of BrakeType (removed field)
        /// </summary>
        private void GetProfileDetails()
        {
            try
            {
                
                    var objCache = _container.Resolve<IUsedBikeDetailsCacheRepository>();
                    _apiGatewayCaller = _container.Resolve<IApiGatewayCaller>();

                    InquiryDetails = objCache.GetProfileDetails(InquiryId);
                    if (InquiryDetails != null && InquiryDetails.Version.VersionId > 0)
                    {
                        GetVersionSpecsSummaryByItemIdAdapter adapt = new GetVersionSpecsSummaryByItemIdAdapter();
                        VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
                        {
                            Versions = new List<int>() { InquiryDetails.Version.VersionId },
                            Items = new List<EnumSpecsFeaturesItems> {
                                EnumSpecsFeaturesItems.Displacement,
                                EnumSpecsFeaturesItems.MaxPower,
                                EnumSpecsFeaturesItems.MaximumTorque,
                                EnumSpecsFeaturesItems.NoOfGears,
                                EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                                EnumSpecsFeaturesItems.TyreType,
                                EnumSpecsFeaturesItems.FrontBrakeType,
                                EnumSpecsFeaturesItems.RearBrakeType,
                                EnumSpecsFeaturesItems.WheelType,
                                EnumSpecsFeaturesItems.KerbWeight,
                                EnumSpecsFeaturesItems.TopSpeed,
                                EnumSpecsFeaturesItems.FuelTankCapacity,
                                EnumSpecsFeaturesItems.Speedometer,
                                EnumSpecsFeaturesItems.FuelGuage,
                                EnumSpecsFeaturesItems.Tachometer,
                                EnumSpecsFeaturesItems.DigitalFuelGuage,
                                EnumSpecsFeaturesItems.TripmeterType,
                                EnumSpecsFeaturesItems.StartType,
                            }
                        };
                        adapt.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                        _apiGatewayCaller.Call();
                        IEnumerable<VersionMinSpecsEntity> minSpecs = adapt.Output;
                        if (minSpecs != null && minSpecs.FirstOrDefault() != null && minSpecs.FirstOrDefault().MinSpecsList != null)
                        {
                            IEnumerable<SpecsItem> minSpecsList = minSpecs.FirstOrDefault().MinSpecsList;
                            InquiryDetails.VersionMinSpecs = minSpecsList.ToList();
                        }
                    }
                    if (InquiryDetails != null && InquiryDetails.MinDetails != null)
                    {

                        ProfileId = InquiryDetails.OtherDetails.Seller + InquiryDetails.OtherDetails.Id;
                        IsAdUserLoggedIn = _customerId == InquiryDetails.CustomerId.ToString();
                        BikeName = string.Format("{0} {1} {2}", InquiryDetails.Make.MakeName, InquiryDetails.Model.ModelName, InquiryDetails.Version.VersionName);
                        ModelYear = InquiryDetails.MinDetails.ModelYear.Year.ToString();
                        Title = string.Format("Used {0} {1} ({2}) for sale in {3} | BikeWale", ModelYear, BikeName, ProfileId, InquiryDetails.City.CityName);
                        Keywords = string.Format("used {0}, used {0} for sale, used {0} in {1}", BikeName, InquiryDetails.City.CityName);
                        Description = string.Format("BikeWale - Used {0} {1} for sale in {2}. This second hand bike is of {3} model and its profile id is {4}. Get phone number of the seller and call directly to inspect and test ride the bike.", InquiryDetails.Make.MakeName, InquiryDetails.Model.ModelName, InquiryDetails.City.CityName, ModelYear, ProfileId);
                        MoreBikeSpecsUrl = string.Format("/{0}-bikes/{1}/specifications-features/?vid={2}#specs", InquiryDetails.Make.MaskingName, InquiryDetails.Model.MaskingName, InquiryDetails.Version.VersionId);
                        MoreBikeFeaturesUrl = string.Format("/{0}-bikes/{1}/specifications-features/?vid={2}#features", InquiryDetails.Make.MaskingName, InquiryDetails.Model.MaskingName, InquiryDetails.Version.VersionId);
                        CanonicalUrl = string.Format("https://www.bikewale.com/used/bikes-in-{0}/{1}-{2}-{3}/", InquiryDetails.City.CityMaskingName, InquiryDetails.Make.MaskingName, InquiryDetails.Model.MaskingName, ProfileId).ToLower();
                        AlternateUrl = string.Format("https://www.bikewale.com/m/used/bikes-in-{0}/{1}-{2}-{3}/", InquiryDetails.City.CityMaskingName, InquiryDetails.Make.MaskingName, InquiryDetails.Model.MaskingName, ProfileId).ToLower();
                        FirstImage = (InquiryDetails.PhotosCount > 0) ? InquiryDetails.Photo.FirstOrDefault() : null;
                        IsBikeSold = (InquiryDetails.AdStatus == 3 || InquiryDetails.AdStatus == 6);
                        IsFakeListing = InquiryDetails.AdStatus == 2;
                        IsPageNotFoundRedirection = !IsAdUserLoggedIn && (InquiryDetails.AdStatus != 1 && !IsBikeSold && !IsFakeListing);
                    }
                    else
                        IsPageNotFoundRedirection = true;                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetProfileDetails");
                
            }
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 29th August 2016
        /// Description : Function to do the redirection on different pages.
        /// 
        /// </summary>
        public void IsValidProfileId(string qsProfile)
        {
            try
            {
                uint _inquiryId;
                if (!string.IsNullOrEmpty(qsProfile) && uint.TryParse(qsProfile.Substring(1), out _inquiryId))
                {
                    InquiryId = _inquiryId;
                }
                else
                {
                    IsPageNotFoundRedirection = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "IsValidProfileId");
                
            }
        }

    }

}