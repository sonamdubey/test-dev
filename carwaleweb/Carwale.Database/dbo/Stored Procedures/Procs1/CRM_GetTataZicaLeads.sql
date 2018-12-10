IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetTataZicaLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetTataZicaLeads]
GO

	
-- ===============================================
-- Author:		VAIBHAV K
-- Create date: 15-Jan-2016
-- Purpose:     Get Tata Zica Leads fro API push.
-- Modified:	Vaibhav K 2-Feb-2016 if no mapped dealer found send details as per Mumbai Dealership
-- ===============================================
CREATE PROCEDURE [dbo].[CRM_GetTataZicaLeads]

AS	

BEGIN	

	with cte as
	(
		SELECT TOP 30 TDR.Id TDRId, TDR.Name as FirstName,TDR.Email,TDR.Mobile,
		C.Name AS CityName, ST.StateCode,C.DefaultPinCode,
		--'Zica XZ 1.05 D' VariantName,'Zica' AS ModelName,
		'Tiago XZ 1.05 D' VariantName, 'Tiago' ModelName,
		(
			SELECT TOP 1 CTD.DealerDivisionId
			FROM CRM_TataCityDealers CTD WITH (NOLOCK)
			WHERE CTD.IsActive = 1
			AND 
			(
			CTD.CWCityId = TDR.CityId AND CTD.PplRowId = '1-DR4I0XM' -- Deales mapped with Zica model
			OR 
			CTD.CWCityId = TDR.CityId) --if not found for that model take any dealer for that city
		) AS DealerCode
		FROM NCS_TDReq TDR WITH (NOLOCK)
		LEFT JOIN Cities C WITH (NOLOCK) ON C.ID = TDR.CityId
		LEFT JOIN States ST WITH(NOLOCK) ON C.StateId = ST.ID
		WHERE TDR.ModelId = 852	   	  
		AND ISNULL(TDR.ApiResponse,'')  = ''
		--AND CAST(TDR.CreatedOn AS date) = CAST(GETDATE()-1 AS date)
		ORDER BY NEWID() 
	)/*
	SELECT * 
	FROM cte WITH (NOLOCK)
	WHERE DealerCode IS NOT NULL AND DealerCode <> ''*/

	--Vaibhav K 2-Feb-2016
	--1-2B59I1 dealer division id for mumbai 
	--400001 Mumbai MH for mumbai by default
	--Vaibhav K 11 Apr 2016 commented below code as we have stopped pushing leads to default Mumbai
	/*
	SELECT cte.TDRId, cte.FirstName, cte.Email,cte.Mobile, cte.ModelName, cte.VariantName
	,CASE ISNULL(cte.DealerCode,'') 
	WHEN '' THEN '1-2B59I1' ELSE cte.DealerCode END AS DealerCode 
	,CASE ISNULL(cte.DealerCode,'') 
	WHEN '' THEN '400001' ELSE cte.DefaultPinCode END AS DefaultPinCode 
	,CASE ISNULL(cte.DealerCode,'')
	WHEN '' THEN 'Mumbai' ELSE cte.CityName END AS CityName 
	,CASE ISNULL(cte.DealerCode,'') 
	WHEN '' THEN 'MH' ELSE cte.StateCode END AS StateCode 
	FROM cte WITH (NOLOCK)
	*/
	
	SELECT cte.TDRId, cte.FirstName, cte.Email,cte.Mobile, cte.ModelName, cte.VariantName,
	DealerCode, DefaultPinCode, CityName, StateCode
	FROM cte WITH (NOLOCK)
	WHERE ISNULL(cte.DealerCode, '') <> '' AND 1 = 2--Vaibhav K 11 Apr 2016 to get no data from the SP to stop pushing leads
	
	/*
	WITH Cte1

	AS (
	SELECT TOP 20 TDR.Id TDRId, ND.Id DealerId, TDR.Name as FirstName,TDR.Mobile,C.Name AS CityName, ST.StateCode,C.DefaultPinCode,
	TDR.Email,'Zica XZ 1.05 D' VariantName,'Zica' AS ModelName,nd.IsActive,'1-11GEZ8N' DealerCode--ND.DealerCode
	FROM NCS_TDReq TDR WITH (NOLOCK)
	INNER JOIN NCS_Dealers ND WITH (NOLOCK) ON TDR.CityId = ND.CityId
	--INNER JOIN NCS_DealerModels DM WITH (NOLOCK) ON DM.DealerId = ND.ID AND TDR.ModelId = 852 AND DM.ModelId = 852 -- Zica model id = 852
	INNER JOIN Cities C WITH (NOLOCK) ON C.ID = ND.CityId
	INNER JOIN States ST WITH(NOLOCK) ON C.StateId = ST.ID
	WHERE TDR.ModelId = 852
		  --AND ND.DealerCode IS NOT NULL 
		  --AND ND.DealerCode <> '' 
		  --AND ND.IsActive = 1 
		  AND ISNULL(TDR.ApiResponse,'')  = ''
	--AND CAST(TDR.CreatedOn AS date) = CAST(GETDATE()-1 AS date)
	),
	Cte2
	AS (SELECT *,ROW_NUMBER() OVER (PARTITION BY TDRId ORDER BY TDRId DESC) RowNumber
	FROM Cte1)

	SELECT *  FROM   Cte2 WITH (NOLOCK)
	WHERE RowNumber = 1
	*/
END
-----------------------------------------------------------------------------------------------------------------------------------------------------------


