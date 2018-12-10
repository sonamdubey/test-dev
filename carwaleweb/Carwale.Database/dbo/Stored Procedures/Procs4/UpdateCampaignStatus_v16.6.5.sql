IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCampaignStatus_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCampaignStatus_v16]
GO

	
-- =============================================
-- Author:		Vinayak
-- Create date: 28/4/16
-- Description:	To toggle IsThirdPartyCampaign Flag of PQ_DealerSponsored
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCampaignStatus_v16.6.5] @Id INT
	,@CampaignProperty INT
	,@Flag BIT
	,@OprUserId INT
AS
BEGIN
	
	IF  @CampaignProperty=1
	BEGIN 
	UPDATE PQ_DealerSponsored
	SET IsThirdPartyCampaign = @Flag
	WHERE Id=@Id;
	END
	ELSE IF @CampaignProperty=2
	BEGIN
	UPDATE PQ_DealerSponsored
	SET isFeaturedEnabled = @Flag
	WHERE Id=@Id;
	END 
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
		,isFeaturedEnabled
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
		,CASE WHEN @CampaignProperty=1 
		THEN 'Third Party campaign set to ' + CASE WHEN @Flag = 1 THEN 'True' ELSE 'False' END
		ELSE 'Featured Property set to ' + CASE WHEN @Flag = 1 THEN 'True' ELSE 'False' END END
		,EnableUserEmail
		,EnableUserSMS
		,EnableDealerEmail
		,EnableDealerSMS
		,CostPerLead
		,IsThirdPartyCampaign
		,isFeaturedEnabled
	FROM PQ_DealerSponsored WITH (NOLOCK)
	WHERE Id = @Id
END

