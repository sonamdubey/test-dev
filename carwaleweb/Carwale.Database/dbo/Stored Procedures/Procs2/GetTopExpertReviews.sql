IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTopExpertReviews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTopExpertReviews]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 4.10.2013
-- Description:	Get All Expert Reviews or Get Expert Reviews Specific to a make
-- Modififed By : Akansha on 6.2.2014
-- Description : Added Masking Name column
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- =============================================
CREATE PROCEDURE [dbo].[GetTopExpertReviews] 
@Top int = 3,
@MakeId int = 0
,@ApplicationId int
AS
BEGIN
	IF (@MakeId = 0)
	BEGIN
		SELECT TOP (@Top)
		CB.Id AS BasicId, 
		CB.AuthorName, 
		CB.Description, 
		CB.DisplayDate, 
		CB.Views, 
		CB.Title, 
		CB.Url, 
		CEI.IsMainImage, 
		CEI.HostURL, 
		CEI.ImagePathThumbnail, 
		CEI.ImagePathLarge, 
		Cmo.Name As ModelName, 
		Cma.Name As MakeName, 
		SC.Name As SubCategory,
		SC.CategoryId AS CategoryId,
		Cmo.MaskingName
		FROM 
		Con_EditCms_Basic AS CB Join Con_EditCms_Cars CC On CC.BasicId = CB.Id And CC.IsActive = 1 
		Join CarModels Cmo On Cmo.ID = CC.ModelId 
		Join CarMakes Cma On Cma.ID = CC.MakeId 
		Left Join Con_EditCms_Images CEI On CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1 
		Left Join Con_EditCms_BasicSubCategories BSC On BSC.BasicId = CB.Id 
		Left Join Con_EditCms_SubCategories SC On SC.Id = BSC.SubCategoryId And SC.IsActive = 1
		Left Join Con_EditCms_Category CCat On SC.CategoryId=CCat.Id
		WHERE CCat.Id In (2,8) AND CB.IsActive = 1 AND CB.IsPublished = 1 And CB.IsFeatured=1 AND CB.ApplicationID = @ApplicationId
		ORDER BY DisplayDate DESC
	END
	ELSE
	BEGIN
	SELECT TOP (@Top) 
	CB.Id AS BasicId, 
	CB.AuthorName, 
	CB.Description, 
	CB.DisplayDate, 
	CB.Views, 
	CB.Title, 
	CB.Url, 
	CEI.IsMainImage, 
	CEI.HostURL, 
	CEI.ImagePathThumbnail, 
	CEI.ImagePathLarge, 
	Cmo.Name As ModelName, 
	Cma.Name As MakeName, 
	SC.Name As SubCategory,
	SC.CategoryId AS CategoryId,
	Cmo.MaskingName
	FROM Con_EditCms_Basic AS CB Join Con_EditCms_Cars CC On CC.BasicId = CB.Id And CC.IsActive = 1 
	Join CarModels Cmo On Cmo.ID = CC.ModelId 
	Join CarMakes Cma On Cma.ID = CC.MakeId 
	Left Join Con_EditCms_Images CEI On CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1
	Left Join Con_EditCms_BasicSubCategories BSC On BSC.BasicId = CB.Id 
	Left Join Con_EditCms_SubCategories SC On SC.Id = BSC.SubCategoryId And SC.IsActive = 1
	Left Join Con_EditCms_Category CCat On SC.CategoryId=CCat.Id
	WHERE CCat.Id In (2,8) AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CC.MakeId = @MakeId And CB.IsFeatured=1 AND CB.ApplicationID = @ApplicationId
	ORDER BY DisplayDate DESC
	END
END


