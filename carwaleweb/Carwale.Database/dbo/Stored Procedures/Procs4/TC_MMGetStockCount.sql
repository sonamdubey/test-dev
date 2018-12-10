IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMGetStockCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMGetStockCount]
GO

	
-- =============================================
-- Name :       TC_MMDealersMatchCount         
-- Author:		Ranjeet Kumar
-- Create date: 05-11-2013
-- Description:	Get the stock count data  for Mix N Match stovk Id
-- Table : TC_MMDealersMatchCount /Function: fnsplitcsv
-- Modified by: Manish on 24-01-2014 adding WITH (NOLOCK)  keyword in all the tables.
-- Modified by: Upendra on 16-11-2015 commented old one and written new query to get MixAndMatch count if not there count is 0.

-- =============================================
CREATE PROCEDURE [dbo].[TC_MMGetStockCount] 
	@DealerId int, 
	@StockIdList nvarchar(500)
	AS
BEGIN
	SET NOCOUNT ON;
	--SELECT  t.MatchViewCount, t.StockId  
	--FROM TC_MMDealersMatchCount  AS t WITH (NOLOCK) 
	--join fnsplitcsv (@StockIdList) AS f  ON f.ListMember = t.StockId 
	--WHERE t.DealerId = @DealerId AND DATEDIFF(mi,t.LastUpdatedOn, GETDATE()) <= 5


	SELECT (CASE WHEN t.MatchViewCount IS NULL THEN 0 ELSE t.MatchViewCount END) AS MatchViewCount,f.ListMember AS StockId
	FROM fnsplitcsv (@StockIdList) AS f 
	LEFT JOIN TC_MMDealersMatchCount  AS t WITH(NOLOCK)  ON f.ListMember = t.StockId 
		 AND  (t.DealerId = @DealerId AND DATEDIFF(HH,t.LastUpdatedOn, GETDATE()) <= 5 )
	--WHERE (t.LastUpdatedOn IS NULL) OR (t.DealerId = @DealerId AND DATEDIFF(DD,t.LastUpdatedOn, GETDATE()) <= 5 )
END


