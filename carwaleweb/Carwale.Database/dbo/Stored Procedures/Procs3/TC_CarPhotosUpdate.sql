IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarPhotosUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarPhotosUpdate]
GO

	-- Author  : Suri  
-- Create date : 09/10/2012 14:00 PM  
-- Description : will set IsActive 1
-- AM Modified 6-11-2012 to insert PhotoId from TC_CarPhotos to CarPhotos
-- Suri Modified 23-11-2012 makeing car approved =1
-- Suri Modified 13-03-2013 updating lastupdated date in stock table
-- Modified By Vivek Gupta on 11-08-2015, added @OrgImgPath to copy originalimgpath of the car photos
-- =============================================      
CREATE PROCEDURE [dbo].[TC_CarPhotosUpdate]      
 -- Add the parameters for the stored procedure here      
 @PhotoId   BIGINT
  
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;  
-- updating final image name  
UPDATE TC_CarPhotos SET IsActive=1
WHERE Id=@PhotoId  


DECLARE 
	@InquiryId BIGINT,
	@StockId BIGINT,
	@ImageUrlFull  VARCHAR(100),
	@ImageUrlThumb  VARCHAR(100),
	@ImageUrlThumbSmall VARCHAR(100),
	@IsMain BIT,
	@HostUrl VARCHAR(100),
	@DirPath VARCHAR(100),
	@OrgImgPath VARCHAR(100),
	@BrachId BIGINT
	
SELECT 
	@StockId=StockId,
	@BrachId = TCS.BranchId,
	@ImageUrlFull=ImageUrlFull,
	@ImageUrlThumb=ImageUrlThumb,	
	@ImageUrlThumbSmall=ImageUrlThumbSmall,
	@IsMain=IsMain ,
	@HostUrl=HostUrl,
	@DirPath=DirectoryPath,
	@OrgImgPath = OriginalImgPath
FROM TC_CarPhotos CP WITH(NOLOCK) INNER JOIN TC_stock TCS WITH(NOLOCK) ON CP.StockId = TCS.Id
WHERE CP.Id=@PhotoId 
	

--Updating stocks modified date
UPDATE TC_Stock SET LastUpdatedDate=GETDATE() WHERE Id=@StockId
	
SELECT @InquiryId = Id FROM SellInquiries Si WITH(NOLOCK) WHERE Si.Tc_StockId = @StockId AND StatusId=1 AND SI.DealerId = @BrachId AND SI.SourceId = 2 --2 for Autobiz
--Inserting the car photos details in the CarPhotos table
IF (@InquiryId IS NOT NULL)
BEGIN
	SELECT ID FROM CarPhotos WITH(NOLOCK) WHERE TC_CarPhotoId  = @PhotoId
	IF @@ROWCOUNT = 0
		BEGIN
			-- AM Modified 6-11-2012 to insert PhotoId from TC_CarPhotos to CarPhotos
			INSERT INTO CarPhotos(InquiryId,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,IsDealer,IsMain,IsActive,IsApproved,DirectoryPath, HostUrl,IsReplicated,TC_CarPhotoId, OriginalImgPath)
							VALUES(@InquiryId,@ImageUrlFull,@ImageUrlThumb,@ImageUrlThumbSmall,1,@IsMain,1,1,@DirPath, @HostUrl,0,@PhotoId, @OrgImgPath)
		END
END
    
END




