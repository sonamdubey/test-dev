
using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Memcache;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;
namespace Bikewale.BindViewModels.Webforms.Compare
{

    public class CompareBikes
    {
        private IBikeModelsCacheRepository<int> objModelCache = null;
        private IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache = null;
        public GlobalCityAreaEntity cityArea = null;
        public bool isPageNotFound, isPermanentRedirect;
        public string redirectionUrl = string.Empty, versionsList;
        public uint versionId1, versionId2;

        public CompareBikes()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>();


                    objModelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                    objModelMaskingCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                }

                ParseQueryString();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.CompareBikes : CompareBikes");
            }

            cityArea = GlobalCityArea.GetGlobalCityArea();
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 Sept 2014
        /// Summary : Get compare Bike detail by version id
        /// </summary>
        protected void GetCompareBikeDetails()
        {
            try
            {
                Bikewale.New.CompareBikes cb = new Bikewale.New.CompareBikes();
                ds = cb.GetComparisonBikeListByVersion(versionsList, cityArea.CityId);
                if (ds != null)
                {
                    bikeDetails = ds.Tables[0];
                    bikeSpecs = ds.Tables[1];
                    bikeFeatures = ds.Tables[2];
                }
                count = bikeDetails.Rows.Count;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    targetedModels += "\"" + ds.Tables[0].Rows[i]["Model"] + "\",";
                }
                if (targetedModels.Length > 2)
                {
                    targetedModels = targetedModels.Substring(0, targetedModels.Length - 1);
                }

                if (count > 1)
                {
                    if (Convert.ToUInt32(bikeDetails.Rows[0]["bikeCount"]) > 0 || Convert.ToUInt32(bikeDetails.Rows[1]["bikeCount"]) > 0)
                    {
                        isUsedBikePresent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of GetCompareBikeDetails


        /// <summary>
        /// Created By : Sushil Kumar on 30th Jan 2017
        /// Summary : To get version list from querystring and memcache
        /// </summary>
        protected void ParseQueryString()
        {
            var request = HttpContext.Current.Request;
            string QueryString = request.QueryString.ToString();


            string bike1 = request["bike1"], bike2 = request["bike2"], modelList = HttpUtility.ParseQueryString(QueryString).Get("mo");
            uint.TryParse(bike1, out versionId1); uint.TryParse(bike2, out versionId2);

            if (versionId1 > 0 && versionId2 > 0)
            {
                versionsList = string.Format("{0},{1}", versionId1, versionId2);
            }
            else if (!string.IsNullOrEmpty(modelList))
            {
                string[] models = HttpUtility.ParseQueryString(QueryString).Get("mo").Split(',');
                ModelMaskingResponse objResponse = null;
                ModelMapping objCache = new ModelMapping();
                int totalModels = models.Length;

                for (int iTmp = 0; iTmp < totalModels; iTmp++)
                {
                    string modelMaskingName = models[iTmp].ToLower();
                    if (!string.IsNullOrEmpty(modelMaskingName) && objModelMaskingCache != null)
                    {
                        objResponse = objModelMaskingCache.GetModelMaskingResponse(modelMaskingName);
                    }

                    if (objResponse != null && objResponse.StatusCode == 200)
                    {
                        versionsList += objCache.GetTopVersionId(models[iTmp].ToLower()) + (((iTmp + 1) < totalModels) ? "," : "");
                    }
                    else if (objResponse != null && objResponse.StatusCode == 301)
                    {
                        isPermanentRedirect = true;
                        if (String.IsNullOrEmpty(redirectionUrl))
                            redirectionUrl = request.RawUrl.Replace(models[iTmp].ToLower(), objResponse.MaskingName);
                        else
                            redirectionUrl = redirectionUrl.Replace(models[iTmp].ToLower(), objResponse.MaskingName);
                    }
                    else
                    {
                        isPageNotFound = true;
                        break;
                    }
                }
            }
        }
        //End of getVersionIdList
    }
}