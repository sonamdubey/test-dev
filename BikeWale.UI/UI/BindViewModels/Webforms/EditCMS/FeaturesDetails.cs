using Bikewale.BAL.BikeData;
using Bikewale.BAL.EditCMS;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Notifications;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
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
    /// Summary    : Common viewmodel for features detail page
    /// </summary>
    public class FeaturesDetails
    {
        public uint BasicId;
        protected int pageId = 1;
        public bool IsContentFound, IsPageNotFound, IsPermanentRedirect;
        public IEnumerable<ModelImage> objImg;
        public ArticlePageDetails objFeature { get; set; }
        public BikeMakeEntityBase taggedMakeObj;
        public BikeModelEntityBase taggedModelObj;
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public string MappedCWId { get; set; }
        private readonly ICMSCacheContent _cache = null;

        public FeaturesDetails()
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

        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Process query string
        /// </summary>
        /// <returns></returns>
        private bool ProcessQueryString()
        {

            var request = HttpContext.Current.Request;
            string qsBasicId = request.QueryString["id"], pn = request.QueryString["pn"];
            try
            {
                if (uint.TryParse(qsBasicId, out BasicId) && BasicId > 0)
                {
                    string _basicId = BasicIdMapping.GetCWBasicId(qsBasicId);

                    if (!_basicId.Equals(qsBasicId))
                    {
                        IsPermanentRedirect = (uint.TryParse(_basicId, out BasicId) && BasicId > 0);
                        MappedCWId = _basicId;
                        return false;
                    }
                    if (!String.IsNullOrEmpty(pn))
                    {
                        IsPageNotFound = !(Int32.TryParse(pn, out pageId) && pageId > 0);
                        return false;
                    }
                }
                else
                {
                    IsPageNotFound = true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.EditCMS.FeaturesDetails.ProcessQueryString");
            }

            return true;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Get all feature details
        /// </summary>
        public void GetFeatureDetails()
        {
            try
            {
                objFeature = _cache.GetArticlesDetails(BasicId);
                if (objFeature != null)
                {
                    IsContentFound = true;
                    objImg = _cache.GetArticlePhotos((int)BasicId);
                    GetTaggedBikeListByMake();
                    GetTaggedBikeListByModel();
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.BindViewModels.Webforms.EditCMS.FeaturesDetails.GetFeatureDetails");
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
                if (objFeature != null && objFeature.VehiclTagsList != null && objFeature.VehiclTagsList.Count > 0)
                {

                    var _taggedMakeObj = objFeature.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                    if (_taggedMakeObj != null)
                    {
                        taggedMakeObj = _taggedMakeObj.MakeBase;
                    }
                    else
                    {
                        taggedMakeObj = objFeature.VehiclTagsList.FirstOrDefault().MakeBase;
                        if (taggedMakeObj != null && taggedMakeObj.MakeId > 0)
                        {
                            taggedMakeObj = new Bikewale.Common.MakeHelper().GetMakeNameByMakeId((uint)taggedMakeObj.MakeId);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.BindViewModels.Webforms.EditCMS.FeaturesDetails.GetTaggedBikeListByMake");

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
                if (objFeature != null && objFeature.VehiclTagsList != null && objFeature.VehiclTagsList.Count > 0)
                {

                    var _taggedModelObj = objFeature.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.ModelBase.MaskingName));
                    if (_taggedModelObj != null)
                    {
                        taggedModelObj = _taggedModelObj.ModelBase;
                    }
                    else
                    {
                        taggedModelObj = objFeature.VehiclTagsList.FirstOrDefault().ModelBase;
                        if (taggedModelObj != null)
                        {
                            taggedModelObj = new Bikewale.Common.ModelHelper().GetModelDataById((uint)taggedModelObj.ModelId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.EditCMS.FeaturesDetails.GetTaggedBikeListByModel");
            }
        }


    }
}