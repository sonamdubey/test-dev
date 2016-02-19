using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
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
    public class BindVideosLandingControl
    {
        public uint TotalRecords { get; set; }
        public int FetchedRecordsCount { get; set; }
        public EnumVideosCategory CategoryId { get; set; }
        public BikeVideoEntity FirstVideoRecord { get; set; }

        [System.ComponentModel.DefaultValue(0)]
        public int DoSkip { get; set; }

        public void BindVideos(Repeater rptr)
        {
            FetchedRecordsCount = 0;
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IVideosCacheRepository, VideosCacheRepository>()
                             .RegisterType<IVideos, Bikewale.BAL.Videos.Videos>()
                             .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IVideosCacheRepository>();
                    objVideosList = objCache.GetVideosByCategory(CategoryId, TotalRecords);

                    if (objVideosList != null && objVideosList.Count() > 0)
                    {
                        FetchedRecordsCount = objVideosList.Count();

                        if (FetchedRecordsCount > 0)
                        {
                            FirstVideoRecord = objVideosList.FirstOrDefault();
                            if(DoSkip == 0)
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

                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}