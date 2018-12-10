using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.CarData
{
    public class CarRootsBL : ICarRoots
    {
        protected readonly ICarModelRootsCacheRepository _modelsRootCache;

        public CarRootsBL(ICarModelRootsCacheRepository modelsRootCache)
        {
            _modelsRootCache = modelsRootCache;
        }

        public List<int> GetYearsByRootId(int rootId)
        {
            List<int> rootIdYears = new List<int>();
            try
            {
                List<CarLaunchDiscontinueYear> rootYears = _modelsRootCache.GetYearsByRootId(rootId);

                int maxYear = rootYears != null && rootYears.Count > 0 ? rootYears.Max(x => x.DiscontinueYear) : 0;

                foreach (var rootYear in rootYears)
                {
                    int maxDiscontinueYear = rootYear.DiscontinueYear;
                    if (maxDiscontinueYear == 0)
                        maxDiscontinueYear = maxYear;

                    for (int year = rootYear.LaunchYear; year <= maxDiscontinueYear; year++)
                        rootIdYears.Add(year);

                    if (maxDiscontinueYear == maxYear)
                        break;
                }
                return rootIdYears.Distinct().OrderByDescending(x => x).ToList();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, string.Format("CarRootsBL.GetYearsByRootId({0}): ", rootId));
                objErr.LogException();
                return null;
            }            
        }
    }
}
