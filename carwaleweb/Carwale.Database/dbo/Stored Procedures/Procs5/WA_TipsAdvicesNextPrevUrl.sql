IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_TipsAdvicesNextPrevUrl]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_TipsAdvicesNextPrevUrl]
GO

	CREATE PROCEDURE [dbo].[WA_TipsAdvicesNextPrevUrl]
@SubCatId INT = NULL,
@BasicId INT
,@ApplicationId INT
AS
--Author: Rakesh Yadav
--Date Created: 05 March 2014
--Desc: Get next and prev basic id of tips and advices
--removed by natesh kumar carwale_com.dbo.con_editcms_basioc to con_editcms_basic and added application id 
BEGIN

--next
SELECT TOP 1 CB.Id AS NextBasicId, CB.AuthorName , CB.Description , CB.DisplayDate , CB.Views, CB.Title, CB.Url 
FROM Con_EditCms_Basic AS CB 
LEFT JOIN Con_EditCms_BasicSubCategories CEB On CEB.BasicId=CB.Id 
WHERE CB.CategoryId = 5 AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CB.ApplicationID=@ApplicationId AND CEB.SubCategoryId NOT IN(21,22,33) AND (@SubCatId IS NULL OR CEB.SubCategoryId=@SubCatId)
AND CB.DisplayDate < (SELECT CB1.DisplayDate FROM Con_EditCms_Basic AS CB1 where CB1.Id=@BasicId AND CB1.ApplicationID=@ApplicationId) ORDER BY CB.DisplayDate DESC
--prev
SELECT TOP 1 CB.Id AS PrevBasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url 
FROM Con_EditCms_Basic AS CB 
LEFT JOIN Con_EditCms_BasicSubCategories CEB On CEB.BasicId=CB.Id 
WHERE CB.CategoryId = 5 AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CB.ApplicationID=@ApplicationId AND CEB.SubCategoryId NOT IN(21,22,33) AND (@SubCatId IS NULL OR CEB.SubCategoryId=@SubCatId)
AND CB.DisplayDate > (SELECT CB1.DisplayDate FROM Con_EditCms_Basic AS CB1 where CB1.Id=@BasicId AND CB1.ApplicationID=@ApplicationId) ORDER BY CB.DisplayDate ASC

END



