using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 4th March 2016
    /// </summary>
    public class VideoMakeModel : System.Web.UI.Page
    {
        protected Repeater rptVideos;
        protected int totalRecords = 0;
        protected bool isModel = false;
        protected string make = string.Empty, model = string.Empty, titleName = string.Empty, canonTitle = string.Empty, pageHeading = string.Empty, metaDescription = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty, canonicalUrl = string.Empty, metaKeywords = string.Empty;
        protected ushort makeId = 0;
        protected uint? modelId;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ParseQueryString();
            BindVideos();
            CreateTitleMeta();
        }

        /// <summary>
        /// Function to create Title, meta tags and description
        /// Written By : Sushil Kumar on 4th March 2016
        /// Modified By  : Sushil Kumar on 4th March 2016
        /// Description : Added titleName for pageTitle
        /// </summary>
        private void CreateTitleMeta()
        {
            if (isModel)
            {
                pageHeading = String.Format("{0} {1} Videos", make, model);
                titleName = String.Format("{0} {1} Videos - BikeWale", make, model);
                canonicalUrl = string.Format("http://www.bikewale.com/bike-videos/{0}-{1}/", makeMaskingName, modelMaskingName);
                metaDescription = string.Format("Check latest {0} {1} videos, watch BikeWale expert's take on {0} {1} - features, performance, price, fuel economy, handling and more.", make, model);
                metaKeywords = string.Format("{0},{1},{0} {1},{0} {1} Videos", make, model);
            }
            else
            {
                pageHeading = String.Format("{0} Bike Videos", make);
                titleName = String.Format("{0} Bike Videos - BikeWale", make);
                canonicalUrl = string.Format("http://www.bikewale.com/bike-videos/{0}/", makeMaskingName);
                metaDescription = string.Format("Check latest {0} bikes videos, watch BikeWale expert's take on {0} bikes - features, performance, price, fuel economy, handling and more.", make);
                metaKeywords = string.Format("{0},{0} Videos", make);
            }
        }   // page load

        /// <summary>
        /// Written By : Sushil Kumar on 4th March 2016
        /// Summary : function to read the query string values.
        /// Modified By  : Sushil Kumar on 4th March 2016
        /// Description : Added try catch for parseString function
        ///               Make check for null objects
        /// </summary>
        private void ParseQueryString()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    makeMaskingName = Request.QueryString["make"];
                    if (!String.IsNullOrEmpty(makeMaskingName))
                    {
                        string _makeId = MakeMapping.GetMakeId(makeMaskingName);
                        if (!string.IsNullOrEmpty(_makeId) && ushort.TryParse(_makeId, out makeId))
                        {
                            modelMaskingName = Request.QueryString["model"];
                            if (!string.IsNullOrEmpty(modelMaskingName))
                                isModel = true;

                            if (makeId > 0 && isModel)
                            {
                                container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                         .RegisterType<ICacheManager, MemcacheManager>()
                                         .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                                var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                                ModelMaskingResponse objResponse = null;
                                objResponse = objCache.GetModelMaskingResponse(modelMaskingName);
                                modelId = objResponse.ModelId;
                            }
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "m/pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "m/pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "ParseQueryString()");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Writtten By : Sushil Kumar on 4th March 2016
        /// Summary : Function to bind the videos to the videos repeater.
        ///           Initially 9 records are binded.
        /// Modified By : Sushil Kumar on 4th March 2016
        /// Description : Made check for makeId for unnecessary call  if query string is wrong or invalid
        /// </summary>
        private void BindVideos()
        {
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {
                if (makeId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IVideosCacheRepository, VideosCacheRepository>()
                                 .RegisterType<IVideos, Bikewale.BAL.Videos.Videos>()
                                 .RegisterType<ICacheManager, MemcacheManager>();

                        var objCache = container.Resolve<IVideosCacheRepository>();
                        if (modelId.HasValue)
                        {
                            objVideosList = objCache.GetVideosByMakeModel(1, 9, makeId, modelId);
                        }
                        else
                        {
                            objVideosList = objCache.GetVideosByMakeModel(1, 9, makeId);
                        }
                        if (objVideosList != null && objVideosList.Count() > 0)
                        {
                            rptVideos.DataSource = objVideosList;
                            rptVideos.DataBind();
                            // Set make and modelName
                            if (objVideosList.FirstOrDefault() != null)
                            {
                                make = objVideosList.FirstOrDefault().MakeName;
                                if (isModel)
                                    model = objVideosList.FirstOrDefault().ModelName;
                            }
                        }
                        else
                        {
                            // As no videos are found, please redirect to 404 error

                            Response.Redirect(CommonOpn.AppPath + "m/pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "BindVideos()");
                objErr.SendMail();
            }
        }
    }
}