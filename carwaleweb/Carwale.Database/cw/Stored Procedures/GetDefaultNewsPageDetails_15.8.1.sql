IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetDefaultNewsPageDetails_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetDefaultNewsPageDetails_15]
GO

	-- =============================================      
-- Author:  <Reshma Shetty>
-- Modifier:<Ravi Koshal>      
-- Create date: <27/08/2012>      
-- Description: <Returns the all the details required in the default page of the news section> 
--Modified By:Ravi Koshal 20/08/2013 Removed the Comments table from the query and also making use of category id variable 
-- Modified By : Supriya K on 4/6/2014 to add column ImagePathCustom 
--Approved by: Manish on 01-07-2014 06:08 m , With (NoLock) is used,non clustered index created on Con_EditCms_Images(BasicId) .
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Modified by: Manish on 06-10-2014 taken getdate() value in separate variable.
-- Modified by rakesh yadav on 06 jan 2016,@CategoryId_AutoExpo is changed from 9(AutoExpo2014) to 19(AutoExpo2016)
-- =============================================      
CREATE PROCEDURE [cw].[GetDefaultNewsPageDetails_15.8.1] -- execute cw.GetDefaultNewsPageDetails  1, 1, 50,1
	-- Add the parameters for the stored procedure here      
	@CategoryId NUMERIC(18, 0) -- New categoryId
	,@startindex INT
	,@endindex INT
	,@ApplicationId INT
	--,@BasicId INT
AS
BEGIN
	DECLARE @CategoryId_AutoExpo TINYINT = 19

	DECLARE @CurrentDateTime DATETIME= GETDATE()

	-- This is done to avoid autoexpo news on pitstop page.
	IF @CategoryId = 12
		SET @CategoryId_AutoExpo = 12
	-- SET NOCOUNT ON added to prevent extra result sets from      
	-- interfering with SELECT statements.      
	SET NOCOUNT ON;
	
	(
			-- Normal Query
			SELECT  *
			FROM (
				SELECT DISTINCT CB.Id AS BasicId
					,CB.AuthorName
					,CB.Description
					,CB.DisplayDate
					,CB.VIEWS
					,CB.Title
					,CB.Url
					,CEI.HostUrl
					,CEI.ImagePathThumbnail
					,CEI.ImagePathLarge
					,CEI.IsMainImage
					,CEI.ImagePathCustom
					,CA.MaskingName
					,CB.ApplicationID
					,ROW_NUMBER() OVER ( ORDER BY DisplayDate DESC ) AS Row_No
					,CB.IsSticky
					,SPC.FacebookCommentCount
					,CEI.OriginalImgPath
				FROM Con_EditCms_Basic AS CB WITH (NOLOCK)
				LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
															  AND CEI.IsMainImage = 1
															  AND CEI.IsActive = 1
				LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON SPC.TypeId = CB.Id
					                                          AND SPC.TypeCategoryId IN ( @CategoryId,@CategoryId_AutoExpo)
				INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
				WHERE CB.CategoryId IN ( @CategoryId,@CategoryId_AutoExpo) -- New and AutoExpo
					 AND CB.IsActive = 1
					 AND CB.IsPublished = 1
					 AND CB.ApplicationID = @ApplicationId
					--AND CB.Id <> @BasicId
					 AND CB.DisplayDate < =@CurrentDateTime      --GETDATE()
					 AND ( IsSticky IS NULL OR IsSticky = 0 OR CAST(StickyToDate AS DATE) < CAST(@CurrentDateTime AS DATE) -- CAST(GETDATE() AS DATE)
						 )
				) AS T
			WHERE T.Row_No BETWEEN @startindex 	AND @endindex
		)

	ORDER BY Row_No

	-- Query to get news record count
	SELECT COUNT(id) AS RecordCount
	FROM Con_EditCms_Basic CB WITH (NOLOCK)
	WHERE CB.CategoryId IN (
			@CategoryId
			,@CategoryId_AutoExpo -- AUTOEXPO
			)
		AND CB.IsActive = 1
		AND CB.IsPublished = 1
		AND CB.ApplicationID = @ApplicationId
		
		
END
