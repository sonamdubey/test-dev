IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'GetAllActiveCampaigns' AND
     DROP VIEW dbo.GetAllActiveCampaigns
GO

	
-- =============================================
-- Created by:		Vicky Lund
-- Creation date:	16-December-2015
-- Description:	Gets all the campaigns which are Active at the moment excluding the DailyGoal constraint
-- SELECT * FROM GetAllActiveCampaigns
-- =============================================
CREATE VIEW [dbo].[GetAllActiveCampaigns]
AS
SELECT DISTINCT PDS.Id CampaignId
	,CCM.ContractBehaviour
FROM PQ_DealerSponsored PDS WITH (NOLOCK)
INNER JOIN TC_ContractCampaignMapping CCM WITH (NOLOCK) ON PDS.Id = CCM.CampaignId
	AND PDS.IsActive = 1
	AND CCM.ContractStatus = 1
	AND (
		(
			CCM.ContractBehaviour = 1
			AND CCM.StartDate <= CONVERT(DATE, GETDATE())
			AND ISNULL(CCM.TotalDelivered, 0) < ISNULL(CCM.TotalGoal, 999999999)
			)
		OR (
			CCM.ContractBehaviour = 2
			AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, CCM.StartDate)
				AND CONVERT(DATE, CCM.EndDate)
			)
		)
	AND (
		(
			EXISTS (
				SELECT DCM.Id
				FROM PQ_DealerCitiesModels DCM WITH (NOLOCK)
				WHERE DCM.CampaignId = PDS.Id
				)
			AND EXISTS (
				SELECT DATPM.ID
				FROM PQ_DealerAd_Template_Platform_Maping DATPM WITH (NOLOCK)
				WHERE DATPM.CampaignId = PDS.Id
				)
			)
		OR (
			EXISTS (
				SELECT PCSCR.Id
				FROM PQ_CrossSellCampaignRules PCSCR WITH (NOLOCK)
				INNER JOIN PQ_CrossSellCampaign PCSC WITH (NOLOCK) ON PCSCR.CrossSellCampaignId = PCSC.Id
					AND PCSC.CampaignId = PDS.Id
				)
			)
		OR (
			EXISTS (
				SELECT DLC.DealerLocatorConfigurationId
				FROM DealerLocatorConfiguration DLC WITH (NOLOCK)
				WHERE DLC.IsLocatorActive = 1
					AND DLC.IsDealerLocatorPremium = 1
					AND DLC.PQ_DealerSponsoredId = PDS.Id
				)
			)
		)
INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = PDS.DealerId
	AND D.IsTCDealer = 1

