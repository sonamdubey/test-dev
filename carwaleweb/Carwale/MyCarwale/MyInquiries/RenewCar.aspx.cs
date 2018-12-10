using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;
using Carwale.Interfaces.Classified.SellCar;
using Microsoft.Practices.Unity;
using Carwale.Service;

namespace Carwale.UI.MyCarwale.MyInquiries
{
    public class RenewCar : Page
    {
        protected Button btnSave;
        protected Label lblMsg;
        protected Panel errorPanel;
        public string inquiryId = "", inquiryType = "", customerId = "";

        private ISellCarRepository _sellCarRepo;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _sellCarRepo = container.Resolve<ISellCarRepository>();
            }
            base.Load += new EventHandler(Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
                Response.Redirect("/Users/login.aspx?returnUrl=/MyCarwale/MyInquiries/");

            String decryptedValue = string.Empty;
            String[] separate;

            if (Request["Id"] == null || String.IsNullOrEmpty(Request["Id"].ToString()))
            {
                btnSave.Enabled = false;
                return;
            }
            else
            {
                try
                {
                    String decrypt = Request["Id"].ToString().Remove(0, 1);
                    decryptedValue = Utility.CarwaleSecurity.Decrypt(decrypt);
                    separate = decryptedValue.Split(',');
                    inquiryId = separate[0];
                    customerId = separate[1];
                }
                catch (Exception err)
                {
                    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                    Response.Redirect("/Users/login.aspx?returnUrl=/MyCarwale/MyInquiries/");
                }
            }
            if (!IsPostBack)
            {
                FillStatus();
            }
        }

        void FillStatus()
        {
            DateTime? expiryDate;
            string carName;
            _sellCarRepo.GetSellCarExpiry(Convert.ToInt32(CurrentUser.Id), Convert.ToInt32(inquiryId), out expiryDate, out carName);

            if(expiryDate != null && !String.IsNullOrWhiteSpace(carName))
            {
                if (expiryDate >= DateTime.Today)
                    lblMsg.Text = "The listing of your car " + carName + " will expire on " + expiryDate.Value.ToString("dd-MMM-yy");
                else
                    lblMsg.Text = "Your car " + carName + " is no more listed. To again start listing it, renew your ad.";
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            bool isSuccess = _sellCarRepo.RenewSellCarListing(Convert.ToInt32(inquiryId), Convert.ToInt32(customerId));
            if (!isSuccess)
            {
                errorPanel.Visible = true;
                btnSave.Enabled = false;
            }
        }
    }
}