using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Entities.Used;
using Bikewale.Mobile.Controls;
using System;

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
                BindUserControls();
            }
        
            ValidateProfileId();
            BindProfileDetails();
            BindUsedBikePhotos();
        }

        private void BindUsedBikePhotos()
        {
            if (inquiryDetails.PhotosCount > 0)
            {
                rptUsedBikePhotos.DataSource = inquiryDetails.Photo;
                rptUsedBikePhotos.DataBind();

                rptUsedBikeNavPhotos.DataSource = inquiryDetails.Photo;
                rptUsedBikeNavPhotos.DataBind();

            }

        private void BindUserControls()
        {
            ctrlSimilarUsedBikes.InquiryId = 42512;
            ctrlSimilarUsedBikes.CityId = 1;
            ctrlSimilarUsedBikes.ModelId = 197;
            ctrlSimilarUsedBikes.TopCount = 5;
            ctrlSimilarUsedBikes.ModelName = "Duke 200";
            ctrlSimilarUsedBikes.ModelMaskingName = "duke200";
            ctrlSimilarUsedBikes.MakeName = "KTM";
            ctrlSimilarUsedBikes.MakeMaskingName = "ktm";
            ctrlSimilarUsedBikes.BikeName = string.Format("{0} {1}", ctrlSimilarUsedBikes.MakeName, ctrlSimilarUsedBikes.ModelName);



            ctrlOtherUsedBikes.InquiryId = 42512;
            ctrlOtherUsedBikes.CityId = 1;
            ctrlOtherUsedBikes.ModelId = 197;
            ctrlOtherUsedBikes.TopCount = 5;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 04 Mar 2016
        /// Bind profile details for the used bike
        /// </summary>
        private void BindProfileDetails()
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

        /// <summary>
        /// Created By  : Sushil Kumar  on   30th August 2016
        /// Description : Function to validate profile id
        /// </summary>
        void ValidateProfileId()
        {
            try
            {
                if (Request["profile"] != null && Request.QueryString["profile"] != "" && uint.TryParse(Request.QueryString["profile"].Substring(1), out inquiryId))
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
            catch (Exception)
            {

                throw;
            }
        }   //  End of ValidateProfileId
    }
}