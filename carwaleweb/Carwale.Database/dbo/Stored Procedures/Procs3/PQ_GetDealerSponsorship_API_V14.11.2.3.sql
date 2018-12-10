IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_GetDealerSponsorship_API_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_GetDealerSponsorship_API_V14]
GO

	-- =============================================
-- Author:		Ashish Verma
-- Create date: 15/07/2014
-- Description:	Get Sponsored dealer
-- exec [dbo].[PQ_GetDealerSponsorshipV1.1] 35,1,""
-- Modified By Vikas : on <11/8/2014>  modified the where clause for cityid and zoneid
--modified by ashish : on 21/08/2014 for showing both autobiz and crm ad on mobile site
--modified by vinayak <31/10/2014> for constrain ads based on count and types
--Modified By ashish on 11/07/2014 added 1 extra input and 3 output parameters
-- input paramrter added by ashish PlatformId
-- output parameters aaded by ashish ,@Template VARCHAR(1000) OUTPUT--Modified By ashishTemplateName,LeadPanel
-- =============================================
CREATE PROCEDURE [dbo].[PQ_GetDealerSponsorship_API_V14.11.2.3] 
	-- Add the parameters for the stored procedure here
	@ModelId NUMERIC
	,@CityId INT
	,@ZoneId INT
	,@PlatformId TINYINT--Modified By ashish
	-- Output Parameters 
	,@CampaignId INT OUTPUT
	,@DealerName VARCHAR(30) OUTPUT
	,@DealerMobile VARCHAR(50) OUTPUT
	,@DealerEmail VARCHAR(50) OUTPUT
	,@DealerActualMobile VARCHAR(100) OUTPUT
	,@DealerLeadBusinessType INT OUTPUT
	,@Template VARCHAR(max) OUTPUT--Modified By ashish
	,@TemplateName VARCHAR(100) OUTPUT
	,@LeadPanel INT OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	--SELECT DealerId,DealerName,Phone AS PhoneNo,DealerEmailId AS DealerEmail
	--FROM PQ_DealerSponsored WITH(NOLOCK)
	--WHERE CityId= @CityId and ModelId=@ModelId and IsActive = 1
	SELECT Top 1 @CampaignId = ds.Id
		,@DealerName = ds.DealerName
		,@DealerMobile = ds.Phone
		,@DealerEmail = ds.DealerEmailId
		,@DealerActualMobile = dl.MobileNo
		,@DealerLeadBusinessType = dl.DealerLeadBusinessType
		,@Template = SDT.Template, --Modified By ashish
		@TemplateName = SDT.TemplateName, --Modified By ashish
		@LeadPanel = ds.LeadPanel--Modified By ashish
	
	FROM PQ_DealerSponsored ds WITH (NOLOCK)
	INNER JOIN PQ_DealerCitiesModels PCM WITH (NOLOCK) ON PCM.CampaignId = ds.Id
	INNER JOIN Dealers dl WITH (NOLOCK) ON dl.ID = ds.DealerId
	INNER JOIN PQ_DealerAd_Template_Platform_Maping TPM WITH(NOLOCK)  ON TPM.CampaignId = ds.Id
	INNER JOIN PQ_SponsoredDealeAd_Templates SDT WITH(NOLOCK)  ON SDT.TemplateId = TPM.AssignedTemplateId
		WHERE	( (PCM.CityId=@cityid  AND ISNULL(PCM.ZoneId,0) =ISNULL(@ZoneId,0) --modified by vikas
                          )-- modified by vikas
                     OR PCM.CityId=-1
                   )
	AND PCM.ModelId=@ModelId AND IsActive = 1 --modified by ashish 
	AND SDT.PlatformId = @PlatformId --modified by ashish
	AND TPM.PlatformId = @PlatformId --modified by ashish
	AND ((ds.TotalCount<ds.TotalGoal AND ds.DailyCount<ds.DailyGoal) or ds.Type <> 2)--modified by vinayak
	AND CONVERT(date,GETDATE()) BETWEEN  CONVERT(date,ds.StartDate) AND CONVERT(date,ds.EndDate)-- modified by vikas
	ORDER BY ds.CampaignPriority, NEWID();

END



/****** Object:  StoredProcedure [dbo].[GetUserInfoCarDetails]    Script Date: 2/16/2015 5:13:12 PM ******/
-- SET ANSI_NULLS ON
