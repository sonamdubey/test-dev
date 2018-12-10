using Carwale.Entity.Classified;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Utility.Classified;
using Nest;
using System;
using System.Collections.Generic;

namespace Carwale.DAL.Classified.ElasticSearch
{
    public class ProcessAggregationElasticJson : IProcessAggregationElasticJson
    {
        public SellerType GetSellerTypeCount(ISearchResponse<StockBaseEntity> searchResponse)
        {
            SellerType sellerType = new SellerType();

            SingleBucketAggregate seller = searchResponse?.Aggs.Filter("SellerCount");
            BucketAggregate sellerCount = (Nest.BucketAggregate)seller?.Aggregations["SellerIdCount"];
            var sellerItems = sellerCount?.Items;

            if (sellerItems != null)
            {
                foreach (KeyedBucket<object> sellerItem in sellerItems)
                {
                    if ((long)sellerItem.Key == 1)
                    {
                        sellerType.Dealer = Convert.ToInt32(sellerItem.DocCount);
                    }

                    if ((long)sellerItem.Key == 2)
                    {
                        sellerType.Individual = Convert.ToInt32(sellerItem.DocCount);
                    }
                }
            }
            return sellerType;
        }



        public CountData GetAllFilterCount(ISearchResponse<StockBaseEntity> searchResponse)
        {
            if (searchResponse == null)
            {
                return null;
            }
            CountData countData = new CountData();

            countData.StockCount = ProcessTotalStockCountResponse(searchResponse);

            // Fetching MakeCounts And RootCounts if available
            countData.MakeCount = ProcessMakeRootCountResponse(searchResponse);

            // For MumbaiAround and Delhi AroundCities  Count
            var mumbaiAroundCount = ProcessMumbaiAndAroundCountResponse(searchResponse);
            var delhiAroundCount = ProcessDelhiAndAroundCountResponse(searchResponse);
            countData.CityCount = new List<City>();
            countData.CityCount.Add(mumbaiAroundCount);
            countData.CityCount.Add(delhiAroundCount);
            // For City Counts
            var cityCount = ProcessCityCountResponse(searchResponse);
            countData.CityCount.AddRange(cityCount);

            // For BodyType Counts
            countData.BodyTypeCount = ProcessBodyTypeCountResponse(searchResponse);

            // For FuelType Counts
            countData.FuelTypeCount = ProcessFuelTypeCountResponse(searchResponse);

            // For Transmission Counts
            countData.TransmissionTypeCount = ProcessTransmissionTypeCountResponse(searchResponse);

            // For Color Counts
            countData.ColorTypeCount = ProcessColorTypeCountResponse(searchResponse);

            //For OwnersType Count
            countData.OwnersTypeCount = ProcessOwnersTypeCountResponse(searchResponse);

            //For CarsWithPhotos
            countData.FilterTypeAdditional = ProcessFilterByAdditionalResponse(searchResponse);
            countData.FilterTypeCount = new FilterBy
            {
                CarsWithPhotos = countData.FilterTypeAdditional.CarsWithPhotos
            };


            //FilterBy Counts for FranchiseCars
            var franchiseCars = searchResponse.Aggs.Filter("FilterBy3");
            countData.FilterTypeCount.FranchiseCars = Convert.ToInt32(franchiseCars.DocCount);

            //Assigning all counts
            return countData;
        }

        private StockCount ProcessTotalStockCountResponse(ISearchResponse<StockBaseEntity> searchResponse)
        {
            var totalStock = searchResponse.Aggs.Filter("TotalStockCount");
            return new StockCount
            {
                TotalStockCount = Convert.ToInt32(totalStock.DocCount)
            };
        }

        private List<StockMake> ProcessMakeRootCountResponse(ISearchResponse<StockBaseEntity> searchResponse)
        {

            var makeIdCountBuckets = searchResponse.Aggs.Filter("MakeCount").Terms("MakeIdCount");

            List<StockMake> objMakeCount = new List<StockMake>();

            foreach (var bucket in makeIdCountBuckets.Buckets)
            {
                StockMake makeObj = new StockMake();
                string[] makeIdBucketKeys = bucket.Key.Split('~');
                if (makeIdBucketKeys != null && makeIdBucketKeys.Length == 2)
                {
                    makeObj.MakeId = Convert.ToInt32(makeIdBucketKeys[0]);
                    makeObj.MakeName = makeIdBucketKeys[1];
                    makeObj.MakeCount = Convert.ToInt32(bucket.DocCount);
                }

                SingleBucketAggregate rootCountFilter = (SingleBucketAggregate)bucket.Aggregations["Root_Count"];


                List<StockRoot> rootObj = new List<StockRoot>();
                if (rootCountFilter.Aggregations != null)
                {
                    var rootIdCountBuckets = rootCountFilter.Terms("RootIdCount");
                    foreach (var rootBucket in rootIdCountBuckets.Buckets)
                    {
                        string[] rootIdBucketKeys = rootBucket.Key.Split('~');

                        if (rootIdBucketKeys != null && rootIdBucketKeys.Length == 4)
                        {
                            rootObj.Add(new StockRoot
                            {
                                RootId = Convert.ToInt32(rootIdBucketKeys[0]),
                                RootName = rootIdBucketKeys[1],
                                RootCount = Convert.ToInt32(rootBucket.DocCount),
                                isSuperLuxury = Convert.ToBoolean(rootIdBucketKeys[3])
                            });
                        }
                    }
                }
                makeObj.RootList = rootObj;
                objMakeCount.Add(makeObj);

            }

            return objMakeCount;
        }


        private static City ProcessMumbaiAndAroundCountResponse(ISearchResponse<StockBaseEntity> searchResponse)
        {
            var mumbaiAroundCount = searchResponse.Aggs.Filter("MumbaiAroundCities");
            return new City
            {
                CityId = 3000,
                CityCount = Convert.ToInt32(mumbaiAroundCount.DocCount),
                CityName = "Mumbai All"
            };
        }

        private static City ProcessDelhiAndAroundCountResponse(ISearchResponse<StockBaseEntity> searchResponse)
        {
            var delhiAroundCount = searchResponse.Aggs.Filter("DelhiAroundCities");
            return new City
            {
                CityId = 3001,
                CityCount = Convert.ToInt32(delhiAroundCount.DocCount),
                CityName = "Delhi NCR"
            };
        }

        private static List<City> ProcessCityCountResponse(ISearchResponse<StockBaseEntity> searchResponse)
        {
            List<City> cities = new List<City>();
            var cityIdCountBuckets = searchResponse.Aggs.Filter("CityCount").Terms("CityIdCount");

            foreach (var cityIdCountBucket in cityIdCountBuckets.Buckets)
            {
                City cityObj = new City();
                string[] cityIdCountBucketKeys = cityIdCountBucket.Key.Split('~');
                if (cityIdCountBucketKeys != null && cityIdCountBucketKeys.Length == 2)
                {
                    cityObj.CityId = Convert.ToInt32(cityIdCountBucketKeys[0]);
                    cityObj.CityName = cityIdCountBucketKeys[1];
                    cityObj.CityCount = Convert.ToInt32(cityIdCountBucket.DocCount);
                    cities.Add(cityObj);
                }
            }
            return cities;
        }

        private static BodyType ProcessBodyTypeCountResponse(ISearchResponse<StockBaseEntity> searchResponse)
        {
            var bodyTypeCount = new BodyType();
            var bodyTypeCountBuckets = searchResponse.Aggs.Filter("BodyTypeCount").Terms("BodyIdCount");

            foreach (var bodyTypeCountBucket in bodyTypeCountBuckets.Buckets)
            {
                switch (bodyTypeCountBucket.Key)
                {
                    case "Hatchback":
                        bodyTypeCount.Hatchback = Convert.ToInt32(bodyTypeCountBucket.DocCount);
                        break;
                    case "Sedan":
                    case "Compact Sedan":
                        bodyTypeCount.Sedan += Convert.ToInt32(bodyTypeCountBucket.DocCount);
                        break;
                    case "Minivan/Van":
                        bodyTypeCount.Minivan = Convert.ToInt32(bodyTypeCountBucket.DocCount);
                        break;
                    case "SUV/MUV":
                        bodyTypeCount.Suv = Convert.ToInt32(bodyTypeCountBucket.DocCount);
                        break;
                    case "Coupe":
                        bodyTypeCount.Coupe = Convert.ToInt32(bodyTypeCountBucket.DocCount);
                        break;
                    case "Station Wagon":
                        bodyTypeCount.StationWagon = Convert.ToInt32(bodyTypeCountBucket.DocCount);
                        break;
                    case "Truck":
                        bodyTypeCount.Truck = Convert.ToInt32(bodyTypeCountBucket.DocCount);
                        break;
                    case "Convertible":
                        bodyTypeCount.Convertible = Convert.ToInt32(bodyTypeCountBucket.DocCount);
                        break;
                        //No default required because every case for count is assigned. No special case here
                }
            }
            return bodyTypeCount;
        }

        private static FuelType ProcessFuelTypeCountResponse(ISearchResponse<StockBaseEntity> searchResponse)
        {
            var fuelTypeCount = new FuelType();
            var fuelTypeCountBuckets = searchResponse.Aggs.Filter("FuelTypeCount").Terms("FuelIdCount");

            foreach (var fuelTypeCountBucket in fuelTypeCountBuckets.Buckets)
            {
                switch (fuelTypeCountBucket.Key)
                {
                    case "Petrol":
                        fuelTypeCount.Petrol = Convert.ToInt32(fuelTypeCountBucket.DocCount);
                        break;
                    case "Diesel":
                        fuelTypeCount.Diesel = Convert.ToInt32(fuelTypeCountBucket.DocCount);
                        break;
                    case "CNG":
                        fuelTypeCount.CNG = Convert.ToInt32(fuelTypeCountBucket.DocCount);
                        break;
                    case "LPG":
                        fuelTypeCount.LPG = Convert.ToInt32(fuelTypeCountBucket.DocCount);
                        break;
                    case "Electric":
                        fuelTypeCount.Electric = Convert.ToInt32(fuelTypeCountBucket.DocCount);
                        break;
                    case "Hybrid":
                        fuelTypeCount.Hybrid = Convert.ToInt32(fuelTypeCountBucket.DocCount);
                        break;
                        //No default required because every case for count is assigned. No special case here
                }
            }
            return fuelTypeCount;
        }

        private static Transmission ProcessTransmissionTypeCountResponse(ISearchResponse<StockBaseEntity> searchResponse)
        {
            var transmissionTypeCount = new Transmission();
            var transmissionCountBuckets = searchResponse.Aggs.Filter("TransmissionCount")
                .Terms("TransmissionIdCount");

            foreach (var transmissionCountBucket in transmissionCountBuckets.Buckets)
            {
                switch (transmissionCountBucket.Key)
                {
                    case "Automatic":
                        transmissionTypeCount.Automatic = Convert.ToInt32(transmissionCountBucket.DocCount);
                        break;
                    case "Manual":
                        transmissionTypeCount.Manual = Convert.ToInt32(transmissionCountBucket.DocCount);
                        break;
                        //No default required because every case for count is assigned. No special case here
                }
            }
            return transmissionTypeCount;
        }

        private static AvailableColors ProcessColorTypeCountResponse(ISearchResponse<StockBaseEntity> searchResponse)
        {
            var colorTypeCount = new AvailableColors();
            var colorCountBuckets = searchResponse.Aggs.Filter("ColorCount").Terms("ColorIdCount");
            foreach (var colorCountBucket in colorCountBuckets.Buckets)
            {
                switch (Convert.ToInt32(colorCountBucket.Key))
                {
                    case 1:
                        colorTypeCount.Beige = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    case 2:
                        colorTypeCount.Black = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    case 3:
                        colorTypeCount.Blue = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    case 4:
                        colorTypeCount.Brown = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    case 5:
                        colorTypeCount.GoldNYellow = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    case 6:
                        colorTypeCount.Green = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    case 7:
                        colorTypeCount.Grey = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    case 8:
                        colorTypeCount.Maroon = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    case 9:
                        colorTypeCount.Purple = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    case 10:
                        colorTypeCount.Red = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    case 11:
                        colorTypeCount.Silver = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    case 12:
                        colorTypeCount.White = Convert.ToInt32(colorCountBucket.DocCount);
                        break;

                    default:
                        colorTypeCount.Others += Convert.ToInt32(colorCountBucket.DocCount);
                        break;
                }
            }
            return colorTypeCount;
        }

        private static Owners ProcessOwnersTypeCountResponse(ISearchResponse<StockBaseEntity> searchResponse)
        {
            var ownersTypeCount = new Owners();
            var ownersCountBuckets = searchResponse.Aggs.Filter("OwnersCount").Terms("OwnerTypeCount");

            foreach (var ownersCountBucket in ownersCountBuckets.Buckets)
            {
                switch (ownersCountBucket.Key.ToLowerInvariant().Trim())
                {
                    case "first owner":
                        ownersTypeCount.First = Convert.ToInt32(ownersCountBucket.DocCount);
                        break;
                    case "second owner":
                        ownersTypeCount.Second = Convert.ToInt32(ownersCountBucket.DocCount);
                        break;
                    case "third owner":
                    case "fourth owner":
                    case "more than 4 owners":
                    case "4 or more owners":
                    case "n/a":
                        ownersTypeCount.ThirdAndAbove += Convert.ToInt32(ownersCountBucket.DocCount);
                        break;
                    case "unregistered car":
                        ownersTypeCount.Unregistered += Convert.ToInt32(ownersCountBucket.DocCount);
                        break;
                        //No default required because every case for count is assigned. No special case here
                }
            }
            return ownersTypeCount;
        }

        private static FilterByAdditional ProcessFilterByAdditionalResponse(ISearchResponse<StockBaseEntity> searchResponse)
        {
            var filterTypeAdditional = new FilterByAdditional();
            var carsWithPhotosCountBuckets = searchResponse.Aggs.Filter("FilterBy2")
                .Terms("CarsWithPhotosCount");
            foreach (var carsWithPhotosCountBucket in carsWithPhotosCountBuckets.Buckets)
            {
                if (Convert.ToInt32(carsWithPhotosCountBucket.Key) == 1)
                {
                    filterTypeAdditional.CarsWithPhotos = Convert.ToInt32(carsWithPhotosCountBucket.DocCount);
                }
            }

            var carTradeCertifiedCarsBuckets = searchResponse.Aggs.Filter("FilterBy1")
                .Terms("CarTradeCertifiedCarsCount");

            foreach (var carTradeCertifiedCarsBucket in carTradeCertifiedCarsBuckets.Buckets)
            {

                if (carTradeCertifiedCarsBucket.Key == Constants.CarTradeCertificationId)
                {
                    filterTypeAdditional.CarTradeCertifiedCars = Convert.ToInt32(carTradeCertifiedCarsBucket.DocCount);
                }
            }
            

            return filterTypeAdditional;
        }
    }
}
