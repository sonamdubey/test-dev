IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetStaleStockList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetStaleStockList]
GO

	
-- =============================================
-- Author:		Anchal gupta
-- Create date: 06-01-2016
-- Description:	brings the list of unrefreshed stock of a dealer for manage pending refreshes screen of opr
-- EXEC [GetStaleStockList]
-- select * from TC_Deals_StockPrices 
-- =============================================
CREATE PROCEDURE [dbo].[GetStaleStockList]
AS
BEGIN
	SELECT tds.BranchId AS DealerId
		,d.Organization AS DealerName
		,d.IsDealerActive
		,d.IsDealerDeleted
		,ci.ID AS CityId
		,d.ApplicationId
		,ci.Name AS DealerCity
		,STUFF((
				SELECT DISTINCT ', ' + CM.NAME
				FROM TC_deals_dealers TD WITH (NOLOCK)
				INNER JOIN Dealers DL WITH (NOLOCK) ON TD.DealerId = DL.Id
				INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON DL.Id = TDM.DealerId
				INNER JOIN CarMakes CM WITH (NOLOCK) ON TDM.MakeId = CM.Id
				WHERE TD.DealerId = tds.BranchId
				FOR XML PATH('')
					,TYPE
				).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS DealerMake
		,count(DISTINCT tdsv.TC_DealsStockVINId) AS staleVIN
		,min(tdsv.LastRefreshedOn) AS OSRT
		,max(tdsv.LastRefreshedOn) AS LSRT
	FROM TC_Deals_Stock TDS WITH (NOLOCK)
	INNER JOIN Dealers d WITH (NOLOCK) ON tds.BranchId = d.Id
	INNER JOIN TC_Deals_Dealers TDD WITH (NOLOCK) ON tds.BranchId = TDD.DealerId
	INNER JOIN TC_Deals_StockVIN TDSV WITH (NOLOCK) ON TDSV.TC_Deals_StockId = TDS.Id
	INNER JOIN TC_Deals_StockPrices TDSP WITH (NOLOCK) ON TDS.Id = TDSP.TC_Deals_StockId
	INNER JOIN Cities ci WITH (NOLOCK) ON D.CityId = ci.Id
	WHERE tdsv.LastRefreshedOn < Convert(DATE, GETDATE())
		AND TDS.isApproved = 1
		AND d.IsDealerActive = 1
		AND d.IsDealerDeleted = 0
		AND TDD.IsDealerDealActive = 1
		AND D.IsTCDealer = 1
		AND D.TC_DealerTypeId IN (
			2
			,3
			)
		AND TDSV.STATUS = 2
		AND ISNULL(TDSP.DiscountedPrice, 0) <> 0
	GROUP BY tds.BranchId
		,d.Organization
		,d.IsDealerActive
		,d.IsDealerDeleted
		,ci.id
		,ci.Name
		,d.ApplicationId
	ORDER BY OSRT
END

