IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_OEMCarsRecommendation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_OEMCarsRecommendation]
GO

	-- =============================================  
-- Author      : Chetan Navin   
-- Create date : 16 May 2013  
-- Description : Cross Selling of lead within similar price range and bodystyle for OEMCarsOnly 
-- Module      : CRM 
-- EXEC CRM_OEMCarsRecommendation 1,1159,1 
-- =============================================  
CREATE PROCEDURE [dbo].[CRM_OEMCarsRecommendation]  
    @HeadAgencyId INT,    
	@VersionId INT,
	@CityId INT
AS  
BEGIN  
 SET NOCOUNT ON;  
   
 DECLARE   
   @SubSegmentId INT,  
   @BodyStyleId INT,  
   @CarFuelType INT,  
   @CarTranmission INT,
   @ModelId INT
     
 IF (@HeadAgencyId = 1)  
   
	BEGIN  
		SELECT @ModelId=CV.CarModelId,@SubSegmentId = CV.SubSegmentId ,@BodyStyleId = CV.BodyStyleId ,@CarFuelType = CV.CarFuelType,@CarTranmission = CV.CarTransmission  
		FROM CarVersions CV WITH (NOLOCK) WHERE CV.ID = @VersionId;
       
	  --Exact Matches 
	  WITH CTE AS
	  (  
		SELECT CMA.Name + ' ' + CMO.Name + ' ' + CV.Name AS Car,CMA.Name AS Make, CMO.Name AS Model ,CMA.ID AS MakeId,   
		CMO.ID AS ModelId,CV.ID AS VersionId,  
		ROW_NUMBER() OVER (PARTITION BY CMO.ID ORDER BY CV.ID) AS ModelTop   
		FROM CarMakes AS CMA WITH (NOLOCK)  
		INNER JOIN CarModels AS CMO WITH (NOLOCK) ON CMO.CarMakeId = CMA.ID AND CMO.IsDeleted = 0 AND CMO.New = 1
		INNER JOIN CarVersions AS CV WITH (NOLOCK) ON CV.CarModelId = CMO.ID AND CV.IsDeleted = 0 AND CV.New = 1
		WHERE CV.SubSegmentId = @SubSegmentId   
		AND CV.BodyStyleId = @BodyStyleId AND CV.CarFuelType = @CarFuelType AND CV.CarTransmission = @CarTranmission AND CMO.ID <> @ModelId --AND CV.ID <> @VersionId  
		AND CMO.ID IN 
		(	
			SELECT DISTINCT CASE ModelId WHEN -1 THEN CMO.ID ELSE ModelId END AS ModelId FROM CRM_ADM_FLCRules CAR, CRM_ADM_FLCGroups CAF, CarModels CMO
			WHERE CAR.GroupId = CAF.Id AND CAF.GroupType IN(1,2) AND CAF.IsActive = 1 AND CAR.IsActive = 1
			AND (CityId = -1 OR CityId = @CityId) AND ModelId NOT IN(SELECT CV.CarModelId FROM CarVersions CV, CRM_BlockList CB WHERE CB.VersionId = CV.Id)
			AND CAR.MakeId = CMO.CarMakeId AND CMO.New = 1 AND CMO.IsDeleted = 0 
		) 
	  )
		SELECT * from CTE WHERE ModelTop = 1
		
	IF @@ROWCOUNT = 0
		BEGIN
		 --Other Matches 
		 WITH CTE AS
		  ( 
			  SELECT CMA.Name + ' ' + CMO.Name + ' ' + CV.Name AS Car, CMA.Name AS Make,CMO.Name AS Model, CMA.ID AS MakeId,   
			  CMO.ID AS ModelId,CV.ID AS VersionId, 
			  ROW_NUMBER() OVER (PARTITION BY CMO.ID ORDER BY CV.ID) AS ModelTop  
			  FROM CarMakes AS CMA WITH (NOLOCK)  
			  INNER JOIN CarModels AS CMO WITH (NOLOCK) ON CMO.CarMakeId = CMA.ID AND CMO.IsDeleted = 0  AND CMO.New = 1
			  INNER JOIN CarVersions AS CV WITH (NOLOCK) ON CV.CarModelId = CMO.ID AND CV.IsDeleted = 0  AND CV.New = 1
			  WHERE CV.SubSegmentId = @SubSegmentId    
			  AND CV.CarFuelType = @CarFuelType AND CMO.ID <> @ModelId --AND CV.ID <> @VersionId  
			  AND CMO.ID IN 
			  (	
				SELECT DISTINCT CASE ModelId WHEN -1 THEN CMO.ID ELSE ModelId END AS ModelId FROM CRM_ADM_FLCRules CAR, CRM_ADM_FLCGroups CAF, CarModels CMO
				WHERE CAR.GroupId = CAF.Id AND CAF.GroupType IN(1,2) AND CAF.IsActive = 1 AND CAR.IsActive = 1
				AND (CityId = -1 OR CityId = @CityId) AND ModelId NOT IN(SELECT CV.CarModelId FROM CarVersions CV, CRM_BlockList CB WHERE CB.VersionId = CV.Id)
				AND CAR.MakeId = CMO.CarMakeId AND CMO.New = 1 AND CMO.IsDeleted = 0
			   ) 
			)
			SELECT * from CTE WHERE ModelTop = 1
		END 
	END  
END  