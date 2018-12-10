using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Microsoft.Practices.Unity;
using Carwale.Service;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Entity.Classified.UsedLeads;
using System.Collections.Generic;
using Carwale.Entity.Classified.Leads;

namespace Carwale.UI.MyCarwale.MyInquiries
{
    public class MyPurchaseRequests : Page
    {
        protected Repeater rptRequests;
        protected DropDownList ddlRequestTime;
        protected HtmlGenericControl errRequests, divRequests;
        protected string inquiryId = string.Empty, requestdate = string.Empty;
        protected int buyerCount = 1, totalPurchaseRequests = 0;

        private ILeadRepository _leadRepo;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _leadRepo = container.Resolve<ILeadRepository>();
            }
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
                Response.Redirect("/users/login.aspx?returnUrl=/mycarwale/");

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                inquiryId = Request.QueryString["id"];
            }

            if (!String.IsNullOrEmpty(Request.QueryString["requestdate"]))
            {
                requestdate = Request.QueryString["requestdate"];
            }

            divRequests.Visible = true;

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    errRequests.Visible = false;
                    GetPurchaseRequests(String.IsNullOrEmpty(requestdate) ? 7 : Convert.ToInt32(requestdate));
                    ddlRequestTime.SelectedValue = requestdate;
                }
                else
                {
                    Response.Redirect("/mycarwale/", true);
                }
            }
        }

        protected void GetPurchaseRequests(int requestDate)
        {
            var classifiedRequests = _leadRepo.GetClassifiedRequests(Convert.ToInt32(inquiryId), requestDate);
            if (classifiedRequests != null && classifiedRequests.Count > 0)
            {
                rptRequests.DataSource = classifiedRequests;
                rptRequests.DataBind();
                totalPurchaseRequests = classifiedRequests.Count;
            }
            else
            {
                errRequests.InnerText = "No Purchase Requests Arrived.";
                errRequests.Visible = true;
                divRequests.Visible = false;
            }
        }
    }
}