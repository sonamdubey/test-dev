using Bikewale.Common;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.New;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;

namespace Bikewale.Mobile.New
{
	public class CompareBikeDetails : System.Web.UI.Page
	{
        protected int count = 0, totalComp = 3, version1 = 0, version2 = 0;
        protected string versions = string.Empty, targetedModels = string.Empty;
        protected DataSet ds = null;
        protected DataTable bikeDetails = null, bikeSpecs = null, bikeFeatures = null;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                getVersionIdList();
                GetCompareBikeDetails();
                Trace.Warn("version List",versions);
                if (count < 2)
                {
                    Response.Redirect("/m/comparebikes/", false);//return;	
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 Sept 2014
        /// Summary : To get version list from querystring and memcache
        /// </summary>
        protected void getVersionIdList()
        {
            string QueryString = Request.QueryString.ToString();

            string _newUrl = string.Empty;
            ModelMaskingResponse objResponse = null;

            if (QueryString.Contains("bike"))
            {
                for (int i = 1; i < totalComp; i++)
                {
                    if (!String.IsNullOrEmpty(Request["bike" + i]) && CommonOpn.CheckId(Request["bike" + i]) && Request["bike" + i].ToString() != "0")
                    {
                        versions += Request["bike" + i] + ",";
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(HttpUtility.ParseQueryString(QueryString).Get("mo")))
                {
                    string[] models = HttpUtility.ParseQueryString(QueryString).Get("mo").Split(',');

                    ModelMapping objCache = new ModelMapping();

                    for (int iTmp = 0; iTmp < models.Length; iTmp++)
                    {
                        objResponse = IsMaskingNameChanged(models[iTmp].ToLower());

                        if (objResponse != null && objResponse.StatusCode == 200)
                        {
                            versions += objCache.GetTopVersionId(models[iTmp].ToLower()) + ",";
                        }
                        else
                        {
                            if (objResponse != null && objResponse.StatusCode == 301)
                            {
                                if (String.IsNullOrEmpty(_newUrl))
                                    _newUrl = Request.RawUrl.Replace(models[iTmp].ToLower(), objResponse.MaskingName);
                                else
                                    _newUrl = _newUrl.Replace(models[iTmp].ToLower(), objResponse.MaskingName);
                            }
                            else
                            {
                                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                            }
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(_newUrl))
            {
                //redirect permanent to new page 
                CommonOpn.RedirectPermanent(_newUrl);
            }

            if (versions.Length > 0)
            {
                versions = versions.Substring(0, versions.Length - 1);
            }
        }   //End of getVersionIdList

        /// <summary>
        /// Written By : Ashwini Todkar on 22 Jan 2014
        /// PopulateWhere to check masking name of model is changed or not
        /// </summary>
        /// <param name="maskingName"></param>
        /// <returns></returns>
        private ModelMaskingResponse IsMaskingNameChanged(string maskingName)
        {
            ModelMaskingResponse objResponse = null;

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                         .RegisterType<ICacheManager, MemcacheManager>()
                         .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                objResponse = objCache.GetModelMaskingResponse(maskingName);
            }

            return objResponse;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 Sept 2014
        /// Summary : Get compare Bike detail by version id
        /// </summary>
        protected void GetCompareBikeDetails()
        {
            try
            {
                CompareBikes cb = new CompareBikes();
                ds = cb.GetComparisonBikeListByVersion(versions);
                if (ds != null)
                {
                    bikeDetails = ds.Tables[0];
                    bikeSpecs = ds.Tables[1];
                    bikeFeatures = ds.Tables[2];
                }
                count = bikeDetails.Rows.Count;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    targetedModels += ds.Tables[0].Rows[i]["Model"] + ",";
                }
                if (targetedModels.Length > 2)
                {
                    targetedModels = targetedModels.Substring(0, targetedModels.Length - 1).ToLower();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of GetCompareBikeDetails

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 Sept 2014
        /// Summary : to get model color by versionId
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        protected string GetModelColors(string versionId)
        {
            string colorString = String.Empty;

            DataView dv = ds.DefaultViewManager.CreateDataView(ds.Tables[3]);
            dv.Sort = "BikeVersionId";
            DataRowView[] drv = dv.FindRows(versionId);

            if (drv.Length > 0)
            {
                Trace.Warn("drv data .............", drv.Length.ToString());

                for (int jTmp = 0; jTmp < drv.Length; jTmp++)
                {
                    colorString += "<div class='colorBox' style='border: 1px solid #e9e9e9;padding-top:5px;background-color:#" + drv[jTmp].Row["HexCode"].ToString() + "'></div>"
                                + "<div class='new-line'>" + drv[jTmp].Row["Color"].ToString() + "</div>";
                }
            }
            return colorString;
        }

        protected string ShowFormatedData(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return "--";
            }
            else
            {
                return value;
            }
        }

        public string ShowFeature(string featureValue)
        {
            string adString = "";

            if (String.IsNullOrEmpty(featureValue))
                return "--";

            switch (featureValue)
            {
                case "True":
                    adString = "<img align=\"absmiddle\" src=\"http://img.carwale.com/images/icons/tick.gif\" />";
                    break;
                case "False":
                    adString = "<img align=\"absmiddle\" src=\"http://img.carwale.com/images/icons/delete.ico\" />";
                    break;
                default:
                    adString = "-";
                    break;
            }
            return adString;
        }   // End of ShowFeature method
	}   //End of Class
}   //End of namespace