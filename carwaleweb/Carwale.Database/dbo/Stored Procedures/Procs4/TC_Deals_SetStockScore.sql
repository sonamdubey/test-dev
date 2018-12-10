IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_SetStockScore]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_SetStockScore]
GO

	CREATE PROCEDURE [dbo].[TC_Deals_SetStockScore]
	-- Add the parameters for the stored procedure here
	@stockId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @savingsWeight DECIMAL(4,2), @popularityWeight DECIMAL(4,2), @popularityScore DECIMAL(8,5), @maxPopularity INT
	SELECT @savingsWeight = Weightage FROM TC_Deals_ScoreCategoryWeights WITH(NOLOCK) WHERE ID = 1
	SELECT @popularityWeight = Weightage FROM TC_Deals_ScoreCategoryWeights WITH(NOLOCK) WHERE ID = 2
	SELECT @maxPopularity = Max(ModelPopularity) FROM CarModels WITH(NOLOCK)
	SELECT @popularityScore = @popularityWeight*(CONVERT(DECIMAL(16,5), ModelPopularity)/@maxPopularity) FROM  CarModels CM WITH(NOLOCK) WHERE 
	CM.ID = (select TOP 1 CV.CarModelId from TC_Deals_Stock TS WITH(NOLOCK) INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = TS.CarVersionId  WHERE TS.Id = @stockId)

	UPDATE TC_Deals_StockPrices SET StockScore = @savingsWeight*((ActualOnRoadPrice - DiscountedPrice + 0.8*CASE WHEN Offer_Value IS NULL THEN 0 ELSE Offer_Value END)/(ActualOnRoadPrice*0.25)) + @popularityScore WHERE TC_Deals_StockId = @stockId
END