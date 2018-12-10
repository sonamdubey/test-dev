IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerVersions_View]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerVersions_View]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
    
      
-- ============================================= [TC_DealerVersions_View] 5      
-- Author:  Kritika Choudhary     
-- Create date: 2nd June 2015     
-- Description: To view Dealer car versions
-- =============================================      
CREATE PROCEDURE [dbo].[TC_DealerVersions_View]      
(     
   
 @DealerId INT       
)        
AS      
BEGIN      
 SET NOCOUNT ON;      
 
   SELECT DM.ID AS ID,CMK.Name AS CMake,CMO.Name AS CModel, CV.Name AS CVersion, CMK.Name AS DMake,
   DMO.DWModelName AS DModel,DM.DWVersionName AS DVersion,CMK.ID AS CMakeId,CMO.ID AS CModelId,CV.ID AS CVersionId,
   DMO.ID AS DModelId,DM.IsDeleted AS IsDeleted
   FROM TC_DealerVersions DM
   JOIN TC_DealerModels DMO ON DM.DWModelId=DMO.ID
   JOIN CarVersions CV ON CV.ID=DM.CWVersionId    
   JOIN CarModels CMO ON CV.CarModelId=CMO.ID 
   Join CarMakes CMK ON CMO.CarMakeId=CMK.ID    
  
    
   WHERE DM.DealerId=@DealerId ORDER BY CMake, Cmodel, Cversion   
  
       
END 

