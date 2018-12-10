IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPromotionalOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPromotionalOffers]
GO
	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertPromotionalOffers]
	
	@ConsumerType 	INT,
	@ConsumerId		NUMERIC,
	@PackageId		INT,
	@OfferCode		VARCHAR(50),
	@OfferValidity	DATETIME,
	@DiscountAmount	NUMERIC,
	@CreatedOn		DATETIME,
	@Status			INTEGER OUTPUT
 AS
	
BEGIN
	
	SET @Status = 0

	BEGIN
		INSERT INTO PromotionalOffers(ConsumerType, ConsumerId, PackageId, OfferCode, OfferValidity, DiscountAmount, CreatedOn) 
		VALUES(@ConsumerType,@ConsumerId,@PackageId,@OfferCode,@OfferValidity,@DiscountAmount,@CreatedOn)

		SET @Status = 1
			
	END	
END
