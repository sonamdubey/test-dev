IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ModelBanners_View]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ModelBanners_View]
GO

	
-----------------------------------------------------
      
-- ============================================= [Microsite_ModelBanners_View] 5      
-- Author:  Kritika Choudhary     
-- Create date: 8th June 2015     
-- Description: To view Dealer Model Banners
-- Modified by : Kritika Choudhary on 3rd July 2015, added Isbanner and ThumbImage
-- Modified By : Komal Manjare on 7th August 2015,OriginalImgPath fetched

-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_ModelBanners_View]      
(     
 @DealerId	INT
       
)        
AS      
BEGIN      
 SET NOCOUNT ON;      
 
   SELECT DB.ID AS ID, CMK.Name AS DMake,DMO.DWModelName AS DModel,CMK.ID AS CMakeId,
   DMO.ID AS DModelId,DB.IsActive AS IsActive,DB.HostUrl AS HostUrl,DB.ImgPath AS ImgPath,
   DB.ImgName AS ImgName,DB.IsMainImg AS IsMainImg,DB.SortOrder AS SortOrder,IsBanner,
   DB.OriginalImgPath AS OriginalImgPath
   FROM Microsite_DealerModelBanners DB
   JOIN TC_DealerModels DMO ON DB.DWModelId=DMO.ID
   JOIN CarModels CMO ON DMO.CWModelId=CMO.ID 
   Join CarMakes CMK ON CMO.CarMakeId=CMK.ID    
  
   WHERE DB.DealerId=@DealerId 
   ORDER BY DMake, DModel   
  
       
END 