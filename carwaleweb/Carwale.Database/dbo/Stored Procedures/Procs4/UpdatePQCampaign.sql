IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdatePQCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdatePQCampaign]
GO

	
-- =============================================
-- Author:		Chetan Thambad
-- Create date: 07-10-2015
-- Description:	Updation of platforms and link text
-- Modified By: Shalini Nair on 17/02/2016 to rename 'Type' column of PQ_DealerSponsored to 'CampaignBehaviour'
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePQCampaign] @IsDesktop BIT
	,@IsMobile BIT
	,@IsAndroid BIT
	,@IsIPhone BIT
	,@Id INT
	,@UpdatedBy INT
	,@DealerName VARCHAR(80)
	,@TemplateId VARCHAR(20) = NULL
	,@LinkText VARCHAR(250) = NULL
AS
BEGIN
	UPDATE PQ_DealerSponsored
	SET IsDesktop = @IsDesktop
		,DealerName = @DealerName
		,IsMobile = @IsMobile
		,IsAndroid = @IsAndroid
		,IsIPhone = @IsIPhone
		,LinkText = @LinkText
		,UpdatedBy = @UpdatedBy
		,UpdatedOn = GETDATE()
	WHERE Id = @Id

	EXEC SavePQCampaignTemplate @CampaignId = @Id
		,@TemplateId = @TemplateId

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
		,CampaignPriority
		,LinkText
		,Remarks
		,EnableUserEmail
		,EnableUserSMS
		,EnableDealerEmail
		,EnableDealerSMS
		,CostPerLead
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
		,CampaignPriority
		,LinkText
		,'Record Updated'
		,EnableUserEmail
		,EnableUserSMS
		,EnableDealerEmail
		,EnableDealerSMS
		,CostPerLead
	FROM PQ_DealerSponsored WITH (NOLOCK)
	WHERE Id = @Id
END

/****** Object:  StoredProcedure [dbo].[usp_PQ_DealerSponsoredDailyLogs]    Script Date: 2/29/2016 11:28:41 AM ******/
SET ANSI_NULLS ON
