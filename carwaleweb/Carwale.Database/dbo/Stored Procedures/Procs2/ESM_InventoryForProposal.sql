IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_InventoryForProposal]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_InventoryForProposal]
GO

	
-- =============================================
-- Author	:	Ajay Singh(8rd August 2015)
-- Description	:	Get data for Inventory Repeater 
-- =============================================
CREATE  PROCEDURE [dbo].[ESM_InventoryForProposal]
@ProposalId INT= NULL
AS
BEGIN
	
	SELECT DISTINCT(EI.Id) AS InventoryId,EI.AdFor,EP.Title,EAU.AdUnitName,CM.Name,EPS.Pages,EI.AdUnit,EI.Placement,EI.TargetedCar,EI.Comment,EI.LastUpdatedOn,OU.UserName AS UpdatedBy
	FROM 
		ESM_Inventory EI WITH(NOLOCK)
		INNER JOIN ESM_InventoryBooking EIB WITH(NOLOCK) ON EIB.InventoryId=EI.Id	
		LEFT JOIN ESM_Proposal EP WITH(NOLOCK) ON EP.id=EI.ProposalId
		LEFT JOIN ESM_Pages EPS WITH(NOLOCK) ON EPS.Id=EI.Placement	
		LEFT JOIN ESM_AdUnit EAU WITH(NOLOCK) ON EAU.Id=EI.AdUnit
		LEFT JOIN CarModels CM WITH(NOLOCK) ON CM.ID=EI.TargetedCar	
		LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id=EI.UpdatedBy  
	WHERE
	    (@ProposalId IS NULL OR EI.ProposalId=@ProposalId)AND EIB.isActive=1
	
	ORDER BY EI.Id DESC		  
END
