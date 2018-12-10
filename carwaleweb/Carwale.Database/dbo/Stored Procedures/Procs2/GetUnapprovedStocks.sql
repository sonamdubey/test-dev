IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUnapprovedStocks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUnapprovedStocks]
GO

	-- =============================================
-- Author:		Sanjay Soni
-- Create date: 07/12/2016
-- Description: unapproved stocks listings
-- exec [GetUnapprovedStocks]
-- =============================================
CREATE PROCEDURE [dbo].[GetUnapprovedStocks]
AS
BEGIN
	SELECT D.Id AS DealerId
		,D.Organization AS 'Dealer Name'
		,t.stockgroup AS 'Number of Unapproved Stock Groups'
		,t.vingroup AS 'Number of Associated VINs'
		,C.NAME AS 'Dealer City(s)'
		,t.dealerMake AS 'Dealer Make(s)'
		,t.oldRefreshTime AS 'Oldest Stock Upload Time'
		,t.latestRefreshTime AS 'Latest Stock Upload Time'
		,D.CityId AS 'City Id'
	FROM TC_deals_dealers AS TD WITH (NOLOCK)
	INNER JOIN Dealers AS D WITH (NOLOCK) ON TD.DealerId = D.Id
	INNER JOIN (
		SELECT COUNT(DISTINCT TDS.Id) AS 'stockgroup'
			,COUNT(DISTINCT V.TC_DealsStockVINId) AS 'vingroup'
			,TDS.BranchId AS 'branchid'
			,STUFF((
					SELECT DISTINCT ', ' + CAST(CM.NAME AS VARCHAR)
					FROM TC_deals_dealers AS TD WITH (NOLOCK)
					INNER JOIN Dealers D WITH (NOLOCK) ON TD.DealerId = D.Id
					INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON D.Id = TDM.DealerId
					INNER JOIN CarMakes CM WITH (NOLOCK) ON TDM.MakeId = CM.Id
					WHERE TD.DealerId = TDS.BranchId
					FOR XML PATH('')
						,TYPE
					).value('.', 'NVARCHAR(MAX)'), 1, 1, ' ') AS 'dealerMake'
			,MIN(TDS.LastUpdatedOn) AS 'oldRefreshTime'
			,MAX(TDS.LastUpdatedOn) AS 'latestRefreshTime'
		FROM TC_Deals_Stock AS TDS WITH (NOLOCK)
		INNER JOIN TC_Deals_StockVIN AS V WITH (NOLOCK) ON TDS.Id = V.TC_Deals_StockId
		WHERE V.STATUS = 1
			AND TDS.isApproved = 0
		GROUP BY TDS.BranchId
		) AS t ON D.id = t.branchid
	INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = D.CityId
	WHERE  D.IsTCDealer = 1
		AND D.IsDealerActive = 1
		AND D.IsDealerDeleted = 0
		AND TD.IsDealerDealActive = 1
	AND  D.TC_DealerTypeId IN (
			2
			,3
			)
		
	ORDER BY t.oldRefreshTime
END