IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SCMapVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SCMapVersion]
GO

	CREATE PROCEDURE [dbo].[OLM_SCMapVersion]
(
@VersionId INT,
@MapVersionId INT
)
AS
BEGIN
   SELECT * FROM OLM_SCMappedVersions WHERE VersionId=@VersionId
   IF @@ROWCOUNT=0
	   BEGIN
	   INSERT INTO OLM_SCMappedVersions(VersionId,MappedVersionId) VALUES(@VersionId,@MapVersionId)
	   END
   ELSE
       BEGIN 
       
       UPDATE OLM_SCMappedVersions SET MappedVersionId = @MapVersionId
       WHERE VersionId=@VersionId
       
       END 
END 