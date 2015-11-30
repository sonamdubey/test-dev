using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using System.Collections;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Photos;
using System.Configuration;
using Bikewale.Notifications;

namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Apr 2014
    /// Summary : 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeModels<T,U> : IBikeModels<T,U> where T : BikeModelEntity, new()
    {
        private readonly IBikeModelsRepository<T, U> modelRepository = null;

        public BikeModels()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModelsRepository<T, U>, BikeModelsRepository<T, U>>();
                modelRepository = container.Resolve<IBikeModelsRepository<T, U>>();
            }
        }

        public List<BikeModelEntityBase> GetModelsByType(EnumBikeType requestType, int makeId)
        {
            List<BikeModelEntityBase> objModelList = null;

            objModelList = modelRepository.GetModelsByType(requestType, makeId);

            return objModelList;
        }

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 20 Aug 
        /// Summary : to retrieve version list for new as well as discontinued bikes
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public List<BikeVersionsListEntity> GetVersionsList(U modelId,bool isNew)
        {
            List<BikeVersionsListEntity> objVersionList = null;

            objVersionList = modelRepository.GetVersionsList(modelId, isNew);

            return objVersionList;
        }

        public BikeDescriptionEntity GetModelSynopsis(U modelId)
        {
            BikeDescriptionEntity objDesc = null;

            objDesc = modelRepository.GetModelSynopsis(modelId);

            return objDesc;
        }

        public UpcomingBikeEntity GetUpcomingBikeDetails(U modelId)
        {
            UpcomingBikeEntity objUpcomingBike = null;

            objUpcomingBike = modelRepository.GetUpcomingBikeDetails(modelId);

            return objUpcomingBike;
        }

        public U Add(T t)
        {
            throw new NotImplementedException();
        }

        public bool Update(T t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(U id)
        {
            T t = modelRepository.GetById(id);

            return t;
        }

        /// <summary>
        /// Created By : Ashish G. Kamble on 12 May 2014
        /// Summary : 
        /// </summary>
        /// <param name="inputParams">Start Index and End Index are mandetory.</param>
        /// <param name="sortBy">Optional. To get all results set default.</param>
        /// <param name="recordCount">Record count</param>
        /// <returns></returns>
        public List<UpcomingBikeEntity> GetUpcomingBikesList(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy, out int recordCount)
        {
            List<UpcomingBikeEntity> objUpcomingBikeList = null;

            objUpcomingBikeList = modelRepository.GetUpcomingBikesList(inputParams, sortBy, out recordCount);

            return objUpcomingBikeList;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 4 June 2014
        /// Summary : To get all recently launched bikes
        /// </summary>
        /// <param name="startIndex">Start Index</param>
        /// <param name="endIndex">End Index</param>
        /// <param name="recordCount">Record Count</param>
        /// <returns></returns>
        public List<NewLaunchedBikeEntity> GetNewLaunchedBikesList(int startIndex, int endIndex, out int recordCount)
        {
            List<NewLaunchedBikeEntity> objNewLaunchedBikeList = null;

            objNewLaunchedBikeList = modelRepository.GetNewLaunchedBikesList(startIndex, endIndex, out recordCount);

            return objNewLaunchedBikeList;
        }


        public Hashtable GetMaskingNames()
        {
            throw new NotImplementedException();
        }

        public Hashtable GetOldMaskingNames()
        {
            throw new NotImplementedException();
        }


        public List<FeaturedBikeEntity> GetFeaturedBikes(uint topRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 7 Oct 2015
        /// Summary : Function to get the model page details
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeModelPageEntity GetModelPageDetails(U modelId)
        {
            BikeModelPageEntity objModelPage = null;            

            try
            {
                objModelPage = modelRepository.GetModelPage(modelId);

                if (objModelPage != null && objModelPage.ModelVersionSpecs != null)
                {                    
                    #region Set Overview
                    objModelPage.objOverview = new Overview()
                    {
                        DisplayName = "Overview"
                    };

                    List<Specs> objOverviewSpecs = new List<Specs>();

                    objOverviewSpecs.Add(new Specs()
                    {
                        DisplayText = "Capacity",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Displacement, "cc")
                    });

                    objOverviewSpecs.Add(new Specs()
                    {
                        DisplayText = "Mileage",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FuelEfficiencyOverall, "kmpl")
                    });

                    objOverviewSpecs.Add(new Specs()
                    {
                        DisplayText = "Max power",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.MaxPower, "PS")
                    });

                    objOverviewSpecs.Add(new Specs()
                    {
                        DisplayText = "Weight",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.KerbWeight, "kg")
                    });

                    objModelPage.objOverview.OverviewList = objOverviewSpecs;

                    #endregion

                    #region Set Specifications
                    objModelPage.objSpecs = new Specifications()
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
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Displacement, "cc")
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Max Power",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.MaxPower, "bhp", objModelPage.ModelVersionSpecs.MaxPowerRPM, "rpm")
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Maximum Torque",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.MaximumTorque, "Nm", objModelPage.ModelVersionSpecs.MaximumTorqueRPM, "rpm")
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "No. of gears",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.NoOfGears)
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Fuel Efficiency",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FuelEfficiencyOverall, "kmpl")
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Brake Type",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.BrakeType)
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Front Disc",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FrontDisc)
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Rear Disc",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.RearDisc)
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Alloy Wheels",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.AlloyWheels)
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Kerb Weight",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.KerbWeight, "kg")
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Chassis Type",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.ChassisType)
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Top Speed",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.TopSpeed, "kmph")
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Tubeless Tyres",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.TubelessTyres)
                    });

                    objSummarySpecs.Add(new Specs()
                    {
                        DisplayText = "Fuel Tank Capacity",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FuelTankCapacity, "litres")
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
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Displacement, "cc")
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Cylinders",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Cylinders)
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Max Power",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.MaxPower, "bhp", objModelPage.ModelVersionSpecs.MaxPowerRPM, "rpm")
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Maximum Torque",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.MaximumTorque, "Nm", objModelPage.ModelVersionSpecs.MaximumTorqueRPM, "rpm")
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Bore",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Bore, "mm")
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Stroke",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Stroke, "mm")
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Valves Per Cylinder",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.ValvesPerCylinder)
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Fuel Delivery System",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FuelDeliverySystem)
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Fuel Type",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FuelType)
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Ignition",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Ignition)
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Spark Plugs",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.SparkPlugsPerCylinder, "Per Cylinder")
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Cooling System",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.CoolingSystem)
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Gearbox Type",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.GearboxType)
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "No. of Gears",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.NoOfGears)
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Transmission Type",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.TransmissionType)
                    });

                    objEngTransSpecs.Add(new Specs()
                    {
                        DisplayText = "Clutch",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Clutch)
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
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.BrakeType)
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Front Disc",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FrontDisc)
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Front Disc/Drum Size",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FrontDisc_DrumSize, "mm")
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Rear Disc",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.RearDisc)
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Rear Disc/Drum Size",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.RearDisc_DrumSize, "mm")
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Calliper Type",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.CalliperType)
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Wheel Size",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.WheelSize, "inches")
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Front Tyre",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FrontTyre)
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Rear Tyre",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.RearTyre)
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Tubeless Tyres",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.TubelessTyres)
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Radial Tyres",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.RadialTyres)
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Alloy Wheels",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.AlloyWheels)
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Front Suspension",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FrontSuspension)
                    });

                    objBrakesWheelsSpecs.Add(new Specs()
                    {
                        DisplayText = "Rear Suspension",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.RearSuspension)
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
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.KerbWeight, "kg")
                    });

                    objChassisSpecs.Add(new Specs()
                    {
                        DisplayText = "Overall Length",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.OverallLength, "mm")
                    });

                    objChassisSpecs.Add(new Specs()
                    {
                        DisplayText = "Overall Width",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.OverallWidth, "mm")
                    });

                    objChassisSpecs.Add(new Specs()
                    {
                        DisplayText = "Overall Height",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.OverallHeight, "mm")
                    });

                    objChassisSpecs.Add(new Specs()
                    {
                        DisplayText = "Wheelbase",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Wheelbase, "mm")
                    });

                    objChassisSpecs.Add(new Specs()
                    {
                        DisplayText = "Ground Clearance",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.GroundClearance, "mm")
                    });

                    objChassisSpecs.Add(new Specs()
                    {
                        DisplayText = "Seat Height",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.SeatHeight, "mm")
                    });

                    objChassisSpecs.Add(new Specs()
                    {
                        DisplayText = "Chassis Type",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.ChassisType)
                    });

                    objChassis.Specs = objChassisSpecs;

                    // Add fuel efficiency and performance
                    SpecsCategory objFuel = new SpecsCategory() 
                    {
                        CategoryName = "FuelEffieciency",
                        DisplayName = "Fuel effieciency and Performance"
                    };

                    List<Specs> objFuelSpecs = new List<Specs>();

                    objFuelSpecs.Add(new Specs()
                    {
                        DisplayText = "Fuel Tank Capacity",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FuelTankCapacity, "litres")
                    });

                    objFuelSpecs.Add(new Specs()
                    {
                        DisplayText = "Reserve Fuel Capacity",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.ReserveFuelCapacity, "litres")
                    });

                    objFuelSpecs.Add(new Specs()
                    {
                        DisplayText = "Fuel Efficiency Overall",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FuelEfficiencyOverall, "kmpl")
                    });

                    objFuelSpecs.Add(new Specs()
                    {
                        DisplayText = "Fuel Efficiency Range",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FuelEfficiencyRange, "km")
                    });

                    objFuelSpecs.Add(new Specs()
                    {
                        DisplayText = "0 to 60 kmph",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Performance_0_60_kmph, "seconds")
                    });

                    objFuelSpecs.Add(new Specs()
                    {
                        DisplayText = "0 to 80 kmph",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Performance_0_80_kmph, "seconds")
                    });

                    objFuelSpecs.Add(new Specs()
                    {
                        DisplayText = "0 to 40 kmph",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Performance_0_40_m, "seconds")
                    });

                    objFuelSpecs.Add(new Specs()
                    {
                        DisplayText = "Top Speed",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.TopSpeed, "kmph")
                    });

                    objFuelSpecs.Add(new Specs()
                    {
                        DisplayText = "60 to 0 kmph",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Performance_60_0_kmph)
                    });

                    objFuelSpecs.Add(new Specs()
                    {
                        DisplayText = "80 to 0 kmph",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Performance_80_0_kmph)
                    });

                    objFuel.Specs = objFuelSpecs;

                    // Add specification categories to the specs object
                    objSpecifications.Add(objSummary);
                    objSpecifications.Add(objEngTrans);
                    objSpecifications.Add(objBrakesWheels);
                    objSpecifications.Add(objChassis);
                    objSpecifications.Add(objFuel);

                    objModelPage.objSpecs.SpecsCategory = objSpecifications;                    

                    #endregion
                    
                    #region Set features
                    objModelPage.objFeatures = new Features()
                    {
                        DisplayName = "Features"
                    };                        

                    List<Specs> objFeatuesList = new List<Specs>();

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Speedometer",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Speedometer)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Fuel Guage",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.FuelGauge)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Tachometer Type",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.TachometerType)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Digital Fuel Guage",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.DigitalFuelGauge)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Tripmeter",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Tripmeter)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Electric Start",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.ElectricStart)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Tachometer",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Tachometer)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Shift Light",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.ShiftLight)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "No. of Tripmeters",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.NoOfTripmeters)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Tripmeter Type",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.TripmeterType)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Low Fuel Indicator",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.LowFuelIndicator)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Low Oil Indicator",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.LowOilIndicator)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Low Battery Indicator",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.LowBatteryIndicator)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Pillion Seat",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.PillionSeat)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Pillion Footrest",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.PillionFootrest)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Pillion Backrest",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.PillionBackrest)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Pillion Grabrail",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.PillionGrabrail)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Stand Alarm",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.StandAlarm)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Stepped Seat",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.SteppedSeat)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Antilock Braking System",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.AntilockBrakingSystem)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Killswitch",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Killswitch)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Clock",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Clock)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Electric System",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.ElectricSystem)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Battery",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Battery)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Headlight Type",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.HeadlightType)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Headlight Bulb Type",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.HeadlightBulbType)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Brake/Tail Light",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.Brake_Tail_Light)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Turn Signal",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.TurnSignal)
                    });

                    objFeatuesList.Add(new Specs()
                    {
                        DisplayText = "Pass Light",
                        DisplayValue = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objModelPage.ModelVersionSpecs.PassLight)
                    });

                    objModelPage.objFeatures.FeaturesList = objFeatuesList;

                    #endregion                    
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.BAL.BikeData.GetModelPageDetails");
                objErr.SendMail();                
            }

            return objModelPage;
        }

    }   // Class
}   // namespace
