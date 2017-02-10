using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using System;
using System.Threading;
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
        protected string photoId = string.Empty, bikeName = string.Empty, modelName = string.Empty, makename = string.Empty, modelImage = string.Empty;
        protected BikeModelEntity objModelEntity = null;
        protected uint modelId = 0;

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
                    objModelEntity = new ModelHelper().GetModelDataById(modelId);
                    if (objModelEntity != null)
                    {
                        modelName = objModelEntity.ModelName;
                        makename = objModelEntity.MakeBase.MakeName;
                        if (objModelEntity.MakeBase != null)
                            bikeName = string.Format("{0} {1}", objModelEntity.MakeBase.MakeName, objModelEntity.ModelName);
                        photoGallary.modelId = objModelEntity.ModelId;
                        photoGallary.BikeName = bikeName;
                        modelImage = Utility.Image.GetPathToShowImages(objModelEntity.OriginalImagePath, objModelEntity.HostUrl, Bikewale.Utility.ImageSize._476x268);
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
            bool success = false;
            try
            {
                if (!string.IsNullOrEmpty(modelQuerystring))
                {
                    objResponse = new ModelHelper().GetModelDataByMasking((modelQuerystring));
                    modelId = HandleModelRedirection(objResponse, modelQuerystring);
                    if (objResponse.StatusCode == 200)
                        success = true;
                }
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
                objErr.SendMail();

                Response.Redirect("/new-bikes-in-india/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            return success;
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 25 Nov 2016
        /// Summary: Private method to handle model masking redirections
        /// </summary>
        private uint HandleModelRedirection(ModelMaskingResponse objResponse, string modelMask)
        {
            uint modelID = 0;
            if (objResponse != null)
            {
                if (objResponse.StatusCode == 200)
                {
                    modelID = objResponse.ModelId;
                }
                else if (objResponse.StatusCode == 301)
                {
                    CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelMask, objResponse.MaskingName));
                }
                else
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            return modelID;
        }   //End of ProcessQueryString
    }   //End of class
}   //End of namespace