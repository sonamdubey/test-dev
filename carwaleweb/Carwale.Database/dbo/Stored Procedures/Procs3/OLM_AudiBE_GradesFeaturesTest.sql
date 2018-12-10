IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_GradesFeaturesTest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_GradesFeaturesTest]
GO

	--sp_he-- ====================================================================================

-- Author:		Supriya Khartode

-- Create date: 26/7/2013

-- Description : To fetch only grades & its features for transaction selected

-- Modified by : Supriya K on 9/10/2012 to add IsActive filter

-- Modified by : Supiya K on 30/10/2013 to fetch comma seperated VersionIds of version specific 

--               features in VersionSpecifics column 

--Modified By : Rakesh Yadav 12 Aug 2014 order by is changed from g.id to g.Priority to make Attraction first in list



-- ====================================================================================

CREATE PROCEDURE [dbo].[OLM_AudiBE_GradesFeaturesTest] @TransactionId INTEGER

AS

BEGIN

	SET NOCOUNT ON;

	SELECT g.Id AS GradeId

		,g.NAME AS GradeName

		,f.NAME AS FeatureName

		,(

			SELECT ',' + Convert(VARCHAR, mgfvs.VersionId)

			FROM OLM_AudiBE_Model_GradeFeatures_VersionSpecifics mgfvs

			WHERE mgfvs.ModelGradeFeatureId = mgf.Id

			FOR XML PATH('')

			) AS VersionSpecifics

	FROM OLM_AudiBE_Transactions t

	LEFT JOIN OLM_AudiBE_Model_GradeFeatures mgf ON t.ModelId = mgf.ModelId

		AND mgf.IsActive = 1

	LEFT JOIN OLM_AudiBE_Grades g ON mgf.GradeId = g.Id

		AND g.IsActive = 1

	LEFT JOIN OLM_AudiBE_Features f ON mgf.FeatureId = f.Id

		AND f.IsActive = 1

	WHERE t.Id = @TransactionId

	ORDER BY g.Priority

END

--lptext OLM_AudiBE_GradesFeatures