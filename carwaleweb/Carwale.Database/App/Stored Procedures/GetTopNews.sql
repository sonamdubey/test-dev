IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetTopNews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetTopNews]
GO

	CREATE PROCEDURE [App].[GetTopNews] 

AS
BEGIN

       SET NOCOUNT ON;

       SELECT 
       TOP 10 
       CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CB.MainImageSet, CEI.HostUrl, CEI.ImagePathThumbnail,CEI.ImagePathLarge
       FROM 
       Con_EditCms_Basic CB WITH (NOLOCK) LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CB.ID = CEI.BasicId
       WHERE CB.CategoryId = 1 AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CEI.IsMainImage = 1 and CEI.IsActive = 1
       ORDER BY CB.DisplayDate Desc
END
