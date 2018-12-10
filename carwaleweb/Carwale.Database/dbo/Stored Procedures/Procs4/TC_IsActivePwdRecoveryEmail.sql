IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_IsActivePwdRecoveryEmail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_IsActivePwdRecoveryEmail]
GO

	-- ============================================================================================================================
-- Author		: Suresh Prajapati
-- Created On	: 08th Mar, 2016
-- Description	: This Procedure checks for active user's Email (LoginId)
-- ============================================================================================================================
CREATE PROCEDURE [dbo].[TC_IsActivePwdRecoveryEmail] @LoginId VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT U.Id
		,U.UserName
		,ISNULL(U.PwdRecoveryEmail, U.Email) AS PwdRecoveryEmail
	FROM TC_Users AS U WITH (NOLOCK)
	WHERE U.Email = @LoginId
		AND U.IsActive = 1
END


