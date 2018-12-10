IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewCarPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertNewCarPrices]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertNewCarPrices]
	@Id			NUMERIC,	-- Id. Will be -1 if Its Insertion
	@CarVersionId		NUMERIC,	
	@DealerId		NUMERIC,
	@ExShowroom 		NUMERIC,
	@RTO			VARCHAR(10),
	@Insurance		VARCHAR(10),
	@OnRoad		VARCHAR(10),
	@Color			VARCHAR(30),
	@ExtendedWarranty1	VARCHAR(10),
	@ExtendedWarranty2 	VARCHAR(10),
	@ExtendedWarranty3 	VARCHAR(10),
	@Comments 		VARCHAR(2000),
	@LastUpdated		DATETIME

	
 AS	
	DECLARE @Average		NUMERIC
BEGIN
	
	IF @Id = -1 -- Insertion
		BEGIN
			INSERT INTO NewCarPrices
				(
					CarVersionId, 		DealerId, 		ExShowroom, 
					RTO, 			Insurance, 		OnRoad, 
					Color, 			ExtendedWarranty1, 	ExtendedWarranty2, 
					ExtendedWarranty3, 	Comments, 		LastUpdated
				)
			VALUES
				(
					@CarVersionId, 		@DealerId, 		@ExShowroom, 
					@RTO, 		@Insurance, 		@OnRoad, 
					@Color, 		@ExtendedWarranty1, 	@ExtendedWarranty2, 
					@ExtendedWarranty3, 	@Comments, 		@LastUpdated
				)
			
			-- Now fetch the Id for this Insertion.
			--SET @RecordId = SCOPE_IDENTITY()
		END
	ELSE -- Updation
		BEGIN
			UPDATE NewCarPrices SET
				ExShowroom = @ExShowroom, 			RTO = @RTO,
				Insurance = @Insurance, 			OnRoad = @OnRoad, 
				Color = @Color,	 				ExtendedWarranty1 = @ExtendedWarranty1, 	
				ExtendedWarranty2 = @ExtendedWarranty2, 	ExtendedWarranty3 = @ExtendedWarranty3, 	
				Comments = @Comments, 			LastUpdated = @LastUpdated
			WHERE
				ID = @Id
		END
	
	-- Now Update NewCarAveragePrices Table so that it may contain latest average.
	
	SELECT @Average=AVG(ExShowroom) FROM NewCarPrices WHERE CarVersionId=@CarVersionId
	DELETE FROM NewCarAveragePrices WHERE CarVersionId=@CarVersionId
	INSERT INTO NewCarAveragePrices 
			(CarVersionId, AveragePrice, LastUpdated)
	VALUES	(@CarVersionId, @Average, @LastUpdated)
END
