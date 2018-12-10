IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_GetInventory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_GetInventory]
GO

	
-- =============================================
-- Author	:	Ajay Singh(8rd August 2015)
-- Description	:	Get data for Inventory Repeater 
-- =============================================
CREATE  PROCEDURE [dbo].[ESM_GetInventory]
@AdUnitDate DATETIME = NULL,
@AdUnitId  INT = NULL
AS
BEGIN
	
	SELECT 
		EI.Id AS InventoryId,EI.ProposalId,EI.AdFor,EP.Title,EAU.AdUnitName,CM.Name,EPS.Pages,EI.AdUnit,EI.Placement,EI.TargetedCar,EI.Comment,EI.LastUpdatedOn,OU.UserName AS UpdatedBy,CONVERT(DATE,EIB.InventoryDate) AS InventoryDate
	FROM 
		ESM_Inventory EI WITH(NOLOCK)
	    INNER JOIN ESM_InventoryBooking EIB WITH(NOLOCK) ON EIB.InventoryId=EI.Id	
		LEFT JOIN ESM_Proposal EP WITH(NOLOCK) ON EP.id=EI.ProposalId
		LEFT JOIN ESM_Pages EPS WITH(NOLOCK) ON EPS.Id=EI.Placement	
		LEFT JOIN ESM_AdUnit EAU WITH(NOLOCK) ON EAU.Id=EI.AdUnit
		LEFT JOIN CarModels CM WITH(NOLOCK) ON CM.ID=EI.TargetedCar	
		LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id=EI.UpdatedBy 
	WHERE (@AdUnitDate IS NULL OR CONVERT(DATE,EIB.InventoryDate)=@AdUnitDate)
	    AND(@AdUnitId IS NULL OR EI.AdUnit=@AdUnitId)
		AND EIB.isActive = 1
	ORDER BY EI.Id DESC		  
END
