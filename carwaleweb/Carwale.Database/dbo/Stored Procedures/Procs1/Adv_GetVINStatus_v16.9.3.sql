IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Adv_GetVINStatus_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Adv_GetVINStatus_v16]
GO

	-- =============================================
-- Author:		Saket Thapliyal
-- Create date: 10th Aug 2016
-- Description:	Get VINId, Status and VINNo for the given DealStockId 
-- Modified by Purohith Guguloth on 15th september, 2016
-- Added an Input parameter @DealInquiryId and added another else condition.
-- =============================================
create PROCEDURE [dbo].[Adv_GetVINStatus_v16.9.3]
	@DealsStockId VARCHAR(250) = NULL,
	@DealsVINId   VARCHAR(250) = NULL,
	@DealInquiryId INT = 0  	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF @DealsVINId Is NOT Null
	BEGIN
		SELECT TC_DealsStockVINId, Status, VINNo FROM TC_Deals_StockVIN WITH(NOLOCK)
		WHERE TC_DealsStockVINId in (SELECT Listmember from fnSplitCSVValuesWithIdentity(@DealsVINId))
		ORDER BY TC_DealsStockVINId
	END
    ELSE IF @DealsStockId Is NOT NULL AND @DealInquiryId = 0
	BEGIN
		SELECT TC_DealsStockVINId, Status, VINNo FROM TC_Deals_StockVIN WITH(NOLOCK)
		WHERE TC_Deals_StockId in (SELECT Listmember from fnSplitCSVValuesWithIdentity(@DealsStockId))
		ORDER BY TC_DealsStockVINId
	END
	ELSE            -- Modified by Purohith Guguloth on 15th september, 2016
	BEGIN
		SELECT TC_DealsStockVINId, Status, VINNo FROM TC_Deals_StockVIN WITH(NOLOCK)
		WHERE TC_DealsStockVINId in (select TC_DealsStockVinId from TC_NewCarInquiries WITH(NOLOCK) where TC_NewCarInquiriesId = @DealInquiryId)
		ORDER BY TC_DealsStockVINId
	END
END

