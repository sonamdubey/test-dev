using Carwale.BL.Videos;
using Carwale.Cache.CMS;
using AEPLCore.Cache;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Interfaces;
using Carwale.Interfaces.CMS;
using Carwale.Notifications;
using Carwale.Service;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carwale.UI.Controls
{
    public class PopularVideoWidget : UserControl
    {
        protected Repeater rptVideoData, rptVideoNewsList;
        protected int videoFlag;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string _Tags;
        public String Tag
        {
            get { return _Tags; }
            set { _Tags = value; }
        }

        private int _videoFlag;
        public int VideoFlag
        {
            get { return _videoFlag; }
            set { _videoFlag = value; }
        }

        private int _topCount = 0;
        public int TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        public void populateVideoWidget()
        {
            videoFlag = VideoFlag;

            if (VideoFlag == 1)
            {
                ShowVideosOnNewsList();
            }
            else
            {
                ShowPopularVideos();
            }
        }
        private void ShowPopularVideos()
        {
            try
            {                
                var newsRepositoryContainer = UnityBootstrapper.Resolve<IVideosBL>();

                ContentFilters year = new ContentFilters();

                IList<VideosEntity> popularVideoList = (IList<VideosEntity>)newsRepositoryContainer.GetVideoList(1, (uint)CMSAppId.Carwale);
                PopulateVideoWithTag(popularVideoList);
                //SettingProperty(popularVideoList);

            }
            catch (Exception err)
            {                
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void ShowVideosOnNewsList()
        {
            try
            {
                IVideosBL blObj = UnityBootstrapper.Resolve<IVideosBL>();

                rptVideoNewsList.DataSource = blObj.GetNewModelsVideosBySubCategory(EnumVideoCategory.JustLatest, CMSAppId.Carwale, 1, TopCount);
                rptVideoNewsList.DataBind();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void PopulateVideoWithTag(IList<VideosEntity> popularVideoList)
        {
            try
            {
                if (rptVideoData != null)
                {
                    IEnumerable<VideosEntity> sortedEnum = popularVideoList.OrderBy(x => x.Popularity);
                    IList<VideosEntity> sortedList = sortedEnum.ToList();
                    HttpContext.Current.Trace.Warn("in video widget: " + sortedList.Count);
                    rptVideoData.DataSource = sortedList;
                    rptVideoData.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

    }
}