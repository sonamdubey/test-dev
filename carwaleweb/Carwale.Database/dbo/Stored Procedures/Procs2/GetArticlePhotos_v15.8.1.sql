IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetArticlePhotos_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetArticlePhotos_v15]
GO

	--================================================================
-- Author: ASHWINI TODKAR ON 26 Sept 2014
--Description: Proc to get article photos 
-- Altered by : Ashish G. Kamble
-- Modified : Added more select parameters to the query.
-- Modified By : Sadhana Upadhyay on 20 Oct 2014
-- Summary : cast null to 0

-- Modified by Satish Sharma On 23/Jul/2015, Image dynamic resize revamp
--================================================================
-- exec dbo.GetArticlePhotos 14129
CREATE PROCEDURE [dbo].[GetArticlePhotos_v15.8.1]
@BasicId INT
AS 
BEGIN
	DECLARE @AppId TINYINT 

	SELECT @AppId = ApplicationId FROM Con_EditCms_Basic WITH (NOLOCK) WHERE ID = @BasicId

	IF @@ROWCOUNT > 0
	BEGIN
		IF ( @AppId = 1 )
			BEGIN
				SELECT 
					CI.Id AS ImageId
					, CI.ImageName
					,CI.Title,
					CP.NAME AS CategoryName
					,ISNULL(Cma.ID,0) AS MakeId
					,Cma.NAME AS MakeName
					,ISNULL(Cmo.ID,0) AS ModelId
					,Cmo.NAME AS ModelName
					,Cmo.MaskingName AS ModelMaskingName
					,CI.HostUrl
					--,CI.ImagePathLarge
					--,CI.ImagePathThumbnail
					,CI.OriginalImgPath
					,CI.Caption
					,CI.AltImageName
					,CI.Description				
				FROM Con_EditCms_Images CI WITH (NOLOCK)
				INNER JOIN Con_PhotoCategory CP WITH (NOLOCK) ON CP.Id = CI.ImageCategoryId
				LEFT JOIN CarModels Cmo WITH (NOLOCK) ON Cmo.Id = CI.ModelId
				LEFT JOIN CarMakes Cma WITH (NOLOCK) ON Cma.Id = CI.MakeId
				WHERE IsActive = 1 AND BasicId = @BasicId
				ORDER BY CI.Sequence
			END
		ELSE 
			BEGIN
				SELECT CI.Id AS ImageId
					, CI.ImageName
					,CI.Title,
					CP.NAME AS CategoryName
					,ISNULL(Cma.ID,0) AS MakeId
					,Cma.NAME AS MakeName
					,ISNULL(Cmo.ID,0) AS ModelId
					,Cmo.NAME AS ModelName
					,Cmo.MaskingName AS ModelMaskingName
					,CI.HostUrl
					--,CI.ImagePathLarge
					--,CI.ImagePathThumbnail
					,CI.OriginalImgPath
					,CI.Caption
					,CI.AltImageName
					,CI.Description
				FROM Con_EditCms_Images CI WITH (NOLOCK)
				INNER JOIN Con_PhotoCategory CP WITH (NOLOCK) ON CP.Id = CI.ImageCategoryId
				LEFT JOIN BikeModels Cmo WITH (NOLOCK) ON Cmo.Id = CI.ModelId
				LEFT JOIN BikeMakes Cma WITH (NOLOCK) ON Cma.Id = CI.MakeId
				WHERE IsActive = 1 AND BasicId = @BasicId
				ORDER BY CI.Sequence
			END
		END
END
