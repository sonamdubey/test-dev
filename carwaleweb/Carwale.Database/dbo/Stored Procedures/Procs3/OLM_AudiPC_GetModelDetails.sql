IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiPC_GetModelDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiPC_GetModelDetails]
GO

	-- =============================================================================================

-- Author	    :  Supriya Khartode

-- Created date :  22 Oct 2013

-- Description  :  SP to get all model details

-- exec OLM_AudiPC_GetModelDetails 4

-- Modified By Supriya on 5/12/2013 to add v.isActive filter in GetModelFuelType
-- Modified By Rakesh Yadav on 14/08/2014 to 
-- =============================================================================================

CREATE PROCEDURE [dbo].[OLM_AudiPC_GetModelDetails] @ModelId INTEGER

AS

BEGIN

	SET NOCOUNT ON;



	--OLM_AudiBE_GetModelFuelType

	SELECT DISTINCT vs.Value AS FuelTypeId

		,CASE vs.Value

			WHEN 1

				THEN 'Diesel'

			ELSE 'Petrol'

			END AS FuelType

	FROM OLM_AudiBE_Versions v

	LEFT JOIN OLM_AudiBE_Version_Specs vs ON v.Id = vs.VersionId

	WHERE vs.SpecId = 1

		AND v.ModelId = @ModelId AND v.isActive=1



	--[OLM_AudiBE_VersionsByModel]

	SELECT uvw.VersionId

		,uvw.VersionName

		,uvw.Specification

		,uvw.Value

	FROM (

		SELECT v.Id AS VersionId

			,v.NAME AS VersionName

			,s.Id AS SpecId

			,s.NAME AS Specification

			,vs.Value

			,MAX(VG.GradeId) AS GradeId

		FROM.OLM_AudiBE_Models m

		LEFT JOIN OLM_AudiBE_Versions v ON m.Id = v.ModelId

		LEFT JOIN OLM_AudiBE_Version_Specs vs ON v.Id = vs.VersionId

		LEFT JOIN OLM_AudiBE_Specs s ON s.Id = vs.SpecId

		LEFT JOIN OLM_AudiBE_VersionGrades VG ON v.Id = VG.VersionId

		WHERE v.IsActive = 1

			AND vs.IsActive = 1

			AND VG.IsActive = 1

			AND m.Id = @ModelId

		GROUP BY v.Id

			,v.NAME

			,s.Id

			,s.NAME

			,vs.Value

		) uvw

	LEFT JOIN OLM_AudiBE_VersionPrices vp ON uvw.VersionId = vp.VersionId

		AND vp.IsActive = 1

		AND vp.StateId = 1

		AND vp.GradeId = uvw.GradeId

	ORDER BY vp.Price

		,uvw.SpecId



	--OLM_AudiBE_GradesFeatures

	--Modified by : Supiya K on 30/10/2013 to fetch comma seperated VersionIds of version specific 

	--              features in VersionSpecifics column 

	SELECT g.Id AS GradeId

		,g.NAME AS GradeName

		,f.NAME AS FeatureName

		,(

			SELECT ',' + Convert(VARCHAR, mgfvs.VersionId)

			FROM OLM_AudiBE_Model_GradeFeatures_VersionSpecifics mgfvs

			WHERE mgfvs.ModelGradeFeatureId = mgf.Id

			FOR XML PATH('')

			) AS VersionSpecifics

	FROM OLM_AudiBE_Model_GradeFeatures mgf

	LEFT JOIN OLM_AudiBE_Grades g ON mgf.GradeId = g.Id

		AND g.IsActive = 1

	LEFT JOIN OLM_AudiBE_Features f ON mgf.FeatureId = f.Id

		AND f.IsActive = 1

	WHERE mgf.ModelId = @ModelId

		AND mgf.IsActive = 1

	ORDER BY g.Priority--changed from g.Id to g.Priority by rakesh Yadav on 14 Aug 2014 to make Grade Atrraction of A3 first in list



	--to get ModelName & MainModelColorId

	SELECT NAME

		,MainModelColorId

	FROM OLM_AudiBE_Models m

	WHERE m.ID = @ModelId

		AND m.IsActive = 1

END


