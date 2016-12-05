﻿using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Controls;
using Bikewale.DAL.Used;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;

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
        protected Bikewale.Entities.Used.ClassifiedInquiryDetails inquiryDetails = null;
        protected bool isPageNotFound, isPhotoRequestDone;
        protected Bikewale.Mobile.Controls.UploadPhotoRequestPopup widgetUploadPhotoRequest;
        public SimilarUsedBikes ctrlSimilarUsedBikes;
        public OtherUsedBikeByCity ctrlOtherUsedBikes;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected string pgAlternateUrl = string.Empty;

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
                Response.Redirect("/pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 10th Oct 2016
        /// Description : Bind similar and other bike widgets
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added Service center Widget
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

                ctrlOtherUsedBikes.InquiryId = inquiryId;
                ctrlOtherUsedBikes.CityId = inquiryDetails.City.CityId;
                ctrlOtherUsedBikes.ModelId = (uint)inquiryDetails.Model.ModelId;
                ctrlOtherUsedBikes.TopCount = 6;


                ctrlServiceCenterCard.MakeId = Convert.ToUInt32(inquiryDetails.Make.MakeId);
                ctrlServiceCenterCard.CityId = inquiryDetails.City.CityId;
                ctrlServiceCenterCard.makeName = inquiryDetails.Make.MakeName;
                ctrlServiceCenterCard.cityName = inquiryDetails.City.CityName;
                ctrlServiceCenterCard.makeMaskingName = inquiryDetails.Make.MaskingName;
                ctrlServiceCenterCard.cityMaskingName = inquiryDetails.City.CityMaskingName;
                ctrlServiceCenterCard.TopCount = 3;
                ctrlServiceCenterCard.headerText = string.Format("You might want to check {0} service centers in {1}!", inquiryDetails.Make.MakeName, inquiryDetails.City.CityName);
                ctrlServiceCenterCard.biLineText = string.Format("Check out authorized {0} service center nearby", inquiryDetails.Make.MakeName);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " BindUserControls");
                objErr.SendMail();
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
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Used.BikeDetails.BindProfileDetails");
                objErr.SendMail();
            }
        }
    }
}