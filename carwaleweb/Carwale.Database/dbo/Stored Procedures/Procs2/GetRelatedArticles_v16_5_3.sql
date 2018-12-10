IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetRelatedArticles_v16_5_3]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetRelatedArticles_v16_5_3]
GO

	--================================================================
-- Author: Natesh Kumar on 28/8/14 
--Description: gets the articles related to the tags  with the given contentTypes as categoryids, tag, applicationid , totalrecords as input parameter
-- modified sp for gettting list related to tags and also for case of no tag ie tag = null   
-- Modified by Satish Sharma On 23/Jul/2015, Image dynamic resize revamp
--================================================================
-- exec dbo.GetRelatedArticles 1,'1','',100
-- EXEC [dbo].[GetRelatedArticles] 1, 1,'honda',8
-- Jitendra CMS Optimization for using new url column
CREATE PROCEDURE [dbo].[GetRelatedArticles_v16_5_3]
		@ApplicationId INT
		,@contentTypes VARCHAR(50)
		,@tag VARCHAR(200)
		,@totalRecords INT

AS

BEGIN

			WITH cte
			AS (
				SELECT cb.Id AS BasicId
					,cet.Id AS TagId
					,cet.Tag as tag
					,CategoryId AS CategoryId		
					,cb.Title AS Title
					,cb.Url AS ArticleUrl  --Jitendra CMS Optimization
					,AuthorName AS AuthorName
					,cb.Description AS Description		
					,cb.VIEWS AS Views
					,(ISNULL(cb.IsSticky,0)) AS IsSticky 
					,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
					,cei.HostURL AS HostUrl
					--,cei.ImagePathLarge AS LargePicUrl
					--,cei.ImagePathThumbnail AS SmallPicUrl
					,CEI.OriginalImgPath AS OrginalImgPath		
					,CB.DisplayDate AS DisplayDate
					,CB.PublishedDate
					,ROW_NUMBER() OVER (
						PARTITION BY cb.Id ORDER BY cb.Id
						) rowno
				FROM     
			        Con_EditCms_Basic CB  WITH(NOLOCK)        
                LEFT JOIN  Con_EditCms_BasicTags CEB WITH(NOLOCK)  ON CB.Id = CEB.BasicId 
				LEFT JOIN Con_EditCms_Tags CET WITH(NOLOCK) ON CET.Id = CEB.TagId
				                                             AND CET.Tag IN (select ListMember from fnSplitCSVValuesWithIdentity(@Tag)) 
				 
				LEFT JOIN SocialPluginsCount SPC WITH(NOLOCK) ON SPC.TypeId = CB.Id
			                                                   AND SPC.TypeCategoryId = 1
				LEFT JOIN Con_EditCms_Images CEI   WITH (NOLOCK) ON CEI.BasicId = CB.Id
					                               AND CEI.IsMainImage = 1
					                               AND CEI.IsActive = 1
				 WHERE  cb.CategoryId IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@contentTypes)
						)
					AND cb.ApplicationID = @ApplicationId
					AND cb.IsActive = 1
					AND cb.IsPublished = 1
					AND CB.DisplayDate < = GETDATE()
					AND CB.DisplayDate >=  DATEADD(month, -6, GETDATE())
					AND (
						CB.IsSticky IS NULL
						OR CB.IsSticky = 0
						OR CAST(CB.StickyToDate AS DATE) < CAST(GETDATE() AS DATE)
						)
				)
			SELECT TOP (@totalRecords) *
			FROM cte
			WHERE rowno = 1
			ORDER BY TagId DESC, DisplayDate DESC
END
