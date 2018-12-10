IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[adv_LogPriceUpdatedFlag]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[adv_LogPriceUpdatedFlag]
GO

	-- =============================================
-- Author:		MUKUL BANSAL
-- Create date: 22nd August, 2016
-- Description:	Log Is Price updated flag value
-- =============================================
CREATE PROCEDURE [dbo].[adv_LogPriceUpdatedFlag]
	@StockId INT,
	@IsPriceUpdated BIT,
	@ModifiedBy INT
AS
BEGIN
	INSERT INTO TC_Deals_StockPriceFlagLog VALUES(@StockId, GETDATE(), @ModifiedBy, @IsPriceUpdated)	
END

