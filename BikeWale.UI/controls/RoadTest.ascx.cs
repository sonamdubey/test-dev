using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
                IEnumerable<ArticleSummary> _objRoadtestList = null;

                if (_topRecords == "2")
                {
                    _topRecords = "4";

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IArticles, Articles>()
                               .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                               .RegisterType<ICacheManager, MemcacheManager>();
                        ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                        _objRoadtestList = _cache.GetMostRecentArticlesById(EnumCMSContentType.News, Convert.ToUInt32(_topRecords), Convert.ToUInt32(MakeId), Convert.ToUInt32(ModelId));

                        if (_objRoadtestList != null && _objRoadtestList.Count() > 0)
                        {

                            divControl.Attributes.Remove("class");
                            rptRoadTest.DataSource = _objRoadtestList.Take(2);
                            rptRoadTest.DataBind();

                            recordCount = 2;
                        }
                        else
                            divControl.Attributes.Add("class", "hide");

                    }

                }
                else
                {

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IArticles, Articles>()
                               .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                               .RegisterType<ICacheManager, MemcacheManager>();
                        ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                        _objRoadtestList = _cache.GetMostRecentArticlesById(EnumCMSContentType.News, Convert.ToUInt32(_topRecords), Convert.ToUInt32(MakeId), Convert.ToUInt32(ModelId));

                        if (_objRoadtestList != null && _objRoadtestList.Count() > 0)
                        {
                            divControl.Attributes.Remove("class");
                            rptRoadTest.DataSource = _objRoadtestList;
                            rptRoadTest.DataBind();

                            recordCount = _objRoadtestList.Count();
                        }
                        else
                            divControl.Attributes.Add("class", "hide");



                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("road test bikes FetchRoadTest Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // end of FetchUpcomingBikes method


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
        protected string GetLink(string BasicId, string Url)
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