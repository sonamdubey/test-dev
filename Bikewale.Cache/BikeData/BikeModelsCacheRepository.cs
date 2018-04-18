using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bikewale.Cache.BikeData
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 7 Oct 2015
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeModelsCacheRepository<T, U> : IBikeModelsCacheRepository<U>
    {
        private readonly ICacheManager _cache;
        private readonly IBikeModelsRepository<T, U> _modelRepository;
        private readonly IPager _objPager;
        private static readonly string _modelIdsWithBodyStyleKey = "BW_ModelIdsWithBodyStyle";

        /// <summary>
        /// Intitalize the references for the cache and BL
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objModels"></param>
        public BikeModelsCacheRepository(ICacheManager cache, IBikeModelsRepository<T, U> modelRepository, IPager objPager)
        {
            _cache = cache;
            _modelRepository = modelRepository;
            _objPager = objPager;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 8 Oct 2015
        /// Summary : Function to get the model page details from the cache. If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Returns BikeModelPageEntity</returns>
        public BikeModelPageEntity GetModelPageDetails(U modelId)
        {
            BikeModelPageEntity objModelPage = null;
            try
            {
                objModelPage = GetModelPageDetails(modelId, 0);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetModelPageDetails");

            }

            return objModelPage;
        }
        /// <summary>
        /// Created By: Sangram Nandkhile on 01 Dec 2016
        /// Summary: To Create a overload of cached model entity with version Id
        /// Modified by Sajal Gupta on 19-05-2017
        /// Description : Changed version of cache key.
        /// Modified by : Vivek Singh Tomar on 27th Sep 2017
        /// Summary : Changed version of cache key
        /// Modified by : Ashutosh Sharma on 29 Sep 2017.
        /// Description : Changed version of key from 'BW_ModelDetail_' to 'BW_ModelDetail_V1_'.
        /// Modified by : Rajan Chauhan on 06 Feb 2018.
        /// Description : Changed version of key from 'BW_ModelDetail_V1_' to 'BW_ModelDetail_'.
        /// </summary>
        public BikeModelPageEntity GetModelPageDetails(U modelId, int versionId)
        {
            BikeModelPageEntity objModelPage = null;
            string key = string.Format("BW_ModelDetail_V1_{0}", modelId);
            try
            {
                objModelPage = _cache.GetFromCache(key, new TimeSpan(1, 0, 0), () => GetModelPageDetailsNew(modelId, versionId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetModelPageDetails() => modelid {0}, versionId: {1}", modelId, versionId));

            }

            return objModelPage;
        }

        /// <summary>
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Description : Moved function from BAL to cache
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private BikeModelPageEntity GetModelPageDetailsNew(U modelId, int versionId)
        {
            return _modelRepository.GetModelPage(modelId, versionId);
        }

        /// <summary>
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Description : Moved function from BAL to cache
        /// </summary>
        /// <param name="bikeSpecificationEntity"></param>
        /// <returns></returns>
        private Features FetchFeatures(BikeSpecificationEntity bikeSpecificationEntity)
        {
            Features objFeatures = new Features()
            {
                DisplayName = "Features"
            };

            List<Specs> objFeatuesList = new List<Specs>();
            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Speedometer",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Speedometer)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Fuel Guage",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FuelGauge)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Tachometer Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.TachometerType)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Digital Fuel Guage",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.DigitalFuelGauge)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Tripmeter",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Tripmeter)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Electric Start",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.ElectricStart)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Tachometer",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Tachometer)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Shift Light",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.ShiftLight)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "No. of Tripmeters",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.NoOfTripmeters)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Tripmeter Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.TripmeterType)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Low Fuel Indicator",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.LowFuelIndicator)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Low Oil Indicator",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.LowOilIndicator)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Low Battery Indicator",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.LowBatteryIndicator)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Pillion Seat",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.PillionSeat)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Pillion Footrest",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.PillionFootrest)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Pillion Backrest",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.PillionBackrest)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Pillion Grabrail",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.PillionGrabrail)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Stand Alarm",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.StandAlarm)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Stepped Seat",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.SteppedSeat)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Antilock Braking System",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.AntilockBrakingSystem)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Killswitch",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Killswitch)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Clock",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Clock)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Electric System",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.ElectricSystem)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Battery",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Battery)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Headlight Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.HeadlightType)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Headlight Bulb Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.HeadlightBulbType)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Brake/Tail Light",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Brake_Tail_Light)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Turn Signal",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.TurnSignal)
            });

            objFeatuesList.Add(new Specs()
            {
                DisplayText = "Pass Light",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.PassLight)
            });

            objFeatures.FeaturesList = objFeatuesList;
            return objFeatures;
        }


        /// <summary>
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Description : Moved function from BAL to cache
        /// </summary>
        /// <param name="bikeSpecificationEntity"></param>
        /// <returns></returns>
        private Specifications FetchSpecList(BikeSpecificationEntity bikeSpecificationEntity)
        {
            Specifications objSpecs = new Specifications()
            {
                DisplayName = "Specifications"
            };

            List<SpecsCategory> objSpecifications = new List<SpecsCategory>();

            // Add summary as subcategory to specifications
            SpecsCategory objSummary = new SpecsCategory()
            {
                CategoryName = "Summary",
                DisplayName = "Summary"
            };

            // Add specifications to the summary
            List<Specs> objSummarySpecs = new List<Specs>();

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Displacement",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Displacement, "cc")
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Max Power",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.MaxPower, "bhp", bikeSpecificationEntity.MaxPowerRPM, "rpm")
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Maximum Torque",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.MaximumTorque, "Nm", bikeSpecificationEntity.MaximumTorqueRPM, "rpm")
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "No. of gears",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.NoOfGears)
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Fuel Efficiency",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FuelEfficiencyOverall, "kmpl")
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Brake Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.BrakeType)
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Front Disc",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FrontDisc)
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Rear Disc",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.RearDisc)
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Alloy Wheels",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.AlloyWheels)
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Kerb Weight",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.KerbWeight, "kg")
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Chassis Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.ChassisType)
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Top Speed",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.TopSpeed, "kmph")
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Tubeless Tyres",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.TubelessTyres)
            });

            objSummarySpecs.Add(new Specs()
            {
                DisplayText = "Fuel Tank Capacity",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FuelTankCapacity, "litres")
            });

            objSummary.Specs = objSummarySpecs;

            // Add specs to the Engine and transmission
            SpecsCategory objEngTrans = new SpecsCategory()
            {
                CategoryName = "EngTrans",
                DisplayName = "Engine & Transmission"
            };

            List<Specs> objEngTransSpecs = new List<Specs>();

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Displacement",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Displacement, "cc")
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Cylinders",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Cylinders)
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Max Power",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.MaxPower, "bhp", bikeSpecificationEntity.MaxPowerRPM, "rpm")
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Maximum Torque",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.MaximumTorque, "Nm", bikeSpecificationEntity.MaximumTorqueRPM, "rpm")
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Bore",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Bore, "mm")
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Stroke",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Stroke, "mm")
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Valves Per Cylinder",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.ValvesPerCylinder)
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Fuel Delivery System",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FuelDeliverySystem)
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Fuel Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FuelType)
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Ignition",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Ignition)
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Spark Plugs",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.SparkPlugsPerCylinder, "Per Cylinder")
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Cooling System",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.CoolingSystem)
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Gearbox Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.GearboxType)
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "No. of Gears",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.NoOfGears)
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Transmission Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.TransmissionType)
            });

            objEngTransSpecs.Add(new Specs()
            {
                DisplayText = "Clutch",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Clutch)
            });

            objEngTrans.Specs = objEngTransSpecs;

            // Add Brakes, wheels and suspension
            SpecsCategory objBrakesWheels = new SpecsCategory()
            {
                CategoryName = "BrakesAndWheels",
                DisplayName = "Brakes, Wheels and Suspension"
            };

            List<Specs> objBrakesWheelsSpecs = new List<Specs>();

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Brake Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.BrakeType)
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Front Disc",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FrontDisc)
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Front Disc/Drum Size",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FrontDisc_DrumSize, "mm")
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Rear Disc",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.RearDisc)
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Rear Disc/Drum Size",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.RearDisc_DrumSize, "mm")
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Calliper Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.CalliperType)
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Wheel Size",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.WheelSize, "inches")
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Front Tyre",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FrontTyre)
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Rear Tyre",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.RearTyre)
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Tubeless Tyres",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.TubelessTyres)
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Radial Tyres",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.RadialTyres)
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Alloy Wheels",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.AlloyWheels)
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Front Suspension",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FrontSuspension)
            });

            objBrakesWheelsSpecs.Add(new Specs()
            {
                DisplayText = "Rear Suspension",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.RearSuspension)
            });

            objBrakesWheels.Specs = objBrakesWheelsSpecs;

            // Add Dimensions and chassis specs

            SpecsCategory objChassis = new SpecsCategory()
            {
                CategoryName = "DimChassis",
                DisplayName = "Dimensions and Chassis"
            };

            List<Specs> objChassisSpecs = new List<Specs>();

            objChassisSpecs.Add(new Specs()
            {
                DisplayText = "Kerb Weight",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.KerbWeight, "kg")
            });

            objChassisSpecs.Add(new Specs()
            {
                DisplayText = "Overall Length",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.OverallLength, "mm")
            });

            objChassisSpecs.Add(new Specs()
            {
                DisplayText = "Overall Width",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.OverallWidth, "mm")
            });

            objChassisSpecs.Add(new Specs()
            {
                DisplayText = "Overall Height",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.OverallHeight, "mm")
            });

            objChassisSpecs.Add(new Specs()
            {
                DisplayText = "Wheelbase",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Wheelbase, "mm")
            });

            objChassisSpecs.Add(new Specs()
            {
                DisplayText = "Ground Clearance",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.GroundClearance, "mm")
            });

            objChassisSpecs.Add(new Specs()
            {
                DisplayText = "Seat Height",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.SeatHeight, "mm")
            });

            objChassisSpecs.Add(new Specs()
            {
                DisplayText = "Chassis Type",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.ChassisType)
            });

            objChassis.Specs = objChassisSpecs;

            // Add fuel efficiency and performance
            SpecsCategory objFuel = new SpecsCategory()
            {
                CategoryName = "FuelEffieciency",
                DisplayName = "Fuel efficiency and Performance"
            };

            List<Specs> objFuelSpecs = new List<Specs>();

            objFuelSpecs.Add(new Specs()
            {
                DisplayText = "Fuel Tank Capacity",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FuelTankCapacity, "litres")
            });

            objFuelSpecs.Add(new Specs()
            {
                DisplayText = "Reserve Fuel Capacity",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.ReserveFuelCapacity, "litres")
            });

            objFuelSpecs.Add(new Specs()
            {
                DisplayText = "Fuel Efficiency Overall",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FuelEfficiencyOverall, "kmpl")
            });

            objFuelSpecs.Add(new Specs()
            {
                DisplayText = "Fuel Efficiency Range",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FuelEfficiencyRange, "km")
            });

            objFuelSpecs.Add(new Specs()
            {
                DisplayText = "0 to 60 kmph",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Performance_0_60_kmph, "seconds")
            });

            objFuelSpecs.Add(new Specs()
            {
                DisplayText = "0 to 80 kmph",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Performance_0_80_kmph, "seconds")
            });

            objFuelSpecs.Add(new Specs()
            {
                DisplayText = "0 to 40 kmph",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Performance_0_40_m, "seconds")
            });

            objFuelSpecs.Add(new Specs()
            {
                DisplayText = "Top Speed",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.TopSpeed, "kmph")
            });

            objFuelSpecs.Add(new Specs()
            {
                DisplayText = "60 to 0 kmph",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Performance_60_0_kmph)
            });

            objFuelSpecs.Add(new Specs()
            {
                DisplayText = "80 to 0 kmph",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Performance_80_0_kmph)
            });

            objFuel.Specs = objFuelSpecs;

            // Add specification categories to the specs object
            objSpecifications.Add(objSummary);
            objSpecifications.Add(objEngTrans);
            objSpecifications.Add(objBrakesWheels);
            objSpecifications.Add(objChassis);
            objSpecifications.Add(objFuel);
            objSpecs.SpecsCategory = objSpecifications;
            return objSpecs;
        }

        /// <summary>
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Description : Moved function from BAL to cache
        /// </summary>
        /// <param name="bikeSpecificationEntity"></param>
        /// <returns></returns>
        private Overview FetchOverViewList(BikeSpecificationEntity bikeSpecificationEntity)
        {
            Overview objOverview = new Overview()
            {
                DisplayName = "Overview"
            };

            List<Specs> objOverviewSpecs = new List<Specs>();

            objOverviewSpecs.Add(new Specs()
            {
                DisplayText = "Capacity",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.Displacement, "cc")
            });

            objOverviewSpecs.Add(new Specs()
            {
                DisplayText = "Mileage",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.FuelEfficiencyOverall, "kmpl")
            });

            objOverviewSpecs.Add(new Specs()
            {
                DisplayText = "Max power",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.MaxPower, "bhp")
            });

            objOverviewSpecs.Add(new Specs()
            {
                DisplayText = "Weight",
                DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeSpecificationEntity.KerbWeight, "kg")
            });

            objOverview.OverviewList = objOverviewSpecs;
            return objOverview;
        }

        /// <summary>
        /// Created by Subodh Jain 12 oct 2016
        /// Desc For getting colour count
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Description : Call Dal instead of Bal function to get data.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<NewBikeModelColor> GetModelColor(U modelId)
        {
            IEnumerable<NewBikeModelColor> objModelPage = null;
            string key = "BW_ModelColor_" + modelId;
            try
            {
                objModelPage = _cache.GetFromCache<IEnumerable<NewBikeModelColor>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetModelColor(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetModelColor");

            }

            return objModelPage;

        }
        /// <summary>
        /// Created by Subodh Jain 17 jan 2017
        /// Desc Get User Review Similar Bike
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Description : Call Dal instead of Bal function to get data.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeUserReviewRating> GetUserReviewSimilarBike(uint modelId, uint topCount)
        {
            IEnumerable<BikeUserReviewRating> objReviewUser = null;
            string key = string.Format("BW_UserReviewSimilarBike_ModelId_{0}_Topcount_{1}", modelId, topCount);
            try
            {
                objReviewUser = _cache.GetFromCache<IEnumerable<BikeUserReviewRating>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetUserReviewSimilarBike(modelId, topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(" BikeModelsCacheRepository.GetUserReviewSimilarBike_modelid_{0}_topcount_{1}", modelId, topCount));

            }
            return objReviewUser;
        }

        /// <summary>
        /// Written By : Sushil Kumar on 28th June 2016
        /// Summary : Function to get the upcoming bikes. If data is not available in the cache it will return data from BL.
        /// Modified by :   Sumit Kate on 08 Jul 2016
        /// Description :   Consider PageNo for Memcache key formation
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="curPageNo"></param>
        /// <returns>Returns List<UpcomingBikeEntity></returns>
        public IEnumerable<UpcomingBikeEntity> GetUpcomingBikesList(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null)
        {
            IEnumerable<UpcomingBikeEntity> objUpcoming = null;
            int recordCount = 0;
            int startIndex = 0, endIndex = 0, currentPageNo = 0;
            string key = string.Format("BW_UpcomingBikes_Cnt_{0}_SO_{1}", pageSize, (int)sortBy);
            currentPageNo = curPageNo.HasValue ? curPageNo.Value : 1;

            _objPager.GetStartEndIndex(pageSize, currentPageNo, out startIndex, out endIndex);

            UpcomingBikesListInputEntity inputParams = new UpcomingBikesListInputEntity()
            {
                PageNo = startIndex,
                PageSize = endIndex,
                MakeId = makeId.HasValue ? makeId.Value : 0,
                ModelId = modelId.HasValue ? modelId.Value : 0
            };

            if (makeId.HasValue && makeId.Value > 0)
                key += "_MK_" + makeId;

            if (modelId.HasValue && modelId.Value > 0)
                key += "_MO_" + modelId;
            if (curPageNo.HasValue && curPageNo.Value > 0)
            {
                key += "_PgNo_" + curPageNo.Value;
            }
            try
            {
                objUpcoming = _cache.GetFromCache<IEnumerable<UpcomingBikeEntity>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetUpcomingBikesList(inputParams, sortBy, out recordCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetUpcomingBikesList");

            }

            return objUpcoming;
        }

        /// <summary>
        /// Written By : Sushil Kumar on 28th June 2016
        /// Summary : Function to get the popular bikes by make . If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Returns BikeModelPageEntity</returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMake(uint makeId)
        {
            IEnumerable<MostPopularBikesBase> objBikes = null;
            try
            {
                string key = "BW_PopularBikesByMake_V1_" + makeId;
                objBikes = _cache.GetFromCache<IEnumerable<MostPopularBikesBase>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetMostPopularBikesByMake((int)makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");

            }

            return objBikes;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 29 Sep 2017 
        /// Description : Cache method for most popular bikes by make with city price if city is selected.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMakeWithCityPrice(int makeId, uint cityId)
        {
            IEnumerable<MostPopularBikesBase> objBikes = null;
            try
            {
                string key = string.Format("BW_PopularBikesByMakeWithCityPrice_V1_{0}_{1}", makeId, cityId);
                objBikes = _cache.GetFromCache(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetMostPopularBikesByMakeWithCityPrice(makeId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");

            }

            return objBikes;
        }
        /// <summary>
        /// Created by :Subodh Jain 22 sep 2013
        /// Des: method to get popular bike by make and city
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId)
        {
            IEnumerable<MostPopularBikesBase> objBikes = null;
            string key = "BW_PopularBikesByMake_V1_" + makeId + "_TC_" + topCount;
            if (cityId > 0)
                key = key + "_City_" + cityId;

            try
            {
                objBikes = _cache.GetFromCache(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetMostPopularBikesbyMakeCity(topCount, makeId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");

            }

            return objBikes;
        }

        /// <summary>
        /// Written By : Sushil Kumar on 30th June 2016
        /// Summary : Function to get the model dscription from the cache. If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Returns BikeDescriptionEntity</returns>
        public BikeDescriptionEntity GetModelSynopsis(U modelId)
        {
            BikeDescriptionEntity objModelPage = null;
            string key = "BW_ModelDesc_" + modelId;

            try
            {
                objModelPage = _cache.GetFromCache<BikeDescriptionEntity>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetModelSynopsis(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetModelSynopsis");

            }

            return objModelPage;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Jul 2016
        /// Description :   Returns New Launched Bike List
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex, int? makeid = null)
        {
            NewLaunchedBikesBase objBikes = null;
            try
            {
                string key = String.Format("BW_NewLaunchedBikes_V1_SI_{0}_EI_{1}", startIndex, endIndex);
                if (makeid.HasValue && makeid > 0)
                    key = key + String.Format("_MKID_{0}", makeid);
                if (makeid.HasValue && makeid > 0)
                    objBikes = _cache.GetFromCache(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetNewLaunchedBikesListByMake(startIndex, endIndex, makeid));
                else
                    objBikes = _cache.GetFromCache(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetNewLaunchedBikesList(startIndex, endIndex));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");

            }
            return objBikes;
        }
        public NewLaunchedBikesBase GetNewLaunchedBikesListByMake(int startIndex, int endIndex, int? makeid = null)
        {
            NewLaunchedBikesBase objBikes = null;
            string key = String.Format("BW_NewLaunchedBikes_SI_{0}_EI_{1}_MKID_{2}", startIndex, endIndex, makeid);

            try
            {
                objBikes = _cache.GetFromCache<NewLaunchedBikesBase>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetNewLaunchedBikesListByMake(startIndex, endIndex, makeid));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");

            }

            return objBikes;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Jul 2016
        /// Description :   GetMostPopularBikes Caching
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikes(int? topCount = null, int? makeId = null)
        {
            IEnumerable<MostPopularBikesBase> objBikes = null;
            try
            {
                string key = "BW_PopularBikes_V1" + (topCount.HasValue ? String.Format("_TC_{0}", topCount.Value) : "") + (makeId.HasValue ? String.Format("_MK_{0}", makeId.Value) : "");
                objBikes = _cache.GetFromCache(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetMostPopularBikes(topCount, makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");

            }

            return objBikes;
        }

        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Get details by version and city for review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="isAlreadyViewed"></param>
        public IEnumerable<ModelColorImage> GetModelColorPhotos(U modelId)
        {
            IEnumerable<ModelColorImage> objColorImages = null;
            string key = string.Format("BW_ModelPhotosColorWise_{0}", modelId);
            try
            {
                objColorImages = _cache.GetFromCache<IEnumerable<ModelColorImage>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetModelColorPhotos(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.Getmodelcolorphotos ==> ModelId: {0}", modelId));
            }
            return objColorImages;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 25 Jan 2017
        /// Summary    : Get body type of a bike model
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public EnumBikeBodyStyles GetBikeBodyType(uint modelId)
        {
            EnumBikeBodyStyles bodystyle = EnumBikeBodyStyles.AllBikes;
            string key = string.Format("BW_BikeBodyType_MO_{0}", modelId);
            try
            {
                bodystyle = _cache.GetFromCache<EnumBikeBodyStyles>(key, new TimeSpan(1, 0, 0), () => (EnumBikeBodyStyles)_modelRepository.GetBikeBodyType(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetBikeBodyType_ModelId {0}", modelId));
            }
            return bodystyle;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 25 Jan 2017
        /// Summary    : Get list of top popular bikes by category
        /// Modified by : Ashutosh Sharma on 29 Sep 2017.
        /// Description : Changed version of key from 'BW_PopularBikesListByBodyType_MO_' to 'BW_PopularBikesListByBodyType_MO_V1_'.
        /// </summary>
        /// <param name="bodyStyleId"></param>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ICollection<MostPopularBikesBase> GetMostPopularBikesByModelBodyStyle(int modelId, int topCount, uint cityId)
        {
            ICollection<MostPopularBikesBase> popularBikesList = null;
            string key = string.Format("BW_PopularBikesListByBodyType_MO_V1_{0}_city_{1}_topcount_{2}", modelId, cityId, topCount);
            try
            {
                popularBikesList = _cache.GetFromCache<Collection<MostPopularBikesBase>>(key, new TimeSpan(1, 0, 0), () => (Collection<MostPopularBikesBase>)_modelRepository.GetPopularBikesByModelBodyStyle(modelId, topCount, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetPopularBikesByBodyStyle: ModelId: {0},CityId {1}", modelId, cityId));

            }
            return popularBikesList;
        }
        /// <summary>
        /// Created by  :   Sushil Kumar on 2nd Jan 2016
        /// Description :   Calls DAL via Cache layer for generic bike info
        /// Modified By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public GenericBikeInfo GetBikeInfo(uint modelId)
        {
            GenericBikeInfo objSearchList = null;
            try
            {
                string key = string.Format("BW_GenericBikeInfo_MO_{0}", modelId);
                objSearchList = _cache.GetFromCache(key, new TimeSpan(23, 0, 0), () => _modelRepository.GetBikeInfo(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BestBikesCacheRepository.GetBikeInfo ModelId:{0}", modelId));
            }
            return objSearchList;
        }
        /// <summary>
        /// Created by  :   Subodh jain 9 Feb 2017
        /// Description :   Calls DAL via Cache layer for generic bike info
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public GenericBikeInfo GetBikeInfo(uint modelId, uint cityId)
        {
            GenericBikeInfo objSearchList = null;
            try
            {
                string key = string.Format("BW_GenericBikeInfo_V1_MO_{0}_cityId_{1}", modelId, cityId);
                TimeSpan cacheTime;
                if (cityId == 0)
                {
                    cacheTime = new TimeSpan(23, 0, 0);
                }
                else
                {
                    cacheTime = new TimeSpan(3, 0, 0);
                }
                objSearchList = _cache.GetFromCache(key, cacheTime, () => _modelRepository.GetBikeInfo(modelId, cityId));

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BestBikesCacheRepository.GetBikeInfo ModelId:{0} CityId:{1}", modelId, cityId));
            }
            return objSearchList;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 12 Jan 2017
        /// Description : To get bike rankings by category
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeRankingEntity GetBikeRankingByCategory(uint modelId)
        {
            string key = string.Format("BW_BikeRankingByModel_MO_{0}", modelId);
            BikeRankingEntity bikeRankObj = null;
            try
            {
                bikeRankObj = _cache.GetFromCache<BikeRankingEntity>(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetBikeRankingByCategory(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BestBikesCacheRepository.GetBikeRankingByCategory: ModelId:{0}", modelId));

            }
            return bikeRankObj;
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 17 Jan 2017
        /// Description : To get top 10 bikes of a given body style
        /// Modified by : Sajal Gupta on 02-02-2017
        /// Description : Passed cityid to get used bikes count. 
        /// Modified by : Ashutosh Sharma on 29 Sep 2017.
        /// Description : Changed version of key from 'BW_BestBikesByBodyStyle_' to 'BW_BestBikesByBodyStyle_V1'.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public ICollection<BestBikeEntityBase> GetBestBikesByCategory(EnumBikeBodyStyles bodyStyle, uint? cityId = null)
        {
            ICollection<BestBikeEntityBase> bestBikesList = null;
            try
            {
                string key = string.Format("BW_BestBikesByBodyStyle_V_{0}", bodyStyle);
                if (cityId != null)
                    key = string.Format("{0}_{1}", key, cityId.Value);
                bestBikesList = _cache.GetFromCache(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetBestBikesByCategory(bodyStyle, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BestBikesCacheRepository.GetBestBikesByCategory: BodyStyle:{0}", bodyStyle));
            }
            return bestBikesList;
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Sep 2017
        /// Description :   Fetches best bikes for particular model in its make
        /// </summary>
        /// <param name="bodyStyle"></param>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BestBikeEntityBase> GetBestBikesByModelInMake(uint modelId, uint? cityId = null)
        {
            IEnumerable<BestBikeEntityBase> bestBikesList = null;
            try
            {
                string key = string.Format("BW_BestBikesByModelInMake_V_{0}", modelId);
                if (cityId != null && cityId.Value > 0)
                {
                    key = string.Format("{0}_City_{1}", key, cityId.Value);
                    bestBikesList = _cache.GetFromCache(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetBestBikesByModelInMake(modelId, cityId.Value));
                }
                else
                    bestBikesList = _cache.GetFromCache(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetBestBikesByModelInMake(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BestBikesCacheRepository.GetBestBikesByCategory: ModelId:{0}", modelId));
            }
            return bestBikesList;
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 10 Feb 2017
        /// Description : To fetch model Image host and original image path
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public ModelHostImagePath GetModelPhotoInfo(U modelId)
        {
            string key = string.Format("BW_Model_HOST_IMG_{0}", modelId);
            ModelHostImagePath modelInfo = null;
            try
            {
                modelInfo = _cache.GetFromCache<ModelHostImagePath>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetModelPhotoInfo(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository<T, U>.GetModelPhotoInfo: ModelId:{0}", modelId));

            }
            return modelInfo;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   returns bikes list from Cache/DAL
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList()
        {
            IEnumerable<NewLaunchedBikeEntityBase> bikes = null;
            try
            {
                string key = "BW_NewLaunchedBikes_V1";
                bikes = _cache.GetFromCache(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetNewLaunchedBikesList());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.GetNewLaunchedBikesList");
            }
            return bikes;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 13 Feb 2017
        /// Description :   GetNewLaunchedBikesList by City
        /// Modified by : Sanskar on 12 Feb 2018
        /// Description : Cache key versioning
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList(uint cityId)
        {
            IEnumerable<NewLaunchedBikeEntityBase> bikes = null;
            try
            {
                string key = String.Format("BW_NewLaunchedBikes_CityId_{0}", cityId);
                bikes = _cache.GetFromCache(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetNewLaunchedBikesList(cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.GetNewLaunchedBikesList");
            }
            return bikes;
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 9 Mar 2017
        /// Summary    : Return list of popular scooters
        /// </summary>
        public IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint topCount, uint? cityId)
        {
            string key = String.Format("BW_MostPopularScooters_topCount_V1_{0}", topCount);
            if (cityId != null)
                key = string.Format("{0}_CityId_{1}", key, cityId.Value);
            IEnumerable<MostPopularBikesBase> popularScooters = null;
            try
            {
                popularScooters = _cache.GetFromCache<IEnumerable<MostPopularBikesBase>>(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetMostPopularScooters(topCount, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.GetMostPopularScooters");
            }
            return popularScooters;
        }
        /// <summary>
        /// Created by:- Subodh Jain 10 March 2017
        /// Summary :- Get comparision list of popular bike 
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint makeId)
        {
            IEnumerable<MostPopularBikesBase> popularBikesList = null;
            string key = string.Format("BW_GetMostPopularScooters_V1_MK_{0}", makeId);
            try
            {
                popularBikesList = _cache.GetFromCache<IEnumerable<MostPopularBikesBase>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetMostPopularScooters(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetMostPopularScooters: MakeId: {0}", makeId));

            }
            return popularBikesList;
        }
        /// <summary>
        /// Created By :- Subodh Jain 07-12-2017
        /// Summary :- Method to GetElectricBikes 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetElectricBikes()
        {
            IEnumerable<MostPopularBikesBase> popularBikesList = null;
            try
            {
                string key = string.Format("BW_ElectricBikes_V1");
                popularBikesList = _cache.GetFromCache(key, new TimeSpan(24, 0, 0), () => _modelRepository.GetElectricBikes());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetElectricBikes"));
            }
            return popularBikesList;
        }

        /// <summary>
        /// Created By :- Subodh Jain 07-12-2017
        /// Summary :- Method to GetElectricBikes 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetElectricBikes(uint cityId)
        {
            IEnumerable<MostPopularBikesBase> popularBikesList = null;
            try
            {
                string key = string.Format("BW_ElectricBikes_V1_CityId_{0}", cityId);
                popularBikesList = _cache.GetFromCache(key, new TimeSpan(24, 0, 0), () => _modelRepository.GetElectricBikes(cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetElectricBikes"));

            }
            return popularBikesList;
        }

        public IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint topCount, uint makeId, uint cityId)
        {
            IEnumerable<MostPopularBikesBase> popularBikesList = null;
            string key = string.Format("BW_GetMostPopularScooters_V1_MK_{0}_CID_{1}_TC_{2}", makeId, cityId, topCount);
            try
            {
                popularBikesList = _cache.GetFromCache<IEnumerable<MostPopularBikesBase>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetMostPopularScooters(topCount, makeId, cityId));

            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetMostPopularScooters({0},{1},{2})", makeId, cityId, topCount));

            }
            return popularBikesList;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 18-Aug-2017
        /// Description : Cache method to get most popular bikes by body style.
        /// </summary>
        /// <param name="bodyStyleId"></param>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetPopularBikesByBodyStyle(ushort bodyStyleId, uint topCount, uint cityId)
        {
            IEnumerable<MostPopularBikesBase> popularBikesList = null;
            string key = string.Empty;


            try
            {
                if (cityId > 0)
                {

                    key = string.Format("BW_PopularBikesListByBodyType_Bodystyle_{0}_City_{1}", bodyStyleId, cityId);
                    popularBikesList = _cache.GetFromCache<IEnumerable<MostPopularBikesBase>>(key, new TimeSpan(1, 0, 0), () => (IEnumerable<MostPopularBikesBase>)_modelRepository.GetPopularBikesByBodyStyle(bodyStyleId, topCount, cityId));
                }
                else
                {

                    key = string.Format("BW_PopularBikesListByBodyType_Bodystyle_{0}", bodyStyleId);
                    popularBikesList = _cache.GetFromCache<IEnumerable<MostPopularBikesBase>>(key, new TimeSpan(24, 0, 0), () => (IEnumerable<MostPopularBikesBase>)_modelRepository.GetPopularBikesByBodyStyle(bodyStyleId, topCount, cityId));
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetPopularBikesByBodyStyle: BodyStyleId: {0}, topCount: {1}, CityId {2}", bodyStyleId, topCount, cityId));

            }
            return popularBikesList;
        }

        /// <summary>
        /// Created By:Snehal Dange on 3rd Nov 2017
        /// Description: Cache method to get mileage info for a bike
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeMileageEntity GetMileageDetails()
        {
            BikeMileageEntity mileageInfo = null;
            string key = string.Empty;
            try
            {
                key = "BW_BikesByMileage";
                mileageInfo = _cache.GetFromCache<BikeMileageEntity>(key, new TimeSpan(24, 0, 0), () => _modelRepository.GetMileageForModel());

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetMileageDetails");

            }
            return mileageInfo;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 28th Nov 2017
        /// Description : Cache layer for getting series entity for given model id
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeSeriesEntityBase GetSeriesByModelId(uint modelId)
        {
            BikeSeriesEntityBase objSeries = null;
            string key = string.Empty;
            try
            {
                key = string.Format("BW_SeriesForModel_{0}", modelId);
                objSeries = _cache.GetFromCache<BikeSeriesEntityBase>(key, new TimeSpan(24, 0, 0), () => _modelRepository.GetSeriesByModelId(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetSeriesByModelId modelId = {0}", modelId));
            }
            return objSeries;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 11th Jan 2018
        /// Description : Get Model Ids with there bodystyle
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ModelIdWithBodyStyle> GetModelIdsForImages()
        {
            IEnumerable<ModelIdWithBodyStyle> modelIdsWithBodyStyle = null;
            try
            {
                modelIdsWithBodyStyle = _cache.GetFromCache<IEnumerable<ModelIdWithBodyStyle>>(_modelIdsWithBodyStyleKey, new TimeSpan(24, 0, 0), () => _modelRepository.GetModelIdsForImages());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.BikeData.BikeModelsCacheRepoostiory.GetModelIdsForImages");
            }
            return modelIdsWithBodyStyle;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 15 Jan 2018
        /// Description :   Returns Model Images with color photos from cache
        /// </summary>
        /// <param name="modelIds"></param>
        /// <returns></returns>
        public ICollection<BikeModelColorImageEntity> GetModelImages(string modelIds)
        {
            ICollection<BikeModelColorImageEntity> modelIdsWithBodyStyle = null;
            string key = "BW_ModelImages_{0}";
            try
            {
                var modelIdsArray = Array.ConvertAll(modelIds.Split(','), int.Parse);
                Array.Sort(modelIdsArray);
                key = String.Format(key, String.Join(",", modelIdsArray).Replace(",", "_"));

                modelIdsWithBodyStyle = _cache.GetFromCache<ICollection<BikeModelColorImageEntity>>(key, new TimeSpan(24, 0, 0), () => _modelRepository.GetModelImages(modelIds));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.BikeData.BikeModelsCacheRepoostiory.GetModelIdsForImages");
            }
            return modelIdsWithBodyStyle;
        }
        /// <summary>
        /// Created By  : Sanskar Gupta on 12 Jan 2018
        /// Description : Cache Layer for getting all the promoted bikes
        /// </summary>
        /// <param name="bikeFilters"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetAdPromotedBike(BikeFilters bikeFilters)
        {
            IEnumerable<MostPopularBikesBase> mostPopularBikes = null;
            try
            {
                string key = string.Format("BW_AdPromotedBike_V1_MakeId_{0}_CityId_{1}", bikeFilters.MakeId, bikeFilters.CityId);
                mostPopularBikes = _cache.GetFromCache(key, new TimeSpan(24, 0, 0), () => _modelRepository.GetAdPromotedBike(bikeFilters));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetAdPromotedBike"));
            }
            return mostPopularBikes;
        }
        public IEnumerable<MostPopularBikesBase> GetAdPromotedBikeWithOutCity(BikeFilters bikeFilters)
        {
            IEnumerable<MostPopularBikesBase> mostPopularBikes = null;
            try
            {
                string key = "BW_AdPromotedBike_V1";
                mostPopularBikes = _cache.GetFromCache(key, new TimeSpan(24, 0, 0), () => _modelRepository.GetAdPromotedBikeWithOutCity(bikeFilters));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetAdPromotedBike"));
            }
            return mostPopularBikes;
        }

    }
}
