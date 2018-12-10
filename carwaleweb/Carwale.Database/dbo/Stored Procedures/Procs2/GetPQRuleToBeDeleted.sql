IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQRuleToBeDeleted]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQRuleToBeDeleted]
GO

	
-- =============================================
-- Author:		Chetan Thambad
-- Create date: 14-09-2016
-- exec GetPQRuleToBeDeleted 58662,58663,58664
-- =============================================
create PROCEDURE [dbo].[GetPQRuleToBeDeleted] @Ids VARCHAR(MAX)
AS
BEGIN
	SELECT CampaignId
		,CityId
		,DealerId
		,ModelId
		,StateId
		,MakeId
	FROM PQ_DealerCitiesModels WITH (NOLOCK)
	WHERE ID IN (
			SELECT ListMember
			FROM fnSplitCSV(@Ids)
			)
END

