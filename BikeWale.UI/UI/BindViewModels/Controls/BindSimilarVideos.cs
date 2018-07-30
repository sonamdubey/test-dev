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
    public class BindSimilarVideos
    {
        public ushort TotalRecords { get; set; }
        public ushort FetchedRecordsCount { get; set; }
        public EnumVideosCategory CategoryId { get; set; }
        public uint VideoBasicId { get; set; }

        /// <summary>
        /// Bind video result to Repeater
        /// </summary>
        /// <param name="rptSimilarVideos"></param>
        /// <param name="basicID"></param>
        public void BindVideos(Repeater rptSimilarVideos, uint basicID)
        {
            FetchedRecordsCount = 0;
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IVideosCacheRepository, VideosCacheRepository>()
                             .RegisterType<IVideos, Bikewale.BAL.Videos.Videos>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IVideoRepository, ModelVideoRepository>();

                    var objCache = container.Resolve<IVideosCacheRepository>();
                    objVideosList = objCache.GetSimilarVideos(basicID, TotalRecords);

                    if (objVideosList != null && objVideosList.Any())
                    {
                        FetchedRecordsCount = Convert.ToUInt16(objVideosList.Count());
                        if (FetchedRecordsCount > 0)
                        {
                            rptSimilarVideos.DataSource = objVideosList;
                            rptSimilarVideos.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }
    }
}