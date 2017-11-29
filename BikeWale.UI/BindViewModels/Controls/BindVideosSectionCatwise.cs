using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.DAL.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 19th February 2016
    /// Description : Bind Repeater with category wise by using old videos api 
    /// </summary>
    public class BindVideosSectionCatwise
    {
        public ushort TotalRecords { get; set; }
        public ushort FetchedRecordsCount { get; set; }
        public EnumVideosCategory CategoryId { get; set; }
        public BikeVideoEntity FirstVideoRecord { get; set; }
        private IEnumerable<BikeVideoEntity> objVideosList { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public int DoSkip { get; set; }

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
                    objVideosList = objCache.GetVideosByCategory(CategoryId, TotalRecords);

                    if (objVideosList != null && objVideosList.Any())
                    {
                        FetchedRecordsCount = Convert.ToUInt16(objVideosList.Count());

                        if (FetchedRecordsCount > 0)
                        {
                            FirstVideoRecord = objVideosList.FirstOrDefault();
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
                if (objVideosList != null && objVideosList.Any())
                {
                    if (DoSkip == 0)
                    {
                        rptr.DataSource = objVideosList;
                    }
                    else
                    {
                        rptr.DataSource = objVideosList.Skip(DoSkip);
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