using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Bikewale.BAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Memcache;
using Bikewale.Common;
using System.Text.RegularExpressions;
using System.Data;
using Bikewale.Entities.Customer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.BAL.Customer;
using Bikewale.BAL.PriceQuote;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Net.Http;
using Bikewale.Mobile.PriceQuote;
using Bikewale.Interfaces.Location;
using Bikewale.DAL.Location;
using Bikewale.Entities.Location;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.BAL.BikeBooking;

namespace Bikewale.PriceQuote
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Class to get the price quote of the bike.
    /// Change  : To remove the version selection for the Price Quote
    /// Author  : Sumit Kate
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected DropDownList ddlMake, ddlModel, /*ddlVersion, */ ddlCity, ddlArea;
        protected string errMsg = "";
        protected string makeId = String.Empty, modelId = String.Empty, versionId = "0", modelName = string.Empty, makeName = string.Empty;
        protected Button btnSavePriceQuote;
        protected HtmlGenericControl div_GetPQ, div_ShowErrorMsg, spnVersion, spnCity, spnBuyTime, errName, errEmail, errMobile, spnArea, spnAgree;
        protected HtmlInputCheckBox userAgreement;
        protected HtmlInputHidden hdn_ddlModel, /*hdn_ddlVersion*/ hdn_ddlCity, hdn_selectedModel, hdn_selectedVersion, hdn_selectedCity, hdn_ddlArea, hdnIsAreaShown;
        protected string qs = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            this.btnSavePriceQuote.Click += new EventHandler(SavePriceQuote);
        }

        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            DeviceDetection deviceDetection = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            deviceDetection.DetectDevice();

            div_ShowErrorMsg.Visible = false;
            try
            {

                if (!Page.IsPostBack)
                {
                    BindMakes();

                    if (ProcessQueryString())
                    {
                        MakeModelVersion mmv = new MakeModelVersion();

                        //if query string contains model then get model details
                        if (!String.IsNullOrEmpty(modelId))
                        {
                            mmv.GetModelDetails(modelId);
                        }

                        //if query string contains version then get version details
                        //if (!String.IsNullOrEmpty(versionId))
                        //{
                        //    mmv.GetVersionDetails(versionId);
                        //    modelId = mmv.ModelId;
                        //}

                        if (!string.IsNullOrEmpty(mmv.MakeId))
                        {
                            makeId = mmv.MakeId;
                            ddlMake.SelectedValue = makeId;
                            modelName = mmv.Model;
                            makeName = mmv.Make;

                            if (BindModelsDropdownList(makeId))
                            {
                                ddlModel.SelectedValue = modelId;
                                //BindVersionsDropdownList(modelId);
                                BindCitiesDropdownList(modelId);

                                ddlMake.SelectedValue = makeId;
                                ddlModel.SelectedValue = modelId;

                                //ddlVersion.SelectedValue = String.IsNullOrEmpty(versionId) ? "0" : versionId;

                                ddlModel.Enabled = true;
                                //ddlVersion.Enabled = true;
                                ddlCity.Enabled = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// method to save PQ details and also register customer if new
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SavePriceQuote(object sender, EventArgs e)
        {
            uint cityId = 0, areaId = 0;
            if (IsPQDetailsValid())
            {

                GetPQDetails(ref cityId, ref areaId);
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                PQOutputEntity objPQOutput = null;
                try
                {
                    modelId = hdn_ddlModel.Value;
                    using (IUnityContainer container = new UnityContainer())
                    {
                        // save price quote
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuote>();
                        IDealerPriceQuote objIPQ = container.Resolve<IDealerPriceQuote>();

                        objPQEntity.CityId = cityId;
                        objPQEntity.AreaId = areaId > 0 ? areaId : 0;
                        objPQEntity.ClientIP = CommonOpn.GetClientIP();
                        objPQEntity.SourceId = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["sourceId"]);
                        objPQEntity.VersionId = Convert.ToUInt32(versionId);
                        objPQEntity.ModelId = Convert.ToUInt32(modelId);
                        // If pqId exists then, set pqId
                        objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Desktop_PQ_Landing);
                        objPQEntity.UTMA = Request.Cookies["__utma"].Value;
                        objPQEntity.UTMZ = Request.Cookies["__utmz"].Value;
                        objPQOutput = objIPQ.ProcessPQ(objPQEntity);

                    }
                }
                catch (Exception ex)
                {                    
                    string selectedParams = string.Format("modelId : {0}, CityId : {1}", modelId, cityId);
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " " + selectedParams);
                    objErr.SendMail();
                }
                finally
                {

                    if (objPQOutput.DealerId > 0 && objPQOutput.PQId>0)
                    {
                        // Save pq cookie
                        PriceQuoteCookie.SavePQCookie(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString());

                        Response.Redirect("/pricequote/dealerpricequote.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    else if(objPQOutput.PQId>0)
                    {
                        PriceQuoteCookie.SavePQCookie(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), "");

                        Response.Redirect("/pricequote/quotation.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    else
                    {
                        div_ShowErrorMsg.Visible = true;
                        div_ShowErrorMsg.InnerText = "Sorry !! Price Quote for this version is not available.";
                        div_GetPQ.Visible = false;
                    }
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Written By : Ashwini Todkar on 28 April 2014
        /// Summary    : method to get price quote details 
        /// </summary>
        /// <param name="name">Customer name</param>
        /// <param name="email">Customer email</param>
        /// <param name="mobile">Customer mobile</param>
        /// <param name="buyTime">Customer buying preference</param>
        private void GetPQDetails(ref uint cityId, ref uint areaId)
        {
            try
            {
                //if (!String.IsNullOrEmpty(hdn_ddlVersion.Value.Trim()))
                //    versionId = hdn_ddlVersion.Value.Trim();
                //else
                //    versionId = "0";

                if (!String.IsNullOrEmpty(hdn_ddlCity.Value.Trim()))
                    cityId = Convert.ToUInt32(hdn_ddlCity.Value.Trim());

                if (!String.IsNullOrEmpty(hdn_ddlArea.Value.Trim()))
                {
                    if (hdnIsAreaShown.Value == "true")
                        areaId = Convert.ToUInt32(hdn_ddlArea.Value.Trim());
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Written By : Ashwini Todkar on 28 April 2014
        /// Summary    : method to get bike makes for price quote 
        /// </summary>
        private void BindMakes()
        {
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();

                ddlMake.DataSource = mmv.GetMakes("PRICEQUOTE");
                ddlMake.DataValueField = "Value";
                ddlMake.DataTextField = "Text";
                ddlMake.DataBind();
                ddlMake.Items.Insert(0, (new ListItem("--Select Make--", "0")));

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        ///     PopulateWhere to bind models drop down list on change of make
        /// </summary>
        /// <param name="makeId"></param>
        protected bool BindModelsDropdownList(string makeId)
        {
            bool isSuccess = false;

            try
            {
                MakeModelVersion mmv = new MakeModelVersion();

                DataTable dt = mmv.GetModels(makeId, "PRICEQUOTE");

                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlModel.DataSource = dt;
                    ddlModel.DataValueField = "Value";
                    ddlModel.DataTextField = "Text";
                    ddlModel.DataBind();

                    ddlModel.Items.Insert(0, (new ListItem("--Select Model--", "0")));
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return isSuccess;
        }   // End of BindModelsDropdownList method

        /// <summary>
        ///     PopulateWhere to bind cities drop down list for the selected model 
        /// </summary>
        protected void BindCitiesDropdownList(string modelId)
        {
            List<CityEntityBase> objCities = null;
            try
            {
                using(IUnityContainer container =new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>();
                    ICity cityRepository = container.Resolve<ICity>();

                    objCities = cityRepository.GetPriceQuoteCities(Convert.ToUInt32(modelId));

                    if(objCities!=null)
                    {
                        ddlCity.DataSource = objCities;
                        ddlCity.DataTextField = "CityName";
                        ddlCity.DataValueField = "CityId";
                        ddlCity.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // End of BindMakesDropdownList method

        #region Commented
        /// <summary>
        ///     PopulateWhere to bind versions drop down list on change of model
        /// </summary>
        /// <param name="modelId"></param>
        [Obsolete("As Version drop down is removed from the page. This method has been marked with Obsolete attribute.", true)]
        protected bool BindVersionsDropdownList(string modelId)
        {
            bool isSuccess = false;

            try
            {
                //MakeModelVersion mmv = new MakeModelVersion();

                //DataTable dt = mmv.GetVersions(modelId, "PRICEQUOTE");

                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    ddlVersion.DataSource = dt;
                //    ddlVersion.DataValueField = "Value";
                //    ddlVersion.DataTextField = "Text";
                //    ddlVersion.DataBind();
                //    ddlVersion.Items.Insert(0, (new ListItem("--Select Version--", "0")));

                //    isSuccess = true;
                //}
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return isSuccess;
        }   // End of BindVersionsDropdownList method
        #endregion
        #endregion

        #region Validation Methods


        /// <summary>
        ///  PopulateWhere to check query string exist or not
        /// </summary>
        /// <returns></returns>
        private bool ProcessQueryString()
        {
            bool isQueryStringValid = false;

            if (!String.IsNullOrEmpty(Request.QueryString["model"]) && CommonOpn.CheckId(Request["model"]))
            {
                isQueryStringValid = true;
                modelId = Request.QueryString["model"];
                hdn_ddlModel.Value = modelId;

            }
            //if (!String.IsNullOrEmpty(Request.QueryString["version"]) && CommonOpn.CheckId(Request["version"]))
            //{
            //    versionId = Request.QueryString["version"];
            //    hdn_ddlVersion.Value = versionId;
            //    isQueryStringValid = true;
            //}

            return isQueryStringValid;
        }   // End of processQueryString method


        /// <summary>
        /// PopulateWhere to validate mobile number
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private bool IsValidMobile(string mobile)
        {
            if (Regex.IsMatch(mobile, @"^[0-9]+$") == true && mobile.Length == 10)
                return true;
            else
                return false;
        }

        /// <summary>
        /// PopulateWhere to validate email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            if (Regex.IsMatch(email, @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$") == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// method to check all fields to get price quote should be filled and valid
        /// Modified By : Sadhana Upadhyay on 28th Oct 2014
        /// Summary : removed validation of unwanted fields
        /// </summary>
        /// <returns></returns>
        private bool IsPQDetailsValid()
        {
            bool retVal = false;
            errMsg = "";

            if (String.IsNullOrEmpty(hdn_ddlCity.Value) || hdn_ddlCity.Value == "0")
            {
                retVal = true;
                spnCity.InnerText = "Required";
            }
            else
                spnCity.InnerText = string.Empty;

            if (hdnIsAreaShown.Value == "true")
            {
                if (String.IsNullOrEmpty(hdn_ddlArea.Value) || hdn_ddlArea.Value == "0")
                {
                    retVal = true;
                    spnArea.InnerText = "Required";
                }
                else
                    spnArea.InnerText = string.Empty;
            }
            else
            {
                hdn_ddlArea.Value = string.Empty;
            }

            if (!userAgreement.Checked)
            {
                retVal = true;
                spnAgree.InnerText = "You must agree with the BikeWale Visitor Agreement and Privacy Policy to continue.";
            }
            else
                spnAgree.InnerText = string.Empty;

            return !retVal;
        }
        #endregion
    }   // class
}   // namespace