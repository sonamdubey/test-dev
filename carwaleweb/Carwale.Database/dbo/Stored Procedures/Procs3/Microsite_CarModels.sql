IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_CarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_CarModels]
GO

	-- Author:  Umesh Ojha  
-- Create date: 19/4/2012  
-- Description: Listing all models for the Car Makes in the Cars listing page using in NCD
-- AM Modified  06-09-2012 not to show certain models to dealers 
-- Modified By: Rakesh Yadav to get originalImgPath
-- =============================================  
CREATE PROCEDURE [dbo].[Microsite_CarModels]  
(    
 @DealerId INT,  
 @CarMakeId INT  
)    
AS  
BEGIN  
 SELECT Mo.ID, (Mk.Name +' '+Mo.Name) ModelName,Mk.Name Make,MO.HostURL,MO.OriginalImgPath,  
 Mo.SmallPic,(SELECT TOP 1 MIN(Price) FROM NewCarShowroomPrices NCP with(nolock) 
 WHERE NCP.CarVersionId IN (SELECT ID FROM CarVersions  
 WHERE CarModelId = Mo.ID AND New = 1) AND NCP.CityId = (SELECT CityId FROM Dealers WHERE Id = @DealerId)) AS MinPrice  
 FROM CarModels Mo INNER JOIN CarMakes Mk On Mk.ID = Mo.CarMakeId  
 WHERE Mo.CarMakeId =@CarMakeId AND Mo.IsDeleted = 0 and Mo.New=1 
 -- AM Modified  06-09-2012 not to show certain models to dealers  
 AND MO.ID not in (
					SELECT ModelId FROM TC_NoDealerModels WHERE DealerId =@DealerId
				  ) 
 ORDER BY MinPrice  
END