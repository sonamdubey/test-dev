IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_SaveDealerMasking]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_SaveDealerMasking]
GO

	

-- =============================================
-- Author		:	Sumit Kate
-- Create date	:	04 Apr 2016
-- Description	:	To Save Dealer Masking
-- Parameters	:	
--		@ConsumerId		:	Dealer Id
--		@MaskingNumber	:	Masking Number
--		@Mobile			:	Dealers Mobile Number
--		@ProductTypeId	:	Value from MM_ProductTypes
--		@DealerType		:	Value from TC_DealerType
--		@NCDBrandId		:	Bike Model Id
--		@LastUpdatedBy	:	user id
--		@LeadCampaignId	:	Campaign Id
-- =============================================
CREATE PROCEDURE [dbo].[BW_SaveDealerMasking] 	
	@ConsumerId INT
	,@MaskingNumber VARCHAR(20)	
	,@Mobile VARCHAR(20)
	,@LeadCampaignId INT
	,@LastUpdatedBy INT
	,@ProductTypeId INT = 3
	,@DealerType INT = 1	
	,@NCDBrandId INT = NULL
	,@OldMaskingNumber VARCHAR(20) = NULL
AS
BEGIN	
	SELECT 
		TOP 1 @NCDBrandId = BMO.TopVersionId
	FROM 
	TC_DealerMakes DM WITH(NOLOCK)
	INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = DM.DealerId AND D.IsDealerActive = 1 AND D.IsDealerDeleted = 0 AND ApplicationId = 2
	INNER JOIN BikeModels BMO WITH(NOLOCK) ON BMO.BikeMakeId = DM.MakeId AND BMO.TopVersionId IS NOT NULL

	

	IF NOT EXISTS (SELECT MM_SellerMobileMaskingId
		FROM MM_SellerMobileMasking WITH (NOLOCK) WHERE LeadCampaignId = @LeadCampaignId AND ApplicationId = 2)
	BEGIN		
		INSERT INTO MM_SellerMobileMasking (
				ConsumerId
				,ConsumerType
				,MaskingNumber
				,Mobile
				,DealerType
				,CreatedOn
				,ProductTypeId
				,NCDBrandId
				,LastUpdatedOn
				,LastUpdatedBy
				,LeadCampaignId
				,ApplicationId
				)
			VALUES (
				@ConsumerId
				,1
				,@MaskingNumber
				,@Mobile
				,@DealerType
				,GETDATE()
				,@ProductTypeId
				,@NCDBrandId
				,GETDATE()
				,@LastUpdatedBy
				,@LeadCampaignId
				,2 -- Bikewale
				)		
	END
	ELSE
	BEGIN						
		UPDATE MM_SellerMobileMasking
		SET Mobile = @Mobile
			,MaskingNumber = @MaskingNumber
			,DealerType = @DealerType
			,NCDBrandId = @NCDBrandId
			,ProductTypeId = @ProductTypeId
			,LastUpdatedOn = GETDATE()
			,LastUpdatedBy = @LastUpdatedBy
		WHERE LeadCampaignId = @LeadCampaignId AND ApplicationId = 2
	END

	DELETE
		FROM MM_AvailableNumbers
		WHERE MaskingNumber = @MaskingNumber
	
	IF NOT EXISTS (SELECT 1 FROM MM_AvailableNumbers WITH(NOLOCK) WHERE @OldMaskingNumber IS NOT NULL AND MaskingNumber = @OldMaskingNumber)
	BEGIN			
		IF @OldMaskingNumber IS NOT NULL
			INSERT INTO MM_AvailableNumbers(MaskingNumber,CityId)
			SELECT @oldMaskingNumber,CityId
			FROM Dealers WITH(NOLOCK) WHERE Id = @ConsumerId
	END

	IF @ProductTypeId = 3
	BEGIN
		UPDATE Dealers
		SET ActiveMaskingNumber = @MaskingNumber
		WHERE ID = @ConsumerId AND ApplicationId = 2
	END
END
