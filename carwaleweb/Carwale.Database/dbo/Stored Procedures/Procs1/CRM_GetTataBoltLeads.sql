IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetTataBoltLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetTataBoltLeads]
GO

	-- ===============================================
-- Author:		Yuga Hatolkar
-- Create date: 29/12/2014 
-- Purpose:     Push Tata Bolt Leads.
-- ===============================================
CREATE PROCEDURE [dbo].[CRM_GetTataBoltLeads]

AS	

BEGIN	

WITH Cte1

AS (
SELECT TOP 20 TDR.Id TDRId, ND.Id DealerId, TDR.Name as FirstName,TDR.Mobile,C.Name AS CityName, ST.StateCode,C.DefaultPinCode,TDR.Email,'Bolt XT QJT 75PS' VariantName,'Bolt' AS ModelName,nd.IsActive,ND.DealerCode
FROM NCS_TDReq TDR
INNER JOIN NCS_Dealers ND ON TDR.CityId = ND.CityId
INNER JOIN NCS_DealerModels DM ON DM.DealerId = ND.ID AND TDR.ModelId = 586 AND DM.ModelId = 586
INNER JOIN Cities C ON C.ID = ND.CityId
INNER JOIN States ST WITH(NOLOCK) ON C.StateId = ST.ID
WHERE ND.DealerCode IS NOT NULL 
	  AND ND.DealerCode <> '' 
	  AND ND.IsActive = 1 
      AND TDR.PushedLeadId IS NULL
AND CAST(TDR.CreatedOn AS date) = CAST(GETDATE()-1 AS date)
),
Cte2
AS (SELECT *,ROW_NUMBER() OVER (PARTITION BY TDRId ORDER BY TDRId DESC) RowNumber
FROM Cte1)

SELECT * FROM   Cte2
WHERE RowNumber = 1

END
