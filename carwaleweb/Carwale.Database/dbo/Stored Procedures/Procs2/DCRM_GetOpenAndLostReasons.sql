IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetOpenAndLostReasons]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetOpenAndLostReasons]
GO

	
-- =============================================
-- Author		:	Sachin Bharti(15th Sep 2014)
-- Description	:	Get lost and open reasons for added DCRM Packages
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetOpenAndLostReasons]
	
AS
BEGIN
	
	SET NOCOUNT ON;

    -- Select OPEN Reasons
	SELECT ID,Name FROM DCRM_PackageStatusReasons(NOLOCK) WHERE IsOpenReason = 1 ORDER BY Name

	-- Select LOST Reasons
	SELECT ID,Name FROM DCRM_PackageStatusReasons(NOLOCK) WHERE IsLostReason = 1 ORDER BY Name
END

