IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_SaveVersionPQAnalysis]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_SaveVersionPQAnalysis]
GO

	CREATE PROCEDURE [dbo].[AP_SaveVersionPQAnalysis]
	@CarVersionId		NUMERIC,
	@PQCarVersions		VarChar(100),
	@LastUpdated		DateTime
 AS
	
BEGIN

	UPDATE AP_VersionPQAnalysis 
	SET PQCarVersions = @PQCarVersions, LastUpdated = @LastUpdated
	WHERE CarVersionId = @CarVersionId
				
	IF @@RowCount = 0
		BEGIN
			INSERT INTO AP_VersionPQAnalysis
					(CarVersionId, PQCarVersions, LastUpdated) 
			VALUES	(@CarVersionId, @PQCarVersions, @LastUpdated)
		END	
END




