IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOptionalCharges]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOptionalCharges]
GO

	
-- =============================================
-- Author:		Chetan A Thambad
-- Create date: 21/06/2016
-- Description:	To get Optional Charges (Dealer Charges) for model city Combination
-- EXEC [GetOptionalCharges] 10,862
-- =============================================
CREATE PROCEDURE [dbo].[GetOptionalCharges]
	-- Add the parameters for the stored procedure here
	@CityId INT
	,@ModelId INT
AS
BEGIN
	SELECT *				-- Getting all versions matching the following condition
	INTO #AdditionalRuletemp
	FROM (
		SELECT PCI.Id
		FROM NewCarSpecifications NCS WITH (NOLOCK)
			,PQ_CategoryItemsAdditionalRules PARS WITH (NOLOCK)
		INNER JOIN PQ_CategoryItems PCI WITH (NOLOCK) ON PARS.ItemCategoryId = PCI.Id
		WHERE (
				(
					(
						NCS.Displacement >= PARS.DisplacementMin
						OR PARS.DisplacementMin IS NULL
						)
					AND (
						NCS.Displacement <= PARS.DisplacementMax
						OR PARS.DisplacementMax IS NULL
						)
					)
				AND (
					(
						NCS.[Length] >= PARS.LengthMin
						OR PARS.LengthMin IS NULL
						)
					AND (
						NCS.[Length] <= PARS.LengthMax
						OR PARS.LengthMax IS NULL
						)
					)
				AND (
					(
						NCS.GroundClearance >= PARS.GroundClearanceMin
						OR PARS.GroundClearanceMin IS NULL
						)
					AND (
						NCS.GroundClearance <= PARS.GroundClearanceMax
						OR PARS.GroundClearanceMax IS NULL
						)
					)
				AND NCS.CarVersionId IN (
					SELECT VersionId
					FROM vwMMV WITH (NOLOCK)
					WHERE ModelId = @ModelId
					)
				)
		) AS tempdata

	SELECT DISTINCT (PCI.Id)
		,PCI.CategoryName
	FROM PQ_CategoryItems PCI WITH (NOLOCK)
	LEFT OUTER JOIN PQ_CategoryItemsModelRules PMR WITH (NOLOCK) ON PCI.Id = PMR.ItemCategoryId
	LEFT OUTER JOIN PQ_CategoryItemsCityRules PCR WITH (NOLOCK) ON PCI.Id = PCR.ItemCategoryId
	LEFT OUTER JOIN PQ_CategoryItemsFuelRules PFR WITH (NOLOCK) ON PCI.Id = PFR.ItemCategoryId
	LEFT OUTER JOIN PQ_CategoryItemsAdditionalRules PAR WITH (NOLOCK) ON PCI.Id = PAR.ItemCategoryId
	WHERE PCI.[Type] = 2
		AND PCI.Scope = 1
		AND PCI.IsActive = 1
		OR (
			PCI.[Type] = 2
			AND PCI.IsActive = 1
			AND PCI.Scope = 2
			AND (
				(
					PMR.ModelId IS NULL
					OR PMR.ModelId = @ModelId
					OR (
						PMR.ModelId = - 1
						AND PMR.MakeId = (
							SELECT TOP 1 MakeId
							FROM vwMMV WITH (NOLOCK)
							WHERE ModelId = @ModelId
							)
						)
					)
				AND (
					PCR.CityId IS NULL
					OR PCR.CityId = @CityId
					OR (
						PCR.CityId = - 1
						AND PCR.StateId = (
							SELECT TOP 1 stateid
							FROM Cities WITH (NOLOCK)
							WHERE ID = @CityId
							)
						)
					)
				AND (
					(PFR.FuelTypeId IS NULL)
					OR (
						PFR.FuelTypeId IN (
							SELECT CarFuelType
							FROM CarVersions WITH (NOLOCK)
							WHERE CarModelId = @ModelId
							)
						)
					)
				AND (
					(PAR.ItemCategoryId IS NULL)
					OR (
						PAR.ItemCategoryId IN (
							SELECT ID
							FROM #AdditionalRuletemp WITH (NOLOCK)
							)
						)
					)
				)
			)
END


-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[GetVersionDetailsById]    Script Date: 05-07-2016 11:37:14 ******/
-- SET ANSI_NULLS ON
