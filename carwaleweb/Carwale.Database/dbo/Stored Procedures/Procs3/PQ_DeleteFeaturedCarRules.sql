IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_DeleteFeaturedCarRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_DeleteFeaturedCarRules]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 27th Jan 2015
-- Description: To delete and maintain the log of featured car rules 
-- =============================================
CREATE PROCEDURE [dbo].[PQ_DeleteFeaturedCarRules] 
	@Ids VARCHAR(MAX),
	@DeletedBy INT
AS
BEGIN
	INSERT INTO PQ_FeaturedCampaignRulesLog(PQ_FeaturedCampaignRulesId,FeaturedCampaignId,StateId,CityId,TargetVersion,FeaturedVersion,DeletedBy,DeletedOn)
	SELECT Id,FeaturedCampaignId,StateId,CityId,TargetVersion,FeaturedVersion,@DeletedBy,GETDATE() 
	FROM PQ_FeaturedCampaignRules 
	WHERE Id IN (SELECT ListMember FROM fnSplitCSV(@Ids))

	DELETE FROM PQ_FeaturedCampaignRules WHERE Id IN (SELECT ListMember FROM fnSplitCSV(@Ids))
END
