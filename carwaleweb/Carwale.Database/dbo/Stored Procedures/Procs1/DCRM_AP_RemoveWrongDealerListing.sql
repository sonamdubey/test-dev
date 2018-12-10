IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AP_RemoveWrongDealerListing]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AP_RemoveWrongDealerListing]
GO

	

CREATE PROCEDURE [dbo].[DCRM_AP_RemoveWrongDealerListing]
	
	AS

	BEGIN
		
		DECLARE @TempDealersData Table( ListingId NUMERIC)
		
		INSERT INTO @TempDealersData
		SELECT TD.Id AS TCStockId
		FROM TC_Stock TD WITH (NOLOCK) 
			INNER JOIN SellInquiries SI WITH (NOLOCK)  ON SI.TC_StockId = TD.Id
			INNER JOIN Livelistings LL WITH (NOLOCK)  ON LL.Inquiryid = SI.ID AND LL.SellerType = 1
			INNER JOIN Dealers AS D WITH (NOLOCK) ON D.Id = SI.DealerId
			LEFT JOIN NewCarShowroomPrices NPC WITH (NOLOCK) ON NPC.CarVersionId = TD.VersionId AND NPC.CityId = D.CityId
		WHERE D.CityId IN(1,6,8,13,40, 1029, 278) AND SI.Price < (((POWER(0.9, DATEDIFF(YY,TD.MakeYear, GETDATE())) * ISNULL(NPC.Price,0)/(POWER(1.07, DATEDIFF(YY,TD.MakeYear, GETDATE()))))*.5)-50000)


		--SELECT TD.Id AS TCStockId FROM TC_Stock TD WITH (NOLOCK) 
		--INNER JOIN SellInquiries SI WITH (NOLOCK)  ON SI.TC_StockId = TD.Id
		--INNER JOIN Livelistings LL WITH (NOLOCK)  ON LL.Inquiryid = SI.ID AND LL.SellerType = 1
		--INNER JOIN DEalers AS D WITH (NOLOCK) ON D.Id = SI.DealerId
		--WHERE TD.Price IN('11111', '21111', '11110', '11112', '12100', '12111', '50111', '111111', '111999', '51111', '51000')
		
		INSERT INTO DCRM_AP_RemovedDealerStocks
		SELECT ListingId, GETDATE() FROM  @TempDealersData
		
		-- Remove Data From LiveListing 
		UPDATE SellInquiries SET StatusId = 2 WHERE TC_StockId IN(SELECT ListingId FROM  @TempDealersData)
		
		-- Remove sync to CarWale in Autobiz
		UPDATE TC_Stock SET IsSychronizedCW = 0 WHERE Id IN(SELECT ListingId FROM  @TempDealersData)
		
	END



