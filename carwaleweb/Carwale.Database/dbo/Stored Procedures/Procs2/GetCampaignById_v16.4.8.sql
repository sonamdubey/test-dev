IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignById_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignById_v16]
GO

	
-- ===================================================================================================
-- Modified: Vicky Lund, 05/04/2016, Used applicationId column of MM_SellerMobileMasking
-- Modified: Vinayak, 06/04/2016, Removed fetching ad templates html  
-- ===================================================================================================

CREATE PROCEDURE [dbo].[GetCampaignById_v16.4.8] @CampaignId INT
	
AS
--Author:Rakesh Yadav On 02 Des 2015
--Desc: Get campaign by campaignId
BEGIN
	SELECT DISTINCT DS.Id
		,DS.DealerId
		,DealerName AS ContactName
		,ContactNumber = CASE 
			WHEN DS.IsDefaultNumber != 0
				THEN CTOLL.TollFreeNumber
			ELSE MM.MaskingNumber
			END
		,D.MobileNo AS ActualMobile
		,DealerEmailId AS ContactEmail
		,vGAC.StartDate
		,vGAC.EndDate
		,D.DealerLeadBusinessType AS Type
		,IsDesktop AS ShowOnDesktop
		,IsMobile AS ShowOnMobile
		,IsAndroid AS ShowOnAndroid
		,IsIPhone AS ShowOniOS
		,vGAC.TotalGoal AS LeadTarget
		,vGAC.TotalCount LeadTargetAchieved
		,vGAC.DailyGoal AS DailyLeadTarget
		,vGAC.DailyCount AS DailyLeadTargetAchieved
		,EnableUserEmail AS NotifyUserByEmail
		,EnableUserSMS AS NotifyUserBySMS
		,CampaignPriority AS Priority
		,LinkText AS ActionText
		,EnableDealerEmail AS NotifyDealerByEmail
		,EnableDealerSMS AS NotifyDealerBySMS
		,ShowEmail AS IsEmailRequired
		,LeadPanel AS LeadPanel
		,DS.IsThirdPartyCampaign AS IsThirdPartyCampaign
	FROM PQ_DealerSponsored DS WITH (NOLOCK)
	JOIN Dealers D WITH (NOLOCK) ON D.ID = DS.DealerId
	JOIN vwRunningCampaigns vGAC WITH (NOLOCK) ON vGAC.CampaignId = DS.Id --changed the view name by Vinayak on 11/02/16 
	LEFT JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON MM.LeadCampaignId = DS.Id
		AND MM.ApplicationId = 1
	LEFT JOIN CarwaleTollFreeNumber CTOLL WITH (NOLOCK) ON DS.IsDefaultNumber = CTOLL.Id
	WHERE DS.Id = @CampaignId
		AND DS.IsActive = 1
		AND d.STATUS = 0
END

