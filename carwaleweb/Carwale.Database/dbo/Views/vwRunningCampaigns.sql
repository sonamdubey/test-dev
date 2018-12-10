IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwRunningCampaigns' AND
     DROP VIEW dbo.vwRunningCampaigns
GO

	
-- =============================================
-- Created by:		Vicky Lund
-- Creation date:	16-December-2015
-- Description:	Gets all the campaigns which are Running at the moment including the DailyGoal constraint
-- SELECT * FROM vwRunningCampaigns
-- =============================================
CREATE VIEW [dbo].[vwRunningCampaigns]
AS
SELECT vwAC.CampaignId
	,vwAC.ContractId
	,vwAC.TotalGoal
	,vwAC.TotalCount
	,vwAC.DailyGoal
	,vwAC.DailyCount
	,vwAC.StartDate
	,vwAC.EndDate
	,vwAC.ContractBehaviour
	,vwAC.ContractType
FROM vwActiveCampaigns vwAC WITH (NOLOCK)
WHERE (
		(
			vwAC.contractBehaviour = 1
			AND ISNULL(vwAC.DailyCount, 0) < ISNULL(vwAC.DailyGoal, vwAC.TotalGoal)
			)
		OR (
			vwAC.contractBehaviour = 2
			AND (ISNULL(vwAC.DailyCount, 0) < ISNULL(vwAC.DailyGoal, 999999999))
			)
		)


