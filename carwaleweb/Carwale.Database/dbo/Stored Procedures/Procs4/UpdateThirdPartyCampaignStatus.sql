IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateThirdPartyCampaignStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateThirdPartyCampaignStatus]
GO

	-- =============================================
-- Author:		Vinayak
-- Create date: 28/4/16
-- Description:	To toggle IsThirdPartyCampaign Flag of PQ_DealerSponsored
-- =============================================
CREATE PROCEDURE [dbo].[UpdateThirdPartyCampaignStatus] @Id INT
	,@EnableThirdPartyCampaign BIT
	,@OprUserId INT
AS
BEGIN
	UPDATE PQ_DealerSponsored
	SET IsThirdPartyCampaign = @EnableThirdPartyCampaign
	WHERE Id=@Id

	-- Logging

	INSERT INTO PQ_DealerSponsoredLog (
		PQ_DealerSponsoredId
		,DealerId
		,DealerName
		,Phone
		,IsActive
		,DealerEmailId
		,NotificationMobile
		,StartDate
		,EndDate
		,ActionTakenBy
		,ActionTakenOn
		,IsDesktop
		,IsMobile
		,IsAndroid
		,IsIPhone
		,CampaignBehaviour
		,TotalGoal
		,DailyGoal
		,LeadPanel
		,CampaignPriority
		,LinkText
		,Remarks
		,EnableUserEmail
		,EnableUserSMS
		,EnableDealerEmail
		,EnableDealerSMS
		,CostPerLead
		,IsThirdPartyCampaign
		)
	SELECT Id
		,DealerId
		,DealerName
		,Phone
		,IsActive
		,DealerEmailId
		,NotificationMobile
		,StartDate
		,EndDate
		,@OprUserId
		,GETDATE()
		,IsDesktop
		,IsMobile
		,IsAndroid
		,IsIPhone
		,CampaignBehaviour
		,TotalGoal
		,DailyGoal
		,LeadPanel
		,CampaignPriority
		,LinkText
		,'Third Party campaign set to ' + CASE WHEN @EnableThirdPartyCampaign = 1 THEN 'True' ELSE 'False' END 
		,EnableUserEmail
		,EnableUserSMS
		,EnableDealerEmail
		,EnableDealerSMS
		,CostPerLead
		,IsThirdPartyCampaign
	FROM PQ_DealerSponsored WITH (NOLOCK)
	WHERE Id = @Id
END

