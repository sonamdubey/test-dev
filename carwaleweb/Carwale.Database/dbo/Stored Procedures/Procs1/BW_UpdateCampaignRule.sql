IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateCampaignRule]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateCampaignRule]
GO

	

-- =============================================
-- Created on	:	Sumit Kate on 18 Mar 2016
-- Description	:	Update BW_PQ_CampaignRule Table
-- Parameters	:
--	@RuleId		:	Rule Id
--	@DealerId	:	Dealer Id
--	@CityId		:	city Id
--	@ModelId	:	Model Id
--	@StateId	:	State Id
--	@MakeId		:	Make Id
--	@UserId		:	User Id
--	@IsActive	:	Is Rule active
-- =============================================
CREATE PROCEDURE [dbo].[BW_UpdateCampaignRule] @RuleId INT
	,@CityId INT
	,@DealerId INT
	,@ModelId INT
	,@StateId INT
	,@MakeId INT
	,@UserID INT
	,@IsActive BIT
AS
BEGIN
	UPDATE BW_PQ_CampaignRules
	SET IsActive = @IsActive
		,CityId = @CityId
		,MakeId = @MakeId
		,ModelId = @ModelId
		,EnteredBy = @UserID
	WHERE ID = @RuleId
END
