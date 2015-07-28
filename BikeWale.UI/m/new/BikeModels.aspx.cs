using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using Bikewale.BAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Common;
using Bikewale.Mobile.Controls;
using Bikewale.Entities.UserReviews;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Core;
using Bikewale.Cache.BikeData;
using Bikewale.DAL.BikeData;

namespace Bikewale.Mobile
{
    public class BikeModels : System.Web.UI.Page
    {
        protected int modelId = 0, versionCount = 0, modelCount = 0, count = 0;
        protected uint versionId = 0;
        protected string versionName = string.Empty, errMsg = string.Empty, formattedPrice = string.Empty;
        
        protected Repeater rptVersions;

        protected BikeModelEntity objModelEntity = null;
        protected BikeDescriptionEntity objDesc = null;
        protected UpcomingBikeEntity objUpcomingBike = null;
        protected BikeSpecificationEntity objSpecs = null;
        protected DropDownList drpVersion, ddlStates;
        protected TopUserReviews reviewList;
        protected EnumBikeType bikeType;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProcessQueryString())
            {
                if (!IsPostBack)
                {
                    BindVersionsDDL();
                    BindStates();
                }

                BindModelData();
                
            }
            else
            {
                Response.Redirect("/m/pagenotfound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        private void BindStates()
        {
            try
            {
                StateCity objStates = new StateCity();

                ddlStates.DataSource = objStates.GetStates();
                ddlStates.DataTextField = "Text";
                ddlStates.DataValueField = "Value";
                ddlStates.DataBind();
                ddlStates.Items.Insert(0, new ListItem("--Select State--","0"));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void BindModelData()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                    IBikeModels<BikeModelEntity, int> objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();

                    //Get Model details
                    objModelEntity = objModel.GetById(modelId);

                    if(objModelEntity.MinPrice > 0)
                        formattedPrice =  CommonOpn.FormatNumeric(objModelEntity.MinPrice.ToString());

                    if (objModelEntity.MinPrice > 0 && objModelEntity.MinPrice != objModelEntity.MaxPrice)
                        formattedPrice +=  "-" + CommonOpn.FormatNumeric(objModelEntity.MaxPrice.ToString());

                    formattedPrice = String.IsNullOrEmpty(formattedPrice) ? "N/A" : formattedPrice;

                    reviewList.ModelId = Convert.ToUInt32(modelId);
                    reviewList.ModelMaskingName = objModelEntity.MaskingName;
                    reviewList.MakeMaskingName = objModelEntity.MakeBase.MaskingName;
                    reviewList.TopCount = 3;
                    reviewList.Filter = FilterBy.MostRecent;
                    reviewList.HeaderText = objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " Reviews";

                    // Get Model synopsis
                    objDesc = objModel.GetModelSynopsis(modelId);
                    
                    if (!objModelEntity.Futuristic)
                    {
                        bikeType = objModelEntity.New ? EnumBikeType.New : EnumBikeType.Used;

                        // Get specs of the version
                        container.RegisterType<IBikeVersions<BikeVersionEntity, int>, BikeVersions<BikeVersionEntity, int>>();
                        IBikeVersions<BikeVersionEntity, int> objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, int>>();

                        // Get VersionsList
                        List<BikeVersionsListEntity> objVersionsList = objVersion.GetVersionsByType(bikeType, modelId);
                        versionCount = objVersionsList.Count;

                        if (versionCount > 0)
                        {
                            rptVersions.DataSource = objVersionsList;
                            rptVersions.DataBind();

                            if ((int)ViewState["VersionCount"] > 0)
                            {
                                versionId = Convert.ToUInt32(drpVersion.SelectedValue);
                                versionName = drpVersion.SelectedItem.Text;

                                objSpecs = objVersion.GetSpecifications(Convert.ToInt32(versionId));
                            }
                        }
                    }
                    else
                    {
                        // If bike is upcoming get upcoming bike details
                        objUpcomingBike = objModel.GetUpcomingBikeDetails(modelId);
                        Trace.Warn("price", objUpcomingBike.ExpectedLaunchId.ToString());
                    }

                    
                    container.RegisterType<IBikeSeries<BikeSeriesEntity, int>, BikeSeries<BikeSeriesEntity, int>>();
                    IBikeSeries<BikeSeriesEntity, int> objSeries = container.Resolve<IBikeSeries<BikeSeriesEntity, int>>();

                    Trace.Warn("objModelEntity.ModelSeries.SeriesId", objModelEntity.ModelSeries.SeriesId.ToString());
                    BikeSeriesEntity objSeriesEntity = objSeries.GetById(objModelEntity.ModelSeries.SeriesId);
                    Trace.Warn(objSeriesEntity.ModelCount.ToString());
                    modelCount = objSeriesEntity.ModelCount;

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

		// Modified By : Sadhana Upadhyay on 25 Aug 2014 to get version whose specifications are available
        protected void BindVersionsDDL()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                    IBikeModels<BikeModelEntity, int> objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();

                    //Get Model details
                    objModelEntity = objModel.GetById(modelId);

                    bikeType = objModelEntity.New ? EnumBikeType.NewBikeSpecs : EnumBikeType.UsedBikeSpecs;

                    container.RegisterType<IBikeVersions<BikeVersionEntity,int>,BikeVersions<BikeVersionEntity,int>>();
                    IBikeVersions<BikeVersionEntity, int> objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, int>>();

                    List<BikeVersionsListEntity> ddlVersionsList = objVersion.GetVersionsByType(bikeType,modelId);

                    count = ddlVersionsList.Count;
                    ViewState["VersionCount"] = count;
                    Trace.Warn("list", count.ToString());
                    drpVersion.DataSource = ddlVersionsList;
                    drpVersion.DataValueField = "VersionId";
                    drpVersion.DataTextField = "VersionName";
                    drpVersion.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private bool ProcessQueryString()
        {

            bool isSuccess = true;

            //Modified By : Ashwini Todkar on 19 Jan 2015

            if (!string.IsNullOrEmpty(Request.QueryString["model"]))
            {
                ModelMaskingResponse objResponse = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                    objResponse = objCache.GetModelMaskingResponse(Request.QueryString["model"]);

                    if (objResponse != null && objResponse.StatusCode == 200)
                    {
                        modelId = Convert.ToInt32(objResponse.ModelId);
                    }
                    else
                    {
                        if (objResponse.StatusCode == 301)
                        {
                            //redirect permanent to new page 
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(Request.QueryString["model"], objResponse.MaskingName));
                        }
                        else
                        {
                            Response.Redirect("/m/pagenotfound.aspx", true);
                            isSuccess = false;
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("/m/pagenotfound.aspx", true);
                isSuccess = false;
            }

            return isSuccess;

            //bool isSuccess = true;

            //if (!String.IsNullOrEmpty(Request.QueryString["model"]))
            //{
            //    ModelMapping objMapping = new ModelMapping();

            //    string _tmpModelId = objMapping.GetModelId(Request.QueryString["model"].ToLower());

            //    if (String.IsNullOrEmpty(_tmpModelId))
            //    {
            //        isSuccess = false;
            //    }
            //    else
            //    {
            //        modelId = Convert.ToInt32(_tmpModelId);
            //    }
            //}
            //else
            //{
            //    isSuccess = false;
            //}

            //return isSuccess;
        }

        protected string ShowNotAvailable(string value)
        {
            if (String.IsNullOrEmpty(value) || value == "0")
            {
                return "--";
            }
            else
            {
                return value;
            }
        }

        protected string GetFeatures(string featureValue)
        {
            string showValue = String.Empty;

            if (String.IsNullOrEmpty(featureValue))
            {
                showValue = "--";
            }
            else
            {
                showValue = featureValue == "True" ? "Yes" : "No";
            }
            return showValue;
        }   // End of GetFeatures method


        /// <summary>
        /// method to check all fields to get price quote should be filled and valid
        /// </summary>
        /// <returns></returns>
        //private bool IsPQDetailsValid()
        //{
        //    bool retVal = true;
        //    errMsg = "";

        //    //if (String.IsNullOrEmpty(Request.QueryString["model"]))
        //    //{
        //    if (String.IsNullOrEmpty(ddlStates.SelectedValue) || Convert.ToInt32(ddlStates.SelectedValue) <= 0)
        //    {
        //        retVal = false;
        //        errMsg = errMsg + "Select State</br>";
        //    }
        //    //}

        //    if (String.IsNullOrEmpty(hdnCity.Value) || Convert.ToInt32(hdnCity.Value) <= 0)
        //    {
        //        retVal = false;
        //        errMsg = errMsg + "Select City</br>";
        //    }
        

        //    if (String.IsNullOrEmpty(txtName.Text.Trim()))
        //    {
        //        retVal = false;
        //        errMsg = errMsg + "Enter Your Name<br>";
        //    }

        //    if (String.IsNullOrEmpty(txtEmail.Text.Trim()))
        //    {
        //        retVal = false;
        //        errMsg = errMsg + "Enter Your Email</br>";

        //    }
        //    else
        //    {
        //        if (IsValidEmail(txtEmail.Text.Trim().ToLower()) == false)
        //        {
        //            retVal = false;
        //            errMsg = errMsg + "Enter valid email</br>";
        //        }
        //    }

        //    if (String.IsNullOrEmpty(hdnMobile.Value))
        //    {
        //        retVal = false;
        //        errMsg = errMsg + "Enter Your Mobile.";
        //    }
        //    else
        //    {
        //        if (IsValidMobile(hdnMobile.Value) == false)
        //        {
        //            retVal = false;
        //            errMsg = errMsg + "Enter valid mobile number</br>";
        //        }
        //    }

        //    return retVal;
        //}
    }   // class
}   // namespace