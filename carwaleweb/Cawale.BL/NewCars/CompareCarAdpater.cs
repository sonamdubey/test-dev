using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using System;
using System.Collections.Generic;

namespace Carwale.BL.NewCars
{
    public class CompareCarAdpater : IServiceAdapter
    {
        private readonly ICompareCarsBL _compRepo;
        protected readonly ICarMakesRepository _makesRepo;
        private readonly ICarModelRepository _carModelsRepo;
        private readonly ICarVersionCacheRepository _carVersionsCacheRepo;
        private readonly int _cityId;
        private readonly List<int> _versionIds;

        public CompareCarAdpater(ICompareCarsBL compRepo, ICarMakesRepository makesRepo, ICarModelRepository carModelsRepo, ICarVersionCacheRepository carVersionsCacheRepo, int cityId, List<int> versionIds)
        {
           _compRepo = compRepo;
           _makesRepo = makesRepo;
           _carModelsRepo = carModelsRepo;
           _versionIds = versionIds;
           _cityId = cityId;
           _carVersionsCacheRepo = carVersionsCacheRepo;
        }
        public CompareCarsModel GetCompareCarsModel()
        {
            CompareCarsModel compareCarsModel = new CompareCarsModel();
            try
            {
                compareCarsModel.HotComparisons = _compRepo.GetHotComaprisons(new Pagination() { PageNo = 1, PageSize = 4 }, _cityId);
                compareCarsModel.Makes = _makesRepo.GetCarMakesByType("new");
                compareCarsModel.Makes.Sort((make1, make2) => make1.MakeName.CompareTo(make2.MakeName));
                if (_versionIds != null && _versionIds.Count > 0)
                {
                    compareCarsModel.VersionsDetails = new List<CarVersionDetails>();
                    foreach (var version in _versionIds)
                    {
                        compareCarsModel.VersionsDetails.Add(_carVersionsCacheRepo.GetVersionDetailsById(version));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CompareCarAdpater.GetCompareCarsModel()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return compareCarsModel;
        }

        public T Get<T>()
        {
            return (T)Convert.ChangeType(GetCompareCarsModel(), typeof(T));
        }
    }
}
