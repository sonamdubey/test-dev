using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Memcache;
using Bikewale.Utility;
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
        protected Repeater rptCommon, rptUsedBikes, rptSpecs, rptFeatures, rptColors;
        protected HtmlAnchor delComp;
        protected AddBikeToCompare addBike;
        DataSet ds = null;
        protected GlobalCityAreaEntity cityArea;
        protected string baseUrl, versions = string.Empty, hashVersions = string.Empty, hashModels = string.Empty, featuredBikeId = string.Empty, templateSummaryTitle = string.Empty, pgTitle = string.Empty, pageTitle = string.Empty, keyword = string.Empty, canonicalUrl = string.Empty, targetedModels = string.Empty,
           bikeQueryString = string.Empty, compareBikeText = string.Empty, estimatePrice = string.Empty, estimateLaunchDate = string.Empty, knowMoreHref = string.Empty, featuredBikeName = string.Empty;
        protected int count = 0, totalComp = 5;
        public int featuredBikeIndex = 0;
        protected bool isFeatured = false, isSponsored = false, isUsedBikePresent;
        protected Int16 trSize = 45, sponsoredModelId = 0;
        public SimilarCompareBikes ctrlSimilarBikes;

        private bool isPermRedirectionNeeded = false;
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
            ParseQueryString(originalUrl);
            cityArea = GlobalCityArea.GetGlobalCityArea();
            if (!IsPostBack)
            {
                getVersionIdList();
                BindRepeater();
                if (isPermRedirectionNeeded && canonicalUrl.Length > 0)
                {
                    string urlnew = (string.Format("/comparebikes/{0}/?{1}", canonicalUrl, bikeQueryString));
                    CommonOpn.RedirectPermanent(urlnew);
                }
                if (count < 2)
                {
                    Response.Redirect("/comparebikes/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                pageTitle = pgTitle;
                BindSimilarCompareBikes(versions);
            }
        }

        /// <summary>
        /// Parses the query string.
        /// Created by: Sangram Nandkhile on 26 Apr 2017
        /// </summary>
        /// <param name="originalUrl">The original URL.</param>
        private void ParseQueryString(string originalUrl)
        {
            string[] strArray = originalUrl.Split('/');
            if (strArray.Length > 1)
            {
                baseUrl = strArray[2];
            }
            string[] queryArr = originalUrl.Split('?');
            if (queryArr.Length > 1)
            {
                bikeQueryString = queryArr[1];
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
                ds = cb.GetComparisonBikeListByVersion(versions, cityArea.CityId);
                count = ds.Tables[0].Rows.Count;

                //to truncate data
                if (count == 3) trSize = 40;
                else if (count == 4) trSize = 35;
                else if (count == 5) trSize = 30;

                if (isFeatured && ds.Tables[4].Rows != null && ds.Tables[4].Rows.Count > 0)
                {
                    estimatePrice = CommonOpn.FormatPrice(Convert.ToString(ds.Tables[4].Rows[0]["EstimatedPriceMin"]));
                    estimateLaunchDate = ds.Tables[4].Rows[0]["ExpectedLaunch"].ToString();
                }

                if (count > 0)
                {
                    rptCommon.DataSource = ds.Tables[0];
                    rptCommon.DataBind();

                    rptUsedBikes.DataSource = ds.Tables[0];
                    rptUsedBikes.DataBind();

                    rptSpecs.DataSource = ds.Tables[1];
                    rptSpecs.DataBind();

                    rptFeatures.DataSource = ds.Tables[2];
                    rptFeatures.DataBind();

                    rptColors.DataSource = ds.Tables[0];
                    rptColors.DataBind();
                }


                List<CompareMakeModelEntity> modelList = new List<CompareMakeModelEntity>();
                for (int i = 0; i < count; i++)
                {
                    string bikName = string.Format("{0} {1}", ds.Tables[0].Rows[i]["make"], ds.Tables[0].Rows[i]["model"]);
                    pgTitle += string.Format("{0} vs ", bikName);
                    templateSummaryTitle += string.Format("{0} vs ", ds.Tables[0].Rows[i]["model"].ToString());
                    keyword += bikName + " and ";
                    modelList.Add(new CompareMakeModelEntity { MakeMaskingName = ds.Tables[0].Rows[i]["MakeMaskingName"].ToString(), ModelMaskingName = ds.Tables[0].Rows[i]["ModelMaskingName"].ToString(), ModelId = Convert.ToUInt32(ds.Tables[0].Rows[i]["ModelId"]), VersionId = Convert.ToString(ds.Tables[0].Rows[i]["BikeVersionId"]) });
                    targetedModels += "\"" + ds.Tables[0].Rows[i]["Model"] + "\",";

                }
                if (pgTitle.Length > 2)
                {
                    pgTitle = pgTitle.Substring(0, pgTitle.Length - 3);
                    templateSummaryTitle = templateSummaryTitle.Substring(0, templateSummaryTitle.Length - 3);
                    keyword = keyword.Substring(0, keyword.Length - 5);
                    targetedModels = targetedModels.Substring(0, targetedModels.Length - 1);
                }

                CreateCompareSummary(ds);
                canonicalUrl = CreateCompareUrl(modelList);
                CheckForRedirection(canonicalUrl);
                if (isFeatured)
                {
                    pgTitle = pgTitle.Substring(0, pgTitle.LastIndexOf(" vs "));
                    keyword = keyword.Substring(0, keyword.LastIndexOf(" and "));

                    // Added by Sangram Nandkhile on 30 Nov
                    // To check if sponsored model id matches with 
                    string featuredModelId = ds.Tables[0].Rows[count - 1]["ModelId"].ToString();
                    if (Int16.TryParse(featuredModelId, out sponsoredModelId))
                    {
                        featuredBikeName = string.Format("{0} {1}", ds.Tables[0].Rows[count - 1]["Make"], ds.Tables[0].Rows[count - 1]["Model"]);
                        string sponsoredModelIds = Bikewale.Utility.BWConfiguration.Instance.SponsoredModelId;
                        if (!string.IsNullOrEmpty(sponsoredModelIds))
                        {
                            string[] modelArray = sponsoredModelIds.Split(',');

                            isSponsored = modelArray.Length > 0 && modelArray.Contains(featuredModelId);

                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "new.default.LoadMakes");
            }
        }

        /// <summary>
        /// Creates the compare summary.
        /// </summary>
        /// <param name="ds">The ds.</param>
        private void CreateCompareSummary(DataSet ds)
        {
            try
            {
                string bikeNames = string.Empty, bikePrice = string.Empty, variants = string.Empty, estimatePrice = string.Empty, estimateLaunchDate = string.Empty;
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int count = (ds.Tables[0].Rows.Count - 1);
                    for (int i = 0; i <= count; i++)
                    {
                        string bikName = string.Format("{0} {1}", ds.Tables[0].Rows[i]["make"], ds.Tables[0].Rows[i]["model"]);
                        Int16 versionCount = Convert.ToInt16(ds.Tables[0].Rows[i]["versioncount"].ToString());
                        int versionId = Convert.ToInt32(ds.Tables[0].Rows[i]["bikeversionid"].ToString());
                        string price = CommonOpn.FormatPrice(Convert.ToString(ds.Tables[0].Rows[i]["price"]));
                        var colorData = from r in (ds.Tables[3]).AsEnumerable() where r.Field<int>("BikeVersionId") == versionId group r by r.Field<int>("ColorId") into g select g;
                        int colorCount = colorData != null ? colorData.Count() : 0;
                        bikeNames += bikName + (i < count - 1 ? ", " : " and ");
                        bikePrice += string.Format(" {0} is Rs. {1} {2}", bikName, price, (i < count - 1 ? ", " : " and "));
                        variants += string.Format(" {0} is available in {1} {4} and {2} {5}{3}", bikName, colorCount, versionCount, (i < count - 1 ? ", " : " and "), colorCount > 1 ? "colours" : "colour", versionCount > 1 ? "variants" : "variant");
                    }
                    bikeNames = bikeNames.Remove(bikeNames.Length - 5);
                    bikePrice = bikePrice.Remove(bikePrice.Length - 6);
                    variants = variants.Remove(variants.Length - 5);
                }
                compareBikeText = string.Format("BikeWale brings you comparison of {0}. The ex-showroom price of{1}.{2}. Apart from prices, you can also find comparison of these bikes based on displacement, mileage, performance, and many more parameters. Comparison between these bikes have been carried out to help users make correct buying decison between {0}.", bikeNames, bikePrice, variants);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.new.CreateCompareSummary()");
            }
        }


        /// <summary>
        /// Checks for redirection.
        /// If current url is not equal to the Canonical url then redirect to canoncial url
        /// </summary>
        /// <param name="canonicalUrl">The canonical URL.</param>
        private void CheckForRedirection(string canonicalUrl)
        {
            isPermRedirectionNeeded = baseUrl == canonicalUrl ? false : true;
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
                Int64 featuredBike = CompareBikes.GetFeaturedBike(versions);
                if (featuredBike > 0)
                {
                    featuredBikeId = featuredBike.ToString();
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
                    return string.Format("<a href='/used/{1}-{2}-bikes-in-india/' title='Used {5} bikes'>{0} Used {3}</a><p>Starting at Rs. {4} </p>",
                        bikeCount, makeMaskingName, modelMaskingName, string.Format("{0} {1}", make, model), minPrice, model);
                }
                else
                {
                    return string.Format("<a href='/used/{1}-{2}-bikes-in-{5}/' title='Used {6} bikes'>{0} Used {3}</a><p>Starting at Rs. {4} </p>",
                        bikeCount, makeMaskingName, modelMaskingName, string.Format("{0} {1}", make, model), minPrice, cityMasking, model);
                }
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
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
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
            ctrlSimilarBikes.TopCount = 8;
            ctrlSimilarBikes.versionsList = verList;

        }

        private string CreateCompareUrl(List<CompareMakeModelEntity> bikeList)
        {
            string url = string.Empty;
            try
            {
                var sorted = bikeList.Where(x => x.ModelId != 0).OrderBy(x => x.ModelId);
                foreach (var bike in sorted)
                {
                    url += string.Format("{0}-{1}-vs-", bike.MakeMaskingName, bike.ModelMaskingName);
                    hashModels += string.Format("{0},", bike.ModelId);
                    hashVersions += string.Format("{0},", bike.VersionId);
                }
                url = url.Remove(url.Length - 4, 4);
                hashModels = hashModels.Remove(hashModels.Length - 1, 1);
                hashVersions = hashVersions.Remove(hashVersions.Length - 1, 1);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "comparebikes.CreateCompareUrl()");
            }
            return url;
        }
    }
}