IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerOffers_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerOffers_Save]
GO

	-- =============================================      
-- Author:  Chetan Kane  
-- Create date:   
-- Description: To Save and Update Dealer offer for the DealerWebSite   
-- Modified By : Tejashree Patil on 9 May 2013, Increase @OfferTitle size to 300 from 50,@OfferDetails,@OfferContent,@OfferTermsCondition to MAX  
-- Modified By : Vikas J on 28 Jul 2013, Insert for Microsite_OfferCities and Microsite_OfferModels tables also being done in the same SP.
-- Modified By Vivek Gupta on 17-02-2014, Added an Update query to update top 1 cityId in Microsite_DealerOffers
-- Modified By Vivek Gupta on 10-04-2015, Added Parameter @VersionId
-- Modified By Kritika Choudhary on 07-05-2015, Added Parameter @IsFeatured
-- Modified By Kritika Choudhary on 14-07-2015, Added Parameter @HostUrl, @ImgPath ,@ImgName and @SlugImgName
-- Modified By Komal Manjare on 7th August 2015
-- Parameter added @OriginalImgPath
-- Modified By Komal Manjare on 11th August 2015
-- Versioning of OriginalImgPath on basis of current date
--  Modified By Kritika Choudhary on 10-09-2015, Added query for Microsite_OfferImages
-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_DealerOffers_Save]  
(      
 @DealerId INT,      
 @Id  INT=NULL OUTPUT,    
 @OfferTitle VARCHAR(300) = NULL,  
 @TillStockLast BIT = NULL,  
 @OfferDetails VARCHAR(MAX) = NULL,  
 @OfferStartDate DATETIME = NULL,  
 @OfferEndDate DATETIME = NULL,  
 @OfferTermsCondition VARCHAR(MAX) = NULL,  
 @OfferContent VARCHAR(MAX) = NULL,  
 @CityId VARCHAR(MAX),  
 @ModelId VARCHAR(MAX),
 @IsFeatured BIT = NULL,
 @VersionId VARCHAR(MAX) = NULL,
 @HostUrl    VARCHAR(50)=NULL,
 @ImgPath    VARCHAR(100)=NULL,
 @ImgName    VARCHAR(50)=NULL,
 @SlugImgName    VARCHAR(50)=NULL,
 @OriginalImgPath VARCHAR(300)=NULL,
 @FlagForSlug    BIT=0,
 @OfferImgId  INT=NULL OUTPUT, 
 @SlugOriginalImgPath VARCHAR(300)=NULL,
 @IsActive   BIT=1
)      
AS      
BEGIN   
 
  -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON;      
 IF(@Id Is NULL)    
  BEGIN      
   INSERT INTO Microsite_DealerOffers(DealerId,OfferTitle,OfferDetails,TillStockLast,OfferStartDate,OfferEndDate,OfferTermsCondition,OfferContent,IsActive,CityId,ModelId,IsFeatured)     
   VALUES(@DealerId,@OfferTitle,@OfferDetails,@TillStockLast,@OfferStartDate,@OfferEndDate,@OfferTermsCondition,@OfferContent,0,null,null,@IsFeatured) ;
   SELECT @Id=SCOPE_IDENTITY(); 
   IF(@FlagForSlug = 1)
   BEGIN
	   INSERT INTO Microsite_OfferImages(OfferId)
	   VALUES (@Id)
	   SET @OfferImgId = SCOPE_IDENTITY(); 
	  
   END
  END      
 ELSE      
  BEGIN    
		DECLARE @todaydate datetime=GETDATE() 

	    IF(@ImgName IS NOT NULL) -- Versioning of the Image on basis of current date time
		BEGIN
			SET @ImgName = @ImgName + '?v=' + REPLACE(CONVERT(VARCHAR,@todaydate,112)+ CONVERT(VARCHAR,@todaydate,114),':','');
		END  

		 IF(@SlugImgName IS NOT NULL) -- Versioning of the Image on basis of current date time
		BEGIN
			SET @SlugImgName = @SlugImgName + '?v=' + REPLACE(CONVERT(VARCHAR,@todaydate,112)+ CONVERT(VARCHAR,@todaydate,114),':','');
		END  
		IF(@OriginalImgPath IS NOT NULL)
		BEGIN
		    SET @OriginalImgPath = @OriginalImgPath + '?v=' + REPLACE(CONVERT(VARCHAR,@todaydate,112)+ CONVERT(VARCHAR,@todaydate,114),':','');
		END
		IF(@SlugOriginalImgPath IS NOT NULL)
		BEGIN
		    SET @SlugOriginalImgPath = @SlugOriginalImgPath + '?v=' + REPLACE(CONVERT(VARCHAR,@todaydate,112)+ CONVERT(VARCHAR,@todaydate,114),':','');
		END

   UPDATE Microsite_DealerOffers   
   SET OfferTitle=ISNULL(@OfferTitle, OfferTitle),OfferDetails=ISNULL(@OfferDetails,OfferDetails),
   TillStockLast=ISNULL(@TillStockLast,TillStockLast),IsActive = 0,IsFeatured=ISNULL(@IsFeatured,IsFeatured),
    OfferStartDate=ISNULL(@OfferStartDate,OfferStartDate),OfferEndDate=ISNULL(@OfferEndDate,OfferEndDate),
	OfferTermsCondition=ISNULL(@OfferTermsCondition,OfferTermsCondition),OfferContent=ISNULL(@OfferContent,OfferContent),
	CityId=null,ModelId=null,HostUrl  = ISNULL(@HostUrl, HostUrl),
	OriginalImgPath=ISNULL(@OriginalImgPath,OriginalImgPath) 
   WHERE DealerId=@DealerId AND Id=@Id

   IF(@OfferImgId IS NOT NULL OR @OfferImgId!=-1)
   BEGIN
	   UPDATE Microsite_OfferImages
	   SET OfferId = ISNULL(@Id, OfferId), HostUrl  = ISNULL(@HostUrl, HostUrl), OriginalImgPath=ISNULL(@SlugOriginalImgPath,OriginalImgPath),
	   IsActive = ISNULL(@IsActive, IsActive), ModifiedDate= GETDATE()
	   WHERE Id = @OfferImgId
   END
   ELSE IF(@FlagForSlug = 1)
	BEGIN
		   INSERT INTO Microsite_OfferImages(OfferId, HostUrl, OriginalImgPath, IsActive, ModifiedDate, ImgType)
		   VALUES (@Id, @HostUrl,@SlugOriginalImgPath,1,GETDATE(),1)
		   SET @OfferImgId = SCOPE_IDENTITY(); 
	END

  END 
  --Updating the cities related to above offer
  DELETE FROM Microsite_OfferCities WHERE OfferId=@Id AND DealerId=@DealerId;
  INSERT INTO Microsite_OfferCities (CityId,DealerId,OfferId) SELECT items,@DealerId ,@Id from dbo.SplitText(@CityId,',');
  UPDATE Microsite_DealerOffers SET CityId = ( SELECT TOP 1 items from dbo.SplitText(@CityId,',')) -- Modified By Vivek Gupta on 17-02-2014
  WHERE DealerId=@DealerId AND Id=@Id     
  --Updating the Models related to above offer
  DELETE FROM Microsite_OfferModels WHERE OfferId=@Id AND DealerId=@DealerId;
  INSERT INTO Microsite_OfferModels (ModelId,DealerId,OfferId) SELECT items,@DealerId ,@Id from dbo.SplitText(@ModelId,',');

   --Updating the versionss related to above offer
  DELETE FROM Microsite_OfferVersions WHERE OfferId=@Id AND DealerId=@DealerId;
  INSERT INTO Microsite_OfferVersions (VersionId,DealerId,OfferId) SELECT items,@DealerId ,@Id from dbo.SplitText(@VersionId,',');
  
  
END 
