IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignById_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignById_v16]
GO

	--===================================================================================================================
-- Author:Rakesh Yadav On 02 Des 2015
-- Desc: Get campaign by campaignId
-- Modified: Vicky Lund, 01/04/2016, Used applicationId column of TC_ContractCampaignMapping, MM_SellerMobileMasking
--===================================================================================================================
CREATE PROCEDURE [dbo].[GetCampaignById_v16.1.8] @CampaignId INT
	,@PlatformId INT
AS
BEGIN
	SELECT DISTINCT DS.Id
		,DCM.DealerId
		,DealerName AS ContactName
		,ContactNumber = CASE 
			WHEN DS.IsDefaultNumber != 0
				THEN CTOLL.TollFreeNumber
			ELSE MM.MaskingNumber
			END
		,DealerEmailId AS ContactEmail
		,DS.StartDate
		,DS.EndDate
		,D.DealerLeadBusinessType AS Type
		,IsDesktop AS ShowOnDesktop
		,IsMobile AS ShowOnMobile
		,IsAndroid AS ShowOnAndroid
		,IsIPhone AS ShowOniOS
		,DS.TotalGoal AS LeadTarget
		,DS.TotalCount LeadTargetAchieved
		,DailyGoal AS DailyLeadTarget
		,DailyCount AS DailyLeadTargetAchieved
		,EnableUserEmail AS NotifyUserByEmail
		,EnableUserSMS AS NotifyUserBySMS
		,CampaignPriority AS Priority
		,LinkText AS ActionText
		,EnableDealerEmail AS NotifyDealerByEmail
		,EnableDealerSMS AS NotifyDealerBySMS
		,ShowEmail AS IsEmailRequired
		,LeadPanel AS LeadPanel
	FROM PQ_DealerSponsored DS WITH (NOLOCK)
	JOIN Dealers D WITH (NOLOCK) ON D.ID = DS.DealerId
	JOIN PQ_DealerCitiesModels DCM WITH (NOLOCK) ON DS.Id = DCM.CampaignId
	JOIN pq_dealerad_template_platform_maping TPM WITH (NOLOCK) ON TPM.CampaignId = DS.Id
	JOIN TC_ContractCampaignMapping CCM WITH (NOLOCK) ON CCM.CampaignId = DS.Id
		AND CCM.ApplicationID = 1
	LEFT JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON MM.LeadCampaignId = DS.Id
		AND MM.ApplicationId = 1
	LEFT JOIN CarwaleTollFreeNumber CTOLL WITH (NOLOCK) ON DS.IsDefaultNumber = CTOLL.Id
	WHERE DCM.CampaignId = @CampaignId
		AND TPM.PlatformId = @PlatformId
		AND DS.IsActive = 1
		AND d.STATUS = 0
		AND (
			MM.MaskingNumber IS NOT NULL
			OR DS.IsDefaultNumber != 0
			)
END
