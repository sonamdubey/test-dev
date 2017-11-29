using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 4 July 2014
    /// Summary : class for model gallery photos
    /// Modified By : Ashwini Todkar on 3 Oct 2014
    /// </summary>
    public class PhotoGallaryMin : System.Web.UI.UserControl
    {
        protected HtmlGenericControl noImageAv;
        protected string selectedImageName = string.Empty, selectedImagePath = string.Empty, selectedImageCategoryName = string.Empty,
            selectedImageMainCategoryName = string.Empty, selectedImageCategory = string.Empty;
        public IEnumerable<ColorImageBaseEntity> objImageList = null;
        public string BikeName { get; set; }
        public int FetchedCount { get; set; }
        public int modelId { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetImageList();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 26 Sept 2014
        /// Summary    : method to get model photo list from carwale api
        /// Modified By: Aditi Srivastava on 18th Aug,2016
        /// Description: Changed method GetModelPhotoGallery to GetModelPhotos
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Descrioption : Called Bal function instead of cache function.
        /// 
        /// </summary>
        private void GetImageList()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IPager, Pager>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>();
                    var objModelEntity = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                    objImageList = objModelEntity.CreateAllPhotoList(modelId);
                    FetchedCount = objImageList != null ? objImageList.Count() : 0;
                    if (FetchedCount > 0)
                    {
                        var firstImage = objImageList.FirstOrDefault();
                        if (firstImage != null)
                        {
                            selectedImagePath = Bikewale.Utility.Image.GetPathToShowImages(firstImage.OriginalImgPath, firstImage.HostUrl, Bikewale.Utility.ImageSize._640x348);
                            selectedImageCategoryName = firstImage.ImageCategory;
                            selectedImageCategory = selectedImageCategoryName != string.Empty ? " - " + selectedImageCategoryName : string.Empty;
                        }
                    }
                    else
                    {
                        noImageAv.Visible = true;
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("PhotoGallaryMin.GetImageList() => ModelId:{0}", modelId));
            }
        }//End of AllImageList
    }// End of class  
}   //End of namespace