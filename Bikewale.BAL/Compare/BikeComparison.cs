using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bikewale.Cache.Compare;
using Bikewale.Cache.Core;
using Bikewale.DAL.Compare;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Bikewale.Utility;
using log4net;
using Microsoft.Practices.Unity;

namespace Bikewale.BAL.Compare
{
    /// <summary>
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Added methods for featured bike and sponsored bike comparisions
    /// Modified By : Sushil Kumar on 2nd Feb 2017
    /// Description : Added methods for comparisions bikes binding using transpose methodology
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added cache call for comparison carousel for further processing in element order
    /// </summary>
    public class BikeComparison : IBikeCompare
    {
        private readonly IBikeCompare _objCompare = null;
        private readonly IBikeCompareCacheRepository _objCache = null;

        static bool _logGrpcErrors = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.LogGrpcErrors);
        static readonly ILog _logger = LogManager.GetLogger(typeof(BikeComparison));
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);

        private readonly string _defaultMinSpecs = FormatMinSpecs.ShowAvailable("");
        /// <summary>
        /// Modified by : Aditi Srivastava on 5 June 2017
        /// Summary     : Resolution for cache layer functions
        /// </summary>
        public BikeComparison()
        {
            using (IUnityContainer objPQCont = new UnityContainer())
            {
                objPQCont.RegisterType<IBikeCompare, BikeCompareRepository>();
                objPQCont.RegisterType<ICacheManager, MemcacheManager>();
                objPQCont.RegisterType<IBikeCompareCacheRepository, BikeCompareCacheRepository>();
                _objCompare = objPQCont.Resolve<IBikeCompare>();
                _objCache = objPQCont.Resolve<IBikeCompareCacheRepository>();

            }
        }

        public Entities.Compare.BikeCompareEntity DoCompare(string versions)
        {
            Entities.Compare.BikeCompareEntity compareEntity = null;
            try
            {
                if (!string.IsNullOrEmpty(versions))
                {
                    compareEntity = _objCompare.DoCompare(versions);
                    TransposeCompareBikeData(ref compareEntity, versions);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DoCompare");
                
            }
            return compareEntity;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Feb 2017
        /// Description : Removed transpose logic from DoCompare and moved into common for both app and website
        /// Modified by : Aditi Srivastava on 18 May 2017
        /// Summary     : used nullable bool function to format specs and features
        /// Modified by : Vivek Singh Tomar on 20th Dec 2017
        /// Summary     : Optimization of TransposeCompareBikeData method reduced extra operations
        /// </summary>
        /// <param name="compareEntity"></param>
        /// <param name="versions"></param>
        private void TransposeCompareBikeData(ref BikeCompareEntity compareEntity, string versions)
        {
            try
            {
                string[] arrVersion = versions.Split(',');

                if (compareEntity != null)
                {
                    CompareMainCategory compareSpecifications = GetCompareMainCategory(BWConstants.Specifications);

                    #region Specifications
                    CompareSubMainCategory engineTransmission = GetEngineAndTransmission(compareEntity, arrVersion);
                    compareSpecifications.Spec.Add(engineTransmission);

                    CompareSubMainCategory brakesWheelsSuspension = GetBrakeWheelSuspension(compareEntity, arrVersion);
                    compareSpecifications.Spec.Add(brakesWheelsSuspension);

                    CompareSubMainCategory dimensionChasis = GetDimensionsAndChasis(compareEntity, arrVersion);
                    compareSpecifications.Spec.Add(dimensionChasis);

                    CompareSubMainCategory fuelEfficiencyPerformance = GetFuelEfficiencyPerformance(compareEntity, arrVersion);
                    compareSpecifications.Spec.Add(fuelEfficiencyPerformance);

                    compareEntity.CompareSpecifications = compareSpecifications;

                    #endregion

                    CompareMainCategory compareFeatures = GetCompareMainCategory(BWConstants.Features);

                    #region Features
                    CompareSubMainCategory features = GetFeatures(compareEntity, arrVersion);
                    compareFeatures.Spec.Add(features);
                    compareEntity.CompareFeatures = compareFeatures;
                    #endregion

                    CompareBikeColorCategory compareColors = GetCompareColors(compareEntity, arrVersion);
                    compareEntity.CompareColors = compareColors;


                    if (compareEntity.Reviews != null && compareEntity.Reviews.Any())
                    {
                        #region Reviews
                        CompareReviewsData userReviewData = new CompareReviewsData();
                        CompareMainCategory compareReviews = GetCompareMainCategory(BWConstants.Reviews);


                        IList<UserReviewComparisonObject> objReviewList = new List<UserReviewComparisonObject>();

                        CompareSubMainCategory ratings = GetCompareSubMainCategory(BWConstants.Ratings);

                        foreach (var version in arrVersion)
                        {
                            UserReviewComparisonObject objReview = new UserReviewComparisonObject();
                            var reviewObj = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var basicInfoObj = compareEntity.BasicInfo != null ? compareEntity.BasicInfo.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)) : null;
                            if (reviewObj != null && reviewObj.ModelReview != null)
                            {
                                var modelReview = reviewObj.ModelReview;
                                objReview.ReviewRate = FormatMinSpecs.ShowAvailable(modelReview.ReviewRate.ToString("0.0"));
                                objReview.RatingCount = FormatMinSpecs.ShowAvailable(modelReview.RatingCount);
                                objReview.ReviewCount = FormatMinSpecs.ShowAvailable(modelReview.ReviewCount);

                                if (basicInfoObj != null && modelReview.UserReviews != null)
                                    objReview.ReviewListUrl = string.Format("/{0}-bikes/{1}/reviews/", basicInfoObj.MakeMaskingName, basicInfoObj.ModelMaskingName);
                            }
                            else
                            {
                                objReview.ReviewRate = objReview.RatingCount = objReview.ReviewCount  = _defaultMinSpecs;
                            }
                            objReviewList.Add(objReview);
                        }
                        userReviewData.OverallRating = objReviewList;

                        //Mileage
                        CompareSubCategory mileage = GetCompareSubCategory(BWConstants.Mileage);

                        int isValuesPresent = 0;
                        foreach (var version in arrVersion)
                        {
                            var reviewsObj = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            if (reviewsObj != null && reviewsObj.ModelReview != null)
                            {
                                var modelReview = reviewsObj.ModelReview;
                                mileage.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(modelReview.Mileage)));

                                if (isValuesPresent <= 1 && modelReview.Mileage > 0)
                                {
                                    ++isValuesPresent;
                                }
                            }
                            else
                                mileage.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(_defaultMinSpecs)));
                        }
                        if (isValuesPresent > 1)
                        {
                            ratings.SpecCategory = new List<CompareSubCategory> { mileage };
                        }
                        #endregion
                        compareReviews.Spec.Add(ratings);
                        #region Performance Paramters
                        CompareSubMainCategory performanceParameters = GetCompareSubMainCategory(BWConstants.PerformanceParameters);
                        //visual Appeal
                        CompareSubCategory visualAppeal = GetCompareSubCategory(BWConstants.VisualAppeal);
                        isValuesPresent = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 4) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                visualAppeal.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue)));

                                if (isValuesPresent <= 1 && objQuestion.AverageRatingValue > 0)
                                {
                                    ++isValuesPresent;
                                }
                            }
                            else
                                visualAppeal.CompareSpec.Add(GetCompareBikeData(_defaultMinSpecs));
                        }
                        if (isValuesPresent > 1)
                        {
                            performanceParameters.SpecCategory = new List<CompareSubCategory> { visualAppeal };
                        }
                        // Reliability
                        CompareSubCategory reliability = GetCompareSubCategory(BWConstants.Reliablity);
                        isValuesPresent = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 5) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                reliability.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue)));

                                if (isValuesPresent <= 1 && objQuestion.AverageRatingValue > 0)
                                {
                                    ++isValuesPresent;
                                }
                            }
                            else
                                reliability.CompareSpec.Add(GetCompareBikeData(_defaultMinSpecs));
                        }
                        if (isValuesPresent > 1)
                        {
                            performanceParameters.SpecCategory.Add(reliability);
                        }
                        // Performance
                        CompareSubCategory performance = GetCompareSubCategory(BWConstants.Performance);
                        isValuesPresent = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 6) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                performance.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue)));

                                if (isValuesPresent <= 1 && objQuestion.AverageRatingValue > 0)
                                {
                                    ++isValuesPresent;
                                }
                            }
                            else
                                performance.CompareSpec.Add(GetCompareBikeData(_defaultMinSpecs));
                        }
                        if (isValuesPresent > 1)
                        {
                            performanceParameters.SpecCategory.Add(performance);
                        }
                        // Comfort
                        CompareSubCategory comfort = GetCompareSubCategory(BWConstants.Comfort);
                        isValuesPresent = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 7) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                comfort.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue)));

                                if (isValuesPresent <= 1 && objQuestion.AverageRatingValue > 0)
                                {
                                    ++isValuesPresent;
                                }
                            }
                            else
                                comfort.CompareSpec.Add(GetCompareBikeData(_defaultMinSpecs));
                        }
                        if (isValuesPresent > 1)
                        {
                            performanceParameters.SpecCategory.Add(comfort);
                        }
                        // Service Experience
                        CompareSubCategory serviceExperience = GetCompareSubCategory(BWConstants.ServiceExperience);
                        isValuesPresent = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 8) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                serviceExperience.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue)));

                                if (isValuesPresent <= 1 && objQuestion.AverageRatingValue > 0)
                                {
                                    ++isValuesPresent;
                                }
                            }
                            else
                                serviceExperience.CompareSpec.Add(GetCompareBikeData(_defaultMinSpecs));
                        }
                        if (isValuesPresent > 1)
                            performanceParameters.SpecCategory.Add(serviceExperience);
                        // Maintenance Cost
                        CompareSubCategory maintenanceCost = GetCompareSubCategory(BWConstants.MaintenanceCost);
                        isValuesPresent = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 9) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                maintenanceCost.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue)));

                                if (isValuesPresent <=1 && objQuestion.AverageRatingValue > 0)
                                {
                                    ++isValuesPresent;
                                }
                            }
                            else
                                maintenanceCost.CompareSpec.Add(GetCompareBikeData(_defaultMinSpecs));
                        }
                        if (isValuesPresent > 1)
                            performanceParameters.SpecCategory.Add(maintenanceCost);
                        // Value for Money
                        CompareSubCategory valueForMoney = GetCompareSubCategory(BWConstants.ValueForMoney);
                        isValuesPresent = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 10) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                valueForMoney.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()) });

                                if (isValuesPresent <= 1 && objQuestion.AverageRatingValue > 0)
                                {
                                    ++isValuesPresent;
                                }
                            }
                            else
                                valueForMoney.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(""), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable("") });
                        }
                        if (isValuesPresent > 1)
                            performanceParameters.SpecCategory.Add(valueForMoney);
                        // Extra Features
                        CompareSubCategory extraFeatures = GetCompareSubCategory(BWConstants.ExtraFeatures);
                        isValuesPresent = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 11) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                extraFeatures.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()) });

                                if (isValuesPresent <= 1&& objQuestion.AverageRatingValue > 0)
                                {
                                    ++isValuesPresent;
                                }
                            }
                            else
                                extraFeatures.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(""), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable("") });
                        }
                        if (isValuesPresent > 1)
                            performanceParameters.SpecCategory.Add(extraFeatures);
                        #endregion

                        if (performanceParameters.SpecCategory.Any())
                            compareReviews.Spec.Add(performanceParameters);

                        userReviewData.CompareReviews = compareReviews;
                        #region Reviews
                        //Most Helpful

                        IList<ReviewObject> objRecentList = new List<ReviewObject>();

                        foreach (var version in arrVersion)
                        {
                            ReviewObject objReview = new ReviewObject();
                            var reviewObj = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var basicInfoObj = compareEntity.BasicInfo != null ? compareEntity.BasicInfo.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)) : null;
                            if (reviewObj != null && basicInfoObj != null && reviewObj.ModelReview != null && reviewObj.ModelReview.UserReviews != null)
                            {
                                var modelReview = reviewObj.ModelReview;
                                objReview.RatingValue = modelReview.UserReviews.OverallRatingId;
                                objReview.ReviewDescription = FormatDescription.TruncateDescription(FormatDescription.SanitizeHtml(modelReview.UserReviews.Description), 85);
                                objReview.ReviewTitle = FormatDescription.TruncateDescription(modelReview.UserReviews.Title, 40);
                                objReview.ReviewListUrl = string.Format("/{0}-bikes/{1}/reviews/", basicInfoObj.MakeMaskingName, basicInfoObj.ModelMaskingName);
                                objReview.ReviewDetailUrl = string.Format("/{0}-bikes/{1}/reviews/{2}/", basicInfoObj.MakeMaskingName, basicInfoObj.ModelMaskingName, modelReview.UserReviews.ReviewId);
                            }
                            objRecentList.Add(objReview);
                        }

                        userReviewData.MostRecentReviews = objRecentList;
                        compareEntity.UserReviewData = userReviewData;

                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Compare.BikeComparison.TransposeCompareBikeData - {0}", versions));
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Dec 2017
        /// Summary : GetEngineAndTransmission details
        /// </summary>
        /// <param name="compareEntity"></param>
        /// <param name="arrVersion"></param>
        /// <returns></returns>
        private CompareSubMainCategory GetEngineAndTransmission(BikeCompareEntity compareEntity, string[] arrVersion)
        {
            CompareSubMainCategory engineTransmission = GetCompareSubMainCategory(BWConstants.EngineAndTransmission, "2");

            CompareSubCategory etDisplacement = GetCompareSubCategory(BWConstants.DisplacementWithUnit, BWConstants.Displacement);

            CompareSubCategory etCylinders = GetCompareSubCategory(BWConstants.Cylinders);

            CompareSubCategory etMaxPower = GetCompareSubCategory(BWConstants.MaxPower);

            CompareSubCategory etMaximumTorque = GetCompareSubCategory(BWConstants.Torque);

            CompareSubCategory etBore = GetCompareSubCategory(BWConstants.BoreWithUnit);

            CompareSubCategory etStroke = GetCompareSubCategory(BWConstants.StrokeWithUnit);

            CompareSubCategory etValvesPerCylinder = GetCompareSubCategory(BWConstants.ValvesPerCylinder);

            CompareSubCategory etFuelDeliverySystem = GetCompareSubCategory(BWConstants.FuelDeliverySystem);

            CompareSubCategory etFuelType = GetCompareSubCategory(BWConstants.FuelType);

            CompareSubCategory etIgnition = GetCompareSubCategory(BWConstants.Ignition);

            CompareSubCategory etSparkPlugs = GetCompareSubCategory(BWConstants.SparkPlugsPerCylinder);

            CompareSubCategory etCoolingSystem = GetCompareSubCategory(BWConstants.CoolingSystem);

            CompareSubCategory etGearBox = GetCompareSubCategory(BWConstants.GearboxType);

            CompareSubCategory etNoGears = GetCompareSubCategory(BWConstants.NumberOfGears);

            CompareSubCategory etTransmissionType = GetCompareSubCategory(BWConstants.TransmissionType);

            CompareSubCategory etClutch = GetCompareSubCategory(BWConstants.Clutch);

            foreach (var version in arrVersion)
            {
                var spec = compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                if (spec != null)
                {
                    etDisplacement.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Displacement.Value, "cc")));
                    etCylinders.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Cylinders.Value)));
                    etMaxPower.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.MaxPower.Value, "bhp", spec.MaxPowerRpm.Value, "rpm")));
                    etMaximumTorque.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.MaximumTorque.Value, "Nm", spec.MaximumTorqueRpm.Value, "rpm")));
                    etBore.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Bore.Value)));
                    etStroke.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Stroke.Value)));
                    etValvesPerCylinder.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.ValvesPerCylinder.Value)));
                    etFuelDeliverySystem.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.FuelDeliverySystem)));
                    etFuelType.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.FuelType)));
                    etIgnition.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Ignition)));
                    etSparkPlugs.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.SparkPlugsPerCylinder)));
                    etCoolingSystem.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.CoolingSystem)));
                    etGearBox.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.GearboxType)));
                    etNoGears.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.NoOfGears.Value)));
                    etTransmissionType.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.TransmissionType)));
                    etClutch.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Clutch)));

                }
            }

            // Add created sub-categories to the main engine-Transmission category
            engineTransmission.SpecCategory = new List<CompareSubCategory> {
                        etDisplacement,
                        etCylinders,
                        etMaxPower,
                        etMaximumTorque,
                        etBore,
                        etStroke,
                        etValvesPerCylinder,
                        etFuelDeliverySystem,
                        etFuelType,
                        etIgnition,
                        etSparkPlugs,
                        etCoolingSystem,
                        etGearBox,
                        etNoGears,
                        etTransmissionType,
                        etClutch
            };
            return engineTransmission; 
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Dec 2017
        /// Summary : Get BrakeWheelSuspension details
        /// </summary>
        /// <param name="compareEntity"></param>
        /// <param name="arrVersion"></param>
        /// <returns></returns>
        private CompareSubMainCategory GetBrakeWheelSuspension(BikeCompareEntity compareEntity, string[] arrVersion)
        {
            CompareSubMainCategory brakesWheelsSuspension = GetCompareSubMainCategory(BWConstants.BrakesWheelsSuspension, "3");

            CompareSubCategory bwsBreakType = GetCompareSubCategory(BWConstants.BrakeType);

            CompareSubCategory bwsFrontDisc = GetCompareSubCategory(BWConstants.FrontDisc);

            CompareSubCategory bwsFrontDisc_DrumSize = GetCompareSubCategory(BWConstants.FrontDiscDrumSize);

            CompareSubCategory bwsRearDisc = GetCompareSubCategory(BWConstants.RearDisc);


            CompareSubCategory bwsRearDisc_DrumSize = GetCompareSubCategory(BWConstants.RearDiscDrumSize);

            CompareSubCategory bwsCalliperType = GetCompareSubCategory(BWConstants.CalliperType);

            CompareSubCategory bwsWheelSize = GetCompareSubCategory(BWConstants.WheelSize);

            CompareSubCategory bwsFrontTyre = GetCompareSubCategory(BWConstants.FrontTyre);

            CompareSubCategory bwsRearTyre = GetCompareSubCategory(BWConstants.RearTyre);

            CompareSubCategory bwsTubelessTyres = GetCompareSubCategory(BWConstants.TubelessTyres);

            CompareSubCategory bwsRadialTyres = GetCompareSubCategory(BWConstants.RadialTyres);

            CompareSubCategory bwsAlloyWheels = GetCompareSubCategory(BWConstants.AlloyWheels);

            CompareSubCategory bwsFrontSuspension = GetCompareSubCategory(BWConstants.FrontSuspension);

            CompareSubCategory bwsRearSuspension = GetCompareSubCategory(BWConstants.RearSuspension);

            foreach (var version in arrVersion)
            {
                var spec = compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                if (spec != null)
                {
                    bwsBreakType.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.BrakeType)));
                    bwsFrontDisc.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.FrontDisc)));
                    bwsFrontDisc_DrumSize.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.FrontDisc_DrumSize.Value)));
                    bwsRearDisc.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.RearDisc)));
                    bwsRearDisc_DrumSize.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.RearDisc_DrumSize.Value)));
                    bwsCalliperType.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.CalliperType)));
                    bwsWheelSize.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.WheelSize.Value)));
                    bwsFrontTyre.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.FrontTyre)));
                    bwsRearTyre.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.RearTyre)));
                    bwsTubelessTyres.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.TubelessTyres)));
                    bwsRadialTyres.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.RadialTyres)));
                    bwsAlloyWheels.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.AlloyWheels)));
                    bwsFrontSuspension.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.FrontSuspension)));
                    bwsRearSuspension.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.RearSuspension)));

                }
            }

            brakesWheelsSuspension.SpecCategory = new List<CompareSubCategory> {
                         bwsBreakType
                        ,bwsFrontDisc
                        ,bwsFrontDisc_DrumSize
                        ,bwsRearDisc
                        ,bwsRearDisc_DrumSize
                        ,bwsCalliperType
                        ,bwsWheelSize
                        ,bwsFrontTyre
                        ,bwsRearTyre
                        ,bwsTubelessTyres
                        ,bwsRadialTyres
                        ,bwsAlloyWheels
                        ,bwsFrontSuspension
                        ,bwsRearSuspension
            };

            return brakesWheelsSuspension;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Dec 2017
        /// Summary : Get DimensionsAndChasis details
        /// </summary>
        /// <param name="compareEntity"></param>
        /// <param name="arrVersion"></param>
        /// <returns></returns>
        private CompareSubMainCategory GetDimensionsAndChasis(BikeCompareEntity compareEntity, string[] arrVersion)
        {
            CompareSubMainCategory dimensionChasis = GetCompareSubMainCategory(BWConstants.DimensionsAndChasis, "4");

            CompareSubCategory KerbWeight = GetCompareSubCategory(BWConstants.KerbWeight);

            CompareSubCategory OverallLength = GetCompareSubCategory(BWConstants.OverallLength);

            CompareSubCategory OverallWidth = GetCompareSubCategory(BWConstants.OverallWidth);

            CompareSubCategory OverallHeight = GetCompareSubCategory(BWConstants.OverallHeight);

            CompareSubCategory Wheelbase = GetCompareSubCategory(BWConstants.WheelBase);

            CompareSubCategory GroundClearance = GetCompareSubCategory(BWConstants.GroundClearance);

            CompareSubCategory SeatHeight = GetCompareSubCategory(BWConstants.SeatHeight);

            foreach (var version in arrVersion)
            {
                var spec = compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                if (spec != null)
                {
                    KerbWeight.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.KerbWeight.Value)));
                    OverallLength.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.OverallLength.Value)));
                    OverallWidth.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.OverallWidth.Value)));
                    OverallHeight.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.OverallHeight.Value)));
                    Wheelbase.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Wheelbase.Value)));
                    GroundClearance.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.GroundClearance.Value)));
                    SeatHeight.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.SeatHeight.Value)));
                }
            }

            dimensionChasis.SpecCategory = new List<CompareSubCategory> {
                        KerbWeight
                        ,OverallLength
                        ,OverallWidth
                        ,OverallHeight
                        ,Wheelbase
                        ,GroundClearance
                        ,SeatHeight
            };
            return dimensionChasis;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Dec 2017
        /// Summary : Get FuelEfficiencyPerformance details
        /// </summary>
        /// <param name="compareEntity"></param>
        /// <param name="arrVersion"></param>
        /// <returns></returns>
        private CompareSubMainCategory GetFuelEfficiencyPerformance(BikeCompareEntity compareEntity, string[] arrVersion)
        {
            CompareSubMainCategory fuelEfficiencyPerformance = GetCompareSubMainCategory(BWConstants.FuelEfficiencyPerformance, "5");

            CompareSubCategory FuelTankCapacity = GetCompareSubCategory(BWConstants.FuelTankCapacity);

            CompareSubCategory ReserveFuelCapacity = GetCompareSubCategory(BWConstants.ReserveFuelCapacity);

            CompareSubCategory FuelEfficiencyOverall = GetCompareSubCategory(BWConstants.FuelEfficiencyOverall);

            CompareSubCategory FuelEfficiencyRange = GetCompareSubCategory(BWConstants.FuelEfficiencyRange);

            CompareSubCategory Performance_0_60_kmph = GetCompareSubCategory(BWConstants.Performance0to60);

            CompareSubCategory Performance_0_80_kmph = GetCompareSubCategory(BWConstants.Performance0to80);

            CompareSubCategory Performance_0_40_m = GetCompareSubCategory(BWConstants.Performance0to40m);

            CompareSubCategory TopSpeed = GetCompareSubCategory(BWConstants.TopSpeed);

            CompareSubCategory Performance_60_0_kmph = GetCompareSubCategory(BWConstants.Performance60to0);

            CompareSubCategory Performance_80_0_kmph = GetCompareSubCategory(BWConstants.Performance80to0);

            foreach (var version in arrVersion)
            {
                var spec = compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                if (spec != null)
                {
                    FuelTankCapacity.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.FuelTankCapacity.Value)));
                    ReserveFuelCapacity.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.ReserveFuelCapacity.Value)));
                    FuelEfficiencyOverall.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.FuelEfficiencyOverall.Value)));
                    FuelEfficiencyRange.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.FuelEfficiencyRange.Value)));
                    Performance_0_60_kmph.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Performance_0_60_kmph.Value)));
                    Performance_0_80_kmph.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Performance_0_80_kmph.Value)));
                    Performance_0_40_m.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Performance_0_40_m.Value)));
                    TopSpeed.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.TopSpeed.Value)));
                    Performance_60_0_kmph.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Performance_60_0_kmph)));
                    Performance_80_0_kmph.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(spec.Performance_80_0_kmph)));
                }
            }

            fuelEfficiencyPerformance.SpecCategory = new List<CompareSubCategory> {
                FuelTankCapacity
                ,ReserveFuelCapacity
                ,FuelEfficiencyOverall
                ,FuelEfficiencyRange
                ,Performance_0_60_kmph
                ,Performance_0_80_kmph
                ,Performance_0_40_m
                ,TopSpeed
                ,Performance_60_0_kmph
                ,Performance_80_0_kmph
            };

            return fuelEfficiencyPerformance;
        }


        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Dec 2017
        /// </summary>
        /// <param name="compareEntity"></param>
        /// <param name="arrVersion"></param>
        /// <returns></returns>
        private CompareSubMainCategory GetFeatures(BikeCompareEntity compareEntity, string[] arrVersion)
        {
            CompareSubMainCategory features = GetCompareSubMainCategory(BWConstants.Features);

            //Speedometer
            CompareSubCategory Speedometer = GetCompareSubCategory(BWConstants.Speedometer);

            //Tachometer
            CompareSubCategory Tachometer = GetCompareSubCategory(BWConstants.Tachometer);

            //Tachometer Type
            CompareSubCategory TachometerType = GetCompareSubCategory(BWConstants.TachometerType);
            //Shift Light
            CompareSubCategory ShiftLight = GetCompareSubCategory(BWConstants.ShiftLight);

            //Electric Start
            CompareSubCategory ElectricStart = GetCompareSubCategory(BWConstants.ElectricStart);

            //Tripmeter
            CompareSubCategory Tripmeter = GetCompareSubCategory(BWConstants.Tripmeter);

            //No Of Tripmeters
            CompareSubCategory NoOfTripmeters = GetCompareSubCategory(BWConstants.NumberOfTripmeters);

            //Tripmeter Type
            CompareSubCategory TripmeterType = GetCompareSubCategory(BWConstants.TripmeterType);

            //Low Fuel Indicator
            CompareSubCategory LowFuelIndicator = GetCompareSubCategory(BWConstants.LowFuelIndicator);

            //Low Oil Indicator
            CompareSubCategory LowOilIndicator = GetCompareSubCategory(BWConstants.LowOilIndicator);

            //Low Battery Indicator
            CompareSubCategory LowBatteryIndicator = GetCompareSubCategory(BWConstants.LowBatteryIndicator);

            //Fuel Gauge
            CompareSubCategory FuelGauge = GetCompareSubCategory(BWConstants.FuelGauge);

            //Digital Fuel Gauges
            CompareSubCategory DigitalFuelGauges = GetCompareSubCategory(BWConstants.DigitalFuelGauges);

            //Pillion Seat
            CompareSubCategory PillionSeat = GetCompareSubCategory(BWConstants.PillionSeat);

            //Pillion Footrest
            CompareSubCategory PillionFootrest = GetCompareSubCategory(BWConstants.PillionFootrest);

            //Pillion Backrest
            CompareSubCategory PillionBackrest = GetCompareSubCategory(BWConstants.PillionBackrest);

            //Pillion Grabrail
            CompareSubCategory PillionGrabrail = GetCompareSubCategory(BWConstants.PillionGabrail);

            //Stand Alarm
            CompareSubCategory StandAlarm = GetCompareSubCategory(BWConstants.StandAlarm);

            //Stepped Seat
            CompareSubCategory SteppedSeat = GetCompareSubCategory(BWConstants.SteppedSeat);

            //Antilock Braking System
            CompareSubCategory AntilockBrakingSystem = GetCompareSubCategory(BWConstants.AntilockBrakingSystem);

            //Killswitch
            CompareSubCategory Killswitch = GetCompareSubCategory(BWConstants.Killswitch);

            //Clock
            CompareSubCategory Clock = GetCompareSubCategory(BWConstants.Clock);

            foreach (var version in arrVersion)
            {
                var feature = compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                if (feature != null)
                {
                    Speedometer.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.Speedometer)));
                    Tachometer.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.Tachometer)));
                    TachometerType.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.TachometerType)));
                    ShiftLight.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.ShiftLight)));
                    ElectricStart.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.ElectricStart)));
                    Tripmeter.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.Tripmeter)));
                    NoOfTripmeters.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.NoOfTripmeters)));
                    TripmeterType.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.TripmeterType)));
                    LowFuelIndicator.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.LowFuelIndicator)));
                    LowOilIndicator.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.LowOilIndicator)));
                    LowBatteryIndicator.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.LowBatteryIndicator)));
                    FuelGauge.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.FuelGauge)));
                    DigitalFuelGauges.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.DigitalFuelGauge)));
                    PillionSeat.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.PillionSeat)));
                    PillionFootrest.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.PillionFootrest)));
                    PillionBackrest.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.PillionBackrest)));
                    PillionGrabrail.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.PillionGrabrail)));
                    StandAlarm.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.StandAlarm)));
                    SteppedSeat.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.SteppedSeat)));
                    AntilockBrakingSystem.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.AntilockBrakingSystem)));
                    Killswitch.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.Killswitch)));
                    Clock.CompareSpec.Add(GetCompareBikeData(FormatMinSpecs.ShowAvailable(feature.Clock)));

                }
            }

            features.SpecCategory = new List<CompareSubCategory> {
                        Speedometer
                        ,Tachometer
                        ,TachometerType
                        ,ShiftLight
                        ,ElectricStart
                        ,Tripmeter
                        ,NoOfTripmeters
                        ,TripmeterType
                        ,LowFuelIndicator
                        ,LowOilIndicator
                        ,LowBatteryIndicator
                        ,FuelGauge
                        ,DigitalFuelGauges
                        ,PillionSeat
                        ,PillionFootrest
                        ,PillionBackrest
                        ,PillionGrabrail
                        ,StandAlarm
                        ,SteppedSeat
                        ,AntilockBrakingSystem
                        ,Killswitch
                        ,Clock
            };
            return features;
        }
        

        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Dec 2017
        /// </summary>
        /// <param name="compareEntity"></param>
        /// <param name="arrVersion"></param>
        /// <returns></returns>
        private CompareBikeColorCategory GetCompareColors(BikeCompareEntity compareEntity, string[] arrVersion)
        {
            CompareBikeColorCategory compareColors = GetCompareBikeColorCategory(BWConstants.Colours);
            compareEntity.Color = compareEntity.Color.GroupBy(p => p.ColorId).Select(grp => grp.First()).ToList<BikeColor>();
            foreach (var version in arrVersion)
            {
                var objBikeColor = new List<BikeColor>();
                foreach (var color in compareEntity.Color)
                {
                    if (color.VersionId == Convert.ToUInt32(version))
                    {
                        objBikeColor.Add(color);
                    }
                }
                compareColors.bikes.Add(new CompareBikeColor() { bikeColors = objBikeColor });
            }

            return compareColors;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 27th Dec 2017
        /// Description : Get Compare Main Category Entity
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private CompareMainCategory GetCompareMainCategory(string text)
        {
            return new CompareMainCategory { Text = text, Value = text, Spec = new List<CompareSubMainCategory>() };
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 27th Dec 2017
        /// Description : Get Compare sub main category entity
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private CompareSubMainCategory GetCompareSubMainCategory(string text)
        {
            return new CompareSubMainCategory { Text = text, Value = text };
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 27th Dec 2017
        /// Description : Get Compare sub main category entity
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private CompareSubMainCategory GetCompareSubMainCategory(string text, string value)
        {
            return new CompareSubMainCategory { Text = text, Value = value };
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 27th Dec 2017
        /// Description : Get CompareSubCategory entity
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private CompareSubCategory GetCompareSubCategory(string text)
        {
            return new CompareSubCategory { Text = text, Value = text, CompareSpec = new List<CompareBikeData>()};
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 27th Dec 2017
        /// Description : Get CompareSubCategory entity
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private CompareSubCategory GetCompareSubCategory(string text, string value)
        {
            return new CompareSubCategory { Text = text, Value = value, CompareSpec = new List<CompareBikeData>()};
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 27th Dec 2017
        /// Description : Get List of Compare Bike Data Entity
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private CompareBikeData GetCompareBikeData(string text)
        {
            return new CompareBikeData { Text = text, Value = text } ;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 27th Dec 2017
        /// Description : Get CompareBikeColorCategory Entity
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private CompareBikeColorCategory GetCompareBikeColorCategory(string text)
        {
            return new CompareBikeColorCategory { Text = text, Value = text, bikes = new List<CompareBikeColor>() };
        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Dec 2016
        /// Description : CAll DAL Layer
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<Entities.Compare.TopBikeCompareBase> CompareList(uint topCount)
        {
            return _objCompare.CompareList(topCount);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Dec 2016
        /// Description : BAL layer to similar cache comaprisions bikes   
        /// Modified by : Aditi Srivastava on 5 June 2017
        /// Summary     : Added cache call instead of DAL
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, ushort topCount, int cityid)
        {
            return _objCache.GetSimilarCompareBikes(versionList, topCount, cityid);
        }


        /// <summary>
        /// Created By : Sushil Kumar on 2nd Dec 2016
        /// Description : BAL layer to similar cache comaprisions bikes with sponsored comparision 
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <param name="cityid"></param>
        /// <param name="sponsoredVersionId"></param>
        /// <returns></returns>
        public ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikeSponsored(string versionList, ushort topCount, int cityid, uint sponsoredVersionId)
        {
            return _objCompare.GetSimilarCompareBikeSponsored(versionList, topCount, cityid, sponsoredVersionId);
        }

        /// <summary>
        /// CReated By : Sushil Kumar on 2nd Feb 2017
        /// Description : Added methods for comparisions bikes binding using transpose methodology
        /// </summary>
        /// <param name="versions"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public BikeCompareEntity DoCompare(string versions, uint cityId)
        {
            BikeCompareEntity compareEntity = null;
            try
            {
                if (!string.IsNullOrEmpty(versions))
                {
                    compareEntity = _objCompare.DoCompare(versions, cityId);
                    TransposeCompareBikeData(ref compareEntity, versions);
                }
                if (compareEntity != null)
                {
                    compareEntity.Features = null;
                    compareEntity.Specifications = null;
                    compareEntity.Color = null;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Compare.BikeComparison.DoCompare - {0} - {1}", versions, cityId));
            }
            return compareEntity;
        }

        /// <summary>
        /// Created By :- Subodh Jain 10 March 2017
        /// Summary :- Populate Compare ScootersList
        /// </summary>
        public IEnumerable<TopBikeCompareBase> ScooterCompareList(uint topCount)
        {
            return _objCompare.ScooterCompareList(topCount);
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 25 Apr 2017
        /// Summary    : Populate compare bikes list 
        /// Modified by: Aditi Srivastava 5 June 2017
        /// Summary    : randomize order of sponsored comparison first
        /// </summary>
        public IEnumerable<SimilarCompareBikeEntity> GetPopularCompareList(uint cityId)
        {
            IEnumerable<SimilarCompareBikeEntity> compareBikes = null;
            try
            {
                compareBikes = (List<SimilarCompareBikeEntity>)_objCache.GetPopularCompareList(cityId);
                if (compareBikes != null && compareBikes.Any())
                {
                    Random rnd = new Random();
                    compareBikes = compareBikes.Where(x => x.IsSponsored && x.SponsoredEndDate >= DateTime.Now && x.SponsoredStartDate <= DateTime.Now).OrderBy(x => rnd.Next())
                                   .Union(compareBikes.Where(x => !x.IsSponsored).OrderBy(x => x.DisplayPriority));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Compare.BikeComparison.GetPopularCompareList - CityId: {0}", cityId));
            }
            return compareBikes;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 5 Apr 2017
        /// Summary    : Populate compare scooters list 
        /// Modified by: Aditi Srivastava 5 June 2017
        /// Summary    : randomize order of sponsored comparison first
        /// </summary>
        public IEnumerable<SimilarCompareBikeEntity> GetScooterCompareList(uint cityId)
        {
            IEnumerable<SimilarCompareBikeEntity> compareScooters = null;
            try
            {
                compareScooters = (List<SimilarCompareBikeEntity>)_objCache.GetScooterCompareList(cityId);
                if (compareScooters != null && compareScooters.Any())
                {
                    Random rnd = new Random();
                    compareScooters = compareScooters.Where(x => x.IsSponsored && x.SponsoredEndDate >= DateTime.Now && x.SponsoredStartDate <= DateTime.Now).OrderBy(x => rnd.Next())
                                   .Union(compareScooters.Where(x => !x.IsSponsored).OrderBy(x => x.DisplayPriority));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Compare.BikeComparison.GetScooterCompareList - CityId: {0}", cityId));
            }
            return compareScooters;
        }

        public SimilarBikeComparisonWrapper GetSimilarBikes(string modelList, ushort topCount)
        {
                return _objCompare.GetSimilarBikes(modelList, topCount);
        }
    }

}

