using Bikewale.BindViewModels.Webforms.Used;
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

        public SimilarUsedBikes ctrlSimilarUsedBikes;
        public OtherUsedBikeByCity ctrlOtherUsedBikes;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidateProfileId();
                BindProfileDetails();
                BindUsedBikePhotos();
                BindUserControls();
            }

        }

        private void BindUsedBikePhotos()
        {
            if (inquiryDetails != null && inquiryDetails.PhotosCount > 0)
            {
                rptUsedBikePhotos.DataSource = inquiryDetails.Photo;
                rptUsedBikePhotos.DataBind();
                rptUsedBikeNavPhotos.DataSource = inquiryDetails.Photo;
                rptUsedBikeNavPhotos.DataBind();
            }
        }
        /// <summary>
        /// Bind similar and other bike widgets
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
        /// Created by  : Sushil Kumar on 04 Mar 2016
        /// Bind profile details for the used bike
        /// </summary>
        private void BindProfileDetails()
        {
            try
            {
                UsedBikeDetailsPage usedBikeDetails = new UsedBikeDetailsPage();
                usedBikeDetails.InquiryId = this.inquiryId;
                usedBikeDetails.BindUsedBikeDetailsPage(rptUsedBikePhotos);
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
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " BindProfileDetails");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar  on   30th August 2016
        /// Description : Function to validate profile id
        /// </summary>
        void ValidateProfileId()
        {
            try
            {
                if (Request["profile"] != null && Request.QueryString["profile"] != string.Empty && uint.TryParse(Request.QueryString["profile"].Substring(1), out inquiryId))
                {

                    if (inquiryId < 1)
                    {
                        Response.Redirect("/pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("/pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " ValidateProfileId");
                objErr.SendMail();
            }
        }   //  End of ValidateProfileId
    }
}