IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignById]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignById]
GO

	

--Author:Rakesh Yadav On 02 Des 2015
--Desc: Get campaign by campaignId
CREATE PROCEDURE [dbo].[GetCampaignById]
@CampaignId INT 
,@PlatformId INT
AS 

BEGIN
	SELECT DISTINCT DS.Id
		,DCM.DealerId
		,DealerName AS ContactName
		,Phone AS ContactNumber
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
		,ShowEmail AS IsEmailRequired,
		LeadPanel AS LeadPanel
			FROM PQ_DealerSponsored DS WITH(NOLOCK)
	JOIN Dealers D WITH(NOLOCK) ON D.ID=DS.DealerId
	JOIN PQ_DealerCitiesModels DCM WITH(NOLOCK) ON DS.Id = DCM.CampaignId
	JOIN pq_dealerad_template_platform_maping TPM WITH (NOLOCK) ON TPM.CampaignId = DS.Id
	JOIN TC_ContractCampaignMapping CCM WITH (NOLOCK) ON CCM.CampaignId=DS.Id
	WHERE DCM.CampaignId=@CampaignId
	AND TPM.PlatformId=@PlatformId
	AND DS.IsActive=1
	AND d.Status=0
END


