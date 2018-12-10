IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'GetAllRunningCampaigns' AND
     DROP VIEW dbo.GetAllRunningCampaigns
GO

	

-- =============================================
-- Created by:		Vicky Lund
-- Creation date:	16-December-2015
-- Description:	Gets all the campaigns which are Running at the moment including the DailyGoal constraint
-- SELECT * FROM GetAllRunningCampaigns
-- =============================================
CREATE VIEW [dbo].[GetAllRunningCampaigns]
AS
SELECT PDS.Id CampaignId
FROM PQ_DealerSponsored PDS WITH (NOLOCK)
INNER JOIN GetAllActiveCampaigns GAAC WITH (NOLOCK) ON PDS.Id = GAAC.CampaignId
	AND (
		(
			GAAC.contractBehaviour = 1
			AND ISNULL(PDS.DailyCount, 0) < ISNULL(PDS.DailyGoal, PDS.TotalGoal)
			)
		OR (
			GAAC.contractBehaviour = 2
			AND (ISNULL(PDS.DailyCount, 0) < ISNULL(PDS.DailyGoal, 999999999))
			)
		)


