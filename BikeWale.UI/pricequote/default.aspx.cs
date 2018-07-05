using Bikewale.BAL.BikeBooking;
using Bikewale.Common;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Location;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

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
                Bikewale.Notifications.ErrorClass.LogError(ex, Request.ServerVariables["URL"]);

            }
        }

        /// <summary>
        /// method to save PQ details and also register customer if new
        /// Modified By : Vivek Gupta on 29-04-2016
        /// Desc : In case of dealerId=0 and isDealerAvailable = true , while redirecting to pricequotes ,don't redirect to BW PQ redirect to dpq
        /// Modified By : Lucky Rathore on 27 June 2016
        /// Description : replace cookie __utmz with _bwutmz
        /// Modified By : Sushil Kumar on 21st December 2016
        /// Description : All pricequote details should be handled on dealerpricequote page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SavePriceQuote(object sender, EventArgs e)
        {
            uint cityId = 0, areaId = 0;
            if (IsPQDetailsValid())
            {

                GetPQDetails(ref cityId, ref areaId);
                Entities.PriceQuote.v2.PriceQuoteParametersEntity objPQEntity = new Entities.PriceQuote.v2.PriceQuoteParametersEntity();
                Bikewale.Entities.BikeBooking.v2.PQOutputEntity objPQOutput = null;
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
                        objPQEntity.UTMA = Request.Cookies["__utma"] != null ? Request.Cookies["__utma"].Value : "";
                        objPQEntity.UTMZ = Request.Cookies["_bwutmz"] != null ? Request.Cookies["_bwutmz"].Value : "";
                        objPQEntity.DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : "";
                        objPQEntity.ManufacturerCampaignPageId = ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Desktop_DealerPriceQuote;
                        objPQOutput = objIPQ.ProcessPQV2(objPQEntity, true);

                    }
                }
                catch (Exception ex)
                {
                    string selectedParams = string.Format("modelId : {0}, CityId : {1}", modelId, cityId);
                    Bikewale.Notifications.ErrorClass.LogError(ex, Request.ServerVariables["URL"] + " " + selectedParams);

                }
                finally
                {

                    if (!string.IsNullOrEmpty(objPQOutput.PQId))
                    {
                        // Save pq cookie
                        //PriceQuoteCookie.SavePQCookie(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString());                        
                        Response.Redirect("/pricequote/dealer/?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString())), false);
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
                Bikewale.Notifications.ErrorClass.LogError(ex, Request.ServerVariables["URL"]);

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

                ////ddlMake.DataSource = mmv.GetMakes("PRICEQUOTE");
                //ddlMake.DataSource = mmv.GetMakes(EnumBikeType requestType);
                //ddlMake.DataValueField = "Value";
                //ddlMake.DataTextField = "Text";
                //ddlMake.DataBind();
                //ddlMake.Items.Insert(0, (new ListItem("--Select Make--", "0")));

                mmv.GetMakes(EnumBikeType.PriceQuote, ref ddlMake);

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, Request.ServerVariables["URL"]);

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
                Bikewale.Notifications.ErrorClass.LogError(ex, Request.ServerVariables["URL"]);

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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>();
                    ICity cityRepository = container.Resolve<ICity>();

                    objCities = cityRepository.GetPriceQuoteCities(Convert.ToUInt32(modelId));

                    if (objCities != null)
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
                Bikewale.Notifications.ErrorClass.LogError(ex, Request.ServerVariables["URL"]);

            }
        }   // End of BindMakesDropdownList method


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