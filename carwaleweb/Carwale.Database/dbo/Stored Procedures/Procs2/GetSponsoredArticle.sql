IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSponsoredArticle]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSponsoredArticle]
GO

	
--================================================================
-- author: rohan sapkal 7th OCT 2016 
--description: gets Sponsored Article
--================================================================
CREATE PROCEDURE [dbo].[GetSponsoredArticle]
	@categorylist VARCHAR(50)
	,@author INT = 1571
AS
BEGIN
	SET NOCOUNT ON;

SELECT top 1 cb.id AS basicid
			,cb.categoryid AS categoryid
			,cb.title AS title
			,cb.url AS articleurl
			,displaydate AS displaydate
			,cb.authorname AS authorname
			,cb.authorid
			,cb.description
			,publisheddate
			,cb.VIEWS AS VIEWS
			,(isnull(cb.issticky, 0)) AS issticky
			,(isnull(cb.isfeatured, 0)) AS isfeatured
			,ca.maskingname AS authormaskingname
			,cb.hosturl AS hosturl
			,cb.mainimagepath AS OriginalImgUrl 
			,'160x89'+cb.mainimagepath as SmallPicUrl
			,(isnull(cb.facebookcommentcount, 0)) AS facebookcommentcount
			, cat.Name as CategoryMaskingName
			,0 AS row_no
		FROM con_editcms_basic cb WITH (NOLOCK)
		INNER JOIN Con_EditCms_Category cat WITH (NOLOCK) ON cat.Id = cb.CategoryId
		INNER JOIN con_editcms_author ca WITH (NOLOCK) ON ca.authorid = cb.authorid				
		WHERE cb.isactive = 1
			AND cb.ispublished = 1
			AND cb.applicationid = 1
			AND cb.categoryid IN (
				SELECT listmember
				FROM fnsplitcsvvalueswithidentity(@categorylist)
				)
			AND cb.displaydate <= getdate()
			AND CB.AuthorId=@author AND CB.IsFeatured=1
			order by CB.DisplayDate desc
END
