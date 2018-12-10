IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewsPhotoGallery]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewsPhotoGallery]
GO

	--===========================
--created on 7/7/2014
-- created by: Natesh Kumar
-- Description: get Images based on tags.
--Approved by Manish on 10-07-2014 05:30 pm
-- Modified by Natesh on 22-7-2014 added search based on multiple tags 
 -- Avishkar 22-07-2014 Added desc to show latest photos
 -- modeified by natesh kumar on 5/12/14 for showallery flag check
--===============================
CREATE PROCEDURE [dbo].[GetNewsPhotoGallery]
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
			CB.Id
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

SELECT TOP 20 HostUrl,ImagePathLarge,ImagePathMedium,ImagePathThumbnail,ImageId,ImageCaption,Id
from CTE
order by LastUpdatedTime  desc  -- Avishkar Added desc to show latest photos


END

