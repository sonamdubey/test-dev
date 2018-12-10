IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_VersionsByFuelType_Specs]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_VersionsByFuelType_Specs]
GO

	
-- =============================================================================================
-- Author:		Supriya Khartode
-- Create date: 26/7/2013
-- Description:	To fetch only versions for fueltype selected in transaction & its all specifications
-- Modified by Supriya K on 10/9/2013 to add isActive filter
-- Modified By Supriya K on 10/10/2013 to add order by price filter
-- =============================================================================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_VersionsByFuelType_Specs] @TransactionId INTEGER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT uvw.VersionId
		,uvw.VersionName
		,s.NAME
		,vs1.Value
	FROM (
		SELECT v.NAME AS VersionName
			,v.Id AS VersionId
			,MAX(VG.GradeId) AS GradeId
		FROM OLM_AudiBE_Transactions t
		LEFT JOIN OLM_AudiBE_Models m ON t.ModelId = m.Id
			AND m.IsActive = 1
		LEFT JOIN OLM_AudiBE_Versions v ON m.Id = v.ModelId
			AND v.IsActive = 1
		LEFT JOIN OLM_AudiBE_Version_Specs vs ON v.Id = vs.VersionId
			AND vs.IsActive = 1
		LEFT JOIN OLM_AudiBE_VersionGrades VG ON v.Id = VG.VersionId
			AND VG.IsActive = 1
		WHERE vs.SpecId = 1
			AND vs.Value = CONVERT(VARCHAR, t.FuelTypeId)
			AND v.IsActive = 1
			AND t.Id = @TransactionId
		GROUP BY v.NAME
			,v.Id
		) uvw
	LEFT JOIN OLM_AudiBE_Version_Specs vs1 ON uvw.VersionId = vs1.VersionId
	LEFT JOIN OLM_AudiBE_Specs s ON vs1.SpecId = s.Id
		AND s.IsActive = 1
	LEFT JOIN OLM_AudiBE_VersionPrices vp ON uvw.VersionId = vp.VersionId
		AND vp.IsActive = 1
		AND vp.StateId = 1
		AND vp.GradeId = uvw.GradeId
	WHERE vs1.SpecId <> 1
	ORDER BY vp.Price
		,vs1.SpecId
END
