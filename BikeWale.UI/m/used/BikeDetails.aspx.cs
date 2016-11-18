using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.DAL.Used;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Mobile.Controls;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// Created by  : Sushil Kumar on 29th August 2016
    /// Description : Used Bikes Details page code behind
    /// </summary>
    public class BikeDetails : System.Web.UI.Page
    {
        protected uint inquiryId;
        protected string bikeName = string.Empty, pgTitle = string.Empty,
            pgDescription = string.Empty, pgKeywords = string.Empty,
            pgCanonicalUrl = string.Empty, modelYear = string.Empty,
            moreBikeSpecsUrl = string.Empty, moreBikeFeaturesUrl = string.Empty, profileId = string.Empty, adStatus = string.Empty;
        protected BikePhoto firstImage = null;
        protected bool isBikeSold;
        protected ClassifiedInquiryDetails inquiryDetails = null;
        protected UsedBikePhotoGallery ctrlUsedBikeGallery;
        protected Repeater rptUsedBikeNavPhotos, rptUsedBikePhotos;
        protected bool isPageNotFound, isPhotoRequestDone;
        protected UploadPhotoRequestPopup widgetUploadPhotoRequest;
        public SimilarUsedBikes ctrlSimilarUsedBikes;
        public OtherUsedBikeByCity ctrlOtherUsedBikes;

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
                widgetUploadPhotoRequest.ProfileId = profileId;
                widgetUploadPhotoRequest.BikeName = bikeName;
                BindUsedBikePhotos();
                BindUserControls();

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
            else
            {
                Response.Redirect("/pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 29th August 2016
        /// Description : Bind used bikes photos for the used bike
        /// </summary>
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
        /// Created by  : Sangram on 04 Mar 2016
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
        /// Created by  : Sushil Kumar on 30th Aug 2016
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Mobile.Used.BikeDetails.BindProfileDetails");
                objErr.SendMail();
            }
        }
    }
}