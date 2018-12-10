using Carwale.Entity.CarData;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.CarData
{
    public class CarMakesBL : ICarMakes
    {
        protected readonly ICarMakesCacheRepository _makesCacheRepo;
        private readonly ICarModels _carModelsBL;
        private readonly ICarPriceQuoteAdapter _prices;
        private readonly ICarModelRepository _modelRepo;
        private readonly ICarModelCacheRepository _modelCacheRepo;

        public CarMakesBL(ICarMakesCacheRepository makesCacheRepo, ICarModels carModelsBL, ICarPriceQuoteAdapter prices, ICarModelRepository modelRepo, ICarModelCacheRepository modelCacheRepo)
        {
            _makesCacheRepo = makesCacheRepo;
            _carModelsBL = carModelsBL;
            _prices = prices;
            _modelRepo = modelRepo;
            _modelCacheRepo = modelCacheRepo;
        }

        /// <summary>
        /// Returns the Title for Make page 
        /// </summary>
        /// <param name="title">Title from Page Meta Tags</param>
        /// <param name="makeName"></param>
        /// <returns></returns>
        public string Title(string title, string makeName)
        {
            if (!string.IsNullOrEmpty(title)) // title from Page meta tags 
                return title;
            else
                return makeName + " Cars in India - Prices (GST Rates), Reviews, Photos & More";
        }

        /// <summary>
        /// Returns the Description for Make page
        /// </summary>
        /// <param name="description">Description from Page Meta Tags</param>
        /// <param name="makeName"></param>
        /// <returns></returns>
        public string Description(string description, string makeName)
        {
            if (!string.IsNullOrEmpty(description))
            {
                return description;         // description from Page Meta tags
            }
            else
            {
                return makeName + " cars in India. Know everything you want to know about " + makeName
                    + " car models. CarWale offers " + makeName + " history, reviews, photos and news etc. Find "
                    + makeName + " dealers, participate in " + makeName + " discussions and know upcoming cars";
            }
        }

        /// <summary>
        /// Returns the Keywords for Make page 
        /// </summary>
        /// <param name="keywords">Keywords from Page Meta tags</param>
        /// <param name="makeName"></param>
        /// <returns></returns>
        public string Keywords(string keywords, string makeName)
        {
            if (!string.IsNullOrEmpty(keywords))
            {
                return keywords;            //keywords from Page Meta tags
            }
            else
            {
                return makeName + " cars, " + makeName + " India, " + makeName + " car prices, buy "
                    + makeName + " cars, " + makeName + " reviews, car reviews, car news";
            }
        }

        /// <summary>
        /// Returns the Heading for Make page 
        /// </summary>
        /// <param name="heading">Heading from PageMeta tags</param>
        /// <param name="makeName"></param>
        /// <returns></returns>
        public string Heading(string heading, string makeName)
        {
            if (!string.IsNullOrEmpty(heading))
            {
                return heading; //heading from Page Meta tags
            }
            else
            {
                return makeName + " Cars";
            }
        }

        /// <summary>
        /// Returns the Summary for MakePage
        /// Written By : Shalini on 26/11/14
        /// </summary>
        /// <param name="summary">Summary from Page Meta tags</param>
        /// <param name="makeName"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public string Summary(string summary, string makeName, int makeId)
        {
            if (!string.IsNullOrEmpty(summary))
            {
                return summary;             // Summary from Page Meta tags
            }
            else
            {
                int count = 0;
                string makeSummary = makeName + " offers ";
                try
                {
                    List<CarMakeDescription> summaryList = _makesCacheRepo.GetSummary(makeId);

                    for (int i = 0; i < summaryList.Count; i++)
                    {
                        if (count == 0)
                        {
                            makeSummary += summaryList[i].NoOfModels + " new car models in " + summaryList[i].ModelSegment + " segment";
                        }
                        else
                        {
                            makeSummary += ", " + summaryList[i].NoOfModels + " in " + summaryList[i].ModelSegment + " segment";
                        }
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler err = new ExceptionHandler(ex, "CarMakesBL.Summary");
                    err.LogException();
                }
                return makeSummary + " in India." + " Choose a " + makeName + " car to know prices, features, reviews and photos.";
            }
        }

        public List<CarMakeEntityBase> GetCarMakesByType(string type)
        {
            throw new NotImplementedException();
        }

        public CarMakeDescription GetCarMakeDescription(int makeId)
        {
            throw new NotImplementedException();
        }

        public List<CarModelSummary> GetNewCarModelsWithDetails(int cityId, int makeId, bool orp = false)
        {
            List<CarModelSummary> newCarModelsDetails = new List<CarModelSummary>();
            try
            {
                newCarModelsDetails = _carModelsBL.GetNewModelsByMake(makeId) ?? new List<CarModelSummary>();
                var modellist = newCarModelsDetails.Select(x => x.ModelId).ToList();
                IDictionary<int, PriceOverview> modelPrices = null;
                if (modellist.Count > 0)
                {
                    modelPrices = _prices.GetModelsCarPriceOverview(modellist, cityId, orp) ?? new Dictionary<int, PriceOverview>();
                }
                if (modelPrices != null)
                {
                    newCarModelsDetails.ForEach(x =>
                    {
                        PriceOverview priceOverview;
                        modelPrices.TryGetValue(x.ModelId, out priceOverview);
                        x.CarPriceOverview = priceOverview ?? new PriceOverview();
                    });
                }
                else
                {
                    newCarModelsDetails.ForEach(x => { x.CarPriceOverview = new PriceOverview(); });
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarMakesBL.GetNewCarModelsWithDetails()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return newCarModelsDetails;
        }
        public List<ModelSummary> GetActiveModelsWithDetails(int cityId, int makeId, bool orp = false)
        {
            List<ModelSummary> orderedModelList = null;
            try
            {
                orderedModelList = _modelCacheRepo.GetActiveModelsByMake(makeId);
                if (orderedModelList == null)
                {
                    orderedModelList = new List<ModelSummary>();
                    List<ModelSummary> activeModelList = _modelRepo.GetActiveModelsByMake(makeId);
                    List<ModelSummary> newModelList = new List<ModelSummary>();
                    List<ModelSummary> upComingModelList = new List<ModelSummary>();
                    foreach (var model in activeModelList)
                    {
                        if (model.New)
                        {
                            model.ShowDate = DateTimeUtility.ShowLaunchLabel(model.LaunchDate, DateTime.Now);
                            newModelList.Add(model);
                        }
                        else
                        {
                            model.ShowDate = model.CWConfidence == 5 && DateTimeUtility.IsDateWithIn31Days(model.LaunchDate, DateTime.Now);
                            model.CarPriceOverview = new PriceOverview { Price = (int)model.MinPrice };
                            upComingModelList.Add(model);
                        }
                    }

                    activeModelList = new List<ModelSummary>();

                    FilterList(newModelList, orderedModelList, activeModelList);

                    SortUpcomingModel(upComingModelList);

                    FilterList(upComingModelList, orderedModelList, activeModelList);

                    if (activeModelList.IsNotNullOrEmpty())
                    {
                        orderedModelList.AddRange(activeModelList);
                    }
                    if (orderedModelList.IsNotNullOrEmpty())
                    {
                        _modelCacheRepo.SetActiveModelsByMakeInCache(makeId, orderedModelList);
                        foreach (var model in orderedModelList)
                        {
                            if (model.New)
                            {
                                model.CarPriceOverview = _prices.GetAvailablePriceForModel(model.ModelId, cityId, ORP: orp) ?? new PriceOverview();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return orderedModelList;
        }
        public void FilterList(List<ModelSummary> modelList, IList<ModelSummary> orderedModelList, IList<ModelSummary> activeModelList)
        {
            foreach (ModelSummary model in modelList)
            {
                if (model.ShowDate)
                {
                    orderedModelList.Add(model);
                }
                else
                {
                    activeModelList.Add(model);
                }
            }
        }
        public void SortUpcomingModel(List<ModelSummary> upComingModelList)
        {
            try
            {
                if (upComingModelList.Count > 0)
                {
                    upComingModelList.Sort((m1, m2) =>
                    {
                        DateTime d1 = m1.ShowDate ? m1.LaunchDate : m1.LaunchDate.LastDateOfMonth();
                        DateTime d2 = m2.ShowDate ? m2.LaunchDate : m2.LaunchDate.LastDateOfMonth();
                        if (d1 == d2)
                        {
                            return m1.MinPrice < m2.MinPrice ? -1 : 1;
                        }
                        else
                        {
                            return d1 < d2 ? -1 : 1;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "failed in Sorting Upcoming List in CarMakesBl.SortUpcomingModel()");
            }
        }
    }
}
