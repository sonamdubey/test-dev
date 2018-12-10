using Carwale.Notifications;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.UI.Common;
using Carwale.Interfaces.Classified.CarDetail;
using Carwale.Service;
using Microsoft.Practices.Unity;
using Carwale.Entity.Classified.CarDetails;
using System.Collections.Generic;

namespace Carwale.UI.Used
{
    public class ReportListing : Page
    {
        protected bool IsDealer = false;
        protected DropDownList ddlReason;
        private IListingDetails _carDetailRepo;
        protected override void OnInit(EventArgs e)
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _carDetailRepo = container.Resolve<IListingDetails>();
            }
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string profileId = string.Empty;
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["car"]))
                {
                    profileId = Request.QueryString["car"].ToString();
                    IsDealer = CommonOpn.CheckIsDealerFromProfileNo(profileId);
                    Trace.Warn("IsDealer: " + IsDealer.ToString());
                    FillReasons(IsDealer);
                }
            }
        }

        private void FillReasons(bool isDealer)
        {
            try
            {
                List<ReportListingReasons> reasons = null;
                reasons = _carDetailRepo.GetReportListingReasons(isDealer);
                ddlReason.DataSource = reasons;
                ddlReason.DataTextField = "Text";
                ddlReason.DataValueField = "Value";
                ddlReason.DataBind();
                ListItem objItem = new ListItem("-- Report Reason --", "-1");
                ddlReason.Items.Insert(0, objItem);
            }
            catch (Exception ex)
            {
                Trace.Warn("EX: " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}