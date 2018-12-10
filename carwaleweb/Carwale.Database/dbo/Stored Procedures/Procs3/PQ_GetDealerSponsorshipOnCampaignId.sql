IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_GetDealerSponsorshipOnCampaignId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_GetDealerSponsorshipOnCampaignId]
GO

	

-- =============================================
-- Author:		Vinayak Mishra
-- Create date: 4/6/2015
-- Description:	Get Sponsored dealer on campaignId
-- =============================================
CREATE PROCEDURE [dbo].[PQ_GetDealerSponsorshipOnCampaignId] 
	-- Add the parameters for the stored procedure here
	@CampaignId INT
	,@PlatformId TINYINT
	-- Output Parameters 
	,@DealerName VARCHAR(30) OUTPUT
	,@DealerMobile VARCHAR(50) OUTPUT
	,@DealerEmail VARCHAR(50) OUTPUT
	,@DealerActualMobile VARCHAR(100) OUTPUT
	,@DealerLeadBusinessType INT OUTPUT
	,@Template VARCHAR(max) OUTPUT
	,@TemplateName VARCHAR(100) OUTPUT
	,@LeadPanel INT OUTPUT
	,@ActualDealerId INT OUTPUT
	,@LinkText varchar(max) OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT @DealerName = ds.DealerName
		,@DealerMobile = ds.Phone
		,@DealerEmail = ds.DealerEmailId
		,@DealerActualMobile = dl.MobileNo
		,@DealerLeadBusinessType = dl.DealerLeadBusinessType
		,@Template = SDT.Template, 
		@TemplateName = SDT.TemplateName,
		@LeadPanel = ds.LeadPanel
		,@ActualDealerId = ds.DealerId
		,@LinkText = ds.LinkText
	FROM PQ_DealerSponsored ds WITH (NOLOCK)
	INNER JOIN PQ_DealerCitiesModels PCM WITH (NOLOCK) ON PCM.CampaignId = ds.Id
	INNER JOIN Dealers dl WITH (NOLOCK) ON dl.ID = ds.DealerId
	INNER JOIN PQ_DealerAd_Template_Platform_Maping TPM WITH(NOLOCK)  ON TPM.CampaignId = ds.Id
	INNER JOIN PQ_SponsoredDealeAd_Templates SDT WITH(NOLOCK)  ON SDT.TemplateId = TPM.AssignedTemplateId
    WHERE ds.Id=@CampaignId AND IsActive = 1
	AND SDT.PlatformId = @PlatformId
	AND TPM.PlatformId = @PlatformId
	AND ((ds.TotalCount<ds.TotalGoal AND ds.DailyCount<ds.DailyGoal) or ds.Type <> 2)
	AND GETDATE() BETWEEN ds.StartDate AND ds.EndDate;
	END





