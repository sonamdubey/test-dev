IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveRecommendCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveRecommendCars]
GO

	
-- =============================================  
-- Author:Prashant Vishe      
-- Create date:16 Oct 2013   
-- Description: for retrieving recommend cars...   
-- Modified By Prashant On 14 Nov 2013 
-- Modification:added queries for retrieving only new versions... 
---Modified By Prashant On 7 Jan 2014 
-- Modification:added queries for retrieving version related information... 
--Modified By Prashant On 20 Feb 2014
--Modification:modified query for retrieving version related information...
-- =============================================  
-- EXEC RetrieveRecommendCars 557
CREATE PROCEDURE [dbo].[RetrieveRecommendCars] 
  -- Add the parameters for the stored procedure here  
  @ModelId NUMERIC 
AS 
  BEGIN 
      -- SET NOCOUNT ON added to prevent extra result sets from  
      -- interfering with SELECT statements.  
      SET nocount ON; 

      -- Insert statements for procedure here  
      SELECT DISTINCT ( RC.recommendcarid ), 
                      CM.name  AS makename, 
                      CMO.name AS modelname, 
                      RC.versionid, 
                      CV.name  AS versionname, 
                      RC.dimensionandspace, 
                      RC.comfort, 
                      RC.performance, 
                      RC.convenience, 
                      RC.safety, 
                      RC.entertainment, 
                      RC.aesthetics, 
                      RC.salesandsupport, 
                      RC.fueleconomy, 
                      RC.isactive 
      FROM   recommendcars RC with(nolock)
             INNER JOIN carversions CV 
                     ON CV.id = RC.versionid 
             INNER JOIN carmodels CMO 
                     ON CMO.id = CV.carmodelid 
             INNER JOIN carmakes CM 
                     ON CM.id = CMO.carmakeid 
      WHERE  CMO.Id = @ModelId 
             AND CV.new = 1 
             AND CV.futuristic = 0
			 AND CV.IsDeleted = 0
			 AND CMO.IsDeleted = 0 
  END 
