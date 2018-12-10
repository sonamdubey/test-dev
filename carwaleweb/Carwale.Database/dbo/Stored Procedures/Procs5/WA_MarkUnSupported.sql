IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_MarkUnSupported]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_MarkUnSupported]
GO

	
CREATE PROCEDURE [dbo].[WA_MarkUnSupported] @VersionIds VARCHAR(50)
	--Author: Rakesh Yadav
	--Date Created: 22 jan 2014
	--Desc: Mark android app versions un supported
AS
BEGIN
	UPDATE WA_AndroidAppVersions
	SET IsSupported = 0
	WHERE VersionId IN (
			SELECT *
			FROM dbo.fnSplitCSV(@VersionIds)
			)
END
