using System;
using System.Collections.Generic;
using System.Linq;
using BikewaleOpr.Interface.BikePricing;
using BikewaleOpr.Entity.BikePricing;
using System.Collections;
using System.Xml;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Entities.BikeData;
using Bikewale.Notifications;
using System.Configuration;
using System.Collections.ObjectModel;
using log4net;
using System.Text;

namespace BikewaleOpr.BAL.BikePricing
{
    /// <summary>
    /// Created By : Prabhu Puredla on 5 june 2018
    /// Description : Provide BAL methods for Bulk Price Upload
    /// </summary>
    public class BulkPrice : IBulkPrice
    {
        private readonly IBulkPriceRepository _bulkPriceRepos;
        private readonly IBikeModelsRepository _bikeModelsRepos;
        private readonly ILocation _locationRepos;
        private readonly IBikeVersions _versionsRepo;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BulkPrice));
        private readonly IBwPrice _bwPrice;
        public BulkPrice(IBulkPriceRepository bulkPriceRepos, IBikeModelsRepository bikeModelsRepos, ILocation locationRepos, IBikeVersions versionsRepo, IBwPrice bwPrice)
        {
            _bulkPriceRepos = bulkPriceRepos;
            _bikeModelsRepos = bikeModelsRepos;
            _locationRepos = locationRepos;
            _versionsRepo = versionsRepo;
            _bwPrice = bwPrice;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Get Processed Data which contains information regarding uploaded price file
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public CompositeBulkPriceEntity GetProcessedData(uint makeId, XmlReader xml)
        {
            try
            {
                CompositeBulkPriceEntity compositeBulkPriceEntity = new CompositeBulkPriceEntity()
              {
                  UnmappedBikes = new Collection<string>(),
                  UnmappedCities = new Collection<string>(),
                  BikeModelList = _bikeModelsRepos.GetModelsByMake(makeId),
                  States = _locationRepos.GetStates(),
                  UpdatedPriceList = new Collection<OemPriceEntity>(),
                  UnmappedOemPricesList = new Collection<OemPriceEntity>(),
                  CitiesTable = new Dictionary<string, uint>(),
                  BikesTable = new Dictionary<string, uint>(),
                  StatesTable = new Dictionary<string, uint>(),
                  OemPricesList = new Collection<OemPriceEntity>(),
                  ModelsTable = new Dictionary<string, uint>()
              };

                //load the bikes into a dictionary                
                LoadMappedBikesData(compositeBulkPriceEntity.BikesTable, compositeBulkPriceEntity.ModelsTable);

                //load the cities into a dictionary                
                LoadMappedCitiesData(compositeBulkPriceEntity.CitiesTable, compositeBulkPriceEntity.StatesTable);

                //now load the inputted price data into a dictionary          
                LoadOemPriceData(xml, compositeBulkPriceEntity.OemPricesList);

                //now process the data file and map the city names, and correspondingly update the status
                ProcessData(makeId, compositeBulkPriceEntity);

                return compositeBulkPriceEntity;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikePricing.GetProcessedData");
                return null;
            }
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Get all mapped bikes into dictionary
        /// </summary>
        /// <param name="bikesTable"></param>
        private void LoadMappedBikesData(IDictionary<string, uint> bikesTable, IDictionary<string, uint> modelsTable)
        {
            IEnumerable<MappedBikesEntity> mappedBikes = _bulkPriceRepos.GetAllMappedBikesData();
            if (mappedBikes != null)
            {
                foreach (var bike in mappedBikes)
                {
                    if (!bikesTable.ContainsKey(bike.OemBikeName.Trim()))
                    {
                        bikesTable.Add(bike.OemBikeName.Trim(), bike.Id);
                        modelsTable.Add(bike.OemBikeName.Trim(), bike.ModelId);
                    }                      
                }
            }

        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : get all mapped cities and states into dictionaries
        /// </summary>
        /// <param name="citiesTable"></param>
        /// <param name="statesTable"></param>
        private void LoadMappedCitiesData(IDictionary<string, uint> citiesTable, IDictionary<string, uint> statesTable)
        {

            IEnumerable<MappedCitiesEntity> mappedCities = _bulkPriceRepos.GetAllMappedCitiesData();
            if (mappedCities != null)
            {
                foreach (var city in mappedCities)
                {
                    if (!citiesTable.ContainsKey(city.OemCityName.Trim()))
                    {
                        citiesTable.Add(city.OemCityName.Trim(), city.Id);//check for id and bikewale bikename
                        statesTable.Add(city.OemCityName.Trim(), city.StateId);
                    }

                }
            }

        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Get all oem prices into collection
        /// </summary>
        /// </summary>
        /// <param name="xReader"></param>
        /// <param name="oemPriceList"></param>
        private void LoadOemPriceData(XmlReader xReader, ICollection<OemPriceEntity> oemPriceList)
        {
            try
            {
                while (xReader.Read())
                {
                    switch (xReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (xReader.Name)
                            {
                                case "version":
                                    string name = xReader.GetAttribute("name");
                                    string price = xReader.GetAttribute("price");
                                    string city = xReader.GetAttribute("city");

                                    OemPriceEntity oemPriceEntity = new OemPriceEntity()
                                    {
                                        BikeName = name,
                                        CityName = city,
                                        Price = price
                                    };
                                    oemPriceList.Add(oemPriceEntity);

                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikePricing.BulkPrice");
            }
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Method to populate the compositeBulkPriceEntity object
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="compositeBulkPriceEntity"></param>
        private void ProcessData(uint makeId, CompositeBulkPriceEntity compositeBulkPriceEntity)
        {
            try
            {
                IEnumerable<BikewaleOpr.Entities.StateEntityBase> states = _locationRepos.GetStates();
                IEnumerable<BikeModelEntityBase> bikes = _bikeModelsRepos.GetModels(makeId, "NEW");

                foreach (var bike in compositeBulkPriceEntity.OemPricesList)
                {
                    string bikeName = bike.BikeName.Trim();
                    string cityName = bike.CityName.Trim();
                    string price = bike.Price;

                    uint cityId = 0, bikeId = 0, stateId = 0, modelId = 0;

                    //get the id for this city
                    if (compositeBulkPriceEntity.CitiesTable.ContainsKey(cityName))
                    {
                        cityId = compositeBulkPriceEntity.CitiesTable[cityName];
                        stateId = compositeBulkPriceEntity.StatesTable[cityName];
                    }
                    if (compositeBulkPriceEntity.BikesTable.ContainsKey(bikeName))
                    {
                        bikeId = compositeBulkPriceEntity.BikesTable[bikeName];
                        modelId = compositeBulkPriceEntity.ModelsTable[bikeName];
                    }
                    if (cityId <= 0 || bikeId <= 0)
                    {
                        bike.BikeId = bikeId;
                        bike.CityId = cityId;
                        compositeBulkPriceEntity.UnmappedOemPricesList.Add(bike);
                    }
                    if (cityId > 0 && bikeId > 0)
                    {
                        OemPriceEntity oemPriceEntity = new OemPriceEntity();
                        price = price.Replace(",", "");
                        double bikeprice;

                        if (Double.TryParse(price, out bikeprice))
                        {
                            oemPriceEntity.CityId = cityId;
                            oemPriceEntity.BikeId = bikeId;
                            oemPriceEntity.Price = price;
                            oemPriceEntity.StateId = stateId;
                            oemPriceEntity.ModelId = modelId;
                            compositeBulkPriceEntity.UpdatedPriceList.Add(oemPriceEntity);
                        }
                    }
                    if (cityId <= 0)
                    {
                        if (!compositeBulkPriceEntity.UnmappedCities.Contains(cityName))
                        {
                            compositeBulkPriceEntity.UnmappedCities.Add(cityName);
                        }
                    }
                    if (bikeId <= 0)
                    {
                        if (!compositeBulkPriceEntity.UnmappedBikes.Contains(bikeName))
                        {
                            compositeBulkPriceEntity.UnmappedBikes.Add(bikeName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikePricing.ProcessData");
            }
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Save the updatable prices 
        /// </summary>
        /// <param name="PricesList"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public bool SavePrices(IEnumerable<OemPriceEntity> PricesList, uint updatedBy)
        {
            DateTime dt1 = DateTime.Now, dt2;
            try
            {
                ILookup<uint, OemPriceEntity> pricesLookUp = PricesList.ToLookup(x => x.BikeId);
                ICollection<uint> modelIds = new Collection<uint>(), cityIds = new Collection<uint>();
                ICollection<uint> modelIdsSet = new HashSet<uint>(), cityIdsSet = new HashSet<uint>();

                foreach (IGrouping<uint, OemPriceEntity> item in pricesLookUp)
                {
                    IEnumerable<OemPriceEntity> oemPriceEntityList = pricesLookUp[item.Key];
                    BikeVersionEntity objVersion = new BikeVersionEntity();

                    GetInsurance(item.Key, oemPriceEntityList, objVersion);
                    GetRto(oemPriceEntityList, objVersion);

                    StringBuilder prices = new StringBuilder();
                    int length = 0;
                    foreach (var oemPriceEntity in oemPriceEntityList)
                    {
                        string oemPriceInfo = string.Format("{0}#c0l#{1}#c0l#{2}#c0l#{3}|r0w|", oemPriceEntity.Price, oemPriceEntity.Insurance, oemPriceEntity.Rto, oemPriceEntity.CityId);
                        int oemPriceInfoLength = oemPriceInfo.Length;

                        //string length to be passed to the sp should be below 60,000
                        if (length + oemPriceInfoLength >= 60000)
                        {
                            //length-5 indicates that do not send row delimiter at the end of the string-constraint of SP
                            if(!_bulkPriceRepos.SavePrices(prices.ToString(0, length - 5), item.Key, updatedBy))
                            {
                                return false;
                            }

                            prices = new StringBuilder(oemPriceInfo);
                            length = oemPriceInfoLength;
                        }
                        else
                        {
                            prices = prices.Append(oemPriceInfo);
                            length += oemPriceInfoLength;
                        }
                        //To get the unique modelids 
                        if (!modelIdsSet.Contains(oemPriceEntity.ModelId))
                        {
                            modelIds.Add(oemPriceEntity.ModelId);
                            modelIdsSet.Add(oemPriceEntity.ModelId);
                        }
                        //To get the unique cityids
                        if (!cityIdsSet.Contains(oemPriceEntity.CityId))
                        {
                            cityIds.Add(oemPriceEntity.CityId);
                            cityIdsSet.Add(oemPriceEntity.CityId);
                        }
                    }
                    string bikePrices = prices.ToString(0,length-5);

                    if (!string.IsNullOrEmpty(bikePrices))
                    {
                        //length-5 indicates that do not send row delimiter at the end of the string-constraint of SP
                        if (!_bulkPriceRepos.SavePrices(bikePrices, item.Key, updatedBy))
                        {
                            return false;
                        }
                    }                   
                }
                _bwPrice.UpdateModelPriceDocumentV2(modelIds,cityIds);
                return true;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikePricing.SavePrices");
                return false;
            }
            finally
            {
                dt2 = DateTime.Now;
                ThreadContext.Properties["New_BulkPrices_TotalTime"] = (dt2 - dt1).TotalMilliseconds;
                _logger.Error("New_BulkPrices_SavePrices");
            }
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Method to return all insurances of a bike in all cities
        /// </summary>
        /// <param name="bikeId"></param>
        /// <param name="oemPriceEntityList"></param>
        /// <param name="objVersion"></param>
        private void GetInsurance(uint bikeId, IEnumerable<OemPriceEntity> oemPriceEntityList, BikeVersionEntity objVersion)
        {
            try
            {
                objVersion = _versionsRepo.GetVersionDetails(bikeId);
                if (objVersion != null)
                {
                    double premium = 0;
                    foreach (var oemPriceEntity in oemPriceEntityList)
                    {
                        double oemPrice = Double.Parse(oemPriceEntity.Price);

                        if (oemPrice > 0 && objVersion.Displacement > 0)
                        {
                            double rate = 0; // rate to be applied on IDV.
                            double depreciation = 5; // depreciation to be applied.
                            string depKey = "", rateKey = "";
                            double idvBike = 0;
                            double liabilities = 0, liaPremium = 0; // liaPADriverOwner = 0, liaPaidDriver = 0;
                            string zone = "B";

                            ushort[] zoneA = new ushort[] { 1, 2, 10, 12, 105, 128, 176, 198, 1066 }; // zoneA array contains list of metro CityIds					

                            depKey = "d_.5"; // even a new bike will have a depreciation of 5%.

                            // normalize cc. Three categories. 1000, 1500 and above 1500.
                            if (objVersion.Displacement <= 150) objVersion.Displacement = 1; // category 1
                            else if (objVersion.Displacement > 150 && objVersion.Displacement <= 350) objVersion.Displacement = 2; // category 2
                            else objVersion.Displacement = 3; // category 3

                            // check if the city comes in zone A?
                            for (int i = 0; i < zoneA.Length; i++)
                            {
                                if (zoneA[i] == Convert.ToUInt16(oemPriceEntity.CityId)) zone = "A";
                            }

                            rateKey = zone + ":" + objVersion.Displacement;

                            double discount = 0;

                            rate = double.Parse(ConfigurationManager.AppSettings[rateKey]);

                            rate = rate * (1 - discount / 100.0);   //deduct the discount

                            // get depreciation % from web.config.						
                            depreciation = 100 - double.Parse(ConfigurationManager.AppSettings[depKey]);

                            // calculate IDV 					
                            idvBike = oemPrice * depreciation / 100.0;

                            // calculate od.
                            premium = idvBike * rate / 100.0;

                            // Third-party Liability.					
                            liaPremium = double.Parse(ConfigurationManager.AppSettings["L:" + objVersion.Displacement]);

                            liabilities = liaPremium;

                            // Get the Premium.
                            premium += liabilities;

                            // Add service tax.						
                            premium += premium * double.Parse(ConfigurationManager.AppSettings["ServiceTax"]) / 100.0;

                            // round premium
                            premium = Math.Round(premium, 2);
                        }
                        else if (objVersion.BikeFuelType == 5)  // Insurance Calculations for electric bikes FuelType - 5 (Electric bikes)
                        {
                            if (objVersion.TopSpeed > 25)
                                premium = 1250;
                            else if (objVersion.TopSpeed <= 25 && objVersion.TopSpeed > 0)
                                premium = 1000;
                        }
                        oemPriceEntity.Insurance = premium;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikePricing.GetInsurances");
            }

        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Method to return all rtos of a bike in all cities
        /// </summary>
        /// <param name="bikeId"></param>
        /// <param name="oemPriceEntityList"></param>
        /// <param name="objVersion"></param>
        private void GetRto(IEnumerable<OemPriceEntity> oemPriceEntityList, BikeVersionEntity objVersion)
        {
            try
            {
                if (objVersion != null)
                {
                    foreach (var oemPriceEntity in oemPriceEntityList)
                    {
                        double tmpTax = 0;
                        double roadTax = 0;
                        double oemPrice = Double.Parse(oemPriceEntity.Price);
                        double regCharges = 0;
                        switch (oemPriceEntity.StateId)
                        {
                            // Maharashtra.
                            /*
                                Maharashtra	Indian	0-99 cc	8%
                                Maharashtra	Indian	100-299 cc	9%
                                Maharashtra	Indian	300 + CC	10%
                                Maharashtra	Imported	0-99 cc	16%
                                Maharashtra	Imported	100-299 cc	18%

                            */
                            case 1:
                                if (objVersion.Displacement >= 0 && objVersion.Displacement <= 99)
                                {
                                    if (objVersion.IsImported)
                                    {
                                        roadTax = oemPrice * 0.18;
                                    }
                                    else
                                    {
                                        roadTax = oemPrice * 0.10;
                                    }
                                }
                                else if (objVersion.Displacement > 100 && objVersion.Displacement <= 299)
                                {
                                    if (objVersion.IsImported)
                                    {
                                        roadTax = oemPrice * 0.20;
                                    }
                                    else
                                    {
                                        roadTax = oemPrice * 0.11;
                                    }
                                }
                                else
                                {
                                    if (objVersion.IsImported)
                                    {
                                        roadTax = oemPrice * 0.22;
                                    }
                                    else
                                    {
                                        roadTax = oemPrice * 0.12;
                                    }
                                }


                                break;
                            // Tamilnadu	All		8%
                            case 11:
                                roadTax = oemPrice * 0.08;
                                break;
                            // Andhra Pradesh. 
                            // 12% if oemPriceupto 1000000
                            // 14% for others
                            case 6:
                                roadTax = oemPrice * 0.09;
                                break;

                            // calculation for telangana
                            case 41:
                                roadTax = oemPrice * 0.09;
                                break;

                            // Delhi. 
                            /*
                                Delhi		0-25000 Rs.	2%
                                Delhi		25000-40000	4%
                                Delhi		40000 - 60000	6%
                                Delhi		60000 +	8%
                            */
                            case 5:
                                if (oemPrice <= 25000)
                                {
                                    roadTax = oemPrice * 0.02;
                                }
                                else if (oemPrice > 25000 && oemPrice <= 40000)
                                {
                                    roadTax = oemPrice * 0.04;
                                }
                                else if (oemPrice > 40000 && oemPrice <= 60000)
                                {
                                    roadTax = oemPrice * 0.06;
                                }
                                else
                                {
                                    roadTax = oemPrice * 0.08;
                                }
                                break;
                            // GOA. 
                            // 5% if oemPriceless than or equal to 600000 + 310
                            // Above 6L = 6%+310
                            case 17:
                                if (oemPrice <= 200000)
                                {
                                    roadTax = oemPrice * 0.08;
                                    //roadTax += 310;
                                }
                                else
                                {
                                    roadTax = oemPrice * 0.12;
                                    //roadTax += 310;
                                }
                                break;

                            // ASSAM. 
                            //Assam	<65 kg		2600
                            //Assam	65-90 kg		3600
                            //Assam	90-135 kg		5000
                            //Assam	135-165 kg		5500
                            //Assam	>165 kg		6500
                            case 16:
                                if (objVersion.KerbWeight < 65)
                                {
                                    roadTax = 2600;
                                }
                                else if (objVersion.KerbWeight >= 65 && objVersion.KerbWeight <= 90)
                                {
                                    roadTax = 3600;
                                }
                                else if (objVersion.KerbWeight > 90 && objVersion.KerbWeight <= 135)
                                {
                                    roadTax = 5000;
                                }
                                else if (objVersion.KerbWeight > 135 && objVersion.KerbWeight <= 165)
                                {
                                    roadTax = 5500;
                                }
                                else
                                {
                                    roadTax = 6500;
                                }
                                break;

                            //Uttar Pradesh

                            // Uttar Prdesh	All		10%
                            case 15:
                                roadTax = oemPrice * 0.1;
                                break;
                            // Madhya Pradesh. 7%!
                            case 3:
                                if (objVersion.BikeFuelType == 5) //FuelType - 5 (Electri Bikes)
                                    roadTax = oemPrice * 0.05;
                                else
                                    roadTax = oemPrice * 0.07;
                                break;
                            // Orissa. 5%!
                            case 20:
                                roadTax = oemPrice * 0.05;
                                break;
                            // Gujarat. 5.2174%, 10.434% (imported)!
                            case 9:
                                roadTax = oemPrice * 0.06;
                                break;

                            //Chhattisgarh 
                            //0-500000 - 5% of Ex
                            //500000+  - 7% of Ex
                            case 8:
                                if (oemPrice <= 500000)
                                {
                                    roadTax = oemPrice * 0.05;
                                }
                                else
                                {
                                    roadTax = oemPrice * 0.07;
                                }
                                break;
                            // Karnataka. 
                            // 14.3% if oemPriceup to 500000
                            // 15.4% if oemPriceupto 1000000
                            // 18.7% if oemPriceupto 2000000
                            // 19.8% otherwise.
                            case 2:
                                if (objVersion.BikeFuelType == 5)
                                {
                                    roadTax = oemPrice * 0.04;
                                }
                                else
                                {
                                    if (oemPrice <= 50000)
                                    {
                                        roadTax = oemPrice * 0.11;
                                    }
                                    else
                                    {
                                        roadTax = oemPrice * 0.13;
                                    }
                                }
                                break;
                            // Bihar. 
                            case 14:
                                roadTax = oemPrice * 0.07;
                                break;
                            // Kerala. 6%!
                            case 4:
                                roadTax = oemPrice * 0.06;
                                break;
                            // Rajasthan. 
                            // 5% if oemPriceless than 6 lakh
                            // 8% if oemPriceless than 10 lakh
                            // 10% otherwise.
                            case 10:

                                if (objVersion.Displacement >= 50)
                                {
                                    roadTax = oemPrice * 0.04;
                                }
                                else
                                {
                                    roadTax = oemPrice * 0.06;
                                }
                                roadTax += 500; //Green Tax for Rajasthan
                                break;
                            // Uttaranchal. 2%!
                            case 25:
                                if (oemPrice <= 1000000)
                                {
                                    roadTax = oemPrice * 0.04;
                                }
                                else
                                {
                                    roadTax = oemPrice * 0.05;
                                }

                                //roadTax = oemPrice* 0.02;
                                break;
                            // Manipur. 
                            // Rs.2925 till 1000kg
                            // Rs.3600 till 1500kg
                            // Rs.4500 till 2000kg
                            // Rs. 4500 + 2925 more than 2000kg
                            case 36:
                                if (objVersion.KerbWeight <= 1000)
                                {
                                    roadTax = 2925;
                                }
                                else if (objVersion.KerbWeight <= 1500)
                                {

                                    roadTax = 3600;
                                }
                                else if (objVersion.KerbWeight <= 2000)
                                {
                                    roadTax = 4500;
                                }
                                else
                                {
                                    roadTax = 4500 + 2925;
                                }
                                break;
                            // Punjab. 2%!
                            case 18:
                                if (objVersion.Displacement >= 50)
                                {
                                    roadTax = oemPrice * 0.015;
                                }
                                else
                                {
                                    roadTax = oemPrice * 0.03;
                                }
                                break;
                            // Chandigarh. 
                            //if oemPrice< 7 lakh Then 2% + 3000.
                            //if oemPrice> 7 lakh and oemPrice< 20 lakh 3% + 5000
                            //if oemPrice> 20 lakh Then 4% + 10000.
                            case 21:
                                if (oemPrice <= 100000)
                                    roadTax = (oemPrice * 0.0268);
                                else if (oemPrice > 100000 && oemPrice <= 400000)
                                    roadTax = (oemPrice * 0.0357);
                                else
                                    roadTax = (oemPrice * 0.0446);
                                break;
                            // Haryana. 
                            //Haryana	< 75,000		4%
                            //Haryana	75,000 - 2 Lakh		6%
                            //Haryana	2 + Lakh		8%

                            case 22:
                                if (oemPrice < 75000)
                                    roadTax = (oemPrice * 0.04);
                                else if (oemPrice >= 75000 && oemPrice <= 200000)
                                    roadTax = (oemPrice * 0.06);
                                else
                                    roadTax = (oemPrice * 0.08);
                                break;

                            //west bengal

                            //West Bengal		Upto 80 cc	6.5% of vehicle cost or Rs 1800 (whichever is higher)
                            //West Bengal		Between 80 and 160 cc	9% of vehicle cost or Rs 3600 (whichever is higher)
                            //West Bengal		More than 160 cc	10% of vehicle cost or Rs 5800 (whichever is higher)
                            case 12:
                                //double tmpTax = 0;

                                if (objVersion.Displacement >= 0 && objVersion.Displacement <= 80)
                                {
                                    tmpTax = oemPrice * 0.065;

                                    if (tmpTax <= 1800)
                                    {
                                        roadTax = 1800;
                                    }
                                    else
                                    {
                                        roadTax = tmpTax;
                                    }
                                }
                                else if (objVersion.Displacement > 80 && objVersion.Displacement <= 160)
                                {
                                    tmpTax = oemPrice * 0.09;

                                    if (tmpTax <= 3600)
                                    {
                                        roadTax = 3600;
                                    }
                                    else
                                    {
                                        roadTax = tmpTax;
                                    }
                                }
                                else
                                {
                                    tmpTax = oemPrice * 0.1;

                                    if (tmpTax <= 5800)
                                    {
                                        roadTax = 5800;
                                    }
                                    else
                                    {
                                        roadTax = tmpTax;
                                    }
                                }
                                break;

                            // Jharkhand. 
                            // 0-5 Seater 3% of Ex-showroom
                            // 6-8 seater 4% of Ex-showroom
                            //8 + seater 5% of Ex-showroom
                            case 23:
                                roadTax = oemPrice * 0.03;
                                break;

                            // Arunachal Pradesh
                            // Correct Logic:
                            //If weight < 100 Kgs		Rs. 2000 + Rs. 90
                            //If weight 100 - 135 Kgs	Rs.3000 + Rs. 90
                            //If weight > 135 Kgs		Rs. 3500 + Rs. 90
                            case 35:
                                if (objVersion.KerbWeight <= 100)
                                {
                                    roadTax = 2090;
                                }
                                else if (objVersion.KerbWeight > 100 && objVersion.KerbWeight <= 135)
                                {
                                    roadTax = 3090;
                                }
                                else
                                {
                                    roadTax = 3590;
                                }
                                break;

                            // Daman & Diu
                            // If oemPrice< 2 lakh	8%
                            // If oemPrice> 2 lakh	12%
                            case 38:
                                if (objVersion.IsImported)
                                {
                                    roadTax = oemPrice * 0.05;
                                }
                                else
                                {
                                    roadTax = oemPrice * 0.025;
                                }
                                break;

                            //Jammu & Kashmir
                            //If Body-Style is Scooter	Rs. 2400
                            //If Body-Style is not Scooter	Rs. 4000
                            case 24:
                                if (objVersion.BodyStyleId == 5)
                                {
                                    roadTax = 2400;
                                }
                                else
                                {
                                    roadTax = 4000;
                                }
                                break;

                            //Meghalaya
                            //If weight < 65 Kgs		Rs. 1050
                            //If weight 65 - 90 Kgs		Rs. 1725
                            //If weight 90 - 135 Kgs	Rs. 2400
                            //If weight > 135 Kgs		Rs. 2850
                            case 13:
                                if (objVersion.KerbWeight <= 65)
                                {
                                    roadTax = 1050;
                                }
                                else if (objVersion.KerbWeight > 65 && objVersion.KerbWeight <= 90)
                                {
                                    roadTax = 1725;
                                }
                                else if (objVersion.KerbWeight > 90 && objVersion.KerbWeight <= 135)
                                {
                                    roadTax = 2400;
                                }
                                else
                                {
                                    roadTax = 2850;
                                }
                                break;

                            //Tripura
                            //If bike is without gear	Rs. 1000
                            //If bike is with gear
                            //If oemPrice< 1 lakh	RS. 2200
                            //If oemPrice> 1 lakh	RS. 2650
                            case 26:
                                if (objVersion.BodyStyleId == 5)
                                {
                                    roadTax = 1000;
                                }
                                else
                                {
                                    if (oemPrice <= 100000)
                                    {
                                        roadTax = 2200;
                                    }
                                    else
                                    {
                                        roadTax = 2650;
                                    }
                                }
                                break;


                            default:
                                break;
                        }

                        // now include approx 1% of oemPriceor 4000 flat 
                        // as dealer commission, service and handling charges etc.

                        if (regCharges != 0)
                        {
                            regCharges = roadTax + regCharges;
                        }
                        else
                        {
                            if (oemPrice <= 500000)
                            {
                                regCharges = roadTax + 1500;
                            }
                            else if (oemPrice > 500000 && oemPrice <= 800000)
                            {
                                regCharges = roadTax + 5000;
                            }
                            else if (oemPrice > 800000 && oemPrice <= 1500000)
                            {
                                regCharges = roadTax + 8000;
                            }
                            else if (oemPrice > 1500000 && oemPrice <= 3000000)
                            {
                                regCharges = roadTax + 12000;
                            }
                            else if (oemPrice > 3000000 && oemPrice <= 8000000)
                            {
                                regCharges = roadTax + 25000;
                            }
                            else
                            {
                                regCharges = roadTax + 50000;
                            }

                            // RTO Calculations for electric bikes whose speed is less than 25
                            if (objVersion.BikeFuelType == 5)
                            {
                                if (objVersion.TopSpeed <= 25 && objVersion.TopSpeed > 0)
                                    regCharges = 0;
                            }
                        }
                        oemPriceEntity.Rto = regCharges;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikePricing.GetRto");
            }
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Add the unmapped bikes prices into updatedPriceList and map the bike 
        /// </summary>
        /// <param name="oemBikeName"></param>
        /// <param name="bikeId"></param>
        /// <param name="unmappedOemPricesList"></param>
        /// <param name="updatedPriceList"></param>
        /// <param name="unmappedBikes"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public bool MapUnmappedBike(string oemBikeName, uint bikeId, IEnumerable<OemPriceEntity> unmappedOemPricesList, ICollection<OemPriceEntity> updatedPriceList, ICollection<string> unmappedBikes, uint updatedBy)
        {
            try
            {
                if (_bulkPriceRepos.MapTheUnmappedBike(oemBikeName, bikeId, updatedBy))
                {
                    var tempList = unmappedOemPricesList;

                    foreach (var bike in tempList)
                    {
                        if (bike.BikeName.Trim() == oemBikeName.Trim())
                        {
                            bike.BikeId = bikeId;
                            if (bike.CityId > 0)
                            {
                                updatedPriceList.Add(bike);
                            }
                        }
                    }
                    unmappedBikes.Remove(oemBikeName.Trim());

                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikePricing.MapUnmappedBike");

            }
            return false;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Add the unmapped cities prices into updatedPriceList and map the city 
        /// </summary>
        /// <param name="oemCityName"></param>
        /// <param name="cityId"></param>
        /// <param name="unmappedOemPricesList"></param>
        /// <param name="updatedPriceList"></param>
        /// <param name="unmappedCities"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public bool MapUnmappedCity(string oemCityName, uint cityId, IEnumerable<OemPriceEntity> unmappedOemPricesList, ICollection<OemPriceEntity> updatedPriceList, ICollection<string> unmappedCities, uint updatedBy)
        {
            try
            {
                if (_bulkPriceRepos.MapTheUnmappedCity(cityId, oemCityName, updatedBy))
                {
                    var tempList = unmappedOemPricesList;

                    foreach (var city in tempList)
                    {
                        if (city.CityName.Trim() == oemCityName)
                        {
                            city.CityId = cityId;

                            if (city.BikeId > 0)
                            {
                                updatedPriceList.Add(city);
                            }
                        }
                    }
                    unmappedCities.Remove(oemCityName);

                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikePricing.MapUnmappedCity");
            }
            return false;
        }
    }
}
