IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetRoadTestPage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetRoadTestPage]
GO

	
-- ==============================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching Road Test details 
-- ==============================================

CREATE PROCEDURE [App].[GetRoadTestPage]
	@BAsicId Integer
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT
		CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CB.MainImageSet, CB.HostURL, CB.ShowGallery, 
		Cmo.Name As ModelName, Cma.Name As MakeName, CV.Name AS VersionName, SC.Name As SubCategory 
	FROM
		Con_EditCms_Basic AS CB 
		Join Con_EditCms_Cars CC 
		On CC.BasicId = CB.Id And CC.IsActive = 1 
		Join CarModels Cmo 
		On Cmo.ID = CC.ModelId 
		Join CarMakes Cma 
		 On Cma.ID = CC.MakeId
		Left Join CarVersions CV
		On CC.VersionId = CV.id 
		Left Join Con_EditCms_BasicSubCategories BSC 
		 On BSC.BasicId = CB.Id 
		Left Join Con_EditCms_SubCategories SC 
		On SC.Id = BSC.SubCategoryId And SC.IsActive = 1
	WHERE
		CB.ID = @BasicId
 
END

