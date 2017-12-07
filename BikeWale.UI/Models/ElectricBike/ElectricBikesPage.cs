
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
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
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;

        public ushort TopCountBrand { get; set; }

        public ElectricBikesPage(IBikeModels<BikeModelEntity, int> bikeModels, IBikeMakesCacheRepository objMakeCache, ICMSCacheContent articles, IVideos videos, IBikeMakesCacheRepository bikeMakes)
        {
            _objMakeCache = objMakeCache;
            _articles = articles;
            _videos = videos;
            _bikeMakes = bikeMakes;
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
                objData.ElectricBikes = _bikeModels.GetElectricBikes();
                BindEditorialWidget();
                objData.Brands = new BrandWidgetModel(TopCountBrand, _bikeMakes).GetData(Entities.BikeData.EnumBikeType.New);
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.ElectricBikesPage.GetData()");
            }

            return objData;
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