using Bikewale.Entities.CMS;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.BAL.BikeData;
using Bikewale.Common;
using Bikewale.Entities.CMS.Photos;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 4 July 2014
    /// Summary : class for model gallery photos
    /// Modified By : Ashwini Todkar on 3 Oct 2014
    /// </summary>
    public class PhotoGallaryMin : System.Web.UI.UserControl
    {
        protected Repeater rptPhotos;
        protected HtmlGenericControl noImageAv;
        protected string selectedImageName = string.Empty,  selectedImagePath = string.Empty, selectedImageCategoryName = string.Empty,
            selectedImageMainCategoryName = string.Empty, selectedImageCategory = string.Empty;
        protected BikeModelEntity objModelEntity = null;
        protected int recordCount = 0;
        public int ModelId { get; set; }

        public string imageId = string.Empty;
        public string ImageId { get; set; }


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetModelDetails();
                GetImageList();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 3 Oct 2014
        /// Summary    : PopulateWhere to get model details
        /// </summary>
        private void GetModelDetails()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                IBikeModels<BikeModelEntity, int> objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();

                //Get Model details
                objModelEntity = objModel.GetById(Convert.ToInt32(ModelId));
          
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 26 Sept 2014
        /// Summary    : method to get model photo list from carwale api
        /// </summary>
        private async void GetImageList()
        {
            try
            {
                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.PhotoGalleries);
                categorList.Add(EnumCMSContentType.ComparisonTests);

                //sets the base URI for HTTP requests
                string contentTypeList = CommonOpn.GetContentTypesString(categorList);
                
                string _applicationid = Utility.BWConfiguration.Instance.ApplicationId;

                string _apiUrl = "webapi/image/modelphotolist/?applicationid=" + _applicationid + "&modelid=" + ModelId + "&categoryidlist=" + contentTypeList;

                List<ModelImage> _objImageList = null;

                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    _objImageList = await objClient.GetApiResponse<List<ModelImage>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objImageList);
                }
                
                if (_objImageList != null && _objImageList.Count > 0)
                    recordCount = _objImageList.Count;
                else
                    recordCount = 0;

                if (recordCount > 0)
                {
                    if (ImageId != string.Empty)
                    {
                        //Trace.Warn("img id ", ImageId);

                        ModelImage value = _objImageList.Find(item => item.ImageId == Convert.ToUInt32(ImageId));

                        if (value != null)
                        {
                            //selectedImagePath = Bikewale.Common.ImagingFunctions.GetPathToShowImages(value.ImagePathLarge, value.HostUrl);
                            selectedImagePath = Bikewale.Utility.Image.GetPathToShowImages(value.OriginalImgPath, value.HostUrl, Bikewale.Utility.ImageSize._640x348);
                            //OriginalImagePath
                            selectedImageCategoryName = value.ImageCategory;
                            selectedImageCategory = selectedImageCategoryName != string.Empty ? " - " + selectedImageCategoryName : "";
                        }
                    }
                    else // first image not selected
                    {
                        // Retrive the first image from list
                        //selectedImagePath = Bikewale.Common.ImagingFunctions.GetPathToShowImages(_objImageList[0].ImagePathLarge, _objImageList[0].HostUrl);
                        selectedImagePath = Bikewale.Utility.Image.GetPathToShowImages(_objImageList[0].OriginalImgPath, _objImageList[0].HostUrl, Bikewale.Utility.ImageSize._640x348);
                        selectedImageCategoryName = _objImageList[0].ImageCategory;
                        selectedImageCategory = selectedImageCategoryName != string.Empty ? " - " + selectedImageCategoryName : "";
                    }
                    BindPhotos(_objImageList);
                }
                else
                {
                    rptPhotos.Visible = false;
                    noImageAv.Visible = true;
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }//End of AllImageList

        /// <summary>
        /// Written By : Ashwini Todkar on 3 Oct 2014
        /// Summary    : PopulateWhere to bind model photos repeater
        /// </summary>
        /// <param name="_objImageList"></param>
        private void BindPhotos(List<ModelImage> _objImageList)
        {

            rptPhotos.Visible = true;
            noImageAv.Visible = false;

            rptPhotos.DataSource = _objImageList;
            rptPhotos.DataBind();
        } 
        
    }// End of class  
    //}   //End of class
}   //End of namespace