﻿using Bikewale.common;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Memcache;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Dated : 15 Feb 2016
    /// Summary : functionality for estimated Price and Launch Date Added.
    /// Modified By : Sangram Nandkhile on 30 Nov
    /// Summary: Added new flag, isSponsored to add a link if Sponsored makeid matches with the config
    /// </summary>
    public class comparebikes : System.Web.UI.Page
    {
        protected Repeater rptCommon, rptSpecs, rptFeatures, rptColors;
        protected HtmlAnchor delComp;
        protected Literal ltrTitle;
        protected AddBikeToCompare addBike;
        DataSet ds = null;
        protected string versions = string.Empty, featuredBikeId = string.Empty, title = string.Empty, pageTitle = string.Empty, keyword = string.Empty, canonicalUrl = string.Empty, targetedModels = string.Empty,
            estimatePrice = string.Empty, estimateLaunchDate = string.Empty, knowMoreHref = string.Empty;
        protected int count = 0, totalComp = 5;
        public int featuredBikeIndex = 0;
        protected bool isFeatured = false, isSponsored = false;
        protected Int16 trSize = 45, sponsoredModelId = 0;
        public SimilarCompareBikes ctrlSimilarBikes;
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
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (!IsPostBack)
            {
                getVersionIdList();
                BindRepeater();
                if (count < 2)
                {
                    Response.Redirect("/comparebikes/", false);//return;	
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                ltrTitle.Text = title;
                pageTitle = title;
                BindSimilarCompareBikes(versions);
            }
        }

        /// <summary>
        /// Modified By : Lucky Rathore
        /// Dated : 15 Feb 2016
        /// Summary : functinality for estimated Price and Launch Date Added.
        /// </summary>
        void BindRepeater()
        {
            try
            {

                CompareBikes cb = new CompareBikes();

                ds = cb.GetComparisonBikeListByVersion(versions);
                count = ds.Tables[0].Rows.Count;

                //to truncate data
                if (count == 3) trSize = 40;
                else if (count == 4) trSize = 35;
                else if (count == 5) trSize = 30;

                if (isFeatured && ds.Tables[4].Rows != null && ds.Tables[4].Rows.Count > 0)
                {
                    estimatePrice = CommonOpn.FormatPrice(Convert.ToString(ds.Tables[4].Rows[0]["EstimatedPriceMin"]));
                    estimateLaunchDate = ds.Tables[4].Rows[0]["ExpectedLaunch"].ToString(); //TODO: Change Date fomate.
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    rptCommon.DataSource = ds.Tables[0];
                    rptCommon.DataBind();

                    rptSpecs.DataSource = ds.Tables[1];
                    rptSpecs.DataBind();

                    rptFeatures.DataSource = ds.Tables[2];
                    rptFeatures.DataBind();

                    rptColors.DataSource = ds.Tables[0];
                    rptColors.DataBind();
                }



                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    title += ds.Tables[0].Rows[i]["Bike"].ToString() + " vs ";
                    keyword += ds.Tables[0].Rows[i]["Bike"].ToString() + " and ";
                    canonicalUrl += ds.Tables[0].Rows[i]["MakeMaskingName"] + "-" + ds.Tables[0].Rows[i]["ModelMaskingName"] + "-vs-";
                    Trace.Warn("Bike Name : ", title);
                    targetedModels += ds.Tables[0].Rows[i]["Model"] + ",";
                }

                if (title.Length > 2)
                {
                    title = title.Substring(0, title.Length - 3);
                    keyword = keyword.Substring(0, keyword.Length - 5);
                    canonicalUrl = canonicalUrl.Substring(0, canonicalUrl.Length - 4);
                    targetedModels = targetedModels.Substring(0, targetedModels.Length - 1).ToLower();
                }

                if (isFeatured)
                {
                    title = title.Substring(0, title.LastIndexOf(" vs "));
                    keyword = keyword.Substring(0, keyword.LastIndexOf(" and "));
                    canonicalUrl = canonicalUrl.Substring(0, canonicalUrl.LastIndexOf("-vs-"));

                    // Added by Sangram Nandkhile on 30 Nov
                    // To check if sponsored model id matches with 
                    string modelId = ds.Tables[0].Rows[count - 1]["ModelId"].ToString();
                    if (Int16.TryParse(modelId, out sponsoredModelId))
                    {
                        string sponsoredModelIds = Bikewale.Utility.BWConfiguration.Instance.SponsoredModelId;
                        string[] modelArray = sponsoredModelIds.Split(',');
                        if (modelArray.Contains(modelId))
                        {
                            isSponsored = true;
                            knowMoreHref = Bikewale.Utility.SponsoredCampaigns.FetchValue(modelId);
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "new.default.LoadMakes");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "new.default.LoadMakes");
                objErr.SendMail();
            }
        }


        protected void getVersionIdList()
        {
            string QueryString = Request.QueryString.ToString();

            if (QueryString.Contains("bike"))
            {
                for (int i = 1; i < totalComp; i++)
                {
                    if (!String.IsNullOrEmpty(Request["bike" + i]) && CommonOpn.CheckId(Request["bike" + i]) && Request["bike" + i].ToString() != "0")
                    {
                        versions += Request["bike" + i] + ",";
                        featuredBikeIndex++;
                    }
                    else
                    {
                        Trace.Warn("QS EMPTY");
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(HttpUtility.ParseQueryString(QueryString).Get("mo")))
                {
                    string[] models = HttpUtility.ParseQueryString(QueryString).Get("mo").Split(',');

                    ModelMapping objCache = new ModelMapping();

                    string _newUrl = string.Empty;
                    ModelMaskingResponse objResponse = null;

                    for (int iTmp = 0; iTmp < models.Length; iTmp++)
                    {

                        objResponse = new ModelHelper().GetModelDataByMasking(models[iTmp].ToLower());

                        if (objResponse != null && objResponse.StatusCode == 200)
                        {
                            versions += objCache.GetTopVersionId(models[iTmp].ToLower()) + ",";
                            featuredBikeIndex++;
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

                    if (!String.IsNullOrEmpty(_newUrl))
                    {
                        //redirect permanent to new page 
                        CommonOpn.RedirectPermanent(_newUrl);
                    }
                }
            }

            if (versions.Length > 0)
            {
                versions = versions.Substring(0, versions.Length - 1);
                // Get version id of the featured bike on the basis of versions selected for comparison
                // There might be multiple featured Bikes available. But only show top 1
                string featuredBike = CompareBikes.GetFeaturedBike(versions);
                if (featuredBike != "-1")
                {
                    featuredBikeId = featuredBike;
                    isFeatured = true;
                }
                // If featured bike available to show.
                // Check if featured bike is already selected by the user.
                if (featuredBikeId != "")
                {
                    versions += "," + featuredBikeId;
                }
            }
            addBike.VersionId = versions;
            addBike.IsFeatured = isFeatured;
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
                    adString = "<span class=\"bwsprite compare-tick\"></span>";
                    break;
                case "False":
                    adString = "<span class=\"bwsprite compare-cross\"></span>";
                    break;
                default:
                    adString = "-";
                    break;
            }
            return adString;
        }   // End of ShowFeature method



        public string GetModelRatings(string versionId)
        {
            string sql = "";

            sql = @" select (select maskingname from bikemakes   where id = cv.bikemakeid) as makemaskingname, cv.bikemodelid as modelid, cv.modelname as modelname,cv.modelmaskingname as modelmaskingname, ifnull(cv.modelreviewrate, 0) as modelrate, ifnull(cv.modelreviewcount, 0) as modeltotal, 
                ifnull(cv.reviewrate, 0) as versionrate, ifnull(cv.reviewcount, 0) as versiontotal 
                from bikeversions as cv   where cv.id = " + versionId;


            string reviewString = "";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    using (IDataReader dr = MySqlDatabase.SelectQuery(sql, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                if (Convert.ToDouble(dr["ModelRate"]) > 0)
                                {
                                    string reviews = Convert.ToDouble(dr["ModelTotal"]) > 1 ? " reviews" : " review";
                                    reviewString += "<div class='margin-top10'>" + CommonOpn.GetRateImage(Convert.ToDouble(dr["ModelRate"].ToString()))
                                                 + " <a style='border-left:1px solid #E2E2E2;' class='margin-left5' href='/" + dr["MakeMaskingName"].ToString() + "-bikes/" + dr["ModelMaskingName"].ToString() + "/user-reviews/'>" + dr["ModelTotal"].ToString() + reviews + " </a></div>";

                                }
                                else
                                    reviewString = "<div style='margin-top:10px;'><a href='/content/userreviews/writereviews.aspx?bikem=" + dr["ModelId"].ToString() + "'>Write a review</a></div>";
                            }
                        }
                        dr.Close();
                    }
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return reviewString;
        }

        protected string GetModelColors(string versionId, int index)
        {
            StringBuilder cs = new StringBuilder();

            var colorData = from r in (ds.Tables[3]).AsEnumerable()
                            where r.Field<int>("BikeVersionId") == Convert.ToInt32(versionId)
                            group r by r.Field<int>("ColorId") into g
                            select g;

            cs.Append("<div style='width:100; text-align:center;padding:5px;'>");
            foreach (var color in colorData)
            {
                cs.AppendFormat("<div class='color-box {0}'>", ((color.Count() >= 3) ? "color-count-three" : (color.Count() == 2) ? "color-count-two" : "color-count-one"));
                IList<string> HexCodeList = new List<string>();
                foreach (var colorList in color)
                {
                    cs.AppendFormat("<span style='background-color:#{0}'></span>", colorList.ItemArray[5]);
                }
                cs.AppendFormat("</div><div style='padding-top:3px;'>{0}</div></div>", color.FirstOrDefault().ItemArray[3]);
            }

            return cs.ToString();

        }

        private void BindSimilarCompareBikes(string verList)
        {
            ctrlSimilarBikes.TopCount = 4;
            ctrlSimilarBikes.versionsList = verList;

        }

    }
}