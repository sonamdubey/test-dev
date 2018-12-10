using AEPLCore.Cache.Interfaces;
using Carwale.Entity;
using Carwale.Entity.Accessories.Tyres;
using Carwale.Entity.AutoComplete;
using Carwale.Interfaces;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.Interfaces.CarData;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.Accessories.Tyres
{
    public class TyresBL : ITyresBL
    {
        private readonly ICarVersionCacheRepository _carVersionCache;
        private readonly ITyresRepository _tyresRepo;
        private readonly IAccessoryCache _accessoryCache;
        private readonly ICarModelCacheRepository _modelCacheRepo;
        private readonly ICarVersionCacheRepository _versionCacheRepo;

        public TyresBL(ICarVersionCacheRepository carVersionCache, ITyresRepository tyresRepo, ICacheManager cache, IAccessoryCache accessoryCache, ICarModelCacheRepository modelCacheRepo, ICarVersionCacheRepository versionCacheRepo)
        {
            _carVersionCache = carVersionCache;
            _tyresRepo = tyresRepo;
            _accessoryCache = accessoryCache;
            _modelCacheRepo = modelCacheRepo;
            _versionCacheRepo = versionCacheRepo;
        }

        public TyreList GetTyresByCarModels(string carModelIds, int pageNumber, int pageSize)
        {
            var tyreList = new TyreList();
            tyreList.Tyres = new List<TyreSummary>();

            var modelTyresData = new List<TyreSummary>();

            modelTyresData = _tyresRepo.GetTyresByModels(carModelIds);
            tyreList.Tyres = modelTyresData;

            if (modelTyresData != null)
            {
                tyreList = GetSponsoredTyreAtTop(tyreList, carModelIds, -1);
                tyreList.Count = modelTyresData.Count;
                tyreList.Tyres = GetTyresList(modelTyresData, pageNumber, pageSize);
            }

            return tyreList;
        }

        public VersionTyres GetTyresByCarVersion(int carVersionId, int pageNumber, int pageSize)
        {
            VersionTyres versionTyres = new VersionTyres();
            versionTyres.Tyres = new List<TyreSummary>();

            versionTyres = _tyresRepo.GetTyresByVersionId(carVersionId);

            if (versionTyres.Tyres != null)
            {
                var sponsoredTyreData = GetSponsoredTyreAtTop(versionTyres, "-1", carVersionId);
                versionTyres.LoadAdslot = sponsoredTyreData.LoadAdslot;
                versionTyres.Count = versionTyres.Tyres.Count;
                versionTyres.Tyres = GetTyresList(versionTyres.Tyres, pageNumber, pageSize);

                foreach (var tyre in versionTyres.Tyres)
                {
                    tyre.TyreDetailPageUrl = ManageCarUrl.TyreDetailPageUrl(tyre.BrandName, tyre.ModelName, tyre.Size, tyre.ItemId);
                }
            }

            return versionTyres;
        }

        private TyreList GetSponsoredTyreAtTop(TyreList tyreList, string modelIds, int versionId)
        {
            try
            {
                int makeId = -1;
                int modelId = modelIds.Split(',').Select(i => int.Parse(i)).FirstOrDefault();
                if (modelId > 0)
                {
                    makeId = _modelCacheRepo.GetModelDetailsById(modelId).MakeId;
                }
                if (versionId > 0)
                {
                    makeId = _versionCacheRepo.GetVersionDetailsById(versionId).MakeId;
                }
                if (makeId == 1 || makeId == 18 || makeId == 19 || makeId == 11 || makeId == 37)
                {
                    var tyres = tyreList.Tyres;
                    int index = tyres.FindIndex(x => (x.BrandName + x.ModelName).ToLower() == "apolloaspire 4g");
                    if (index >= 0)
                    {
                        var topItem = tyres[index];
                        topItem.IsSponsored = true;
                        tyres.RemoveAt(index);
                        tyres.Insert(0, topItem);
                    }
                    else
                    {
                        tyreList.LoadAdslot = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return tyreList;
        }

        public TyreDetailSummary GetTyreDataByItemId(int itemId)
        {
            TyreDetailSummary tyreData = new TyreDetailSummary();
            try
            {
                ItemData itemData = _accessoryCache.GetAccessoryDataByItemId(itemId);

                tyreData.TyreSummary = itemData.ItemSummary;

                List<LabelValue> overview = new List<LabelValue>();

                string size = string.Format("{0}/{1} {2}{3} {4}{5}", itemData.ItemFeatures[0].Value, string.IsNullOrEmpty(itemData.ItemFeatures[1].Value) ? "" : itemData.ItemFeatures[1].Value.Split('.')[0], itemData.ItemFeatures[2].Value, itemData.ItemFeatures[3].Value, itemData.ItemFeatures[5].Value, itemData.ItemFeatures[4].Value);
                tyreData.Size = size;

                foreach (var featureCat in itemData.FeatureCategories)
                {
                    IEnumerable<LabelValue> data = itemData.ItemFeatures
                               .Where(T => (T.FeatureCategoryId == featureCat.Id && T.Id > 2 && T.Id != 4 && !String.IsNullOrEmpty(T.Value)))
                               .Select(T => new LabelValue { Label = T.Name, Value = T.Value });
                    overview.AddRange(data);
                }
                if (!String.IsNullOrEmpty(itemData.ItemSummary.Weight))
                    overview.Insert(3, new LabelValue { Label = "Tyre Weight", Value = itemData.ItemSummary.Weight.ToString() });
                tyreData.Overview = overview;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return tyreData;
        }

        public TyreList GetTyresByBrandId(int brandId, int pageNumber, int pageSize)
        {
            var tyreList = new TyreList();
            tyreList.Tyres = new List<TyreSummary>();

            var brandTyresData = _tyresRepo.GetTyresByBrandId(brandId);
            
            if (brandTyresData != null)
            {
                tyreList.Count = brandTyresData.Count;
                tyreList.Tyres = GetTyresList(brandTyresData, pageNumber, pageSize);
            }

            return tyreList;
        }

        private List<TyreSummary> GetTyresList(List<TyreSummary> tyreList, int pageNumber, int pageSize)
        {
            var skip = pageSize * (pageNumber - 1);
            return tyreList.Skip(skip).Take(pageSize).ToList();
        }

        public List<int> CheckForTyres(List<int> versionIds)
        {
            List<int> versionsWithTyres = new List<int>();
            for (int i = 0; i < versionIds.Count; i++)
            {
                var tyreData = GetTyresByCarVersion(versionIds[i], 0, 10);
                if (tyreData != null && tyreData.Count > 0)
                {
                    versionsWithTyres.Add(versionIds[i]);
                }
            }
            return versionsWithTyres;
        }
    }
}
