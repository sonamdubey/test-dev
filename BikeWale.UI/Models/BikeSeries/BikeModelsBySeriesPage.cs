using Bikewale.BAL.GrpcFiles.Specs_Features;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Pages;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.BikeSeries
{
    public class BikeModelsBySeriesPage
    {
        private readonly IBikeSeries _bikeSeries = null;
        public BikeModelsBySeriesPage(IBikeSeries bikeSeries)
        {
            _bikeSeries = bikeSeries;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Sep 2017
        /// Summary : Model page for Bike Models By SeriesId
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public BikeSeriesModelsVM GetData(uint modelId, uint seriesId)
        {
            BikeSeriesModelsVM objModelsVM = new BikeSeriesModelsVM();
            try
            {
                objModelsVM.SeriesModels = _bikeSeries.GetModelsListBySeriesId(modelId, seriesId);
                if (objModelsVM.SeriesModels.NewBikes != null && objModelsVM.SeriesModels.NewBikes.Any())
                {
                    IEnumerable<NewBikeEntityBase> newBikeList = objModelsVM.SeriesModels.NewBikes;
                    var versionMinSpecsDict = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(newBikeList.Select(m => m.objVersion.VersionId),
                                                                            new List<EnumSpecsFeaturesItem> {
                                                                                EnumSpecsFeaturesItem.Displacement,
                                                                                EnumSpecsFeaturesItem.FuelEfficiencyOverall,
                                                                                EnumSpecsFeaturesItem.MaxPower}).ToDictionary(t => t.VersionId);
                    foreach (var seriesBike in newBikeList)
                    {
                        VersionMinSpecsEntity minSpecs;
                        if (versionMinSpecsDict.TryGetValue(seriesBike.objVersion.VersionId, out minSpecs))
                        {
                            seriesBike.MinSpecsList = minSpecs.MinSpecsList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Models.BikeSeries.BikeModelsBySeriesPage.GetModelsBySeries ModelId = {0} and SeriesId = {1}", modelId, seriesId));
            }
            return objModelsVM;
        }
    }
}