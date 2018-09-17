using System;
using System.Web;
using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Controls;
using Bikewale.DAL.Used;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using Bikewale.Common;

namespace Bikewale.Used
{
    /// <summary>
    /// Created by  : Sushil Kumar on 10th Oct 2016
    /// Description : Used Bikes Details page code behind desktop
    /// </summary>
    public class BikeDetails : System.Web.UI.Page
    {
        protected uint inquiryId;
        protected string bikeName = string.Empty, pgTitle = string.Empty,
            pgDescription = string.Empty, pgKeywords = string.Empty,
            pgCanonicalUrl = string.Empty, modelYear = string.Empty,
            moreBikeSpecsUrl = string.Empty, moreBikeFeaturesUrl = string.Empty, profileId = string.Empty;
        protected BikePhoto firstImage = null;
        protected bool isBikeSold;
        protected bool isFakeListing;
        protected Bikewale.Entities.Used.ClassifiedInquiryDetails inquiryDetails = null;
        protected bool isPageNotFound, isPhotoRequestDone;
        protected Bikewale.Mobile.Controls.UploadPhotoRequestPopup widgetUploadPhotoRequest;
        public SimilarUsedBikes ctrlSimilarUsedBikes;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected string pgAlternateUrl = string.Empty;
        protected UsedBikeModel ctrlusedBikeModel;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();

            BindProfileDetails();

            if (!isPageNotFound)
            {

                if (inquiryDetails != null)
                {
                    BindUserControls();
                    widgetUploadPhotoRequest.ProfileId = profileId;
                    widgetUploadPhotoRequest.BikeName = bikeName;
                    if (inquiryDetails.PhotosCount == 0)
                    {
                        using (IUnityContainer container = new UnityContainer())
                        {
                            bool isDealer;
                            string inquiryId = "", consumerType = "";
                            CustomerEntityBase buyer = new CustomerEntityBase();

                            BWCookies.GetBuyerDetailsFromCookie(ref buyer);

                            if (buyer.CustomerId > 0)
                            {
                                container.RegisterType<IUsedBikeBuyerRepository, UsedBikeBuyerRepository>();
                                IUsedBikeBuyerRepository _buyerRepo = container.Resolve<IUsedBikeBuyerRepository>();
                                UsedBikeProfileId.SplitProfileId(profileId, out inquiryId, out consumerType);
                                //set bool for dealer listing or individual
                                isDealer = consumerType.Equals("D", StringComparison.CurrentCultureIgnoreCase);

                                isPhotoRequestDone = _buyerRepo.IsPhotoRequestDone(inquiryId, buyer.CustomerId, isDealer);
                            }

                        }
                    }
                }
            }
            else
            {
                UrlRewrite.Return404();
            }
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 10th Oct 2016
        /// Description : Bind similar and other bike widgets
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added Service center Widget
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :-Made count for other used bike 9
        /// Modified by :- Subodh Jain on 20 march 2017
        /// Summary :-Changed heading used bike widget
        /// </summary>
        private void BindUserControls()
        {
            try
            {
                ctrlSimilarUsedBikes.InquiryId = inquiryId;
                ctrlSimilarUsedBikes.CityId = inquiryDetails.City.CityId;
                ctrlSimilarUsedBikes.CityMaskingName = inquiryDetails.City.CityMaskingName;
                ctrlSimilarUsedBikes.CityName = inquiryDetails.City.CityName;
                ctrlSimilarUsedBikes.ModelId = (uint)inquiryDetails.Model.ModelId;
                ctrlSimilarUsedBikes.TopCount = 6;
                ctrlSimilarUsedBikes.ModelName = inquiryDetails.Model.ModelName;
                ctrlSimilarUsedBikes.ModelMaskingName = inquiryDetails.Model.MaskingName;
                ctrlSimilarUsedBikes.MakeName = inquiryDetails.Make.MakeName;
                ctrlSimilarUsedBikes.MakeMaskingName = inquiryDetails.Make.MaskingName;
                ctrlSimilarUsedBikes.BikeName = bikeName;


                ctrlServiceCenterCard.MakeId = Convert.ToUInt32(inquiryDetails.Make.MakeId);
                ctrlServiceCenterCard.CityId = inquiryDetails.City.CityId;
                ctrlServiceCenterCard.makeName = inquiryDetails.Make.MakeName;
                ctrlServiceCenterCard.cityName = inquiryDetails.City.CityName;
                ctrlServiceCenterCard.makeMaskingName = inquiryDetails.Make.MaskingName;
                ctrlServiceCenterCard.cityMaskingName = inquiryDetails.City.CityMaskingName;
                ctrlServiceCenterCard.TopCount = 3;
                ctrlServiceCenterCard.widgetHeading = string.Format("{0} service centers in {1}", inquiryDetails.Make.MakeName, inquiryDetails.City.CityName);
                if (ctrlusedBikeModel != null)
                {
                    if (inquiryDetails.City.CityId > 0)
                        ctrlusedBikeModel.CityId = inquiryDetails.City.CityId;

                    ctrlusedBikeModel.WidgetTitle = string.Format("Used Bikes in {0}", inquiryDetails.City.CityId > 0 ? inquiryDetails.City.CityName : "India");
                    ctrlusedBikeModel.header = string.Format("More second-hand bikes in {0}", inquiryDetails.City.CityId > 0 ? inquiryDetails.City.CityName : "India");
                    ctrlusedBikeModel.WidgetHref = string.Format("/used/bikes-in-{0}/", inquiryDetails.City.CityId > 0 ? inquiryDetails.City.CityMaskingName : "india");
                    ctrlusedBikeModel.TopCount = 9;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, Request.ServerVariables["URL"] + " BindUserControls");

            }
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 10th Oct 2016
        /// Description : Bind profile details for the used bike
        /// </summary>
        private void BindProfileDetails()
        {
            UsedBikeDetailsPage usedBikeDetails = null;
            try
            {
                if (Request["profile"] != null)
                {
                    usedBikeDetails = new UsedBikeDetailsPage();
                    usedBikeDetails.IsValidProfileId(Request.QueryString["profile"]);
                    if (!usedBikeDetails.IsPageNotFoundRedirection)
                    {
                        inquiryId = usedBikeDetails.InquiryId;
                        usedBikeDetails.BindUsedBikeDetailsPage();
                        pgTitle = usedBikeDetails.Title;
                        pgDescription = usedBikeDetails.Description;
                        pgKeywords = usedBikeDetails.Keywords;
                        pgCanonicalUrl = usedBikeDetails.CanonicalUrl;
                        pgAlternateUrl = usedBikeDetails.AlternateUrl;
                        inquiryDetails = usedBikeDetails.InquiryDetails;
                        firstImage = usedBikeDetails.FirstImage;
                        bikeName = usedBikeDetails.BikeName;
                        modelYear = usedBikeDetails.ModelYear;
                        moreBikeSpecsUrl = usedBikeDetails.MoreBikeSpecsUrl;
                        moreBikeFeaturesUrl = usedBikeDetails.MoreBikeFeaturesUrl;
                        profileId = usedBikeDetails.ProfileId;
                        isPageNotFound = usedBikeDetails.IsPageNotFoundRedirection;
                        isBikeSold = usedBikeDetails.IsBikeSold;
                        isFakeListing = usedBikeDetails.IsFakeListing;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Exception : Bikewale.Used.BikeDetails.BindProfileDetails");

            }
        }
    }
}