﻿using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.CompareBikes;
using Bikewale.Models.Used;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Bikewale.Models.BikeSeries
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 17 Nov 2017
    /// Description : Provide methods to get data for series page.
    /// </summary>
    public class SeriesPage
    {
        private bool IsScooter;
        public CompareSources CompareSource { get; set; }
        public string MaskingName { get; set; }
        public bool IsMobile { get; set; }
        private readonly IUsedBikesCache _usedBikesCache;
        private readonly IBikeSeries _bikeSeries = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly IBikeSeriesCacheRepository _seriesCache = null;
        private readonly IBikeCompare _compareScooters = null;

        public SeriesPage(IBikeSeriesCacheRepository seriesCache, IUsedBikesCache usedBikesCache, IBikeSeries bikeSeries, ICMSCacheContent articles, IVideos videos, IBikeCompare compareScooters)
        {
            _bikeSeries = bikeSeries;
            _usedBikesCache = usedBikesCache;
            _articles = articles;
            _videos = videos;
            _compareScooters = compareScooters;
            _seriesCache = seriesCache;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Base method to get data for series page.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public SeriesPageVM GetData(uint seriesId)
        {
            SeriesPageVM objSeriesPage = null;
            try
            {
                objSeriesPage = new SeriesPageVM();
                objSeriesPage.SeriesModels = _seriesCache.GetModelsListBySeriesId(seriesId);
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
                objSeriesPage.City = new CityEntityBase();
                if (location != null && location.CityId > 0)
                {
                    objSeriesPage.City.CityId = location.CityId;
                    objSeriesPage.City.CityName = location.City;

                }
				
				BindSeriesBase(objSeriesPage, seriesId);
                
                BindSeriesSynopsis(objSeriesPage);

                objSeriesPage.objUsedBikes = GetUsedBikesForSeries(seriesId, objSeriesPage.City.CityId);


                objSeriesPage.SeriesModels = new BikeSeriesModels();
                objSeriesPage.SeriesModels.NewBikes = _bikeSeries.GetNewModels(seriesId, objSeriesPage.City.CityId);

                if (objSeriesPage.SeriesModels.NewBikes != null && objSeriesPage.SeriesModels.NewBikes.Any())
                {
                    var firstNewBike = objSeriesPage.SeriesModels.NewBikes.FirstOrDefault();
                    if (firstNewBike != null)
                    {
                        objSeriesPage.BikeMake = firstNewBike.BikeMake;
                    }
                    IsScooter = objSeriesPage.SeriesModels.NewBikes.All(m => m.BodyStyle == (ushort)EnumBikeBodyStyles.Scooter);
                }

                objSeriesPage.SeriesModels.UpcomingBikes = _bikeSeries.GetUpcomingModels(seriesId);

                GetBikesToCompare(objSeriesPage);
                BindCMSContent(objSeriesPage);
                BindOtherSeriesFromMake(objSeriesPage);
                BindPageMetas(objSeriesPage);
                SetBreadcrumList(objSeriesPage);
                SetPageJSONLDSchema(objSeriesPage);
                BindCompareScooters(objSeriesPage, CompareSource);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.GetData");
            }
            return objSeriesPage;
        }

		private void BindSeriesBase(SeriesPageVM objSeriesPage, uint seriesId)
		{
			try
			{
				objSeriesPage.SeriesBase = new BikeSeriesEntityBase();
				objSeriesPage.SeriesBase.SeriesId = seriesId;
				objSeriesPage.SeriesBase.MaskingName = MaskingName;
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, "ModelPage.BindSeriesBase()");
			}
		}

		private UsedBikeByModelCityVM GetUsedBikesForSeries(uint seriesid, uint cityId)
        {
            UsedBikeByModelCityVM usedBikeModel = new UsedBikeByModelCityVM();
            try
            {
                UserBikeSeriesModelsWidget objUsedBike = new UserBikeSeriesModelsWidget(_usedBikesCache, seriesid, cityId);
                usedBikeModel = objUsedBike.GetData();
				if (usedBikeModel != null)
				{
					usedBikeModel.City = new CityHelper().GetCityById(cityId);
				}
			}
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SeriesPage.BindUsedBikeByModel()");
            }

            return usedBikeModel;

        }


        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Method to bind content for news, videos and expert reviews.
        /// </summary>
        /// <param name="objSeriesPage"></param>
        private void BindCMSContent(SeriesPageVM objSeriesPage)
        {
            try
            {
                StringBuilder modelIdList = new StringBuilder("");
                foreach (var bike in objSeriesPage.SeriesModels.NewBikes)
                {
                    modelIdList.Append(bike.BikeModel.ModelId);
                    modelIdList.Append(",");
                }
                foreach (var bike in objSeriesPage.SeriesModels.UpcomingBikes)
                {
                    modelIdList.Append(bike.BikeModel.ModelId);
                    modelIdList.Append(",");
                }
                if (modelIdList.Length > 0)
                {
                    modelIdList.Remove(modelIdList.Length - 1, 1);
                }

                ushort topCount = 3;
                RecentNews recentNews = new RecentNews(topCount, (uint)objSeriesPage.BikeMake.MakeId, Convert.ToString(modelIdList), _articles)
                {
                    IsScooter = IsScooter
                };
                objSeriesPage.News = recentNews.GetData();
                objSeriesPage.News.Title = string.Format("{0} {1} News", objSeriesPage.BikeMake.MakeName, objSeriesPage.SeriesBase.SeriesName);

                RecentExpertReviews recentExpertReviews = new RecentExpertReviews(topCount, (uint)objSeriesPage.BikeMake.MakeId, Convert.ToString(modelIdList), _articles)
                {
                    IsScooter = IsScooter
                };
                objSeriesPage.ExpertReviews = recentExpertReviews.GetData();

                ushort pageNo = 1;
                ushort pageSize = (ushort)(IsMobile ? 2 : 4);
                RecentVideos recentVideos = new RecentVideos(pageNo, pageSize, Convert.ToString(modelIdList), _videos)
                {
                    IsScooter = IsScooter
                };
                objSeriesPage.Videos = recentVideos.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.BindCMSContent");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Method to bind other series form same make.
        /// </summary>
        /// <param name="objSeriesPage"></param>
        private void BindOtherSeriesFromMake(SeriesPageVM objSeriesPage)
        {
            try
            {
                if (objSeriesPage.BikeMake != null && objSeriesPage.SeriesBase != null)
                {
                    objSeriesPage.OtherSeriesList = _bikeSeries.GetOtherSeriesFromMake(objSeriesPage.BikeMake.MakeId, objSeriesPage.SeriesBase.SeriesId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.BindOtherSeriesFromMake");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Method to bind series synopsis.
        /// </summary>
        /// <param name="objSeriesPage"></param>
        private void BindSeriesSynopsis(SeriesPageVM objSeriesPage)
        {
            try
            {
                if (objSeriesPage.SeriesBase != null)
                {
                    objSeriesPage.SeriesDescription = _bikeSeries.GetSynopsis(objSeriesPage.SeriesBase.SeriesId);
                    objSeriesPage.SeriesBase.SeriesName = objSeriesPage.SeriesDescription.Name;
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.BindSeriesSynopsis");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Method to bind Json LD schema.
        /// </summary>
        /// <param name="objSeriesPage"></param>
        private void SetPageJSONLDSchema(SeriesPageVM objSeriesPage)
        {
            try
            {
                WebPage webpage = SchemaHelper.GetWebpageSchema(objSeriesPage.PageMetaTags, objSeriesPage.BreadcrumbList);
                objSeriesPage.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.SetPageJSONLDSchema");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Method to set breadcrum list.
        /// </summary>
        /// <param name="objSeriesPage"></param>
        private void SetBreadcrumList(SeriesPageVM objSeriesPage)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string bikeUrl, scooterUrl;
                bikeUrl = scooterUrl = string.Format("{0}/", BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    bikeUrl += "m/";
                    scooterUrl += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));
                if (objSeriesPage.BikeMake != null)
                {
                    bikeUrl = string.Format("{0}{1}-bikes/", bikeUrl, objSeriesPage.BikeMake.MakeMaskingName);
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} Bikes", objSeriesPage.BikeMake.MakeName)));
                    if (IsScooter)
                    {
                        scooterUrl = string.Format("{0}{1}-scooters/", scooterUrl, objSeriesPage.BikeMake.MakeMaskingName);
                        BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, scooterUrl, string.Format("{0} Scooters", objSeriesPage.BikeMake.MakeName)));
                    }
                }
                if (objSeriesPage.SeriesBase != null && objSeriesPage.BikeMake != null)
                {
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objSeriesPage.BikeMake.MakeName + " " + objSeriesPage.SeriesBase.SeriesName));
                }
                objSeriesPage.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.SetBreadcrumList");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Method to bind page metas.
        /// </summary>
        /// <param name="objSeriesPage"></param>
        private void BindPageMetas(SeriesPageVM objSeriesPage)
        {
            try
            {
                if (objSeriesPage.SeriesBase != null && objSeriesPage.BikeMake != null)
                {
                    objSeriesPage.PageMetaTags.Title = string.Format("{0} {1} Price, {2} {1} Models, Images, Colours, Mileage & Reviews | BikeWale",
                            objSeriesPage.BikeMake.MakeName, objSeriesPage.SeriesBase.SeriesName, DateTime.Now.Year);

                    if (objSeriesPage.SeriesModels != null && objSeriesPage.SeriesModels.NewBikes != null && objSeriesPage.SeriesModels.NewBikes.Any(b => b.BikeModel != null))
                    {
                        objSeriesPage.PageMetaTags.Description = string.Format("{0} {1} price in India – Rs. {2} - {3}." +
                        " It is available in {4} models in India. {0} {5} is the most popular {1}. " +
                        "Check out {1} on road price, reviews, mileage, versions, news & images at Bikewale",
                            objSeriesPage.BikeMake.MakeName, objSeriesPage.SeriesBase.SeriesName,
                            Format.FormatPrice(Convert.ToString(objSeriesPage.SeriesModels.NewBikes.Min(x => x.Price.AvgPrice))),
                            Format.FormatPrice(Convert.ToString(objSeriesPage.SeriesModels.NewBikes.Max(x => x.Price.AvgPrice))),
                            objSeriesPage.SeriesModels.NewBikes.Count(), objSeriesPage.SeriesModels.NewBikes.FirstOrDefault().BikeModel.ModelName);
                    }

                    if (objSeriesPage.SeriesModels.NewBikes != null)
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append(objSeriesPage.BikeMake.MakeName);
                        str.Append(" ");
                        str.Append(objSeriesPage.SeriesBase.SeriesName);
                        foreach (var bike in objSeriesPage.SeriesModels.NewBikes)
                        {
                            str.Append(", ");
                            str.Append(bike.BikeMake.MakeName);
                            str.Append(" ");
                            str.Append(bike.BikeModel.ModelName);
                        }
                        objSeriesPage.PageMetaTags.Keywords = Convert.ToString(str);
                    }

                    objSeriesPage.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/{2}/", BWConfiguration.Instance.BwHostUrl, objSeriesPage.BikeMake.MakeMaskingName, objSeriesPage.SeriesBase.MaskingName);
                    objSeriesPage.PageMetaTags.AlternateUrl = string.Format("{0}/m/{1}-bikes/{2}/", BWConfiguration.Instance.BwHostUrl, objSeriesPage.BikeMake.MakeMaskingName, objSeriesPage.SeriesBase.MaskingName);
                    objSeriesPage.AdTags.TargetedSeries = objSeriesPage.SeriesBase.SeriesName;

                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.BindPageMetas");
            }
        }

        /// <summary>
        /// Created By :- Subodh Jain 17-11-2017
        /// Summary :- GetCompareBikes Details
        /// </summary>
        /// <param name="objSeriesPage"></param>
        private void GetBikesToCompare(SeriesPageVM objSeriesPage)
        {

            try
            {
                objSeriesPage.ObjModel = new BikeSeriesCompareVM();
                objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs = _seriesCache.GetBikesToCompare(objSeriesPage.SeriesBase.SeriesId);

                for (int i = 0; i < objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Count(); i++)
                {
                    objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.ElementAt(i).Price = objSeriesPage.SeriesModels.NewBikes.ElementAt(i).Price.ExShowroomPrice > 0 ? objSeriesPage.SeriesModels.NewBikes.ElementAt(i).Price.ExShowroomPrice : objSeriesPage.SeriesModels.NewBikes.ElementAt(i).Price.AvgPrice;
                }


                IList<string> objList = new List<string>();
                objList.Add("Price");
                objList.Add("Displacement");
                objList.Add("Weight");
                objList.Add("Fuel Tank Capacity");
                objList.Add("Mileage");
                objList.Add("Seat Height");
                objList.Add("Brake Type");
                objList.Add("Gears");
                objList.Add("Max Power");


                objSeriesPage.ObjModel.ObjBikeSpecs = new BikeSpecs();
                objSeriesPage.ObjModel.ObjBikeSpecs.Price = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.Price != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Min(m => m.Price)).Count() + 1);
                objSeriesPage.ObjModel.ObjBikeSpecs.MaxPower = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.MaxPower != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.MaxPower)).Count() + 1);
                objSeriesPage.ObjModel.ObjBikeSpecs.Mileage = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.Mileage != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.Mileage)).Count() + 1);
                objSeriesPage.ObjModel.ObjBikeSpecs.Weight = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.Weight != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Min(m => m.Weight)).Count() + 1);
                objSeriesPage.ObjModel.ObjBikeSpecs.FuelCapacity = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.FuelCapacity != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.FuelCapacity)).Count() + 1);
                objSeriesPage.ObjModel.ObjBikeSpecs.Displacement = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.Displacement != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.Displacement)).Count() + 1);

                objSeriesPage.ObjModel.BikeCompareSegments = objList;
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.GetBikesToCompare");
            }
        }


        private void BindCompareScooters(SeriesPageVM objViewModel, CompareSources CompareSource)
        {
            try
            {
                string versionList = string.Join(",", objViewModel.SeriesModels.NewBikes.Select(m => m.objVersion.VersionId));
                PopularModelCompareWidget objCompare = new PopularModelCompareWidget(_compareScooters, 1, objViewModel.City.CityId, versionList);
                objViewModel.SimilarCompareScooters = objCompare.GetData();
                objViewModel.SimilarCompareScooters.CompareSource = CompareSource;
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "ScootersIndexPageModel.BindCompareScootes()");
            }
        }

    }
}