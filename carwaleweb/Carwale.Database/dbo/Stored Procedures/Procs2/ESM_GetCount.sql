IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_GetCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_GetCount]
GO

	
-- =============================================
-- Author	:	Ajay Singh(29th Oct 2015)
-- Description	:	Get count for inventory
-- =============================================
CREATE PROCEDURE [dbo].[ESM_GetCount]	
AS
BEGIN

SELECT COUNT(EIB.InventoryId) AS TotalInv, EI.AdUnit, CONVERT(Date, EIB.InventoryDate) AS AdDate,MAX(EP.Probability)AS Probability
FROM ESM_InventoryBooking EIB WITH(NOLOCK) 
INNER JOIN ESM_Inventory EI WITH(NOLOCK) ON EIB.InventoryId = EI.Id  
LEFT JOIN ESM_Proposal EP WITH(NOLOCK) ON  EI.ProposalId=EP.id
WHERE EIB.isActive=1
GROUP BY EI.AdUnit, CONVERT(Date, EIB.InventoryDate)

END




