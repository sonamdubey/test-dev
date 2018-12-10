IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_DropDownList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_DropDownList]
GO

	-- =============================================
-- Author	:	AjaY Singh(18th Aug 2015)
-- Description	:	Get data for Client/Agency,Status,Account Manager 
-- exec DCRM_DealerVisitsTrackerDetail 4,8,2015,86
--Modifier: Ajay Singh(16th nov 2015)
--Description : To remove duplicate data from client dropdown
-- =============================================
CREATE Procedure [dbo].[ESM_DropDownList]
AS
	BEGIN
		-- Get Data For Proposal Status
		EXEC ESM_GetProposalStatus
		-- Get Data For Client/Agency
		SELECT 
	    OrgName AS Text,MAX(id) AS Id
		FROM
		ESM_OrganizationName EON WITH(NOLOCK)
		WHERE EON.IsActive=1
		GROUP BY 
		OrgName
		ORDER BY OrgName
		--Get Data For Account Manager
		SELECT 
		OU.Id AS Id,OU.UserName AS Text
		FROM
		OprUsers OU WITH(NOLOCK)
		INNER JOIN ESM_Users EU With(NOLOCK) ON OU.Id=EU.UserId
		WHERE
		OU.IsActive=1
		ORDER BY
		OU.UserName
	END


	
