IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MarkSupported]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MarkSupported]
GO

	CREATE PROCEDURE [dbo].[MarkSupported]

@VersionIds Varchar(100)

--Author: Rakesh Yadav

--Date Created: 22 jan 2014

--Desc: Mark android app versions as supported

AS

BEGIN

UPDATE AndroidAppVersions

SET IsSupported = 1 WHERE VersionId IN ( SELECT * FROM dbo.fnSplitCSV(@VersionIds))

END