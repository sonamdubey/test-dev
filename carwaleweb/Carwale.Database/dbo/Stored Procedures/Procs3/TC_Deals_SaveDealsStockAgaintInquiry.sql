IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_SaveDealsStockAgaintInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_SaveDealsStockAgaintInquiry]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: April 19,2016
-- Description:	To save DealsStockId against Masking no inquiry
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_SaveDealsStockAgaintInquiry]
	@DealsStockId INT,
	@NewCarInquiryId INT,
	@VersionId INT,
	@CityId INT
AS
BEGIN
	UPDATE  TC_NewCarInquiries 
	SET TC_Deals_StockId = @DealsStockId
	,VersionId = @VersionId
	,CityId = @CityId
	WHERE TC_NewCarInquiriesId = @NewCarInquiryId
END
