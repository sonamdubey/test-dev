IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CarPhotos_DeleteStockImageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CarPhotos_DeleteStockImageDetails]
GO

	-- =============================================
-- Author:		Navead Kazi
-- Create date: 14 July 2016
-- Description:	Remove Photos from carwale
-- =============================================
CREATE PROCEDURE [dbo].[CarPhotos_DeleteStockImageDetails] 
	@StockId INT,
	@SellerType INT,
	@SourceId INT,
	@TC_CarPhotoId INT,
	@Deleted BIT OUTPUT
AS
BEGIN
	
	DECLARE @InquiryId INT = null

	SET @Deleted = 0

	IF @SellerType = 1
	BEGIN
		--SELECT @InquiryId = Id FROM SellInquiries WITH(NOLOCK) WHERE TC_StockId = @StockId AND SourceId = @SourceId 
		SELECT @InquiryId = s.Id 
		FROM SellInquiries s WITH(NOLOCK) 
		inner join carphotos c WITH(NOLOCK) 
		on c.inquiryid = s.id 
		WHERE s.TC_StockId = @StockId 
		AND s.SourceId = @SourceId 
		AND c.tc_carphotoid = @TC_CarPhotoId

		IF @InquiryId IS NOT NULL
		BEGIN
			UPDATE CarPhotos
			SET	IsActive = 0
			WHERE TC_CarPhotoId=@TC_CarPhotoId AND InquiryId=@InquiryId AND IsDealer = 1

			SET @Deleted = 1
		END
	END
END


