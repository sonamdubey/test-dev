IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetContentListByCategory_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetContentListByCategory_v16]
GO

	--================================================================
-- Author: Natesh Kumar on 28/8/14 
--Description: gets the article list based on categoryid/categoryids with applicationid, categoryids list , startindex, endindex, makeid and modelid as input parameter
-- modified by natesh on 4/11/14 for recordcount
-- modified by natesh kumar checking isdeleted flag on join on 26/11/14
-- modified by natesh kumar on 1/12/14 for null and 0 for isdeleted
-- Modified By : Shalini Nair on 17/02/15 added left join on CBS and CS for applicationId=2
-- Modified by Satish Sharma On 23/Jul/2015, Image dynamic resize revamp
-- Modified by Sachin Bharti On 22/Sep/2015, Added category masking name 
-- Modifier by Sachin Bharti on 14/10/2015, Added CategoryMaskingName for application id 2
-- exec [dbo].[GetContentListByCategory_v15.9.4] 1,12,1,10
-- Modified by Rakesh Yadav on 17-12-2015 commented sticky cases since it is not using as on now.and also made chages regarding optimization of the code only for Carwale query.
-- Modified by Manish on 21-12-2015 rearrangening the join in CarWale query for optimization purpose
-- Modified by Sumit Kate on 16-02-2016 Returnt the IsFeatured for applicationId = 2
 -- AM Added 04-05-2016 to order by Basic ID
--================================================================
CREATE PROCEDURE [dbo].[GetContentListByCategory_v16.2.2]
	@ApplicationId TINYINT
	,@CategoryList VARCHAR(50)
	,@StartIndex INT
	,@EndIndex INT
	,@MakeId INT = NULL
	,@ModelId INT = NULL
AS
BEGIN
     SET NOCOUNT ON;
	IF (@ApplicationId = 1)
	BEGIN
		-- Sticky Query.
		/*(
				SELECT *
				FROM (
					SELECT DISTINCT cb.Id AS BasicId
						,CB.CategoryId AS CategoryId
						,cb.Title AS Title
						,cb.Url AS ArticleUrl
						,DisplayDate AS DisplayDate
						,CB.AuthorName AS AuthorName
						,CA.MaskingName AS AuthorMaskingName
						,cb.Description AS Description
						,PublishedDate
						,cb.VIEWS AS VIEWS
						,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
						,(ISNULL(cb.IsSticky, 0)) AS IsSticky
						,cei.HostURL AS HostUrl
						--,cei.ImagePathLarge AS LargePicUrl
						--,cei.ImagePathThumbnail AS SmallPicUrl
						,CEI.OriginalImgPath AS OrginalImgPath
						,CS.NAME AS SubCategoryName
						--,'' AS MakeId, '' AS ModelId, '' AS MakeName, '' AS ModelName, '' AS ModelMaskingName 	
						,M.Id AS MakeId
						,MO.ID AS ModelId
						,M.NAME AS MakeName
						,MO.NAME AS ModelName
						,MO.MaskingName AS ModelMaskingName
						,CC.CategoryMaskingName
						,ROW_NUMBER() OVER (
							PARTITION BY CB.Id ORDER BY DisplayDate DESC
							) AS Row_No
						,'1' AS Row_Num
						
					FROM Con_EditCms_Basic AS CB WITH (NOLOCK)
					INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
					LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
						AND CEI.IsMainImage = 1
						AND CEI.IsActive = 1
					LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON SPC.TypeId = CB.Id
					LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
						AND C.IsActive = 1
					LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
					LEFT JOIN CarMakes M WITH (NOLOCK) ON M.ID = C.MakeId
					LEFT JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CB.Id = CBS.BasicId
					LEFT JOIN Con_EditCms_SubCategories CS WITH (NOLOCK) ON CS.Id = CBS.SubCategoryId
					LEFT JOIN Con_EditCms_Category CC WITH (NOLOCK) ON CC.Id = CB.CategoryId
					WHERE CB.ApplicationID = @ApplicationId
						AND CB.CategoryId IN (
							SELECT ListMember
							FROM fnSplitCSVValuesWithIdentity(@CategoryList)
							)
						AND CB.IsActive = 1
						AND CB.IsPublished = 1
						AND ISNULL(MO.IsDeleted, 0) = 0
						AND ISNULL(M.IsDeleted, 0) = 0
						AND IsSticky = 1
						AND (
							CAST(GETDATE() AS DATE) BETWEEN CAST(StickyFromDate AS DATE)
								AND CAST(StickyToDate AS DATE)
							)
					) AS CTE
				WHERE Row_No = 1
				)
		
		UNION ALL
		
		(*/
			-- Normal Query
			SELECT *
			FROM (
				SELECT *
					,ROW_NUMBER() OVER (
						ORDER BY BasicID DESC
						) AS Row_Num
				FROM (
					SELECT Top 1000 cb.Id AS BasicId
						,CB.CategoryId AS CategoryId
						,cb.Title AS Title
						,cb.Url AS ArticleUrl
						,DisplayDate AS DisplayDate
						,CB.AuthorName AS AuthorName
						,CA.MaskingName AS AuthorMaskingName
						,cb.Description AS Description
						,PublishedDate
						,cb.VIEWS AS VIEWS
						,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
						,(ISNULL(cb.IsSticky, 0)) AS IsSticky						
						,(ISNULL(cb.IsFeatured,0)) AS IsFeatured
						,cei.HostURL AS HostUrl
						--,cei.ImagePathLarge AS LargePicUrl
						--,cei.ImagePathThumbnail AS SmallPicUrl
						,CEI.OriginalImgPath AS OrginalImgPath
						,CS.NAME AS SubCategoryName
						,M.Id AS MakeId
						,MO.ID AS ModelId
						,M.NAME AS MakeName
						,MO.NAME AS ModelName
						,MO.MaskingName AS ModelMaskingName
						,CC.CategoryMaskingName
						,ROW_NUMBER() OVER (
							PARTITION BY CB.Id ORDER BY DisplayDate DESC
							) AS Row_No
						
					FROM Con_EditCms_Basic CB WITH (NOLOCK)
					INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
					LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
															AND CEI.IsActive = 1
															AND CEI.IsMainImage = 1
					
					LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
						AND C.IsActive = 1
					LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId AND (@ModelId IS NULL OR MO.ID = @ModelId )
					LEFT JOIN CarMakes M WITH (NOLOCK) ON M.ID = MO.CarMakeId AND (@MakeId IS NULL OR M.Id = @MakeId )
					LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON SPC.TypeId = CB.Id
					LEFT JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CB.Id = CBS.BasicId
					LEFT JOIN Con_EditCms_SubCategories CS WITH (NOLOCK) ON CS.Id = CBS.SubCategoryId
					LEFT JOIN Con_EditCms_Category CC WITH (NOLOCK) ON CC.Id = CB.CategoryId
					WHERE 
						 CB.IsActive = 1 -- and CB.Id>=21283  -- AM Added 04-05-2016 to restrict 2016 news
						AND CB.IsPublished = 1
						AND CB.ApplicationID = @ApplicationId
						AND CB.CategoryId IN (
							SELECT ListMember
							FROM fnSplitCSVValuesWithIdentity(@CategoryList)
							)
						AND (@MakeId IS NULL OR M.Id = @MakeId )
						AND (@ModelId IS NULL OR MO.ID = @ModelId )
						AND ISNULL(MO.IsDeleted, 0) = 0
						AND ISNULL(M.IsDeleted, 0) = 0
						AND CB.DisplayDate <= GETDATE()
						AND (
							IsSticky IS NULL
							OR IsSticky = 0
							OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE)
							)
						order by cb.Id desc  -- AM Added 04-05-2016 to order by Basic ID
						
					) AS CTE1
				WHERE Row_No = 1
				) CTE
			WHERE Row_Num BETWEEN @StartIndex
					AND @EndIndex
			--)
		ORDER BY Row_Num 
	

		-- Record Count Query
		SELECT COUNT(DISTINCT CB.Id) AS RecordCount
		FROM Con_EditCms_Basic CB WITH (NOLOCK)
		INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
		LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
			AND C.IsActive = 1
		LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId AND ( @ModelId IS NULL OR MO.ID = @ModelId )
		LEFT JOIN CarMakes M WITH (NOLOCK) ON M.ID = Mo.CarMakeId AND ( @MakeId IS NULL OR M.Id = @MakeId )
		WHERE 
		CB.IsActive = 1
		AND CB.IsPublished = 1
		AND CB.ApplicationID = @ApplicationId
		AND CB.CategoryId IN (
				SELECT ListMember
				FROM fnSplitCSVValuesWithIdentity(@CategoryList)
				)
		AND ( @MakeId IS NULL OR M.Id = @MakeId )
		AND ( @ModelId IS NULL OR MO.ID = @ModelId )
		AND ISNULL(MO.IsDeleted, 0) = 0
		AND ISNULL(M.IsDeleted, 0) = 0
		AND CB.DisplayDate <= GETDATE()
		AND (
			IsSticky IS NULL
			OR IsSticky = 0
			OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE)
			)
		
				
	END
	ELSE
		IF (@ApplicationId = 2)
		BEGIN
			-- Sticky Query.
			(
					SELECT *
					FROM (
						SELECT DISTINCT top 1000  cb.Id AS BasicId
							,cb.CategoryId AS CategoryId
							,cb.Title AS Title
							,cb.Url AS ArticleUrl
							,DisplayDate AS DisplayDate
							,CB.AuthorName AS AuthorName
							,CA.MaskingName AS AuthorMaskingName
							,cb.Description AS Description
							,PublishedDate
							,cb.VIEWS AS VIEWS
							,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
							,(ISNULL(cb.IsSticky, 0)) AS IsSticky
							,(ISNULL(cb.IsFeatured,0)) AS IsFeatured
							,cei.HostURL AS HostUrl
							--,cei.ImagePathLarge AS LargePicUrl
							--,cei.ImagePathThumbnail AS SmallPicUrl
							,CEI.OriginalImgPath AS OrginalImgPath
							,CS.NAME AS SubCategoryName
							--,'' AS MakeId, '' AS ModelId, '' AS MakeName, '' AS ModelName, '' AS ModelMaskingName 	
							,M.Id AS MakeId
							,MO.ID AS ModelId
							,M.NAME AS MakeName
							,MO.NAME AS ModelName
							,MO.MaskingName AS ModelMaskingName
							,CC.CategoryMaskingName
							,ROW_NUMBER() OVER (
								PARTITION BY CB.Id ORDER BY DisplayDate DESC
								) AS Row_No
							,'1' AS Row_Num
						FROM Con_EditCms_Basic AS CB WITH (NOLOCK)
						INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
						LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
							AND CEI.IsMainImage = 1
							AND CEI.IsActive = 1
						LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON SPC.TypeId = CB.Id
						LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
							AND C.IsActive = 1
						LEFT JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
						LEFT JOIN BikeMakes M WITH (NOLOCK) ON M.ID = C.MakeId
						-- Added By : Shalini Nair added left join CBS and CS
						LEFT JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CB.Id = CBS.BasicId
						LEFT JOIN Con_EditCms_SubCategories CS WITH (NOLOCK) ON CS.Id = CBS.SubCategoryId
						LEFT JOIN Con_EditCms_Category CC WITH (NOLOCK) ON CC.Id = CB.CategoryId
						WHERE CB.ApplicationID = @ApplicationId
							AND CB.CategoryId IN (
								SELECT ListMember
								FROM fnSplitCSVValuesWithIdentity(@CategoryList)
								)
							AND CB.IsActive = 1
							AND CB.IsPublished = 1
							AND ISNULL(MO.IsDeleted, 0) = 0
							AND ISNULL(M.IsDeleted, 0) = 0
							AND IsSticky = 1
							AND (
								CAST(GETDATE() AS DATE) BETWEEN CAST(StickyFromDate AS DATE)
									AND CAST(StickyToDate AS DATE)
								)
						) AS CTE
					WHERE Row_No = 1
					)
			
			UNION ALL
			
			(
				-- Normal Query
				SELECT *
				FROM (
					SELECT *
						,ROW_NUMBER() OVER (
							ORDER BY DisplayDate DESC
							) AS Row_Num
					FROM (
						SELECT cb.Id AS BasicId
							,cb.CategoryId AS CategoryId
							,cb.Title AS Title
							,cb.Url AS ArticleUrl
							,DisplayDate AS DisplayDate
							,CB.AuthorName AS AuthorName
							,CA.MaskingName AS AuthorMaskingName
							,cb.Description AS Description
							,PublishedDate
							,cb.VIEWS AS VIEWS
							,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
							,(ISNULL(cb.IsSticky, 0)) AS IsSticky
							,(ISNULL(cb.IsFeatured,0)) AS IsFeatured
							,cei.HostURL AS HostUrl
							--,cei.ImagePathLarge AS LargePicUrl
							--,cei.ImagePathThumbnail AS SmallPicUrl
							,CEI.OriginalImgPath AS OrginalImgPath
							,CS.NAME AS SubCategoryName
							,M.Id AS MakeId
							,MO.ID AS ModelId
							,M.NAME AS MakeName
							,MO.NAME AS ModelName
							,MO.MaskingName AS ModelMaskingName
							,CC.CategoryMaskingName
							,ROW_NUMBER() OVER (
								PARTITION BY CB.Id ORDER BY DisplayDate DESC
								) AS Row_No
						FROM Con_EditCms_Basic CB WITH (NOLOCK)
						INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
						LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
							AND CEI.IsMainImage = 1
							AND CEI.IsActive = 1
						LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON SPC.TypeId = CB.Id
						LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
							AND C.IsActive = 1
						LEFT JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
						LEFT JOIN BikeMakes M WITH (NOLOCK) ON M.ID = C.MakeId
						LEFT JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CB.Id = CBS.BasicId
						LEFT JOIN Con_EditCms_SubCategories CS WITH (NOLOCK) ON CS.Id = CBS.SubCategoryId
						LEFT JOIN Con_EditCms_Category CC WITH (NOLOCK) ON CC.Id = CB.CategoryId
						WHERE CB.CategoryId IN (
								SELECT ListMember
								FROM fnSplitCSVValuesWithIdentity(@CategoryList)
								)
							AND CB.ApplicationID = @ApplicationId
							AND CB.IsActive = 1
							AND CB.IsPublished = 1
							AND ISNULL(MO.IsDeleted, 0) = 0
							AND ISNULL(M.IsDeleted, 0) = 0
							AND CB.DisplayDate <= GETDATE()
							AND (
								IsSticky IS NULL
								OR IsSticky = 0
								OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE)
								)
							AND (
								@MakeId IS NULL
								OR M.Id = @MakeId
								)
							AND (
								@ModelId IS NULL
								OR MO.ID = @ModelId
								)
						) AS CTE1
					WHERE Row_No = 1
					) CTE
				WHERE Row_Num BETWEEN @StartIndex
						AND @EndIndex
				)
			ORDER BY Row_Num

			-- Record Count Query
			SELECT COUNT(DISTINCT CB.Id) AS RecordCount
			FROM Con_EditCms_Basic CB WITH (NOLOCK)
			INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
				AND C.IsActive = 1
			LEFT JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
			LEFT JOIN BikeMakes M WITH (NOLOCK) ON M.ID = C.MakeId
			WHERE CB.CategoryId IN (
					SELECT ListMember
					FROM fnSplitCSVValuesWithIdentity(@CategoryList)
					)
				AND CB.IsPublished = 1
				AND CB.ApplicationID = @ApplicationId
				AND CB.IsActive = 1
				AND ISNULL(MO.IsDeleted, 0) = 0
				AND ISNULL(M.IsDeleted, 0) = 0
				AND CB.DisplayDate <= GETDATE()
				AND (
					IsSticky IS NULL
					OR IsSticky = 0
					OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE)
					)
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