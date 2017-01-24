﻿using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
using Bikewale.New;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Bikewale.Mobile.New
{
    public class CompareBikeDetails_v2 : System.Web.UI.Page
    {
        protected int count = 0, totalComp = 3, version1 = 0, version2 = 0;
        protected string versions = string.Empty, targetedModels = string.Empty;
        protected DataSet ds = null;
        protected DataTable bikeDetails = null, bikeSpecs = null, bikeFeatures = null;
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
                getVersionIdList();
                GetCompareBikeDetails();
                Trace.Warn("version List", versions);
                if (count < 2)
                {
                    Response.Redirect("/m/comparebikes/", false);//return;	
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }


                BindSimilarCompareBikes(versions);

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
                    targetedModels += "\"" + ds.Tables[0].Rows[i]["Model"] + "\",";
                }
                if (targetedModels.Length > 2)
                {
                    targetedModels = targetedModels.Substring(0, targetedModels.Length - 1);
                }

                if (count > 1)
                {
                    if (Convert.ToUInt32(bikeDetails.Rows[0]["bikeCount"]) > 0 || Convert.ToUInt32(bikeDetails.Rows[1]["bikeCount"]) > 0)
                    {
                        isUsedBikePresent = true;
                    }
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
        /// Modified By : Aditi Srivastava on 15 Dec 2016
        /// Description : FIxed html formatting for colors displayed
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        protected string GetModelColors(string versionId)
        {
            StringBuilder cs = new StringBuilder();

            var colorData = from r in (ds.Tables[3]).AsEnumerable()
                            where r.Field<int>("BikeVersionId") == Convert.ToInt32(versionId)
                            group r by r.Field<int>("ColorId") into g
                            select g;


            foreach (var color in colorData)
            {
                cs.AppendFormat("<div style='text-align:center;' class='color-box {0}'>", ((color.Count() >= 3) ? "color-count-three" : (color.Count() == 2) ? "color-count-two" : "color-count-one"));
                IList<string> HexCodeList = new List<string>();
                foreach (var colorList in color)
                {
                    cs.AppendFormat("<span style='background-color:#{0}'></span>", colorList.ItemArray[5]);   //5 is for hexcode
                }
                cs.AppendFormat("</div><div style='padding-top:3px;'>{0}</div>", color.FirstOrDefault().ItemArray[3]);    //3 is for colorName
            }

            return cs.ToString();
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
        protected string CreateUsedBikeLink(uint bikeCount, string make, string makeMaskingName, string model, string modelMaskingName, string minPrice)
        {
            if (bikeCount == 0)
                return "--";
            else
            {
                isUsedBikePresent = true;
                return string.Format("<a href='/m/used/{1}-{2}-bikes-in-india/' title='Used {3}'>{0} Used {3}</a><p>Starting at Rs. {4} </p>",
                    bikeCount, makeMaskingName, modelMaskingName, string.Format("{0} {1}", make, model), minPrice);
            }
        }
    }   //End of Class
}   //End of namespace