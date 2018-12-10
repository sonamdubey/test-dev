IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CarPhotos_UpdateStockImageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CarPhotos_UpdateStockImageDetails]
GO

	-- =============================================
-- Author:		Navead kazi
-- Create date: 14/07/2016
-- Description:	Update CarPhotos table data recieved from stockImageApi
-- =============================================
CREATE PROCEDURE [dbo].[CarPhotos_UpdateStockImageDetails] 
	@StockId INT,
	@Description VARCHAR(500),
	@SellerType INT,
	@IsMain BIT,
	@SourceId INT,
	@TC_CarPhotoId INT = null,
	@OriginalImgPath VARCHAR(150),
	@PhotoId INT OUTPUT,
	@Updated BIT OUTPUT
AS
BEGIN
	
	DECLARE @InquiryId INT
	DECLARE @MainCarPhotoId INT
	SET @Updated = 0;
	IF @SellerType = 1 
	BEGIN
		SELECT TOP 1 @InquiryId = Id 
		FROM SellInquiries WITH(NOLOCK) 
		WHERE TC_StockId = @StockId 
		AND SourceId = @SourceId
		AND StatusId = 1
		ORDER BY ID DESC 

		SELECT @MainCarPhotoId = TC_CarPhotoId FROM CarPhotos WITH(NOLOCK) WHERE InquiryId = @InquiryId AND isMain = 1 AND IsActive = 1 AND IsApproved = 1

		-- if the updated row is not main image row
		IF @MainCarPhotoId <> @TC_CarPhotoId
		BEGIN

			IF @IsMain = 1 --if setting main image then set all other image of stock as non main image
				UPDATE CarPhotos
				SET IsMain = 0
				WHERE  TC_CarPhotoId <> @TC_CarPhotoId AND InquiryId=@InquiryId AND IsActive = 1 AND IsApproved = 1;

		END
		UPDATE CarPhotos
		SET IsMain = @IsMain,
		[Description] = @Description,
		OriginalImgPath = @OriginalImgPath
		WHERE TC_CarPhotoId=@TC_CarPhotoId AND InquiryId=@InquiryId;

		SET @Updated= 1;

		SELECT TOP 1 @PhotoId = Id 
		FROM CarPhotos 
		WITH(NOLOCK) WHERE TC_CarPhotoId=@TC_CarPhotoId AND InquiryId=@InquiryId
		ORDER BY ID DESC;
	END
	
END


