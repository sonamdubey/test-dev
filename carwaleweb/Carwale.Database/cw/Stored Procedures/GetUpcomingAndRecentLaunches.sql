IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetUpcomingAndRecentLaunches]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetUpcomingAndRecentLaunches]
GO

	-- =============================================                    
-- Author:  <Prashant Vishe>                    
-- Create date: <26 Dec 2012>                    
-- Description:<Returns upcoming and Recently launched Cars >     
--Modified By:Parshant vishe on 29 aug 2013   added priority fetching query  
--Modified by:Prashant vishe on 31 Jan 2013    
--Modification:added query for retrieving carwale confidence related data

             
CREATE PROCEDURE [cw].[GetUpcomingAndRecentLaunches]                    
 -- Add the parameters for the stored procedure here                    
  @IsNew INT,            
  @IsActive INT,            
  @IsLaunched INT,            
  @IsFuturistic INT            
                    
AS                    
BEGIN                    
 -- SET NOCOUNT ON added to prevent extra result sets from                    
 -- interfering with SELECT statements.                    
 SET NOCOUNT ON;            
 if(@IsNew=1)  --recent launches           
  begin                    
   SELECT EC.Id, EC.LaunchDate, EC.ExpectedLaunch, EC.CarModelId, CMA.Name +'-'+ CMO.Name AS CarName,CMA.ID AS CarMakeId                
   , EC.EstimatedPriceMin, EC.EstimatedPriceMax , EC.IsDeleted,EC.IsLaunched ,EC.Priority,EC.CWConfidence   --modified by prashant         
   FROM ExpectedCarLaunches EC             
   INNER JOIN CarModels CMO ON EC.CarModelId = CMO.ID             
   INNER JOIN CarMakes CMA ON CMO.CarMakeId = CMA.ID             
   WHERE EC.IsLaunched=@IsLaunched AND EC.IsDeleted=@IsActive   and CMO.Futuristic=@IsFuturistic          
   AND CMO.New=1             
   ORDER BY EC.LaunchDate DESC              
  end            
 else -- upcoming            
  begin            
    SELECT EC.Id, EC.LaunchDate, EC.ExpectedLaunch, EC.CarModelId, CMA.Name +'-'+ CMO.Name AS CarName,CMA.ID AS CarMakeId               
   , EC.EstimatedPriceMin, EC.EstimatedPriceMax , EC.IsDeleted,EC.IsLaunched ,EC.Priority,EC.CWConfidence,
   CASE WHEN  DATEADD(dd, 2, DATEDIFF(dd, 0, GETDATE())) = DATEADD(dd, 0, DATEDIFF(dd, 0, LaunchDate))  THEN 1 ELSE 0 END  AS IsLaunching           
   FROM ExpectedCarLaunches EC              
   INNER JOIN CarModels CMO ON EC.CarModelId = CMO.ID             
   INNER JOIN CarMakes CMA ON CMO.CarMakeId = CMA.ID             
   WHERE EC.IsLaunched=@IsLaunched AND EC.IsDeleted=@IsActive and CMO.Futuristic=@IsFuturistic            
  ORDER BY CASE WHEN EC.Priority Is NULL Then 1 Else 0 End, EC.Priority ASC ,EC.LaunchDate ASC              
  end            
              
END                 
    
    

