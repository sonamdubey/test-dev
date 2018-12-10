IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_UpdateReplacingModelRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_UpdateReplacingModelRules]
GO

	-- =============================================
-- Author:		<Khushaboo Patil >
-- Create date: <1 Apr 2015>
-- Description:	<Update ReplacingModel Rules>
-- =============================================
CREATE PROCEDURE [dbo].[Con_UpdateReplacingModelRules]
	@ModelId		INT,
	@ReplacingModel	INT

AS
BEGIN
	
	IF ISNULL(@ModelId,0) > 0 AND ISNULL(@ReplacingModel,0) > 0
		BEGIN
			INSERT INTO CRM_ADM_QueueRuleParams (QueueId,MakeId,ModelId,CityId,CreatedOn,UpdatedBy,UpdatedOn,SourceId,IsResearch,IsActive,DealerId)
			SELECT QueueId,MakeId,@ModelId,CityId,CreatedOn,UpdatedBy,UpdatedOn,SourceId,IsResearch,IsActive,DealerId
			FROM CRM_ADM_QueueRuleParams
			WHERE ModelId = @ReplacingModel
			
			INSERT INTO CRM_ADM_FLCRules (GroupId,MakeId,ModelId,CityId,SourceId,IsActive,Rank,UpdatedOn,UpdatedBy)
			SELECT GroupId,MakeId,@ModelId,CityId,SourceId,IsActive,Rank,UpdatedOn,UpdatedBy FROM CRM_ADM_FLCRules
			WHERE ModelId = @ReplacingModel

			INSERT INTO PQ_DealerCitiesModels (PqId,CityId,ZoneId,DealerId,ModelId,StateId,MakeId,CampaignId)
			SELECT PqId,CityId,ZoneId,DealerId,@ModelId,StateId,MakeId,CampaignId FROM PQ_DealerCitiesModels
			WHERE ModelId = @ReplacingModel


			UPDATE MM_SellerMobileMasking SET NCDBrandId = @ModelId WHERE NCDBrandId = @ReplacingModel
		END

END
