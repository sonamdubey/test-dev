IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCharges]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCharges]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 21/06/2016
-- EXEC [GetCharges] 1
-- Modify By : Sanjay Soni, Added autoPopulate specs select query
-- Modified : Vicky Lund, 22/07/2016, Added Explanation column
-- =============================================
CREATE PROCEDURE [dbo].[GetCharges] @Type INT
AS
BEGIN
	SELECT PCI.Id
		,PCI.CategoryName
		,PCI.[Type]
		,PCI.Scope
		,PCI.Explanation
	FROM PQ_CategoryItems PCI WITH (NOLOCK)
	WHERE (
			PCI.[Type] = @Type
			OR @Type = - 1
			)
		AND PCI.IsActive = 1

	SELECT PCIMR.Id
		,PCIMR.ItemCategoryId
		,PCIMR.MakeId
		,PCIMR.ModelId
		,CASE 
			WHEN PCIMR.MakeId = - 1
				THEN 'All Makes'
			ELSE CM.NAME
			END MakeName
		,CASE 
			WHEN PCIMR.ModelId = - 1
				THEN 'All Models'
			ELSE CM2.NAME
			END ModelName
	FROM PQ_CategoryItemsModelRules PCIMR WITH (NOLOCK)
	INNER JOIN PQ_CategoryItems PCI WITH (NOLOCK) ON PCI.Id = PCIMR.ItemCategoryId
		AND (
			PCI.[Type] = @Type
			OR @Type = - 1
			)
		AND PCI.IsActive = 1
	LEFT OUTER JOIN CarMakes CM WITH (NOLOCK) ON PCIMR.MakeId = CM.ID
	LEFT OUTER JOIN CarModels CM2 WITH (NOLOCK) ON PCIMR.ModelId = CM2.ID
	ORDER BY MakeName
		,ModelName

	SELECT PCICR.Id
		,PCICR.ItemCategoryId
		,PCICR.StateId
		,PCICR.CityId
		,PCICR.ZoneId
		,CASE 
			WHEN PCICR.StateId = - 1
				THEN 'All States'
			ELSE S.NAME
			END StateName
		,CASE 
			WHEN PCICR.CityId = - 1
				THEN 'All Cities'
			ELSE C.NAME
			END CityName
		,CZ.ZoneName
	FROM PQ_CategoryItemsCityRules PCICR WITH (NOLOCK)
	INNER JOIN PQ_CategoryItems PCI WITH (NOLOCK) ON PCI.Id = PCICR.ItemCategoryId
		AND (
			PCI.[Type] = @Type
			OR @Type = - 1
			)
		AND PCI.IsActive = 1
	LEFT OUTER JOIN States S WITH (NOLOCK) ON PCICR.StateId = S.ID
	LEFT OUTER JOIN Cities C WITH (NOLOCK) ON PCICR.CityId = C.ID
	LEFT OUTER JOIN CityZones CZ WITH (NOLOCK) ON PCICR.ZoneId = CZ.Id
	ORDER BY StateName
		,CityName
		,ZoneName

	SELECT PCIFR.Id
		,PCIFR.ItemCategoryId
		,PCIFR.FuelTypeId FuelId
		,CFT.FuelType FuelName
	FROM PQ_CategoryItemsFuelRules PCIFR WITH (NOLOCK)
	INNER JOIN PQ_CategoryItems PCI WITH (NOLOCK) ON PCI.Id = PCIFR.ItemCategoryId
		AND (
			PCI.[Type] = @Type
			OR @Type = - 1
			)
		AND PCI.IsActive = 1
	INNER JOIN CarFuelType CFT WITH (NOLOCK) ON PCIFR.FuelTypeId = CFT.FuelTypeId
	ORDER BY FuelName

	SELECT PCIAR.Id
		,PCIAR.ItemCategoryId
		,PCIAR.DisplacementMin
		,PCIAR.DisplacementMax
		,PCIAR.LengthMin
		,PCIAR.LengthMax
		,PCIAR.GroundClearanceMin
		,PCIAR.GroundClearanceMax
		,PCIAR.ExShowroomMin
		,PCIAR.ExShowroomMax
	FROM PQ_CategoryItemsAdditionalRules PCIAR WITH (NOLOCK)
	INNER JOIN PQ_CategoryItems PCI WITH (NOLOCK) ON PCI.Id = PCIAR.ItemCategoryId
		AND (
			PCI.[Type] = @Type
			OR @Type = - 1
			)
		AND PCI.IsActive = 1

	SELECT PCAPS.Id
		,PCAPS.IsAutoPopulate
		,PCAPS.ItemCategoryId
		,PCAPS.ValueType
		,PCAPS.Value
		,PCAPS.RefChargeId
	FROM PQ_CategoryItemsAutoPopulateSpecification PCAPS WITH (NOLOCK)
	INNER JOIN PQ_CategoryItems PCI WITH (NOLOCK) ON PCI.Id = PCAPS.ItemCategoryId
		AND (
			PCI.[Type] = @Type
			OR @Type = - 1
			)
		AND PCI.IsActive = 1
END

