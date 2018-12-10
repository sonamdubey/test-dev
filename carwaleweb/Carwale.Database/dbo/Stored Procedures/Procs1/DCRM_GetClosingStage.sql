IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetClosingStage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetClosingStage]
GO
	-- =============================================
-- Author	:	Sachin Bharti(26th Sep 2014)
-- Description	:	Get Closing Stages for packages
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetClosingStage]
	

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT *FROM Dcrm_PackagesClosingStage DP(NOLOCK) WHERE DP.ClosingStage <> 100
END
