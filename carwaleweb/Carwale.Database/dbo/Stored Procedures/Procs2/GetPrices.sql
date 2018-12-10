IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPrices]
GO

	--==========================================================
-- Created : Vicky Lund, 20/07/2016
-- EXEC GetPrices 229, 160
-- EXEC GetPrices 552, 10
--==========================================================
CREATE PROCEDURE [dbo].[GetPrices] @ModelId INT
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
		INTO #Versions
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
			--AND CNCNP.IsActive = 1
			WHERE CV.CarModelId = @ModelId
				AND CV.IsDeleted = 0
				AND CV.New = 1
			) AS V

		SELECT DR.PriceAvailabilityId
			,DR.CityRuleId
			,DR.StateId
			,DR.CityId
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
		INTO #DenormRules
		FROM (
			SELECT PPA.Id PriceAvailabilityId
				,PPACR.Id CityRuleId
				,PPACR.StateId
				,PPACR.CityId
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
				,PPA.Explanation
			FROM PQ_PriceAvailability PPA WITH (NOLOCK)
			INNER JOIN PQ_PriceAvailabilityCityRules PPACR WITH (NOLOCK) ON PPACR.StateId = @StateId
				AND (
					PPACR.CityId = @CityId
					OR PPACR.CityId = - 1
					)
				AND PPA.Id = PPACR.PriceAvailabilityId
				AND PPA.IsActive = 1
				AND PPA.[Type] = 2
			LEFT OUTER JOIN PQ_PriceAvailabilityModelRules PPAMR WITH (NOLOCK) ON PPA.Id = PPAMR.PriceAvailabilityId
			LEFT OUTER JOIN PQ_PriceAvailabilityFuelRules PPAFR WITH (NOLOCK) ON PPAFR.PriceAvailabilityId = PPA.Id
			LEFT OUTER JOIN PQ_PriceAvailabilitySegmentRules PPASR WITH (NOLOCK) ON PPASR.PriceAvailabilityId = PPA.Id
			LEFT OUTER JOIN PQ_PriceAvailabilityAirBagRules PPAABR WITH (NOLOCK) ON PPAABR.PriceAvailabilityId = PPA.Id
			LEFT OUTER JOIN PQ_PriceAvailabilityAdditionalRules PPAAR WITH (NOLOCK) ON PPAAR.PriceAvailabilityId = PPA.Id
			) AS DR

		SELECT AR.VersionId
			,AR.PriceAvailabilityId
			,AR.RowNumber
			,AR.Explanation
		INTO #ApplicableRules
		FROM (
			SELECT V.VersionId
				,DR.PriceAvailabilityId
				,ROW_NUMBER() OVER (
					PARTITION BY V.VersionId ORDER BY DR.PriceAvailabilityId
					) RowNumber
				,DR.Explanation
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
				WHEN AR.PriceAvailabilityId IS NOT NULL
					THEN 1
				ELSE 0
				END IsVersionBlocked
			,AR.Explanation ReasonText
		FROM #Versions V WITH (NOLOCK)
		LEFT OUTER JOIN #ApplicableRules AR WITH (NOLOCK) ON V.VersionId = AR.VersionId
			AND AR.RowNumber = 1
		INNER JOIN Cities C WITH (NOLOCK) ON C.ID = @CityId
			AND C.IsDeleted = 0

		DROP TABLE #Versions

		DROP TABLE #DenormRules

		DROP TABLE #ApplicableRules
	END
END

