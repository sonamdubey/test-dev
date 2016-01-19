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
using Bikewale.Interfaces.Location;
using Bikewale.DAL.Location;
using Bikewale.Entities.Location;
using Bikewale.Entities.BikeBooking;
using Bikewale.Mobile.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.BAL.BikeBooking;
using Bikewale.Utility;

namespace Bikewale.Mobile.PriceQuote
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Class to get the price quote of the bike.
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected DropDownList ddlMake, ddlModel, /*ddlVersion,*/ ddlCity, ddlArea;
        protected HiddenField hdnmodel;
        protected TextBox txtMake, txtModel, /*txtVersion,*/ txtCity, txtArea;
        protected string errMsg = "";
        protected string makeId = String.Empty, modelId = String.Empty, versionId = "0", modelName = string.Empty, makeName = string.Empty;
        protected LinkButton btnSubmit;
        protected HtmlInputHidden hdnIsAreaShown;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            this.btnSubmit.Click += new EventHandler(SavePriceQuote);
        }

        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindMakes();

                    ddlModel.Items.Insert(0, new ListItem("--Select Model--", "0"));
                    //ddlVersion.Items.Insert(0, new ListItem("--Select Version--", "0"));
                    ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));

                    if (ProcessQueryString())
                    {
                        MakeModelVersion mmv = new MakeModelVersion();

                        //if query string contains model then get model details
                        if (!String.IsNullOrEmpty(modelId))
                        {
                            mmv.GetModelDetails(modelId);
                        }

                        ////if query string contains version then get version details
                        //if (!String.IsNullOrEmpty(versionId))
                        //{
                        //    mmv.GetVersionDetails(versionId);
                        //    modelId = mmv.ModelId;
                        //}

                        if (!string.IsNullOrEmpty(mmv.MakeId))
                        {
                            makeId = mmv.MakeId;
                            txtMake.Text = makeId;
                            modelName = mmv.Model;
                            makeName = mmv.Make;

                            if (BindModelsDropdownList(makeId))
                            {
                                //BindVersionsDropdownList(modelId);
                                BindCitiesDropdownList(modelId);

                                ddlMake.SelectedValue = makeId;
                                ddlModel.SelectedValue = modelId;

                                //ddlVersion.SelectedValue = String.IsNullOrEmpty(versionId) ? "0" : versionId;

                                //DataTable dt = mmv.GetVersions(modelId, "New");
                                //if (dt != null && dt.Rows.Count > 0)
                                //{
                                //    versionId = Convert.ToString(dt.Rows[0]["Value"]);
                                //    txtVersion.Text = versionId;
                                //}

                                ddlModel.Enabled = true;
                                //ddlVersion.Enabled = true;
                                ddlCity.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        ddlModel.Items.Insert(0, new ListItem("--Select Model--", "0"));
                        //ddlVersion.Items.Insert(0, new ListItem("--Select Version--", "0"));
                        ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
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
                #region Old
                //try
                //{
                //    GetPQDetails(ref cityId, ref areaId);

                //    #region Commented
                //    //using (IUnityContainer container = new UnityContainer())
                //    //{
                //    //    // save price quote
                //    //    container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                //    //    IPriceQuote objIPQ = container.Resolve<IPriceQuote>();

                //    //    PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                //    //    objPQEntity.CityId = cityId;
                //    //    objPQEntity.AreaId = areaId > 0 ? areaId : 0;
                //    //    objPQEntity.ClientIP = CommonOpn.GetClientIP();
                //    //    objPQEntity.SourceId = Convert.ToUInt16(Bikewale.Common.Configuration.MobileSourceId);
                //    //    objPQEntity.VersionId = Convert.ToUInt32(versionId);

                //    //    quoteId = objIPQ.RegisterPriceQuote(objPQEntity);

                //    //} 
                //    #endregion

                //    // Get Default versionid for the given modelId
                //    MakeModelVersion mmv = new MakeModelVersion();
                //    DataTable dt = mmv.GetVersions(txtModel.Text, "New");
                //    if (dt != null && dt.Rows.Count > 0)
                //    {
                //        versionId = Convert.ToString(dt.Rows[0]["Value"]);
                //        using (IUnityContainer container = new UnityContainer())
                //        {
                //            // save price quote
                //            container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                //            IPriceQuote objIPQ = container.Resolve<IPriceQuote>();

                //            PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                //            objPQEntity.CityId = cityId;
                //            objPQEntity.AreaId = areaId > 0 ? areaId : 0;
                //            objPQEntity.ClientIP = CommonOpn.GetClientIP();
                //            objPQEntity.SourceId = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["sourceId"]);
                //            objPQEntity.VersionId = Convert.ToUInt32(versionId);
                //            // If pqId exists then, set pqId
                //            quoteId = objIPQ.RegisterPriceQuote(objPQEntity);

                //        }
                //    }

                //}
                //catch (Exception ex)
                //{
                //    string selectedParams = "name : " + name + " : email : " + email + " : mobile : " + mobile
                //                     + " : buyTime : " + buyTime + " : versionId : " + versionId + " : cityid : " + cityId;
                //    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " " + selectedParams);
                //    objErr.SendMail();
                //}
                //finally
                //{

                //    if (quoteId > 0)
                //    {
                //        // Save pq cookie
                //        PriceQuoteCookie.SavePQCookie(cityId.ToString(), quoteId.ToString(), areaId.ToString(), versionId, "");

                //        //redirect to the quotation page
                //        if (hdnIsAreaShown.Value == "true")
                //        {
                //            if (areaId > 0)
                //            {
                //                isDealerExists(Convert.ToUInt32(versionId), Convert.ToUInt32(areaId));
                //            }
                //            else
                //                Response.Redirect("/m/pricequote/quotation.aspx", true);
                //        }
                //        else
                //            Response.Redirect("/m/pricequote/quotation.aspx", true);
                //    }
                //    else
                //    {
                //        Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Sorry, Price for this Version is not available.');", true);
                //    }
                //} 
                #endregion
                GetPQDetails(ref cityId, ref areaId);
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                PQOutputEntity objPQOutput = null;
                try
                {
                    modelId = txtModel.Text;
                    using (IUnityContainer container = new UnityContainer())
                    {
                        // save price quote
                        container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                        IDealerPriceQuote objIPQ = container.Resolve<IDealerPriceQuote>();

                        objPQEntity.CityId = cityId;
                        objPQEntity.AreaId = areaId > 0 ? areaId : 0;
                        objPQEntity.ClientIP = CommonOpn.GetClientIP();
                        objPQEntity.SourceId = Convert.ToUInt16(Bikewale.Common.Configuration.MobileSourceId);
                        objPQEntity.VersionId = Convert.ToUInt32(versionId);
                        objPQEntity.ModelId = Convert.ToUInt32(modelId);
                        // If pqId exists then, set pqId
                        objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Mobile_PQ_Landing);
                        objPQEntity.UTMA = Request.Cookies["__utma"] != null ? Request.Cookies["__utma"].Value : "";
                        objPQEntity.UTMZ = Request.Cookies["__utmz"] != null ? Request.Cookies["__utmz"].Value : "";
                        objPQEntity.DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : "";
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

                    if (objPQOutput.DealerId > 0 && objPQOutput.PQId > 0)
                    {
                        // Save pq cookie
                        //PriceQuoteCookie.SavePQCookie(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString());                        
                        Response.Redirect("~/m/pricequote/dealerpricequote.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString())), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    else if (objPQOutput.PQId > 0)
                    {
                        //PriceQuoteCookie.SavePQCookie(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString());                        
                        Response.Redirect("~/m/pricequote/quotation.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString())), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Sorry, Price for this Version is not available.');", true);
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
                //if (!String.IsNullOrEmpty(txtVersion.Text))
                //    versionId = txtVersion.Text.ToString();
                //else
                //    versionId = "0";
                cityId = Convert.ToUInt32(String.IsNullOrEmpty(txtCity.Text) ? "0" : txtCity.Text);
                areaId = Convert.ToUInt32(String.IsNullOrEmpty(txtArea.Text) ? "0" : txtArea.Text);
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
                ddlMake.DataTextField = "TEXT";
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
                txtModel.Text = modelId;
                hdnmodel.Value = modelId;

            }
            //if (!String.IsNullOrEmpty(Request.QueryString["version"]) && CommonOpn.CheckId(Request["version"]))
            //{
            //    versionId = Request.QueryString["version"];
            //    txtVersion.Text = versionId;
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
            bool retVal = true;
            errMsg = "";

            if (String.IsNullOrEmpty(Request.QueryString["model"]))
            {
                if (String.IsNullOrEmpty(ddlMake.SelectedValue) || Convert.ToInt32(ddlMake.SelectedValue) <= 0)
                {
                    retVal = false;
                    errMsg = errMsg + "Select Make</br>";
                }
            }
            if (String.IsNullOrEmpty(txtModel.Text) || Convert.ToInt32(txtModel.Text) <= 0)
            {
                retVal = false;
                errMsg = errMsg + "Select Model</br>";
            }

            if (String.IsNullOrEmpty(txtCity.Text) || Convert.ToInt32(txtCity.Text) <= 0)
            {
                retVal = false;
                errMsg = errMsg + "Select City</br>";
            }
            if (hdnIsAreaShown.Value == "true")
            {
                if (String.IsNullOrEmpty(txtArea.Text) || Convert.ToInt32(txtArea.Text) <= 0)
                {
                    retVal = false;
                    errMsg = errMsg + "Select Area</br>";
                }
            }

            return retVal;
        }
        #endregion
    }   // class
}   // namespace