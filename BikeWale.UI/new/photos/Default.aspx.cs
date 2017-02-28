using Bikewale.BindViewModels.Controls;
using Bikewale.BindViewModels.Webforms.Photos;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PriceQuote;
using Bikewale.Utility;
using System;
using System.Web;

namespace Bikewale.New.Photos
{
    public class Default : System.Web.UI.Page
    {
        protected GenericBikeInfo bikeInfo;
        protected BindModelPhotos vmModelPhotos = null;
        protected SimilarBikeWithPhotos ctrlSimilarBikesWithPhotos;
        protected bool IsUpcoming { get; set; }
        protected bool IsDiscontinued { get; set; }
        protected bool isModelPage;
        protected uint VideoCount;
        protected PQSourceEnum pqSource;
        protected string bikeUrl = string.Empty, bikeName = string.Empty;
        protected NewVideosControl ctrlVideos;
        protected uint gridSize = 25, imageIndex = 0;
        private uint _modelId;
        protected GenericBikeInfoControl ctrlGenericBikeInfo;
        protected string JSONImageList = string.Empty, JSONVideoList = string.Empty, JSONFirstImage = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        /// <summary>       
        /// Summary :- model page photo bind condition added in query string
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Description : Get imageindex from querystring.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(Request.RawUrl);
            dd.DetectDevice();

            if (!String.IsNullOrEmpty(Request.QueryString["modelpage"]))
            {
                isModelPage = true;
            }

            if (!String.IsNullOrEmpty(Request.QueryString["imageindex"]))
            {
                imageIndex = Convert.ToUInt32(Request.QueryString["imageindex"]);
            }

            BindPhotosPage();
            BindPageWidgets();
        }

        /// <summary>
        /// Created By : Created By : Sajal Gupta on 09-02-2017
        /// Description : Bind photos page with metas,photos and widgets 
        /// Modified : Changed viewmodel function.
        /// </summary>
        private void BindPhotosPage()
        {
            try
            {
                vmModelPhotos = new BindModelPhotos();

                if (!vmModelPhotos.isRedirectToModelPage && !vmModelPhotos.isPermanentRedirection && !vmModelPhotos.isPageNotFound)
                {
                    vmModelPhotos.isDesktop = true;
                    vmModelPhotos.GridSize = 24;
                    vmModelPhotos.NoOfGrid = 8;
                    vmModelPhotos.isModelpage = isModelPage;
                    vmModelPhotos.GetPhotoGalleryData();
                    IsDiscontinued = vmModelPhotos.IsDiscontinued;
                    BindBikeInfo genericBikeInfo = new BindBikeInfo();

                    if (vmModelPhotos.objModel != null)
                    {
                        genericBikeInfo.ModelId = (uint)vmModelPhotos.objModel.ModelId;
                    }
                    bikeInfo = genericBikeInfo.GetBikeInfo();

                    if (bikeInfo != null)
                    {
                        if (bikeInfo.Make != null && bikeInfo.Model != null)
                        {
                            bikeUrl = string.Format("{0}", Bikewale.Utility.UrlFormatter.BikePageUrl(bikeInfo.Make.MaskingName, bikeInfo.Model.MaskingName));
                            bikeName = string.Format("{0} {1}", bikeInfo.Make.MakeName, bikeInfo.Model.ModelName);
                        }
                        pqSource = PQSourceEnum.Desktop_Photos_page;
                        IsUpcoming = genericBikeInfo.IsUpcoming;
                        IsDiscontinued = genericBikeInfo.IsDiscontinued;
                        VideoCount = bikeInfo.VideosCount;

                        if (ctrlGenericBikeInfo != null && bikeInfo.Model != null)
                        {
                            var objresponse = new ModelHelper().GetModelDataByMasking((bikeInfo.Model.MaskingName));

                            ctrlGenericBikeInfo.ModelId = objresponse.ModelId;
                            ctrlGenericBikeInfo.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
                            ctrlGenericBikeInfo.PageId = BikeInfoTabType.Image;
                            ctrlGenericBikeInfo.TabCount = 4;
                        }

                        JSONImageList = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64((new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(vmModelPhotos.objImageList));
                        JSONVideoList = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64((new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(vmModelPhotos.objVideosList));
                        JSONFirstImage = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64((new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(vmModelPhotos.firstImage));
                    }

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.New.Photos : BindPhotosPage");
            }
            finally
            {
                if (vmModelPhotos.isRedirectToModelPage)  ///new/ page for photos exception
                {
                    Response.Redirect("/m/new-bikes-in-india/", true);
                }
                else if (vmModelPhotos.isPermanentRedirection) //301 redirection
                {
                    Bikewale.Common.CommonOpn.RedirectPermanent(vmModelPhotos.pageRedirectUrl);
                }
                else if (vmModelPhotos.isPageNotFound)  //page not found
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 09-02-2017
        /// Description : bind photos page widgets
        /// </summary>
        private void BindPageWidgets()
        {
            try
            {
                if (vmModelPhotos != null && vmModelPhotos.objModel != null && vmModelPhotos.objMake != null)
                {
                    if (ctrlSimilarBikesWithPhotos != null && !IsDiscontinued)
                    {
                        ctrlSimilarBikesWithPhotos.TotalRecords = 6;
                        ctrlSimilarBikesWithPhotos.ModelId = vmModelPhotos.objModel.ModelId;
                        ctrlSimilarBikesWithPhotos.WidgetHeading = "Images of similar bikes";
                    }

                    if (ctrlVideos != null)
                    {
                        ctrlVideos.TotalRecords = 3;
                        ctrlVideos.WidgetTitle = bikeName;
                        ctrlVideos.ModelId = vmModelPhotos.objModel.ModelId;
                        ctrlVideos.ModelName = vmModelPhotos.objModel.ModelName;
                        ctrlVideos.ModelMaskingName = vmModelPhotos.objModel.MaskingName;
                        ctrlVideos.MakeId = vmModelPhotos.objMake.MakeId;
                        ctrlVideos.MakeName = vmModelPhotos.objMake.MakeName;
                        ctrlVideos.MakeMaskingName = vmModelPhotos.objMake.MaskingName;
                    }

                    _modelId = (uint)vmModelPhotos.objModel.ModelId;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("Bikewale.New.Photos : BindPageWidgets for modelId {0}", _modelId));
            }
        }
    }
}