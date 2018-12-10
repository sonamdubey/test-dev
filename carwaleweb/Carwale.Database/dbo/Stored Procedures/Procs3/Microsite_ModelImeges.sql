IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ModelImeges]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ModelImeges]
GO

	-- =============================================  
-- Author:  Chetan Kane  
-- Create date: 20/03/2012  
-- Description: This SP will be used for the home page Model slider for Microsites  
-- Umesh 20-02-2013 Added HostURL for Dynamic image path 
-- Modified By: Rakesh Yadav On 05 Aug 2015, to get OriginalImgPath
-- EXEC [dbo].[Microsite_ModelImeges] 1550,2
-- =============================================  
CREATE PROCEDURE [dbo].[Microsite_ModelImeges]    
 -- Add the parameters for the stored procedure here  
 @DealerId INT,  
 @MakeID INT  
AS  
BEGIN  
   
 SET NOCOUNT ON;  
   
   -- Umesh 20-02-2013 Added HostURL for Dynamic image path 
	SELECT DISTINCT CM.HostURL,CM.SmallPic,CM.OriginalImgPath, (MK.Name + ' ' + CM.Name) As ModelName, Cm.Id ModelId  
	,(SELECT TOP 1 MIN(Price) FROM NewCarShowroomPrices NCP WITH(NOLOCK) WHERE NCP.CarVersionId IN  
	(SELECT ID FROM CarVersions WITH(NOLOCK) WHERE CarModelId = CM.ID AND New = 1)  
	AND NCP.CityId = (SELECT CityId FROM Dealer_NewCar WITH(NOLOCK) WHERE Id = @DealerId)) AS MinPrice  
	FROM CarModels AS CM WITH(NOLOCK) inner join  CarMakes Mk WITH(NOLOCK) ON CM.CarMakeId=Mk.ID  
	inner join CarVersions AS CV WITH(NOLOCK) on CV.CarModelId = CM.ID  
	inner join NewCarSpecifications NS WITH(NOLOCK) on NS.CarVersionID=CV.ID  
	WHERE CM.IsDeleted = 0 and CM.New=1 and Mk.IsDeleted=0 and CM.CarMakeId =@MakeID order by MinPrice  
   
END 
