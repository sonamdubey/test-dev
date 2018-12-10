IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustStockStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustStockStatus]
GO

	-- =============================================
-- Author:		Garule Prabhudas
-- Create date: 26th oct,2016
-- Description:	To get the status of custStock Data i.e (isLiveOnCT,IsSharedToCT,IsUserPermitStockToShare)
-- =============================================
CREATE PROCEDURE [dbo].[GetCustStockStatus]
@CustSellInquiryId INT,
@IsCustStockShared BIT OUTPUT,
@IsUserPermitStockToShare BIT OUTPUT,
@IsLiveOnCT BIT OUTPUT

AS
BEGIN
	-- See if stock already shared with CT
	SET NOCOUNT ON;
	SET @IsCustStockShared = 0;
	-- See if stock is Live on CT
	SET @IsLiveOnCT = 0;
	IF EXISTS(SELECT 1 FROM CustStockShared WITH(NOLOCK) WHERE InquiryId = @CustSellInquiryId)
	BEGIN
		SET @IsCustStockShared = 1;
		IF EXISTS (SELECT 1 FROM CustStockShared WITH(NOLOCK) WHERE InquiryId=@CustSellInquiryId AND IsLive=1)
		BEGIN
			SET @IsLiveOnCT = 1;
		END
	END

	-- See if Customer permits to share stock with CT
	SET @IsUserPermitStockToShare = 0;
	IF EXISTS(SELECT 1 FROM CustomerSellInquiries WITH(NOLOCK) WHERE Id = @CustSellInquiryId AND ShareToCT = 1)
	BEGIN
		SET @IsUserPermitStockToShare = 1;
	END;
END

