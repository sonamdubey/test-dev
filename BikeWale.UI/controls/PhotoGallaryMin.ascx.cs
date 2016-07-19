using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
        protected string selectedImageName = string.Empty, selectedImagePath = string.Empty, selectedImageCategoryName = string.Empty,
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
        private void GetImageList()
        {
            try
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IBikeModelsCacheRepository<int>>();

                    List<ModelImage> _objImageList = objCache.GetModelPhotoGallery(ModelId);

                    if (_objImageList != null && _objImageList.Count > 0)
                        recordCount = _objImageList.Count;
                    else
                        recordCount = 0;

                    if (recordCount > 0)
                    {
                        if (ImageId != string.Empty)
                        {

                            ModelImage value = _objImageList.Find(item => item.ImageId == Convert.ToUInt32(ImageId));

                            if (value != null)
                            {
                                selectedImagePath = Bikewale.Utility.Image.GetPathToShowImages(value.OriginalImgPath, value.HostUrl, Bikewale.Utility.ImageSize._640x348);
                                selectedImageCategoryName = value.ImageCategory;
                                selectedImageCategory = selectedImageCategoryName != string.Empty ? " - " + selectedImageCategoryName : "";
                            }
                        }
                        else // first image not selected
                        {
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