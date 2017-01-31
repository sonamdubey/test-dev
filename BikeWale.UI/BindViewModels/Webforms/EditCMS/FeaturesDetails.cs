using Bikewale.BAL.BikeData;
using Bikewale.BAL.EditCMS;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
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
    [System.Runtime.InteropServices.GuidAttribute("0144D8A4-43A8-4F62-8718-EC64CCE02660")]
    public class FeaturesDetails
    {
        public uint BasicId;
        public string qsBasicId = string.Empty;
        protected int basicId = 0, pageId = 1;
        public bool IsContentFound { get; set; }
        public bool IsPageNotFound { get; set; }
        public bool IsPermanentRedirect { get; set; }
        public IEnumerable<ModelImage> objImg;
        public ArticlePageDetails objFeature { get; set; }
        public BikeMakeEntityBase taggedMakeObj;
        public BikeModelEntityBase taggedModelObj;
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public FeaturesDetails()
        {
            if (ProcessQueryString() && BasicId>0)
            {
                GetFeatureDetails();
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Process query string
        /// </summary>
        /// <returns></returns>
        private bool ProcessQueryString()
        {
            bool isSucess = true;
            var Request = HttpContext.Current.Request;
            qsBasicId = Request.QueryString["id"];
            if (qsBasicId!= null && !String.IsNullOrEmpty(qsBasicId) && CommonOpn.CheckId(qsBasicId))
            {
                string _basicId = BasicIdMapping.GetCWBasicId(qsBasicId);

                if (!_basicId.Equals(qsBasicId))
                {
                    string _newUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                    var _titleStartIndex = _newUrl.IndexOf('/');
                    var _titleEndIndex = _newUrl.LastIndexOf('-');
                    string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex + 1);
                    _newUrl = _newUrlTitle + _basicId + "/";
                    CommonOpn.RedirectPermanent(_newUrl);
                    IsPermanentRedirect = true;

                }
                uint.TryParse(_basicId, out BasicId);

                if (Request.QueryString["pn"] != null && !String.IsNullOrEmpty(Request.QueryString["pn"]) && CommonOpn.CheckId(Request.QueryString["pn"]))
                {
                    if (!Int32.TryParse(Request.QueryString["pn"], out pageId))
                    {
                        isSucess = false;
                        IsPageNotFound = true;
                    }
                }
            }
            else
            {
                IsPageNotFound = true;
                isSucess = false;
            }

            return isSucess;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Get all feature details
        /// </summary>
        private void GetFeatureDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                            .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    objFeature = _cache.GetArticlesDetails(BasicId);
                    objImg = _cache.GetArticlePhotos((int)BasicId);
                    if (objFeature != null)
                    {
                        IsContentFound = true;
                       
                        GetTaggedBikeListByMake();
                        GetTaggedBikeListByModel();
                        GetTaggedBikeBodyStyle();
                    }
                    else
                    {
                        IsContentFound = false;
                    }
                }

            }
            catch (Exception err)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, "Bikewale.BindViewModels.Webforms.EditCMS.FeaturesDetails.GetFeatureDetails");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Jan 2017
        /// Summary    : Get body style of tagged model
        /// </summary>
        private void GetTaggedBikeBodyStyle()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();

                    IBikeModelsCacheRepository<int> modelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                    if (taggedModelObj != null)
                        BodyStyle = modelCache.GetBikeBodyType((uint)taggedModelObj.ModelId);
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.EditCMS.FeatureDetails.GetTaggedBikeBodyStyle");
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
                if (objFeature != null && objFeature.VehiclTagsList.Count > 0)
                {

                    var _taggedMakeObj = objFeature.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                    if (_taggedMakeObj != null)
                    {
                        taggedMakeObj = _taggedMakeObj.MakeBase;
                    }
                    else
                    {
                        taggedMakeObj = objFeature.VehiclTagsList.FirstOrDefault().MakeBase;
                        FetchMakeDetails();
                    }
                }
            }
            catch(Exception err)
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.mobile.viewF.FetchMakeDetails");
                objErr.SendMail();
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
                if (objFeature != null && objFeature.VehiclTagsList.Count > 0)
                {

                    var _taggedModelObj = objFeature.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.ModelBase.MaskingName));
                    if (_taggedModelObj != null)
                    {
                        taggedModelObj = _taggedModelObj.ModelBase;
                    }
                    else
                    {
                        taggedModelObj = objFeature.VehiclTagsList.FirstOrDefault().ModelBase;
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