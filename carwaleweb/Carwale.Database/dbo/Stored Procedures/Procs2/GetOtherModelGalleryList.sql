IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOtherModelGalleryList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOtherModelGalleryList]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 3 Sept 2014
-- Description:	Proc to get the Other models gallery list
-- EXEC GetOtherModelGalleryList 1, 1, 5, 504, '1,2,3,4'
-- Modified By : Sadhana Upadhyay on 9 Sept 2014
-- Description : To get Other models gallery list Count
-- Modified by : NAtesh Kumar on 7/10/14 for required no. of list
-- modified by natesh kumar on 27/11/14 for is active flag
-- =============================================
CREATE PROCEDURE [dbo].[GetOtherModelGalleryList]
	@ApplicationId TINYINT,
	@StartIndex INT,
	@EndIndex INT,
	@ModelId INT,
	@CategoryId VARCHAR(50)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @MakeId INT

	IF(@ApplicationId = 1)
	BEGIN
		SELECT @MakeId = CarMakeId FROM CarModels WHERE Id = @ModelId

		SELECT COUNT(*) AS recordCount FROM CarModels as Mo WITH(NOLOCK)
			INNER JOIN CarMakes as Ma WITH(NOLOCK) ON Ma.ID= MO.CarMakeId
			INNER JOIN
			(
			SELECT DISTINCT CEI.ModelId 
			FROM Con_EditCms_Basic CB WITH(NOLOCK) 
			INNER JOIN Con_EditCms_Images CEI WITH(NOLOCK) ON CEI.BasicId = CB.Id
			INNER JOIN [dbo].[fnSplitCSV](@CategoryId) AS FN  ON FN.ListMember=CB.CategoryId
			WHERE CEI.IsActive = 1 AND CB.IsPublished=1 AND CB.IsActive = 1  AND CEI.MakeId = @MakeId AND CEI.ModelId <> @ModelId AND CB.ApplicationID = @ApplicationId
			) AS tmp ON tmp.ModelId = Mo.ID
			WHERE Ma.IsDeleted = 0 AND Mo.IsDeleted = 0 AND (Ma.New = 1 OR Ma.Futuristic = 1) AND (Mo.New = 1 OR Mo.Futuristic = 1)

		SELECT * FROM (
			SELECT Ma.Name AS MakeName, Mo.Name AS ModelName, Mo.MaskingName AS ModelMaskingName,Ma.ID AS MakeId,Mo.ID AS ModelId,
					Mo.HostURL, Mo.SmallPic, Mo.LargePic, ROW_NUMBER() OVER ( ORDER BY MO.MinPrice) RowNum
			FROM CarModels as Mo WITH(NOLOCK)
			INNER JOIN CarMakes as Ma WITH(NOLOCK) ON Ma.ID= MO.CarMakeId
			INNER JOIN
			(
			SELECT DISTINCT CEI.ModelId 
			FROM Con_EditCms_Basic CB WITH(NOLOCK) 
			INNER JOIN Con_EditCms_Images CEI WITH(NOLOCK) ON CEI.BasicId = CB.Id
			INNER JOIN [dbo].[fnSplitCSV](@CategoryId) AS FN ON FN.ListMember=CB.CategoryId
			WHERE CEI.IsActive = 1 AND CB.IsPublished=1 AND CB.IsActive = 1 AND CEI.MakeId = @MakeId AND CEI.ModelId <> @ModelId AND CB.ApplicationID = @ApplicationId
			) AS tmp ON tmp.ModelId = Mo.ID
			WHERE Ma.IsDeleted = 0 AND Mo.IsDeleted = 0 AND (Ma.New = 1 OR Ma.Futuristic = 1) AND (Mo.New = 1 OR Mo.Futuristic = 1)
		) AS CTE WHERE RowNum BETWEEN @StartIndex AND @EndIndex
	END

	ELSE IF(@ApplicationId = 2)
	BEGIN
		SELECT @MakeId = BikeMakeId FROM BikeModels WITH(NOLOCK) WHERE Id = @ModelId

		SELECT COUNT(*) AS recordCount FROM BikeModels as Mo WITH(NOLOCK)
			INNER JOIN BikeMakes as Ma WITH(NOLOCK) ON Ma.ID= MO.BikeMakeId
			INNER JOIN
			(
			SELECT DISTINCT CEI.ModelId
			FROM Con_EditCms_Basic CB WITH (NOLOCK)
			INNER JOIN Con_EditCms_Images CEI WITH(NOLOCK) ON CEI.BasicId = CB.Id
			INNER JOIN [dbo].[fnSplitCSV](@CategoryId) AS FN ON FN.ListMember=CB.CategoryId
			WHERE CEI.IsActive = 1 AND CB.IsPublished=1 AND CB.IsActive = 1  AND CEI.MakeId = @MakeId AND CEI.ModelId <> @ModelId and CB.ApplicationID = @ApplicationId
			) AS tmp ON tmp.ModelId = Mo.ID
			WHERE Ma.IsDeleted = 0 AND Mo.IsDeleted = 0 AND (Ma.New = 1 OR Ma.Futuristic = 1) AND (Mo.New = 1 OR Mo.Futuristic = 1)

		SELECT * FROM (
			SELECT Ma.Name AS MakeName, Mo.Name AS ModelName, Mo.MaskingName AS ModelMaskingName,Ma.ID AS MakeId,Mo.ID AS ModelId,
					Mo.HostURL, Mo.SmallPic, Mo.LargePic, ROW_NUMBER() OVER (ORDER BY MO.MinPrice) RowNum
			FROM BikeModels as Mo WITH(NOLOCK)
			INNER JOIN BikeMakes as Ma WITH(NOLOCK) ON Ma.ID= MO.BikeMakeId
			INNER JOIN
			(
			SELECT DISTINCT CEI.ModelId
			FROM Con_EditCms_Basic CB WITH(NOLOCK) 
			INNER JOIN Con_EditCms_Images CEI WITH(NOLOCK) ON CEI.BasicId = CB.Id
			INNER JOIN [dbo].[fnSplitCSV](@CategoryId) AS FN ON FN.ListMember=CB.CategoryId
			WHERE CEI.IsActive = 1 AND CB.IsPublished=1 AND CB.IsActive = 1 AND CEI.MakeId = @MakeId AND CEI.ModelId <> @ModelId and CB.ApplicationID = @ApplicationId
			) AS tmp ON tmp.ModelId = Mo.ID
			WHERE Ma.IsDeleted = 0 AND Mo.IsDeleted = 0 AND (Ma.New = 1 OR Ma.Futuristic = 1) AND (Mo.New = 1 OR Mo.Futuristic = 1)
		) AS CTE WHERE RowNum BETWEEN @StartIndex AND @EndIndex
	END


END



