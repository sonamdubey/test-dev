IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FillCarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FillCarModels]
GO

	
-- =============================================    
-- Author:Prashant Vishe      
-- Create date: <20 sept 2013>    
-- Description: for filling car models data...    
--Modified By:prashant vishe on 25 sept 2013  
--Modification:added query for selecting MaskingName  
--Modified By:Prashant Vishe On 29 Jan 2014 
--Modification:added query for retrieving car model roots 
-- =============================================    
CREATE PROCEDURE [dbo].[FillCarModels] 
  -- Add the parameters for the stored procedure here    
  @ModelId NUMERIC 
AS 
  BEGIN 
      -- SET NOCOUNT ON added to prevent extra result sets from    
      -- interfering with SELECT statements.    
      SET nocount ON; 

      -- Insert statements for procedure here    
      SELECT Mo.name, 
             MO.used, 
             MO.new, 
             MO.indian, 
             MO.imported, 
             MO.classic, 
             MO.modified, 
             MO.futuristic, 
             ( CASE 
                 WHEN TSC.status IS NULL THEN 0 
                 ELSE TSC.status 
               END ) AS IsTopSelling, 
             Mo.discontinuationid, 
             Mo.replacedbymodelid, 
             Mo.comment, 
             Mo.discontinuation_date, 
             Mo.maskingname, 
             Mo.platform, 
             Mo.generation, 
             Mo.upgrade, 
             Mo.ModelLaunchDate as LaunchDate, 
             CR.rootname, 
             Mo.rootid 
      FROM   carmodels Mo 
             LEFT JOIN con_topsellingcars TSC 
                    ON TSC.modelid = Mo.id 
             LEFT JOIN carmodelroots CR 
                    ON CR.rootid = Mo.rootid 
      WHERE  MO.isdeleted = 0 
             AND Mo.id = @ModelId 
  END 
