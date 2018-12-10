IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPaidCrossSellRuleToBeDeleted]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPaidCrossSellRuleToBeDeleted]
GO

	
-- =============================================
-- Author:		Sanjay Soni
-- Create date: 15-09-2016
-- =============================================
create PROCEDURE [dbo].[GetPaidCrossSellRuleToBeDeleted] @Ids VARCHAR(1000)
AS
BEGIN
	SELECT TargetVersion As VersionId
		,CityId
		,StateId
	FROM PQ_CrossSellCampaignRules WITH (NOLOCK)
	WHERE ID IN (
			SELECT ListMember
			FROM fnSplitCSV(@Ids)
			)
END
