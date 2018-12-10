IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetDefaultNewsPageDetails_test]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetDefaultNewsPageDetails_test]
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
-- Modified by: Ajay Singh on 28 april 2016,added three more categoryId features,comparison test and expert review in the case of news and wrong RecordCount solved 
-- =============================================      
create PROCEDURE [cw].[GetDefaultNewsPageDetails_test] -- execute cw.[GetDefaultNewsPageDetails_16.4.8]  1, 1, 1000,1
	-- Add the parameters for the stored procedure here      
	@CategoryId NUMERIC(18, 0) -- New categoryId
	,@startindex INT
	,@endindex INT
	,@ApplicationId INT
	--,@BasicId INT
AS
BEGIN
	DECLARE @CategoryId_AutoExpo TINYINT = 19	
	DECLARE @CategoryId_Features TINYINT = 1	
	DECLARE @CategoryId_ExpertReview TINYINT = 1	
	DECLARE @CategoryId_ComparisonTest TINYINT = 1

	DECLARE @CurrentDateTime DATETIME= GETDATE()

	-- This is done to avoid autoexpo news on pitstop page.
	IF @CategoryId = 12
		SET @CategoryId_AutoExpo = 12
	-- SET NOCOUNT ON added to prevent extra result sets from      
	-- interfering with SELECT statements.    
	
	--This is for all editorial content at news page
	IF @CategoryId = 1
	BEGIN
	  SET @CategoryId_ComparisonTest = 2
	  SET @CategoryId_Features = 6
	  SET @CategoryId_ExpertReview = 8
    END
	SET NOCOUNT ON;
	
	 
			 
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
					,CB.CategoryId
					,MO.MaskingName AS ModelMaskingName
					,M.Name AS MakeName 
					into #temp
				FROM Con_EditCms_Basic AS CB WITH (NOLOCK)
				LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
															  AND CEI.IsMainImage = 1
															  AND CEI.IsActive = 1
				LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON SPC.TypeId = CB.Id
					                                         AND SPC.TypeCategoryId IN ( @CategoryId,@CategoryId_AutoExpo,@CategoryId_Features,@CategoryId_ComparisonTest,@CategoryId_ExpertReview)
				INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId

				LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = CEI.ModelId 
				LEFT JOIN CarMakes M WITH (NOLOCK) ON M.ID = MO.CarMakeId

				WHERE CB.CategoryId IN ( @CategoryId,@CategoryId_AutoExpo,@CategoryId_Features,@CategoryId_ComparisonTest,@CategoryId_ExpertReview) -- New and AutoExpo
					 AND CB.IsActive = 1
					 AND CB.IsPublished = 1
					 AND CB.ApplicationID = @ApplicationId
					--AND CB.Id <> @BasicId
					 AND CB.DisplayDate < =@CurrentDateTime      --GETDATE()
					 AND ( IsSticky IS NULL OR IsSticky = 0 OR CAST(StickyToDate AS DATE) < CAST(@CurrentDateTime AS DATE) -- CAST(GETDATE() AS DATE)
						 )
			SELECT * FROM #temp 
			WHERE Row_No BETWEEN @startindex 	AND @endindex
		 	ORDER BY Row_No

			SELECT COUNT(*) as RecordCount FROM #temp
          
		
END
