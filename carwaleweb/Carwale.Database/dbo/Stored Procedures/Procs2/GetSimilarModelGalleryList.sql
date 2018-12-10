IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSimilarModelGalleryList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSimilarModelGalleryList]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 3 Sept 2014
-- Description:	Proc to get the similar models gallery list
--EXEC GetSimilarModelGalleryList 1,1, 2, 504, '1,2,8'
-- Modified By : Sadhana Upadhyay on 9 Sept 2014
-- Description : To get similar models gallery list count
-- Modified by : NAtesh Kumar on 7/10/14 for required no. of list
-- modified by natesh kumar on 27/11/14 for is active flag
-- ============================================= 
CREATE PROCEDURE [dbo].[GetSimilarModelGalleryList]
	-- Add the parameters for the stored procedure here
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

DECLARE @SimilarModels VARCHAR(MAX)

SELECT @SimilarModels = SCM.SimilarModels FROM SimilarCarModels SCM WHERE SCM.ModelId = @ModelId

IF(@ApplicationId = 1)
BEGIN	
	SELECT COUNT(*) AS recordCount FROM CarModels as Mo WITH(NOLOCK)
		INNER JOIN CarMakes as Ma WITH(NOLOCK) ON Ma.ID= MO.CarMakeId
		INNER JOIN
		(
		SELECT DISTINCT CEI.ModelId
		FROM Con_EditCms_Basic CB WITH(NOLOCK) 
		INNER JOIN [dbo].[fnSplitCSV](@CategoryId) AS FN ON FN.ListMember = CB.CategoryId
		INNER JOIN Con_EditCms_Images CEI WITH(NOLOCK) ON CEI.BasicId = CB.Id
		INNER JOIN [dbo].[fnSplitCSV](@SimilarModels) AS SM ON SM.ListMember = CEI.ModelId				
		WHERE CEI.IsActive = 1 AND CB.IsPublished=1 AND CB.IsActive = 1 AND CB.ApplicationID = @ApplicationId
		) AS tmp ON tmp.ModelId = Mo.ID
		WHERE Ma.IsDeleted = 0 AND Mo.IsDeleted = 0 AND Ma.New = 1 AND Mo.New = 1

	SELECT ModelId,MakeId, MakeName, ModelName, ModelMaskingName,
				HostURL, SmallPic, LargePic FROM (
		SELECT Mo.ID AS ModelId,Ma.ID AS MakeId, Ma.Name AS MakeName, Mo.Name AS ModelName, Mo.MaskingName AS ModelMaskingName,
				Mo.HostURL, Mo.SmallPic, Mo.LargePic, ROW_NUMBER() OVER (ORDER BY MO.MinPrice) RowNum
		FROM CarModels as Mo WITH(NOLOCK)
		INNER JOIN CarMakes as Ma WITH(NOLOCK) ON Ma.ID= MO.CarMakeId
		INNER JOIN
		(
		SELECT DISTINCT CEI.ModelId, SM.ListMember 
		FROM Con_EditCms_Basic CB WITH(NOLOCK) 
		INNER JOIN [dbo].[fnSplitCSV](@CategoryId) AS FN ON FN.ListMember = CB.CategoryId
		INNER JOIN Con_EditCms_Images CEI WITH(NOLOCK) ON CEI.BasicId = CB.Id
		INNER JOIN [dbo].[fnSplitCSV](@SimilarModels) AS SM ON SM.ListMember = CEI.ModelId				
		WHERE CEI.IsActive = 1 AND CB.IsPublished=1 AND CB.IsActive = 1 AND CB.ApplicationID = @ApplicationId
		) AS tmp ON tmp.ModelId = Mo.ID
		WHERE Ma.IsDeleted = 0 AND Mo.IsDeleted = 0 AND Ma.New = 1 AND Mo.New = 1
	) AS CTE WHERE RowNum BETWEEN @StartIndex AND @EndIndex
END



END



