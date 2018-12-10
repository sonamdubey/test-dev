IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCampaignStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCampaignStatus]
GO

	
-- =============================================
-- Author:		Chetan Thambad	
-- Create date: 16th Feb 2016
-- Description:	To activate/deactivate the PQ Campaigns and maintain a log of it
-- Added Conditional Remarks(vinayak)
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCampaignStatus] @Ids VARCHAR(500)
	,@DeletedBy INT
	,@IsActive BIT
AS
BEGIN
	UPDATE PQ_DealerSponsored
	SET IsActive = @IsActive
		,PausedDate = CASE 
			WHEN @IsActive = 0
				THEN GETDATE()
			ELSE PausedDate
			END
		,UpdatedBy = @DeletedBy
		,UpdatedOn = GETDATE()
	WHERE Id IN (
			SELECT ListMember
			FROM fnSplitCSV(@Ids)
			)

	INSERT INTO PQ_DealerSponsoredLog (
		PQ_DealerSponsoredId
		,DealerId
		,DealerName
		,Phone
		,IsActive
		,DealerEmailId
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
		,EnableUserEmail
		,EnableUserSMS
		,EnableDealerEmail
		,EnableDealerSMS
		,CampaignPriority
		,Remarks
		)
	SELECT Id
		,DealerId
		,DealerName
		,Phone
		,IsActive
		,DealerEmailId
		,StartDate
		,EndDate
		,UpdatedBy
		,UpdatedOn
		,IsDesktop
		,IsMobile
		,IsAndroid
		,IsIPhone
		,CampaignBehaviour
		,TotalGoal
		,DailyGoal
		,LeadPanel
		,EnableUserEmail
		,EnableUserSMS
		,EnableDealerEmail
		,EnableDealerSMS
		,CampaignPriority
		,CASE 
			WHEN @IsActive = 1
				THEN 'Record Activated'
			ELSE 'Record Deactivated'
			END
	FROM PQ_DealerSponsored WITH (NOLOCK)
	WHERE Id IN (
			SELECT ListMember
			FROM fnSplitCSV(@Ids)
			)
END

