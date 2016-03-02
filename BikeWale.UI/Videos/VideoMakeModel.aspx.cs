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
using Bikewale.Utility.StringExtention;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Videos
{
    public class VideoMakeModel : System.Web.UI.Page
    {
        protected Repeater rptVideos;
        protected int totalRecords = 0;
        protected bool isModel=false;
        protected string make = string.Empty, model = string.Empty, titleName = string.Empty, canonTitle = string.Empty, pageHeading = string.Empty, descName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty, canonicalUrl = string.Empty;
        protected uint makeId = 6;
        protected uint? modelId;


        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DeviceDetection dd = new DeviceDetection();
            dd.DetectDevice();
            //GetMakeModelDetails();
            ParseQueryString();
            BindVideos();
            CreateTitleMeta();
        }

        /// <summary>
        /// Function to create Title, meta tags and description
        /// </summary>
        private void CreateTitleMeta()
        {
            if(isModel)
            {
                pageHeading = String.Format("{0} {1} videos", make, model);
                canonicalUrl = string.Format("http://www.bikewale.com/bike-videos/{0}-{1}/", makeMaskingName, modelMaskingName);
            }
            else
            {
                pageHeading = String.Format("{0} bike videos", make);
                canonicalUrl = string.Format("http://www.bikewale.com/bike-videos/{0}/", makeMaskingName);
            }
        }   // page load

        /// <summary>
        /// Written By : Sangram Nandkhile on 01 Mar 2016
        /// Summary : function to read the query string values.
        /// </summary>
        private void ParseQueryString()
        {
            modelMaskingName = Request.QueryString["model"];
            if (!string.IsNullOrEmpty(modelMaskingName))
                isModel = true;

            using (IUnityContainer container = new UnityContainer())
            {
                if (!String.IsNullOrEmpty(Request.QueryString["make"]))
                {
                    makeMaskingName = Request.QueryString["make"];
                    makeId = Convert.ToUInt16(MakeMapping.GetMakeId(makeMaskingName));
                }
                if (isModel)
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                    ModelMaskingResponse objResponse = null;
                    objResponse = objCache.GetModelMaskingResponse(modelMaskingName);
                    modelId = objResponse.ModelId;
                    // get model and make name
                    
                }
                else
                {
                    // Make Videos
                }
            }

            //if (!String.IsNullOrEmpty(Request.QueryString.Get("id")))
            //{
            //    if(isModel)
            //        modelId = Convert.ToUInt16(Request.QueryString.Get("id"));
            //    else
            //        makeId = Convert.ToUInt16(Request.QueryString.Get("id"));
            //    makeId = 6;
            //}
            //pageHeading = string.Format("{0}{1} Videos", make, model!=string.Empty? "" :" " + model);
            //canonTitle = titleName.ToLower();
            //if (!string.IsNullOrEmpty(titleName))
            //{
            //    //capitalize title
            //    titleName = StringHelper.Capitlization(titleName);
            //    titleName = titleName.Replace('-', ' ');
            //    pageHeading = string.Format("{0} Video", titleName);
            //    titleName = string.Format("{0} Video Review - BikeWale", titleName);
            //}
            //descName = string.Format("{0} - Watch BikeWale's Expert's Take on New Bike and Scooter Launches - Features, performance, price, fuel economy, handling and more",
            //titleName);
            //title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison -   BikeWale";
            //desc = "Check latest bike and scooter videos, " + descText; 
        }

        /// <summary>
        /// Writtten By : Lucky Rathore
        /// Summary : Function to bind the videos to the videos repeater.
        ///           Initially 9 records are binded.
        /// </summary>
        private void BindVideos()
        {
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IVideosCacheRepository, VideosCacheRepository>()
                             .RegisterType<IVideos, Bikewale.BAL.Videos.Videos>()
                             .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IVideosCacheRepository>();
                    objVideosList = objCache.GetVideosByMake(makeId, 1, 9);
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
                        if (objVideosList.FirstOrDefault()!= null)
                        {
                            make = objVideosList.FirstOrDefault().MakeName;
                            if (isModel)
                                model = objVideosList.FirstOrDefault().ModelName;
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
        /// <summary>
        /// Call api and set make model details
        /// </summary>
        private void GetMakeModelDetails()
        {
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString.Get("model")))
                    isModel = true;
                using (IUnityContainer container = new UnityContainer())
                {
                    if (isModel)
                    {
                        container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                        IBikeModelsRepository<BikeModelEntity, int> _bikeModel = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();
                        BikeModelEntity objModel = _bikeModel.GetById(99);
                        make = objModel.MakeBase.MakeName;
                        model = objModel.ModelName;
                    }
                    else
                    {
                        container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                        IBikeMakes<BikeMakeEntity, int> _bikeMake = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                        Bikewale.Entities.BikeData.BikeMakeEntityBase objMake = _bikeMake.GetMakeDetails("6");
                        make = objMake.MakeName;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : GetMakeModelDetails");
                objErr.SendMail();
            }
        }
    }
}