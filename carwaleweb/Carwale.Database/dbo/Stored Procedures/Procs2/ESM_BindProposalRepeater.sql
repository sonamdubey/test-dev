IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_BindProposalRepeater]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_BindProposalRepeater]
GO

	-- =============================================
-- Author	:	Ajay Singh(12rd August 2015)
-- Description	:	Get data for Repeater 
-- Modifier : Amit Yadav(1st Oct 2015)
-- Description : To remove inactive Client/Agency
-- =============================================
CREATE Procedure [dbo].[ESM_BindProposalRepeater]
AS
BEGIN

	SELECT EON.id AS id, EON.OrgName as OrgName, EON.type as type, EON.IsActive as IsActive,OU.UserName as AccountManager
    FROM ESM_organizationName EON WITH(NOLOCK)
		LEFT JOIN OprUsers OU  WITH(NOLOCK) ON  OU.Id = EON.AccountManager AND 	OU.IsActive=1
	WHERE EON.IsActive=1
	ORDER BY EON.OrgName  ASC
END


