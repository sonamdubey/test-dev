IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MarkUnSupported]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MarkUnSupported]
GO

	CREATE PROCEDURE [dbo].[MarkUnSupported]

@VersionIds Varchar(100)

--Author: Rakesh Yadav

--Date Created: 22 jan 2014

--Desc: Mark android app versions un supported

AS

BEGIN

UPDATE AndroidAppVersions

SET IsSupported = 0 WHERE VersionId IN ( SELECT * FROM dbo.fnSplitCSV(@VersionIds))

END