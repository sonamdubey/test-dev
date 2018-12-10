IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_GetDealerSponsorshipOnCampaignId_V_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_GetDealerSponsorshipOnCampaignId_V_16]
GO

	
CREATE PROCEDURE [dbo].[PQ_GetDealerSponsorshipOnCampaignId_V_16.1.8] 
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
	,@ShowEmail INT OUTPUT	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT @DealerName = ds.DealerName
		,@DealerMobile = CASE 
				WHEN DS.IsDefaultNumber != 0 THEN
					CTOLL.TollFreeNumber
				ELSE MM.MaskingNumber
					END
		,@DealerEmail = ds.DealerEmailId
		,@DealerActualMobile = dl.MobileNo
		,@DealerLeadBusinessType = dl.DealerLeadBusinessType
		,@Template = SDT.Template, 
		@TemplateName = SDT.TemplateName,
		@LeadPanel = ds.LeadPanel
		,@ActualDealerId = ds.DealerId
		,@LinkText = ds.LinkText
		,@ShowEmail = ds.ShowEmail
	FROM PQ_DealerSponsored ds WITH (NOLOCK)
	INNER JOIN PQ_DealerCitiesModels PCM WITH (NOLOCK) ON PCM.CampaignId = ds.Id
	INNER JOIN Dealers dl WITH (NOLOCK) ON dl.ID = ds.DealerId
	INNER JOIN PQ_DealerAd_Template_Platform_Maping TPM WITH(NOLOCK)  ON TPM.CampaignId = ds.Id
	INNER JOIN PQ_SponsoredDealeAd_Templates SDT WITH(NOLOCK)  ON SDT.TemplateId = TPM.AssignedTemplateId
	LEFT JOIN MM_SellerMobileMasking MM WITH(NOLOCK) ON MM.LeadCampaignId = DS.Id
	LEFT JOIN CarwaleTollFreeNumber CTOLL WITH(NOLOCK) ON DS.IsDefaultNumber = CTOLL.Id
    WHERE ds.Id=@CampaignId AND IsActive = 1
	AND SDT.PlatformId = @PlatformId
	AND TPM.PlatformId = @PlatformId
	AND (ds.EndDate IS NULL AND ds.TotalCount < ds.TotalGoal AND ds.DailyCount < IsNull(ds.DailyGoal,ds.TotalCount)--Lead Based
					 OR 
		(ds.EndDate IS NOT NULL AND CONVERT(DATE,GETDATE()) BETWEEN ds.StartDate AND CONVERT(DATE,IsNUll(DS.EndDate,'2099-12-31'))))--Time Based
	AND (MM.MaskingNumber IS NOT NULL OR DS.IsDefaultNumber != 0 )
END

