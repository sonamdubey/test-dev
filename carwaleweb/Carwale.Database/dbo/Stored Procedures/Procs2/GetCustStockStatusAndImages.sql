IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustStockStatusAndImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustStockStatusAndImages]
GO

	-- =============================================
-- Author:		Garule Prabhudas
-- Create date: 26th oct,2016 
-- Description:	See if the stock is shared with cartrade and fetch all the images of stock corresponding to InquiryId
-- =============================================
CREATE PROCEDURE [dbo].[GetCustStockStatusAndImages]
	@CustSellInquiryId INT,
	@IsCustStockShared BIT OUTPUT,
	@IsLiveOnCW BIT OUTPUT,
	@IsLiveOnCT BIT OUTPUT,
	@CarTradeStockId INT OUTPUT
	AS
BEGIN
	
	SET NOCOUNT ON;
	SET @IsLiveOnCW = 0
	SET @IsLiveOnCT = 0
	SET @IsCustStockShared = 0
	SET @CarTradeStockId=-1

	-- See if InquiryId is live on CW
	IF EXISTS(SELECT 1 FROM LiveListings WITH(NOLOCK) WHERE Inquiryid = @CustSellInquiryId AND SellerType=2)
	 BEGIN
		SET @IsLiveOnCW=1
	 END

	 -- see if inquiry id is shared with CT
	IF @IsLiveOnCW=1
	  BEGIN
		DECLARE @TempStockId INT
		SET @TempStockId = (SELECT TOP 1 CarTradeStockId FROM CustStockShared WITH(NOLOCK) WHERE InquiryId = @CustSellInquiryId);
		IF @TempStockId IS NOT NULL
			BEGIN
				SET @CarTradeStockId = @TempStockId;
			END 
		IF EXISTS (SELECT 1  FROM CustStockShared WITH(NOLOCK) WHERE InquiryId=@CustSellInquiryId AND IsLive=1)
		BEGIN
			SET @IsLiveOnCT = 1
		END
	  END

	  -- if stock is shared with CT then get All images of the corresponding inquiryId
	IF @CarTradeStockId != -1 AND @IsLiveOnCT = 1
	BEGIN
		SET @IsCustStockShared=1

		SELECT Id AS ImageId,IsMain,'http://ipc.carwale.com:8083' + OriginalImgPath AS ImageUrl
		FROM CarPhotos WITH(NOLOCK) WHERE InquiryId = @CustSellInquiryId AND IsActive=1 AND IsApproved=1 AND IsDealer=0
	END
END

