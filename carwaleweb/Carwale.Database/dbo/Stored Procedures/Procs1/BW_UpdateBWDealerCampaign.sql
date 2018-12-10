IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateBWDealerCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateBWDealerCampaign]
GO

	-- =============================================
-- Created on	:	Sumit Kate on 18 Mar 2016
-- Description	:	Update the BW_PQ_DealerCampaign Table through operations
-- Parameters	:
--	@CampaignId					:	Campaign Id
--	@DealerId					:	Dealer Id
--	@DealerName					:	Dealer Name
--	@Phone						:	Masking number
--	@DealerEmail				:	Dealer Email Id
--	@UpdatedBy					:	User Id
--	@IsActive					:	Is campaign active
--	@ContractId					:	Contract Id
--	@DealerLeadServingRadius	:	Lead serving radius
--	@IsBookingAvailable			:	Does campaign support booking
-- =============================================	
CREATE PROCEDURE [dbo].[BW_UpdateBWDealerCampaign] @CampaignId INT
	,@DealerId INT
	,@DealerName VARCHAR(200)
	,@Phone VARCHAR(50)
	,@DealerEmail VARCHAR(250)
	,@DealerLeadServingRadius INT
	,@IsBookingAvailable BIT
	,@IsActive BIT
	,@UpdatedBy INT
	,@ContractId INT
AS
BEGIN
	DECLARE @DealerMobile VARCHAR(20)
	DECLARE @oldMaskingNumber VARCHAR(50)	
	SELECT @DealerMobile = MobileNo FROM Dealers WITH(NOLOCK)
	WHERE Id = @DealerId
	
	SELECT @oldMaskingNumber = Number FROM BW_PQ_DealerCampaigns WITH(NOLOCK)
	WHERE Id = @CampaignId

	UPDATE BW_PQ_DealerCampaigns
	SET DealerName = @DealerName
		,DealerId = @DealerId
		,Number = @Phone
		,DealerEmailId = @DealerEmail
		,DealerLeadServingRadius = @DealerLeadServingRadius
		,IsBookingAvailable = @IsBookingAvailable
		,IsActive = @IsActive
		,UpdatedBy = @UpdatedBy
		,UpdatedOn = GETDATE()
	WHERE Id = @CampaignId

	IF (@IsActive = 1)
	BEGIN
		UPDATE TC_ContractCampaignMapping
		SET ContractStatus = 1
		WHERE CampaignId = @CampaignId
			AND ApplicationId = 2
	END

	EXEC BW_SaveDealerMasking @DealerId,@Phone,@DealerMobile,@CampaignId,@UpdatedBy,3,1,NULL, @oldMaskingNumber

	
END
