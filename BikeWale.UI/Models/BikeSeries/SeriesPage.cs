using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Bikewale.Models.BikeSeries
{
	public class SeriesPage
	{
		private bool IsScooter;
		public bool IsMobile;
		private readonly IBikeSeries _bikeSeries = null;
		private readonly ICMSCacheContent _articles = null;
		private readonly IVideos _videos = null;
		public SeriesPage(IBikeSeries bikeSeries, ICMSCacheContent articles, IVideos videos)
        {
			_bikeSeries = bikeSeries;
			_articles = articles;
			_videos = videos;
        }

		public SeriesPageVM GetData(uint seriesId)
        {
            SeriesPageVM objSeriesPage = null;
            try
            {
                objSeriesPage = new SeriesPageVM();
				GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
				objSeriesPage.City = new CityEntityBase();
				if (location != null && location.CityId > 0)
				{
					objSeriesPage.City.CityId = location.CityId;
					objSeriesPage.City.CityName = location.City;
				}
				objSeriesPage.SeriesBase = new BikeSeriesEntityBase();
				objSeriesPage.SeriesBase.SeriesId = seriesId;

				BindSeriesSynopsis(objSeriesPage);
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
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.GetData");
            }
            return objSeriesPage;
		}

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
				RecentNews recentNews = new RecentNews(3, (uint)objSeriesPage.BikeMake.MakeId, modelIdList.ToString(), _articles)
				{
					IsScooter = IsScooter
				};
				objSeriesPage.News = recentNews.GetData();
				objSeriesPage.News.Title = string.Format("{0} {1} News", objSeriesPage.BikeMake.MakeName, objSeriesPage.SeriesBase.SeriesName);

				RecentExpertReviews recentExpertReviews = new RecentExpertReviews(3, (uint)objSeriesPage.BikeMake.MakeId, modelIdList.ToString(), _articles)
				{
					IsScooter = IsScooter
				};
				objSeriesPage.ExpertReviews = recentExpertReviews.GetData();

				RecentVideos recentVideos = new RecentVideos(1, 2, modelIdList.ToString(), _videos)
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

		private void BindOtherSeriesFromMake(SeriesPageVM objSeriesPage)
		{
			try
			{
				if (objSeriesPage.BikeMake != null)
				{
					 objSeriesPage.OtherSeriesList = _bikeSeries.GetOtherSeriesFromMake(objSeriesPage.BikeMake.MakeId);
				}
			}
			catch (Exception ex)
			{

				throw;
			}
		}

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

		private void SetBreadcrumList(SeriesPageVM objSeriesPage)
		{
			try
			{
				IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
				string bikeUrl;
				bikeUrl = "/";
				ushort position = 1;
				if (IsMobile)
				{
					bikeUrl += "m/";
				}

				BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));
				if (objSeriesPage.BikeMake != null)
				{
					bikeUrl = string.Format("{0}/{1}-{2}/", bikeUrl, objSeriesPage.BikeMake.MakeMaskingName, IsScooter ? "scooters":"bikes");
					BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} {1}", objSeriesPage.BikeMake.MakeName, IsScooter ? "Scooters" : "Bikes")));
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

		private void BindPageMetas(SeriesPageVM objSeriesPage)
		{
			try
			{
				//<Make> <Series> price in India – Rs. <x> - <z>. It is available 
				// in <y> models in India. <Model> is the most popular <Series>. 
				// Check out <Series> on road price, reviews, mileage, versions, news & images at Bikewale
				if (objSeriesPage.SeriesBase != null && objSeriesPage.BikeMake != null)
				{
					objSeriesPage.PageMetaTags.Title = string.Format("{0} {1} Price, {2} {1} Models, Images, Colours, Mileage & Reviews | BikeWale",
							objSeriesPage.BikeMake.MakeName, objSeriesPage.SeriesBase.SeriesName, DateTime.Now.Year);

					if (objSeriesPage.SeriesModels != null && objSeriesPage.SeriesModels.NewBikes != null && objSeriesPage.SeriesModels.NewBikes.Any( b => b.BikeModel != null))
					{
						objSeriesPage.PageMetaTags.Description = string.Format("{0} {1} price in India – Rs. {2} - {3}." +
						"It is available in {4} models in India. {0} {5} is the most popular {1}. " +
						"Check out {1} on road price, reviews, mileage, versions, news & images at Bikewale",
							objSeriesPage.BikeMake.MakeName, objSeriesPage.SeriesBase.SeriesName,
							objSeriesPage.SeriesModels.NewBikes.Min(x => x.Price.AvgPrice),
							objSeriesPage.SeriesModels.NewBikes.Max(x => x.Price.AvgPrice),
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

					objSeriesPage.PageMetaTags.CanonicalUrl = string.Format("/{0}-bikes/{1}/", objSeriesPage.BikeMake.MakeMaskingName, objSeriesPage.SeriesBase.MaskingName);
					objSeriesPage.PageMetaTags.AlternateUrl = string.Format("/m/{0}", objSeriesPage.PageMetaTags.CanonicalUrl);
					//objSeriesPage.PageMetaTags.OGImage
				}
				
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.BindPageMetas");
			}
        }
        /// <summary>
        /// Created By :- Subodh Jain 17-11-2013
        /// Summary :- GetCompareBikes Details
        /// </summary>
        /// <param name="objSeriesPage"></param>
        private void GetBikesToCompare(SeriesPageVM objSeriesPage)
        {
            objSeriesPage.ObjModel = new BikeSeriesCompareVM();
            objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs = _bikeSeries.GetBikesToCompare(3);

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
            var objData = objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.ToList();

            objSeriesPage.ObjModel.ObjBikeSpecs.MaxPower = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.MaxPower != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.MaxPower)).Count() + 1);
            objSeriesPage.ObjModel.ObjBikeSpecs.Mileage = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.Mileage != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.Mileage)).Count() + 1);
            objSeriesPage.ObjModel.ObjBikeSpecs.Weight = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.Weight != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Min(m => m.Weight)).Count() + 1);
            objSeriesPage.ObjModel.ObjBikeSpecs.FuelCapacity = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.FuelCapacity != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.FuelCapacity)).Count() + 1);
            objSeriesPage.ObjModel.ObjBikeSpecs.Displacement = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.Displacement != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.Displacement)).Count() + 1);





            objSeriesPage.ObjModel.BikeCompareSegments = objList;




        }

    }
}