using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Bikewale.Entities.CMS.Articles;
using System.Configuration;
using Bikewale.Entities.CMS;
using System.Collections.Generic;

namespace Bikewale.Controls
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 1/8/2012
    ///     Class will show the upcoming bikes list
    ///     Modified By : Ashwini Todkar on 6 Oct 2014
    /// </summary>
    public partial class RoadTest : System.Web.UI.UserControl
    {
        protected Repeater rptRoadTest;
        protected HtmlGenericControl divControl;
        private int recordCount = 0;
        private string _topRecords = "4";

        public string TopRecords
        {
            get { return _topRecords; }
            set { _topRecords = value; }
        }

        private string _width = "grid_2";

        public string ControlWidth
        {
            get { return _width; }
            set { _width = value; }
        }
        
        private string _imageWidth = "136px;";

        public string ImageWidth
        {
            get { return _imageWidth; }
            set { _imageWidth = value; }
        }

        public int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }

        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public bool Corousal { get; set; }
        public string WhereClause { get; set; }
        public string HeaderText { get; set; }
        public string SeriesId { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //if(!String.IsNullOrEmpty(SeriesId) || !String.IsNullOrEmpty(ModelId) || !String.IsNullOrEmpty(MakeId))
                    FetchRoadTest();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 6 Oct 2014
        /// Summary    : PopulateWhere to fetch roadtests from api asynchronously
        /// </summary>
        protected async void FetchRoadTest()
        {
            try
            {
                List<ArticleSummary> _objRoadtestList = null;

                if (_topRecords == "2")
                {
                    _topRecords = "4";
                    
                    int _contentType = (int)EnumCMSContentType.RoadTest;
                    string _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + _topRecords;


                    if (!String.IsNullOrEmpty(MakeId) || !String.IsNullOrEmpty(ModelId))
                    {
                        if (!String.IsNullOrEmpty(ModelId))
                            _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + _topRecords + "&makeid=" + MakeId + "&modelid=" + ModelId;
                        else
                            _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + _topRecords + "&makeid=" + MakeId;
                    }

                    using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                    {
                        _objRoadtestList = await objClient.GetApiResponse<List<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objRoadtestList);
                    }
                    
                    if (_objRoadtestList != null && _objRoadtestList.Count > 0)
                    {
                        List<ArticleSummary> objRoadTests = new List<ArticleSummary>();

                        for (int i = 0; i < 2; i++)
                            objRoadTests.Add(_objRoadtestList[i]);

                        divControl.Attributes.Remove("class");
                        rptRoadTest.DataSource = objRoadTests;
                        rptRoadTest.DataBind();

                        recordCount = objRoadTests.Count;
                    }
                    else
                        divControl.Attributes.Add("class", "hide");
                }
                else
                {                    
                    int _contentType = (int)EnumCMSContentType.RoadTest;
                    string _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + _topRecords;


                    if (!String.IsNullOrEmpty(MakeId) || !String.IsNullOrEmpty(ModelId))
                    {
                        if (!String.IsNullOrEmpty(ModelId))
                            _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + _topRecords + "&makeid=" + MakeId + "&modelid=" + ModelId;
                        else
                            _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + _topRecords + "&makeid=" + MakeId;
                    }

                    using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                    {
                        _objRoadtestList = await objClient.GetApiResponse<List<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objRoadtestList);
                    }                    

                    if (_objRoadtestList != null && _objRoadtestList.Count > 0)
                    {
                        divControl.Attributes.Remove("class");
                        rptRoadTest.DataSource = _objRoadtestList;
                        rptRoadTest.DataBind();

                        recordCount = _objRoadtestList.Count;
                    }
                    else
                        divControl.Attributes.Add("class", "hide");
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("road test bikes FetchRoadTest Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // end of FetchUpcomingBikes method

        //protected void FetchRoadTest()
        //{
        //    Database db = null;
        //    DataSet ds = null;

        //    try 
        //    {
        //        db = new Database();

        //        using(SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "GetRoadTestMin";

        //            Trace.Warn("MakeId : ", MakeId);
        //            Trace.Warn("ModelId : ", ModelId);

        //            cmd.Parameters.Add("@TopCount", SqlDbType.SmallInt).Value = TopRecords;
        //            cmd.Parameters.Add("@ControlWidth", SqlDbType.VarChar, 10).Value = ControlWidth;
        //            cmd.Parameters.Add("@FetchAllRecords", SqlDbType.Bit).Value = Corousal;
        //            if (!String.IsNullOrEmpty(ModelId)) { cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = ModelId; }
        //            if (!String.IsNullOrEmpty(MakeId)) { cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = MakeId; }
        //            if (!String.IsNullOrEmpty(SeriesId)) { cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = SeriesId; }
                   
        //            ds = db.SelectAdaptQry(cmd);
                  
        //            if (ds != null && ds.Tables[0].Rows.Count > 0)
        //            {
        //                divControl.Attributes.Remove("class");
        //                DataTable dt = ds.Tables[0];

        //                rptRoadTest.DataSource = dt;
        //                rptRoadTest.DataBind();
        //            }
        //            else
        //                divControl.Attributes.Add("class", "hide");

        //            recordCount = rptRoadTest.Items.Count;
        //            //Trace.Warn("++++record count ", recordCount.ToString());
        //        }
        //    }
        //    catch (SqlException exSql)
        //    {
        //        Trace.Warn("road test bikes FetchRoadTest sqlex: ", exSql.Message);
        //        ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.Warn("road test bikes FetchRoadTest Ex: ", ex.Message);
        //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }            
        //}   // end of FetchUpcomingBikes method

        /// <summary>
        /// Retrun Topic name if the topic name lenght is greater than 30 then it should be substring and showing small string for that
        /// </summary>
        /// <param name="Topic"></param>
        /// <returns></returns>
        protected string FormatedTopic(string Topic)
        {
            string result = Topic.Length > 125 ? Topic.Substring(0, Topic.IndexOf(" ", 110)) + "..." : Topic;
            return result;
        }

        /// <summary>
        ///     Fuction will for the redirect url
        /// </summary>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected string GetLink(string BasicId,string Url)
        {
            //return "/road-tests/" + UrlRewrite.FormatSpecial(make) + "-bikes/" + UrlRewrite.FormatSpecial(model) + "/road-tests/";
            return "/road-tests/" + Url + "-" + BasicId + ".html";
        }

        protected string TruncateDesc(string _desc)
        {
            _desc = Regex.Replace(_desc, @"<[^>]+>", string.Empty);

            if (_desc.Length < 120)
                return _desc;
            else
            {
                _desc = _desc.Substring(0, 120);
                _desc = _desc.Substring(0, _desc.LastIndexOf(" "));
                return _desc + " ...";
            }
        }

    }   // End of class
}   // End of namespace