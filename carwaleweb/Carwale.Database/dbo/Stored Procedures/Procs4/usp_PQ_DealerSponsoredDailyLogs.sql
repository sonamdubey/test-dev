IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[usp_PQ_DealerSponsoredDailyLogs]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[usp_PQ_DealerSponsoredDailyLogs]
GO

	
-- =============================================
-- Author:		Kundan Dombale
-- Create date: 09-12-2015
-- Description:	This SP will capture daily snapshot of PQ_DealerSponsored at the start of the day
-- Modified By: Shalini Nair on 17/02/2016 to rename 'Type' column of PQ_DealerSponsored to 'CampaignBehaviour'
-- =============================================
CREATE PROCEDURE [dbo].[usp_PQ_DealerSponsoredDailyLogs]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[PQ_DealerSponsoredDailyLogs] (
		[Id]
		,AsOnDate
		,[ModelId]
		,[CityId]
		,[DealerId]
		,[DealerName]
		,[Phone]
		,[IsActive]
		,[DealerEmailId]
		,[StartDate]
		,[EndDate]
		,[ZoneId]
		,[UpdatedBy]
		,[UpdatedOn]
		,[DealerLeadBusinessType]
		,[IsDesktop]
		,[IsMobile]
		,[IsAndroid]
		,[IsIPhone]
		,[Type]
		,[TotalGoal]
		,[DailyGoal]
		,[TotalCount]
		,[DailyCount]
		,[LeadPanel]
		,[EnableUserEmail]
		,[EnableUserSMS]
		,[CampaignPriority]
		,[LinkText]
		,[IsMailerSent]
		,[EnableDealerEmail]
		,[EnableDealerSMS]
		,[CostPerLead]
		,[ShowEmail]
		,[PausedDate]
		)
	SELECT [Id]
		,Convert(DATE, getdate())
		,[ModelId]
		,[CityId]
		,[DealerId]
		,[DealerName]
		,[Phone]
		,[IsActive]
		,[DealerEmailId]
		,[StartDate]
		,[EndDate]
		,[ZoneId]
		,[UpdatedBy]
		,[UpdatedOn]
		,[DealerLeadBusinessType]
		,[IsDesktop]
		,[IsMobile]
		,[IsAndroid]
		,[IsIPhone]
		,[CampaignBehaviour]
		,[TotalGoal]
		,[DailyGoal]
		,[TotalCount]
		,[DailyCount]
		,[LeadPanel]
		,[EnableUserEmail]
		,[EnableUserSMS]
		,[CampaignPriority]
		,[LinkText]
		,[IsMailerSent]
		,[EnableDealerEmail]
		,[EnableDealerSMS]
		,[CostPerLead]
		,[ShowEmail]
		,[PausedDate]
	FROM [dbo].[PQ_DealerSponsored] WITH (NOLOCK)
END

/****** Object:  StoredProcedure [dbo].[GetCampaignDetails_v16.1.1]    Script Date: 2/29/2016 11:29:02 AM ******/
SET ANSI_NULLS ON
