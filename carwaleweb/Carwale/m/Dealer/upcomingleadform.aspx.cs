using Carwale.Cache.CarData;
using AEPLCore.Cache;
using Carwale.DAL.CarData;
using Carwale.Entity.CarData;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carwale.MobileWeb.Dealer
{
    public class upcomingleadform : System.Web.UI.Page
    {
        protected DropDownList drpStates;
        protected CarModelDetails upcomingModel;
        protected string defaultModelVal, defaultImgUrl,entireCarName;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["UpcomingModelId"] != null && Request.QueryString["UpcomingModelId"].ToString() != "" && RegExValidations.IsPositiveNumber(Request.QueryString["UpcomingModelId"].ToString()))
            {
                ProcessQueryString();
                FillControls.FillStates(drpStates);
            }
            else
            {
                UrlRewrite.Return404();
            }
        }
        protected void ProcessQueryString()
        {
            try
            {
                CarModelsCacheRepository cache = new CarModelsCacheRepository(new CarModelsRepository(), new CacheManager());
                defaultModelVal = Request.QueryString["UpcomingModelId"].ToString();
                upcomingModel = cache.GetModelDetailsById(int.Parse(defaultModelVal));
                if (upcomingModel==null || upcomingModel.ModelId < 1)
                {
                    UrlRewrite.Return404();
                }
                entireCarName = string.Format("{0} {1}", upcomingModel.MakeName, upcomingModel.ModelName);
                defaultImgUrl = ImageSizes.CreateImageUrl(upcomingModel.HostUrl, ImageSizes._227X128, upcomingModel.OriginalImage);
            }
            catch (Exception ex)
            {
                //Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}
