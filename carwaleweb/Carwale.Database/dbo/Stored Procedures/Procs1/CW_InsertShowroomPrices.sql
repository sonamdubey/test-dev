IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_InsertShowroomPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_InsertShowroomPrices]
GO

	/* EXEC CW_InsertShowroomPrices 2914,10,GETDATE(),1,'400' */
-- Created By : Raghu on 15/10/2013
-- Modified By : Raghu on 27/11/2013 (Added a condition to check whether price>0 before inserting into newcarshowroomprices)
-- Modified by Reshma Shetty 12/12/2013 commented a condition to avoid cases where the price of the version whose min or max price is present in the carmodels table is itself changed.
-- Modified by Manish on 24-04-2014 passed city id parameter in UpdateModelPrices sp
-- Modified by Sanjay on 10-09-2015 Added if block IsMultiplePriceExist
-- Modifed by Jitendra Solanki  28/04/2015 Checking whether price changed or not
-- Modified by Shalini Nair on 07/06/2016 to log in PriceLog_ItemValues everytime
-- Modified by Sanjay Soni on 20/07/2016 to update national avg price insert or updating prices
-- Modified: Shalini Nair, 26/07/2016 to pass SetId as varchar
CREATE PROCEDURE [dbo].[CW_InsertShowroomPrices] @CarVersionId NUMERIC
	,-- Car Version Id
	@CityId NUMERIC
	,-- 
	@LastUpdated DATETIME
	,-- Entry Date
	@PQ_CategoryItemId INT
	,-- Enter the CategoryId of LocalTax,insurance,dealercharges from PQ_CategoriesItems
	@PQ_CategoryItemvalue NUMERIC(18, 0)
	,-- Enter value of particular item
	@CategoryId INT
	,@isMetallic BIT
	,@UpdatedBy INT
	,@SetID VARCHAR(50)
AS
DECLARE @Id NUMERIC
	,@OldItemValue NUMERIC
	,@CarModelId NUMERIC
	,@MakeId NUMERIC
	,@Price NUMERIC(18, 0)
	,@RTO NUMERIC(18, 0)
	,@Insurance NUMERIC(18, 0)
	,@CorporateRTO NUMERIC(18, 0)
	,@OldPrice NUMERIC(18, 0)
	,@OldRTO NUMERIC(18, 0)
	,@OldInsurance NUMERIC(18, 0)
	,@OldRTOCorporate NUMERIC
	,@CategoryId_Price SMALLINT = 3
	,@CategoryId_RTO SMALLINT = 4
	,@CategoryId_Insurance SMALLINT = 5
	,@CategoryId_optional SMALLINT = 7
	,@ItemId_ExShowroomPrice SMALLINT = 2
	,@ItemId_RTO SMALLINT = 3
	,@ItemId_CorporateRTO SMALLINT = 4
	,@ItemId_Insurance SMALLINT = 5

BEGIN
	SELECT @CarModelId = CarModelId
	FROM CarVersions WITH (NOLOCK)
	WHERE ID = @CarVersionId

	DECLARE @RecordExist INT
	DECLARE @existingColorInd BIT

	SELECT @Id = Id
		,@OldPrice = ISNULL(Price, 0)
		,@OldRTO = ISNULL(RTO, 0)
		,@OldRTOCorporate = ISNULL(CorporateRTO, 0)
		,@OldInsurance = ISNULL(Insurance, 0)
		,@existingColorInd = isMetallic
	FROM NewCarShowroomPrices WITH (NOLOCK)
	WHERE CarVersionId = @CarVersionId
		AND CityId = @CityId

	SET @RecordExist = @@ROWCOUNT

	IF @RecordExist <= 0
	BEGIN
		SET @Price = CASE 
				WHEN @CategoryId = @CategoryId_Price
					THEN @PQ_CategoryItemValue
				ELSE @OldPrice
				END
		SET @RTO = CASE 
				WHEN @CategoryId = @CategoryId_RTO
					AND @PQ_CategoryItemId = @ItemId_RTO
					THEN @PQ_CategoryItemValue
				ELSE 0
				END
		SET @Insurance = CASE 
				WHEN @CategoryId = @CategoryId_Insurance
					THEN @PQ_CategoryItemValue
				ELSE 0
				END
		SET @CorporateRTO = CASE 
				WHEN @CategoryId = @CategoryId_RTO
					AND @PQ_CategoryItemId = @ItemId_CorporateRTO
					THEN @PQ_CategoryItemvalue
				ELSE NULL
				END

		IF @Price > 0 -- Added by Raghu to check condition
		BEGIN
			INSERT INTO NewCarShowroomPrices (
				CarVersionId
				,CityId
				,Price
				,RTO
				,Insurance
				,CorporateRTO
				,LastUpdated
				,IsActive
				,CarModelId
				,isMetallic --Modified by Reshma Shetty 08/07/2013  Add CarModelId on insertion of price for a new car
				)
			VALUES (
				@CarVersionId
				,@CityId
				,@Price
				,@RTO
				,@Insurance
				,@CorporateRTO
				,@LastUpdated
				,1
				,@CarModelId
				,@isMetallic
				)
		END
	END
	ELSE
	BEGIN
		INSERT INTO PriceLog (
			VersionId
			,CityId
			,Price
			,RTO
			,Insurance
			,UpdatedOn
			,UpdatedBy
			)
		VALUES (
			@CarVersionId
			,@CityId
			,@OldPrice
			,@OldRTO
			,@OldInsurance
			,@LastUpdated
			,@UpdatedBy
			)

		SET @Price = CASE 
				WHEN @CategoryId = @CategoryId_Price
					THEN @PQ_CategoryItemValue
				ELSE @OldPrice
				END
		SET @RTO = CASE 
				WHEN @CategoryId = @CategoryId_RTO
					AND @PQ_CategoryItemId = @ItemId_RTO
					THEN @PQ_CategoryItemValue
				ELSE @OldRTO
				END
		SET @CorporateRTO = CASE 
				WHEN @CategoryId = @CategoryId_RTO
					AND @PQ_CategoryItemId = @ItemId_CorporateRTO
					THEN @PQ_CategoryItemValue
				ELSE @OldRTOCorporate
				END
		SET @Insurance = CASE 
				WHEN @CategoryId = @CategoryId_Insurance
					THEN @PQ_CategoryItemvalue
				ELSE @OldInsurance
				END

		IF NOT (
				@existingColorInd = 0
				AND @isMetallic = 1
				)
		BEGIN
			UPDATE NewCarShowroomPrices
			SET CarVersionId = @CarVersionId
				,Price = @Price
				,Insurance = @Insurance
				,RTO = @RTO
				,CorporateRTO = @CorporateRTO
				,LastUpdated = @LastUpdated
				,isMetallic = @isMetallic
			WHERE ID = @Id
		END
	END

	SELECT @Id = PQN.Id
		,@OldItemvalue = ISNULL(PQ_CategoryItemValue, 0)
	FROM CW_NewCarShowroomPrices PQN WITH (NOLOCK)
	INNER JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = PQN.PQ_CategoryItem -- AND CI.CategoryId = @CategoryId
	WHERE CarVersionId = @CarVersionId
		AND CityId = @CityId
		AND PQN.PQ_CategoryItem = @PQ_CategoryItemId
		AND isMetallic = @isMetallic

	SET @RecordExist = @@ROWCOUNT

	DECLARE @OnRoadPriceInd BIT

	SET @OnRoadPriceInd = CASE 
			WHEN @CategoryId_optional = @CategoryId
				THEN 0
			ELSE 1
			END

	IF @PQ_CategoryItemvalue > 0
	BEGIN
		IF @RecordExist <= 0 -- Insertion
		BEGIN
			--Added by Raghu for new Enchanment Changes in PriceQuote
			DECLARE @IsMultiplePriceExist BIT

			SELECT @CategoryId = c.CategoryId
				,@IsMultiplePriceExist = C.IsMultiplePriceExist
			FROM PQ_CATEGORY AS C WITH (NOLOCK)
			JOIN PQ_CategoryItems AS SC WITH (NOLOCK) ON C.CategoryId = SC.CategoryId
			WHERE SC.ID = @PQ_CategoryItemId

			-- Added if block IsMultiplePriceExist by Sanjay on 10-09-2015
			IF @IsMultiplePriceExist = 0
			BEGIN
				DELETE
				FROM CW_NewCarShowroomPrices
				WHERE CityId = @CityId
					AND CarVersionId = @CarVersionId
					AND isMetallic = @isMetallic
					AND PQ_CategoryItem IN (
						SELECT ID
						FROM PQ_CategoryItems WITH (NOLOCK)
						WHERE CategoryId = @CategoryId
						)
			END

			INSERT INTO CW_NewCarShowroomPrices (
				CarVersionId
				,CityId
				,PQ_CategoryItem
				,PQ_CategoryItemValue
				,OnRoadPriceInd
				,isMetallic
				,LastUpdated
				)
			VALUES (
				@CarVersionId
				,@CityId
				,@PQ_CategoryItemId
				,@PQ_CategoryItemValue
				,@OnRoadPriceInd
				,@isMetallic
				,@LastUpdated
				)
		END
		ELSE -- Updation
		BEGIN
			DECLARE @IsModifiedPrice BIT

			SET @IsModifiedPrice = CASE 
					WHEN @PQ_CategoryItemvalue = @OldItemValue
						THEN 0
					ELSE 1
					END

				INSERT INTO PriceLog_ItemValues (
					VersionId
					,CityId
					,PQ_CategoryItem
					,PQ_CategoryItemValue
					,IsModified
					,SetID
					,IsMetallic
					,UpdatedBy
					,UpdatedOn
					)
				VALUES (
					@CarVersionId
					,@CityId
					,@PQ_CategoryItemId
					,@OldItemValue
					,@IsModifiedPrice
					,@SetID
					,@isMetallic
					,@UpdatedBy
					,@LastUpdated
					)

			UPDATE CW_NewCarShowroomPrices
			SET CarVersionId = @CarVersionId
				,LastUpdated = @LastUpdated
				,PQ_CategoryItem = @PQ_CategoryItemId
				,PQ_CategoryItemValue = @PQ_CategoryItemValue
			WHERE ID = @Id
		END
	END
	ELSE
	BEGIN
		IF @RecordExist > 0
		BEGIN
			DELETE
			FROM CW_NewCarShowroomPrices
			WHERE ID = @Id
		END
	END

	-- Update Min & Max Price of Model to CarModels table
	-- logic to pass this store procedure for the all city by jitendra on 07-10-2015
	--IF(@CityId=10 
	--Commented by Reshma Shetty 12/12/2013 to avoid cases where the price of the version whose min or max price is present in the carmodels table is itself changed.
	--AND NOT EXISTS(SELECT CMO.ID
	--			FROM CarModels CMO WITH(NOLOCK) -- Added by Raghu on 27/11/2013 add  with(nolock)
	--			WHERE CMO.ID = @CarModelId
	--				AND @Price BETWEEN MinPrice AND MaxPrice
	--			)
	--			)--Check whether city is New Delhi and newly added price is not in between the already existing price 
	EXEC UpdateModelPrices @CarVersionId
		,@CityId ---add City id parameter by Manish on 24-04-204

	EXEC [dbo].[Con_SaveNewCarNationalPrices] @CarVersionId,@UpdatedBy,@LastUpdated
END
