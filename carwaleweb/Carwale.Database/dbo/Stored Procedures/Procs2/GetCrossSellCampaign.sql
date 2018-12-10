IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCrossSellCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCrossSellCampaign]
GO

	-- =============================================
-- Author:		Sourav Roy						EXEC GetAllVersionsOnRoadPrice 99,13
-- Create date: 30/9/2015
-- Description:	Get Cross Sell Details
-- =============================================
CREATE PROCEDURE [dbo].[GetCrossSellCampaign]
	-- Add the parameters for the stored procedure here
	@CityId INT,
	@ZoneId INT,
	@TargetVersionId INT	
AS
BEGIN
	SELECT CMK.Name AS CrossSellMakeName,CM.Name AS CrossSellModelName,CV.Name AS CrossSellVersionName,CSR.CrossSellVersion AS CrossSellVersionId,CV.CarModelId AS CrossSellModelId,SDT.TemplateName AS TemplateName,SDT.Template AS TemplateHtml FROM PQ_CrossSellCampaignRules CSR WITH (NOLOCK)
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.ID=CSR.CrossSellVersion  
	INNER JOIN CarModels CM  WITH (NOLOCK) ON CM.Id=CV.CarModelId INNER JOIN CarMakes CMK WITH (NOLOCK)ON CMK.ID=CM.CarMakeId 
	INNER JOIN PQ_CrossSellCampaign CSC WITH (NOLOCK) ON CSC.Id=CSR.CrossSellCampaignId 
	INNER JOIN PQ_CrossSell_Template_Platform_Maping CTPM WITH (NOLOCK) ON CTPM.CrossSellCampaignId=CSC.Id
	INNER JOIN PQ_SponsoredDealeAd_Templates SDT WITH (NOLOCK) ON CTPM.AssignedTemplateId=SDT.TemplateId 
	WHERE SDT.PlatformId=1
	AND SDT.CategoryId=2
	AND ( 
	         (CSR.CityId=@CityId AND CSR.ZoneId=@ZoneId) OR (CSR.CityId = -1)
		)
	AND CSR.TargetVersion=@TargetVersionId
END