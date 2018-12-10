IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetUpcomingAndRecentLaunches_v16_9_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetUpcomingAndRecentLaunches_v16_9_1]
GO

	
-- =============================================                    
-- Author:  <Prashant Vishe>                    
-- Create date: <26 Dec 2012>                    
-- Description:<Returns upcoming and Recently launched Cars >     
-- Modified By:Parshant vishe on 29 aug 2013   added priority fetching query  
-- Modified by:Prashant vishe on 31 Jan 2013    
-- Modification:added query for retrieving carwale confidence related data
-- Modifier : Sachin Bharti (1st Sept 2016)
-- Purpose : Get upcoming and new launches models and versions both
-- execute [cw].[GetUpcomingAndRecentLaunches_v16_9_1] 0,0,1,1
-- Modified By Jitendra upcomming cars fetching order correct             
CREATE PROCEDURE [cw].[GetUpcomingAndRecentLaunches_v16_9_1]                    
 -- Add the parameters for the stored procedure here 
  @IsActive INT,            
  @IsLaunched INT = NULL,            
  @IsFuturistic INT = NULL,
  @ReportType INT = NULL --- 1- All, 2- Models, 3-Version
AS                    
BEGIN                    
 -- SET NOCOUNT ON added to prevent extra result sets from                    
 -- interfering with SELECT statements.                    
SET NOCOUNT ON;
IF(@IsLaunched =1 AND @ReportType = 1)--all recent launches or models only
	BEGIN                    
		SELECT	
			EC.Id, EC.LaunchDate, EC.ExpectedLaunch, EC.CarModelId,
			CMA.Name +'-'+ CMO.Name + (CASE WHEN EC.CarVersionId IS NOT NULL THEN '-'+CV.Name ELSE '' END) AS CarName,CMA.ID AS CarMakeId,
			EC.EstimatedPriceMin, EC.EstimatedPriceMax, EC.IsDeleted, EC.IsLaunched, EC.Priority, EC.CWConfidence, ISNULL(EC.CarVersionId,0) AS CarVersionId
		FROM ExpectedCarLaunches EC(NOLOCK)             
		INNER JOIN CarModels CMO(NOLOCK) ON EC.CarModelId = CMO.ID             
		INNER JOIN CarMakes CMA(NOLOCK) ON CMO.CarMakeId = CMA.ID
		LEFT JOIN CarVersions CV(NOLOCK) ON CV.ID = EC.CarVersionId
		WHERE CMO.New = 1 AND EC.IsLaunched=@IsLaunched AND EC.IsDeleted=@IsActive        
		AND CMO.New=1
		ORDER BY EC.EntryDate DESC
	END   
ELSE IF(@IsLaunched =1 AND @ReportType = 2)--recent launches with models
	 BEGIN
		SELECT 
			EC.Id, EC.LaunchDate, EC.ExpectedLaunch, EC.CarModelId, CMA.Name +'-'+ CMO.Name AS CarName,CMA.ID AS CarMakeId,
			EC.EstimatedPriceMin, EC.EstimatedPriceMax, EC.IsDeleted, EC.IsLaunched, EC.Priority, EC.CWConfidence, ISNULL(EC.CarVersionId,0) AS CarVersionId
		FROM ExpectedCarLaunches EC(NOLOCK)            
		INNER JOIN CarModels CMO(NOLOCK) ON EC.CarModelId = CMO.ID             
		INNER JOIN CarMakes CMA(NOLOCK) ON CMO.CarMakeId = CMA.ID
		WHERE EC.IsLaunched=@IsLaunched AND EC.IsDeleted=@IsActive          
		AND CMO.New=1 AND EC.CarVersionId IS NULL
		ORDER BY EC.LaunchDate DESC
	 END
ELSE IF(@IsLaunched =1  AND @ReportType = 3)--recent launches with versions
	 BEGIN
		SELECT 
			EC.Id, EC.LaunchDate, EC.ExpectedLaunch, EC.CarModelId, CMA.Name +'-'+ CMO.Name+ (CASE WHEN EC.CarVersionId IS NOT NULL THEN '-'+CV.Name ELSE '' END) AS CarName,CMA.ID AS CarMakeId,
			EC.EstimatedPriceMin, EC.EstimatedPriceMax, EC.IsDeleted,EC.IsLaunched ,EC.Priority,EC.CWConfidence, ISNULL(EC.CarVersionId,0) AS CarVersionId
		FROM ExpectedCarLaunches EC(NOLOCK)             
		INNER JOIN CarModels CMO(NOLOCK) ON EC.CarModelId = CMO.ID             
		INNER JOIN CarMakes CMA(NOLOCK) ON CMO.CarMakeId = CMA.ID
		INNER JOIN CarVersions CV(NOLOCK) ON CV.ID = EC.CarVersionId
		WHERE EC.IsLaunched=@IsLaunched AND EC.IsDeleted=@IsActive          
		AND CMO.New=1 
		ORDER BY EC.LaunchDate DESC
	 END
ELSE IF (@IsFuturistic =1 AND @ReportType = 1)--all upcoming cars 
	BEGIN
		SELECT 
			EC.Id, EC.LaunchDate, EC.ExpectedLaunch, EC.CarModelId, 
			CMA.Name +'-'+ CMO.Name +(CASE WHEN EC.CarVersionId IS NOT NULL THEN (SELECT '-'+C.Name FROM CarVersions C WHERE C.ID = EC.CarVersionId) ELSE '' END) AS CarName,
			CMA.ID AS CarMakeId, EC.EstimatedPriceMin, EC.EstimatedPriceMax , EC.IsDeleted, EC.IsLaunched ,
			EC.Priority, EC.CWConfidence,ISNULL(EC.CarVersionId,0) AS CarVersionId,
			CASE WHEN  DATEADD(dd, 2, DATEDIFF(dd, 0, GETDATE())) = DATEADD(dd, 0, DATEDIFF(dd, 0, EC.LaunchDate))  THEN 1 ELSE 0 END  AS IsLaunching,
			EC.CarVersionId, CMO.Futuristic,  ISNULL(EC.CarVersionId,0) AS CarVersionId
		FROM ExpectedCarLaunches EC(NOLOCK)              
		INNER JOIN CarModels CMO(NOLOCK) ON EC.CarModelId = CMO.ID             
		INNER JOIN CarMakes CMA(NOLOCK) ON CMO.CarMakeId = CMA.ID      
		WHERE EC.IsDeleted=@IsActive AND EC.IsLaunched = @IsLaunched
		ORDER BY CASE WHEN EC.Priority Is NULL Then 1 Else 0 End,EC.Priority ASC,EC.EntryDate DESC --upcomming order change
	END
ELSE IF (@IsFuturistic =1 AND @ReportType = 2)--all upcoming cars or with models
	BEGIN
		SELECT 
			EC.Id, EC.LaunchDate, EC.ExpectedLaunch, EC.CarModelId, CMA.Name +'-'+ CMO.Name AS CarName,CMA.ID AS CarMakeId,
			EC.EstimatedPriceMin, EC.EstimatedPriceMax , EC.IsDeleted,EC.IsLaunched ,EC.Priority,EC.CWConfidence,
			CASE WHEN  DATEADD(dd, 2, DATEDIFF(dd, 0, GETDATE())) = DATEADD(dd, 0, DATEDIFF(dd, 0, EC.LaunchDate))  THEN 1 ELSE 0 END  AS IsLaunching, ISNULL(EC.CarVersionId,0) AS CarVersionId
		FROM ExpectedCarLaunches EC(NOLOCK)              
		INNER JOIN CarModels CMO(NOLOCK) ON EC.CarModelId = CMO.ID             
		INNER JOIN CarMakes CMA(NOLOCK) ON CMO.CarMakeId = CMA.ID             
		WHERE EC.IsDeleted=@IsActive AND EC.IsLaunched = @IsLaunched AND ISNULL(EC.CarVersionId,0) =0
		ORDER BY CASE WHEN EC.Priority Is NULL Then 1 Else 0 End,EC.Priority ASC,EC.EntryDate DESC --upcomming order change
	END                 
ELSE IF (@IsFuturistic =1 AND @ReportType = 3)--upcoming with versions
	BEGIN
		SELECT 
			EC.Id, EC.LaunchDate, EC.ExpectedLaunch, EC.CarModelId, CMA.Name +'-'+ CMO.Name+'-'+CV.Name AS CarName,CMA.ID AS CarMakeId,
			EC.EstimatedPriceMin, EC.EstimatedPriceMax , EC.IsDeleted,EC.IsLaunched ,EC.Priority,EC.CWConfidence,
			CASE WHEN  DATEADD(dd, 2, DATEDIFF(dd, 0, GETDATE())) = DATEADD(dd, 0, DATEDIFF(dd, 0, EC.LaunchDate))  THEN 1 ELSE 0 END  AS IsLaunching, ISNULL(EC.CarVersionId,0) AS CarVersionId
		FROM ExpectedCarLaunches EC(NOLOCK)              
		INNER JOIN CarModels CMO(NOLOCK) ON EC.CarModelId = CMO.ID             
		INNER JOIN CarMakes CMA(NOLOCK) ON CMO.CarMakeId = CMA.ID             
		INNER JOIN CarVersions CV(NOLOCK) ON EC.CarVersionId = CV.ID
		WHERE EC.IsDeleted=@IsActive AND EC.IsLaunched = @IsLaunched AND EC.CarVersionId IS NOT NULL
		ORDER BY CASE WHEN EC.Priority Is NULL Then 1 Else 0 End,EC.Priority ASC,EC.EntryDate DESC  --upcomming order change
	END
END                 
    
  