IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_DeletePQDealerRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_DeletePQDealerRules]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 16th Oct 2014
-- Description:	to delete and maintain the log of PQDealer Rules
-- =============================================
CREATE PROCEDURE [dbo].[Con_DeletePQDealerRules]
	@Ids VARCHAR(MAX),
	@DeletedBy INT
AS
BEGIN
	INSERT INTO PQ_DealerCitiesModelsLog(PQ_DealerCitiesModelsId,CampaignId,CityId,ZoneId,DealerId,ModelId,StateId,MakeId,DeletedBy,DeletedOn)
	SELECT Id,CampaignId,CityId,ZoneId,DealerId,ModelId,StateId,MakeId,@DeletedBy,GETDATE()
	FROM PQ_DealerCitiesModels
	WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@Ids))

	DELETE FROM PQ_DealerCitiesModels WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@Ids))
END
