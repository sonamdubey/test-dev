using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Notifications;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
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
        public bool IsContentFound, IsPageNotFound, IsPermanentRedirect;
        public ArticlePageDetails objRoadtest;
        public IEnumerable<ModelImage> objImg;
        public BikeMakeEntityBase taggedMakeObj;
        public BikeModelEntityBase taggedModelObj;
        public string MappedCWId { get; set; }
        private readonly ICMSCacheContent _cache = null;

        public RoadTestDetails()
        {
            if (ProcessQueryString())
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                       .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                       .RegisterType<ICacheManager, MemcacheManager>();
                    _cache = container.Resolve<ICMSCacheContent>();

                }
            }
        }

        private bool ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            string qsBasicId = request.QueryString["id"];
            try
            {
                qsBasicId = BasicIdMapping.GetCWBasicId(qsBasicId);
                if (!qsBasicId.Equals(request.QueryString["id"]))
                {
                    IsPermanentRedirect = true;
                    MappedCWId = qsBasicId;
                    return false;
                }
                IsPageNotFound = !(uint.TryParse(qsBasicId, out BasicId) && BasicId > 0);
                return !IsPageNotFound;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.EditCMS.RoadTestDetails.ProcessQueryString");
            }
            return true;
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Get details of road test article
        /// </summary>
        public void GetRoadTestDetails()
        {
            try
            {
                objRoadtest = _cache.GetArticlesDetails(BasicId);
                if (objRoadtest != null)
                {
                    IsContentFound = true;
                    objImg = _cache.GetArticlePhotos(Convert.ToInt32(BasicId));
                    GetTaggedBikeListByMake();
                    GetTaggedBikeListByModel();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.EditCMS.RoadTestDetails.GetRoadTestDetails");
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Get tagged make in expert reviews
        /// </summary>
        private void GetTaggedBikeListByMake()
        {
            try
            {
                if (objRoadtest != null && objRoadtest.VehiclTagsList!=null && objRoadtest.VehiclTagsList.Count > 0)
                {

                    var _taggedMakeObj = objRoadtest.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                    if (_taggedMakeObj != null)
                    {
                        taggedMakeObj = _taggedMakeObj.MakeBase;
                    }
                    else
                    {
                        taggedMakeObj = objRoadtest.VehiclTagsList.FirstOrDefault().MakeBase;
                        if (taggedMakeObj != null && taggedMakeObj.MakeId > 0)
                        {
                            taggedMakeObj = new Bikewale.Common.MakeHelper().GetMakeNameByMakeId((uint)taggedMakeObj.MakeId);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.BindViewModels.Webforms.EditCMS.RoadTestDetails.GetTaggedBikeListByMake");
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
                if (objRoadtest != null && objRoadtest.VehiclTagsList != null && objRoadtest.VehiclTagsList.Count > 0)
                {

                    var _taggedModelObj = objRoadtest.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.ModelBase.MaskingName));
                    if (_taggedModelObj != null)
                    {
                        taggedModelObj = _taggedModelObj.ModelBase;
                    }
                    else
                    {
                        taggedModelObj = objRoadtest.VehiclTagsList.FirstOrDefault().ModelBase;
                        if (taggedModelObj != null)
                        {
                            taggedModelObj = new Bikewale.Common.ModelHelper().GetModelDataById((uint)taggedModelObj.ModelId);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.EditCMS.RoadTestDetails.GetTaggedBikeListByModel");
            }
        }
    }
}