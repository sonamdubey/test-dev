IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckVersionStatusForApp_V14111]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckVersionStatusForApp_V14111]
GO

	------------------------------------------------
--For checking the version status of app with sourceid and appversionid as input parameter and moving inline query to sp
-- created by Natesh kumar on 6/11/2014
------------------------------------------------
CREATE PROCEDURE [dbo].[CheckVersionStatusForApp_V14111]
@AppVersionId INT,
@SourceId TINYINT

AS
BEGIN

	SELECT 
	VersionId
	,IsSupported
	,IsLatest 
	FROM WA_AndroidAppVersions WITH (NOLOCK)
	WHERE VersionId=@AppVersionId AND ApplicationType = @SourceId

END



