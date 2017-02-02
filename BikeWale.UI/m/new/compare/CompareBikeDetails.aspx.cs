using Bikewale.BindViewModels.Webforms.Compare;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;

namespace Bikewale.Mobile.New
{
    public class CompareBikeDetails : System.Web.UI.Page
    {


        protected PageMetaTags pageMetas = null;
        protected GlobalCityAreaEntity cityArea;
        protected BikeCompareEntity vmCompare = null;

        /// ********************************************************************/
        protected int count = 0, totalComp = 3, version1 = 0, version2 = 0;
        protected string versions = string.Empty, targetedModels = string.Empty;

        public SimilarCompareBikes ctrlSimilarBikes;
        protected bool isUsedBikePresent;


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
                cityArea = GlobalCityArea.GetGlobalCityArea();

                BindCompareBikes();
                //getVersionIdList();
                ////GetCompareBikeDetails();
                //Trace.Warn("version List", versions);
                //if (count < 2)
                //{
                //    Response.Redirect("/m/comparebikes/", false);//return;	
                //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                //    this.Page.Visible = false;
                //}
                //BindSimilarCompareBikes(versions);

            }
        }

        private void BindCompareBikes()
        {
            var objCompare = new CompareBikes();
            try
            {
                if (!objCompare.isPermanentRedirect && !objCompare.isPageNotFound)
                {
                    objCompare.GetComparedBikeDetails();
                    vmCompare = objCompare.comparedBikes;
                    pageMetas = objCompare.PageMetas;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Mobile.New.CompareBikeDetails.BindCompareBikes");
            }
            finally
            {
                if (objCompare.isPermanentRedirect)
                {
                    Bikewale.Common.CommonOpn.RedirectPermanent(objCompare.redirectionUrl);
                }
                else if (objCompare.isPageNotFound)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
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
                                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                                this.Page.Visible = false;
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

        private void BindSimilarCompareBikes(string verList)
        {
            ctrlSimilarBikes.TopCount = 4;
            ctrlSimilarBikes.versionsList = verList;
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
                    adString = "<span class=\"bwmsprite compare-tick\"></span>";
                    break;
                case "False":
                    adString = "<span class=\"bwmsprite compare-cross\"></span>";
                    break;
                default:
                    adString = "-";
                    break;
            }
            return adString;
        }   // End of ShowFeature method

        /// <summary>
        /// Created by: Sangram Nandkhile on 20 Jan 2017
        /// Summary: Create used bike links with bikeCount
        /// </summary>
        /// <returns></returns>
        protected string CreateUsedBikeLink(uint bikeCount, string make, string makeMaskingName, string model, string modelMaskingName, string minPrice, string cityMasking)
        {
            if (bikeCount == 0)
                return "--";
            else
            {
                isUsedBikePresent = true;
                if (cityArea.CityId == 0)
                {
                    return string.Format("<a href='/m/used/{1}-{2}-bikes-in-india/' title='Used {5} bikes'>{0} Used {3}</a><p>Starting at Rs. {4} </p>",
                        bikeCount, makeMaskingName, modelMaskingName, string.Format("{0} {1}", make, model), minPrice, model);
                }
                else
                {
                    return string.Format("<a href='/m/used/{1}-{2}-bikes-in-{5}/' title='Used {6} bikes'>{0} Used {3}</a><p>Starting at Rs. {4} </p>",
                        bikeCount, makeMaskingName, modelMaskingName, string.Format("{0} {1}", make, model), minPrice, cityMasking, model);
                }
            }
        }
    }   //End of Class
}   //End of namespace