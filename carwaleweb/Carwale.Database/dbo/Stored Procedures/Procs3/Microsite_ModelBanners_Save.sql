IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ModelBanners_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ModelBanners_Save]
GO

	----------------------------------------------------------------------------
-- =============================================      
-- Author:  Kritika Choudhary
-- Create date:  8th June 2015 
-- Description: To Save Dealer Model Banners  
-- Modified by : Kritika Choudhary on 3rd July 2015, added IsBanner and ThumbImage
-- Modified by : Komal Manjare on 7th August 2015 added parameter @OriginalImgPath
-- Modified by : Kritika Choudhary on 11th August 2015, done versioning of originalimgpath 
-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_ModelBanners_Save]  
(  
 @DealerId   INT=NULL,      
 @DWModelId  INT,
 @IsMainImg  BIT=NULL,
 @HostUrl    VARCHAR(50)=NULL,
 @ImgPath    VARCHAR(100)=NULL,
 @ImgName    VARCHAR(50)=NULL,
 @IsBanner   BIT= NULL,
 @ThumbImage VARCHAR(50)=NULL,
 @ID         INT=NULL OUTPUT,
 @OriginalImgPath VARCHAR(300)=NULL
)      
AS      
BEGIN   

  -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON; 
 IF(@IsMainImg=1)
	   BEGIN
		UPDATE Microsite_DealerModelBanners
        SET IsMainImg=0
        WHERE DWModelId= @DWModelId AND DealerId=@DealerId AND IsBanner=@IsBanner;
	   END
  IF(@ID IS NULL AND @DealerId IS NOT NULL)
  BEGIN
	   INSERT INTO Microsite_DealerModelBanners(DealerId,DWModelId,IsMainImg,IsBanner)    
	   VALUES(@DealerId,@DWModelId,@IsMainImg,@IsBanner)
	   SET @ID = SCOPE_IDENTITY();
 END
   ELSE
	BEGIN
	    DECLARE @todaydate datetime=GETDATE() 

	    IF(@ImgName IS NOT NULL) -- Versioning of the Image on basis of current date time
		BEGIN
			SET @ImgName = @ImgName + '?v=' + REPLACE(CONVERT(VARCHAR,@todaydate,112)+ CONVERT(VARCHAR,@todaydate,114),':','');
		END
		IF(@ThumbImage IS NOT NULL) -- Versioning of the Image on basis of current date time
		BEGIN
			SET @ThumbImage = @ThumbImage + '?v=' + REPLACE(CONVERT(VARCHAR,@todaydate,112)+ CONVERT(VARCHAR,@todaydate,114),':','');
		END
		IF(@OriginalImgPath IS NOT NULL) -- Versioning of the Image on basis of current date time
	    BEGIN
		     SET @OriginalImgPath = @OriginalImgPath + '?v=' + REPLACE(CONVERT(VARCHAR,@todaydate,112)+ CONVERT(VARCHAR,@todaydate,114),':','');
	    END
	 	
	    UPDATE Microsite_DealerModelBanners
		SET HostUrl  = ISNULL(@HostUrl, HostUrl), ImgPath = ISNULL(@ImgPath, ImgPath), ImgName= ISNULL(@ImgName, ImgName),
			IsMainImg = ISNULL(@IsMainImg, IsMainImg), ModifiedDate= GETDATE(),IsBanner= ISNULL(@IsBanner,IsBanner),
			OriginalImgPath=ISNULL(@OriginalImgPath,OriginalImgPath)
			
        WHERE ID=@ID;

	END
		   
END