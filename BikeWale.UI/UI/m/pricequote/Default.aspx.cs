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

                                ddlModel.Enabled = true;
                                //ddlVersion.Enabled = true;
                                ddlCity.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        ddlModel.Items.Insert(0, new ListItem("--Select Model--", "0"));
                        ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
        }

        /// <summary>
        /// method to save PQ details and also register customer if new
        /// Modified By : Vivek Gupta on 02-05-2016
        /// Desc : redirection condition isDealerAvailbale added
        /// Modified By : Lucky Rathore on 27 June 2016
        /// Description : replace cookie __utmz with _bwutmz
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
                        objPQEntity.UTMZ = Request.Cookies["_bwutmz"] != null ? Request.Cookies["_bwutmz"].Value : "";
                        objPQEntity.DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : "";
                        objPQEntity.ManufacturerCampaignPageId = ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_DealerPriceQuote;
                        objPQOutput = objIPQ.ProcessPQV2(objPQEntity, true);

                    }
                }
                catch (Exception ex)
                {
                    string selectedParams = string.Format("modelId : {0}, CityId : {1}", modelId, cityId);
                    ErrorClass.LogError(ex, Request.ServerVariables["URL"] + " " + selectedParams);
                    
                }
                finally
                {
                    if (!string.IsNullOrEmpty(objPQOutput.PQId))
                    {
                        Response.Redirect("~/m/pricequote/dealer/?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString())), false);
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
                cityId = Convert.ToUInt32(String.IsNullOrEmpty(txtCity.Text) ? "0" : txtCity.Text);
                areaId = Convert.ToUInt32(String.IsNullOrEmpty(txtArea.Text) ? "0" : txtArea.Text);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
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
                mmv.GetMakes(EnumBikeType.PriceQuote, ref ddlMake);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
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
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
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
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
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
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
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