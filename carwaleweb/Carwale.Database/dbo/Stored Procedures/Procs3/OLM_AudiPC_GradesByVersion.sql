IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiPC_GradesByVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiPC_GradesByVersion]
GO

	-- =======================================================================================================  

-- Author:  Supriya K.  

-- Create date: 22/10/13  

-- Description: To fetch grades of versions for particular modelId ([OLM_AudiBE_GradesByVersion])

-- ========================================================================================================  

CREATE PROCEDURE [dbo].[OLM_AudiPC_GradesByVersion] @ModelId INTEGER

AS

BEGIN

	SET NOCOUNT ON;

	

	SELECT vg.VersionId

		,vg.GradeId

	FROM OLM_AudiBE_Models m 

		

	LEFT JOIN OLM_AudiBE_Versions v ON m.Id = v.ModelId

		AND v.IsActive = 1

	LEFT JOIN OLM_AudiBE_VersionGrades vg ON v.Id = vg.VersionId

		AND vg.IsActive = 1

	LEFT JOIN OLM_AudiBE_Version_Specs vs ON v.id = vs.VersionId

		AND vs.specId = 1

	WHERE m.Id = @ModelId AND m.IsActive = 1 

END
