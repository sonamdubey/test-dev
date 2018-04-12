
using Bikewale.BAL.GrpcFiles.Specs_Features;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Bikewale.Models
{
    public class ElectricBikesPage
    {
        private ElectricBikesPageVM objData = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly IBikeModelsCacheRepository<int> _modelCacheRepository = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels;

        public ushort TopCountBrand { get; set; }
        public bool IsMobile { get; set; }
        public ElectricBikesPage(IBikeModelsCacheRepository<int> modelCacheRepository, IBikeMakesCacheRepository objMakeCache, ICMSCacheContent articles, IVideos videos, IBikeMakesCacheRepository bikeMakes, IBikeModels<BikeModelEntity, int> bikeModels)
        {
            _objMakeCache = objMakeCache;
            _articles = articles;
            _videos = videos;
            _bikeMakes = bikeMakes;
            _modelCacheRepository = modelCacheRepository;
            _bikeModels = bikeModels;
        }

        public uint EditorialTopCount { get; set; }

        /// <summary>
        /// Created By :- Subodh Jain 07-12-2017
        /// Summary :- Method for GetData
        /// </summary>
        /// <returns></returns>
        public ElectricBikesPageVM GetData()
        {
            objData = new ElectricBikesPageVM();
            try
            {
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
                uint customerCityId = location.CityId;
                if (customerCityId > 0)
                    objData.ElectricBikes = _bikeModels.GetElectricBikes(customerCityId);
                else
                    objData.ElectricBikes = _bikeModels.GetElectricBikes();
                BindEditorialWidget();
                objData.Brands = new BrandWidgetModel(TopCountBrand, _bikeMakes).GetData(Entities.BikeData.EnumBikeType.New);
                BindPageMetas();

            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.ElectricBikesPage.GetData()");
            }

            return objData;
        }

        /// Modified by : Snehal Dange on 28th Dec 2017
        /// Descritption : Added 'New Bikes' in Breadcrumb
        private void SetBreadcrumList(ref ElectricBikesPageVM objData)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    url += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", url), "New Bikes"));

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Electric Bike and Scooters"));


                objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.ElectricBikesPage.BindPageMetas()");
            }

        }

        private void BindPageMetas()
        {
            try
            {
                objData.PageMetaTags.Description = "Find electric bikes and scooters prices, images, and reviews on BikeWale. Know about dealers and offers for your favorite electric bike.";
                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/electric-bikes/", BWConfiguration.Instance.BwHostUrl);
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/electric-bikes/", BWConfiguration.Instance.BwHostUrl);
                objData.PageMetaTags.Title = "Electric Bikes & Scooters, Electric Scooty- Prices, Images, and Reviews- BikeWale";
                SetBreadcrumList(ref objData);
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.ElectricBikesPage.BindPageMetas()");
            }

        }

        /// <summary>
        /// Created By :- Subodh Jain 07-12-2017
        /// Summary :- Method for BindEditorialWidget
        /// </summary>
        /// <returns></returns>
        private void BindEditorialWidget()
        {
            try
            {

                string modelList = string.Join(",", objData.ElectricBikes.Select(m => m.objModel.ModelId));
                var News = Task.Factory.StartNew(() => new RecentNews(EditorialTopCount, 0, modelList, _articles).GetData());
                var ExpertReviews = Task.Factory.StartNew(() => new RecentExpertReviews(EditorialTopCount, 0, modelList, _articles).GetData());
                var Videos = Task.Factory.StartNew(() => new RecentVideos(1, (ushort)EditorialTopCount, modelList, _videos).GetData());

                Task.WaitAll(News, ExpertReviews, Videos);


                objData.News = News.Result;
                objData.ExpertReviews = ExpertReviews.Result;
                objData.Videos = Videos.Result;
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.ElectricBikesPage.BindEditorialWidget()");
            }




        }
    }

}