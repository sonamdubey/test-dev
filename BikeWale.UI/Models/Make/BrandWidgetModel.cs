using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 10 Mar 2017
    /// Summary: Model to holf scooter's brands- topBrands and remaining brands
    /// </summary>
    public class BrandWidgetModel
    {
        private IEnumerable<BikeMakeEntityBase> _brands = null;
        public ushort TopCount { get; private set; }
        private readonly IBikeMakesCacheRepository _makeCacheRepo = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache = null;
        public BrandWidgetModel(ushort topCount, IBikeMakesCacheRepository bikeMakes)
        {
            _makeCacheRepo = bikeMakes;
            TopCount = topCount;
        }

        public BrandWidgetModel(ushort topCount, IBikeMakesCacheRepository bikeMakes, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache)
        {
            _makeCacheRepo = bikeMakes;
            TopCount = topCount;
            _objModelCache = objModelCache;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   TO Support New launched bike makes
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="newLaunches"></param>
        public BrandWidgetModel(ushort topCount, INewBikeLaunchesBL newLaunches)
        {
            _newLaunches = newLaunches;
            TopCount = topCount;
        }

        public BrandWidgetVM GetData(EnumBikeType page)
        {

            BrandWidgetVM objData = new BrandWidgetVM();
            switch (page)
            {

                case EnumBikeType.New:
                    _brands = _makeCacheRepo.GetMakesByType(page);
                    foreach (var make in _brands)
                    {
                        make.Href = String.Format("/{0}-bikes/", make.MaskingName);
                        make.Title = String.Format("{0} bikes", make.MakeName);
                    }
                    break;
                case EnumBikeType.Used:
                    break;
                case EnumBikeType.Dealer:
                    _brands = _makeCacheRepo.GetMakesByType(page);
                    foreach (var make in _brands)
                    {
                        make.Href = String.Format("/dealer-showrooms/{0}/", make.MaskingName);
                        make.Title = String.Format("{0} dealer showrooms in India", make.MakeName);
                    }
                    break;
                case EnumBikeType.ServiceCenter:
                    _brands = _makeCacheRepo.GetMakesByType(page);
                    foreach (var make in _brands)
                    {
                        make.Href = String.Format("/service-centers/{0}/", make.MaskingName);
                        make.Title = String.Format("{0} service centers in India", make.MakeName);
                    }
                    break;
                case EnumBikeType.Scooters:
                    _brands = _makeCacheRepo.GetScooterMakes();
                    foreach (var make in _brands)
                    {
                        make.Href = String.Format("/{0}-{1}/", make.MaskingName, !make.IsScooterOnly ? "scooters" : "bikes");
                        make.Title = String.Format("{0} scooters", make.MakeName);
                    }
                    break;
                case EnumBikeType.NewLaunched:
                    if (_newLaunches != null)
                    {
                        _brands = _newLaunches.GetMakeList().Select(m => m.Make);
                        foreach (var make in _brands)
                        {
                            make.Href = Utility.UrlFormatter.FormatMakeWiseBikeLaunchedUrl(make.MaskingName);
                            make.Title = String.Format("Newly launched {0} bikes", make.MakeName);
                        }
                    }
                    break;
                case EnumBikeType.Videos:
                    _brands = _objModelCache.GetMakeIfVideo();
                    foreach (var make in _brands)
                    {
                        make.Href = String.Format("/{0}-bikes/videos/", make.MaskingName);
                        make.Title = String.Format("{0} bikes videos", make.MakeName);
                    }
                    break;
                case EnumBikeType.UserReviews:
                    _brands = _makeCacheRepo.GetMakesByType(page);
                    foreach (var make in _brands)
                    {
                        make.Href = String.Format("/{0}-bikes/reviews/", make.MaskingName);
                        make.Title = String.Format("{0} bikes reviews", make.MakeName);
                    }
                    break;
                default:
                    break;
            }
            if (_brands != null)
            {
                objData.TopBrands = _brands.Take(TopCount);
                if (_brands.Count() > TopCount)
                    objData.OtherBrands = _brands.Skip(TopCount).OrderBy(m => m.MakeName);
            }
            return objData;
        }
    }
}