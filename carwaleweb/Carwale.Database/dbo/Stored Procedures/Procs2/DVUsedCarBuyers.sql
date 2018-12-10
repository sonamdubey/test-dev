IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DVUsedCarBuyers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DVUsedCarBuyers]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 14-08-2013
-- Description:	Return Used Car Buyers count
-- =============================================
CREATE PROCEDURE [dbo].[DVUsedCarBuyers]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--CREATE TABLE #tempUsedCarBuyers(
	--Day varchar(20),
	--UsedCarBuyersCount bigint	
	--)
	
    
    WITH CTE AS(
	SELECT CONVERT(varchar(8),RequestDateTime,112)  Day,
	CustomerID
	FROM UsedCarPurchaseInquiries with(nolock)
	WHERE CONVERT(varchar(8),RequestDateTime,112) = CONVERT(varchar(8),GETDATE()-1,112)
	UNION ALL
	SELECT CONVERT(varchar(8),RequestDateTime,112)  Day,
	CustomerID
	FROM ClassifiedRequests with(nolock)
	WHERE CONVERT(varchar(8),RequestDateTime,112) = CONVERT(varchar(8),GETDATE()-1,112)
	)
	--INSERT INTO #tempUsedCarBuyers(Day,UsedCarBuyersCount)
	SELECT Day,COUNT(DISTINCT CustomerID) UsedCarBuyersCount FROM CTE
	GROUP BY Day
	ORDER BY Day
	
	--SELECT Day,UsedCarBuyersCount
	--FROM #tempUsedCarBuyers
	
	
	--DROP table #tempUsedCarBuyers
END
