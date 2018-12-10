IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetUserLevel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetUserLevel]
GO

	

-- =============================================
-- Author	:	Sachin Bharti(10th June 2015)
-- Description	:	Get field executives hierarchy level
-- Modifier	:	Sachin Bharti(13th July 2015)
-- Purpose	:	Added @AliasUserId and  @@AliasUserName as a output parameter
--				Get aliasUserId when user does not have any data
-- execute [dbo].[M_GetUserLevel] 59,null,null
-- =============================================
CREATE PROCEDURE [dbo].[M_GetUserLevel]
	
	@OprUserId	INT ,
	@UserLevel	TINYINT = NULL OUTPUT ,
	@NumberOfReportee	TINYINT = NULL OUTPUT,
	@AliasUserId	INT = NULL OUTPUT,
	@AliasUserName	VARCHAR(50) = NULL OUTPUT
AS
BEGIN
	SET @UserLevel = NULL
	SET @NumberOfReportee = NULL
	
	SELECT
		@UserLevel = MU.UserLevel,
		--@AliasUserId = MU.AliasUserId,
		@AliasUserId = MU.OprUserId,
		@AliasUserName = OU.UserName
	FROM
		DCRM_ADM_MappedUsers MU(NOLOCK)
		--INNER JOIN OprUsers OU(NOLOCK) ON MU.AliasUserId = OU.Id
		INNER JOIN OprUsers OU(NOLOCK) ON MU.OprUserId = OU.Id
	WHERE
		MU.OprUserId = @OprUserId
		AND MU.IsActive = 1

	SELECT @NumberOfReportee = (SELECT TOP 1 COUNT(*) FROM [dbo].Fn_DCRM_GetAllChildUsers(@AliasUserId))

	PRINT @UserLevel
	PRINT @NumberOfReportee
	PRINT @AliasUserId
	PRINT @AliasUserName	
END