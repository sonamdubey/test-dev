IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_GetVersionGrades]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_GetVersionGrades]
GO

	
-- =============================================================================================
-- Author:		Ashish G. Kamble
-- Create date: 29 July 2013
-- Description:	SP to get the version grades for a particular model
-- EXEC OLM_AudiBE_GetVersionGrades 133
-- =============================================================================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_GetVersionGrades]
	@TransactionId BIGINT
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT t.modelId, vg.VersionId, v.Name AS VersionName, vg.GradeId, g.Name AS GradeName
	FROM OLM_AudiBE_Transactions t
	LEFT JOIN OLM_AudiBE_Versions v ON t.ModelId = v.ModelId AND v.IsActive = 1
	LEFT JOIN OLM_AudiBE_VersionGrades vg ON v.Id = vg.VersionId AND vg.IsActive = 1
	INNER JOIN dbo.OLM_AudiBE_Grades as g on g.Id = vg.GradeId AND g.IsActive = 1 
	WHERE t.Id=@TransactionId
	
END
