IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetBUUserInfo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetBUUserInfo]
GO

	-- =============================================
-- Author	   : Suresh Prajapati
-- Created On  : 08th July, 2015
-- Description : Get user logins
-- EXEC DCRM_GetBUUserInfo 1
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetBUUserInfo] 
	@BusinessUnitId INT
AS
BEGIN
	SELECT 
		OU.LoginId,
		OU.UserName
	FROM 
		DCRM_ADM_MappedUsers AS MU  WITH (NOLOCK)
		INNER JOIN OprUsers AS OU WITH (NOLOCK) ON MU.OprUserId = OU.Id
	WHERE 
		MU.BusinessUnitId = @BusinessUnitId
		AND OU.IsActive = 1
		AND MU.IsActive = 1
		--AND ou.Id = 85
END

