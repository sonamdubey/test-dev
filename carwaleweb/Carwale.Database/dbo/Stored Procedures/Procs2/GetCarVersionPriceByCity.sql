IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarVersionPriceByCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarVersionPriceByCity]
GO

	-- =============================================          
-- Author:  <Vikas J>    
-- Create date: <15/02/2013>          
-- Description: <Returns the ex show room price of all the version available for the provided modelId and CityId.    
--Modified by Prashant Vishe on 22/04/2013 for eliminating discontinued cars  
-- =============================================          
CREATE PROCEDURE [dbo].[GetCarVersionPriceByCity] -- EXEC GetCarVersionPriceByCity 263, 1     
 @ModelId INT = 0, --Model Id of car for which ex showroom price needed    
 @CityId  INT = 0 --city Id in which price is needed    
AS    
BEGIN    
 --Returns the ex show room price of all the version available for the provided a given model in a given city    
 SELECT CV.ID AS ID, CV.Name AS Version, CV.CarModelId AS ModelId,     
  Displacement, FuelType, TransmissionType, SeatingCapacity, MileageOverall, NCP.Price AS ExshowroomPrice,    
  CV.New,ZC.Name AS City,     
  IsNull(CV.ReviewRate, 0) AS ReviewRate,     
  IsNull(CV.ReviewCount, 0) AS ReviewCount     
 FROM CarVersions AS CV WITH(NOLOCK)     
 INNER JOIN NewCarShowroomPrices NCP WITH(NOLOCK) ON NCP.CarVersionId=CV.ID     
 INNER JOIN NewCarSpecifications NS WITH(NOLOCK) ON CV.ID=NS.CarVersionID    
 INNER JOIN Cities ZC WITH(NOLOCK) ON ZC.ID=NCP.CityId    
 WHERE CV.IsDeleted=0 AND CV.CarModelId=@ModelId    
 AND NCP.CityId=@CityId AND CV.New = 1   --Modified by Prashant Vishe for eliminating discontinued cars 
 ORDER BY CV.New DESC, ExshowroomPrice ASC, CV.Name          
END     