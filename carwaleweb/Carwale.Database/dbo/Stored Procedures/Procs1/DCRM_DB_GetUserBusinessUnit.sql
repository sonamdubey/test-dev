IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DB_GetUserBusinessUnit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DB_GetUserBusinessUnit]
GO

	-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 26-02-2016
-- Description:	fetch users business unit id 
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DB_GetUserBusinessUnit] 
	@UserId				INT
AS
BEGIN
    DECLARE @AliasUserId INT = 0

	SELECT @AliasUserId=ISNULL(DAMU.AliasUserId,0)
	FROM   DCRM_ADM_MappedUsers(NOLOCK) DAMU
	WHERE  DAMU.OprUserId=@UserId AND IsActive=1

	IF @AliasUserId > 0
	BEGIN
		SELECT DAMU.OprUserId,DAMU.BusinessUnitId
		FROM   DCRM_ADM_MappedUsers(NOLOCK) DAMU
		WHERE  DAMU.OprUserId=@AliasUserId AND IsActive=1
	END
	ELSE
	BEGIN
		SELECT DAMU.OprUserId,DAMU.BusinessUnitId
		FROM   DCRM_ADM_MappedUsers(NOLOCK) DAMU
		WHERE  OprUserId=@UserId AND IsActive=1
	END

END

-----------------------------------------------------------------------------------------------------------------

