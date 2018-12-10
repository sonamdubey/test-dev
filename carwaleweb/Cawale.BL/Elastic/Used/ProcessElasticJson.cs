using Carwale.DTOs.Classified.Stock;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Enum;
using Carwale.Entity.Elastic;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carwale.BL.Elastic
{
    public class ProcessElasticJson : IProcessElasticJson
    {
        /// <summary>
        /// Class Added By Jugal for Converting ElasticSearch JSON into our own JSON.|| Added By Jugal on 16 Nov 2014
        /// This method will parse each and every aggregation (even Nested Aggs) And will assign their respective counts to filters.
        /// </summary>
        /// 

        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepository;
        private static readonly string CarTradeCertificationId = ConfigurationManager.AppSettings["CartradeCertificationId"];

        public ProcessElasticJson(IGeoCitiesCacheRepository geoCitiesCacheRepository)
        {
            _geoCitiesCacheRepository = geoCitiesCacheRepository;
        }

        public CountData ConvertAggregationsToJson(ISearchResponse<StockBaseEntity> filters, ElasticOuptputs filterInputs)
        {
            CountData AllFiltersCount = null;
            try
            {
                ResultsFiltersPagerDesktop usedElasticDTO = new ResultsFiltersPagerDesktop();

                usedElasticDTO.FiltersData = new CountData();

                // Fetching TotalStockCounts
                usedElasticDTO.FiltersData.StockCount = new StockCount();
                SingleBucketAggregate totalStock = filters.Aggs.Filter("TotalStockCount");
                usedElasticDTO.FiltersData.StockCount.TotalStockCount = Convert.ToInt32(totalStock.DocCount);

                // For MumbaiAround and Delhi AroundCities  Count
                usedElasticDTO.FiltersData.CityCount = new List<City>();

                SingleBucketAggregate MumbaiAroundCount = filters.Aggs.Filter("MumbaiAroundCities");
                SingleBucketAggregate DelhiAroundCount = filters.Aggs.Filter("DelhiAroundCities");
                SingleBucketAggregate allIndia = filters.Aggs.Filter("AllIndia");

                City AroundCity = new City();

                AroundCity.CityId = 3000;
                AroundCity.CityCount = Convert.ToInt32(MumbaiAroundCount.DocCount);
                AroundCity.CityName = "Mumbai All";
                usedElasticDTO.FiltersData.CityCount.Add(AroundCity);

                AroundCity = new City();
                AroundCity.CityId = 3001;
                AroundCity.CityCount = Convert.ToInt32(DelhiAroundCount.DocCount);
                AroundCity.CityName = "Delhi NCR";
                usedElasticDTO.FiltersData.CityCount.Add(AroundCity);

                AroundCity = new City();
                AroundCity.CityId = 9999;
                AroundCity.CityCount = Convert.ToInt32(allIndia.DocCount);
                AroundCity.CityName = "All India";
                usedElasticDTO.FiltersData.CityCount.Add(AroundCity);

                // Fetching MakeCounts And RootCounts if available
                usedElasticDTO.FiltersData.MakeCount = new List<StockMake>();

                List<StockMake> objAllMake = new List<StockMake>();

                SingleBucketAggregate allMake = filters.Aggs.Filter("AllMakeCount");
                BucketAggregate allMakeIdBucket = (Nest.BucketAggregate)allMake.Aggregations["AllMakeIdCount"];
                var allMakeItems = allMakeIdBucket.Items;

                foreach (KeyedBucket<object> x in allMakeItems)
                {
                    StockMake objMake = new StockMake();
                    string[] makeIds = x.Key.ToString().Split('~');

                    objMake.MakeId = Convert.ToInt32(makeIds[0]);
                    objMake.MakeName = makeIds[1];
                    objMake.MakeCount = 0;

                    SingleBucketAggregate roots = (SingleBucketAggregate)x.Aggregations["AllRoot_Count"];
                    var root1 = ((BucketAggregate)roots.Aggregations["AllRootIdCount"]).Items;

                    //Creating Object to prevent failure in android app
                    List<StockRoot> rootObj = new List<StockRoot>();
                    if (filterInputs.cars != null)
                    {
                        foreach (KeyedBucket<object> y in root1) // For Root Counts
                        {
                            string[] rootIds = ((string)y.Key).Split('~');
                            rootObj.Add(new StockRoot()
                            {
                                RootId = Convert.ToInt32(rootIds[0]),
                                RootName = rootIds[1],
                                RootCount = 0,
                                isSuperLuxury = Convert.ToBoolean(rootIds[3])
                            });

                        }
                        objMake.RootList = rootObj;
                    }
                    else
                        objMake.RootList = rootObj;

                    objAllMake.Add(objMake);
                }


                SingleBucketAggregate make = filters.Aggs.Filter("MakeCount");
                BucketAggregate makeIdBucket = (Nest.BucketAggregate)make.Aggregations["MakeIdCount"];
                var makeItems = makeIdBucket.Items;

                List<StockMake> objMakeCount = new List<StockMake>();

                foreach (KeyedBucket<object> x in makeItems) // For Make Counts
                {
                    StockMake makeObj = new StockMake();

                    string[] makeIds = x.Key.ToString().Split('~');

                    makeObj.MakeId = Convert.ToInt32(makeIds[0]);
                    makeObj.MakeName = makeIds[1];
                    makeObj.MakeCount = Convert.ToInt32(x.DocCount);

                    SingleBucketAggregate roots = (SingleBucketAggregate)x.Aggregations["Root_Count"];
                    var root1 = ((BucketAggregate)roots.Aggregations["RootIdCount"]).Items;

                    //Creating object to prevent failure in android app
                    List<StockRoot> rootObj = new List<StockRoot>();
                    if (filterInputs.cars != null)
                    {
                        foreach (KeyedBucket<object> y in root1) // For Root Counts
                        {
                            string[] rootIds = ((string)y.Key).Split('~');
                            rootObj.Add(new StockRoot()
                            {
                                RootId = Convert.ToInt32(rootIds[0]),
                                RootName = rootIds[1],
                                RootCount = Convert.ToInt32(y.DocCount),
                                isSuperLuxury = Convert.ToBoolean(rootIds[3])
                            });

                        }
                        makeObj.RootList = rootObj;
                    }
                    else
                        makeObj.RootList = rootObj;

                    objMakeCount.Add(makeObj);
                }

                foreach (var allMakes in objAllMake)
                {
                    foreach (var filteredMake in objMakeCount)
                    {
                        if (allMakes.MakeId == filteredMake.MakeId)
                        {
                            allMakes.MakeCount = filteredMake.MakeCount;

                            if (allMakes.RootList != null && allMakes.RootList.Count > 0)
                            {
                                foreach (var allRoots in allMakes.RootList)
                                {
                                    foreach (var filteredRoot in filteredMake.RootList)
                                    {
                                        if (allRoots.RootId == filteredRoot.RootId)
                                            allRoots.RootCount = filteredRoot.RootCount;
                                    }
                                }
                            }
                        }
                    }
                }

                usedElasticDTO.FiltersData.MakeCount = objAllMake;
                // For City Counts
                var isFilterCityExist = false;
                SingleBucketAggregate city = filters.Aggs.Filter("CityCount");
                var cityItems = city.Terms("CityIdCount").Buckets;

                var tempCityList = new List<City>();

                foreach (var cityItem in cityItems)
                {
                    string[] cityIds = cityItem.Key?.ToString()?.Split('~');
                    if (cityIds != null && cityIds.Length > 1)
                    {
                        City cityObj = new City
                        {
                            CityId = Convert.ToInt32(cityIds[0]),
                            CityName = cityIds[1].ToString(),
                            CityCount = Convert.ToInt32(cityItem.DocCount)
                        };
                        tempCityList.Add(cityObj);
                        if(filterInputs.cities == null || filterInputs.cities[0] == cityIds[0]) // filterInputs.cities == null for 'ALL INDIA' case, in all india 
                        {                                                                       // we don't have to include city in list, so making isFilterCityExist = true 
                            isFilterCityExist = true;
                        }
                    }
                }
                if(!isFilterCityExist && filterInputs.cities != null && filterInputs.cities.Length > 0)  //if no cars found in currCity then upper result does
                {                                                                                        //not contain filter city object, So explicitaly adding 
                    var cityObj = GetCityObj(filterInputs.cities[0]);                                    //filter city obj with count 0.
                    if (cityObj != null)
                    {
                        tempCityList.Add(cityObj);
                    }
                }
                //Should LINQ be avoided because it's slow:- https://stackoverflow.com/questions/3769989/should-linq-be-avoided-because-its-slow
                usedElasticDTO.FiltersData.CityCount.AddRange(tempCityList.OrderBy(x => x.CityName));

                // For Seller Counts
                SingleBucketAggregate seller = filters.Aggs.Filter("SellerCount");
                BucketAggregate sellerCount = (Nest.BucketAggregate)seller.Aggregations["SellerIdCount"];
                var sellerItems = sellerCount.Items;

                usedElasticDTO.FiltersData.SellerTypeCount = new SellerType();

                foreach (KeyedBucket<object> s1 in sellerItems)
                {
                    if ((long)s1.Key == 1)
                        usedElasticDTO.FiltersData.SellerTypeCount.Dealer = Convert.ToInt32(s1.DocCount);

                    if ((long)s1.Key == 2)
                        usedElasticDTO.FiltersData.SellerTypeCount.Individual = Convert.ToInt32(s1.DocCount);

                }

                //For Area Counts
                //SingleBucket area = filters.Aggs.Filter("AreaCount");
                //Bucket areaCount = (Nest.Bucket)area.Aggregations["AreaIdCount"];
                //var areaItems = areaCount.Items;

                //usedElasticDTO.FiltersData.AreaCount = new List<Area>();

                //foreach (KeyItem a1 in areaItems)
                //{
                //    Area areaObj = new Area();
                //    areaObj.AreaId = Convert.ToInt32(a1.Key);
                //    areaObj.AreaCount = Convert.ToInt32(a1.DocCount);
                //    usedElasticDTO.FiltersData.AreaCount.Add(areaObj);
                //}

                // For BodyType Counts
                SingleBucketAggregate bodyType = filters.Aggs.Filter("BodyTypeCount");
                BucketAggregate bodyTypeCount = (Nest.BucketAggregate)bodyType.Aggregations["BodyIdCount"];
                var bodyItems = bodyTypeCount.Items;

                usedElasticDTO.FiltersData.BodyTypeCount = new BodyType();

                foreach (KeyedBucket<object> b1 in bodyItems)
                {
                    if ((string)b1.Key == "Hatchback")
                        usedElasticDTO.FiltersData.BodyTypeCount.Hatchback = Convert.ToInt32(b1.DocCount);

                    if ((string)b1.Key == "Sedan")
                        usedElasticDTO.FiltersData.BodyTypeCount.Sedan += Convert.ToInt32(b1.DocCount);

                    //Adding count of sedan and compact sedan
                    if ((string)b1.Key == "Compact Sedan")
                        usedElasticDTO.FiltersData.BodyTypeCount.Sedan += Convert.ToInt32(b1.DocCount);

                    if ((string)b1.Key == "Minivan/Van")
                        usedElasticDTO.FiltersData.BodyTypeCount.Minivan = Convert.ToInt32(b1.DocCount);

                    if ((string)b1.Key == "SUV/MUV")
                        usedElasticDTO.FiltersData.BodyTypeCount.Suv = Convert.ToInt32(b1.DocCount);

                    if ((string)b1.Key == "Coupe")
                        usedElasticDTO.FiltersData.BodyTypeCount.Coupe = Convert.ToInt32(b1.DocCount);

                    if ((string)b1.Key == "Station Wagon")
                        usedElasticDTO.FiltersData.BodyTypeCount.StationWagon = Convert.ToInt32(b1.DocCount);

                    if ((string)b1.Key == "Truck")
                        usedElasticDTO.FiltersData.BodyTypeCount.Truck = Convert.ToInt32(b1.DocCount);

                    if ((string)b1.Key == "Convertible")
                        usedElasticDTO.FiltersData.BodyTypeCount.Convertible = Convert.ToInt32(b1.DocCount);
                }

                // For FuelType Counts
                SingleBucketAggregate fuelType = filters.Aggs.Filter("FuelTypeCount");
                BucketAggregate fuelTypeCount = (Nest.BucketAggregate)fuelType.Aggregations["FuelIdCount"];
                var fuelItems = fuelTypeCount.Items;

                usedElasticDTO.FiltersData.FuelTypeCount = new FuelType();

                foreach (KeyedBucket<object> f1 in fuelItems)
                {
                    if ((string)f1.Key == "Petrol")
                        usedElasticDTO.FiltersData.FuelTypeCount.Petrol = Convert.ToInt32(f1.DocCount);

                    if ((string)f1.Key == "Diesel")
                        usedElasticDTO.FiltersData.FuelTypeCount.Diesel = Convert.ToInt32(f1.DocCount);

                    if ((string)f1.Key == "CNG")
                        usedElasticDTO.FiltersData.FuelTypeCount.CNG = Convert.ToInt32(f1.DocCount);

                    if ((string)f1.Key == "LPG")
                        usedElasticDTO.FiltersData.FuelTypeCount.LPG = Convert.ToInt32(f1.DocCount);

                    if ((string)f1.Key == "Electric")
                        usedElasticDTO.FiltersData.FuelTypeCount.Electric = Convert.ToInt32(f1.DocCount);

                    if ((string)f1.Key == "Hybrid")
                        usedElasticDTO.FiltersData.FuelTypeCount.Hybrid = Convert.ToInt32(f1.DocCount);
                }

                // For Transmission Counts
                SingleBucketAggregate transmission = filters.Aggs.Filter("TransmissionCount");
                BucketAggregate transmissionCount = (Nest.BucketAggregate)transmission.Aggregations["TransmissionIdCount"];
                var transmissionItems = transmissionCount.Items;

                usedElasticDTO.FiltersData.TransmissionTypeCount = new Transmission();

                foreach (KeyedBucket<object> t1 in transmissionItems)
                {
                    if ((string)t1.Key == "Automatic")
                        usedElasticDTO.FiltersData.TransmissionTypeCount.Automatic = Convert.ToInt32(t1.DocCount);

                    if ((string)t1.Key == "Manual")
                        usedElasticDTO.FiltersData.TransmissionTypeCount.Manual = Convert.ToInt32(t1.DocCount);

                }

                // For Color Counts
                SingleBucketAggregate color = filters.Aggs.Filter("ColorCount");
                BucketAggregate colorCount = (Nest.BucketAggregate)color.Aggregations["ColorIdCount"];
                var colorItems = colorCount.Items;

                usedElasticDTO.FiltersData.ColorTypeCount = new AvailableColors();

                foreach (KeyedBucket<object> c1 in colorItems)
                {
                    switch (Convert.ToInt32((double)c1.Key))
                    {
                        case 1:
                            usedElasticDTO.FiltersData.ColorTypeCount.Beige = Convert.ToInt32(c1.DocCount);
                            break;

                        case 2:
                            usedElasticDTO.FiltersData.ColorTypeCount.Black = Convert.ToInt32(c1.DocCount);
                            break;

                        case 3:
                            usedElasticDTO.FiltersData.ColorTypeCount.Blue = Convert.ToInt32(c1.DocCount);
                            break;

                        case 4:
                            usedElasticDTO.FiltersData.ColorTypeCount.Brown = Convert.ToInt32(c1.DocCount);
                            break;

                        case 5:
                            usedElasticDTO.FiltersData.ColorTypeCount.GoldNYellow = Convert.ToInt32(c1.DocCount);
                            break;

                        case 6:
                            usedElasticDTO.FiltersData.ColorTypeCount.Green = Convert.ToInt32(c1.DocCount);
                            break;

                        case 7:
                            usedElasticDTO.FiltersData.ColorTypeCount.Grey = Convert.ToInt32(c1.DocCount);
                            break;

                        case 8:
                            usedElasticDTO.FiltersData.ColorTypeCount.Maroon = Convert.ToInt32(c1.DocCount);
                            break;

                        case 9:
                            usedElasticDTO.FiltersData.ColorTypeCount.Purple = Convert.ToInt32(c1.DocCount);
                            break;

                        case 10:
                            usedElasticDTO.FiltersData.ColorTypeCount.Red = Convert.ToInt32(c1.DocCount);
                            break;

                        case 11:
                            usedElasticDTO.FiltersData.ColorTypeCount.Silver = Convert.ToInt32(c1.DocCount);
                            break;

                        case 12:
                            usedElasticDTO.FiltersData.ColorTypeCount.White = Convert.ToInt32(c1.DocCount);
                            break;

                        default:
                            usedElasticDTO.FiltersData.ColorTypeCount.Others += Convert.ToInt32(c1.DocCount);
                            break;
                    }
                }

                //For OwnersType Count
                SingleBucketAggregate owners = filters.Aggs.Filter("OwnersCount");
                BucketAggregate ownersCount = (Nest.BucketAggregate)owners.Aggregations["OwnerTypeCount"];
                var ownersItems = ownersCount.Items;

                usedElasticDTO.FiltersData.OwnersTypeCount = new Owners();

                int third = 0, fourth = 0, moreThanFour1 = 0, moreThanFour2 = 0, NA = 0;
                foreach (KeyedBucket<object> o1 in ownersItems)
                {
                    if ((string)o1.Key == "First Owner ")
                        usedElasticDTO.FiltersData.OwnersTypeCount.First = Convert.ToInt32(o1.DocCount);

                    if ((string)o1.Key == "Second Owner ")
                        usedElasticDTO.FiltersData.OwnersTypeCount.Second = Convert.ToInt32(o1.DocCount);

                    if ((string)o1.Key == "Third Owner ")
                        third = Convert.ToInt32(o1.DocCount);

                    if ((string)o1.Key == "Fourth Owner")
                        fourth = Convert.ToInt32(o1.DocCount);

                    if (((string)o1.Key).Contains("More Than 4 Owners"))
                        moreThanFour1 = Convert.ToInt32(o1.DocCount);

                    if (((string)o1.Key).Contains("More than 4 owners"))
                        moreThanFour2 = Convert.ToInt32(o1.DocCount);

                    if ((string)o1.Key == "N/A")
                        NA = Convert.ToInt32(o1.DocCount);

                    if ((string)o1.Key == "UnRegistered Car")
                        usedElasticDTO.FiltersData.OwnersTypeCount.Unregistered = Convert.ToInt32(o1.DocCount);

                }

                usedElasticDTO.FiltersData.OwnersTypeCount.ThirdAndAbove = third + fourth + moreThanFour1 + moreThanFour2 + NA;

                //For CarsWithPhotos
                SingleBucketAggregate carsWithPhotos = filters.Aggs.Filter("FilterBy2");
                BucketAggregate carsWithPhotosCount = (Nest.BucketAggregate)carsWithPhotos.Aggregations["CarsWithPhotosCount"];
                var carsWithPhotosItems = carsWithPhotosCount.Items;

                //FilterBy Counts for CarTradeCertifiedCars
                SingleBucketAggregate carTradeCertifiedCars = filters.Aggs.Filter("FilterBy1");
                BucketAggregate carTradeCertifiedCarsCount = (Nest.BucketAggregate)carTradeCertifiedCars.Aggregations["CarTradeCertifiedCarsCount"];
                var carTradeCertifiedCarItems = carTradeCertifiedCarsCount.Items;

                usedElasticDTO.FiltersData.FilterTypeCount = new FilterBy();
                usedElasticDTO.FiltersData.FilterTypeAdditional = new FilterByAdditional();

                foreach (KeyedBucket<object> fb2 in carsWithPhotosItems)
                {
                    if (Convert.ToInt32((double)fb2.Key) == 1)
                    {
                        usedElasticDTO.FiltersData.FilterTypeCount.CarsWithPhotos = Convert.ToInt32(fb2.DocCount);
                        usedElasticDTO.FiltersData.FilterTypeAdditional.CarsWithPhotos = Convert.ToInt32(fb2.DocCount);
                    }
                }

                foreach (KeyedBucket<object> fb2 in carTradeCertifiedCarItems)
                {

                    if ((string)fb2.Key == CarTradeCertificationId)
                    {
                        usedElasticDTO.FiltersData.FilterTypeAdditional.CarTradeCertifiedCars = Convert.ToInt32(fb2.DocCount);
                    }
                }

                //FilterBy Counts for FranchiseCars
                SingleBucketAggregate franchiseCars = filters.Aggs.Filter("FilterBy3");
                usedElasticDTO.FiltersData.FilterTypeCount.FranchiseCars = Convert.ToInt32(franchiseCars.DocCount);

                //Assigning all counts
                AllFiltersCount = new CountData()
                {
                    MakeCount = usedElasticDTO.FiltersData.MakeCount,
                    CityCount = usedElasticDTO.FiltersData.CityCount,
                    FuelTypeCount = usedElasticDTO.FiltersData.FuelTypeCount,
                    BodyTypeCount = usedElasticDTO.FiltersData.BodyTypeCount,
                    TransmissionTypeCount = usedElasticDTO.FiltersData.TransmissionTypeCount,
                    OwnersTypeCount = usedElasticDTO.FiltersData.OwnersTypeCount,
                    SellerTypeCount = usedElasticDTO.FiltersData.SellerTypeCount,
                    FilterTypeCount = usedElasticDTO.FiltersData.FilterTypeCount,
                    FilterTypeAdditional = usedElasticDTO.FiltersData.FilterTypeAdditional,
                    ColorTypeCount = usedElasticDTO.FiltersData.ColorTypeCount,
                    //AreaCount = usedElasticDTO.FiltersData.AreaCount,
                    StockCount = usedElasticDTO.FiltersData.StockCount,
                };
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : BindListingsData");
                objErr.SendMail();
            }
            return AllFiltersCount;
        }

        public Tuple<List<StockBaseEntity>, int, int, int, int> TakeTopResults(List<SearchResponse<StockBaseEntity>> onlyResults, ElasticOuptputs filterInputs, int totalFeaturedSlotCount, int PageSize, bool isRequestFromNearbyCity, int totalDiamondSlotCount)
        {
            int nonPremiumCount = 0;
            int nonPremiumListingsIndex = 0;
            int premiumListingsIndex = onlyResults.Count > 1 ? 1 : -1;
            int diamondDealerListingsIndex = onlyResults.Count > 2 ? 2 : -1;
            int premiumDealerListingCount = 0;

            List<StockBaseEntity> resultListings = new List<StockBaseEntity>(); // For listing results
            List<StockBaseEntity> nonPremiumListings = (List<StockBaseEntity>)onlyResults[nonPremiumListingsIndex].Documents;

            int totalStockCount = 0;
            if (!isRequestFromNearbyCity)
            {
                totalStockCount = (int)onlyResults[nonPremiumListingsIndex].Total;
            }

            if (nonPremiumListings.Count > 0)
            {
                AddNonPremiumFlagToListings(nonPremiumListings);
                //Add non premium listings in TakeTopResults
                if (string.IsNullOrEmpty(filterInputs.sc) && premiumListingsIndex != -1) // If sorting is not applied and we have some featured stock .
                {
                    if (filterInputs.ShouldFetchNearbyCars)
                    {
                        //assign bucket to non premium
                        for (int i = 0; i < nonPremiumListings.Count; i++)
                        {
                            nonPremiumListings[i].NearbyCarsBucket = (NearbyCarsBucket)Convert.ToInt32(onlyResults[nonPremiumListingsIndex].HitsMetaData.Hits.ElementAt(i).Sorts.FirstOrDefault());
                        }
                        NearbyCarsBucket nonPremiumStockBucket = nonPremiumListings.FirstOrDefault().NearbyCarsBucket;
                        AddNearbyCarsFeaturedListings(onlyResults, filterInputs, totalFeaturedSlotCount, totalDiamondSlotCount, 
                                                      premiumListingsIndex, diamondDealerListingsIndex, resultListings, nonPremiumStockBucket);
                    }
                    else
                    {
                        AddFeaturedListings(onlyResults, filterInputs, totalFeaturedSlotCount, totalDiamondSlotCount, premiumListingsIndex, diamondDealerListingsIndex, resultListings);
                    }

                    premiumDealerListingCount = resultListings.Count;
                    nonPremiumCount = PageSize - resultListings.Count;
                    if (nonPremiumListings.Count < nonPremiumCount)
                    {
                        AddToResultListings(nonPremiumListings, resultListings, filterInputs);
                    }
                    else
                    {
                        if (nonPremiumCount > 0) //If pagesize less than number of premium listing fetched, nonpremium count would go -ve, elastic would throw exception
                        {
                            AddToResultListings(nonPremiumListings.GetRange(0, nonPremiumCount), resultListings, filterInputs);
                        }
                    }
                }
                else // If Sorting is applied . SortCriteria is not null.
                {
                    //assign bucket to non premium
                    for (int i = 0; filterInputs.ShouldFetchNearbyCars && i < nonPremiumListings.Count; i++)
                    {
                        nonPremiumListings[i].NearbyCarsBucket = (NearbyCarsBucket)Convert.ToInt32(onlyResults[nonPremiumListingsIndex].HitsMetaData.Hits.ElementAt(i).Sorts.FirstOrDefault());
                    }
                    AddToResultListings(nonPremiumListings, resultListings, filterInputs);
                }

                AddMultiCityDealerDeliveryText(resultListings, filterInputs);
                FillIsPremiumPackageProperty(resultListings);
            }
            int numberNonPremiumListings = resultListings.Count - premiumDealerListingCount;
            return new Tuple<List<StockBaseEntity>, int, int, int, int>(resultListings, numberNonPremiumListings, premiumDealerListingCount, 0 /* Premium Individual Listings Count */ , totalStockCount);
        }

        private void AddFeaturedListings(List<SearchResponse<StockBaseEntity>> onlyResults, ElasticOuptputs filterInputs, int totalFeaturedSlotCount, int totalDiamondSlotCount,
                    int premiumListingsIndex, int diamondDealerListingsIndex, List<StockBaseEntity> resultListings)
        {
            if (diamondDealerListingsIndex != -1)
            {
                List<StockBaseEntity> diamondDealerListings = GetPremiumDealerListings(onlyResults[diamondDealerListingsIndex], totalDiamondSlotCount, resultListings);
                if (diamondDealerListings != null && diamondDealerListings.Count > 0)
                {
                    AddToResultListings(diamondDealerListings, resultListings, filterInputs);
                }
            }
            int nonDiamondSlotCount = totalFeaturedSlotCount - resultListings.Count;
            List<StockBaseEntity> premiumDealerListings = GetPremiumDealerListings(onlyResults[premiumListingsIndex], nonDiamondSlotCount, resultListings);
            if (premiumDealerListings != null && premiumDealerListings.Count > 0)
            {
                AddToResultListings(premiumDealerListings, resultListings, filterInputs);
            }
        }

        private void AddNearbyCarsFeaturedListings(List<SearchResponse<StockBaseEntity>> onlyResults, ElasticOuptputs filterInputs, int totalFeaturedSlotCount, int totalDiamondSlotCount, int premiumListingsIndex, int diamondDealerListingsIndex, List<StockBaseEntity> resultListings, NearbyCarsBucket bucket)
        {
            // Get diamond and premium listings List with enum bucket value set
            List<StockBaseEntity> diamondListings = (diamondDealerListingsIndex == -1) 
                                                  ? null 
                                                  : GetPremiumDealerListings(onlyResults[diamondDealerListingsIndex], totalDiamondSlotCount, resultListings, filterInputs.ShouldFetchNearbyCars);

            AddListingsForBucket(bucket, filterInputs, diamondListings, resultListings);

            int nonDiamondSlotCount = totalFeaturedSlotCount - resultListings.Count;

            List<StockBaseEntity> platinumListings = (premiumListingsIndex == -1) 
                                                   ? null 
                                                   : GetPremiumDealerListings(onlyResults[premiumListingsIndex], nonDiamondSlotCount, resultListings, filterInputs.ShouldFetchNearbyCars);
            AddListingsForBucket(bucket, filterInputs, platinumListings, resultListings);
        }

        private void AddListingsForBucket(NearbyCarsBucket nearbyCarsBucket, ElasticOuptputs filterInputs, List<StockBaseEntity> listings, List<StockBaseEntity> resultListings)
        {
            // Add all the stocks of input bucket in resultListing

            if (listings == null)
            {
                return;
            }

            List<StockBaseEntity> bucketListings = new List<StockBaseEntity>();
            foreach (var item in listings)
            {
                if (item.NearbyCarsBucket == nearbyCarsBucket)
                {
                    bucketListings.Add(item);
                }
            }
            AddToResultListings(bucketListings, resultListings, filterInputs);
        }

        private static List<StockBaseEntity> GetPremiumDealerListings(ISearchResponse<StockBaseEntity> searchResponse, int featuredSlotCount, List<StockBaseEntity> resultlistings, bool shouldFetchNearbyCars = false)
        {
            List<StockBaseEntity> premiumDealerStocks = new List<StockBaseEntity>();
            try
            {
                var dealerBucket = searchResponse.Aggs.Terms("dealers");
                List<List<StockBaseEntity>> stocksLists = new List<List<StockBaseEntity>>();

                foreach (var bucket in dealerBucket.Buckets)
                {
                    var stockList = bucket.TopHits("stocks").Documents<StockBaseEntity>().ToList();
                    if (shouldFetchNearbyCars)
                    {
                        NearbyCarsBucket nearbyBucket;
                        Enum.TryParse(bucket.Key.Split('_')[0], out nearbyBucket);
                        foreach (var item in stockList)
                        {
                            item.NearbyCarsBucket = nearbyBucket;
                        }
                    }
                    stocksLists.Add(stockList);
                }

                int currIndex = 0;
                while ((currIndex < featuredSlotCount) && (premiumDealerStocks.Count < featuredSlotCount))
                {
                    for (int i = 0; i < stocksLists.Count && premiumDealerStocks.Count < featuredSlotCount; i++)
                    {
                        if (stocksLists[i].Count > currIndex && resultlistings.TrueForAll(stock => stock.ProfileId != stocksLists[i][currIndex].ProfileId))
                        {
                            premiumDealerStocks.Add(stocksLists[i][currIndex]);
                        }
                    }
                    currIndex++;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return premiumDealerStocks;
        }

        private void FillIsPremiumPackageProperty(List<StockBaseEntity> listings)
        {
            listings.Select(listing => { listing.IsPremiumPackage = listing.IsPremium; return listing; }).ToList();
        }

        private static void AddToResultListings(List<StockBaseEntity> listingsToBeAdded, List<StockBaseEntity> resultListings, ElasticOuptputs filterInputs)
        {
            //Reorder the listings- the listings with user preferred root (passed from client) should come at the top.
            if (filterInputs != null && filterInputs.userPreferredRootId > 0 && string.IsNullOrEmpty(filterInputs.sc) && (filterInputs.cars == null || filterInputs.cars.Length == 0) && listingsToBeAdded != null)
            {
                listingsToBeAdded = listingsToBeAdded.OrderBy(pdl => pdl.RootId == filterInputs.userPreferredRootId.ToString() ? 0 : 1).ToList();
            }
            resultListings.AddRange(listingsToBeAdded);
        }

        private static void AddNonPremiumFlagToListings(List<StockBaseEntity> listings)
        {
            listings.Select(listing => { listing.IsPremium = false; return listing; }).ToList();
        }

        private void AddMultiCityDealerDeliveryText(List<StockBaseEntity> listings, ElasticOuptputs filterInputs)
        {
            if (filterInputs.cities != null && filterInputs.cities.Length > 0 && filterInputs.cities[0] != "9999")
            {
                int deliveryCityId;
                string deliveryCity = string.Empty;
                if (filterInputs.multiCityId != 0)
                {
                    deliveryCityId = filterInputs.multiCityId;
                    deliveryCity = filterInputs.multiCityName;
                }
                else
                {

                    if (int.TryParse(filterInputs.cities[0].Trim(), out deliveryCityId))
                    {
                        deliveryCity = _geoCitiesCacheRepository.GetCityNameById(deliveryCityId.ToString());
                    }
                }

                foreach (var listing in listings)
                {
                    //Check if car comes from (multi-city dealer) and car has different city as compared to user input cities(filterInputs.cities) then show car delivery text.
                    if (listing.CityId != null && !IsCityMatch(listing.CityId, filterInputs.cities))
                    {
                        listing.DeliveryCity = deliveryCityId;
                        listing.DeliveryText = "Delivery available in " + deliveryCity;
                    }
                }
            }
        }

        private bool IsCityMatch(string cityId, string[] filterInputCities)
        {
            foreach (var filterInputCity in filterInputCities)
            {
                if (filterInputCity.Trim().Equals(cityId.Trim()))
                {
                    return true;
                }
            }
            return false;
        }

        private City GetCityObj(string cityId)
        {
            int CityId;
            City city = new City();
            if (!string.IsNullOrEmpty(cityId))
            {
                if (cityId == "3000")
                {
                    city.CityName = "Mumbai";
                }
                else if (cityId == "3001")
                {
                    city.CityName = "Delhi NCR";
                }
                else
                {
                    city.CityName = _geoCitiesCacheRepository.GetCityNameById(cityId);
                }
                int.TryParse(cityId, out CityId);
                city.CityId = CityId;
            }
            return city;
        }
    }
}
