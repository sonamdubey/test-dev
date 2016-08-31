using Bikewale.Cache.Core;
using Bikewale.Cache.Used;
using Bikewale.DAL.Used;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Webforms.Used
{
    public class UsedBikeDetailsPage
    {
        public uint InquiryId { get; set; }
        public string BikeName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string CanonicalUrl { get; set; }
        public BikePhoto FirstImage { get; set; }
        public string ModelYear { get; set; }
        public ClassifiedInquiryDetails InquiryDetails = null;
        public string MoreBikeSpecsUrl { get; set; }
        public string MoreBikeFeaturesUrl { get; set; }
        public bool IsPageNotFoundRedirection { get; set; }

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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }

        /// <summary>
        /// Created by  : Sushil Kumar on 29th August 2016
        /// Description : Bind profile details for the used bike
        /// </summary>
        private void GetProfileDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                            ;
                    var objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();

                    InquiryDetails = objCache.GetProfileDetails(InquiryId);
                    if (InquiryDetails != null && InquiryDetails.MinDetails != null)
                    {
                        BikeName = string.Format("{0} {1} {2}", InquiryDetails.Make.MakeName, InquiryDetails.Model.ModelName, InquiryDetails.Version.VersionName);
                        ModelYear = (InquiryDetails.MinDetails.ModelYear != null) ? InquiryDetails.MinDetails.ModelYear.Year.ToString() : string.Empty;
                        Title = string.Format("Used {0} {1} (S{2}) for sale in {3} | BikeWale", ModelYear, BikeName, InquiryDetails.OtherDetails.Id, InquiryDetails.City.CityName);
                        Description = string.Format("used {0}, used {0} for sale, used {0} in {1}", BikeName, InquiryDetails.City.CityName);
                        Keywords = string.Format("BikeWale - Used {0} {1} for sale in {2}. This second hand bike is of {3} model and its profile id is S{4}. Get phone number of the seller and call directly to inspect and test ride the bike.", InquiryDetails.Make.MakeName, InquiryDetails.Model.ModelName, InquiryDetails.City.CityName, ModelYear, InquiryDetails.OtherDetails.Id);
                        MoreBikeSpecsUrl = string.Format("/{0}-bikes/{1}/specifications-features/?vid={2}#specs", InquiryDetails.Make.MaskingName, InquiryDetails.Model.MaskingName, InquiryDetails.Version.VersionId);
                        MoreBikeFeaturesUrl = string.Format("/{0}-bikes/{1}/specifications-features/?vid={2}#features", InquiryDetails.Make.MaskingName, InquiryDetails.Model.MaskingName, InquiryDetails.Version.VersionId);
                        FirstImage = (InquiryDetails.PhotosCount > 0) ? InquiryDetails.Photo.FirstOrDefault() : null;
                    }
                    else
                        IsPageNotFoundRedirection = true;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "GetProfileDetails");
                objErr.SendMail();
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "IsValidProfileId");
                objErr.SendMail();
            }
        }

    }

}