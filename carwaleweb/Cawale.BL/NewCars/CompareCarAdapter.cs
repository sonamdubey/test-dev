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
    public class CompareCarAdapter : IServiceAdapter
    {
        private readonly ICompareCarsBL _compRepo;
        private readonly ICarMakesRepository _makesRepo;
        private readonly ICarVersionCacheRepository _carVersionsCacheRepo;
        private readonly int _cityId;
        private readonly System.Collections.Specialized.NameValueCollection _versionIds;
        private readonly List<int> _removedCompareCarMakes = new List<int> { 24, 26, 38, 53 };
        public CompareCarAdapter(ICompareCarsBL compRepo, ICarMakesRepository makesRepo, ICarVersionCacheRepository carVersionsCacheRepo,
            int cityId, System.Collections.Specialized.NameValueCollection versionIds)
        {
            _compRepo = compRepo;
            _makesRepo = makesRepo;
            _versionIds = versionIds;
            _cityId = cityId;
            _carVersionsCacheRepo = carVersionsCacheRepo;
        }
        public CompareCarsModel GetCompareCarsModel()
        {
            CompareCarsModel compareCarsModel = new CompareCarsModel();
            try
            {
                compareCarsModel.HotComparisons = _compRepo.GetHotComaprisons(new Pagination() { PageNo = 1, PageSize = 6 }, _cityId, true);
                compareCarsModel.Makes = _makesRepo.GetCarMakesByType("compareall");
                if (compareCarsModel.Makes != null)
                {
                    compareCarsModel.Makes.RemoveAll(make => _removedCompareCarMakes.IndexOf(make.MakeId) != -1);
                    compareCarsModel.Makes.Sort((make1, make2) => make1.MakeName.CompareTo(make2.MakeName));
                }
                if (_versionIds != null && _versionIds.Count > 0)
                {
                    compareCarsModel.VersionsDetails = new List<CarVersionDetails>();
                    GetVersionDetails(compareCarsModel);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CompareCarAdapter.GetCompareCarsModel()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return compareCarsModel;
        }

        public T Get<T>()
        {
            return (T)Convert.ChangeType(GetCompareCarsModel(), typeof(T));
        }

        private void GetVersionDetails(CompareCarsModel compareCarsModel)
        {
            List<int> versionKeys = new List<int>();
            int carVersionCount = _versionIds.Count;
            for (int count = 1; count <= carVersionCount; count++)
            {
                if (_versionIds["car" + count] != null)
                {
                    int versionId = -1;
                    if (int.TryParse(_versionIds["car" + count], out versionId))
                    {
                        versionKeys.Add(versionId);
                    }
                }
            }
            if (versionKeys.Count > 0)
            {
                var versionDetailsData = _carVersionsCacheRepo.MultiGetVersionDetails(versionKeys);
                foreach (var key in versionKeys)
                {
                    CarVersionDetails carData = versionDetailsData[key];
                    if (carData != null)
                    {
                        compareCarsModel.VersionsDetails.Add(carData);
                    }
                }
            }
        }
    }
}
