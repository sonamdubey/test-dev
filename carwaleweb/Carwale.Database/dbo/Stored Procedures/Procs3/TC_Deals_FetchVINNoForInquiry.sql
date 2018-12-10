IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_FetchVINNoForInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_FetchVINNoForInquiry]
GO

	


-- =============================================
-- Created By: Upendra Kumar
-- Created Date:6 Jan ,2016
-- Description: To Get the VINNos for NewCarInquiry for Deals SKU where Inquiry is done and VINNo is boked by him/her or free 
-- EXEC TC_Deals_FetchVINNoForInquiry 13888
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_FetchVINNoForInquiry]
	@NCDInquiryId INT
AS
BEGIN	
	--DECLARE @DealsStockVINId INT, @DealsStockId INT


	SELECT TD.TC_DealsStockVINId AS DealsStockVINId, TD.VINNo, CASE WHEN TD.Status != 2 THEN 1 ELSE 0 END AS IsSelect
	FROM TC_NewCarInquiries TI WITH(NOLOCK)  INNER JOIN TC_Deals_StockVIN TD WITH(NOLOCK) ON TI.TC_Deals_StockId = TD.TC_Deals_StockId 
	WHERE TC_NewCarInquiriesId = @NCDInquiryId AND (TD.Status = 2 OR TD.TC_DealsStockVINId = TI.TC_DealsStockVINId)


	--SELECT @DealsStockVINId = TC_DealsStockVINId, @DealsStockId = TC_Deals_StockId
	--FROM TC_NewCarInquiries WITH(NOLOCK)  
	--WHERE TC_NewCarInquiriesId = @NCDInquiryId 



	--SELECT TC_DealsStockVINId AS DealsStockVINId, VINNo ,CASE WHEN TC_DealsStockVINId = @DealsStockVINId THEN 1 ELSE 0 END AS IsSelect
	--FROM TC_Deals_StockVIN  WITH(NOLOCK) 
	--WHERE TC_Deals_StockId = @DealsStockId AND (Status = 2 OR Status = 4 AND TC_DealsStockVINId = @DealsStockVINId)

END
