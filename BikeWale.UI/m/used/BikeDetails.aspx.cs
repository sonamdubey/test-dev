﻿using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Entities.Used;
using Bikewale.Mobile.Controls;
using Bikewale.Notifications;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Used
{
    public class BikeDetails : System.Web.UI.Page
    {
        protected uint inquiryId;
        protected string bikeName = string.Empty, pgTitle = string.Empty,
            pgDescription = string.Empty, pgKeywords = string.Empty,
            pgCanonicalUrl = string.Empty, modelYear = string.Empty,
            moreBikeSpecsUrl = string.Empty, moreBikeFeaturesUrl = string.Empty, profileId = string.Empty;
        protected BikePhoto firstImage = null;
        protected ClassifiedInquiryDetails inquiryDetails = null;
        protected UsedBikePhotoGallery ctrlUsedBikeGallery;
        protected Repeater rptUsedBikeNavPhotos, rptUsedBikePhotos;
        private bool _isPageNotFound;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            BindProfileDetails();
            if (inquiryId > 0)
            {
                BindUsedBikePhotos();
            }
            else
            {
                Response.Redirect("/pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void BindUsedBikePhotos()
        {
            if (inquiryDetails.PhotosCount > 1)
            {
                rptUsedBikePhotos.DataSource = inquiryDetails.Photo;
                rptUsedBikePhotos.DataBind();

                rptUsedBikeNavPhotos.DataSource = inquiryDetails.Photo;
                rptUsedBikeNavPhotos.DataBind();

            }

        }

        /// <summary>
        /// Created by  : Sushil Kumar on 04 Mar 2016
        /// Bind profile details for the used bike
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
                        inquiryDetails = usedBikeDetails.InquiryDetails;
                        firstImage = usedBikeDetails.FirstImage;
                        bikeName = usedBikeDetails.BikeName;
                        modelYear = usedBikeDetails.ModelYear;
                        moreBikeSpecsUrl = usedBikeDetails.MoreBikeSpecsUrl;
                        moreBikeFeaturesUrl = usedBikeDetails.MoreBikeFeaturesUrl;
                        profileId = string.Format("S{0}", inquiryId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Mobile.Used.BikeDetails.BindProfileDetails");
                objErr.SendMail();
            }


        }
    }
}