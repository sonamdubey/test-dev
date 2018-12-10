IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerWebsiteOffer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerWebsiteOffer]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
  
-- =============================================    
-- Author:  Vikas J    
-- Create date: 29/7/2013    
-- Description: To View all all Dealer offers on dealer side (For SKODA Dealers) 
-- =============================================    
CREATE PROCEDURE [dbo].[Microsite_DealerWebsiteOffer]    
(      
 @DealerId	INT,
 @ModelId	INT = NULL,-- optional parametre 
 @CityId	INT = NULL -- optional parametre 
     
)      
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 SET NOCOUNT ON;    
 IF(@ModelId IS NULL AND @CityId IS NULL) --If only dealer id is provided  
    
	 SELECT MD.Id,OfferTitle,OfferDetails,OfferStartDate,OfferEndDate,OfferTermsCondition,OfferContent,OC.CityId,OM.ModelId,C.Name as City,CM.Name as Model  FROM Microsite_DealerOffers MD  
	 INNER JOIN Microsite_OfferModels OM ON OM.DealerId=@DealerId AND MD.Id=OM.offerid   
	 INNER JOIN Microsite_OfferCities OC ON OC.DealerId=@DealerId AND MD.Id=OC.offerid
	 INNER JOIN Cities C ON C.ID=OC.CityId   
	 INNER JOIN CarModels CM ON CM.ID=OM.ModelId
	 WHERE MD.DealerId = @DealerId AND MD.IsDeleted = 0 AND IsActive = 1 AND ( OfferEndDate   > = GETDATE() OR OfferEndDate IS NULL)  ORDER BY ID DESC;
   
 ELSE IF(@ModelId IS NULL) --If dealerId and CityId is provided
   	
	 SELECT MD.Id,OfferTitle,OfferDetails,OfferStartDate,OfferEndDate,OfferTermsCondition,OfferContent,OC.CityId,OM.ModelId,C.Name as City,CM.Name as Model  FROM Microsite_DealerOffers MD  
	 INNER JOIN Microsite_OfferModels OM ON OM.DealerId=@DealerId AND MD.Id=OM.offerid   
	 INNER JOIN Microsite_OfferCities OC ON OC.DealerId=@DealerId AND MD.Id=OC.offerid AND OC.CityId=@CityId
	 INNER JOIN Cities C ON C.ID=OC.CityId   
	 INNER JOIN CarModels CM ON CM.ID=OM.ModelId  
	 WHERE MD.DealerId = @DealerId AND MD.IsDeleted = 0 AND IsActive = 1 AND ( OfferEndDate   > = GETDATE() OR OfferEndDate IS NULL)  ORDER BY ID DESC;
   
 ELSE IF(@CityId IS NULL) --If dealerId and ModelId is provided
   	
	 SELECT MD.Id,OfferTitle,OfferDetails,OfferStartDate,OfferEndDate,OfferTermsCondition,OfferContent,OC.CityId,OM.ModelId,C.Name as City,CM.Name as Model  FROM Microsite_DealerOffers MD  
	 INNER JOIN Microsite_OfferModels OM ON OM.DealerId=@DealerId AND MD.Id=OM.offerid  AND OM.ModelId=@ModelId 
	 INNER JOIN Microsite_OfferCities OC ON OC.DealerId=@DealerId AND MD.Id=OC.offerid
	 INNER JOIN Cities C ON C.ID=OC.CityId   
	 INNER JOIN CarModels CM ON CM.ID=OM.ModelId   
	 WHERE MD.DealerId = @DealerId AND MD.IsDeleted = 0 AND IsActive = 1 AND ( OfferEndDate   > = GETDATE() OR OfferEndDate IS NULL)  ORDER BY ID DESC;
   
 ELSE						--If dealerId, ModelId and CityId is provided
	 SELECT MD.Id,OfferTitle,OfferDetails,OfferStartDate,OfferEndDate,OfferTermsCondition,OfferContent,OC.CityId,OM.ModelId,C.Name as City,CM.Name as Model  FROM Microsite_DealerOffers MD  
	 INNER JOIN Microsite_OfferModels OM ON OM.DealerId=@DealerId AND MD.Id=OM.offerid AND OM.ModelId=@ModelId  
	 INNER JOIN Microsite_OfferCities OC ON OC.DealerId=@DealerId AND MD.Id=OC.offerid AND OC.CityId=@CityId
	 INNER JOIN Cities C ON C.ID=OC.CityId   
	 INNER JOIN CarModels CM ON CM.ID=OM.ModelId  
	 WHERE MD.DealerId = @DealerId AND MD.IsDeleted = 0 AND IsActive = 1 AND ( OfferEndDate   > = GETDATE() OR OfferEndDate IS NULL)  ORDER BY ID DESC;
END    
    