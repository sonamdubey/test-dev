IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ApplyOffersOnStocks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ApplyOffersOnStocks]
GO

	
-- =============================================
-- Author:		Vinayak Patil
-- Create date: 15/01/2014
-- Description:	For Mapping offers with the Stock
-- Modified By Vivek Gupta on 18-07-2014, Added with nolock in select queries
-- =============================================


CREATE PROCEDURE [dbo].[TC_ApplyOffersOnStocks]
@Branchid INT,
@StockIds VARCHAR(100),
@OfferIds VARCHAR(100),
@IsRemove BIT = 0

AS 
	BEGIN

	
	CREATE TABLE #tempStocks(StockIds INT)

	CREATE TABLE #tempOffers(OfferIds INT) 

	CREATE TABLE #temp(ID INT identity,StockId INT,OfferId INT)

	IF(@IsRemove = 0)
		BEGIN
		DECLARE @Count TINYINT,@i TINYINT = 1

			INSERT INTO #tempStocks
			SELECT * FROM [dbo].[fnSplitCSV] (@StockIds)

			INSERT INTO #tempOffers
			SELECT * FROM [dbo].[fnSplitCSV] (@OfferIds)

			INSERT INTO #temp (StockId,OfferId)
			SELECT S.StockIds,O.OfferIds
			FROM #tempOffers O 
			CROSS JOIN #tempStocks S
			ORDER BY StockIds

			SET @Count = (SELECT COUNT(*) FROM #temp)

	
			WHILE (@i <= @Count)
			BEGIN
				IF NOT EXISTS(SELECT * FROM TC_MappingOfferWithStock WITH(NOLOCK)
							  WHERE StockId=(SELECT StockId FROM #temp WHERE ID = @i)
							  AND TC_UsedCarOfferId = (SELECT OfferId FROM #temp WHERE ID = @i))

					BEGIN
						INSERT INTO TC_MappingOfferWithStock(StockId,
															 TC_UsedCarOfferId,
															 StartDate,
															 EndDate)
	
						SELECT T.StockId,T.OfferId,UCO.StartDate,UCO.EndDate
						FROM #temp T 
						LEFT JOIN TC_UsedCarOffers UCO ON T.OfferId = UCO.TC_UsedCarOfferId 
						WHERE T.ID = @i
						ORDER BY StockId
				 
					END 
		
				SET @i = @i + 1
	
			END
	
			DROP TABLE #tempStocks

			DROP TABLE #tempOffers

			DROP TABLE #temp
		END
		ELSE IF(@IsRemove = 1)
		BEGIN

			INSERT INTO #tempStocks
			SELECT * FROM [dbo].[fnSplitCSV] (@StockIds)

			INSERT INTO #tempOffers
			SELECT * FROM [dbo].[fnSplitCSV] (@OfferIds)

			DELETE
			FROM TC_MappingOfferWithStock 
			WHERE TC_UsedCarOfferId IN (SELECT OfferIds FROM #tempOffers)
			AND StockId IN (SELECT StockIds FROM #tempStocks)

			DROP TABLE #tempStocks

			DROP TABLE #tempOffers

			DROP TABLE #temp
		END
END





