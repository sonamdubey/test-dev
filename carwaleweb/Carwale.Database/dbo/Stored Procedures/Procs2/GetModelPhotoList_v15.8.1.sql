IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelPhotoList_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelPhotoList_v15]
GO

	--------------------------------------------------------
-- Created By : Sadhana Upadhyay on 2 Sept 2014
-- Summary : To get list of photos by categoryId and modelid
-- exec GetModelPhotoList 7, '10,8', 1
--EXEC [dbo] .[GetModelPhotoList] 7,'1,2,3,4,5,6,7,8,9,10,11,12',2
-- exec [GetModelPhotoList_v15.8.1] 458,10,1
--modified by natesh kumar on 27/11/14 for is active flag
--------------------------------------------------------
CREATE PROCEDURE [dbo].[GetModelPhotoList_v15.8.1] @ModelId INT
	,@CategoryId VARCHAR(50)
	,@ApplicationId TINYINT
AS
BEGIN
	IF (@ApplicationId = 1)
	BEGIN
		SELECT IMG.Id AS ImageId
			,PC.MainCategory AS MainCategoryId
			,PC.NAME AS ImageCategory
			,IMG.Caption
			,IMG.ImageName
			,IMG.AltImageName
			,IMG.Title AS ImageTitle
			,IMG.Description AS ImageDescription
			,IMG.HostUrl
			--,IMG.ImagePathLarge
		    --,IMG.ImagePathThumbnail
			,IMG.OriginalImgPath
			,MK.Id AS MakeId
			,MK.NAME AS MakeName
			,MO.ID AS ModelId
			,MO.NAME AS ModelName
			,MO.MaskingName AS ModelMaskingName
			,img.Sequence
		FROM Con_EditCms_Images AS IMG WITH (NOLOCK)
		INNER JOIN Con_PhotoCategory PC WITH (NOLOCK) ON PC.Id = IMG.ImageCategoryId
		INNER JOIN Con_EditCms_Basic AS BA WITH (NOLOCK) ON BA.Id = IMG.BasicId
			AND BA.ApplicationID = @ApplicationId
		INNER JOIN fnSplitCSV(@CategoryId) AS FN ON FN.ListMember = BA.CategoryId
		INNER JOIN CarModels Mo WITH (NOLOCK) ON MO.ID = IMG.ModelId
		INNER JOIN CarMakes MK WITH (NOLOCK) ON MK.ID = MO.CarMakeId
		WHERE BA.ApplicationId = @ApplicationId
			AND BA.IsPublished = 1
			AND BA.IsActive = 1
			AND IMG.IsActive = 1
			AND IMG.ModelId = @ModelId
			AND Mo.IsDeleted = 0
			AND MK.IsDeleted = 0
		ORDER BY IMG.Sequence DESC
			,IMG.Id DESC
	END
	ELSE
		IF (@ApplicationId = 2)
		BEGIN
			SELECT IMG.Id AS ImageId
				,PC.MainCategory AS MainCategoryId
				,PC.NAME AS ImageCategory
				,IMG.Caption
				,IMG.ImageName
				,IMG.AltImageName
				,IMG.Title AS ImageTitle
				,IMG.Description AS ImageDescription
				,IMG.HostUrl
				--,IMG.ImagePathLarge
				--,IMG.ImagePathThumbnail
				,IMG.OriginalImgPath
				,MK.Id AS MakeId
				,MK.NAME AS MakeName
				,MO.ID AS ModelId
				,MO.NAME AS ModelName
				,MO.MaskingName AS ModelMaskingName
			FROM Con_EditCms_Images AS IMG WITH (NOLOCK)
			INNER JOIN Con_PhotoCategory PC WITH (NOLOCK) ON PC.Id = IMG.ImageCategoryId
			INNER JOIN Con_EditCms_Basic AS BA WITH (NOLOCK) ON BA.Id = IMG.BasicId
				AND BA.ApplicationID = @ApplicationId
			INNER JOIN fnSplitCSV(@CategoryId) AS FN ON FN.ListMember = BA.CategoryId
			INNER JOIN BikeModels Mo WITH (NOLOCK) ON MO.ID = IMG.ModelId
			INNER JOIN BikeMakes MK WITH (NOLOCK) ON MK.ID = MO.BikeMakeId
			WHERE BA.ApplicationId = @ApplicationId
				AND BA.IsPublished = 1
				AND BA.IsActive = 1
				AND IMG.IsActive = 1
				AND IMG.ModelId = @ModelId
				AND Mo.IsDeleted = 0
				AND MK.IsDeleted = 0
			ORDER BY IMG.Sequence DESC
				,IMG.Id DESC
		END
END

