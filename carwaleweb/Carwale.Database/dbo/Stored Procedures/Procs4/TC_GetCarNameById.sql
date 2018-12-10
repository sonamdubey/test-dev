IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCarNameById]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCarNameById]
GO

	
CREATE PROCEDURE [dbo].[TC_GetCarNameById] @VersionId INT = NULL
	,@ModelId INT = NULL
	,@VersionName VARCHAR = NULL
	,@ApplicationId TINYINT = 1
	,@CarDetails VARCHAR(150) OUTPUT
AS
BEGIN
	IF (@VersionId IS NULL)
	BEGIN
		SELECT TOP 1 @VersionId = V.ID
		FROM CarVersions V WITH (NOLOCK)
		WHERE V.NAME LIKE '%version%'
	END

	--SELECT TOP 1 @VersionId = V.ID
	--FROM CarVersions V WITH (NOLOCK)
	--WHERE V.CarModelId = 
	--AND V.IsDeleted = 0 --AND V.New=1 
	--AND V.Futuristic = 0
	SELECT @CarDetails = V.Make + ' ' + V.Model + ' ' + V.Version + ' ' 
	FROM vwAllMMV V WITH(NOLOCK)
	WHERE (
			@VersionId IS NULL
			OR V.VersionId = @VersionId
			)
		AND (
			@ModelId IS NULL
			OR V.ModelId = @ModelId
			)
		AND V.ApplicationId = ISNULL(@ApplicationId, 1)

END


