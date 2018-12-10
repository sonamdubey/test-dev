IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetCityBasedStocks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetCityBasedStocks]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 13th April 2016
-- Description:	To fetch the cities of a particular dealer for which he has added deals stock
-- EXEC TC_Deals_GetCityBasedStocks 5,40
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_GetCityBasedStocks] 
	@DealerId INT,
	@CityId INT
AS
BEGIN
	SELECT	DISTINCT DS.Id DealStockId ,MMV.Car CarName , 
			VC.Color VersionColor, RIGHT(CONVERT(VARCHAR(11),DS.MakeYear, 106),8) MakeYear, DS.CarVersionId AS VersionId
	FROM	TC_Deals_Stock DS WITH (NOLOCK)
			INNER JOIN TC_Deals_StockPrices DSP WITH (NOLOCK) ON DSP.TC_Deals_StockId = DS.Id
			INNER JOIN TC_Deals_StockVIN DSV WITH (NOLOCK) ON DSV.TC_Deals_StockId = DS.Id
			INNER JOIN VersionColors VC WITH (NOLOCK) ON VC.ID = DS.VersionColorId
			INNER JOIN vwAllMMV MMV ON MMV.VersionId = DS.CarVersionId 
	WHERE	DS.BranchId = @DealerId 
			AND DSP.CityId = @CityId 
			AND DSV.Status = 2 -- Active Vin
			AND MMV.ApplicationId = 1
	ORDER BY MMV.Car
END
