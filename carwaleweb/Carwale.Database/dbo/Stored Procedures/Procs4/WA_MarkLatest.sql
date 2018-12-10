IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_MarkLatest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_MarkLatest]
GO
	CREATE PROCEDURE [dbo].[WA_MarkLatest]

@VersionIds Varchar(50)

--Author: Vinay Kumar Praajapati
--Date Created: 19 Jun 2014
--Desc: Mark android app versions Latest

AS
BEGIN
	UPDATE WA_AndroidAppVersions
	SET IsLatest = 1 WHERE VersionId IN ( SELECT * FROM dbo.fnSplitCSV(@VersionIds)) AND IsSupported=1

END
