using Bikewale.BAL.BikeData;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Web;
using System.Web.UI;

namespace Bikewale.New.PhotoGallery
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 2 July 2014
    /// Summmary : class for model photo gallery
    /// </summary>
    public class BikePhotos : System.Web.UI.Page
    {
        protected PhotoGallaryMin photoGallary;
        protected string modelId = string.Empty, photoId = string.Empty, imageId = string.Empty, selectedImagePath = string.Empty, bikeName = string.Empty, modelName = string.Empty, makename = string.Empty;
        protected BikeModelEntity objModelEntity = null;
        // protected int modelCount = 0;
        //protected BikeSeriesEntity objSeriesEntity;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
            if (!Page.IsPostBack)
            {
                if (ProcessQueryString())
                {

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                        IBikeModels<BikeModelEntity, int> objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                        //Get Model details
                        objModelEntity = objModel.GetById(Convert.ToInt32(modelId));
                        modelName = objModelEntity.ModelName;
                        makename = objModelEntity.MakeBase.MakeName;
                        bikeName = string.Format("{0} {1}", objModelEntity.MakeBase.MakeName, objModelEntity.ModelName);
                        photoGallary.modelId = objModelEntity.ModelId;
                        photoGallary.ImageId = imageId;
                    }
                }
            }
        }
        /// <summary>
        /// Created By : Sadhana Upadhyay on 4 July 2014
        /// Summary : Validation for query string
        /// </summary>
        /// <returns></returns>
        private bool ProcessQueryString()
        {
            bool isSuccess = true;

            if (!String.IsNullOrEmpty(Request.QueryString["model"]))
            {
                ModelMapping objMapping = new ModelMapping();

                string _tmpModelId = objMapping.GetModelId(Request.QueryString["model"].ToLower());

                if (String.IsNullOrEmpty(_tmpModelId))
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSuccess = false;
                }
                else
                {
                    modelId = _tmpModelId;
                }
            }
            else
            {
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSuccess = false;
            }

            if (!String.IsNullOrEmpty(Request.QueryString["imgid"]))
            {
                imageId = Request.QueryString["imgid"];
            }

            return isSuccess;
        }   //End of ProcessQueryString
    }   //End of class
}   //End of namespace