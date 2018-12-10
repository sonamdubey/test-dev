using AutoMapper;
using Carwale.BL.Stock;
using Carwale.DTOs.Classified.CarDetails;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock.Finance;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified.CarDetail;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web;

namespace Carwale.BL.Classified.CarDetail
{
    public class CarDetail : ICarDetail
    {
        private ushort sellerType = 0;
        private const string notAvailableText = "Not Available";
        private readonly ICarDetailsCache _carDetailsCache;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepo;
        private readonly ICarMakesCacheRepository _carMakesCache;
        private readonly ICarDataLogic _carDataLogic;
        public CarDetail(ICarDetailsCache carDetailsCache, ICarDataLogic carDataLogic, IGeoCitiesCacheRepository geoCitiesCacheRepo, ICarMakesCacheRepository carMakesCache)
        {
            _carDetailsCache = carDetailsCache;
            _geoCitiesCacheRepo = geoCitiesCacheRepo;
            _carMakesCache = carMakesCache;
            _carDataLogic = carDataLogic;
        }

        #region GetCompleteCarDetails
        /// <summary>
        /// Created By : Sadhana Upadhyay on 1 June 2015
        /// Summary : To get Complete Car Detail by profile Id
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <returns></returns>
        public CarDetailsEntity GetCompleteCarDetails(string ProfileId, Platform platformType)
        {
            CarDetailsEntity objDetails = new CarDetailsEntity();
            try
            {
                objDetails = GetCarDetails(ProfileId);

                if (objDetails != null && objDetails.BasicCarInfo != null)
                {
                    PopulateFeatureAndSpecification(objDetails);
                    FormatBasicInfo(objDetails.BasicCarInfo);
                    FormatFeatureList(objDetails);
                    FormatSpecsList(objDetails);
                    FormatNonAbsureCarConditions(objDetails);
                    FormatIndividualWarranty(objDetails);
                    if (objDetails.Finance !=null && objDetails.Finance.IsEligibleForFinance)
                    {
                        var stockFinanceData = StockFinanceBL.GetFinanceData(new FinanceUrlParameter
                        {
                            HostUrl = (platformType == Platform.CarwaleDesktop ? ConfigurationManager.AppSettings["CTFinanceDesktop"] :
                                                        ConfigurationManager.AppSettings["CTFinanceMsite"]),
                            ProfileId = objDetails.BasicCarInfo.ProfileId,
                            MakeId = (int)objDetails.BasicCarInfo.MakeId,
                            ModelId = (int)objDetails.BasicCarInfo.ModelId,
                            MakeYear = objDetails.BasicCarInfo.MakeYear.Year,
                            CityId = (int)objDetails.BasicCarInfo.CityId,
                            PriceNumeric = Convert.ToInt32(objDetails.BasicCarInfo.PriceNumeric),
                            OwnerNumeric = objDetails.BasicCarInfo.OwnerNumber,
                            MakeMonth = objDetails.BasicCarInfo.MakeMonth
                        });
                        objDetails.Finance.FinanceUrl = stockFinanceData.FinanceUrl;
                        objDetails.Finance.FinanceUrlText = stockFinanceData.FinanceUrlText;
                    }
                    objDetails.ValuationUrl = String.Format("{0}/used/valuation/v1/report/?car={1}&year={2}&city={3}&askingPrice={4}&owner={5}{6}&kms={7}"
                        , platformType == Platform.CarwaleMobile? "/m": String.Empty 
                        , objDetails.BasicCarInfo.VersionId
                        , objDetails.BasicCarInfo.MakeYear.Year
                        , objDetails.BasicCarInfo.CityId
                        , objDetails.BasicCarInfo.PriceNumeric
                        , objDetails.BasicCarInfo.OwnerNumber ?? 0
                        , string.IsNullOrEmpty(objDetails.DealerInfo?.RatingText) ? "" 
                            : string.Format("&ratingText={0}", HttpUtility.UrlEncode(objDetails.DealerInfo.RatingText))
                        ,objDetails.BasicCarInfo.KmNumeric);
                    if (objDetails.BasicCarInfo.IsNew )
                    {
                        objDetails.BasicCarInfo.IsNewCarDealerAvailable = _carMakesCache.GetDealerAvailabilityForMakeCity((int)objDetails.BasicCarInfo.MakeId,(int)objDetails.BasicCarInfo.CityId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return objDetails;
        }

        private CarDetailsEntity GetCarDetails(string profileId)
        {
            CarDetailsEntity objDetails = null;
            sellerType = GetSellerType(profileId);
            uint inquiryId = Convert.ToUInt32(GetInquiryId(profileId));

            if (sellerType == (ushort)SellerType.Dealer)
            {
                objDetails = _carDetailsCache.GetDealerListingDetails(inquiryId);
            }
            else if (sellerType == (ushort)SellerType.Individual)
            {
                objDetails = _carDetailsCache.GetIndividualListingDetails(inquiryId);
            }
            return objDetails;
        }

        private void PopulateFeatureAndSpecification(CarDetailsEntity objDetails)
        {
            var carDataForVersions = _carDataLogic.GetCombinedCarData(new List<int> { Convert.ToInt32(objDetails.BasicCarInfo.VersionId) });
            if(carDataForVersions != null && carDataForVersions.Count > 0)
            {
                //Passing only one version in the version list for GetCombinedCarData will only return one carDataForVersion
                var carDataForVersion = carDataForVersions[0];
                objDetails.SpecificationList = Mapper.Map<List<SpecificationList>>(carDataForVersion.Specifications);
                objDetails.FeatureList = Mapper.Map<List<FeatureList>>(carDataForVersion.Features);
            }
        }
        #endregion

        #region FormatBasicInfo
        private void FormatBasicInfo(BasicCarInfo basicCarInfo)
        {
            try
            {
                if (basicCarInfo != null)
                {
                    basicCarInfo.Price = Format.FormatFullPrice(basicCarInfo.Price);
                    basicCarInfo.CarAvailbaleAt = !String.IsNullOrEmpty(basicCarInfo.AreaName) ? basicCarInfo.AreaName + ", " + basicCarInfo.CityName : basicCarInfo.CityName;
                    basicCarInfo.SellerName = !String.IsNullOrEmpty(basicCarInfo.SellerName) ? basicCarInfo.SellerName : notAvailableText;
                    basicCarInfo.NoOfOwners = basicCarInfo.OwnerNumber != null ? StockBL.FormatOwnerInfo(basicCarInfo.OwnerNumber.Value) : notAvailableText;
                    basicCarInfo.FuelEconomy = !String.IsNullOrEmpty(basicCarInfo.FuelEconomy) ? basicCarInfo.FuelEconomy + " kpl" : notAvailableText;
                    basicCarInfo.Color = !String.IsNullOrEmpty(basicCarInfo.Color) ? basicCarInfo.Color : notAvailableText;
                    basicCarInfo.RegisterCity = !String.IsNullOrEmpty(basicCarInfo.RegisterCity) ? basicCarInfo.RegisterCity : notAvailableText;
                    basicCarInfo.RegistrationNumber = !String.IsNullOrEmpty(basicCarInfo.RegistrationNumber) ? basicCarInfo.RegistrationNumber : notAvailableText;
                    basicCarInfo.LifeTimeTax = !String.IsNullOrEmpty(basicCarInfo.LifeTimeTax) ? basicCarInfo.LifeTimeTax : notAvailableText;
                    basicCarInfo.LastUpdatedDate = !String.IsNullOrEmpty(basicCarInfo.LastUpdatedDate) ? Format.GetDisplayTimeSpan(basicCarInfo.LastUpdatedDate) : notAvailableText;
                    basicCarInfo.MakeMonth = (basicCarInfo.MakeYear != null && !String.IsNullOrEmpty(basicCarInfo.MakeYear.ToString())) ? basicCarInfo.MakeYear.Month : 0;
                    if (string.IsNullOrEmpty(basicCarInfo.Insurance) || basicCarInfo.Insurance == "N/A")
                        basicCarInfo.Insurance = notAvailableText;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region FormatSpecsList
        private void FormatSpecsList(CarDetailsEntity objDetails)
        {
            try
            {
                if (objDetails.SpecificationList != null && objDetails.SpecificationList.Count > 0)
                {
                    objDetails.Specification = new List<Specification>();

                    foreach (var specification in objDetails.SpecificationList)
                    {
                        Specification objSpecification = new Specification();
                        objSpecification.SpecificationList = new List<SpecItems>();
                        objSpecification.CategoryName = specification.CategoryName;

                        foreach (var subSpecs in specification.Items)
                        {
                            if (!string.IsNullOrWhiteSpace(subSpecs.ItemValue))
                            {
                                SpecItems objSubSpecs = new SpecItems();

                                objSubSpecs.SpecName = subSpecs.ItemName;
                                objSubSpecs.SpecValue = subSpecs.ItemValue;
                                objSubSpecs.SpecUnit = subSpecs.ItemUnit;

                                objSpecification.SpecificationList.Add(objSubSpecs); 
                            }
                        }
                        objDetails.Specification.Add(objSpecification);
                    }
                }
                else
                    objDetails.Specification = null;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region FormatFeatureList
        private void FormatFeatureList(CarDetailsEntity objDetails)
        {
            try
            {
                Boolean flagUsed = true;
                //Format Feature start here
                if (objDetails.FeatureList != null)
                {
                    objDetails.Features = new List<Features>();
                    Boolean flag = false;
                    foreach (var feature in objDetails.FeatureList)
                    {
                        flag = false;
                        Features objFeature = new Features();
                        objFeature.FeatureItemList = new List<FeatureItems>();
                        objFeature.CategoryName = feature.CategoryName;
                        if (objFeature.CategoryName == "Safety" || objFeature.CategoryName == "Braking & Traction" || objFeature.CategoryName == "Locks & Security" || objFeature.CategoryName == "Comfort & Convenience")
                            flag = true;
                        foreach (var subFeature in feature.Items)
                        {
                            if (objFeature.CategoryName == "Manufacturer Warranty")
                                break;
                            FeatureItems objSubFeature = new FeatureItems();
                            objSubFeature.ItemValue = subFeature.DataTypeId == 2 ? subFeature.ItemValue != "0" : (!String.IsNullOrWhiteSpace(subFeature.ItemValue) && subFeature.ItemValue != "No");
                            if (!objSubFeature.ItemValue)
                                objSubFeature.ItemName = "No " + subFeature.ItemName;
                            else
                                objSubFeature.ItemName = subFeature.ItemName;
                            objFeature.FeatureItemList.Add(objSubFeature);
                            if (objSubFeature.ItemValue)
                            {
                                flag = true;
                                flagUsed = false;
                            }
                        }
                        if (flag)
                            objDetails.Features.Add(objFeature);
                    }
                    if (flagUsed)
                        if (objDetails.UsedCarFeatures != null)
                            FormatUsedCarFeatures(objDetails);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void FormatUsedCarFeatures(CarDetailsEntity objDetails)
        {
            try
            {
                objDetails.Features = new List<Features>();
                if (!string.IsNullOrEmpty(objDetails.UsedCarFeatures.Features_SafetySecurity))
                {
                    string[] safetyfeatures = objDetails.UsedCarFeatures.Features_SafetySecurity.Split('|');
                    if (FillUsedFeatures(safetyfeatures, "Safety & Security").FeatureItemList.Count > 0)
                        objDetails.Features.Add(FillUsedFeatures(safetyfeatures, "Safety & Security"));
                }
                if (!string.IsNullOrEmpty(objDetails.UsedCarFeatures.Features_Comfort))
                {
                    string[] comfortFeatures = objDetails.UsedCarFeatures.Features_Comfort.Split('|');
                    if (FillUsedFeatures(comfortFeatures, "Comfort").FeatureItemList.Count > 0)
                        objDetails.Features.Add(FillUsedFeatures(comfortFeatures, "Comfort"));
                }
                if (!string.IsNullOrEmpty(objDetails.UsedCarFeatures.Features_Others))
                {
                    string[] otherFeatures = objDetails.UsedCarFeatures.Features_Others.Split('|');
                    if (FillUsedFeatures(otherFeatures, "Others Features").FeatureItemList.Count > 0)
                        objDetails.Features.Add(FillUsedFeatures(otherFeatures, "Others Features"));
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        //to fill the features of used in case if new car features are not available
        private Features FillUsedFeatures(string[] objFeatures, string category)
        {

            Features objfeature = new Features();
            try
            {
                objfeature.FeatureItemList = new List<FeatureItems>();
                objfeature.CategoryName = category;
                foreach (var feature in objFeatures)
                {
                    FeatureItems objSubFeature = new FeatureItems();
                    objSubFeature.ItemName = feature;
                    objSubFeature.ItemValue = true;
                    objfeature.FeatureItemList.Add(objSubFeature);

                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return objfeature;
        }
        //Format Feature end here
        #endregion

        #region FormatCarConditions

        private void FormatNonAbsureCarConditions(CarDetailsEntity objDetails)
        {
            try
            {
                if (objDetails.NonAbsureCarCondition != null)
                {
                    PropertyInfo[] properties = objDetails.NonAbsureCarCondition.GetType().GetProperties();
                    bool isEmpty = true;
                    foreach (PropertyInfo prop in properties)
                    {
                        var value = prop.GetValue(objDetails.NonAbsureCarCondition);
                        value = !string.IsNullOrEmpty(value.ToString()) ? value : notAvailableText;
                        if (value.ToString() != notAvailableText)
                            isEmpty = false;
                        prop.SetValue(objDetails.NonAbsureCarCondition, value);
                    }
                    if (isEmpty)
                        objDetails.NonAbsureCarCondition = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region GetSellerType
        /// <summary>
        /// Created By : Sadhana Upadhyay on 1 June 2015
        /// Summary : To get SellerType from profileId
        /// </summary>
        /// <param name="profileNo"></param>
        /// <returns></returns>
        private ushort GetSellerType(string profileNo)
        {
            ushort sellerType = 0;

            switch (profileNo.Substring(0, 1).ToUpper())
            {
                case "D":
                    sellerType = 1;
                    break;

                case "S":
                    sellerType = 2;
                    break;

                default:
                    sellerType = 1;
                    break;
            }

            return sellerType;
        }
        #endregion

        #region GetInquiryId
        /// <summary>
        /// Created By : Sadhana Upadhyay on 1 June 2015
        /// Summary : To get inquiryId form profileId
        /// </summary>
        /// <param name="profileNo"></param>
        /// <returns></returns>
        private string GetInquiryId(string profileNo)
        {
            string retVal = "";

            switch (profileNo.Substring(0, 1).ToUpper())
            {
                case "D":
                    retVal = profileNo.Substring(1, profileNo.Length - 1);
                    break;

                case "S":
                    retVal = profileNo.Substring(1, profileNo.Length - 1);
                    break;

                default:
                    retVal = profileNo;
                    break;
            }

            return retVal;
        }   //End of GetInquiryId
        #endregion

        #region FormatWarranties
        /// <summary>
        /// Created By : Supriya Bhide on 1 June 2015
        /// Summary : To check which warranty exists
        /// </summary>
        /// <param name="objDetails"></param>
        private void FormatIndividualWarranty(CarDetailsEntity objDetails)
        {
            try
            {
                if (objDetails.BasicCarInfo.SellerId == 1)  // If Dealer Car
                {
                    objDetails.IndividualWarranty = null;
                }
                else
                {
                    if (objDetails.IndividualWarranty != null && string.IsNullOrEmpty(objDetails.IndividualWarranty.WarrantyDescription))
                        objDetails.IndividualWarranty = null;
                }
            }

            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        public UsedCarDetails GetCompleteCarDetailsMobile(string profileId, string deliveryCity, string imeiCode, int usedCarNotificationId, int sourceId)
        {
            UsedCarDetails carDetails = null;
            CarDetailsEntity objDetails = GetCarDetails(profileId);

            if (objDetails != null && objDetails.BasicCarInfo != null)
            {
                carDetails = new UsedCarDetails();
                carDetails.ProfileId = profileId;
                carDetails.DeliveryCity = !String.IsNullOrEmpty(deliveryCity) ? Convert.ToInt16(deliveryCity) : 0;
                carDetails.DeliveryText = !String.IsNullOrEmpty(deliveryCity) ? "Delivery available in " + (deliveryCity == "3000" ? "Mumbai" : deliveryCity == "3001" ? "Delhi NCR" : _geoCitiesCacheRepo.GetCityNameById(deliveryCity)) : String.Empty;

                carDetails.general = FormatGeneralInfoMobile(objDetails.BasicCarInfo);
                carDetails.features = FormatFeaturesInfoMobile(objDetails.UsedCarFeatures);
                carDetails.carCondition = FormatCarConditionInfoMobile(objDetails.NonAbsureCarCondition);
                carDetails.carPhoto = FormatCarPhotoInfoMobile(objDetails.ImageList);
                carDetails.carAdditionalInfo = FormatAdditionalInfoMobile(objDetails.DealerInfo, objDetails.Modifications, objDetails.IndividualWarranty);
                carDetails.absureInfo = new List<object>();
                carDetails.carFinanceQuoteInfo = new List<object>();
                carDetails.sellerOfferData = new List<object>();

                carDetails.MakeName = objDetails.BasicCarInfo.MakeName;
                carDetails.ModelName = objDetails.BasicCarInfo.ModelName;
                carDetails.CarName = objDetails.BasicCarInfo.MakeName + " " + objDetails.BasicCarInfo.ModelName + " " + objDetails.BasicCarInfo.VersionName;
                carDetails.Price = Format.GetPrice(objDetails.BasicCarInfo.Price);
                carDetails.Year = objDetails.BasicCarInfo.MakeYear.ToString("MMM, yyyy");
                carDetails.Kms = objDetails.BasicCarInfo.Kilometers;
                carDetails.SellerNote = objDetails.OwnerComments.SellerNote;
                carDetails.ReasonForSelling = objDetails.OwnerComments.ReasonForSell;
                carDetails.CertifiedLogoUrl = sellerType == (ushort)SellerType.Dealer ? String.Empty : null;
                carDetails.IsDealerCar = sellerType == (ushort)SellerType.Dealer ? "1" : "0";
                carDetails.ShareUrl = "http://www.carwale.com/used/cars-in-" + Format.FormatSpecial(objDetails.BasicCarInfo.CityName) + "/" + Format.FormatSpecial(objDetails.BasicCarInfo.MakeName) + "-" + objDetails.BasicCarInfo.MaskingName + "-" + profileId + "/";
                carDetails.StatusId = 1;
                carDetails.dealerQuickBloxId = -1;
                carDetails.DealerRatingText = objDetails.DealerInfo?.RatingText;

                if (objDetails.IsSold)
                {
                    carDetails.StatusId = 2;
                    carDetails.general = new List<Data>();
                    carDetails.features = new List<Data>();
                    carDetails.carCondition = new List<ConditionData>();
                    carDetails.carPhoto = new List<CarPhoto>();
                }

                carDetails.SmallPicUrl = "http://img.carwale.com/used/no-cars.jpg";
                carDetails.LargePicUrl = "http://img.carwale.com/used/no-cars.jpg";
                foreach (var photo in carDetails.carPhoto)
                {
                    if (photo.IsMainUrl)
                    {
                        carDetails.SmallPicUrl = photo.SmallPicUrl;
                        carDetails.LargePicUrl = photo.LargePicUrl;
                        break;
                    }
                }
            }
            return carDetails;
        }

        private List<Data> FormatGeneralInfoMobile(BasicCarInfo basicCarInfo)
        {
            List<Data> generalInfo = new List<Data>();
            generalInfo.Add(new Data { Label = "Registration", Value = !String.IsNullOrEmpty(basicCarInfo.RegisterCity) ? basicCarInfo.RegisterCity : "--" });
            generalInfo.Add(new Data { Label = "Owner", Value = basicCarInfo.OwnerNumber != null ? StockBL.FormatOwnerInfo(basicCarInfo.OwnerNumber.Value) : "--" });
            generalInfo.Add(new Data { Label = "Color (Exterior, Interior)", Value = (!String.IsNullOrEmpty(basicCarInfo.Color) ? basicCarInfo.Color : "-- ") + " , " + (!String.IsNullOrEmpty(basicCarInfo.InteriorColor) ? basicCarInfo.InteriorColor : "--") });
            generalInfo.Add(new Data { Label = "Insurance", Value = !String.IsNullOrEmpty(basicCarInfo.Insurance) ? basicCarInfo.Insurance : "--" });
            generalInfo.Add(new Data { Label = "Lifetime-Tax", Value = !String.IsNullOrEmpty(basicCarInfo.LifeTimeTax) ? basicCarInfo.LifeTimeTax : "--" });
            generalInfo.Add(new Data { Label = "Engine", Value = (!String.IsNullOrEmpty(basicCarInfo.TransmissionType) ? basicCarInfo.TransmissionType : "-- ") + ", " + (!String.IsNullOrEmpty(basicCarInfo.FuelName) ? basicCarInfo.FuelName : "-- ") });
            generalInfo.Add(new Data { Label = "Fuel Economy", Value = !String.IsNullOrEmpty(basicCarInfo.FuelEconomy) ? basicCarInfo.FuelEconomy + " kpl" : "--" });
            generalInfo.Add(new Data { Label = "Last Updated", Value = !String.IsNullOrEmpty(basicCarInfo.LastUpdatedDate) ? Convert.ToDateTime(basicCarInfo.LastUpdatedDate).ToString("dd MMM, yyyy") : "--" });
            return generalInfo;
        }

        private List<Data> FormatFeaturesInfoMobile(UsedCarFeatures carFeatures)
        {
            List<Data> featuresInfo = new List<Data>();
            if (carFeatures != null)
            {
                featuresInfo.Add(new Data { Label = "Safety & Security", Value = !String.IsNullOrEmpty(carFeatures.Features_SafetySecurity) ? carFeatures.Features_SafetySecurity : "--" });
                featuresInfo.Add(new Data { Label = "Comfort", Value = !String.IsNullOrEmpty(carFeatures.Features_Comfort) ? carFeatures.Features_Comfort : "--" });
                featuresInfo.Add(new Data { Label = "Others", Value = !String.IsNullOrEmpty(carFeatures.Features_Others) ? carFeatures.Features_Others : "--" });
            }
            return featuresInfo;
        }

        private List<ConditionData> FormatCarConditionInfoMobile(NonAbsureCarCondition carCondition)
        {
            List<ConditionData> conditionInfo = new List<ConditionData>();
            if (carCondition != null)
            {
                conditionInfo.Add(new ConditionData { Label = "AC", Value = !String.IsNullOrEmpty(carCondition.AC) ? carCondition.AC : "--" });
                conditionInfo.Add(new ConditionData { Label = "Engine", Value = !String.IsNullOrEmpty(carCondition.Engine) ? carCondition.Engine : "--" });
                conditionInfo.Add(new ConditionData { Label = "Suspensions", Value = !String.IsNullOrEmpty(carCondition.Suspensions) ? carCondition.Suspensions : "--" });
                conditionInfo.Add(new ConditionData { Label = "Brakes", Value = !String.IsNullOrEmpty(carCondition.Brakes) ? carCondition.Brakes : "--" });
                conditionInfo.Add(new ConditionData { Label = "Battery", Value = !String.IsNullOrEmpty(carCondition.Battery) ? carCondition.Battery : "--" });
                conditionInfo.Add(new ConditionData { Label = "Tyres", Value = !String.IsNullOrEmpty(carCondition.Tyres) ? carCondition.Tyres : "--" });
                conditionInfo.Add(new ConditionData { Label = "Electricals", Value = !String.IsNullOrEmpty(carCondition.Electricals) ? carCondition.Electricals : "--" });
                conditionInfo.Add(new ConditionData { Label = "Seats", Value = !String.IsNullOrEmpty(carCondition.Seats) ? carCondition.Seats : "--" });
                conditionInfo.Add(new ConditionData { Label = "Interior", Value = !String.IsNullOrEmpty(carCondition.Interior) ? carCondition.Interior : "--" });
                conditionInfo.Add(new ConditionData { Label = "Exterior", Value = !String.IsNullOrEmpty(carCondition.Exterior) ? carCondition.Exterior : "--" });
                conditionInfo.Add(new ConditionData { Label = "Overall", Value = !String.IsNullOrEmpty(carCondition.OverAll) ? carCondition.OverAll : "--" });
            }
            return conditionInfo;
        }

        private List<CarPhoto> FormatCarPhotoInfoMobile(CarDetailsImageGallery imageList)
        {
            List<CarPhoto> photoInfo = new List<CarPhoto>();
            if (imageList != null)
            {
                foreach (ImageUrl url in imageList.ImageUrlAttributes)
                {
                    photoInfo.Add(new CarPhoto
                    {
                        SmallPicUrl = ImageSizes.CreateImageUrl(url.HostUrl, ImageSizes._310X174, url.OriginalImgPath),
                        LargePicUrl = ImageSizes.CreateImageUrl(url.HostUrl, ImageSizes._762X429, url.OriginalImgPath),
                        IsMainUrl = url.IsMain
                    });
                }
            }
            return photoInfo;
        }

        private List<CarAdditionalInfo> FormatAdditionalInfoMobile(DealerInfo dealerInfo, Modifications modifications, IndividualWarranty indWarranty)
        {
            CarAdditionalInfo additionalInfo = new CarAdditionalInfo();
            additionalInfo.IsCarInWarranty = "False";
            additionalInfo.HasExtendedWarranty = "False";
            additionalInfo.HasAnyServiceRecords = "False";
            additionalInfo.HasRSAAvailable = "False";
            additionalInfo.HasFreeRSA = "False";

            if (sellerType == (ushort)SellerType.Dealer)
            {
                additionalInfo.MaskingNumber = String.Empty;
                additionalInfo.Modifications = modifications != null ? modifications.Comments : String.Empty;
            }
            else
            {
                additionalInfo.IndividualWarranty = indWarranty != null ? indWarranty.WarrantyDescription : String.Empty;
                additionalInfo.IndividualModifications = modifications != null ? modifications.Comments : String.Empty;
            }
            return new List<CarAdditionalInfo> { additionalInfo };
        }
    }
}