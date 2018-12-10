IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_UpdateStockScore]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_UpdateStockScore]
GO

	-- =============================================
-- Author:		MUKUL BANSAL
-- Create date: 31-08-2016
-- Description:	Update stock relevance score for a stock id.
-- Modified : Added offervalue in calculating stockscore. 6th October, 2016.
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_UpdateStockScore]
	-- Add the parameters for the stored procedure here
AS
BEGIN

CREATE TABLE #StockScore(
 StockId INT,
 CityId INT,
 StockScore DECIMAL(8,5))

DECLARE @savingsWeight DECIMAL(4,2), @popularityWeight DECIMAL(4,2), @maxPopularity INT
SELECT @savingsWeight = Weightage FROM TC_Deals_ScoreCategoryWeights WITH(NOLOCK) WHERE ID = 1
SELECT @popularityWeight = Weightage FROM TC_Deals_ScoreCategoryWeights WITH(NOLOCK) WHERE ID = 2
SELECT @maxPopularity = MAX(ModelPopularity) FROM CarModels WITH(NOLOCK)
INSERT INTO #StockScore(StockId, CityId, StockScore)
SELECT vw.StockId, vw.CityId, @savingsWeight*4*((CONVERT(DECIMAL(16,5), vw.Savings) + 0.8 * CONVERT(DECIMAL(16,5), CASE WHEN vw.Offer_Value IS NULL THEN 0 ELSE vw.Offer_Value END ))/vw.ActualOnroadPrice) + @popularityWeight*CONVERT(DECIMAL(16,5),CM.ModelPopularity)/@maxPopularity AS NormalizedPopularity FROM vwLiveDeals vw WITH(NOLOCK) INNER JOIN CarModels CM WITH(NOLOCK)
ON vw.ModelId = CM.ID 

UPDATE SP 
SET SP.StockScore = SS.StockScore
FROM #StockScore SS INNER JOIN
TC_Deals_StockPrices SP WITH(NOLOCK)
ON SS.StockId = SP.TC_Deals_StockId AND SS.CityId = SP.CityId

--Update LiveDeals Table
UPDATE LD
SET LD.StockScore = SS.StockScore
FROM #StockScore SS INNER JOIN
LiveDeals LD WITH(NOLOCK)
ON SS.StockId = LD.StockId AND SS.CityId = LD.CityId

DROP TABLE #StockScore

END

