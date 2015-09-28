using Bikewale.BAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Controls;
using Bikewale.Common;

namespace Bikewale.New.PhotoGallery
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 2 July 2014
    /// Summmary : class for model photo gallery
    /// </summary>
    public class BikePhotos : System.Web.UI.Page
    {
        protected PhotoGallaryMin photoGallary;
        protected string modelId = string.Empty, photoId = string.Empty, imageId = string.Empty, selectedImagePath = string.Empty;
        protected BikeModelEntity objModelEntity = null;
        protected int modelCount = 0;
        protected BikeSeriesEntity objSeriesEntity;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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

                        container.RegisterType<IBikeSeries<BikeSeriesEntity, int>, BikeSeries<BikeSeriesEntity, int>>();
                        IBikeSeries<BikeSeriesEntity, int> objSeries = container.Resolve<IBikeSeries<BikeSeriesEntity, int>>();

                        objSeriesEntity = objSeries.GetById(objModelEntity.ModelSeries.SeriesId);
                        
                        modelCount = objSeriesEntity.ModelCount;

                        photoGallary.ModelId = objModelEntity.ModelId;
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