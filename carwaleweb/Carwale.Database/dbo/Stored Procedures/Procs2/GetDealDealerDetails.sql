IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealDealerDetails]
GO

	
-- =============================================
-- Author:		Anchal Gupta
-- Create date: 08-01-2015
-- Description:	Getting information of dealer to add on manage dealers tab on opr
-- EXEC [GetDealDealerDetails]
-- =============================================
CREATE PROCEDURE [dbo].[GetDealDealerDetails]
AS
BEGIN
	SELECT dealerId
		,DealerName
		,ContactEmail
		,ContactMobile
		,IsDealerActive
		,IsDealerDeleted
		,CityId
		,ApplicationId
		,sum(activeStock) as activeStock
		,sum(activeVIN) as activeVIN
		,DealerCity
		,DealerMake
	FROM (
		SELECT TDD.dealerId AS dealerId
			,d.Organization AS DealerName
			,TDD.ContactEmail
			,TDD.ContactMobile
			,d.IsDealerActive
			,d.IsDealerDeleted
			,ci.ID AS CityId
			,d.ApplicationId
			,count(DISTINCT TSV.TC_Deals_StockId) AS activeStock
			,count(DISTINCT TSV.TC_DealsStockVINId) AS activeVIN
			,ci.Name AS DealerCity
			,STUFF((
					SELECT DISTINCT ', ' + CM.NAME
					FROM TC_deals_dealers TD WITH (NOLOCK)
					INNER JOIN Dealers DL WITH (NOLOCK) ON TD.DealerId = DL.Id
					INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON DL.Id = TDM.DealerId
					INNER JOIN CarMakes CM WITH (NOLOCK) ON TDM.MakeId = CM.Id
					WHERE TD.DealerId = TDD.DealerId
					FOR XML PATH('')
						,TYPE
					).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS DealerMake
		FROM TC_deals_dealers TDD WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON TDD.DealerId = D.Id
		LEFT JOIN TC_Deals_Stock TDS WITH (NOLOCK) ON TDS.BranchId = TDD.DealerId
		LEFT JOIN TC_Deals_StockVIN TSV WITH (NOLOCK) ON TSV.TC_Deals_StockId = TDS.Id
		LEFT JOIN TC_Deals_StockStatus TSS WITH (NOLOCK) ON TSS.Id = TSV.STATUS
		INNER JOIN Cities ci WITH (NOLOCK) ON D.CityId = ci.Id
		WHERE TDD.IsDealerDealActive = 1
			AND (
				TSS.IsActive = 1
				OR ISNULL(TSS.IsActive, 0) = 0
				)
			AND (
				TSV.STATUS = 2
				OR ISNULL(TSV.STATUS, 0) = 0
				)
			AND d.IsDealerActive = 1
			AND d.IsDealerDeleted = 0
			AND D.IsTCDealer = 1
			AND D.TC_DealerTypeId IN (
				2
				,3
				)
		GROUP BY TDD.DealerId
			,d.Organization
			,TDD.ContactEmail
			,TDD.ContactMobile
			,d.IsDealerActive
			,d.IsDealerDeleted
			,ci.id
			,ci.Name
			,d.ApplicationId
		
		UNION ALL
		
		SELECT TDD.dealerId
			,d.Organization AS DealerName
			,TDD.ContactEmail
			,TDD.ContactMobile
			,d.IsDealerActive
			,d.IsDealerDeleted
			,ci.ID AS CityId
			,d.ApplicationId
			,0 AS activeStock
			,0 AS activeVIN
			,ci.Name AS DealerCity
			,STUFF((
					SELECT DISTINCT ', ' + CM.NAME
					FROM TC_deals_dealers TD WITH (NOLOCK)
					INNER JOIN Dealers DL WITH (NOLOCK) ON TD.DealerId = DL.Id
					INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON DL.Id = TDM.DealerId
					INNER JOIN CarMakes CM WITH (NOLOCK) ON TDM.MakeId = CM.Id
					WHERE TD.DealerId = TDD.DealerId
					FOR XML PATH('')
						,TYPE
					).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS DealerMake
		FROM TC_deals_dealers TDD WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON TDD.DealerId = D.Id
		LEFT JOIN TC_Deals_Stock TDS WITH (NOLOCK) ON TDS.BranchId = TDD.DealerId
		LEFT JOIN TC_Deals_StockVIN TSV WITH (NOLOCK) ON TSV.TC_Deals_StockId = TDS.Id
		LEFT JOIN TC_Deals_StockStatus TSS WITH (NOLOCK) ON TSS.Id = TSV.STATUS
		INNER JOIN Cities ci WITH (NOLOCK) ON D.CityId = ci.Id
		WHERE TDD.IsDealerDealActive = 1
			AND TSV.STATUS <> 2
			AND d.IsDealerActive = 1
			AND d.IsDealerDeleted = 0
			AND D.IsTCDealer = 1
			AND D.TC_DealerTypeId IN (
				2
				,3
				)
		GROUP BY TDD.DealerId
			,d.Organization
			,TDD.ContactEmail
			,TDD.ContactMobile
			,d.IsDealerActive
			,d.IsDealerDeleted
			,ci.id
			,ci.Name
			,d.ApplicationId
		) AS tab
	GROUP BY dealerId
		,DealerName
		,ContactEmail
		,ContactMobile
		,IsDealerActive
		,IsDealerDeleted
		,CityId
		,ApplicationId
		,DealerCity
		,DealerMake
		order by DealerName asc
END

