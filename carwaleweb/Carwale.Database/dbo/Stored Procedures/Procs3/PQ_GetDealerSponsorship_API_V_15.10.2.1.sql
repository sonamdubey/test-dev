IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_GetDealerSponsorship_API_V_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_GetDealerSponsorship_API_V_15]
GO

	
-- =============================================
-- Author:		Anchal Gupta
-- Create date: 25/09/2015
-- Description:	adding the toggle value for showing email
-- Modified By Chetan Thambad 14/10/2015 To Show All Cities For Perticular State If Available
-- exec [dbo].[PQ_GetDealerSponsorship_API_V_15.10.2.1] 113,1,'',1
-- =============================================
CREATE PROCEDURE [dbo].[PQ_GetDealerSponsorship_API_V_15.10.2.1]
	-- Add the parameters for the stored procedure here
	@ModelId INT
	,@CityId INT
	,@ZoneId INT
	,@PlatformId TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from   
	-- interfering with SELECT statements.   
	SET NOCOUNT ON;

	WITH dealerCampaigns_cte
	AS (
		SELECT ds.Id
			,ds.DealerName
			,ds.Phone
			,ds.DealerEmailId
			,dl.MobileNo
			,dl.DealerLeadBusinessType
			,SDT.Template
			,SDT.TemplateName
			,ds.LeadPanel
			,ds.DealerId
			,ds.LinkText
			,ds.CampaignPriority
			,ds.ShowEmail
		FROM pq_dealersponsored ds WITH (NOLOCK)
		INNER JOIN pq_dealercitiesmodels PCM WITH (NOLOCK) ON PCM.CampaignId = ds.Id
		INNER JOIN dealers dl WITH (NOLOCK) ON dl.ID = ds.DealerId
		INNER JOIN pq_dealerad_template_platform_maping TPM WITH (NOLOCK) ON TPM.CampaignId = ds.Id
		INNER JOIN pq_sponsoreddealead_templates SDT WITH (NOLOCK) ON SDT.TemplateId = TPM.AssignedTemplateId
		WHERE PCM.MakeId = (SELECT CarMakeId FROM CarModels WITH (NOLOCK) WHERE Id = @ModelId)
			AND (
					PCM.ModelId = @ModelId
					OR PCM.ModelId = - 1
				)
			AND (
				(PCM.StateId = - 1) --Pan India
				OR (
					PCM.StateId = ( SELECT StateId FROM Cities WITH (NOLOCK) WHERE Id = @CityId )
					AND 
						(
							(
								PCM.CityId = @CityId
								AND (ISNULL(PCM.ZoneId, 0) = ISNULL(@ZoneId, 0) OR  @CityId NOT IN (1,10))
							)
							OR PCM.CityId = - 1
						)
					) --City wise or Pan State
				)
			AND ds.IsActive = 1
			AND TPM.PlatformId = @PlatformId --modified by ashish   
			AND (
					(
						ds.EndDate IS NULL
						AND ds.TotalCount < ds.TotalGoal
						AND ds.DailyCount < IsNull(ds.DailyGoal, ds.TotalGoal)
					) --Lead Based
				OR (
						ds.EndDate IS NOT NULL
						AND CONVERT(DATE, GETDATE()) BETWEEN ds.StartDate
						AND CONVERT(DATE, IsNUll(DS.EndDate, '2099-12-31'))
					) --Time Based
				)
		)

	SELECT cc.Id
		,cc.DealerName
		,cc.Phone
		,cc.DealerEmailId
		,cc.MobileNo
		,cc.DealerLeadBusinessType
		,cc.Template
		,cc.TemplateName
		,cc.LeadPanel
		,cc.DealerId
		,cc.LinkText
		,cc.CampaignPriority
		,cc.ShowEmail
	FROM dealerCampaigns_cte cc WITH (NOLOCK)
	WHERE campaignpriority = ( SELECT Min(campaignpriority)	FROM dealerCampaigns_cte WITH (NOLOCK))
END
