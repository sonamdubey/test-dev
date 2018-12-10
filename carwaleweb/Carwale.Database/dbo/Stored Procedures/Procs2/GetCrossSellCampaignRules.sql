IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCrossSellCampaignRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCrossSellCampaignRules]
GO

	
-- =============================================
-- Author: Vinayak
-- Create date: 30/10/2015
-- Description:	Get Cross Sell rules
-- =============================================
CREATE PROCEDURE [dbo].[GetCrossSellCampaignRules]
	-- Add the parameters for the stored procedure here
	@CampaignId INT
AS
BEGIN
	SELECT CSR.StateId,CSR.CityId,CSR.ZoneId,CSR.TargetVersion AS VersionId
	FROM PQ_CrossSellCampaignRules CSR WITH (NOLOCK)
	INNER JOIN PQ_CrossSellCampaign CSC WITH (NOLOCK) ON CSC.Id = CSR.CrossSellCampaignId
	WHERE CSC.CampaignId=@CampaignId
	END


