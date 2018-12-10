IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteCrossSellRules_v16_6_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteCrossSellRules_v16_6_1]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 18/05/2016
-- EXEC [DeleteCrossSellRules_16_5_1] '1,2,3'
-- =============================================
CREATE PROCEDURE [dbo].[DeleteCrossSellRules_v16_6_1] @RuleIds VARCHAR(MAX)
	,@UpdatedBy INT
AS
BEGIN
	DELETE
	FROM PQ_CrossSellCampaignRules
	WHERE ID IN (
			SELECT ListMember
			FROM fnSplitCSVMAx(@RuleIds)
			)
END

