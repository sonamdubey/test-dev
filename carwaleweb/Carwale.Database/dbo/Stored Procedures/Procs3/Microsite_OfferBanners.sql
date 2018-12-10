IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_OfferBanners]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_OfferBanners]
GO

	-- ============================================= [Microsite_OfferBanners] 5      
-- Author: Kritika Choudhary    
-- Create date: 21st July 2015     
-- Description: To retrieve offers banners 
-- Modified by: Kritika CHoudhary on 14th Aug, deleted imgpath and imgname  
-- Modified by: Vaibhav K 2-Dec-2015 Condition for Offer Start and End date added
-- Distinct records to be fetched for the offer banner images
-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_OfferBanners]      
(        
 @DealerId    INT,  
 @ModelId     INT=NULL,
 @UseDWModels BIT,
 @CityId      INT		
)        
AS      
BEGIN      
if(@ModelId IS NULL)
BEGIN
	 SELECT HostUrl,OriginalImgPath
	 FROM Microsite_DealerOffers WITH(NOLOCK)
	 WHERE  DealerId = @DealerId AND IsFeatured = 1 AND IsActive = 1 AND IsDeleted = 0 AND OriginalImgPath IS NOT NULL
	AND (OfferEndDate >= GETDATE() OR OfferEndDate IS NULL)
	AND (OfferStartDate <= GETDATE() OR OfferStartDate IS NULL)
END   
ELSE
	IF @UseDWModels = 1
	BEGIN
		SELECT Distinct DO.HostUrl, DO.OriginalImgPath
		FROM Microsite_DealerOffers DO WITH(NOLOCK)
		JOIN Microsite_OfferVersions OV WITH(NOLOCK) ON DO.Id=OV.OfferId
		JOIN Microsite_OfferCities OC WITH(NOLOCK) ON OC.OfferId= DO.Id
		JOIN TC_DealerVersions DV WITH(NOLOCK) ON OV.VersionId= DV.CWVersionId
		WHERE  DO.DealerId = @DealerId AND DO.IsActive = 1 AND DO.IsDeleted = 0 
		AND DO.OriginalImgPath IS NOT NULL AND DV.DWModelId=@ModelId AND OC.CityId= @CityId
		AND (DO.OfferEndDate >= GETDATE() OR DO.OfferEndDate IS NULL)
		AND (DO.OfferStartDate <= GETDATE() OR DO.OfferStartDate IS NULL)
	END      
    ELSE
	BEGIN
		SELECT DO.HostUrl, DO.OriginalImgPath
		FROM Microsite_DealerOffers DO WITH(NOLOCK)
		JOIN Microsite_OfferModels OM WITH(NOLOCK) ON DO.Id=OM.OfferId 
		JOIN Microsite_OfferCities OC WITH(NOLOCK) ON OC.OfferId= DO.Id
		WHERE  DO.DealerId = @DealerId AND DO.IsActive = 1 AND DO.IsDeleted = 0 
		AND DO.OriginalImgPath IS NOT NULL AND OM.ModelId=@ModelId AND OC.CityId= @CityId
		AND (DO.OfferEndDate >= GETDATE() OR DO.OfferEndDate IS NULL)
		AND (DO.OfferStartDate <= GETDATE() OR DO.OfferStartDate IS NULL)
	END   
END 
