IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_GetClients]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_GetClients]
GO

	-- =============================================
-- Author:		Amit Yadav
-- Create date: 12th Oct 2015
-- Description:	To get the Client And Agency Table.
-- =============================================
CREATE PROCEDURE [dbo].[ESM_GetClients]

AS
BEGIN
	
	SELECT EON.ID,EON.OrgName,EON.type,OU.UserName AS AccountManager,SUM(ECT.Target) AS Target,
		   ISNULL(ERR.FinalRoValue,EP.ProposedAmount) AS Amount

	FROM 
		ESM_OrganizationName EON 
		LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON  OU.Id = EON.AccountManager
		LEFT JOIN ESM_ClientTargets AS ECT WITH(NOLOCK) ON ECT.OrgId = EON.id
		LEFT JOIN ESM_Proposal AS EP WITH(NOLOCK) ON EP.ClientId = EON.id And EP.Probability > 100
		LEFT JOIN ESM_ROReceives AS ERR WITH(NOLOCK) ON ERR.ESM_ProposedProductId=EP.id  
	WHERE
	 
		EON.IsActive=1 
	
	GROUP BY EON.id,EON.OrgName,EON.type,EON.IsActive,OU.UserName,EON.UpdatedOn,EP.ProposedAmount,EP.Probability,ERR.FinalRoValue

	ORDER BY EON.OrgName
END
