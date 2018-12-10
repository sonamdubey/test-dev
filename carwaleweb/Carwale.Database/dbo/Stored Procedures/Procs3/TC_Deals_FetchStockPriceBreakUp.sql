IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_FetchStockPriceBreakUp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_FetchStockPriceBreakUp]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 11th May 2016
-- Description:	To fetch price break up against stockid
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_FetchStockPriceBreakUp] 
	@StockId INT,
	@CityId INT
AS
BEGIN
	SELECT StockId,TC_PQComponentId,ComponentPrice,DSP.ActualOnroadPrice OnroadPrice
	FROM TC_Deals_StockPricesBreakup SPB WITH (NOLOCK) 
	INNER JOIN TC_PQComponents PQC WITH (NOLOCK) ON PQC.TC_PQComponentsId = SPB.TC_PQComponentId
	INNER JOIN TC_Deals_StockPrices DSP WITH (NOLOCK) ON DSP.TC_Deals_StockId = SPB.StockId AND DSP.CityId=SPB.CityId -- Added join to fetch ActualOnroadPrice
	WHERE SPB.StockId = @StockId AND SPB.CityId = @CityId AND PQC.IsActive=1
END

-------------------------------------------------------------------------------------------------------------------------


