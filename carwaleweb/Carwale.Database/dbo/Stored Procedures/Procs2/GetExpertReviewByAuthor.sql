IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetExpertReviewByAuthor]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetExpertReviewByAuthor]
GO

	
-- =============================================
-- Author:		Shalini 
-- Create date: 29/07/14
-- Description:	Gets the list of Expert Reviews based on AuthorId passed 
-- =============================================
CREATE PROCEDURE [dbo].[GetExpertReviewByAuthor] -- exec GetExpertReviewByAuthor 80,1
	-- Add the parameters for the stored procedure here
	@AuthorId INT,
	@ApplicationId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	--SELECT 
	--TOP 5 CB.Title
	--	,CB.Url
	--	,CMO.MaskingName
	--	,Cma.NAME AS MakeName
	--	,CB.CategoryId
	--	,CB.Id AS BasicId
	--FROM Con_EditCms_Basic AS CB WITH (NOLOCK)
	--left JOIN Con_EditCms_Cars CC with(nolock) ON CB.Id= CC.BasicId  
	--	AND CC.IsActive = 1
	--left JOIN CarModels Cmo ON Cmo.ID = CC.ModelId
	--left JOIN CarMakes Cma ON Cma.ID = Cmo.CarMakeId
	--	where CB.CategoryId IN (2,8)
	--	AND CB.ApplicationID = @ApplicationId
	--	AND CB.AuthorId = @AuthorId
	--	AND CB.IsPublished = 1
	--	AND CB.IsActive = 1
	--ORDER BY CB.DisplayDate DESC

	WITH CTE
AS
(
SELECT 
	TOP 5 CB.Title
		,CB.Url
		,CMO.MaskingName
		,Cma.NAME AS MakeName
		,CB.CategoryId
		,CB.Id AS BasicId
		,CB.DisplayDate
		,ROW_NUMBER() OVER(PARTITION BY CB.Id ORDER BY CB.DisplayDate DESC,CC.LastUpdatedTime DESC) AS ROW
	FROM Con_EditCms_Basic AS CB WITH (NOLOCK)
	left JOIN Con_EditCms_Cars CC WITH(NOLOCK) ON CB.Id= CC.BasicId  AND CC.IsActive = 1
	left JOIN CarModels Cmo ON Cmo.ID = CC.ModelId
	left JOIN CarMakes Cma ON Cma.ID = Cmo.CarMakeId
		where CB.CategoryId IN (2,8)
		AND CB.ApplicationID = @ApplicationId
		AND CB.AuthorId = @AuthorId
		AND CB.IsPublished = 1
		AND CB.IsActive = 1
)
SELECT Title
		,Url
		,MaskingName
		,MakeName
		,CategoryId
		,BasicId
FROM CTE
WHERE row=1
order by DisplayDate desc
END

