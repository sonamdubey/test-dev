IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerOffersView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerOffersView]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
    
      
-- ============================================= [Microsite_DealerOffersView] 5      
-- Author:  Chetan Kane      
-- Create date: 24/2/2012      
-- Description: To view Dealer offers    
-- modified by: Vikas Jyoty on 26/7/13 ORDER BY ID DESC phrase added to select statement 
-- modified by: Kritika Choudhary on 8th May 2015, added @IsFeature column to each query    
-- modified by: Kritika Choudhary on 14th May 2015, added @MakeId parameter  
-- modified by: Kritika Choudhary on 18th May 2015, added JOIN Microsite_OfferModels
-- modified by: Kritika Choudhary on 14th July 2015, added HostUrl, ImgPath, ImgName and SlugImgName
--Modified By : Rakesh Yadav on 06 Aug 2015, added OriginalImgPath
--Modified By : Kritika Choudhary on 14th Aug 2015, deleted imgpath,imgname and slugimag
--  Modified By Kritika Choudhary on 10-09-2015, Added parameters from Microsite_OfferImages
-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_DealerOffersView]      
(        
 @DealerId INT,       
 @IsActive BIT = 0,
 @MakeId INT = -1      
)        
AS      
BEGIN      
 SET NOCOUNT ON;      
 IF(@IsActive = 1) -- For DealerWebSite      
  BEGIN      
   SELECT MD.Id,OfferTitle,OfferDetails,OfferStartDate,OfferEndDate,OfferTermsCondition,OfferContent,IsActive,ISNULL(C.Name,'') as City,ISNULL(CM.Name,'') as Model,IsFeatured       
   FROM Microsite_DealerOffers MD      
   LEFT OUTER JOIN Cities C ON C.ID=MD.CityId      
   LEFT OUTER JOIN CarModels CM ON CM.ID=MD.ModelId      
   WHERE DealerId=@DealerId AND MD.IsDeleted = 0 AND IsActive = 1 AND ( OfferEndDate   > = GETDATE() OR OfferEndDate IS NULL)  ORDER BY ID DESC    
  END      
       
      
 ELSE   -- For ManageWebSite       
  BEGIN    
  SELECT MD.Id,OfferTitle,OfferDetails,OfferStartDate,OfferEndDate,OfferTermsCondition,OfferContent,
  MD.IsActive,ISNULL(C.Name,'') as City, ISNULL(CM.Name,'') as Model,IsFeatured, MD.HostUrl,MD.OriginalImgPath,
  MDI.HostUrl AS SlugHostUrl, MDI.OriginalImgPath AS SlugOriginalImgPath, MDI.IsActive AS SlugIsActive    
  FROM Microsite_DealerOffers MD   
  LEFT JOIN Microsite_OfferImages MDI ON MDI.OfferId = MD.Id  
  JOIN Microsite_OfferModels MO ON MD.Id=MO.OfferId 
  LEFT OUTER JOIN Cities C ON C.ID=MD.CityId   
  LEFT OUTER JOIN CarModels CM ON CM.ID=MO.ModelId    
  WHERE MD.DealerId=@DealerId AND (@MakeId=-1 OR CM.CarMakeId=@MakeId) AND MD.IsDeleted = 0 AND ( OfferEndDate   > = GETDATE() OR OfferEndDate IS NULL) ORDER BY ID DESC     
 END
 	   
      
       
END 
