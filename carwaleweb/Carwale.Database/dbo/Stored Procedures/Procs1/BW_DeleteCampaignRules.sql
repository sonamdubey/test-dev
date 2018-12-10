IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_DeleteCampaignRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_DeleteCampaignRules]
GO

	

-- =============================================
-- Created on	:	Sumit Kate on 18 Mar 2016
-- Description	:	Delete BW_PQ_CampaignRule Table
-- Parameters	:
--	@RuleIds	:	Comma Separated Rule Ids
--	@UserId		:	User Id
-- =============================================
CREATE PROCEDURE [dbo].[BW_DeleteCampaignRules] @UserId INT
	,@RuleIds VARCHAR(MAX)
AS
BEGIN
	UPDATE Rules
	SET IsActive = 0
		,UpdatedBy = @UserId
		,UpdatedOn = GETDATE()
	FROM BW_PQ_CampaignRules Rules
	INNER JOIN fnSplitCSV(@RuleIds) val ON Rules.Id = val.ListMember
END
