IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQDealersSponserdRules_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQDealersSponserdRules_16]
GO

	-- =============================================
-- Author: Vinayak   
-- Create date:06/10/2015
-- Description:for Getting pq dealers data
-- EXEC [dbo].[GetPQDealersSponserdRules_16.7.6] 3691
-- =============================================
CREATE PROCEDURE [dbo].[GetPQDealersSponserdRules_16.7.6] 
     @CampaignId int
     AS
             BEGIN
				SELECT DCM.StateId,DCM.CityId,DCM.ZoneId,DCM.MakeId,DCM.ModelId,PDS.DealerId
				FROM PQ_DealerCitiesModels DCM WITH(NOLOCK) 
				INNER JOIN PQ_dealerSponsored PDS WITH(NOLOCK) ON PDS.ID=DCM.CampaignId
				WHERE CampaignId=@CampaignId
				END
