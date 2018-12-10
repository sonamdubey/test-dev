IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IsCampaignActive]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[IsCampaignActive]
GO

	-- =============================================
-- Created by:		Vicky Lund
-- Creation date:	30-October-2015
-- Description:		
-- SELECT dbo.IsCampaignActive (3907)
-- Modified: Vicky Lund, 01/04/2016, Used applicationId column of TC_ContractCampaignMapping
-- =============================================
CREATE FUNCTION [dbo].[IsCampaignActive] (@CampaignId INT)
RETURNS BIT
AS
BEGIN
	DECLARE @IsActive BIT = 0

	SELECT @IsActive = ISNULL(CASE 
				WHEN (
						(
							(
								PDS.EndDate IS NOT NULL
								AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, PDS.StartDate)
									AND CONVERT(DATE, PDS.EndDate)
								AND (ISNULL(PDS.DailyCount, 0) < ISNULL(PDS.DailyGoal, 999999999))
								)
							OR (
								PDS.EndDate IS NULL
								AND PDS.StartDate <= CONVERT(DATE, GETDATE())
								AND ISNULL(PDS.TotalCount, 0) < PDS.TotalGoal
								AND ISNULL(PDS.DailyCount, 0) < ISNULL(PDS.DailyGoal, PDS.TotalGoal)
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
						)
					THEN 1
				ELSE 0
				END, 0)
	FROM PQ_DealerSponsored PDS WITH (NOLOCK)
	INNER JOIN TC_ContractCampaignMapping CCM WITH (NOLOCK) ON PDS.Id = CCM.CampaignId
		AND PDS.Id = @CampaignId
		AND CCM.ApplicationId = 1
		AND PDS.IsActive = 1
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = PDS.DealerId
		AND D.IsTCDealer = 1

	RETURN @IsActive
END
