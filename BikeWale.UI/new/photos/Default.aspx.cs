using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
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
        protected string modelId = string.Empty, photoId = string.Empty, imageId = string.Empty, selectedImagePath = string.Empty, bikeName = string.Empty, modelName = string.Empty, makename = string.Empty, modelImage = string.Empty;
        protected BikeModelEntity objModelEntity = null;


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
                        if (objModelEntity != null)
                        {
                            modelName = objModelEntity.ModelName;
                            makename = objModelEntity.MakeBase.MakeName;
                            bikeName = string.Format("{0} {1}", objModelEntity.MakeBase.MakeName, objModelEntity.ModelName);
                            photoGallary.modelId = objModelEntity.ModelId;
                            photoGallary.ImageId = imageId;
                            modelImage = Utility.Image.GetPathToShowImages(objModelEntity.OriginalImagePath, objModelEntity.HostUrl, Bikewale.Utility.ImageSize._476x268);
                        }
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
            ModelMaskingResponse objResponse = null;
            string modelQuerystring = Request.QueryString["model"];
            string VersionIdStr = Request.QueryString["vid"];
            bool success = false;
            Trace.Warn("modelQuerystring 1 : ", modelQuerystring);
            try
            {
                if (!string.IsNullOrEmpty(modelQuerystring))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                ;
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                        objResponse = objCache.GetModelMaskingResponse(modelQuerystring);
                        success = true;
                    }

                }

            }
            catch (Exception ex)
            {
                Trace.Warn("exception 1 : ");
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
                objErr.SendMail();

                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            finally
            {
                Trace.Warn("finally");
                if (!string.IsNullOrEmpty(modelQuerystring))
                {
                    if (objResponse != null)
                    {
                        Trace.Warn(" objResponse.StatusCode : ", objResponse.StatusCode.ToString());
                        Trace.Warn(" objResponse.ModelId : ", objResponse.ModelId.ToString());
                        //Trace.Warn(" objResponse.MaskingName : ", objResponse.MaskingName.ToString());
                        if (objResponse.StatusCode == 200)
                        {
                            modelId = Convert.ToString(objResponse.ModelId);
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            //redirect permanent to new page 
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelQuerystring, objResponse.MaskingName));
                        }
                        else
                        {
                            Trace.Warn("pageNotFound.aspx 1");
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                    else
                    {
                        Trace.Warn("pageNotFound.aspx 2");
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                else
                {
                    Trace.Warn("pageNotFound.aspx 3");
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }

            }
            return success;
        }   //End of ProcessQueryString
    }   //End of class
}   //End of namespace