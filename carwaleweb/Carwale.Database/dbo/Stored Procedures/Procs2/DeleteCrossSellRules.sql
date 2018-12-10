IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteCrossSellRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteCrossSellRules]
GO

	-- =============================================
-- Author:		Sourav Roy
-- Create date: 4th Nov2015
-- Description:	to delete CrossSell Rules
-- =============================================
CREATE PROCEDURE [dbo].[DeleteCrossSellRules]
	@Ids VARCHAR(MAX)
	
AS
BEGIN
	
	DELETE FROM PQ_CrossSellCampaignRules WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@Ids))
END

