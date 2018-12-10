IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_GetDealerSponsorship_API_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_GetDealerSponsorship_API_V15]
GO

	

-- =============================================   
-- Author:    Ashish Verma   
-- Create date: 15/07/2014   
-- Description:  Get Sponsored dealer   
-- exec [dbo].[PQ_GetDealerSponsorshipV1.1] 35,1,""   
-- Modified By Vikas : on <11/8/2014>  modified the where clause for cityid and zoneid   
-- modified by ashish : on 21/08/2014 for showing both autobiz and crm ad on mobile site   
-- modified by vinayak <31/10/2014> for constrain ads based on count and types   
-- Modified By ashish on 11/07/2014 added 1 extra input and 3 output parameters   
-- input paramrter added by ashish PlatformId   
-- output parameters aaded by ashish ,@Template VARCHAR(1000) OUTPUT--Modified By ashishTemplateName,LeadPanel   
-- Modified By: Shalini Nair on 19/03/15 to retrieve ActualDealerId    
-- Modified By : Shalini Nair on 07/04/15 to retrieve Custom linkText   
-- =============================================   
CREATE PROCEDURE [dbo].[PQ_GetDealerSponsorship_API_V15.5.2]
	-- Add the parameters for the stored procedure here   
	@ModelId NUMERIC
	,@CityId INT
	,@ZoneId INT
	,@PlatformId TINYINT --Modified By ashish   
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
			,--Modified By ashish                 
			ds.DealerId
			,ds.LinkText
			,ds.CampaignPriority
		FROM pq_dealersponsored ds WITH (NOLOCK)
		INNER JOIN pq_dealercitiesmodels PCM WITH (NOLOCK) ON PCM.CampaignId = ds.Id
		INNER JOIN dealers dl WITH (NOLOCK) ON dl.ID = ds.DealerId
		INNER JOIN pq_dealerad_template_platform_maping TPM WITH (NOLOCK) ON TPM.CampaignId = ds.Id
		INNER JOIN pq_sponsoreddealead_templates SDT WITH (NOLOCK) ON SDT.TemplateId = TPM.AssignedTemplateId
		WHERE (
				(
					PCM.CityId = @cityid
					AND Isnull(PCM.ZoneId, 0) = Isnull(@ZoneId, 0)
					)
				-- modified by vikas                  
				OR PCM.CityId = - 1
				)
			AND PCM.ModelId = @ModelId
			AND isactive = 1
			AND SDT.PlatformId = @PlatformId
			AND TPM.PlatformId = @PlatformId --modified by ashish   
			AND (
				(
					ds.TotalCount < ds.TotalGoal
					AND ds.DailyCount < ds.DailyGoal
					)
				OR ds.type <> 2
				) --modified by vinayak   
			AND Getdate() BETWEEN ds.StartDate
				AND ds.EndDate
		)
	SELECT *
	FROM dealerCampaigns_cte
	WHERE campaignpriority = (
			SELECT Min(campaignpriority)
			FROM dealerCampaigns_cte
			)
END

