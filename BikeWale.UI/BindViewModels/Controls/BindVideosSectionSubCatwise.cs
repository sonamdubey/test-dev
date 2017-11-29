using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.DAL.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{

    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 19th February 2016
    /// Description : Bind Repeater with sub category wise by using new videos api for single/multiple categories at once
    /// </summary>
    public class BindVideosSectionSubCatwise
    {
        public ushort TotalRecords { get; set; }
        public ushort FetchedRecordsCount { get; set; }
        public string CategoryIdList { get; set; }

        public BikeVideoEntity FirstVideoRecord { get; set; }
        private BikeVideosListEntity objVideosList { get; set; }

        private int _doSkip = 0;
        public int DoSkip { get { return _doSkip; } set { _doSkip = value; } }

        private ushort _pageNo = 1;
        public ushort PageNo { get { return _pageNo; } set { _pageNo = value; } }

        private VideosSortOrder _sortOrder = VideosSortOrder.JustLatest;
        public VideosSortOrder sortOrder { get { return _sortOrder; } set { _sortOrder = value; } }




        public void FetchVideos()
        {
            FetchedRecordsCount = 0;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IVideosCacheRepository, VideosCacheRepository>()
                             .RegisterType<IVideos, Bikewale.BAL.Videos.Videos>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IVideoRepository, ModelVideoRepository>();

                    var objCache = container.Resolve<IVideosCacheRepository>();
                    objVideosList = objCache.GetVideosBySubCategory(CategoryIdList, PageNo, TotalRecords, sortOrder);

                    if (objVideosList != null && objVideosList.TotalRecords > 0 && objVideosList.Videos != null)
                    {

                        FetchedRecordsCount = Convert.ToUInt16(objVideosList.Videos.Count());

                        if (FetchedRecordsCount > 0)
                        {
                            FirstVideoRecord = objVideosList.Videos.FirstOrDefault();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }

        public void BindVideos(Repeater rptr)
        {
            try
            {
                if (objVideosList != null && objVideosList.TotalRecords > 0 && objVideosList.Videos != null)
                {
                    if (DoSkip == 0)
                    {
                        rptr.DataSource = objVideosList.Videos;
                    }
                    else
                    {
                        rptr.DataSource = objVideosList.Videos.Skip(DoSkip);
                    }
                    rptr.DataBind();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }
    }
}