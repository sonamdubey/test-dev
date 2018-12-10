IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQData]
GO

	-- =============================================
-- Author:		Chetan Thambad
-- Create date: 07-10-2015
-- Description:	Getting Data to prefill
-- EXEC GetPQData 4440, 5
-- EXEC GetPQData 4429,11658
-- Modified By  Chetan Thambad on 11-03-2016 Getting dealerId From PQ_DealerSponsored instead of passing throgh code behind
-- Modified By: Shalini Nair on 25/10/2016 to fetch AssignedGroupId
-- =============================================
CREATE PROCEDURE [dbo].[GetPQData]
	-- Add the parameters for the stored procedure here
	@CampaignId INT,
	@DealerId INT = NULL
AS
BEGIN
		SELECT @DealerId = dealerid from PQ_DealerSponsored WITH(NOLOCK) where Id = @CampaignId

		SELECT DealerName, IsDesktop, IsMobile, IsAndroid, IsIPhone, LinkText, DealerId 
		FROM PQ_DealerSponsored  PQDS WITH(NOLOCK)
		WHERE PQDS.Id= @CampaignId AND DealerId= @DealerId

		SELECT PQDTPM.AssignedTemplateId, PST.TemplateName,PQDTPM.PlatformId,PQDTPM.AssignedGroupId 
		 FROM PQ_DealerAd_Template_Platform_Maping PQDTPM WITH(NOLOCK)
		  left outer join  PQ_SponsoredDealeAd_Templates PST WITH(NOLOCK)
		ON AssignedTemplateId = TemplateId 
		WHERE CampaignId = @CampaignId

END

