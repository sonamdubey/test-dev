IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DO_InsertDealerCityforOffer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DO_InsertDealerCityforOffer]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 24th Nov 2014
-- Description:	to add cities and zones against offer
-- =============================================
CREATE PROCEDURE [dbo].[DO_InsertDealerCityforOffer] 
	@OfferId	INT,
	@OfferType	INT,
	@DealerId	VARCHAR(250),
	@CityId		INT,
	@ZoneId		INT,
	@EnteredBy	INT,
	@Type		INT,
	@Status		INT OUTPUT
AS
BEGIN
	DECLARE @ExistingCity INT
	SELECT @ExistingCity = CityId FROM DealerOffersDealers WHERE OfferId  = @OfferId
		
	IF @@ROWCOUNT = 0 OR (@ExistingCity != -1 AND ISNULL(@CityId,0) != -1)
	BEGIN				
		IF @OfferType = 1 
		BEGIN
			IF @ZoneId IS NOT NULL
			BEGIN
				IF @ZoneId = 645--THANE WITH OCTROI
				BEGIN
					SET @CityId = 645
					SET @ZoneId = NULL
				END
				ELSE
				BEGIN
					SELECT @CityId = CityId FROM CityZones WHERE Id = @ZoneId
				END
				
				IF @ZoneId IS NOT NULL
					SELECT * FROM DealerOffersDealers WHERE OfferId = @OfferId AND CityId = @CityId AND ZoneId = @ZoneId
				ELSE
					SELECT * FROM DealerOffersDealers WHERE OfferId = @OfferId AND CityId = 645 AND ZoneId IS NULL
				
				IF @@ROWCOUNT <= 0 
				BEGIN
					IF @Type = 1
						INSERT INTO DealerOffersDealers(OfferId,DealerId,CityId,ZoneId)
						SELECT DISTINCT @OfferId,DealerId,@CityId,@ZoneId FROM DealerOffersDealers WHERE OfferId = @OfferId
					IF @Type = 2
					BEGIN
						INSERT INTO DealerOffersDealers(OfferId,DealerId,CityId,ZoneId)
						SELECT @OfferId,ListMember,@CityId,@ZoneId FROM fnSplitCSV(@DealerId)
						UPDATE DealerOffersDealers SET DealerId = CAST(@DealerId AS INT) WHERE OfferId = @OfferId
					END
					SET @Status = 1
				END
				ELSE
				BEGIN
					IF @Type = 2
					BEGIN
						UPDATE DealerOffersDealers SET DealerId = CAST(@DealerId AS INT) WHERE OfferId = @OfferId
						SET @Status = 1
					END
					ELSE
						SET @Status = 0
				END
			END
			ELSE
			BEGIN
				IF @CityId IS NOT NULL
					BEGIN
						SELECT * FROM DealerOffersDealers WHERE OfferId = @OfferId AND CityId = @CityId AND ZoneId IS NULL
						IF @@ROWCOUNT <= 0 
						BEGIN
							IF @Type = 1
								INSERT INTO DealerOffersDealers(OfferId,DealerId,CityId,ZoneId)
								SELECT DISTINCT @OfferId,DealerId,@CityId,@ZoneId FROM DealerOffersDealers WHERE OfferId = @OfferId
							IF @Type = 2
							BEGIN
								INSERT INTO DealerOffersDealers(OfferId,DealerId,CityId,ZoneId)
								SELECT @OfferId,ListMember,@CityId,@ZoneId FROM fnSplitCSV(@DealerId)
								UPDATE DealerOffersDealers SET DealerId = CAST(@DealerId AS INT) WHERE OfferId = @OfferId
							END
							SET @Status = 1
					END
				END
				ELSE
				BEGIN
					IF @Type = 2
					BEGIN
						UPDATE DealerOffersDealers SET DealerId = CAST(@DealerId AS INT) WHERE OfferId = @OfferId
						SET @Status = 1
					END
					ELSE
						SET @Status = 0
				END
			END
		END
		ELSE
		BEGIN
			DELETE FROM DealerOffersDealers WHERE OfferId = @OfferId
			
			IF NOT EXISTS (SELECT * FROM DealerOffersDealers WHERE OfferId = @OfferId AND CityId = @CityId)
				INSERT INTO DealerOffersDealers(OfferId,DealerId,CityId,ZoneId)
				SELECT @OfferId,ListMember,@CityId,@ZoneId FROM fnSplitCSV(@DealerId)
			
			SET @Status = 1
		END
	END
	ELSE
		SET @Status = 0
	
	INSERT INTO DealerOffersDealersLog(OfferId,DealerId,CityId,ZoneId,EnteredBy,EntryDate)
	SELECT @OfferId,ListMember,@CityId,@ZoneId,@EnteredBy,GETDATE() FROM fnSplitCSV(@DealerId)
END