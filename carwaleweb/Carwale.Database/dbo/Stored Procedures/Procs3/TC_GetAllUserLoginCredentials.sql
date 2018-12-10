IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAllUserLoginCredentials]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAllUserLoginCredentials]
GO
	-- ==========================================================================
-- Author		: Suresh Prajapati
-- Created on	: 25th Feb, 2015
-- Description	: To get all TC user's EmailId and Password
-- ==========================================================================
CREATE PROCEDURE [dbo].[TC_GetAllUserLoginCredentials]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT U.Id
		--,Email
		,[Password]
	FROM TC_Users AS U WITH (NOLOCK)
	--JOIN Dealers AS D WITH (NOLOCK) ON D.ID = U.BranchId
		WHERE U.[Password] IS NOT NULL
		--	AND D.IsDealerActive = 1
		--	AND D.IsDealerDeleted = 0
END


