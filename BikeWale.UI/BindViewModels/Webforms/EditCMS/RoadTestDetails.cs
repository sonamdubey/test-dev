using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Webforms.EditCMS
{
    /// <summary>
    /// Created by : Aditi Srivastava on 30 Jan 2017
    /// Summary    : Common viewmodel for expert reviews/road test detail page
    /// </summary>
    public class RoadTestDetails
    {
        public uint BasicId;
        public bool IsContentFound { get; set; }
        public bool IsPageNotFound { get; set; }
        public bool IsPermanentRedirect { get; set; }
        public string baseUrl { get; set; }
        public string qsBasicId=string.Empty;
        public ArticlePageDetails objRoadtest;
        public IEnumerable<ModelImage> objImg;
        public BikeMakeEntityBase taggedMakeObj;
        public BikeModelEntityBase taggedModelObj;


        public RoadTestDetails(string url)
        {
            this.baseUrl = url;
            if (ProcessQueryString() && BasicId > 0)
            {
                GetRoadTestDetails();
            }
        }

        private bool ProcessQueryString()
        {
            bool isSuccess = true;

            var Request = HttpContext.Current.Request;
            qsBasicId = Request.QueryString["id"];
            if (!String.IsNullOrEmpty(qsBasicId) && CommonOpn.CheckId(qsBasicId))
            {
                string basicId = BasicIdMapping.GetCWBasicId(Request["id"]);

                if (!basicId.Equals(qsBasicId))
                {
                    string _newUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];

                    var _titleStartIndex = _newUrl.LastIndexOf('/') + 1;
                    var _titleEndIndex = _newUrl.LastIndexOf('-');
                    string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex + 1);
                    _newUrl = String.Format("{0}{1}{2}.html", baseUrl, _newUrlTitle, basicId);
                    CommonOpn.RedirectPermanent(_newUrl);
                }

                 BasicId = Convert.ToUInt32(qsBasicId);
                
            }
            else
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Get details of road test article
        /// </summary>
        private void GetRoadTestDetails()
        {
            try
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                       .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                       .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    objRoadtest = _cache.GetArticlesDetails(BasicId);
                    objImg = _cache.GetArticlePhotos(Convert.ToInt32(BasicId));


                    if (objRoadtest != null)
                    {
                        IsContentFound = true;
                        GetTaggedBikeListByMake();
                        GetTaggedBikeListByModel();
                    }
                    else
                    {
                        IsContentFound = false;
                    }

                }


            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.EditCMS.RoadTestDetails.GetRoadTestDetails");
            }
        }



        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Get tagged make in feature
        /// </summary>
        private void GetTaggedBikeListByMake()
        {
            try
            {
                if (objRoadtest != null && objRoadtest.VehiclTagsList.Count > 0)
                {

                    var _taggedMakeObj = objRoadtest.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                    if (_taggedMakeObj != null)
                    {
                        taggedMakeObj = _taggedMakeObj.MakeBase;
                    }
                    else
                    {
                        taggedMakeObj = objRoadtest.VehiclTagsList.FirstOrDefault().MakeBase;
                        FetchMakeDetails();
                    }
                    }
            }
            catch (Exception err)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, "Bikewale.BindViewModels.Webforms.EditCMS.FeaturesDetails.GetTaggedBikeListByMake");

            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Get details of tagged make if not available with the article
        /// </summary>
        private void FetchMakeDetails()
        {
            try
            {
                if (taggedMakeObj != null && taggedMakeObj.MakeId > 0)
                {

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                        var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                        taggedMakeObj = makesRepository.GetMakeDetails(taggedMakeObj.MakeId.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.EditCMS.FeaturesDetails.FetchMakeDetails");
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Get model details if model is tagged
        /// </summary>
        private void GetTaggedBikeListByModel()
        {
            try
            {
                if (objRoadtest != null && objRoadtest.VehiclTagsList.Count > 0)
                {

                    var _taggedModelObj = objRoadtest.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.ModelBase.MaskingName));
                    if (_taggedModelObj != null)
                    {
                        taggedModelObj = _taggedModelObj.ModelBase;
                    }
                    else
                    {
                        taggedModelObj = objRoadtest.VehiclTagsList.FirstOrDefault().ModelBase;
                        taggedModelObj = new Bikewale.Common.ModelHelper().GetModelDataById((uint)taggedModelObj.ModelId);

                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.EditCMS.GetTaggedBikeListByModel");
            }
        }
    }
}