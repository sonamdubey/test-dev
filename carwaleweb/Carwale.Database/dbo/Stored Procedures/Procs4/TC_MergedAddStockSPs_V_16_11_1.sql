IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MergedAddStockSPs_V_16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MergedAddStockSPs_V_16_11_1]
GO

	
-- =============================================
-- Author:		<Vivek,,Gupta>
-- Create date: <Create Date,24-07-2015,>
-- Description:	<Description,Merged some imp stored procedure to load in one go while loading add stock page,>
-- =============================================
CREATE PROCEDURE [dbo].[TC_MergedAddStockSPs_V_16_11_1] @BranchId BIGINT
	,@StockId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	EXEC TC_GetCustomerBenefits @StockId

	-- EXEC TC_GetCertifiedOrg @DealerId = @BranchId
	EXEC TC_FetchDealerBranchLocations @BranchId = @BranchId

	EXEC TC_GetRegistrationCode
END
