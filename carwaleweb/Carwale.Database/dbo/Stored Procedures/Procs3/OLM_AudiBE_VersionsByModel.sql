IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_VersionsByModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_VersionsByModel]
GO

	
-- =============================================================================================
-- Author:		Ashish G. Kamble
-- Create date: 28 July 2013
-- Description:	SP to get the versions and specifications of the versions for a particular model
-- Modified By Supriya K on 10/10/2013 to add order by price filter
-- exec OLM_AudiBE_VersionsByModel 133
-- =============================================================================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_VersionsByModel] @TransactionId INTEGER
AS
BEGIN
	SET NOCOUNT ON;

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
		FROM OLM_AudiBE_Transactions t
		LEFT JOIN OLM_AudiBE_Models m ON t.ModelId = m.Id
		LEFT JOIN OLM_AudiBE_Versions v ON m.Id = v.ModelId
		LEFT JOIN OLM_AudiBE_Version_Specs vs ON v.Id = vs.VersionId
		LEFT JOIN OLM_AudiBE_Specs s ON s.Id = vs.SpecId
		LEFT JOIN OLM_AudiBE_VersionGrades VG ON v.Id = VG.VersionId
		WHERE v.IsActive = 1
			AND vs.IsActive = 1
			AND VG.IsActive = 1
			AND t.Id = @TransactionId
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
END
