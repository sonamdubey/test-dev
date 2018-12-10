IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_MarkSupported]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_MarkSupported]
GO

	
CREATE PROCEDURE [dbo].[WA_MarkSupported] @VersionIds VARCHAR(50)
	--Author: Rakesh Yadav
	--Date Created: 22 jan 2014
	--Desc: Mark android app versions as supported
AS
BEGIN
	UPDATE WA_AndroidAppVersions
	SET IsSupported = 1
	WHERE VersionId IN (
			SELECT *
			FROM dbo.fnSplitCSV(@VersionIds)
			)
END
