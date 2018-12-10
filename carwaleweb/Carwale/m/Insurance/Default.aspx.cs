//using Carwale.Insurance;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using Carwale.Notifications;
using Carwale.Interfaces;
using Microsoft.Practices.Unity;
using Carwale.Interfaces.Geolocation;
using Carwale.DAL.Geolocation;
using Carwale.Cache.Geolocation;
using AEPLCore.Cache;
using System.Configuration;
using Carwale.Cache.CarData;
using Carwale.DAL.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CarData;
using Carwale.Service;
using Carwale.Utility;
using Carwale.DAL.Customers;
using AEPLCore.Cache.Interfaces;

namespace MobileWeb.Insurance
{
    /// <summary>
    /// modified by sachin bharti on 8/10/15
    /// purpose - new process implemented.Remove rabbit mq code.Calculating no calim bonus on 
    ///           the page instead of asking.
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected TextBox txtCC, txtRegistration, txtEmail, txtMobile, txtRegDate, txtName;
        protected Button btnCalculate;
        protected HtmlInputHidden hdn_CityName, hdn_StateId, hdn_StateName, hdn_CityId, hdn_VersionId, hdn_radio, hdn_MakeId, hdn_ModelId, hdn_Make, hdn_Model, hdn_Version, hdn_RegDate;
        protected string requestStatusCode = string.Empty, json = string.Empty, cwLeadId = string.Empty, netPremium = string.Empty, serviceTax = string.Empty, totalPremium = string.Empty;
        protected int bjDailyCount = 0, bjMonthlyCount = 0;
        protected bool isSent = false;

        protected HtmlGenericControl divErrMsg, divInsuranceForm, insSelectState, insSelectCity, insSelectCar;
        protected Label lblErrorMsg;
        protected CustomerDetails cd;

        string customerId = "";
        string custName = string.Empty, custEmail = string.Empty, custMobile = string.Empty;
        protected string cholaAssitanceNo = ConfigurationManager.AppSettings["CholaAssitanceNo"];
        int regYear = System.DateTime.Now.Year, regMonth=System.DateTime.Now.Month;
        public string mailLeadToChola = ConfigurationManager.AppSettings["MailLeadToChola"];
        protected DateTime regDate;
        protected string insuranceThanksMsg = ConfigurationManager.AppSettings["InsuranceThankYouMsg"] ?? "";

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            customerId = CurrentUser.Id;
            custName = txtName.Text.Trim();
            custEmail = txtEmail.Text.Trim();
            custMobile = txtMobile.Text.Trim();

            if (customerId != "-1")
            {
                cd = new CustomerDetails(CurrentUser.Id);
                if (string.IsNullOrWhiteSpace(txtName.Text)) txtName.Text = cd.Name;
                if (string.IsNullOrWhiteSpace(txtEmail.Text)) txtEmail.Text = CurrentUser.Email;
                //txtEmail.Enabled = false;
                if (string.IsNullOrWhiteSpace(txtMobile.Text)) txtMobile.Text = cd.Mobile;
            }

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["cityId"]) && RegExValidations.IsNumeric(Request.QueryString["cityId"]))
                    BindStateCity(Request.QueryString["cityId"]);
                else if (HttpContext.Current.Request.Cookies["_CustCityIdMaster"] != null && HttpContext.Current.Request.Cookies["_CustCityMaster"] != null && RegExValidations.IsNumeric(HttpContext.Current.Request.Cookies["_CustCityIdMaster"].Value) && HttpContext.Current.Request.Cookies["_CustCityIdMaster"].Value != "-1")
                    BindStateCity(Request.Cookies["_CustCityIdMaster"].Value);

            }

            if (HttpContext.Current.Request["car"] != null && HttpContext.Current.Request.QueryString["car"] != "" && RegExValidations.IsNumeric(HttpContext.Current.Request["car"])) 
            {
                int versionId = CustomParser.parseIntObject(HttpContext.Current.Request.QueryString["car"]);
                PrefillCar(versionId); 
            }
            
            if (!string.IsNullOrEmpty(Request.QueryString["reg"]) && RegExValidations.IsValidDate(Request.QueryString["reg"]))
            {
                DateTime.TryParse(Request.QueryString["reg"], out regDate);
                hdn_RegDate.Value = regDate < DateTime.Now && regDate.Year > 1995 ? Convert.ToDateTime(Request.QueryString["reg"]).ToString("MMM, yyyy") : "";                
            }
            divErrMsg.Visible = false;            
        }

         /// <summary>
        /// written by Jitendra on 22/01/2016
        /// to prefill Car makes-model-version based on query string
        /// </summary>
        private void PrefillCar(int versionId)
        {
            try
            {
                IUnityContainer container = new UnityContainer();
                container.RegisterType<ICarVersionRepository, CarVersionsRepository>()
                    .RegisterType<ICarVersionCacheRepository, CarVersionsCacheRepository>()
                    .RegisterType<ICacheManager, CacheManager>();

                ICarVersionCacheRepository _carVersionCacheRepository = container.Resolve<ICarVersionCacheRepository>();

                CarVersionDetails carDetails = _carVersionCacheRepository.GetVersionDetailsById(versionId);
                hdn_VersionId.Value = versionId.ToString();
                hdn_MakeId.Value = carDetails.MakeId.ToString();
                hdn_ModelId.Value = carDetails.ModelId.ToString();
                insSelectCar.InnerText = carDetails.MakeName + " " + carDetails.ModelName + " " + carDetails.VersionName;
                hdn_Make.Value = carDetails.MakeName;
                hdn_Model.Value = carDetails.ModelName;
                hdn_Version.Value = carDetails.VersionName;
            }
            catch (Exception err) 
            {
                // Show error message
                divInsuranceForm.Visible = false;
                divErrMsg.Visible = true;
                lblErrorMsg.Text = "Oops some error occured. We are sorry for this inconvenience. Error cause has been sent to the administrators. We will resolve the problem very soon.";

                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }

        /// <summary>
        /// written by sachin bharti on 12/10/2015
        /// to prefill state and city based on global city
        /// </summary>
        private void BindStateCity(string strCityId)
        {
            try
            {
                IUnityContainer container = new UnityContainer();
                container.RegisterType<IGeoCitiesRepository, GeoCitiesRepository>()
                    .RegisterType<IGeoCitiesCacheRepository, GeoCitiesCacheRepository>()
                    .RegisterType<ICacheManager, CacheManager>();
                IGeoCitiesCacheRepository _geoCache = container.Resolve<IGeoCitiesCacheRepository>();

                int cityId;

                Int32.TryParse(strCityId, out cityId);

                hdn_CityName.Value = _geoCache.GetCityNameById(cityId.ToString());

                hdn_CityId.Value = cityId.ToString();

                insSelectCity.InnerText = hdn_CityName.Value;

                Carwale.Entity.Geolocation.States state = _geoCache.GetStateByCityId(cityId);

                insSelectState.InnerText = state.StateName;               
                hdn_StateId.Value = state.StateId.ToString();
                hdn_StateName.Value = state.StateName;
            }
            catch (Exception err)
            {
                // Show error message
                divInsuranceForm.Visible = false;
                divErrMsg.Visible = true;
                lblErrorMsg.Text = "Oops some error occured. We are sorry for this inconvenience. Error cause has been sent to the administrators. We will resolve the problem very soon.";

                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        } // Page_Load        
    }    // End of Class
}   // End of NameSpace