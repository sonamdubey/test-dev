using Bikewale.Cache.Core;
using Bikewale.Cache.Used;
using Bikewale.Cache.UsedBikes;
using Bikewale.DAL.Used;
using Bikewale.DAL.UsedBikes;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    public class BindUsedBikesControl
    {
        public uint TotalRecords { get; set; }
        public int FetchedRecordsCount { get; set; }
        public int? CityId { get; set; }
        public string makeName = string.Empty;
        public string modelName = string.Empty;
        public string cityName = string.Empty;
        public string makeMaskingName = string.Empty;
        public string modelMaskingName = string.Empty;
        public string cityMaskingName = string.Empty;
        public string pageHeading = string.Empty;
        public IEnumerable<MostRecentBikes> objMostRecentBikes = null;
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }

        public uint TopCount { get; set; }

        private IEnumerable<PopularUsedBikesEntity> popularUsedBikes = null;

        public void BindRepeater(Repeater repeater)
        {
            FetchedRecordsCount = 0;

            try
            {
                FetchPopularUsedBike(TotalRecords, CityId);

                if (repeater != null && FetchedRecordsCount > 0)
                {
                    repeater.DataSource = popularUsedBikes;
                    repeater.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }
        /// <summary>
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added cityMaskingName
        /// </summary>
        /// <returns></returns>
        private void FetchPopularUsedBike(uint topCount, int? cityId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IPopularUsedBikesCacheRepository, PopularUsedBikesCacheRepository>();
                    container.RegisterType<IUsedBikesRepository, UsedBikesRepository>();
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    IPopularUsedBikesCacheRepository _objUsedBikes = container.Resolve<IPopularUsedBikesCacheRepository>();
                    popularUsedBikes = _objUsedBikes.GetPopularUsedBikes(topCount, cityId);
                }

                if (popularUsedBikes != null && popularUsedBikes.Any())
                {
                    FetchedRecordsCount = popularUsedBikes.Count();
                    cityMaskingName = popularUsedBikes.FirstOrDefault().CityMaskingName;
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }
        /// <summary>
        /// Created By Subodh Jain 16 sep 2016
        /// Desc:- Bind Used Bike weidegt on mobile for model and make page
        /// </summary>
        /// <param name="MakeId"></param>
        /// <param name="ModelId"></param>
        /// <param name="CityId"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public int BindRecentUsedBikes(Repeater rptRecentUsedBikes, Repeater rptUsedBikeNoCity)
        {
            try
            {

                if (TopCount <= 0) { TopCount = 6; }


                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikes, Bikewale.BAL.UsedBikes.UsedBikes>();
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>();
                    container.RegisterType<IUsedBikesRepository, UsedBikesRepository>();
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    container.RegisterType<IUsedBikesCache, UsedBikesCache>();
                    container.RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>();
                    IUsedBikesCache objUsedBikes = container.Resolve<IUsedBikesCache>();

                    objMostRecentBikes = objUsedBikes.GetUsedBikes(MakeId, ModelId, Convert.ToUInt32(CityId.HasValue ? CityId.Value : 0), TopCount);
                    if (objMostRecentBikes != null)
                        FetchedRecordsCount = objMostRecentBikes.Count();

                    if (objMostRecentBikes != null && objMostRecentBikes.Any())
                    {
                        MostRecentBikes objMostRecentUsedBikes = null;
                        objMostRecentUsedBikes = objMostRecentBikes.FirstOrDefault();
                        if (objMostRecentUsedBikes != null)
                        {
                            makeName = objMostRecentUsedBikes.MakeName;
                            if (ModelId > 0)
                            {
                                modelName = objMostRecentUsedBikes.ModelName;
                                modelMaskingName = objMostRecentUsedBikes.ModelMaskingName;
                            }

                            makeMaskingName = objMostRecentUsedBikes.MakeMaskingName;
                            pageHeading = String.Format("{0} {1}", makeName, modelName);

                            if (CityId.HasValue && CityId.Value > 0)
                            {
                                cityName = objMostRecentUsedBikes.CityName;
                                cityMaskingName = objMostRecentUsedBikes.CityMaskingName;
                                rptRecentUsedBikes.DataSource = objMostRecentBikes;
                                rptRecentUsedBikes.DataBind();

                            }
                            else
                            {
                                rptUsedBikeNoCity.DataSource = objMostRecentBikes;
                                rptUsedBikeNoCity.DataBind();

                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, String.Format("UsedBikeControl.BindUsedBikes({0},{1})", ModelId, CityId.HasValue ? CityId.Value : 0));
                
            }

            return FetchedRecordsCount;

        }
        /// <summary>
        /// Fetch recent
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="makeID"></param>
        /// <param name="modelID"></param>
        /// <param name="cityID"></param>
        /// <returns></returns>
        public IEnumerable<MostRecentBikes> FetchUsedBikes(uint topCount, uint makeID, uint modelID, int? cityID)
        {
            try
            {
                uint curCity = 0;
                if (topCount == 0) { topCount = 6; }
                if (cityID != null) { curCity = Convert.ToUInt32(cityID); }

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikes, Bikewale.BAL.UsedBikes.UsedBikes>()
                        .RegisterType<IUsedBikesRepository, UsedBikesRepository>()
                        .RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                        .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IUsedBikesCache, UsedBikesCache>();
                    IUsedBikesCache objUsedBikes = container.Resolve<IUsedBikesCache>();
                    objMostRecentBikes = objUsedBikes.GetUsedBikes(makeID, modelID, curCity, TopCount);
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, ":- FetchUsedBikes");
                
            }

            return objMostRecentBikes;
        }
    }
}