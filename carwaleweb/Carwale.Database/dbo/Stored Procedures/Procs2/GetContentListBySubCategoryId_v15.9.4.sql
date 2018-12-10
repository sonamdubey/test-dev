IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetContentListBySubCategoryId_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetContentListBySubCategoryId_v15]
GO

	--===================================================================================
--Created By Natesh Kumar on 1-01-14
--description: get article list on subcategory id
-- exec [dbo].[GetContentListBySubCategoryId_v15.9.4] 1,null,5,1,10
-- modified by natesh kuamr change in sp for model and make related data on 4/11/14 
-- modified by natesh kumar checking isdeleted flag on join on 1/12/14
-- modified by natesh kumar subcatid is changed to nullable on join on 3/2/15
-- Modified by Satish Sharma On 23/Jul/2015, Image dynamic resize revamp
-- Modified by Sachin Bharti On 25/09/2015, added category masking name
-- Modifier by Sachin Bharti on 14/10/2015, Added CategoryMaskingName for application id 2
-- Modified by Manish Chourasiya on 17-12-2015 commented sticky cases since it is not using as on now.and also made chages regarding optimization of the code only for Carwale query.
--======================================================================================

CREATE PROCEDURE [dbo].[GetContentListBySubCategoryId_v15.9.4]
		@ApplicationId INT
		,@SubCategory VARCHAR(30) = NULL
		,@CategoryIds VARCHAR(30)
		,@StartIndex INT
		,@EndIndex INT
		,@MakeId INT = NULL
		,@ModelId INT = NULL

AS

BEGIN
	IF(@ApplicationId = 1)
		BEGIN
			-- Sticky Query.
			/*(	
			SELECT DISTINCT
				cb.Id AS BasicId
				,CB.CategoryId AS CategoryId
				,cb.Title AS Title
				,cb.Url AS ArticleUrl
				,DisplayDate AS DisplayDate
				,CB.AuthorName AS AuthorName
				,CA.MaskingName AS AuthorMaskingName
				,cb.Description AS Description
				,PublishedDate 
				,cb.Views AS Views
				,(ISNULL(cb.IsSticky,0)) AS IsSticky
				,cei.HostURL AS HostUrl	
				--,cei.ImagePathLarge AS LargePicUrl
				--,cei.ImagePathThumbnail AS SmallPicUrl
				,CEI.OriginalImgPath AS OrginalImgPath
				,SC.Name AS SubCategoryName
				,MO.ID as ModelId
				,M.ID as MakeId
				,CC.CategoryMaskingName
				,'1' AS Row_No
				,'1' as Row_Num
			FROM Con_EditCms_Basic AS CB WITH(NOLOCK)
			INNER JOIN Con_EditCms_BasicSubCategories BSC  WITH (NOLOCK) ON BSC.BasicId = CB.Id
			INNER JOIN Con_EditCms_SubCategories SC WITH (NOLOCK) ON SC.Id = BSC.SubCategoryId AND SC.IsActive = 1
			INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
			LEFT JOIN Con_EditCms_Images CEI  WITH(NOLOCK) ON CEI.BasicId = CB.Id AND CEI.IsMainImage = 1 AND CEI.IsActive = 1
			
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
				AND C.IsActive = 1
			LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId 
			LEFT JOIN CarMakes M WITH (NOLOCK) ON M.ID = C.MakeId
			LEFT JOIN Con_EditCms_Category CC WITH (NOLOCK) ON CC.Id = CB.CategoryId
			WHERE CB.ApplicationID = @ApplicationId AND (
						@SubCategory IS NULL
						OR SC.Id =@SubCategory
						)
			    AND CB.CategoryId IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@CategoryIds)
						)
				AND CB.IsActive = 1 AND CB.IsPublished = 1 AND IsSticky = 1
				AND ISNULL(MO.IsDeleted, 0) = 0
				AND ISNULL(M.IsDeleted, 0) = 0
				AND (CAST(GETDATE() AS DATE) BETWEEN CAST(StickyFromDate AS DATE)AND CAST(StickyToDate AS DATE))
			)
			UNION ALL
			(-- Normal Query
			*/
			SELECT  * FROM
			(
				SELECT *, ROW_NUMBER() OVER (ORDER BY DisplayDate DESC) AS Row_Num FROM(
				SELECT
					cb.Id AS BasicId
					,CB.CategoryId AS CategoryId
					,cb.Title AS Title
					,cb.Url AS ArticleUrl
					,DisplayDate AS DisplayDate
					,CB.AuthorName AS AuthorName
					,CA.MaskingName AS AuthorMaskingName
					,cb.Description AS Description
					,PublishedDate 
					,cb.Views AS Views
					,(ISNULL(cb.IsSticky,0)) AS IsSticky
					,cei.HostURL AS HostUrl		
					--,cei.ImagePathLarge AS LargePicUrl
					--,cei.ImagePathThumbnail AS SmallPicUrl
					,CEI.OriginalImgPath AS OrginalImgPath
					,SC.Name AS SubCategoryName
					,MO.ID as ModelId
					,M.ID as MakeId
					,CC.CategoryMaskingName	
					,ROW_NUMBER() OVER (PARTITION BY CB.Id ORDER BY DisplayDate DESC) AS Row_No	
				FROM Con_EditCms_Basic CB WITH (NOLOCK)
				INNER JOIN Con_EditCms_BasicSubCategories BSC  WITH (NOLOCK) ON BSC.BasicId = CB.Id
				INNER JOIN Con_EditCms_SubCategories SC WITH (NOLOCK) ON SC.Id = BSC.SubCategoryId AND SC.IsActive = 1
				INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId	
				LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id AND CEI.IsMainImage = 1 AND CEI.IsActive = 1

				LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
					AND C.IsActive = 1
				LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId AND (@ModelId IS NULL OR MO.ID = @ModelId)
				LEFT JOIN CarMakes M WITH (NOLOCK) ON M.ID = Mo.CarMakeId AND  (@MakeId IS NULL OR M.Id = @MakeId)
				LEFT JOIN Con_EditCms_Category CC WITH (NOLOCK) ON CC.Id = CB.CategoryId

				WHERE  
				      CB.ApplicationID = @ApplicationId 
				 AND CB.IsActive = 1 
				 AND CB.IsPublished = 1
				 AND CB.CategoryId IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@CategoryIds)
						)
				 AND (
						@SubCategory IS NULL
						OR SC.Id =@SubCategory
						)
			    AND (@MakeId IS NULL OR M.Id = @MakeId)
				AND (@ModelId IS NULL OR MO.ID = @ModelId)
				AND ISNULL(MO.IsDeleted, 0) = 0
			    AND ISNULL(M.IsDeleted, 0) = 0
                AND CB.DisplayDate <= GETDATE()
				AND (IsSticky IS NULL OR IsSticky = 0 OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE))
				) AS CTE1 WHERE Row_No = 1
			) CTE WHERE Row_Num BETWEEN @StartIndex AND @EndIndex
			--) 
			ORDER BY Row_Num
		
					-- Record Count Query
			SELECT COUNT(DISTINCT CB.Id) AS RecordCount
			FROM Con_EditCms_Basic CB WITH (NOLOCK)
			INNER JOIN Con_EditCms_BasicSubCategories BSC  WITH (NOLOCK) ON BSC.BasicId = CB.Id
			INNER JOIN Con_EditCms_SubCategories SC WITH (NOLOCK) ON SC.Id = BSC.SubCategoryId AND SC.IsActive = 1
			INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
			AND C.IsActive = 1
			LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId  AND (@ModelId IS NULL OR MO.ID = @ModelId)
			LEFT JOIN CarMakes M WITH (NOLOCK) ON M.ID = C.MakeId      AND  (@MakeId IS NULL OR M.Id = @MakeId)

			WHERE 
			      CB.ApplicationID = @ApplicationId
				  AND CB.IsActive = 1
				  AND CB.IsPublished = 1 
				  AND CB.CategoryId IN (
						                SELECT ListMember
						                FROM fnSplitCSVValuesWithIdentity(@CategoryIds)
						               )
				  AND  
			           (
						@SubCategory IS NULL
						OR SC.Id =@SubCategory
						)
			  AND (@MakeId IS NULL OR M.Id = @MakeId)
			  AND (@ModelId IS NULL OR MO.ID = @ModelId)
			  AND ISNULL(MO.IsDeleted, 0) = 0
			  AND ISNULL(M.IsDeleted, 0) = 0
			 
	END

	IF(@ApplicationId = 2)
		BEGIN
			-- Sticky Query.
			(	
			SELECT DISTINCT
				cb.Id AS BasicId
				,CB.CategoryId AS CategoryId
				,cb.Title AS Title
				,cb.Url AS ArticleUrl
				,DisplayDate AS DisplayDate
				,CB.AuthorName AS AuthorName
				,CA.MaskingName AS AuthorMaskingName
				,cb.Description AS Description
				,PublishedDate 
				,cb.Views AS Views
				,(ISNULL(cb.IsSticky,0)) AS IsSticky
				,cei.HostURL AS HostUrl	
				--,cei.ImagePathLarge AS LargePicUrl
				--,cei.ImagePathThumbnail AS SmallPicUrl
				,CEI.OriginalImgPath AS OrginalImgPath
				,MO.ID as ModelId
				,M.ID as MakeId
				,CC.CategoryMaskingName	
				,'1' AS Row_No
				,'1' as Row_Num
			FROM Con_EditCms_Basic AS CB WITH(NOLOCK)
			INNER JOIN Con_EditCms_BasicSubCategories BSC  WITH (NOLOCK) ON BSC.BasicId = CB.Id
			INNER JOIN Con_EditCms_SubCategories SC WITH (NOLOCK) ON SC.Id = BSC.SubCategoryId AND SC.IsActive = 1
			INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
			LEFT JOIN Con_EditCms_Images CEI  WITH(NOLOCK) ON CEI.BasicId = CB.Id AND CEI.IsMainImage = 1 AND CEI.IsActive = 1
			
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
				AND C.IsActive = 1
			LEFT JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
			LEFT JOIN BikeMakes M WITH (NOLOCK) ON M.ID = C.MakeId
			LEFT JOIN Con_EditCms_Category CC WITH (NOLOCK) ON CC.Id = CB.CategoryId

			WHERE CB.ApplicationID = @ApplicationId AND  (
						@SubCategory IS NULL
						OR SC.Id =@SubCategory
						)
			    AND CB.CategoryId IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@CategoryIds)
						)
				AND CB.IsActive = 1 AND CB.IsPublished = 1 AND IsSticky = 1
				AND ISNULL(MO.IsDeleted, 0) = 0
				AND ISNULL(M.IsDeleted, 0) = 0
				AND (CAST(GETDATE() AS DATE) BETWEEN CAST(StickyFromDate AS DATE)AND CAST(StickyToDate AS DATE))
			)
			UNION ALL
			(-- Normal Query
			SELECT  * FROM
			(
				SELECT *, ROW_NUMBER() OVER (ORDER BY DisplayDate DESC) AS Row_Num FROM(
				SELECT
					cb.Id AS BasicId
					,CB.CategoryId AS CategoryId
					,cb.Title AS Title
					,cb.Url AS ArticleUrl
					,DisplayDate AS DisplayDate
					,CB.AuthorName AS AuthorName
					,CA.MaskingName AS AuthorMaskingName
					,cb.Description AS Description
					,PublishedDate 
					,cb.Views AS Views
					,(ISNULL(cb.IsSticky,0)) AS IsSticky
					,cei.HostURL AS HostUrl		
					--,cei.ImagePathLarge AS LargePicUrl
					--,cei.ImagePathThumbnail AS SmallPicUrl
					,CEI.OriginalImgPath AS OrginalImgPath
					,MO.ID as ModelId
					,M.ID as MakeId
					,CC.CategoryMaskingName	
					,ROW_NUMBER() OVER (PARTITION BY CB.Id ORDER BY DisplayDate DESC) AS Row_No		
				FROM Con_EditCms_Basic CB WITH (NOLOCK)
				INNER JOIN Con_EditCms_BasicSubCategories BSC  WITH (NOLOCK) ON BSC.BasicId = CB.Id
				INNER JOIN Con_EditCms_SubCategories SC WITH (NOLOCK) ON SC.Id = BSC.SubCategoryId AND SC.IsActive = 1
				INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId	
				LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id AND CEI.IsMainImage = 1 AND CEI.IsActive = 1

				LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
					AND C.IsActive = 1
				LEFT JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
				LEFT JOIN BikeMakes M WITH (NOLOCK) ON M.ID = C.MakeId
				LEFT JOIN Con_EditCms_Category CC WITH (NOLOCK) ON CC.Id = CB.CategoryId

				WHERE  (
						@SubCategory IS NULL
						OR SC.Id =@SubCategory
						)
			    AND CB.CategoryId IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@CategoryIds)
						)
				AND CB.ApplicationID = @ApplicationId AND CB.IsActive = 1 AND CB.IsPublished = 1
				AND ISNULL(MO.IsDeleted, 0) = 0
				AND ISNULL(M.IsDeleted, 0) = 0
				AND (
					@MakeId IS NULL
					OR M.Id = @MakeId
					)
				AND (
					@ModelId IS NULL
					OR MO.ID = @ModelId
					)

				AND CB.DisplayDate <= GETDATE()
				AND (IsSticky IS NULL OR IsSticky = 0 OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE))
				) AS CTE1 WHERE Row_No = 1
			) CTE WHERE Row_Num BETWEEN @StartIndex AND @EndIndex
			) ORDER BY Row_Num
		
					-- Record Count Query
			SELECT COUNT(DISTINCT CB.Id) AS RecordCount
			FROM Con_EditCms_Basic CB WITH (NOLOCK)
			INNER JOIN Con_EditCms_BasicSubCategories BSC  WITH (NOLOCK) ON BSC.BasicId = CB.Id
			INNER JOIN Con_EditCms_SubCategories SC WITH (NOLOCK) ON SC.Id = BSC.SubCategoryId AND SC.IsActive = 1
			INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
			AND C.IsActive = 1
			LEFT JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId 
			LEFT JOIN BikeMakes M WITH (NOLOCK) ON M.ID = C.MakeId

			WHERE  (
						@SubCategory IS NULL
						OR SC.Id =@SubCategory
						)
			    AND CB.CategoryId IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@CategoryIds)
						)
			AND CB.IsPublished = 1 AND CB.ApplicationID = @ApplicationId AND CB.IsActive = 1
			AND ISNULL(MO.IsDeleted, 0) = 0
			AND ISNULL(M.IsDeleted, 0) = 0
			AND (
				@MakeId IS NULL
				OR M.Id = @MakeId
				)
			AND (
				@ModelId IS NULL
				OR MO.ID = @ModelId
				)
	END
END
