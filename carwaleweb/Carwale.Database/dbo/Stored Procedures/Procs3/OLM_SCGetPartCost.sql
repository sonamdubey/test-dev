IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SCGetPartCost]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SCGetPartCost]
GO

	-- =============================================
-- Author:		<Rahul Kumar>
-- Create date: <13-Nov-2013>
-- Description:	<GET PART COST,SERVICECOST ,REPAIR PARTS COST>
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SCGetPartCost] @VersionId INT
	,@CityId INT
	,@TypeofCost INT
	,@ReplacementParts VARCHAR(50)
	,@ServiceKm VARCHAR(10)
	,@RepairCityCost INT OUTPUT
	,@ServiceLabourCost INT OUTPUT
AS
BEGIN
	DECLARE @ServiceLabourCostType INT = 3
	DECLARE @CityCost INT = 500

	SET @ServiceLabourCost = - 1

	--To set values for @ServiceLabourCostType and @CityCost
	IF EXISTS (
			SELECT CityId
			FROM OLM_SCACities
			WHERE CityId = @CityId
			)
	BEGIN
		SET @ServiceLabourCostType = 3
		SET @CityCost = 500
	END
	ELSE
	BEGIN
		SET @ServiceLabourCostType = 4
		SET @CityCost = 400
	END

	--Sets @RepairCityCost output parameter
	SET @RepairCityCost = @CityCost

	--To process for Service Cost Request OR Both Request
	IF (
			@TypeofCost = 1
			OR @TypeofCost = 3
			)
	BEGIN
		--Fetches the Service Cost data table 
		SELECT SCPM.PartDescription
			,SCSP.Price
			,SCSP.Quantity
			,SCPM.PartType
			,CV.NAME AS VersionName
			,CASE 
				WHEN @ServiceKm = '[105K]'
					THEN SCSP.[105K]
				WHEN @ServiceKm = '[120K]'
					THEN SCSP.[120K]
				WHEN @ServiceKm = '[135K]'
					THEN SCSP.[135K]
				WHEN @ServiceKm = '[150K]'
					THEN SCSP.[150K]
				WHEN @ServiceKm = '[15K]'
					THEN SCSP.[15K]
				WHEN @ServiceKm = '[30K]'
					THEN SCSP.[30K]
				WHEN @ServiceKm = '[45K]'
					THEN SCSP.[45K]
				WHEN @ServiceKm = '[60K]'
					THEN SCSP.[60K]
				WHEN @ServiceKm = '[75K]'
					THEN SCSP.[75K]
				WHEN @ServiceKm = '[90K]'
					THEN SCSP.[90K]
				END AS Cost
		FROM OLM_SCPartsMaster SCPM
		JOIN OLM_SCServiceParts SCSP ON SCPM.Id = SCSP.PartId
		JOIN OLM_SCMappedVersions SCMV ON SCMV.MappedVersionId = SCSP.VersionId
		JOIN CarVersions CV ON CV.ID = SCMV.VersionId
		WHERE SCMV.VersionId = @VersionId
			AND SCPM.PartType = 1
		ORDER BY PartId

		--Fetches the service labour cost
		SET @ServiceLabourCost = ISNULL((
					SELECT TOP 1 CASE 
							WHEN @ServiceKm = '[105K]'
								THEN SCSP.[105K]
							WHEN @ServiceKm = '[120K]'
								THEN SCSP.[120K]
							WHEN @ServiceKm = '[135K]'
								THEN SCSP.[135K]
							WHEN @ServiceKm = '[150K]'
								THEN SCSP.[150K]
							WHEN @ServiceKm = '[15K]'
								THEN SCSP.[15K]
							WHEN @ServiceKm = '[30K]'
								THEN SCSP.[30K]
							WHEN @ServiceKm = '[45K]'
								THEN SCSP.[45K]
							WHEN @ServiceKm = '[60K]'
								THEN SCSP.[60K]
							WHEN @ServiceKm = '[75K]'
								THEN SCSP.[75K]
							WHEN @ServiceKm = '[90K]'
								THEN SCSP.[90K]
							END
					FROM OLM_SCPartsMaster SCPM
					JOIN OLM_SCServiceParts SCSP ON SCPM.Id = SCSP.PartId
					JOIN OLM_SCMappedVersions SCMV ON SCMV.MappedVersionId = SCSP.VersionId
					JOIN CarVersions CV ON CV.ID = SCMV.VersionId
					WHERE SCMV.VersionId = @VersionId
						AND PartType = @ServiceLabourCostType
					ORDER BY PartId
					), 0)
	END

	--To process for Repair Cost Request OR Both Request
	IF (
			@TypeofCost = 2
			OR @TypeofCost = 3
			)
	BEGIN
		SELECT SCPM.PartDescription
			,SCRP.PartCost
			,SCRP.LabourPercentage
			,SCPM.PartType
			,CV.NAME AS VersionName
		FROM OLM_SCPartsMaster SCPM
		JOIN OLM_SCReplacementParts SCRP ON SCPM.Id = SCRP.PartId
		JOIN OLM_SCMappedVersions SCMV ON SCMV.MappedVersionId = SCRP.VersionId
		JOIN CarVersions CV ON CV.ID = SCMV.VersionId
		WHERE SCMV.VersionId = @VersionId
			AND SCRP.PartId IN (
				SELECT F.ListMember
				FROM fnSplitCSV(@ReplacementParts) AS F
				)
		ORDER BY PartId
	END

	PRINT '@RepairCityCost : ' + CONVERT(VARCHAR, @RepairCityCost)
	PRINT '@ServiceLabourCost : ' + CONVERT(VARCHAR, ISNULL(@ServiceLabourCost, 0))
END
