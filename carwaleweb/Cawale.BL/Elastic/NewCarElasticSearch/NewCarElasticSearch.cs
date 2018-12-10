using Carwale.DAL.CoreDAL;
using Carwale.Entity;
using Carwale.Entity.Deals;
using Carwale.Entity.ElasticEntities;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Interfaces.PriceQuote;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using Carwale.Notifications.Logs;
using RabbitMqPublishing;
using System.Collections.Specialized;
using Carwale.Entity.Enum;
using System.Configuration;
using Carwale.Interfaces.Elastic;
using Carwale.Utility;
using AEPLCore.Queue;
using AEPLCore.Utils.Serializer;
using Entities.GrpcClass.ProtoClass;

namespace Carwale.BL.Elastic.NewCarSearch
{
    public class NewCarElasticSearch : INewCarElasticSearch
    {
        protected static int takecount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["carindexcount"] ?? "2000");
        protected ICarPriceQuoteAdapter CarPQAdapter;
        protected IDeals cardeals;
        protected IDealsCache dealsCache;
        protected static string ncfIndex = System.Configuration.ConfigurationManager.AppSettings["newcarfinderindex"] ?? "newcarfinderindex";
        protected static string ncfDocType = System.Configuration.ConfigurationManager.AppSettings["ncfcardoctype"] ?? "ncfcar";
        private static string _elasticCarDataQueue = ConfigurationManager.AppSettings["ElasticCarDataQueue"].ToString();
        private static readonly TimeSpan _queueDelay = new TimeSpan(0, 6, 0);

        public NewCarElasticSearch(ICarPriceQuoteAdapter CarPQAdapter, IDeals cardeals, IDealsCache dealsCache)
        {
            this.CarPQAdapter = CarPQAdapter;
            this.cardeals = cardeals;
            this.dealsCache = dealsCache;
        }
        public QueryContainer GetQueryDescriptor<T>(NewCarSearchInputs inputs, bool compensatePrice = true, bool isNewCarFinder = false) where T : class
        {
            QueryContainer queryContainer = new QueryContainer();
            QueryContainerDescriptor<T> FDS = new QueryContainerDescriptor<T>();
            queryContainer &= FDS.Terms(terms => terms.Field("makeId").Terms(inputs.makes));
            string[] modelIds = new string[inputs.ModelIds.Count];
            for (int i = 0; i < inputs.ModelIds.Count; i++)
                modelIds[i] = inputs.ModelIds[i].ToString();

            string[] excludedModelIds = new string[inputs.ExcludedModelIds.Count];
            for (int i = 0; i < inputs.ExcludedModelIds.Count; i++)
                excludedModelIds[i] = inputs.ExcludedModelIds[i].ToString();

            queryContainer &= FDS.Terms(terms => terms.Field("modelId").Terms(modelIds));
            queryContainer &= FDS.Terms(terms => terms.Field("fuelType").Terms(inputs.fueltype));
            queryContainer &= FDS.Terms(terms => terms.Field("transmissionType").Terms(inputs.transmission));
            queryContainer &= FDS.Terms(terms => terms.Field("bodyStyle").Terms(inputs.bodytype));
            queryContainer &= FDS.Term("new", inputs.newCarsOnly);
            queryContainer &= !FDS.Terms(terms => terms.Field("modelId").Terms(excludedModelIds));
            QueryContainer rangeContainer = null;

            if (inputs.budgets.Count > 0 && !isNewCarFinder)
            {
                rangeContainer = new QueryContainer();

                if (compensatePrice)
                {
                    RangeLimit minBudget = inputs.budgets[0];
                    RangeLimit maxBudget = inputs.budgets[0];
                    for (int i = 1; i < inputs.budgets.Count; i++)
                    {
                        if (inputs.budgets[i].LowerLimit < minBudget.LowerLimit) minBudget = inputs.budgets[i];
                        if (inputs.budgets[i].UpperLimit > maxBudget.UpperLimit) maxBudget = inputs.budgets[i];
                    }

                    minBudget.LowerLimit = (int)(minBudget.LowerLimit * 0.9);
                    maxBudget.UpperLimit = (int)(maxBudget.UpperLimit * 1.1);
                }
                foreach (var budget in inputs.budgets)
                {
                    if (budget.LowerLimit == 0 && budget.UpperLimit >= 0)
                    {
                        rangeContainer |= FDS.Range(r => r.Field("avgPrice").GreaterThan(budget.LowerLimit).LessThanOrEquals(budget.UpperLimit));
                    }
                    else if (budget.LowerLimit == 0)
                    {
                        rangeContainer |= FDS.Range(r => r.Field("avgPrice").GreaterThan(budget.LowerLimit));
                    }
                    else if (budget.UpperLimit >= 0)
                    {
                        rangeContainer |= FDS.Range(r => r.Field("avgPrice").GreaterThanOrEquals(budget.LowerLimit).LessThanOrEquals(budget.UpperLimit));
                    }
                    else
                    {
                        rangeContainer |= FDS.Range(r => r.Field("avgPrice").GreaterThanOrEquals(budget.LowerLimit));
                    }
                }

                queryContainer &= rangeContainer;
            }
            if (isNewCarFinder)
            {
                rangeContainer = new QueryContainer();
                if (inputs.budgets.Count > 0)
                {
                    if (inputs.cityId < 0)
                    {

                        rangeContainer |= FDS.Range(r => r.Field("avgPrice").GreaterThanOrEquals(inputs.budgets[0].LowerLimit).LessThanOrEquals(inputs.budgets[0].UpperLimit));

                    }
                    else
                    {
                        rangeContainer |= FDS.Nested(n => n.Path("onRoadCityPrice").InnerHits()
                                  .Query(f => f.Term("onRoadCityPrice.cityId", inputs.cityId) && f.Range(r => r.Field("onRoadCityPrice.price").GreaterThanOrEquals(inputs.budgets[0].LowerLimit).LessThanOrEquals(inputs.budgets[0].UpperLimit))));

                    }
                }
                else if (inputs.cityId > 0)
                {
                    rangeContainer |= FDS.Nested(n => n.Path("onRoadCityPrice").InnerHits()
                                      .Query(f => f.Term("onRoadCityPrice.cityId", inputs.cityId)
                                   ));
                }
                queryContainer &= rangeContainer;
            }

            if (inputs.seatingCapacity.Count > 0)
            {
                rangeContainer = new QueryContainer();
                foreach (var item in inputs.seatingCapacity)
                {
                    if (item.UpperLimit >= 0)
                    {
                        rangeContainer |= FDS.Range(r => r.Field("seatingCapacity").GreaterThanOrEquals(item.LowerLimit).LessThanOrEquals(item.UpperLimit));
                    }
                    else
                    {
                        rangeContainer |= FDS.Range(r => r.Field("seatingCapacity").GreaterThanOrEquals(item.LowerLimit));
                    }
                }

                queryContainer &= rangeContainer;
            }

            if (inputs.enginePower.Count > 0)
            {
                rangeContainer = new QueryContainer();
                foreach (var item in inputs.enginePower)
                {
                    if (item.UpperLimit >= 0)
                    {
                        rangeContainer |= FDS.Range(r => r.Field("powerBHP").GreaterThanOrEquals(item.LowerLimit).LessThanOrEquals(item.UpperLimit));
                    }
                    else
                    {
                        rangeContainer |= FDS.Range(r => r.Field("powerBHP").GreaterThanOrEquals(item.LowerLimit));
                    }
                }

                queryContainer &= rangeContainer;
            }

            if (inputs.useEMI)
            {
                if (inputs.EMI.UpperLimit >= 0)
                {
                    queryContainer &= FDS.Range(r => r.Field("eMI").GreaterThanOrEquals(inputs.EMI.LowerLimit).LessThanOrEquals(inputs.EMI.UpperLimit));
                }
                else
                {
                    queryContainer &= FDS.Range(r => r.Field("eMI").GreaterThanOrEquals(inputs.EMI.LowerLimit));
                }
            }
            return queryContainer;
        }

        public ElasticCarData GetVersions(NewCarSearchInputs inputs, bool isOrp = false)
        {
            ElasticCarData data = new ElasticCarData();
            if (inputs.pageNo < 1 || inputs.pageSize < 1) return null;

            data = GetModelsAndVersions(inputs);

            Dictionary<int, CarBaseEntity> Versions = new Dictionary<int, CarBaseEntity>();

            bool isShowDeals = cardeals.IsShowDeals(inputs.cityId, true);

            foreach (var model in data.ModelVersionDict)
            {
                IDictionary<int, PriceOverview> versionsPrice = CarPQAdapter.GetVersionsPriceForSameModel(model.Key, model.Value.Keys.ToList<int>(), inputs.cityId, isOrp);
                Dictionary<int, Carwale.Entity.Deals.DiscountSummary> versionDiscountByModel = isShowDeals ? dealsCache.BestVersionDealsByModel(model.Key, inputs.cityId) : null;

                foreach (var version in model.Value)
                {
                    DiscountSummary DiscountSummary = null;
                    PriceOverview priceOverview = null;
                    if (versionsPrice.TryGetValue(version.Key, out priceOverview))
                        model.Value[version.Key].PriceOverview = priceOverview;

                    if (inputs.cityId > 0 && isOrp && priceOverview != null)
                    {
                        string emi = Calculation.Calculation.CalculateEmi(priceOverview.Price);
                        version.Value.EMI = Convert.ToInt32(!string.IsNullOrEmpty(emi) ? emi.Replace(",", "") : "0");
                    }

                    if (versionDiscountByModel != null && versionDiscountByModel.TryGetValue(version.Key, out DiscountSummary))
                        model.Value[version.Key].DiscountSummary = DiscountSummary;

                    model.Value[version.Key].MatchingVersionsCount = model.Value.Count;

                    Versions.Add(version.Key, model.Value[version.Key]);
                }
            }

            return data;
        }

        public ElasticCarData GetModels(NewCarSearchInputs inputs, bool isNcf = false)
        {
            ElasticCarData data = new ElasticCarData();
            if (inputs.pageNo < 1 || inputs.pageSize < 1) return null;

            data = GetModelsAndVersions(inputs, isNcf);

            CarBaseEntity tempver = null;

            foreach (var model in data.ModelVersionDict)
            {
                tempver = model.Value.First().Value;
                model.Value[tempver.VersionId].MatchingVersionsCount = model.Value.Count;

                IEnumerable<VersionPrice> prices = CarPQAdapter.GetAllVersionPriceByModelCity(model.Key, inputs.cityId, inputs.ShowOrp) ?? new List<VersionPrice>();
                List<VersionPrice> pricesList = prices.OrderBy(v => v.VersionBase.AveragePrice).ToList();
                model.Value[tempver.VersionId].PriceOverview = CarPQAdapter.GetAvailablePriceForModel(model.Key, inputs.cityId, prices, inputs.ShowOrp);
                int maxprice = 0, minprice = 0;
                if (pricesList.Count > 0)
                {
                    if (inputs.cityId < 1)
                    {
                        minprice = pricesList[0].VersionBase.AveragePrice;
                        maxprice = pricesList[pricesList.Count - 1].VersionBase.AveragePrice;
                    }
                    else
                    {
                        minprice = pricesList[0].VersionBase.ExShowroomPrice > 0 ? pricesList[0].VersionBase.ExShowroomPrice : pricesList[0].VersionBase.AveragePrice;
                        int c = pricesList.Count - 1;
                        maxprice = pricesList[c].VersionBase.ExShowroomPrice > 0 ? pricesList[c].VersionBase.ExShowroomPrice : pricesList[c].VersionBase.AveragePrice;
                    }
                }
                model.Value[tempver.VersionId].ModelMinPrice = minprice;
                model.Value[tempver.VersionId].ModelMaxPrice = maxprice;
            }
            return data;
        }


        public ElasticCarData GetModelsV2(NewCarSearchInputs inputs)
        {
            ElasticCarData data = new ElasticCarData();
            if (inputs.pageNo < 1 || inputs.pageSize < 1) return null;
            data = GetModelsAndVersions(inputs);
            CarBaseEntity tempver = null;
            foreach (var model in data.ModelVersionDict)
            {
                tempver = model.Value.First().Value;
                model.Value[tempver.VersionId].MatchingVersionsCount = model.Value.Count;
                IEnumerable<VersionPrice> prices = CarPQAdapter.GetAllVersionPriceByModelCity(model.Key, inputs.cityId, true) ?? new List<VersionPrice>();
                List<VersionPrice> pricesList = prices.OrderBy(v => v.VersionBase.AveragePrice).ToList();
                model.Value[tempver.VersionId].PriceOverview = CarPQAdapter.GetAvailablePriceForModel(model.Key, inputs.cityId, prices, true);
            }
            return data;
        }


        protected List<int> GetPaginatedModelIds(NewCarSearchInputs inputs, IReadOnlyCollection<CarBaseEntity> modelIds, ref int modelCount)
        {
            var modelSet = new HashSet<int>();
            foreach (var model in modelIds)
            {
                if (model.ModelId > 0)
                {
                    modelSet.Add(model.ModelId);
                }
            }
            int startIndex = ((inputs.pageNo - 1) * inputs.pageSize);
            int takeCount = 0;
            if (startIndex + 1 > modelSet.Count) { return null; }
            else if (startIndex + inputs.pageSize <= modelSet.Count) takeCount = inputs.pageSize;
            else takeCount = modelSet.Count - startIndex;
            modelCount = modelSet.Count;
            return modelSet.ToList().GetRange(startIndex, takeCount);
        }
        public NCFElasticCarData GetNCFModels(NewCarSearchInputs inputs)
        {
            if (!inputs.CountsOnly && (inputs.pageNo < 1 || inputs.pageSize < 1))
            {
                return new NCFElasticCarData();
            }

            var query = GetQueryDescriptor<NewCarFinderEntity>(inputs, false, true);
            ISearchResponse<NewCarFinderEntity> response = ElasticQuery<NewCarFinderEntity>(inputs, query, ncfIndex, ncfDocType, true, true);
            if (response == null || !response.Hits.Any())
            {
                return new NCFElasticCarData();
            }
            var carData = new NCFElasticCarData();
            try
            {
                carData.TotalVersions = (int)response.Total;
                int totalModels = 0;
                inputs.ModelIds = GetPaginatedModelIds(inputs, response.Documents, ref totalModels);
                carData.TotalModels = totalModels;
                if (inputs.ModelIds != null && !inputs.CountsOnly)
                {
                    query = GetQueryDescriptor<NewCarFinderEntity>(inputs, false, true);
                    var finalresponse = ElasticQuery<NewCarFinderEntity>(inputs, query, ncfIndex, ncfDocType, false, true);
                    if (finalresponse != null && finalresponse.Hits.Any())
                    {
                        for (int index = 0; index < finalresponse.Documents.Count; index++)
                        {
                            var version = finalresponse.Documents.ElementAt(index);
                            version.PriceOverview = new PriceOverview();
                            if (inputs.cityId > 0)
                            {
                                int offsetKey = finalresponse.Hits.ElementAt(index).InnerHits.ElementAt(0).Value.Hits.Hits.ElementAt(0).Nested.Offset;
                                if (offsetKey >= 0)
                                {
                                    version.PriceOverview.Price = version.OnRoadCityPrice.ElementAt(offsetKey).Price;
                                    version.PriceOverview.PriceLabel = "On-Road Price";
                                }
                            }
                            else
                            {
                                version.PriceOverview.Price = version.AvgPrice;
                                version.PriceOverview.PriceLabel = "Avg. Ex-Showroom";
                            }
                            if (carData.ModelVersionDict.ContainsKey(version.ModelId))
                            {
                                if (!carData.ModelVersionDict[version.ModelId].ContainsKey(version.VersionId))
                                {
                                    carData.ModelVersionDict[version.ModelId].Add(version.VersionId, version);
                                }
                            }
                            else
                            {
                                carData.ModelVersionDict.Add(version.ModelId, new Dictionary<int, NewCarFinderEntity>() { { version.VersionId, version } });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Faliure in forming NCFElasticCarData in GetNCFModels");
            }
            return carData;
        }

        public ElasticCarData GetModelsAndVersions(NewCarSearchInputs inputs, bool isNcf = false)
        {
            var query = GetQueryDescriptor<CarBaseEntity>(inputs, false, isNcf);
            ISearchResponse<CarBaseEntity> elasticResponse = ElasticQuery<CarBaseEntity>(inputs, query, ncfIndex, ncfDocType, true, isNcf);
            if (elasticResponse == null || !elasticResponse.Hits.Any())
            {
                return new ElasticCarData();
            }
            var carData = new ElasticCarData();
            carData.TotalVersions = (int)elasticResponse.Total;
            int totalModels = 0;
            inputs.ModelIds = GetPaginatedModelIds(inputs, elasticResponse.Documents, ref totalModels);
            carData.TotalModels = totalModels;
            if (inputs.ModelIds != null && !inputs.CountsOnly)
            {
                query = GetQueryDescriptor<CarBaseEntity>(inputs, false, isNcf);
                ISearchResponse<CarBaseEntity> finalresponse = ElasticQuery<CarBaseEntity>(inputs, query, ncfIndex, ncfDocType, false, isNcf);
                foreach (var version in finalresponse.Documents)
                {
                    if (carData.ModelVersionDict.ContainsKey(version.ModelId))
                    {
                        if (!carData.ModelVersionDict[version.ModelId].ContainsKey(version.VersionId))
                        {
                            carData.ModelVersionDict[version.ModelId].Add(version.VersionId, version);
                        }
                    }
                    else
                    {
                        carData.ModelVersionDict.Add(version.ModelId, new Dictionary<int, CarBaseEntity>() { { version.VersionId, version } });
                    }
                }
            }
            return carData;
        }

        public ISearchResponse<T> ElasticQuery<T>(NewCarSearchInputs inputs, QueryContainer queryInputs, string indexName, string docType, bool onlyModel, bool isNcf = false) where T : CarBaseEntity
        {
            if (queryInputs != null && indexName != null && docType != null)
            {
                try
                {
                    ElasticClient clientElastic = ElasticClientInstance.GetInstance();
                    ISearchResponse<T> elasticReponse = null;
                    if (onlyModel)
                    {
                        elasticReponse = clientElastic.Search<T>(s => s
                                          .Index(indexName)
                                          .Type(docType)
                                          .Take(takecount)
                                          .Query(q => queryInputs)
                                          .Source(sf => sf
                                          .Includes(i => i.Fields(fe => fe.ModelId, fe => fe.ModelPopularity, fe => fe.MakeId, fe => fe.MakeName)))
                                          .Sort(sq => GetSortDescriptor(inputs, isNcf))
                                          );
                    }
                    else
                    {
                        elasticReponse = clientElastic.Search<T>(s => s
                                  .Index(indexName)
                                  .Type(docType)
                                  .Take(takecount)
                                  .Query(q => queryInputs)
                                  .Sort(sq => GetSortDescriptor(inputs, isNcf))
                                  );
                    }
                    return elasticReponse;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Error in fethching elastic data");
                }
            }
            return null;
        }
        public static void PushInCarDataQueue(int modelId, IEnumerable<int> versionIds, int cityId, CarDocumentFields type)
        {
            try
            {
                Dictionary<String, Object> arguments = new Dictionary<String, Object>();
                arguments.Add("x-delayed-type", "direct");
                var headers = new Dictionary<String, Object>();
                headers.Add("x-delay", 420000);
                PublishManager rabbitMQManager = new PublishManager();
                var message = new CarDataElasticDoc();

                message.Models.Add(modelId);
                if (versionIds.IsNotNullOrEmpty())
                {
                    message.Versions.AddRange(versionIds);
                }
                message.Cities.Add(cityId);
                message.Type = (UpdateType)type;
                rabbitMQManager.PublishMessage(_elasticCarDataQueue,
                    new QueueMessage
                    {
                        DeadletterCount = 0,
                        FunctionName = "UpdateElasticCarData",
                        ModuleName = CWConfiguration.NewCarConsumerModuleName,
                        Payload = Serializer.ConvertProtobufMsgToBytes(message)
                    }, _queueDelay);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Faliure in updating prices in ElasticCarData");
            }
        }
        public ISearchResponse<CarBaseEntity> GetBodyTypes(NewCarSearchInputs inputs)
        {
            var query = GetQueryDescriptor<CarBaseEntity>(inputs, false, true);
            var aggQuery = GetAggregationContainerDescriptor(false);
            ISearchResponse<CarBaseEntity> elasticResponse = TypeElasticQuery<CarBaseEntity>(query, ncfIndex, ncfDocType, aggQuery);
            return elasticResponse;
        }
        public ISearchResponse<T> TypeElasticQuery<T>(QueryContainer queryInputs, string indexName, string docType, AggregationContainerDescriptor<CarBaseEntity> aggQuery) where T : CarBaseEntity
        {
            if (queryInputs != null && indexName != null && docType != null)
            {
                try
                {
                    ElasticClient clientElastic = ElasticClientInstance.GetInstance();
                    ISearchResponse<T> elasticReponse = null;
                    elasticReponse = clientElastic.Search<T>(s => s
                                          .Index(indexName)
                                          .Take(takecount)
                                          .Type(docType)
                                          .Query(q => queryInputs)
                                          .Source(sf => sf
                                           .Excludes(e => e
                                                .Fields("*")
                                            ))
                                          .Aggregations(q => aggQuery)
                                          );

                    return elasticReponse;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Error in fethching elastic data");
                }
            }
            return null;
        }

        public ISearchResponse<CarBaseEntity> GetFuelTypes(NewCarSearchInputs inputs)
        {
            var query = GetQueryDescriptor<CarBaseEntity>(inputs, false, true);
            var aggQuery = GetAggregationContainerDescriptor(true);
            ISearchResponse<CarBaseEntity> elasticResponse = TypeElasticQuery<CarBaseEntity>(query, ncfIndex, ncfDocType, aggQuery);
            return elasticResponse;
        }
        public static AggregationContainerDescriptor<CarBaseEntity> GetAggregationContainerDescriptor(bool isFuelType = true)
        {
            string type = isFuelType ? "fuelType" : "bodyStyle";
            var acd = new AggregationContainerDescriptor<CarBaseEntity>();
            acd.Terms("bucket", tm => tm.Field(type).Aggregations(x => x.Cardinality("models", cc => cc.Field(p => p.ModelId))));
            return acd;
        }
        public static SortDescriptor<CarBaseEntity> GetSortDescriptor(NewCarSearchInputs input, bool isNcf = false)
        {
            SortDescriptor<CarBaseEntity> sortDescriptor = new SortDescriptor<CarBaseEntity>();
            if (!input.IsBodyTypeByBudgetPage && input.cityId > 0 && isNcf)
            {
                sortDescriptor.Field(x => x.Field("onRoadCityPrice.price").Order(SortOrder.Ascending).NestedPath("onRoadCityPrice").NestedFilter(y => y.Term("onRoadCityPrice.cityId", input.cityId)));
            }
            else
            {
                sortDescriptor.Field(f => f.Field(input.sortField1.Item1).Order(input.sortField1.Item2))
                                                            .Field(f => f.Field(input.sortField2.Item1).Order(input.sortField2.Item2));
            }
            return sortDescriptor;
        }
        public NewCarSearchInputs GetElasticInputs(NameValueCollection queryString)
        {
            string str_comma = ",";
            char c_comma = ',';
            NewCarSearchInputs inputs = new NewCarSearchInputs();

            if (RegExValidations.IsPositiveNumber(queryString["pageNo"])) inputs.pageNo = Convert.ToInt32(queryString["pageNo"]);
            else inputs.pageNo = 1;
            if (RegExValidations.IsPositiveNumber(queryString["pageSize"])) inputs.pageSize = Convert.ToInt32(queryString["pageSize"]);
            else inputs.pageSize = 10;
            if (queryString["carMakeIds"] != null) { queryString["carMakeIds"] = queryString["carMakeIds"].Trim(); }
            if (RegExValidations.ValidateCommaSeperatedNumbers(queryString["carMakeIds"])) inputs.makes = queryString["carMakeIds"].Split(c_comma).Select(x => x.Trim()).ToArray();
            if (RegExValidations.ValidateCommaSeperatedNumbers(queryString["modelId"])) inputs.ModelIds = queryString["modelId"].Split(c_comma).Select(int.Parse).ToList();
            if (RegExValidations.IsPositiveNumber(queryString["cityId"])) inputs.cityId = Convert.ToInt32(queryString["cityId"]);
            if (RegExValidations.ValidateNumericRange(queryString["budget"], str_comma))
            {
                var budgetRange = queryString["budget"].Split(c_comma);
                int lowerLimit = 0;
                int upperLimit = 0;
                int.TryParse(budgetRange[0], out lowerLimit);
                int.TryParse(budgetRange[1], out upperLimit);
                inputs.budgets.Add(new RangeLimit() { LowerLimit = lowerLimit, UpperLimit = upperLimit });
            }
            else if (RegExValidations.IsPositiveNumber(queryString["budget"]))
            {
                int budget = 0;
                int.TryParse(queryString["budget"], out budget);
                if (budget > 0)
                {
                    var lowerLimit = Convert.ToInt32(Math.Round((budget - budget * 0.2)));
                    var upperLimit = Convert.ToInt32(Math.Round((budget + budget * 0.2)));
                    if (lowerLimit >= 0 && upperLimit > 0)
                    {
                        inputs.budgets.Add(new RangeLimit() { LowerLimit = lowerLimit, UpperLimit = upperLimit });
                    }
                }
            }
            if (RegExValidations.ValidateCommaSeperatedNumbers(queryString["fuelTypeIds"])) inputs.fueltype = queryString["fuelTypeIds"].Split(c_comma);
            if (RegExValidations.ValidateCommaSeperatedNumbers(queryString["transmissionTypeIds"])) inputs.transmission = queryString["transmissionTypeIds"].Split(c_comma);
            if (RegExValidations.ValidateCommaSeperatedNumbers(queryString["bodyStyleIds"]))
            {
                inputs.bodytype = queryString["bodyStyleIds"].Split(c_comma);
                if (inputs.bodytype.Contains("6") && !inputs.bodytype.Contains("9"))
                {
                    var temp = inputs.bodytype.ToList();
                    temp.Add("9");
                    inputs.bodytype = temp.ToArray();
                }
            }
            if (RegExValidations.ValidateCommaSeperatedNumbers(queryString["modelIds"]))
            {
                inputs.ModelIds = queryString["modelIds"].Split(c_comma).Select(Int32.Parse).ToList();
            }
            if (RegExValidations.ValidateCommaSeperatedNumbers(queryString["removedModelIds"]))
            {
                inputs.ExcludedModelIds = queryString["removedModelIds"].Split(c_comma).Select(Int32.Parse).ToList();
            }
            if (RegExValidations.ValidateNumericRange(queryString["seatingCapacity"], str_comma))
            {
                var seatingRange = queryString["seatingCapacity"].Split(c_comma);
                inputs.seatingCapacity.Add(new RangeLimit() { LowerLimit = Convert.ToInt32(seatingRange[0]), UpperLimit = Convert.ToInt32(seatingRange[1]) });
            }
            if (RegExValidations.ValidateCommaSeperatedNumbers(queryString["seats"]))
            {
                var seats = queryString["seats"].Split(c_comma);
                foreach (var seat in seats)
                {
                    inputs.seatingCapacity.Add(new RangeLimit() { LowerLimit = Convert.ToInt32(seat), UpperLimit = Convert.ToInt32(seat) >= 9 ? 1000 : Convert.ToInt32(seat) });
                }
            }
            if (RegExValidations.ValidateNumericRange(queryString["enginePower"], str_comma))
            {
                var engineRange = queryString["enginePower"].Split(c_comma);
                inputs.enginePower.Add(new RangeLimit() { LowerLimit = Convert.ToInt32(engineRange[0]), UpperLimit = Convert.ToInt32(engineRange[1]) });
            }
            if (!string.IsNullOrWhiteSpace(queryString["emi"]) && RegExValidations.ValidateNumericRange(queryString["emi"], str_comma))
            {
                var emiRange = queryString["emi"].Split(c_comma);
                inputs.EMI.LowerLimit = Convert.ToInt32(emiRange[0]);
                inputs.EMI.UpperLimit = Convert.ToInt32(emiRange[1]);
                inputs.useEMI = true;
            }
            if (!string.IsNullOrWhiteSpace(queryString["countOnly"]) && queryString["countOnly"].ToLower() == "true")
            {
                inputs.CountsOnly = true;
            }
            string sortOrder1 = "asc";
            string sortOrder2 = "asc";
            string sortField1 = NewCarSearchInputs.defaultSortField1;
            string sortField2 = NewCarSearchInputs.defaultSortField2;
            if (!string.IsNullOrWhiteSpace(queryString["sortOrder1"])) sortOrder1 = queryString["sortOrder1"];
            if (!string.IsNullOrWhiteSpace(queryString["sortOrder2"])) sortOrder2 = queryString["sortOrder2"];
            if (!string.IsNullOrWhiteSpace(queryString["sortField1"])) sortField1 = queryString["sortField1"];
            if (!string.IsNullOrWhiteSpace(queryString["sortField2"])) sortField2 = queryString["sortField2"];
            if (sortOrder1 == "desc") inputs.sortField1 = new Tuple<Field, SortOrder>(new Field(sortField1), SortOrder.Descending);
            else inputs.sortField1 = new Tuple<Field, SortOrder>(new Field(sortField1), SortOrder.Ascending);
            if (sortOrder2 == "desc") inputs.sortField2 = new Tuple<Field, SortOrder>(new Field(sortField2), SortOrder.Descending);
            else inputs.sortField2 = new Tuple<Field, SortOrder>(new Field(sortField2), SortOrder.Ascending);
            return inputs;
        }


        public List<ElasticModel> GetModelList(NewCarSearchInputs inputs)
        {
            List<ElasticModel> modelList = new List<ElasticModel>();
            try
            {
                var query = GetQueryDescriptor<NewCarFinderEntity>(inputs, false, true);
                ISearchResponse<NewCarFinderEntity> response = ElasticQuery<NewCarFinderEntity>(inputs, query, ncfIndex, ncfDocType, true, true);
                for (int index = 0; index < response.Documents.Count; index++)
                {
                    var document = response.Documents.ElementAt(index);
                    modelList.Add(new ElasticModel
                    {
                        Id = document.ModelId,
                        MakeId = document.MakeId,
                        ModelPopularity = document.ModelPopularity,
                        MakeName = document.MakeName
                    });
                }
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is ArgumentNullException || ex is IndexOutOfRangeException || ex is InvalidCastException)
            {
                Logger.LogException(ex);
            }
            return modelList;
        }
    }
}
