IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RelatedNews_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RelatedNews_15]
GO

	CREATE PROCEDURE [dbo].[RelatedNews_15.8.1]  -- Exec RelatedNews 'honda' , 440,1
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
					 ,cet.tag as tag
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
					,CEI.OriginalImgPath	
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
