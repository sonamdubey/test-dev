IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_GetClientTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_GetClientTarget]
GO

	-- =============================================
-- Author:		Amit Yadav(8th Oct 2015)
-- Description:	To Get Client Target.
-- =============================================
CREATE PROCEDURE [dbo].[ESM_GetClientTarget]

AS
BEGIN
	SELECT ECT.Id,  
	   EC.Client,
	   EON.OrgName,
	   EFY.FinancialYear,
	   ECT.Target

	FROM ESM_ClientTargets ECT WITH(NOLOCK)
		 LEFT JOIN ESM_Client EC WITH(NOLOCK) ON EC.Id = ECT.Type
		 LEFT JOIN ESM_OrganizationName EON WITH(NOLOCK) ON EON.Id = ECT.OrgId
		 LEFT JOIN ESM_FinancialYear EFY WITH(NOLOCK) ON EFY.Id=ECT.FinancialYear
	
	WHERE 
		EON.IsActive = 1

	ORDER BY
		 EON.OrgName ASC
END


