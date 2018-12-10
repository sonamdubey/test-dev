using AutoMapper;
using Carwale.DTOs.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Notifications;
using Carwale.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carwale.UI.Controls
{
    public class NewsRightWidget : UserControl
    {
        protected Repeater rptPopularNews, rptRecentNews, rptListPagePoularNews;
        protected int categoryId, basicId;
        private int _numberOfRecords;
        protected ICMSContent cmsContentCacheRepo = UnityBootstrapper.Resolve<ICMSContent>();
        public int NumberofRecords
        {
            get { return _numberOfRecords; }
            set { _numberOfRecords = value; }
        }
        public int RowCount { get; set; }

        private int _basicId;
        public int BasicId
        {
            get { return _basicId; }
            set { _basicId = value; }
        }

        private int _categoryId;
        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        private void Page_Load(object sender, EventArgs e)
        {

        }

        public void PopulateNewsWidget()
        {
            GetWidgetData();
        }


        private void GetWidgetData()
        {
            try
            {
                categoryId = CategoryId;
                basicId = BasicId;


                var queryStringNews = new ArticleRecentURI();
                queryStringNews.ApplicationId = Convert.ToUInt16(Carwale.Entity.CMS.CMSAppId.Carwale);
                queryStringNews.ContentTypes = CategoryId.ToString();
                queryStringNews.TotalRecords = Convert.ToUInt16(NumberofRecords);


                var querystringPopular = new ArticleByCatURI();
                querystringPopular.ApplicationId = Convert.ToUInt16(Carwale.Entity.CMS.CMSAppId.Carwale);
                querystringPopular.CategoryIdList = "1";
                querystringPopular.StartIndex = 1;
                querystringPopular.EndIndex = Convert.ToUInt32(ConfigurationManager.AppSettings["PopularNewsList"].ToString());

                if (BasicId != 0)
                {
                    var objRecentNews = GetRecentNewsAsync(queryStringNews);
                    List<ArticleSummary> newsRightWidgetList = objRecentNews;
                    PopulateRecentNews(newsRightWidgetList, BasicId);
                }

                var objPopularNews = GetPopularNewsAsync(querystringPopular);

                IList<ArticleSummaryDTOV3> newsPopularList = objPopularNews.Articles;

                PopulatePopularNews(newsPopularList);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.UI.Controls.NewsRightWidget.NewsRightWidget");
                objErr.LogException();
            }
        }

        protected List<ArticleSummary> GetRecentNewsAsync(ArticleRecentURI queryStringNews)
        {
            int appId = Convert.ToInt32(Carwale.Entity.CMS.CMSAppId.Carwale);
            List<ArticleSummary> objTask = null;
            try
            {                
                objTask = Mapper.Map<List<Carwale.Entity.CMS.Articles.ArticleSummary>, List<Carwale.DTOs.CMS.Articles.ArticleSummary>>(cmsContentCacheRepo.GetMostRecentArticles(queryStringNews));
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                err.SendMail();
            }

            return objTask;
        }

        protected Carwale.DTOs.CMS.Articles.CMSContentDTOV2 GetPopularNewsAsync(ArticleByCatURI querystringPopular)
        {
            CMSContentDTOV2 dto = null;
            try
            {                
                dto = Mapper.Map<Carwale.Entity.CMS.Articles.CMSContent, Carwale.DTOs.CMS.Articles.CMSContentDTOV2>(cmsContentCacheRepo.GetContentListByCategory(querystringPopular));
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                err.SendMail();
            }

            return dto;
        }

        private void PopulateRecentNews(List<ArticleSummary> newsList, int BasicId)
        {
            try
            {
                for (int i = 0; i < newsList.Count; i++)
                {
                    if (newsList[i].BasicId == Convert.ToUInt32(BasicId))
                    {
                        newsList.RemoveAt(i);
                    }
                }
                if (newsList.Count == 6)
                {
                    newsList.RemoveAt(5);
                }

                if (BasicId != 0)
                {
                    if (rptRecentNews != null)
                    {
                        rptRecentNews.DataSource = newsList;
                        rptRecentNews.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.UI.Controls.NewsRightWidget.PopulateRecentNews");
                objErr.LogException();
            }            
        }

        private void PopulatePopularNews(IList<ArticleSummaryDTOV3> newsList)
        {
            try
            {
                IList<ArticleSummaryDTOV3> filteredList = newsList.OrderByDescending(x => Convert.ToInt32(x.Views)).Take(NumberofRecords).ToList();

                for (int i = 0; i < filteredList.Count; i++)
                {
                    if (filteredList[i].BasicId == Convert.ToUInt32(BasicId))
                    {
                        filteredList.RemoveAt(i);
                    }
                }
                if (filteredList.Count == 6)
                {
                    filteredList.RemoveAt(5);
                }
                if (BasicId != 0)
                {
                    if (rptPopularNews != null)
                    {
                        rptPopularNews.DataSource = filteredList;
                        rptPopularNews.DataBind();
                    }
                }
                else
                {
                    rptListPagePoularNews.DataSource = filteredList;
                    rptListPagePoularNews.DataBind();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.UI.Controls.NewsRightWidget.PopulatePopularNews");
                objErr.LogException();
            }
        }

    }
}