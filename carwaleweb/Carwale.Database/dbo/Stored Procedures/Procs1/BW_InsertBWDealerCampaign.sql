IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_InsertBWDealerCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_InsertBWDealerCampaign]
GO

	--=============================================
-- Created on	:	Sumit Kate on 18 Mar 2016
-- Description	:	Insert BW_PQ_DealerCampaign Table
-- Parameters	:
--	@DealerId					:	Dealer Id
--	@DealerName					:	Dealer Name
--	@Phone						:	Masking number
--	@DealerEmail				:	Dealer Email Id
--	@UpdatedBy					:	User Id
--	@IsActive					:	Is campaign active
--	@ContractId					:	Contract Id
--	@DealerLeadServingRadius	:	Lead serving radius
--	@NewCampaignId				:	Newly inserted campaignId
--	@IsBookingAvailable			:	Does campaign support booking
-- =============================================
CREATE PROCEDURE [dbo].[BW_InsertBWDealerCampaign] @DealerId INT
	,@DealerName VARCHAR(200)
	,@Phone VARCHAR(50)
	,@DealerEmail VARCHAR(250)
	,@UpdatedBy INT
	,@IsActive BIT
	,@ContractId INT
	,@DealerLeadServingRadius INT
	,@NewCampaignId INT OUTPUT
	,@IsBookingAvailable BIT = NULL
AS
BEGIN
	DECLARE @CampaignId INT
	DECLARE @DealerMobile VARCHAR(20)

	SELECT @DealerMobile = MobileNo FROM Dealers WITH(NOLOCK)
	WHERE Id = @DealerId
	
	INSERT INTO BW_PQ_DealerCampaigns (
		DealerId
		,ContractId
		,DealerName
		,Number
		,IsActive
		,DealerEmailId
		,DealerLeadServingRadius
		,IsBookingAvailable
		,EnteredBy
		)
	VALUES (
		@DealerId
		,@ContractId
		,@DealerName
		,@Phone
		,@IsActive
		,@DealerEmail
		,@DealerLeadServingRadius
		,@IsBookingAvailable
		,@UpdatedBy
		)

	SET @CampaignId = IDENT_CURRENT('BW_PQ_DealerCampaigns')
	SET @NewCampaignId = @CampaignId 
	IF (@IsActive = 1)
	BEGIN
		UPDATE TC_ContractCampaignMapping
		SET ContractStatus = 1
			,CampaignId = @CampaignId
		WHERE ContractId = @ContractId
			AND ApplicationId = 2
	END
	EXEC BW_SaveDealerMasking @DealerId,@Phone,@DealerMobile,@NewCampaignId,@UpdatedBy
END
