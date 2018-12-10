IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTopFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTopFeatures]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 25.09.2013
-- Description:	Get the top features 
-- =============================================
CREATE PROCEDURE GetTopFeatures
@Top int
AS
BEGIN
Select top (@Top) CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, 
CB.Title, CB.Url, CEI.HostURL, CEI.ImagePathThumbnail, CEI.ImagePathLarge 
From Con_EditCms_Basic AS CB Left Join Con_EditCms_Images CEI 
On CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1 
Where
CB.IsActive = 1 AND CB.IsPublished = 1 AND CB.CategoryId = 6
Order By EntryDate desc	
END

