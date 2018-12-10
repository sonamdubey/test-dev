IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetAllReportingUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetAllReportingUsers]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(26th May 2015)
-- Description	:	Get all reporting users
-- execute [dbo].[DCRM_GetAllReportingUsers] 3
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetAllReportingUsers]
	
	@UserId	INT 

AS
BEGIN
    
	SELECT 
		OU.Id AS Value,
		OU.UserName AS Text
	FROM
		DCRM_ADM_MappedUsers MU(NOLOCK)
		INNER JOIN OprUsers OU(NOLOCK) ON MU.OprUserId = OU.Id
	WHERE
		MU.NodeRec.GetAncestor(1) = (SELECT NodeRec FROM DCRM_ADM_MappedUsers WHERE OprUserId = @UserId) 
	ORDER BY Text 
END

