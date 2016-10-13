﻿using Bikewale.BindViewModels.Webforms.Used;
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
        protected Bikewale.Entities.Used.ClassifiedInquiryDetails inquiryDetails = null;
        //protected Repeater rptUsedBikeNavPhotos, rptUsedBikePhotos;
        protected bool isPageNotFound, isPhotoRequestDone;
        protected Bikewale.Mobile.Controls.UploadPhotoRequestPopup widgetUploadPhotoRequest = new Bikewale.Mobile.Controls.UploadPhotoRequestPopup();
        public Bikewale.Controls.SimilarUsedBikes ctrlSimilarUsedBikes;
        public Bikewale.Controls.OtherUsedBikeByCity ctrlOtherUsedBikes;
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
            BindProfileDetails();

            if (!isPageNotFound)
            {
                // widgetUploadPhotoRequest.ProfileId = profileId;
                //widgetUploadPhotoRequest.BikeName = bikeName;
                // BindUsedBikePhotos();
                BindUserControls();

                if (inquiryDetails != null && inquiryDetails.PhotosCount == 0)
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
            else
            {
                Response.Redirect("/pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        ///// <summary>
        ///// Created by  : Sushil Kumar on 10th Oct 2016
        ///// Description : Bind used bikes photos for the used bike
        ///// </summary>
        //private void BindUsedBikePhotos()
        //{
        //    if (inquiryDetails != null && inquiryDetails.PhotosCount > 0)
        //    {
        //        rptUsedBikePhotos.DataSource = inquiryDetails.Photo;
        //        rptUsedBikePhotos.DataBind();
        //        rptUsedBikeNavPhotos.DataSource = inquiryDetails.Photo;
        //        rptUsedBikeNavPhotos.DataBind();
        //    }
        //}
        /// <summary>
        /// Created by  : Sushil Kumar on 10th Oct 2016
        /// Description : Bind similar and other bike widgets
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
                ctrlSimilarUsedBikes.TopCount = 4;
                ctrlSimilarUsedBikes.ModelName = inquiryDetails.Model.ModelName;
                ctrlSimilarUsedBikes.ModelMaskingName = inquiryDetails.Model.MaskingName;
                ctrlSimilarUsedBikes.MakeName = inquiryDetails.Make.MakeName;
                ctrlSimilarUsedBikes.MakeMaskingName = inquiryDetails.Make.MaskingName;
                ctrlSimilarUsedBikes.BikeName = bikeName;

                ctrlOtherUsedBikes.InquiryId = inquiryId;
                ctrlOtherUsedBikes.CityId = inquiryDetails.City.CityId;
                ctrlOtherUsedBikes.ModelId = (uint)inquiryDetails.Model.ModelId;
                ctrlOtherUsedBikes.TopCount = 4;
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
                        profileId = string.Format("S{0}", inquiryId);
                        isPageNotFound = usedBikeDetails.IsPageNotFoundRedirection;
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