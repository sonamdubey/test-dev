IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetDealerCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetDealerCities]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 13th April 2016
-- Description:	To fetch the cities of a particular dealer for which he has added deals stock
-- EXEC TC_Deals_GetDealerCities 5
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_GetDealerCities]
	@DealerId INT
AS
BEGIN
	SELECT	DISTINCT C.ID AS CityId , C.Name CityName 
	FROM	TC_Deals_StockPrices DSP WITH (NOLOCK)
			INNER JOIN Cities C WITH (NOLOCK) ON C.ID = DSP.CityId
			INNER JOIN TC_Deals_Stock DS WITH (NOLOCK) ON DS.Id = DSP.TC_Deals_StockId
	WHERE	DS.BranchId = @DealerId
	ORDER BY C.Name ASC
END
------------------------------------------------------------------------------------------------------------------
