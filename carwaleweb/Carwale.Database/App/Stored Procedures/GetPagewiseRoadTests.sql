IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetPagewiseRoadTests]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetPagewiseRoadTests]
GO

	
-- =======================================================================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching Road Tests according to MakeId,ModelId,PageSize and PageNo
-- ========================================================================================

CREATE PROCEDURE [App].[GetPagewiseRoadTests]

@MakeId Numeric = -1,
@ModelId Numeric = -1,
@StartIndex Numeric = -1,
@LastIndex Numeric = -1
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	
		
	SELECT (((Row-1)/10)+1) As PageNo,TempView.*
	FROM 
	( 
		SELECT ROW_NUMBER() OVER (ORDER BY DisplayDate Desc) AS Row,
		CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CB.MainImageSet,
		Cmo.Name As ModelName,Cmo.Id As ModelId,Cma.Name As MakeName,Cma.Id As MakeId,SC.Name As SubCategory,
		CEI.HostUrl,CEI.ImagePathThumbnail 
		FROM
		Carwale_com.dbo.Con_EditCms_Basic AS CB 
		Join Carwale_com.dbo.Con_EditCms_Cars CC 
		On CC.BasicId = CB.Id And CC.IsActive = 1
		Left Join Carwale_com.dbo.Con_EditCms_Images CEI
		On CEI.BasicId=CB.Id And CEI.IsMainImage=1 
		Join Carwale_com.dbo.CarModels Cmo 
		On Cmo.ID = CC.ModelId 
		Join Carwale_com.dbo.CarMakes Cma 
		On Cma.ID = CC.MakeId 
		Left Join Carwale_com.dbo.Con_EditCms_BasicSubCategories BSC 
		On BSC.BasicId = CB.Id 
		Left Join Carwale_com.dbo.Con_EditCms_SubCategories SC 
		On SC.Id = BSC.SubCategoryId And SC.IsActive = 1
		WHERE
		CB.CategoryId = 8 AND CB.IsActive = 1 AND CB.IsPublished = 1 
		AND (@MakeId = -1 OR CC.MakeId = @MakeId) 
		AND (@ModelId = -1 OR CC.ModelId = @ModelId)
	) TempView
	WHERE 
	@StartIndex = -1 OR (TempView.Row >= @StartIndex AND TempView.Row <= @LastIndex)	 
END