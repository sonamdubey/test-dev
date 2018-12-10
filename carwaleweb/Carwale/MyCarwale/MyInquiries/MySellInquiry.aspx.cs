using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Carwale.UI.Common;
using Carwale.Interfaces.Classified.SellCar;
using Microsoft.Practices.Unity;
using Carwale.Service;
using System.Collections.Generic;
using Carwale.Interfaces.PaymentGateway;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity.Classified.SellCar;

namespace Carwale.UI.MyCarwale.MyInquiries
{
    public class MySellInquiry : Page
    {
        protected HtmlGenericControl spnError;
        protected Repeater rptSellInq;

        private string _ACTIVE_STATUS = "Active";
        public string customerId;
        public string customerEmail;
        protected int notificationCode = 0;
        protected bool showListingNotification = false;
        protected bool isCustomerEditable = true;

        public string carPicUrl = CommonOpn.ImagePath + "cars/";

        public string CurrentInquiryId
        {
            get { return ViewState["CurrentInquiryId"].ToString(); }
            set { ViewState["CurrentInquiryId"] = value; }
        }

        public string CurrentStepId
        {
            get { return ViewState["CurrentStepId"].ToString(); }
            set { ViewState["CurrentStepId"] = value; }
        }

        private ISellCarRepository _sellCarRepo;
        private ISellCarBL _sellCarBL;
        private IPackageRepository _packageRepo;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            using(IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _sellCarRepo = container.Resolve<ISellCarRepository>();
                _sellCarBL = container.Resolve<ISellCarBL>();
                _packageRepo = container.Resolve<IPackageRepository>();
            }
            base.Load += new EventHandler(Page_Load);          
            rptSellInq.ItemDataBound += new RepeaterItemEventHandler(ManageSellInquiryStatus);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            spnError.InnerText = "";

            if (CurrentUser.Id == "-1")
                Response.Redirect("/users/login.aspx?returnUrl=/MyCarwale/MyInquiries/MySellInquiry.aspx");

            customerId = CurrentUser.Id;
            customerEmail = CurrentUser.Email;

            if (!IsPostBack)
            {
                BindRepeaters();
            }
            bool isPremium = false;
            if (Request["ispremium"] != null && bool.TryParse(Request["ispremium"], out isPremium) && isPremium)
            {
                isCustomerEditable = false;
            }
        }

        void BindRepeaters()
        {
            int defaultPackageId = Convert.ToInt32(ConfigurationManager.AppSettings["freePkgId"]);
            List<CustomerSellInquiry> customerSellInquiries = _sellCarRepo.GetCustomerSellInquiries(customerEmail, defaultPackageId);
            
            if (customerSellInquiries != null && customerSellInquiries.Count > 0)
            {
                rptSellInq.DataSource = customerSellInquiries;
                rptSellInq.DataBind();
            }
            else
            {
                spnError.InnerText = "You don't have any car listed now.";
            }
        }
        

        private void ManageSellInquiryStatus(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                bool isListingCompleted = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsListingCompleted"));
                string currentStep = (DataBinder.Eval(e.Item.DataItem, "CurrentStep")).ToString();
                string llInquiryId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "LLInquiryId")) > 0 ? (DataBinder.Eval(e.Item.DataItem, "LLInquiryId")).ToString() : null;
                string paymentMode = (DataBinder.Eval(e.Item.DataItem, "PaymentMode")).ToString();
                int freeListingCnt = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Free"));
                int paidListingCnt = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Paid"));
                string expDate = DataBinder.Eval(e.Item.DataItem, "ClassifiedExpiryDate").ToString();
                bool isPremium = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsPremium"));

                InquiryStatus objStatus = new InquiryStatus(llInquiryId, isListingCompleted, currentStep, paymentMode, freeListingCnt, paidListingCnt, expDate, isPremium);

                Literal ltrInquiryStatus = (Literal)e.Item.FindControl("ltrInquiryStatus");
                ltrInquiryStatus.Text = objStatus.Status;

                Literal ltrInquiryText = (Literal)e.Item.FindControl("ltrInquiryText");
                ltrInquiryText.Text = objStatus.Message;

                Button btnListing = (Button)e.Item.FindControl("btnListing");

                if (objStatus.ShowButton)
                {
                    btnListing.CommandName = objStatus.ButtonCommandName;
                    btnListing.Text = objStatus.ButtonText;
                    btnListing.Visible = true;
                }
                else
                {
                    btnListing.Visible = false;
                }
            }
        }

        protected string SellInquiryStatus(string inquiryId, string isPremium, string classifiedExpiryDate, string packageExpiryDate, string Status, string currentStep, string isListingCompleted, string packageId)
        {
            string inquiryStatus = string.Empty;
            string expiryDate = string.Empty;
            DateTime classifiedDate;
            DateTime packageExpDate;

            if (String.IsNullOrEmpty(isPremium) && String.IsNullOrEmpty(currentStep))
            {
                inquiryStatus = "";
            }
            else
            {
                if (Convert.ToBoolean(isListingCompleted))
                {
                    if (currentStep == ((int)(SellCarSteps.Confirmation)).ToString())
                    {
                        if (!String.IsNullOrEmpty(classifiedExpiryDate))
                        {
                            classifiedDate = Convert.ToDateTime(classifiedExpiryDate);
                            packageExpDate = Convert.ToDateTime(packageExpiryDate);
                            TimeSpan dateDiff = (classifiedDate.Date - DateTime.Today.Date);
                            int noOfDaysExpiry = dateDiff.Days;
                            Boolean isRenewButtonVisible = false;

                            if (!Boolean.Parse(isPremium))
                            {
                                if (packageExpDate.Date >= DateTime.Now.Date)
                                {
                                    if (Status == _ACTIVE_STATUS)
                                    {
                                        if (noOfDaysExpiry <= 7)
                                        {
                                            if (classifiedDate.Date != packageExpDate.Date)
                                            {
                                                isRenewButtonVisible = true;
                                            }
                                        }
                                    }
                                }
                            }

                            String encryptedValue = Utility.CarwaleSecurity.Encrypt(inquiryId + "," + customerId);
                            encryptedValue = "S" + encryptedValue;

                            if (classifiedDate >= DateTime.Today)
                            {
                                inquiryStatus = "[Expiring on : " + classifiedDate.ToString("dd-MMM-yy") + "] ";
                                if (isRenewButtonVisible)
                                {
                                    inquiryStatus += "<a style='cursor:pointer;' onclick=\"renewCar('" + encryptedValue + "')\">[Renew]</a>";
                                }
                            }
                            else
                            {
                                inquiryStatus = "[Expired on : " + classifiedDate.ToString("dd-MMM-yy") + "] ";
                                if (isRenewButtonVisible)
                                {
                                    inquiryStatus += "<a style='cursor:pointer;' onclick=\"renewCar('" + encryptedValue + "')\">[Renew]</a>";
                                }
                            }
                        }
                    }
                }
            }
            return inquiryStatus;
        }
    }
}