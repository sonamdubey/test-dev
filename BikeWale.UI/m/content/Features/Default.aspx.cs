using Bikewale.BAL.EditCMS;
using Bikewale.BAL.Pager;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 20 May 2014
    /// Modified By : Ashwini Todkar on 30 Sept 2014
    /// </summary>
    public class Features : System.Web.UI.Page
    {
        protected Repeater rptFeatures;
        protected ListPagerControl listPager;
        protected int curPageNo = 1;
        protected string prevPageUrl = String.Empty, nextPageUrl = String.Empty;
        private const int _pageSize = 10;
        private bool _isContentFound = true;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["pn"]))
                    Int32.TryParse(Request.QueryString["pn"], out curPageNo);

                GetFeaturesList();
            }
        }
        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// Summary    : PopulateWhere to get features list from web api asynchronously
        /// </summary>
        private void GetFeaturesList()
        {
            try
            {
                // get pager instance
                IPager objPager = GetPager();

                int _startIndex = 0, _endIndex = 0;// _featuresCategoryId = (int)EnumCMSContentType.Features;

                objPager.GetStartEndIndex(_pageSize, curPageNo, out _startIndex, out _endIndex);

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.Features);
                categorList.Add(EnumCMSContentType.SpecialFeature);
                string _featuresCategoryId = CommonApiOpn.GetContentTypesString(categorList);

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                            .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    var _objFeaturesList = _cache.GetArticlesByCategoryList(_featuresCategoryId, _startIndex, _endIndex, 0, 0);

                    if (_objFeaturesList != null && _objFeaturesList.Articles.Count > 0)
                    {

                        int _totalPages = objPager.GetTotalPages(Convert.ToInt32(_objFeaturesList.RecordCount), _pageSize);
                        BindFeatures(_objFeaturesList);
                        BindLinkPager(objPager, Convert.ToInt32(_objFeaturesList.RecordCount), _totalPages);

                    }
                    else
                    {
                        _isContentFound = false;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                {
                    Response.Redirect("/m/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private void BindLinkPager(IPager objPager, int recordCount, int totalPages)
        {
            PagerEntity pagerEntity = new PagerEntity();

            pagerEntity.BaseUrl = "/m/features/";
            pagerEntity.PageNo = curPageNo;
            pagerEntity.PagerSlotSize = totalPages;
            pagerEntity.PageUrlType = "page/";
            pagerEntity.TotalResults = recordCount;
            pagerEntity.PageSize = _pageSize;

            PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);

            listPager.PagerOutput = pagerOutput;
            listPager.TotalPages = totalPages;
            listPager.CurrentPageNo = curPageNo;
            listPager.BindPageNumbers();

            //get next and prev page links for SEO
            prevPageUrl = pagerOutput.PreviousPageUrl;
            nextPageUrl = pagerOutput.NextPageUrl;
        }

        private void BindFeatures(CMSContent _objFeaturesList)
        {
            rptFeatures.DataSource = _objFeaturesList.Articles;
            rptFeatures.DataBind();
        }

        private IPager GetPager()
        {
            IPager _objPager = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPager, Pager>();
                _objPager = container.Resolve<IPager>();
            }
            return _objPager;
        }
    }
}