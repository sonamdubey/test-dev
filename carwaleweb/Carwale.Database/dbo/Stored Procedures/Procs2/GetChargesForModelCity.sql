IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetChargesForModelCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetChargesForModelCity]
GO

	
-- =============================================
-- Author:		Chetan A Thambad
-- Create date: 14/07/2016
-- Description:	To get Compulsory Charges (Dealer Charges) and Optional Charges for model city Combination
-- Modified by Chetan T. on <18/07/2016> - Removed condition of showroom price check of NewcarshowroomPrices table
-- Modified by Chetan T. on <10/08/2016> - Filter Ex Showroom, RTO, insurance, TCS in both dropdown
-- EXEC [GetChargesForModelCity] 10,269,2
-- =============================================
CREATE PROCEDURE [dbo].[GetChargesForModelCity]
	-- Add the parameters for the stored procedure here
	@CityId INT
	,@ModelId INT
	,@Type INT
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
				AND PCI.Id NOT IN (2,3,5,77) -- Added by Chetan
				)
		) AS tempdata

	SELECT DISTINCT (PCI.Id)
		,PCI.CategoryName
	FROM PQ_CategoryItems PCI WITH (NOLOCK)
	LEFT OUTER JOIN PQ_CategoryItemsModelRules PMR WITH (NOLOCK) ON PCI.Id = PMR.ItemCategoryId
	LEFT OUTER JOIN PQ_CategoryItemsCityRules PCR WITH (NOLOCK) ON PCI.Id = PCR.ItemCategoryId
	LEFT OUTER JOIN PQ_CategoryItemsFuelRules PFR WITH (NOLOCK) ON PCI.Id = PFR.ItemCategoryId
	LEFT OUTER JOIN PQ_CategoryItemsAdditionalRules PAR WITH (NOLOCK) ON PCI.Id = PAR.ItemCategoryId
	WHERE PCI.[Type] = @Type
		AND PCI.Scope = 1
		AND PCI.IsActive = 1
		AND PCI.Id NOT IN (2,3,5,77) -- Added by Chetan
		OR (
			PCI.[Type] = @Type
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
			
			DROP TABLE #AdditionalRuletemp;
			
END
