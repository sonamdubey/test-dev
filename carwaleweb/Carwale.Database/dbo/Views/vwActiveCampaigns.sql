IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwActiveCampaigns' AND
     DROP VIEW dbo.vwActiveCampaigns
GO

	


-- =============================================
-- Created by:		Vicky Lund
-- Creation date:	16-December-2015
-- Description:	Gets all the campaigns which are Active at the moment excluding the DailyGoal constraint
-- SELECT * FROM vwActiveCampaigns
-- Modified: Vicky Lund, 01/04/2016, Used applicationId column of TC_ContractCampaignMapping
-- Modified: Vicky Lund, 05/04/2016, Used applicationId column of MM_SellerMobileMasking
-- Modified: Chetan Thambad, 13/10/2016, Fetching dealerId
-- =============================================
CREATE VIEW [dbo].[vwActiveCampaigns]
AS
WITH CTE
AS (
	SELECT PDS.Id CampaignId
		,CCM.ContractId
		,CCM.TotalGoal
		,CCM.TotalDelivered TotalCount
		,PDS.DailyGoal
		,PDS.DailyCount
		,CCM.StartDate
		,CCM.EndDate
		,CCM.ContractBehaviour
		,CASE 
			WHEN CCM.ContractBehaviour = 1
				THEN 'Lead based'
			WHEN CCM.ContractBehaviour = 2
				THEN 'Date based'
			END ContractType
		,ROW_NUMBER() OVER (
			PARTITION BY PDS.Id ORDER BY CCM.StartDate ASC
			) AS RowNumber
		,D.ID DealerId
	FROM PQ_DealerSponsored PDS WITH (NOLOCK)
	INNER JOIN TC_ContractCampaignMapping CCM WITH (NOLOCK) ON PDS.Id = CCM.CampaignId
		AND PDS.IsActive = 1
		AND CCM.ApplicationId = 1
		AND PDS.IsDefaultNumber IS NOT NULL
		AND CCM.ContractStatus = 1
		AND (
			(
				--Lead based
				CCM.ContractBehaviour = 1
				AND CCM.StartDate <= CONVERT(DATE, GETDATE())
				AND ISNULL(CCM.TotalDelivered, 0) < ISNULL(CCM.TotalGoal, 999999999)
				)
			OR (
				--Date based
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
		AND D.[Status] = 0
	LEFT OUTER JOIN MM_SellerMobileMasking MSMM WITH (NOLOCK) ON PDS.Id = MSMM.LeadCampaignId
		AND MSMM.ApplicationId = 1
	WHERE (
			MSMM.MM_SellerMobileMaskingId IS NOT NULL
			OR PDS.IsDefaultNumber != 0
			)
	)
SELECT CTE.CampaignId
	,CTE.ContractId
	,CTE.TotalGoal
	,CTE.TotalCount
	,CTE.DailyGoal
	,CTE.DailyCount
	,CTE.StartDate
	,CTE.EndDate
	,CTE.ContractBehaviour
	,CTE.ContractType
	,CTE.DealerId
FROM CTE
WHERE RowNumber = 1

