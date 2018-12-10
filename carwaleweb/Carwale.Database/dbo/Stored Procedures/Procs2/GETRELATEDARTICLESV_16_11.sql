IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GETRELATEDARTICLESV_16_11]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GETRELATEDARTICLESV_16_11]
GO

-- =============================================  
-- Author:  Rohan s
-- Create date: 30 nov 2016
-- Description: Get related articles 

-- =============================================  
CREATE PROCEDURE [dbo].[GETRELATEDARTICLESV_16_11]
--DECLARE
	@basicId INT --=26031
	,@applicationId INT = 1
	,@categorylist VARCHAR(100) ='1,2,6,8,12,19,22'
	,@startIndex Int = NULL
	,@endIndex Int = NULL
AS
BEGIN 
WITH ARTICLES AS (
SELECT distinct cbt.basicid as articleId 
FROM  Con_EditCms_BasicTags CBT WITH(NOLOCK)
INNER JOIN Con_EditCms_BasicTags CBT2 WITH(NOLOCK) ON CBT2.TagId = CBT.TagId AND CBT2.BasicId=@basicId
)
SELECT *
			FROM (
				SELECT cb.id AS basicid
					,cb.categoryid AS categoryid
					,cb.title AS title
					,cb.url AS articleurl
					,CB.displaydate AS displaydate
					,cb.authorname AS authorname
					,cb.authorid
					,cb.description
					,CB.publisheddate
					,cb.VIEWS AS VIEWS
					,(isnull(cb.issticky, 0)) AS issticky
					,(isnull(cb.isfeatured, 0)) AS isfeatured
					,ca.maskingname AS authormaskingname
					,cb.hosturl AS hosturl
					,cb.mainimagepath AS orginalimgpath
					,(isnull(cb.facebookcommentcount, 0)) AS facebookcommentcount
					,cat.NAME AS CategoryMaskingName
					,row_number() OVER (
						ORDER BY cb.displaydate DESC
						) AS row_no
				FROM con_editcms_basic cb WITH (NOLOCK)
				INNER JOIN ARTICLES ON cb.id=articleId
				INNER JOIN Con_EditCms_Category cat WITH (NOLOCK) ON cat.Id = cb.CategoryId
				INNER JOIN con_editcms_author ca WITH (NOLOCK) ON ca.authorid = cb.authorid
				WHERE cb.isactive = 1
					AND cb.ispublished = 1
					AND cb.applicationid = 1
					AND cb.categoryid IN(
						SELECT listmember
						FROM fnsplitcsvvalueswithidentity(@categorylist)
						) --(1,2,6,8,12,19,22)
					AND cb.displaydate <= getdate()
				) CTE
			WHERE (@startIndex IS NULL) OR (row_no between @startIndex AND @endIndex)
			ORDER BY row_no
END
