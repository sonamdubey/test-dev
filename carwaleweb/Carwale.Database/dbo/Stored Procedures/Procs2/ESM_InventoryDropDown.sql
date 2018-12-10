IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_InventoryDropDown]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_InventoryDropDown]
GO

	
-- =============================================
-- Author:		Ajay Singh
-- Description:	to bind inventory dropdown
-- Modified by:  Amit Yadav(29th Oct 2015)
-- Purpose:		 To get Make and Placement for binding dropdown. 
-- Modified by:  Amit Yadav(4th Nov 2015)
-- Purpose:		 To get Proposal, Status, Account Manager. 
-- =============================================
CREATE PROCEDURE [dbo].[ESM_InventoryDropDown]

@PageId INT = NULL,
@MakeId INT = NULL

AS
BEGIN

	SET NOCOUNT ON;
		-- AD UNIT DATA
		EXECUTE ESM_BindAdUnit @PageId
		--GET STATUS
		EXECUTE ESM_GetProposalStatus
		--GET PLACEMENT
		SELECT Id,Pages FROM  ESM_Pages WITH(NOLOCK)
		ORDER BY Id
		--GET MAKE
		SELECT ID AS Id,Name AS Make FROM CarMakes WITH(NOLOCK) WHERE IsDeleted=0 ORDER BY Make
		--GET PROPOSAL
		SELECT Id AS Id,Title AS Name FROM ESM_Proposal WITH(NOLOCK) WHERE Probability<130  ORDER BY Name
		--GET PLATFORM
		SELECT EP.ESM_PlatformId AS Id, EP.Name AS Name FROM ESM_Platforms EP WITH(NOLOCK)
		--GET MODEL
		SELECT CM.ID AS Id,CM.Name AS Name FROM CarModels CM WITH(NOLOCK) WHERE IsDeleted=0 AND (@MakeId IS NULL OR CM.CarMakeId=@MakeId)
END

