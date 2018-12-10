IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_RemoveLatest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_RemoveLatest]
GO
	CREATE PROCEDURE [dbo].[WA_RemoveLatest]

@VersionIds Varchar(50)

--Author: Vinay Kumar Praajapati
--Date Created: 19 Jun 2014
--Desc: Remove Latest android app versions 

AS
BEGIN
	UPDATE WA_AndroidAppVersions
	SET IsLatest = 0 WHERE VersionId IN ( SELECT * FROM dbo.fnSplitCSV(@VersionIds)) 

END