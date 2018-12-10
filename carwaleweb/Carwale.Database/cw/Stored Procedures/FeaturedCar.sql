IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[FeaturedCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[FeaturedCar]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ================================================    
-- Template generated from Template Explorer using:    
-- Create Procedure (New Menu).SQL    
--    
-- Use the Specify Values for Template Parameters     
-- command (Ctrl-Shift-M) to fill in the parameter     
-- values below.    
--    
-- This block of comments will not be included in    
-- the definition of the procedure.    
-- ================================================    
    
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description    
CREATE PROCEDURE [cw].[FeaturedCar]    
 -- Add the parameters for the stored procedure here    
      
     
     
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
 SELECT Top 1 FL.ID FeaturedId, CMO.Id AS ModelId, CMA.Name AS MakeName,
  CMO.Name AS ModelName, '' AS VersionName, IsModel,FL.Description, 
  ShowResearch,  ShowPrice, ShowRoadTest, Link, FL.HostUrl, E.BasicId
 FROM Con_FeaturedListings AS FL  
 INNER JOIN CarModels AS CMO ON CMO.ID = FL.CarId  
 INNER JOIN CarMakes AS CMA ON CMA.ID = CMO.CarMakeId 
 LEFT JOIN (SELECT EC.BasicId, EC.ModelId FROM Con_EditCms_Cars EC INNER JOIN Con_EditCms_Basic EB ON EB.Id = EC.BasicId AND EB.CategoryId = 8) AS E ON E.ModelId = CMO.ID 
 WHERE FL.IsModel = 1 AND FL.IsVisible = 1 AND FL.IsActive = 1 ORDER BY FeaturedId DESC 
      
END 








