IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CarPhotos_InsertStockImageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CarPhotos_InsertStockImageDetails]
GO

	-- =============================================
-- Author:		Navead Kazi
-- Create date: 13th July 2016
-- Description:	Insert into CarPhotos table data recieved from stockImageApi
-- exec CarPhotos_InsertStockImageDetails 1,1,1,'a',1,null,0,1,'sdf'
-- Modified By Prachi Phalak. Added check TC_CarPhotoId to avoid duplicate entries in CarPhotos on 12/10/2016
-- Modified By Sahil Sharma. Added InquiryId check in already exist query to enable photo insert from both autobiz and cartrade with same photo id.
-- =============================================
CREATE PROCEDURE [dbo].[CarPhotos_InsertStockImageDetails] 
	@SourceId INT,
	@SellerType INT,
	@StockId INT,
	@Description VARCHAR(500),
	@IsMain BIT,
	--@Title VARCHAR(100),
	@HostURL VARCHAR(50) = null,
	@IsReplicated		BIT=0,
	@TC_CarPhotoId INT = null,
	@OriginalImgPath VARCHAR(150),
	@PhotoId INT OUTPUT

AS
BEGIN
  
	DECLARE @InquiryId INT
	DECLARE @ProfileId VARCHAR(50)
	DECLARE @DirectoryPath VARCHAR(100) = null
	DECLARE @Status SMALLINT = 1
	DECLARE @IsApproved BIT = 0
	DECLARE @InquiryPresent BIT = 0
	DECLARE @IsDealer BIT = 0
	
	IF @SellerType = 1
	BEGIN
	
		SET @IsApproved = 1
		SET @IsDealer = 1

		-- Fetch inquiryId from StockId
		SELECT TOP 1 @InquiryId= id
		FROM SellInquiries WITH(NOLOCK) 
		WHERE TC_StockId = @StockId 
		and SourceId = @SourceId
		and StatusId = 1
		ORDER BY Id desc;
	END
	
	IF @InquiryId IS NOT NULL 
	BEGIN
	 IF NOT EXISTS(SELECT TOP 1 TC_CarPhotoId from CarPhotos WITH(NOLOCK) WHERE TC_CarPhotoId=@TC_CarPhotoId AND IsActive = 1 AND InquiryId = @InquiryId)
	   BEGIN
	

		INSERT INTO CarPhotos(InquiryId, [Description], IsDealer, IsMain, DirectoryPath, HostURL,IsReplicated,StatusId,TC_CarPhotoId,IsApproved,OriginalImgPath)
		VALUES(@InquiryId, @Description, @IsDealer, @IsMain, @DirectoryPath, @HostUrl,@IsReplicated,@Status,@TC_CarPhotoId,@IsApproved,@OriginalImgPath)

		SET @PhotoId = SCOPE_IDENTITY()

		--IF @TC_CarPhotoId IS NULL -- Set the TC_CarPhotoId same as Id for individual so that it can be used during delete operation
			--UPDATE CarPhotos SET TC_CarPhotoId = @PhotoId WHERE Id = @PhotoId

	   END
	END

END

