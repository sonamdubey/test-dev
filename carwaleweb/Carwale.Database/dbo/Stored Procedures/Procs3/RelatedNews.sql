IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RelatedNews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RelatedNews]
GO

	--======================
-- created on : 7/7/2014
-- created by : Natesh Kumar
-- Description: To get news related to the tags 
-- Approved by Manish on 10-07-2014 05:30 pm checked indexes
-- Modified by Natesh on 22-7-2014 Added new line for search based on  multiple tags and recency upto last 6 months
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- modified by natesh kumar on 22-8-2014 for news of different basicid 

CREATE PROCEDURE [dbo].[RelatedNews]  -- Exec RelatedNews 'honda,amaze' , 440,1
@Tag VARCHAR(200),
@BasicId INT
,@ApplicationId INT
--@CategoryId INT

AS

BEGIN
   SET NOCOUNT ON;
   DECLARE @RowCount INT

   DECLARE  @CategoryId INT  =1
	       ,@startindex INT =1
	       ,@endindex INT=5;

 
 WITH CTE AS (
				SELECT 
					 CB.Id AS BasicId
					,CB.AuthorName
					,CB.Description
					,CB.DisplayDate
					,CB.PublishedDate
					,CB.VIEWS
					,CB.Title
					,CB.Url
					,SPC.FacebookCommentCount
					,CEI.HostUrl
					,CEI.ImagePathThumbnail
					,CEI.ImagePathLarge
					,CEI.IsMainImage
					,CEI.ImagePathCustom
					,CB.IsSticky
					,CET.ID
					,CB.ApplicationID	
					,ROW_NUMBER() OVER( partition by  cb.Id order by cb.Id )rowno				
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
				     WHERE 
					 CB.IsActive = 1 
				AND CB.ApplicationID = @ApplicationId
				AND CB.IsPublished = 1 
				AND CB.Id <> @BasicId 
				AND CB.CategoryId = 1
				AND CB.DisplayDate < = GETDATE()
				AND CB.DisplayDate >=  DATEADD(month, -6, GETDATE())
				AND (
					IsSticky IS NULL
					OR IsSticky = 0
					OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE)
					)
		)
					SELECT TOP (@endindex) * FROM CTE WHERE rowno=1 ORDER BY ID DESC ,DisplayDate DESC
             			
	  END

 

 

 





