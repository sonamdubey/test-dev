IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[getNews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[getNews]
GO

	
-- =============================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching Top 10 News 
-- =============================================

CREATE PROCEDURE [App].[getNews]

AS
BEGIN

	SET NOCOUNT ON;
	SELECT TOP 10
	CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CB.MainImageSet, CEI.HostURL, CEI.ImagePathThumbnail,CEI.ImagePathLarge
	FROM Con_EditCms_Basic CB, Con_EditCms_Images CEI
	WHERE CB.CategoryId = 1 AND CB.IsActive = 1 AND CB.IsPublished = 1 
	AND CB.Id = CEI.BasicId AND CEI.IsMainImage = 1 AND CEI.IsActive = 1 
	ORDER BY CB.DisplayDate Desc
END

