IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RecommendCar_SP_AddMakesFeaturesPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RecommendCar_SP_AddMakesFeaturesPrices]
GO

	CREATE PROCEDURE [dbo].[RecommendCar_SP_AddMakesFeaturesPrices]

 AS
	
BEGIN
	
	--insert all the makes prices
	Delete From RecommendCar_MakesPrices
	
	Insert Into RecommendCar_MakesPrices 
		SELECT MA.ID, MA.Name,  
			(SELECT MIN(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN 
			(SELECT ID FROM CarVersions WHERE CarModelId IN  
			(SELECT ID FROM CarModels WHERE CarMakeId = MA.ID))) AS MinPriceNew,  
			(SELECT MAX(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN 
			(SELECT ID FROM CarVersions WHERE CarModelId IN  
			(SELECT ID FROM CarModels WHERE CarMakeId = MA.ID))) AS MaxPriceNew,  
			(SELECT Top 1 MA1.Name + ' ' + MO.Name + ' ' + CV.Name FROM CarVersions AS CV, CarModels AS MO, 
			CarMakes AS MA1, NewCarShowroomPrices AS NPMin WHERE MA1.ID = MO.CarMakeId AND MO.ID = CV.CarModelId AND 
			CV.New = 1 AND  CV.ID = NPMin.CarVersionId AND NPMin.CityId = 1 and NPMin.Price > 0 Order By NPMin.Price) AS MinPriceNewCar,
			(SELECT MIN(Price) FROM SellInquiries WHERE Price >= 20000 AND  CarVersionId IN 
			(SELECT ID FROM CarVersions WHERE CarModelId IN  (SELECT ID FROM CarModels WHERE CarMakeId = MA.ID))) AS MinPriceUsed,  
			(SELECT MAX(Price) FROM SellInquiries WHERE Price >= 20000 AND  CarVersionId IN 
			(SELECT ID FROM CarVersions WHERE CarModelId IN  
			(SELECT ID FROM CarModels WHERE CarMakeId = MA.ID))) AS MaxPriceUsed,  
			(SELECT MA1.Name + ' ' + MO.Name + ' ' + CV.Name  FROM CarVersions AS CV, CarModels AS MO, CarMakes AS MA1  WHERE MA1.ID = MO.CarMakeId AND MO.ID = CV.CarModelId AND  CV.ID IN 
			(SELECT TOP 1 CarVersionId FROM SellInquiries WHERE  Price >= 20000 ORDER BY Price ASC)) AS MinPriceUsedCar  
		FROM 
			CarMakes AS MA
			
			
	--Now insert all features prices
	Delete From RecommendCar_FeaturesPrices
	
	Insert Into RecommendCar_FeaturesPrices
		SELECT  
			(SELECT MIN(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE ACR > 0)) AS AC,  
			(SELECT MAX(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE ACR > 0)) AS ACMax,  
			(SELECT MIN(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE PowerSteeringR > 0)) AS PowerSteering,  
			(SELECT MAX(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE PowerSteeringR > 0)) AS PowerSteeringMax,  
			(SELECT MIN(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE PowerWindowsR > 0)) AS PowerWindows,  
			(SELECT MAX(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE PowerWindowsR > 0)) AS PowerWindowsMax,  
			(SELECT MIN(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE CentralLockingR > 0)) AS CentralLocking,  
			(SELECT MAX(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE CentralLockingR > 0)) AS CentralLockingMax,  
			(SELECT MIN(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE AutomaticTransmissionR > 0)) AS AutomaticTransmission,  
			(SELECT MAX(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE AutomaticTransmissionR > 0)) AS AutomaticTransmissionMax,  
			(SELECT MIN(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE ABSR > 0)) AS ABSR,  
			(SELECT Max(Price) FROM NewCarShowroomPrices WHERE CityId = 1 AND  CarVersionId IN
				(SELECT VersionId FROM RecommendCarData WHERE ABSR > 0)) AS ABSRMax,  
			(SELECT MIN(Price) FROM SellInquiries AS SI, CarVersions AS CV  WHERE SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
				(SELECT ModelId FROM RecommendCarData WHERE ACR > 0)) AS ACUsed, 
			(SELECT MAX(Price) FROM SellInquiries AS SI, CarVersions AS CV  WHERE SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
				(SELECT ModelId FROM RecommendCarData WHERE ACR > 0)) AS ACUsedMax,  
			(SELECT MIN(Price) FROM SellInquiries AS SI, CarVersions AS CV  WHERE SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
				(SELECT ModelId FROM RecommendCarData WHERE PowerSteeringR > 0)) AS PowerSteeringUsed,  
			(SELECT MAX(Price) FROM SellInquiries AS SI, CarVersions AS CV  
			WHERE 
				SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
					(SELECT ModelId FROM RecommendCarData WHERE PowerSteeringR > 0)) AS PowerSteeringUsedMax,  
					(SELECT MIN(Price) FROM SellInquiries AS SI, CarVersions AS CV  
					WHERE SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
					(SELECT ModelId FROM RecommendCarData WHERE PowerWindowsR > 0)) AS PowerWindowsUsed,  
					(SELECT MAX(Price) FROM SellInquiries AS SI, CarVersions AS CV  
					WHERE SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
					(SELECT ModelId FROM RecommendCarData WHERE PowerWindowsR > 0)) AS PowerWindowsUsedMax,  
					(SELECT MIN(Price) FROM SellInquiries AS SI, CarVersions AS CV  
					WHERE SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
					(SELECT ModelId FROM RecommendCarData WHERE CentralLockingR > 0)) AS CentralLockingUsed,  
					(SELECT MAX(Price) FROM SellInquiries AS SI, CarVersions AS CV  
					WHERE SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
					(SELECT ModelId FROM RecommendCarData WHERE CentralLockingR > 0)) AS CentralLockingUsedMax,  
					(SELECT MIN(Price) FROM SellInquiries AS SI, CarVersions AS CV  
					WHERE SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
					(SELECT ModelId FROM RecommendCarData WHERE AutomaticTransmissionR > 0)) AS AutomaticTransmissionUsed,  
					(SELECT MAX(Price) FROM SellInquiries AS SI, CarVersions AS CV  
					WHERE SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
					(SELECT ModelId FROM RecommendCarData WHERE AutomaticTransmissionR > 0)) AS AutomaticTransmissionUsedMax,  
					(SELECT MIN(Price) FROM SellInquiries AS SI, CarVersions AS CV  
					WHERE SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
					(SELECT ModelId FROM RecommendCarData WHERE ABSR > 0)) AS ABSRUsed,  
					(SELECT MAX(Price) FROM SellInquiries AS SI, CarVersions AS CV  
					WHERE SI.Price >= 20000 AND SI.CarVersionId = CV.ID AND  CV.CarModelId IN 
					(SELECT ModelId FROM RecommendCarData WHERE ABSR > 0)) 
			AS ABSRUsedMax 	
	
	
END
