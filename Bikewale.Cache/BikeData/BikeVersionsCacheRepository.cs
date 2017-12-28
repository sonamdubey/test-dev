using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.BikeData
{

    /// <summary>
    /// Created by  :    Sushil Kumar on 28th June 2016
    /// Description :   Bike Versions Repository Cache
    /// </summary>
    public class BikeVersionsCacheRepository<T, U> : IBikeVersionCacheRepository<T, U>
    {
        private readonly ICacheManager _cache;
        private readonly IBikeVersions<T, U> _objVersions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objMakes"></param>
        public BikeVersionsCacheRepository(ICacheManager cache, IBikeVersions<T, U> objVersions)
        {
            _cache = cache;
            _objVersions = objVersions;
        }


        /// <summary>
        /// Created by  :    Sushil Kumar on 28th June 2016
        /// Summary     :   Gets the Similar Bikes  list
        /// Modified by : Ashutosh Sharma on 03 Oct 2017
        /// Description : Changed key from 'BW_SimilarBikes_' to 'BW_SimilarBikes_V1_'.
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public IEnumerable<Entities.BikeData.SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint cityid)
        {
            IEnumerable<Entities.BikeData.SimilarBikeEntity> versions = null;
            string key = String.Format("BW_SimilarBikes_V1_{0}_Cnt_{1}_{2}", versionId, topCount, cityid);
            try
            {
                TimeSpan cacheTime = new TimeSpan(3, 0, 0);
                if (cityid == 0)
                {
                    cacheTime = new TimeSpan(23, 0, 0);
                }
                versions = _cache.GetFromCache<IEnumerable<Entities.BikeData.SimilarBikeEntity>>(key, cacheTime, () => _objVersions.GetSimilarBikesList(versionId, topCount, cityid));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetSimilarBikesList");

            }
            return versions;
        }


        /// <summary>
        /// Created by  :    Sushil Kumar on 28th June 2016
        /// Summary     :   Gets the versions by type and modelid and cityId
        /// Modified by :   Sumit Kate on 16 Feb 2017
        /// Description :   Consider City Id for memcache key
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null)
        {
            List<BikeVersionsListEntity> versions = null;
            int timeSpan = 24;

            string key = String.Format("BW_VersionsByType_{0}_MO_{1}", (int)requestType, modelId);

            if (cityId.HasValue && cityId.Value > 0)
            {
                key = string.Format("{0}_CI_{1}", key, cityId);
                timeSpan = 1;
            }
            try
            {
                versions = _cache.GetFromCache<List<BikeVersionsListEntity>>(key, new TimeSpan(timeSpan, 0, 0), () => _objVersions.GetVersionsByType(requestType, modelId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetVersionsByType");

            }
            return versions;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 28th June 2016
        /// Summary     : Gets the version details
        /// Modified By: Snehal Dange on 13th Oct 2017
        /// Description : - Versioned the cache key
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public T GetById(U versionId)
        {
            T versionDetails = default(T);
            string key = String.Format("BW_VersionDetails_{0}_V1", versionId);
            try
            {
                versionDetails = _cache.GetFromCache<T>(key, new TimeSpan(1, 0, 0), () => _objVersions.GetById(versionId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetById");

            }
            return versionDetails;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 28th June 2016
        /// Summary     : Gets version minspecs
        /// Modified by : Ashutosh Sharma on 03 Oct 2017
        /// Description :  Changed key from 'BW_VersionMinSpecs_' to 'BW_VersionMinSpecs_V1_'.
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew)
        {
            List<BikeVersionMinSpecs> versions = null;
            string key = String.Format("BW_VersionMinSpecs_V1_{0}_New_{1}", modelId, isNew);
            try
            {
                versions = _cache.GetFromCache<List<BikeVersionMinSpecs>>(key, new TimeSpan(1, 0, 0), () => _objVersions.GetVersionMinSpecs(modelId, isNew));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetVersionMinSpecs");

            }
            return versions;
        }
        /// <summary>
        /// Created By: Aditi Srivastava on 17 Oct 2016
        /// Summary :   Get version colors
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public IEnumerable<BikeColorsbyVersion> GetColorsbyVersionId(uint versionId)
        {
            IEnumerable<BikeColorsbyVersion> versionColors = null;
            string key = String.Format("BW_VersionColor_{0}", versionId);
            try
            {
                versionColors = _cache.GetFromCache<IEnumerable<BikeColorsbyVersion>>(key, new TimeSpan(1, 0, 0), () => _objVersions.GetColorsbyVersionId(versionId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikeVersionsCacheRepository.GetColorsbyVersionId: {0}", versionId));

            }
            return versionColors;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 26 Dec 2017
        /// Description : Cache method to get all specifications of a version.
        /// </summary>
        /// <param name="versionId">Version Id of bike version for which specifications are to be fetched.</param>
        /// <returns>Specs and features of a version.</returns>
        public TransposeModelSpecEntity GetSpecifications(U versionId)
        {
            try
            {
                string key = string.Format("BW_SpecsFeatures_version_{0}", versionId);
                return _cache.GetFromCache(key, new TimeSpan(1, 0, 0), () => GetTransposedSpecs(versionId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikeVersionsCacheRepository.GetSpecifications_{0}", versionId));
            }
            return null;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 26 Dec 2017
        /// Description : Method to get transposed specs and features.
        /// </summary>
        /// <param name="versionId">Version Id of bike version for which specifications are to be transposed.</param>
        /// <returns>Transposed specs and features of a version.</returns>
        private TransposeModelSpecEntity GetTransposedSpecs(U versionId)
        {
            TransposeModelSpecEntity transposeModelSpec = null;
            try
            {
                transposeModelSpec = new TransposeModelSpecEntity();
                BikeSpecificationEntity objSpecs = _objVersions.GetSpecifications(versionId);
                if (objSpecs != null)
                {
                    transposeModelSpec.BikeVersionId = Convert.ToUInt32(versionId);
                    transposeModelSpec.objFeatures = FetchFeatures(objSpecs);
                    transposeModelSpec.objSpecs = FetchSpecList(objSpecs);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikeVersionsCacheRepository.GetTransposedSpecs{0}", versionId));
            }
            return transposeModelSpec;
        }
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
        /// Gets the dealer versions by model.
        /// </summary>
        /// <param name="dealerId">The dealer identifier.</param>
        /// <param name="modelId">The model identifier.</param>
        /// <returns></returns>
        public IEnumerable<BikeVersionWithMinSpec> GetDealerVersionsByModel(uint dealerId, uint modelId)
        {
            IEnumerable<BikeVersionWithMinSpec> versions = null;
            string key = String.Format("BW_Versions_Dealer_{0}_Model_{1}", dealerId, modelId);
            try
            {
                versions = _cache.GetFromCache<IEnumerable<BikeVersionWithMinSpec>>(key, new TimeSpan(0, 30, 0), () => _objVersions.GetDealerVersionsByModel(dealerId, modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeMakesCacheRepository.GetDealerVersionsByModel: DealerId: {0}, ModelId: {1}", dealerId, modelId));

            }
            return versions;
        }
    }
}
