IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetBusinessId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetBusinessId]
GO

	
-- =============================================
-- Author	:	Ajay Singh(2th Dec 2015)
-- Description	:to get Business id of Current user
-- EXEC DCRM_GetBusinessId 51
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetBusinessId]
@CurrentUserId INT = NULL,
@Retval INT = NULL OUT
AS
BEGIN
	SET @Retval = (SELECT BusinessUnitId from DCRM_ADM_MappedUsers AS AD WITH(NOLOCK) WHERE AD.OprUserId=@CurrentUserId AND AD.IsActive=1 )
	
END




