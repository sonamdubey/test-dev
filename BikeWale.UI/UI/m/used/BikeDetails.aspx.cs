using System;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.DAL.Used;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Mobile.Controls;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using Bikewale.Common;

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
        protected ServiceCenterCard ctrlServiceCenterCard;
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
            BindProfileDetails();

            if (!isPageNotFound)
            {
                widgetUploadPhotoRequest.ProfileId = profileId;
                widgetUploadPhotoRequest.BikeName = bikeName;
                BindUsedBikePhotos();
                BindUserControls();
                if(inquiryDetails != null)
                {
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
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added Service center Widget
        /// Modified by :- Subodh Jain on 20 march 2017
        /// Summary :-Changed heading used bike widget
        /// </summary>
        private void BindUserControls()
        {
            try
            {
                if(inquiryDetails != null)
                {
                    if(ctrlSimilarUsedBikes != null)
                    {
                        ctrlSimilarUsedBikes.InquiryId = inquiryId;
                        if(inquiryDetails.City != null)
                        {
                            ctrlSimilarUsedBikes.CityId = inquiryDetails.City.CityId;
                            ctrlSimilarUsedBikes.CityMaskingName = inquiryDetails.City.CityMaskingName;
                            ctrlSimilarUsedBikes.CityName = inquiryDetails.City.CityName;
                        }
                        if(inquiryDetails.Model != null)
                        {
                            ctrlSimilarUsedBikes.ModelId = (uint)inquiryDetails.Model.ModelId;
                            ctrlSimilarUsedBikes.ModelName = inquiryDetails.Model.ModelName;
                            ctrlSimilarUsedBikes.ModelMaskingName = inquiryDetails.Model.MaskingName;
                        
                        }
                        if(inquiryDetails.Make != null)
                        {
                            ctrlSimilarUsedBikes.MakeName = inquiryDetails.Make.MakeName;
                            ctrlSimilarUsedBikes.MakeMaskingName = inquiryDetails.Make.MaskingName;
                        }
                        ctrlSimilarUsedBikes.TopCount = 4;
                        ctrlSimilarUsedBikes.BikeName = bikeName;
                        if(inquiryDetails.Make != null && inquiryDetails.Model != null && inquiryDetails.City != null)
                        {
                            ctrlSimilarUsedBikes.WidgetHref = string.Format("/m/used/{0}-{1}-bikes-in-{2}/", inquiryDetails.Make.MaskingName, inquiryDetails.Model.MaskingName, inquiryDetails.City.CityId > 0 ? inquiryDetails.City.CityMaskingName : "india");
                            ctrlSimilarUsedBikes.WidgetTitle = string.Format("More second-hand {0} {1} Bikes in {2}", inquiryDetails.Make.MakeName, inquiryDetails.Model.ModelName, inquiryDetails.City.CityId > 0 ? inquiryDetails.City.CityName : "India");
                        
                        }
                        
                    }
                    if(ctrlServiceCenterCard != null)
                    {
                        if (inquiryDetails.Make != null)
                        {
                            ctrlServiceCenterCard.MakeId = Convert.ToUInt32(inquiryDetails.Make.MakeId);
                            ctrlServiceCenterCard.makeMaskingName = inquiryDetails.Make.MaskingName;
                            ctrlServiceCenterCard.makeName = inquiryDetails.Make.MakeName;
                        }
                        if (inquiryDetails.City != null)
                        {
                            ctrlServiceCenterCard.CityId = inquiryDetails.City.CityId;
                            ctrlServiceCenterCard.cityName = inquiryDetails.City.CityName;
                            ctrlServiceCenterCard.cityMaskingName = inquiryDetails.City.CityMaskingName;
                        }
                        ctrlServiceCenterCard.TopCount = 9;
                        if(inquiryDetails.Make != null && inquiryDetails.City != null)
                        {
                            ctrlServiceCenterCard.widgetHeading = string.Format("{0} service centers in {1}", inquiryDetails.Make.MakeName, inquiryDetails.City.CityName);
                        }
                        
                
                    }
                    if (ctrlusedBikeModel != null)
                    {
                        if(inquiryDetails.City != null)
                        {
                            if (inquiryDetails.City.CityId > 0)
                            {
                                ctrlusedBikeModel.CityId = inquiryDetails.City.CityId;
                            }
                            ctrlusedBikeModel.WidgetTitle = string.Format("Second-hand Bikes in {0}", inquiryDetails.City.CityId > 0 ? inquiryDetails.City.CityName : "India");
                            ctrlusedBikeModel.header = string.Format("More second-hand bikes in {0}", inquiryDetails.City.CityId > 0 ? inquiryDetails.City.CityName : "India");
                            ctrlusedBikeModel.WidgetHref = string.Format("/m/used/bikes-in-{0}/", inquiryDetails.City.CityId > 0 ? inquiryDetails.City.CityMaskingName : "india");
                            ctrlusedBikeModel.TopCount = 9;
                        }
                        
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, Request.ServerVariables["URL"] + " BindUserControls");
                
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
                Bikewale.Notifications.ErrorClass.LogError(ex, "Exception : Bikewale.Mobile.Used.BikeDetails.BindProfileDetails");
                
            }
        }
    }
}