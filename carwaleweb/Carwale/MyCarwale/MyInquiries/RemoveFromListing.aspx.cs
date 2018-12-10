using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.UI.Common;
using Carwale.Interfaces.Classified.Leads;
using Microsoft.Practices.Unity;
using Carwale.Service;
using Carwale.Interfaces.Classified.SellCar;

namespace Carwale.UI.MyCarwale.MyInquiries
{
    public class RemoveFromListing : Page
    {
        protected Button btnSave;
        protected DropDownList drpStatus;
        public string inquiryId = "", inquiryType = "";
        protected Label lblMsg;
        protected TextBox txtComments;
        private ILeadRepository _leadRepo;
        private ISellCarRepository _sellCarRepo;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            using(IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _leadRepo = container.Resolve<ILeadRepository>();
                _sellCarRepo = container.Resolve<ISellCarRepository>();
            }
            base.Load += new EventHandler(Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
                Response.Redirect("/Users/login.aspx?returnUrl=/MyCarwale/MyInquiries/");

            if (Request["Id"] == null || !CommonOpn.CheckId(Request["Id"]))
            {
                btnSave.Enabled = false;
                return;
            }
            else
                inquiryId = Request["Id"].ToString();

            if (Request["type"] == null || !CommonOpn.CheckId(Request["type"]))
            {
                btnSave.Enabled = false;
                return;
            }
            else
                inquiryType = Request["type"].ToString();

            if (!_sellCarRepo.IsCustomerAuthorizedToManageCar(Convert.ToInt32(CurrentUser.Id), Convert.ToInt32(inquiryId)))
            {
                Response.Redirect("/mycarwale/", true);
            }

            if (!IsPostBack)
            {
                FillStatus();
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            //inquiryType=> 1: SellInquiry, 2: PurchaseInquiryDealer, 3: PurchaseInquiryIndividual
            _leadRepo.UpdateInquiryStatus(Convert.ToInt32(inquiryId), Convert.ToInt32(inquiryType), Convert.ToInt32(CurrentUser.Id), Convert.ToInt32(drpStatus.SelectedValue));

            string inquiryTypeName = "Sell Car";
            Mails.InquiryArchivedByCustomer(inquiryTypeName, CurrentUser.Id, inquiryId, drpStatus.SelectedItem.Text, txtComments.Text);
        }

        void FillStatus()
        {
            drpStatus.Items.Add(new ListItem("-- Reason to remove from listing --", "0"));
            drpStatus.Items.Add(new ListItem("Sold through Carwale.com", "2"));
            drpStatus.Items.Add(new ListItem("Sold through other dealer/individual", "3"));
            drpStatus.Items.Add(new ListItem("I am not satisfied with Carwale.com services", "4"));
            drpStatus.Items.Add(new ListItem("I am not selling it any more", "6"));

            lblMsg.Text = "Please choose a reason to remove the car from listing and then click 'Remove My Car' button.";
            btnSave.Text = "Remove My Car";
        }
    }
}