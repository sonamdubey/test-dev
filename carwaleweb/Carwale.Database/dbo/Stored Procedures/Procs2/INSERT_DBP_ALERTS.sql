IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[INSERT_DBP_ALERTS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[INSERT_DBP_ALERTS]
GO

	CREATE PROCEDURE [dbo].[INSERT_DBP_ALERTS] 
@DealerId	NUMERIC(18, 0), 
@CityIds	VARCHAR(500), 
@MakeId		NUMERIC(18, 0), 
@ModelIds	VARCHAR(500), 
@MinPrice	NUMERIC(18, 2), 
@MaxPrice	NUMERIC(18, 2), 
@MinKm		NUMERIC(18, 0), 
@MaxKm		NUMERIC(18, 0), 
@MinYear	NUMERIC(18, 0), 
@MaxYear	NUMERIC(18, 0), 
@NoOfOwners INT,
@RegNo		VARCHAR(4),
@Result		TINYINT OUTPUT
AS
BEGIN

	SET @Result = 0
	DECLARE @CityRowCount INT = 0
	DECLARE @ModelRowCount INT = 0
	DECLARE @CityId		INT = 0
	DECLARE	@ModelId	INT = 0

	-- Create a table variable to store cityId
	DECLARE @cityTable TABLE
	(
		ID			INT		IDENTITY(1,1),
		CityId		INT
	)

	INSERT INTO @cityTable (CityId)
				SELECT ListMember AS CityId FROM fnSplitCSV(@CityIds)
	
	SELECT @CityRowCount = COUNT(*) FROM @cityTable
	
	IF @MakeId != -1 AND @MakeId != 0
	BEGIN
		
		-- Create a table variable to store modelId
		DECLARE @modelTable TABLE
		(
			ID			INT		IDENTITY(1,1),
			ModelId		INT
		)

		INSERT INTO @modelTable (ModelId)
					SELECT ListMember AS ModelId FROM fnSplitCSV(@ModelIds)

		SELECT @ModelRowCount = COUNT(*) FROM @modelTable

		-- Declare an iterator
		DECLARE @I INT
		DECLARE @J INT
		
		-- Initialize the iterator
		SET @I = 1

		-- Loop through the rows of a table @cityTable
		WHILE (@I <= @CityRowCount)
			BEGIN
				SELECT @CityId = CityID FROM @cityTable WHERE ID = @I

				SET @J = 1
				WHILE (@J <= @ModelRowCount)
					BEGIN
					
						SELECT @ModelId = ModelId FROM @modelTable WHERE ID = @J

						INSERT INTO DBP_Alerts
						(DealerId, CityId, MakeId, ModelId, MinPrice, MaxPrice, MinKm, MaxKm, MinYear, MaxYear, NoOfOwners, RegNo)
						VALUES
						(@DealerId, @CityId, @MakeId, @ModelId, @MinPrice, @MaxPrice, @MinKm, @MaxKm, @MinYear, @MaxYear, @NoOfOwners, @RegNo)

						SET @J = @J + 1
						SET @Result = 1
					END
				SET @I = @I + 1
			END
	END
	ELSE IF @MakeId = -1
	BEGIN
		-- Declare an iterator
		DECLARE @C INT

		SET @C = 1
		-- Loop through the rows of a table @cityTable
		WHILE (@C <= @CityRowCount)
			BEGIN
				SELECT @CityId = CityID FROM @cityTable WHERE ID = @C
		
				INSERT INTO DBP_Alerts
				(DealerId, CityId, MakeId, ModelId, MinPrice, MaxPrice, MinKm, MaxKm, MinYear, MaxYear, NoOfOwners, RegNo)
				VALUES
				(@DealerId, @CityId, -1, -1, @MinPrice, @MaxPrice, @MinKm, @MaxKm, @MinYear, @MaxYear, @NoOfOwners, @RegNo)

				SET @Result = 1
				SET @C += 1
			END
	END
	
END
