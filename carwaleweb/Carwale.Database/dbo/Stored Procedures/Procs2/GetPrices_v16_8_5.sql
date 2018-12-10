IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPrices_v16_8_5]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPrices_v16_8_5]
GO

	--==========================================================
-- Created : Vicky Lund, 20/07/2016
-- EXEC [GetPrices_v16_10_4] 229, 160
-- EXEC [GetPrices_v16_10_4] 552, 1
-- Modified : Vicky Lund, 05/10/2016, Create temp table schema
--==========================================================
CREATE PROCEDURE [dbo].[GetPrices_v16_8_5] @ModelId INT
	,@CityId INT
AS
BEGIN
	DECLARE @StateId INT

	IF @CityId = - 1
	BEGIN
		SELECT CV.ID VersionId
			,CNCNP.AvgPrice AveragePrice
			,NULL ExShowroomPrice
			,NULL CityId
			,NULL CityName
			,0 IsVersionBlocked
			,NULL ReasonText
		FROM CarVersions CV WITH (NOLOCK)
		LEFT OUTER JOIN Con_NewCarNationalPrices CNCNP WITH (NOLOCK) ON CV.ID = CNCNP.VersionId
		WHERE CV.CarModelId = @ModelId
			AND CV.IsDeleted = 0
			AND CV.New = 1
	END
	ELSE
	BEGIN
		SELECT @StateId = C.StateId
		FROM Cities C WITH (NOLOCK)
		WHERE C.ID = @CityId

		CREATE TABLE #Versions (
			VersionId NUMERIC(18, 0)
			,VersionName VARCHAR(50)
			,ModelId NUMERIC(18, 0)
			,MakeId NUMERIC(18, 0)
			,Price NUMERIC(18, 0)
			,Displacement INT
			,[Length] INT
			,GroundClearance INT
			,SegmentId NUMERIC(18, 0)
			,FuelTypeId SMALLINT
			,AirBagId INT
			,AvgPrice NUMERIC(18, 0)
			)

		INSERT INTO #Versions (
			VersionId
			,VersionName
			,ModelId
			,MakeId
			,Price
			,Displacement
			,[Length]
			,GroundClearance
			,SegmentId
			,FuelTypeId
			,AirBagId
			,AvgPrice
			)
		SELECT V.VersionId
			,V.VersionName
			,V.ModelId
			,V.MakeId
			,V.Price
			,V.Displacement
			,V.[Length]
			,V.GroundClearance
			,V.SegmentId
			,V.FuelTypeId
			,V.AirBagId
			,V.AvgPrice
		FROM (
			SELECT CV.ID VersionId
				,CV.NAME VersionName
				,CV.CarModelId ModelId
				,CM.CarMakeId MakeId
				,NCSP.Price
				,NCS.Displacement
				,NCS.[Length]
				,NCS.GroundClearance
				,CV.SegmentId
				,CFT.FuelTypeId
				,IV.UserDefinedId AS AirBagId
				,CNCNP.AvgPrice
			FROM CarVersions CV WITH (NOLOCK)
			INNER JOIN CarModels CM WITH (NOLOCK) ON CM.ID = CV.CarModelId
				AND CM.IsDeleted = 0
				AND CM.New = 1
			INNER JOIN CarFuelType CFT WITH (NOLOCK) ON CV.CarFuelType = CFT.FuelTypeId
			LEFT OUTER JOIN NewCarSpecifications NCS WITH (NOLOCK) ON CV.ID = NCS.CarVersionId
			LEFT OUTER JOIN cd.ItemValues IV WITH (NOLOCK) ON IV.CarVersionId = CV.ID
				AND ItemMasterId = 155
			LEFT OUTER JOIN NewCarShowroomPrices NCSP WITH (NOLOCK) ON CV.ID = NCSP.CarVersionId
				AND NCSP.IsActive = 1
				AND NCSP.CityId = @CityId
			LEFT OUTER JOIN Con_NewCarNationalPrices CNCNP WITH (NOLOCK) ON CV.ID = CNCNP.VersionId
			WHERE CV.CarModelId = @ModelId
				AND CV.IsDeleted = 0
				AND CV.New = 1
			) AS V

		CREATE TABLE #FilteredRules (
			Id INT
			,Explanation VARCHAR(200)
			,ShowPrice INT
			)

		INSERT INTO #FilteredRules (
			Id
			,Explanation
			,ShowPrice
			)
		SELECT FR.Id
			,FR.Explanation
			,FR.ShowPrice
		FROM (
			SELECT PPA.Id
				,PPA.Explanation
				,0 ShowPrice
			FROM PQ_PriceAvailability PPA WITH (NOLOCK)
			INNER JOIN PQ_PriceAvailabilityCityRules PPACR WITH (NOLOCK) ON PPACR.StateId = @StateId
				AND (
					PPACR.CityId = @CityId
					OR PPACR.CityId = - 1
					)
				AND PPA.Id = PPACR.PriceAvailabilityId
				AND PPA.IsActive = 1
				AND PPA.[Type] = 2
			
			UNION
			
			(
				SELECT PPA2.Id
					,PPA2.Explanation
					,0 ShowPrice
				FROM PQ_PriceAvailability PPA2 WITH (NOLOCK)
				WHERE PPA2.IsActive = 1
					AND PPA2.[Type] = 1
					AND PPA2.Id NOT IN (
						SELECT PPA1.Id
						FROM PQ_PriceAvailability PPA1 WITH (NOLOCK)
						INNER JOIN PQ_PriceAvailabilityCityRules PPACR WITH (NOLOCK) ON PPACR.StateId = @StateId
							AND (
								PPACR.CityId = @CityId
								OR PPACR.CityId = - 1
								)
							AND PPA1.Id = PPACR.PriceAvailabilityId
							AND PPA1.IsActive = 1
							AND PPA1.[Type] = 1
						)
				)
			
			UNION
			
			(
				SELECT PPA.Id
					,PPA.Explanation
					,1 ShowPrice
				FROM PQ_PriceAvailability PPA WITH (NOLOCK)
				INNER JOIN PQ_PriceAvailabilityCityRules PPACR WITH (NOLOCK) ON PPACR.StateId = @StateId
					AND (
						PPACR.CityId = @CityId
						OR PPACR.CityId = - 1
						)
					AND PPA.Id = PPACR.PriceAvailabilityId
					AND PPA.IsActive = 1
					AND PPA.[Type] = 1
				)
			) AS FR

		CREATE TABLE #DenormRules (
			PriceAvailabilityId INT
			,ModelRuleId INT
			,MakeId INT
			,ModelId INT
			,VersionId INT
			,SegmentRuleId INT
			,SegmentId INT
			,AirBagRuleId INT
			,AirBagId INT
			,FuelRuleId INT
			,FuelTypeId INT
			,AdditionalRuleId INT
			,DisplacementMin INT
			,DisplacementMax INT
			,LengthMin INT
			,LengthMax INT
			,GroundClearanceMin INT
			,GroundClearanceMax INT
			,ExShowroomMin INT
			,ExShowroomMax INT
			,Explanation VARCHAR(200)
			,ShowPrice INT
			)

		INSERT INTO #DenormRules (
			PriceAvailabilityId
			,ModelRuleId
			,MakeId
			,ModelId
			,VersionId
			,SegmentRuleId
			,SegmentId
			,AirBagRuleId
			,AirBagId
			,FuelRuleId
			,FuelTypeId
			,AdditionalRuleId
			,DisplacementMin
			,DisplacementMax
			,LengthMin
			,LengthMax
			,GroundClearanceMin
			,GroundClearanceMax
			,ExShowroomMin
			,ExShowroomMax
			,Explanation
			,ShowPrice
			)
		SELECT DR.PriceAvailabilityId
			,DR.ModelRuleId
			,DR.MakeId
			,DR.ModelId
			,DR.VersionId
			,DR.SegmentRuleId
			,DR.SegmentId
			,DR.AirBagRuleId
			,DR.AirBagId
			,DR.FuelRuleId
			,DR.FuelTypeId
			,DR.AdditionalRuleId
			,DR.DisplacementMin
			,DR.DisplacementMax
			,DR.LengthMin
			,DR.LengthMax
			,DR.GroundClearanceMin
			,DR.GroundClearanceMax
			,DR.ExShowroomMin
			,DR.ExShowroomMax
			,DR.Explanation
			,DR.ShowPrice
		FROM (
			SELECT FR.Id PriceAvailabilityId
				,PPAMR.Id ModelRuleId
				,PPAMR.MakeId
				,PPAMR.ModelId
				,PPAMR.VersionId
				,PPASR.Id SegmentRuleId
				,PPASR.SegmentId
				,PPAABR.Id AirBagRuleId
				,PPAABR.AirBagId
				,PPAFR.Id FuelRuleId
				,PPAFR.FuelTypeId
				,PPAAR.Id AdditionalRuleId
				,PPAAR.DisplacementMin
				,PPAAR.DisplacementMax
				,PPAAR.LengthMin
				,PPAAR.LengthMax
				,PPAAR.GroundClearanceMin
				,PPAAR.GroundClearanceMax
				,PPAAR.ExShowroomMin
				,PPAAR.ExShowroomMax
				,FR.Explanation
				,FR.ShowPrice
			FROM #FilteredRules FR
			LEFT OUTER JOIN PQ_PriceAvailabilityModelRules PPAMR WITH (NOLOCK) ON FR.Id = PPAMR.PriceAvailabilityId
			LEFT OUTER JOIN PQ_PriceAvailabilityFuelRules PPAFR WITH (NOLOCK) ON PPAFR.PriceAvailabilityId = FR.Id
			LEFT OUTER JOIN PQ_PriceAvailabilitySegmentRules PPASR WITH (NOLOCK) ON PPASR.PriceAvailabilityId = FR.Id
			LEFT OUTER JOIN PQ_PriceAvailabilityAirBagRules PPAABR WITH (NOLOCK) ON PPAABR.PriceAvailabilityId = FR.Id
			LEFT OUTER JOIN PQ_PriceAvailabilityAdditionalRules PPAAR WITH (NOLOCK) ON PPAAR.PriceAvailabilityId = FR.Id
			) AS DR

		CREATE TABLE #ApplicableRules (
			VersionId INT
			,PriceAvailabilityId INT
			,RowNumber INT
			,Explanation VARCHAR(200)
			,ShowPrice INT
			)

		INSERT INTO #ApplicableRules (
			VersionId
			,PriceAvailabilityId
			,RowNumber
			,Explanation
			,ShowPrice
			)
		SELECT AR.VersionId
			,AR.PriceAvailabilityId
			,AR.RowNumber
			,AR.Explanation
			,AR.ShowPrice
		FROM (
			SELECT V.VersionId
				,DR.PriceAvailabilityId
				,ROW_NUMBER() OVER (
					PARTITION BY V.VersionId ORDER BY DR.ShowPrice DESC
						,DR.PriceAvailabilityId DESC
					) RowNumber
				,DR.Explanation
				,DR.ShowPrice
			FROM #DenormRules DR WITH (NOLOCK)
			INNER JOIN #Versions V WITH (NOLOCK) ON (
					DR.ModelRuleId IS NULL
					OR (
						DR.VersionId = V.VersionId
						OR DR.VersionId = - 1
						)
					AND (
						DR.ModelId = V.ModelId
						OR DR.ModelId = - 1
						)
					AND (DR.MakeId = V.MakeId)
					)
				AND (
					DR.SegmentRuleId IS NULL
					OR (DR.SegmentId = V.SegmentId)
					)
				AND (
					DR.AirBagRuleId IS NULL
					OR (DR.AirBagId = V.AirBagId)
					)
				AND (
					DR.FuelRuleId IS NULL
					OR (DR.FuelTypeId = V.FuelTypeId)
					)
				AND (
					DR.AdditionalRuleId IS NULL
					OR (
						(
							V.Displacement >= DR.DisplacementMin
							OR DR.DisplacementMin IS NULL
							)
						AND (
							V.Displacement <= DR.DisplacementMax
							OR DR.DisplacementMax IS NULL
							)
						AND (
							V.[Length] >= DR.LengthMin
							OR DR.LengthMin IS NULL
							)
						AND (
							V.[Length] <= DR.LengthMax
							OR DR.LengthMax IS NULL
							)
						AND (
							V.GroundClearance >= DR.GroundClearanceMin
							OR DR.GroundClearanceMin IS NULL
							)
						AND (
							V.GroundClearance <= DR.GroundClearanceMax
							OR DR.GroundClearanceMax IS NULL
							)
						AND (
							V.Price >= DR.ExShowroomMin
							OR DR.ExShowroomMin IS NULL
							)
						AND (
							V.Price <= DR.ExShowroomMax
							OR DR.ExShowroomMax IS NULL
							)
						)
					)
			) AS AR

		SELECT V.VersionId
			,ISNULL(V.AvgPrice, 0) AveragePrice
			,ISNULL(CONVERT(INT, V.Price), 0) ExShowroomPrice
			,@CityId CityId
			,C.[Name] CityName
			,CASE 
				WHEN AR.ShowPrice = 0
					AND AR.PriceAvailabilityId IS NOT NULL
					THEN 1
				ELSE 0
				END IsVersionBlocked
			,CASE 
				WHEN AR.ShowPrice = 0
					AND AR.PriceAvailabilityId IS NOT NULL
					THEN AR.Explanation
				ELSE NULL
				END ReasonText
		FROM #Versions V WITH (NOLOCK)
		LEFT OUTER JOIN #ApplicableRules AR WITH (NOLOCK) ON V.VersionId = AR.VersionId
			AND AR.RowNumber = 1
		INNER JOIN Cities C WITH (NOLOCK) ON C.ID = @CityId
			AND C.IsDeleted = 0

		DROP TABLE #Versions

		DROP TABLE #FilteredRules

		DROP TABLE #DenormRules

		DROP TABLE #ApplicableRules
	END
END
