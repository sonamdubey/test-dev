IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPrices]
GO
-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <05/08/2016>
-- Description:	<Checks if input categoryitems are already present in the table for the version and then inserts/updates the prices>
-- =============================================
CREATE PROCEDURE [dbo].[InsertPrices]
	-- Add the parameters for the stored procedure here
	@VersionId INT
	,@CityId INT
	,@IsMetallic INT
	,@PricesKeyValuePairs VARCHAR(MAX)
	,@LastUpdated DATETIME
	,@UpdatedBy INT
AS
DECLARE @Id INT,
		@OldPrice INT,
		@OldRTO INT,
		@OldRTOCorporate INT,
		@OldInsurance INT,
		@ExistingColorInd BIT,
		@RecordExist INT,
		@ModelId INT,
		@Price INT,
		@RTO INT,
		@CorporateRTO INT,
		@Insurance INT
BEGIN
	IF EXISTS (SELECT 1 FROM CarVersions WITH (NOLOCK) WHERE ID = @VersionId)
	BEGIN
		CREATE TABLE #TempCharges (Item INT, Value INT)
		CREATE TABLE #TempPrices (CategoryId INT, PQ_CategoryItem INT, PQ_CategoryItemValue BIGINT, IsPresent BIT)

		INSERT INTO #TempCharges (Item, Value)
		SELECT CP.Val1 AS Item, CP.Val2 AS Value
		FROM dbo.SplitTextByTwoDelimiters(@PricesKeyValuePairs,',','-') AS CP

		INSERT INTO #TempPrices (CategoryId, PQ_CategoryItem, PQ_CategoryItemValue, IsPresent)
		SELECT CI.CategoryId, TC.Item AS PQ_CategoryItem, TC.Value AS PQ_CategoryItemValue, CASE WHEN NCP.Id IS NULL THEN 0 ELSE 1 END AS IsPresent
		FROM #TempCharges AS TC
			 LEFT OUTER JOIN CW_NewCarShowroomPrices NCP WITH (NOLOCK) ON NCP.CarVersionId = @VersionId AND NCP.CityId = @CityId AND NCP.isMetallic = @IsMetallic AND NCP.PQ_CategoryItem = TC.Item
			 JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = TC.Item

		IF EXISTS (SELECT 1 FROM #TempPrices WITH (NOLOCK) WHERE IsPresent = 0)
		BEGIN
			INSERT INTO CW_NewCarShowroomPrices (CarVersionId, CityId, PQ_CategoryItem, PQ_CategoryItemValue, OnRoadPriceInd, isMetallic, LastUpdated, UpdatedBy)
			SELECT @VersionId AS CarVersionId, @CityId, TP.PQ_CategoryItem, TP.PQ_CategoryItemValue, CASE 
																									 WHEN TP.CategoryId = 7 -- Optional Charge CategoryId = 7
																									 THEN 0
																									 ELSE 1
																									 END AS OnRoadPriceInd, @IsMetallic AS isMetallic, @LastUpdated, @UpdatedBy
			FROM #TempPrices AS TP WITH (NOLOCK)
			WHERE TP.IsPresent = 0
		END

		IF EXISTS (SELECT 1 FROM #TempPrices WITH (NOLOCK) WHERE IsPresent = 1)
		BEGIN
			INSERT INTO PricesLog (VersionId, CityId, IsMetallic, PQ_CategoryItemId, PQ_CategoryItemValue, UpdatedOn, UpdatedBy, Status)
			SELECT @VersionId, @CityId, @IsMetallic, NCP.PQ_CategoryItem, NCP.PQ_CategoryItemValue, LastUpdated, UpdatedBy, 'U'
			FROM CW_NewCarShowroomPrices NCP WITH (NOLOCK)
				INNER JOIN #TempPrices TP WITH (NOLOCK) ON NCP.PQ_CategoryItem = TP.PQ_CategoryItem AND TP.IsPresent = 1
																					  AND CarVersionId = @VersionId
																					  AND CityId = @CityId
																					  AND isMetallic = @IsMetallic

			UPDATE NCP
			SET NCP.PQ_CategoryItemValue = TP.PQ_CategoryItemValue
				,NCP.LastUpdated = @LastUpdated
			FROM CW_NewCarShowroomPrices NCP WITH (NOLOCK)
				JOIN #TempPrices TP WITH (NOLOCK) ON NCP.PQ_CategoryItem = TP.PQ_CategoryItem
													AND CarVersionId = @VersionId
													AND CityId = @CityId
													AND isMetallic = @IsMetallic
			WHERE TP.IsPresent = 1
		END
			
		DROP TABLE #TempPrices

		IF EXISTS (SELECT TOP 1 1 FROM VersionPricesUpdationLog WITH (NOLOCK) WHERE VersionId = @VersionId AND CityId = @CityId AND IsMetallic = @IsMetallic)
		BEGIN
			UPDATE VersionPricesUpdationLog
			SET LastUpdated = @LastUpdated,
				LastUpdatedBy = @UpdatedBy
			WHERE VersionId = @VersionId
				  AND CityId = @CityId
				  AND IsMetallic = @IsMetallic
		END
		ELSE
		BEGIN
			INSERT INTO VersionPricesUpdationLog (VersionId, CityId, IsMetallic, LastUpdated, LastUpdatedBy)
			VALUES (@VersionId, @CityId, @IsMetallic, @LastUpdated, @UpdatedBy)
		END

		--------------------------------------------------------------------------------------------------------------------------------------------------------

		CREATE TABLE #TempShowroomPrices (Price INT, RTO INT, CorporateRTO INT, Insurance INT)

		INSERT INTO #TempShowroomPrices (Price, RTO, CorporateRTO, Insurance)
		SELECT CASE WHEN TC.Item = 2 THEN TC.Value ELSE 0 END AS Price,
			   CASE WHEN TC.Item = 3 THEN TC.Value ELSE 0 END AS RTO,
			   CASE WHEN TC.Item = 4 THEN TC.Value ELSE 0 END AS CorporateRTO,
			   CASE WHEN TC.Item = 5 THEN TC.Value ELSE 0 END AS Insurance
		FROM #TempCharges AS TC
		WHERE TC.Item IN (2,3,4,5)

		SET @ModelId = (SELECT CarModelId FROM CarVersions WITH (NOLOCK) WHERE ID = @VersionId)

		SET @Price = (SELECT Price FROM #TempShowroomPrices WITH (NOLOCK) WHERE Price != 0)
		SET @RTO = (SELECT RTO FROM #TempShowroomPrices WITH (NOLOCK) WHERE RTO != 0)
		SET @CorporateRTO = (SELECT CorporateRTO FROM #TempShowroomPrices WITH (NOLOCK) WHERE RTO != 0)
		SET @Insurance = (SELECT Insurance FROM #TempShowroomPrices WITH (NOLOCK) WHERE Insurance != 0)

		SET @ExistingColorInd = (SELECT TOP 1 isMetallic FROM NewCarShowroomPrices WITH (NOLOCK) WHERE CarVersionId = @VersionId AND CityId = @CityId)

		IF (@ExistingColorInd IS NULL)
		BEGIN
			INSERT INTO NewCarShowroomPrices (CarVersionId, CityId, Price, Insurance, RTO, CorporateRTO, LastUpdated, IsActive, CarModelId, isMetallic)
			VALUES (@VersionId, @CityId, @Price, @Insurance, @RTO, @CorporateRTO, @LastUpdated, 1, @ModelId, @IsMetallic)
		END
		ELSE IF (NOT (@ExistingColorInd = 0 AND @IsMetallic = 1))
		BEGIN
			IF (@Price IS NOT NULL)
			BEGIN
				UPDATE NewCarShowroomPrices
				SET Price = @Price, isMetallic = @IsMetallic
				WHERE CarVersionId = @VersionId AND CityId = @CityId
			END
			IF (@RTO IS NOT NULL)
			BEGIN
				UPDATE NewCarShowroomPrices
				SET RTO = @RTO, isMetallic = @IsMetallic
				WHERE CarVersionId = @VersionId AND CityId = @CityId
			END
			IF (@Insurance IS NOT NULL)
			BEGIN
				UPDATE NewCarShowroomPrices
				SET Insurance = @Insurance, isMetallic = @IsMetallic
				WHERE CarVersionId = @VersionId AND CityId = @CityId
			END
			IF (@CorporateRTO IS NOT NULL)
			BEGIN
				UPDATE NewCarShowroomPrices
				SET CorporateRTO = @CorporateRTO, isMetallic = @IsMetallic
				WHERE CarVersionId = @VersionId AND CityId = @CityId
			END
		END

		DROP TABLE #TempShowroomPrices

		DROP TABLE #TempCharges

		--------------------------------------------------------------------------------------------------------------------------------------------------------
		IF (@CityId != 10)
		BEGIN
			EXEC [dbo].[UpdateModelPrices_v16_6_1] @VersionId, @CityId, @UpdatedBy -- Update max and min prices
		END

		EXEC [dbo].[Con_SaveNewCarNationalPrices] @VersionId, @UpdatedBy, @LastUpdated -- Insert/Update car showroom prices details
	END
END