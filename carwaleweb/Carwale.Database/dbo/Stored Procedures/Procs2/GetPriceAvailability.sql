IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPriceAvailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPriceAvailability]
GO

	
-- =============================================
-- Author:		Vicky Lund
-- Create date: 21/06/2016
-- EXEC [GetPriceAvailability]
-- =============================================
CREATE PROCEDURE [dbo].[GetPriceAvailability]
AS
BEGIN
	SELECT PPA.Id
		,PPA.[Name]
		,PPA.Explanation
		,PPA.[Type]
	FROM PQ_PriceAvailability PPA WITH (NOLOCK)
	WHERE PPA.IsActive = 1

	SELECT PPAMR.Id
		,PPAMR.PriceAvailabilityId
		,PPAMR.MakeId
		,PPAMR.ModelId
		,PPAMR.VersionId
		,CASE 
			WHEN PPAMR.MakeId = - 1
				THEN 'All Makes'
			ELSE CM.NAME
			END MakeName
		,CASE 
			WHEN PPAMR.ModelId = - 1
				THEN 'All Models'
			ELSE CM2.NAME
			END ModelName
		,CASE 
			WHEN PPAMR.VersionId = - 1
				THEN 'All Versions'
			ELSE CV.NAME
			END VersionName
	FROM PQ_PriceAvailabilityModelRules PPAMR WITH (NOLOCK)
	INNER JOIN PQ_PriceAvailability PPA WITH (NOLOCK) ON PPA.Id = PPAMR.PriceAvailabilityId
		AND PPA.IsActive = 1
	LEFT OUTER JOIN CarMakes CM WITH (NOLOCK) ON PPAMR.MakeId = CM.ID
	LEFT OUTER JOIN CarModels CM2 WITH (NOLOCK) ON PPAMR.ModelId = CM2.ID
	LEFT OUTER JOIN CarVersions CV WITH (NOLOCK) ON PPAMR.VersionId = CV.ID
	ORDER BY MakeName
		,ModelName
		,VersionName

	SELECT PPACR.Id
		,PPACR.PriceAvailabilityId
		,PPACR.StateId
		,PPACR.CityId
		,PPACR.ZoneId
		,CASE 
			WHEN PPACR.StateId = - 1
				THEN 'All States'
			ELSE S.NAME
			END StateName
		,CASE 
			WHEN PPACR.CityId = - 1
				THEN 'All Cities'
			ELSE C.NAME
			END CityName
		,CZ.ZoneName
	FROM PQ_PriceAvailabilityCityRules PPACR WITH (NOLOCK)
	INNER JOIN PQ_PriceAvailability PPA WITH (NOLOCK) ON PPA.Id = PPACR.PriceAvailabilityId
		AND PPA.IsActive = 1
	LEFT OUTER JOIN States S WITH (NOLOCK) ON PPACR.StateId = S.ID
	LEFT OUTER JOIN Cities C WITH (NOLOCK) ON PPACR.CityId = C.ID
	LEFT OUTER JOIN CityZones CZ WITH (NOLOCK) ON PPACR.ZoneId = CZ.Id
	ORDER BY StateName
		,CityName
		,ZoneName

	SELECT PPAFR.Id
		,PPAFR.PriceAvailabilityId
		,PPAFR.FuelTypeId FuelId
		,CFT.FuelType FuelName
	FROM PQ_PriceAvailabilityFuelRules PPAFR WITH (NOLOCK)
	INNER JOIN PQ_PriceAvailability PPA WITH (NOLOCK) ON PPA.Id = PPAFR.PriceAvailabilityId
		AND PPA.IsActive = 1
	INNER JOIN CarFuelType CFT WITH (NOLOCK) ON PPAFR.FuelTypeId = CFT.FuelTypeId
	ORDER BY FuelName

	SELECT PPAABR.Id
		,PPAABR.PriceAvailabilityId
		,PPAABR.AirBagId
		,UDM.NAME AirBagName
	FROM PQ_PriceAvailabilityAirBagRules PPAABR WITH (NOLOCK)
	INNER JOIN PQ_PriceAvailability PPA WITH (NOLOCK) ON PPA.Id = PPAABR.PriceAvailabilityId
		AND PPA.IsActive = 1
	INNER JOIN CD.UserDefinedMaster UDM WITH (NOLOCK) ON PPAABR.AirBagId = UDM.UserDefinedId
		AND UDM.ItemMasterId = 155 --155 implies AirBags

	SELECT PPASR.Id
		,PPASR.PriceAvailabilityId
		,PPASR.SegmentId
		,CS.NAME SegmentName
	FROM PQ_PriceAvailabilitySegmentRules PPASR WITH (NOLOCK)
	INNER JOIN PQ_PriceAvailability PPA WITH (NOLOCK) ON PPA.Id = PPASR.PriceAvailabilityId
		AND PPA.IsActive = 1
	INNER JOIN CarSegments CS WITH (NOLOCK) ON PPASR.SegmentId = CS.ID

	SELECT PPAR.Id
		,PPAR.PriceAvailabilityId
		,PPAR.DisplacementMin
		,PPAR.DisplacementMax
		,PPAR.LengthMin
		,PPAR.LengthMax
		,PPAR.GroundClearanceMin
		,PPAR.GroundClearanceMax
		,PPAR.ExShowroomMin
		,PPAR.ExShowroomMax
	FROM PQ_PriceAvailabilityAdditionalRules PPAR WITH (NOLOCK)
	INNER JOIN PQ_PriceAvailability PPA WITH (NOLOCK) ON PPA.Id = PPAR.PriceAvailabilityId
		AND PPA.IsActive = 1
END

