IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewsPhotoGallery_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewsPhotoGallery_15]
GO

	CREATE PROCEDURE [dbo].[GetNewsPhotoGallery_15.8.1] --'alto',1
@Tags VARCHAR(MAX),
@ApplicationId INT
AS

BEGIN

	with CTE
	As(	
	SELECT distinct 
			CEI.HostUrl AS HostUrl,
			CEI.ImagePathLarge AS ImagePathLarge,
			CEI.ImagePathThumbnail AS ImagePathMedium,
			CEI.ImagePathThumbnail AS ImagePathThumbnail,
			CEI.Id AS ImageId,
			CEI.Caption AS ImageCaption,
			CEI.LastUpdatedTime,
			CB.Id,
			CEI.OriginalImgPath
		 FROM 
			Con_EditCms_Tags  CET WITH (NOLOCK)
			INNER JOIN Con_EditCms_BasicTags CEB
			WITH (NOLOCK) ON  CET.Id = CEB.TagId
			INNER JOIN Con_EditCms_Images CEI
			WITH (NOLOCK) ON CEI.BasicId = CEB.BasicId
			INNER JOIN Con_editcms_basic CB 
			WITH (NOLOCK) ON CB.id = CEI.BasicId 
				AND CEI.IsMainImage = 1
				AND CB.CategoryId<>1 
				AND CB.IsPublished = 1
				AND CEI.IsActive = 1
				AND CB.ApplicationID=@ApplicationId
				WHERE CET.Tag IN (select ListMember from fnSplitCSVValuesWithIdentity(@Tags)) 
				AND CB.ShowGallery = 1
	)

SELECT TOP 20 HostUrl,ImagePathLarge,ImagePathMedium,ImagePathThumbnail,ImageId,ImageCaption,Id,OriginalImgPath
from CTE
order by LastUpdatedTime  desc  -- Avishkar Added desc to show latest photos


END
