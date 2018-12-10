IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DataForDailyScoreBoard]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DataForDailyScoreBoard]
GO

	

-- =============================================
-- Author	:	Sachin Bharti(25th May 2015)
-- Description	:	Get page load data for field executive daily scoreboard
-- execute [dbo].[DCRM_DataForDailyScoreBoard] 3
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DataForDailyScoreBoard]
	@OprUserId	INT
AS
BEGIN
	--get all the matrix those are applicable for user
	SELECT 
		DISTINCT ES.Id AS MatrixId,
		ES.MetricName ,
		MU.UserLevel
	FROM 
		DCRM_FieldExecutivesTarget ET(NOLOCK)
		INNER JOIN DCRM_ExecScoreBoardMetric ES(NOLOCK) ON ES.Id = ET.MetricId
		INNER JOIN DCRM_ADM_MappedUsers MU(NOLOCK) ON MU.OprUserId = @OprUserId
	WHERE 
		ET.OprUserId = @OprUserId

END

