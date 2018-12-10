IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerModelsView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerModelsView]
GO

	
------------------------------------------------------------
        
-- ============================================= [TC_DealerModelsView] 5      
-- Author:  Kritika Choudhary     
-- Create date : 21st May 2015     
-- Description : To view Dealer models
-- Modified by : Mihir Chheda(27-05-2015) [IsDeleted Column fetched]
-- Modified by : Kritika Choudhary on 30th June 2015, order by Make and Model
-- Modified by : Komal Manjare on 7th August 2015
-- OrignalImgPath fetched
-- =============================================      
CREATE PROCEDURE [dbo].[TC_DealerModelsView]      
(        
 @DealerId INT
    
)          
AS      
BEGIN      
 SET NOCOUNT ON;      
 
   SELECT DM.ID AS Id,DWModelName,CM.Name AS Model,CMK.Name AS Make,DM.HostUrl AS HostUrl,DM.OriginalImgPath AS OriginalImgPath,
   ImgPath,ImgName,DM.CWModelId AS ModelId,CMK.ID AS MakeId,DM.IsDeleted, DM.DWBodyStyleId AS BodyStyleId, BS.BodyStyleName AS BodyStyleName   
   FROM TC_DealerModels DM  
   LEFT JOIN Microsite_ModelBodyStyles BS ON DM.DWBodyStyleId=BS.Id   
   JOIN CarModels CM ON CM.ID=DM.CWModelId
   JOIN CarMakes CMK ON CM.CarMakeId=CMK.ID      
   WHERE DM.DealerId=@DealerId 
   ORDER BY Make,Model    --AND DM.IsDeleted = 0 //Removed by Mihir
  
       
END 