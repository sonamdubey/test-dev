IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LogApiResponseAndUpdateStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LogApiResponseAndUpdateStock]
GO

	-- =============================================
-- Author:		Garule Prabhudas
-- Create date: 26th oct,2016
-- Description:	Log Api response and update stock if response is success
-- =============================================
CREATE PROCEDURE [dbo].[LogApiResponseAndUpdateStock]
 @CustSellInquiryId INT,
 @CarTradeStockId INT,
 @ApiStatus BIT,
 @IsLive BIT,
 @ActionType TINYINT,
 @StatusCode INT,
 @Errors VARCHAR(100),
 @IsStock BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Log API response (Update)
	INSERT INTO CustApiLog (InquiryId, ActionType,LastUpdateDateTime,IsSuccessful, StatusCode, Errors,IsStockOrImage)
	VALUES (@CustSellInquiryId,@ActionType,getdate(),@ApiStatus, @StatusCode, @Errors,@IsStock)

	-- If Api Response is Success then Update the stockShared table --
		IF (@ApiStatus=1 AND @IsStock=1)
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM CustStockShared WITH(NOLOCK) WHERE InquiryId = @CustSellInquiryId)
					BEGIN
						INSERT INTO CustStockShared (InquiryId,CarTradeStockId,IsLive,LastUpdated) VALUES 
						(
						@CustSellInquiryId,
						@CarTradeStockId,
						@IsLive,
						getdate()
						) 
					END
				ELSE
					BEGIN
						UPDATE CustStockShared				-- If Already shared then Update else create new record
						SET IsLive = @IsLive, LastUpdated = getdate()
						WHERE InquiryId = @CustSellInquiryId;
					END
			END
END

