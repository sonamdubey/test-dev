IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerOffersEdit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerOffersEdit]
GO

	
-- =============================================  
-- Author:  Chetan Kane  
-- Create date: 13/3/2012  
-- Description: To View all all Dealer offers on dealer side
-- Modified By VIkas J  on 28 Jul 2013, JOIN with Microsite_OfferModels and Microsite_OfferCities added
-- Modified By Kritika Choudhary on 8th May 2015, added @IsFeature in select query
-- Modified By Kritika Choudhary on 14th July 2015, added HostUrl, ImgPath, ImgName, SlugImgName in select query
-- Modified by Komal Manjare on 7th August 2015,
-- OriginalImgPath fetched
--  Modified By Kritika Choudhary on 10-09-2015, Added parameters from Microsite_OfferImages
-- =============================================  
CREATE PROCEDURE [dbo].[Microsite_DealerOffersEdit]  
(    
 @DealerId INT,  
 @Id INT  
)    
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
  DECLARE @VersionId INT
  DECLARE @MakeId INT
  DECLARE @ModelId INT

  SET @VersionId = (SELECT Top 1 VersionId FROM  Microsite_OfferVersions WITH(NOLOCK) WHERE DealerId = @DealerId AND OfferId = @Id)
  SELECT @MakeId = MakeId ,@ModelId = ModelId FROM vwAllMMV WITH(NOLOCK) WHERE VersionId = @VersionId
    -- Insert statements for procedure here  
 SELECT MD.Id,OfferTitle,OfferDetails,OfferStartDate,OfferEndDate,OfferTermsCondition,OfferContent,OC.CityId,
        OV.VersionId, @MakeId AS MakeId, @ModelId AS ModelId,IsFeatured, MD.HostUrl, MD.OriginalImgPath, MDI.HostUrl AS SlugHostUrl,
		MDI.OriginalImgPath AS SlugOriginalImgPath, MDI.Id AS OfferImgId, MDI.IsActive AS SlugIsActive 
 FROM Microsite_DealerOffers MD
    LEFT JOIN Microsite_OfferImages MDI ON MDI.OfferId = MD.Id
	--LEFT OUTER JOIN Microsite_OfferModels OM ON OM.DealerId=@DealerId AND OM.offerid=@Id AND MD.Id=OM.offerid 
	LEFT OUTER JOIN Microsite_OfferCities OC ON OC.DealerId=@DealerId AND OC.offerid=@Id AND  MD.Id=OC.offerid 
	LEFT OUTER JOIN Microsite_OfferVersions OV ON OC.DealerId=@DealerId AND OV.offerid=@Id AND  MD.Id=OC.offerid 
	WHERE MD.DealerId = @DealerId AND MD.Id = @Id
 
END  
  
  
