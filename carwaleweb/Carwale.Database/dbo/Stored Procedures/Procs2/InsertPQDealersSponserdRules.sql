IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPQDealersSponserdRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPQDealersSponserdRules]
GO
	-- =============================================
-- Author: Chetan Thambad   
-- Create date:06/10/2015
-- Description:for inserting pq dealers data
-- Modified by Chetan on <11/3/2016>: Fetching dealerId by campaignId from PQ_DealerSponsored
-- =============================================
CREATE PROCEDURE [dbo].[InsertPQDealersSponserdRules] 
	@CampaignId int,
	@CityId int,
	@ZoneId int=null, 
	@DealerId int=null, 
	@ModelId int, 
	@StateId int, 
	@MakeId int,
	@DeletedBy int,
	@DeletedOn datetime= GETDATE

	AS
        BEGIN

		  SELECT @DealerId = DealerId FROM PQ_DealerSponsored WITH(NOLOCK) WHERE Id = @CampaignId

          INSERT INTO PQ_DealerCitiesModels (PqId, CampaignId, CityId, ZoneId, DealerId, ModelId, StateId, MakeID)
            VALUES (@CampaignId, @CampaignId, @CityId, @ZoneId, @DealerId, @ModelId, @StateId, @MakeId)

			INSERT INTO PQ_DealerCitiesModelsLog (PQ_DealerCitiesModelsId, CampaignId, CityId, ZoneId, DealerId, ModelId, StateId, MakeID, DeletedBy, DeletedOn)
            VALUES (SCOPE_IDENTITY() ,@CampaignId, @CityId, @ZoneId, @DealerId, @ModelId, @StateId, @MakeId, @DeletedBy, @DeletedOn)
        END