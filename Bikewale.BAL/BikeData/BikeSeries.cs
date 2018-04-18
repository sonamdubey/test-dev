using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BAL.BikeData
{
    public class BikeSeries : IBikeSeries
    {
        private readonly IBikeSeriesCacheRepository _bikeSeriesCacheRepository = null;
        private readonly IApiGatewayCaller _apiGatewayCaller;
        public BikeSeries(IBikeSeriesCacheRepository bikeSeriesCacheRepository, IApiGatewayCaller apiGatewayCaller)
        {
            _bikeSeriesCacheRepository = bikeSeriesCacheRepository;
            _apiGatewayCaller = apiGatewayCaller;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : BAL method to get new models of a series with city price.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId)
        {
            IEnumerable<NewBikeEntityBase> objModels = null;
            try
            {
                objModels = _bikeSeriesCacheRepository.GetNewModels(seriesId, cityId);
                var specItemList = new List<EnumSpecsFeaturesItems>
                {
                    EnumSpecsFeaturesItems.Displacement,
                    EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                    EnumSpecsFeaturesItems.MaxPowerBhp,
                    EnumSpecsFeaturesItems.KerbWeight
                };
                BindMinSpecs(objModels, specItemList);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetNewModels_SeriesId_{0}_CityId_{1}", seriesId, cityId));
            }
            return objModels;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : BAL method to get upcoming models of a series.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId)
        {
            IEnumerable<UpcomingBikeEntityBase> objModels = null;
            try
            {
                objModels = _bikeSeriesCacheRepository.GetUpcomingModels(seriesId);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetUpcomingModels_SeriesId_{0}", seriesId));
            }
            return objModels;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Sep 2017
        /// Summary : Get models by series id
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public BikeSeriesModels GetModelsListBySeriesId(uint modelId, uint seriesId)
        {
            BikeSeriesModels objModels = null;
            try
            {
                if (modelId > 0 && seriesId > 0)
                {
                    objModels = _bikeSeriesCacheRepository.GetModelsListBySeriesId(seriesId);
                    if (objModels != null)
                    {
                        if (objModels.NewBikes != null)
                        {
                            objModels.NewBikes = objModels.NewBikes.Where(bike => bike.BikeModel.ModelId != modelId);
                            IEnumerable<NewBikeEntityBase> newBikeList = objModels.NewBikes;
                            if (newBikeList != null && newBikeList.Any())
                            {
                                var specItemList = new List<EnumSpecsFeaturesItems>
                                {
                                    EnumSpecsFeaturesItems.Displacement,
                                    EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                                    EnumSpecsFeaturesItems.MaxPowerBhp
                                };
                                BindMinSpecs(newBikeList, specItemList);
                            }
                        }

                        if (objModels.UpcomingBikes != null)
                        {
                            objModels.UpcomingBikes = objModels.UpcomingBikes.Where(bike => bike.BikeModel.ModelId != modelId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetModelsListBySeries ModelId = {0} and SeriesId = {1}", modelId, seriesId));
            }
            return objModels;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : BAL method to get synopsis of a series.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public BikeDescriptionEntity GetSynopsis(uint seriesId)
        {
            BikeDescriptionEntity synopsis = null;
            try
            {
                synopsis = _bikeSeriesCacheRepository.GetSynopsis(seriesId);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetSynopsis_SeriesId_{0}", seriesId));
            }
            return synopsis;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : BAL method to get all series of a make.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId, uint seriesId)
        {
            IEnumerable<BikeSeriesEntity> bikeSeriesEntityList = null;
            try
            {
                bikeSeriesEntityList = _bikeSeriesCacheRepository.GetOtherSeriesFromMake(makeId);
                if (bikeSeriesEntityList != null && bikeSeriesEntityList.Any())
                {
                    bikeSeriesEntityList = bikeSeriesEntityList.Where(m => m.SeriesId != seriesId);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetOtherSeriesFromMake_makeId_{0}_SeriesId_{1}", makeId, seriesId));
            }
            return bikeSeriesEntityList;
        }

        public IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId)
        {
            IEnumerable<BikeSeriesCompareBikes> bikeSeriesCompareBikes = null;
            try
            {
                bikeSeriesCompareBikes = _bikeSeriesCacheRepository.GetBikesToCompare(seriesId);
                BindSeriesModelsCompareSpecs(bikeSeriesCompareBikes);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetBikesToCompare_seriesId = {0}", seriesId));
            }
            return bikeSeriesCompareBikes;
        }

        /// <summary>
        /// created by : vivek singh tomar on 24th nov 2017
        /// summary : get modelids as comma separated string by series id
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public string GetModelIdsBySeries(uint seriesId)
        {
            string modelIds = string.Empty;
            try
            {
                if (seriesId > 0)
                {
                    modelIds = _bikeSeriesCacheRepository.GetModelIdsBySeries(seriesId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeSeries.GetMaskingNames seriesId {0}", seriesId));
            }
            return modelIds;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 11 Apr 2018.
        /// Description : Method to call specs features service and bind specs features data in bikeList object.
        /// </summary>
        /// <param name="bikesList">List of bikes object in which specs binding has to be done.</param>
        /// <param name="specItemList">List of specs ids for which specs data has to be done.</param>
        private void BindMinSpecs(IEnumerable<NewBikeEntityBase> bikesList, IEnumerable<EnumSpecsFeaturesItems> specItemList)
        {
            try
            {
                if (bikesList != null && bikesList.Any())
                {
                    GetVersionSpecsByItemIdAdapter adapt1 = new GetVersionSpecsByItemIdAdapter();
                    VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
                    {
                        Versions = bikesList.Select(m => m.objVersion.VersionId),
                        Items = specItemList
                    };
                    adapt1.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                    _apiGatewayCaller.Call();

                    IEnumerable<VersionMinSpecsEntity> specsResponseList = adapt1.Output;
                    if (specsResponseList != null)
                    {
                        var specsEnumerator = specsResponseList.GetEnumerator();
                        var bikesEnumerator = bikesList.GetEnumerator();
                        while (bikesEnumerator.MoveNext() && specsEnumerator.MoveNext())
                        {
                            bikesEnumerator.Current.MinSpecsList = specsEnumerator.Current.MinSpecsList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeSeries.BindMinSpecs_bikesList_{0}_specItemList_{1}", bikesList, specItemList));
            }
        }

        private void BindSeriesModelsCompareSpecs(IEnumerable<BikeSeriesCompareBikes> seriesCompareBikesWithSpecs)
        {
            if (seriesCompareBikesWithSpecs != null && seriesCompareBikesWithSpecs.Any())
            {
                GetVersionSpecsByItemIdAdapter adapt1 = new GetVersionSpecsByItemIdAdapter();
                VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
                {
                    Versions = seriesCompareBikesWithSpecs.Select(m => m.VersionId),
                    Items = new List<EnumSpecsFeaturesItems>
                    {
                        EnumSpecsFeaturesItems.Displacement,
                        EnumSpecsFeaturesItems.KerbWeight,
                        EnumSpecsFeaturesItems.FuelTankCapacity,
                        EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                        EnumSpecsFeaturesItems.SeatHeight,
                        EnumSpecsFeaturesItems.RearBrakeType,
                        EnumSpecsFeaturesItems.NoOfGears,
                        EnumSpecsFeaturesItems.MaxPowerBhp,
                        EnumSpecsFeaturesItems.MaxPowerRpm
                    }
                };
                adapt1.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                _apiGatewayCaller.Call();

                IEnumerable<VersionMinSpecsEntity> versionMinSpecs = adapt1.Output;
                if (versionMinSpecs != null)
                {
                    var minSpecs = versionMinSpecs.GetEnumerator();
                    foreach (var seriesBike in seriesCompareBikesWithSpecs)
                    {
                        if (minSpecs.MoveNext())
                        {
                            float value;
                            ushort gears;
                            foreach (var spec in minSpecs.Current.MinSpecsList)
                            {
                                switch ((EnumSpecsFeaturesItems)spec.Id)
                                {
                                    case EnumSpecsFeaturesItems.Displacement:
                                        if (float.TryParse(spec.Value, out value))
                                            seriesBike.Displacement = value;
                                        break;
                                    case EnumSpecsFeaturesItems.KerbWeight:
                                        if (float.TryParse(spec.Value, out value))
                                            seriesBike.Weight = value;
                                        break;
                                    case EnumSpecsFeaturesItems.FuelEfficiencyOverall:
                                        if (float.TryParse(spec.Value, out value))
                                            seriesBike.Mileage = value;
                                        break;
                                    case EnumSpecsFeaturesItems.FuelTankCapacity:
                                        if (float.TryParse(spec.Value, out value))
                                            seriesBike.FuelCapacity = value;
                                        break;
                                    case EnumSpecsFeaturesItems.SeatHeight:
                                        if (float.TryParse(spec.Value, out value))
                                            seriesBike.SeatHeight = value;
                                        break;
                                    case EnumSpecsFeaturesItems.RearBrakeType:
                                        seriesBike.BrakeType = spec.Value;
                                        break;
                                    case EnumSpecsFeaturesItems.NoOfGears:
                                        if (ushort.TryParse(spec.Value, out gears))
                                            seriesBike.Gears = gears;
                                        break;
                                    case EnumSpecsFeaturesItems.MaxPowerRpm:
                                        if (float.TryParse(spec.Value, out value))
                                            seriesBike.MaxPowerRpm = value;
                                        break;
                                    case EnumSpecsFeaturesItems.MaxPowerBhp:
                                        if (float.TryParse(spec.Value, out value))
                                            seriesBike.MaxPower = value;
                                        break;
                                }
                            }
                        }
                    }
                }
                

            }
        }
    }   // class
}   // namespace
