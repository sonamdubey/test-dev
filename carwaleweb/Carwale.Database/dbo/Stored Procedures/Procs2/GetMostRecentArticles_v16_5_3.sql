IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMostRecentArticles_v16_5_3]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMostRecentArticles_v16_5_3]
GO

	--================================================================
-- Author: Natesh Kumar on 28/8/14 
--Description: gets the most recent articles  with the given contentTypes as categoryids , applicationid, totalrecords , makeid and modelid as input parameter
-- modified by natesh kumar for fetching make and model from carwale and bikewale resp.
-- modified by natesh kumar added subcategory name for bikewale too on 17.11.14
-- modified by natesh kumar added ismainimg flag and isdeleted check on 1/12/14
-- Modified by Satish Sharma On 23/Jul/2015, Image dynamic resize revamp
-- Modified by Manish on 09-12-2015 changed carwale query for optimization.
-- Modified by Manish on 18-12-2015 changed carwale query for optimization.
-- Modified By Rakesh Yadav on 22 Jun 2016 to show sponsored news BMW (Sponsored)(1561),
-- Modified By: Rakesh Yadav On 8 Aug 2016, Added condtion on display date to avoid future articles in list
-- Modified By Rakesh Yadav on 8 Sep 2016 to show sponsored news Mahindra Electric (Sponsored)(1571),
-- Modified By Manish on 05-10-2016 changed temp table creation. First create temp table and than insert record.
--================================================================
-- exec [dbo].[GetMostRecentArticles_v16_5_3] 1,8,50, 0,595

CREATE PROCEDURE [dbo].[GetMostRecentArticles_v16_5_3] @ApplicationId INT
	,@contentTypes VARCHAR(50)
	,@totalRecords INT
	,@MakeId INT = NULL
	,@ModelId INT = NULL
AS
BEGIN
	-- print (@MakeId + @ModelId);
	declare @RecordFound tinyint;
	
	IF (@MakeId IS NOT NULL OR @ModelId IS NOT NULL)
	BEGIN		
		WITH cte
		AS (
			SELECT DISTINCT cb.Id AS BasicId
				,cb.CategoryId AS CategoryId
				,cb.Title AS Title
				,cb.Url AS ArticleUrl
				,CB.DisplayDate AS DisplayDate
				,CB.AuthorName AS AuthorName
				,cb.Description AS Description
				,CB.PublishedDate
				,cb.VIEWS AS VIEWS
				,(ISNULL(cb.IsSticky, 0)) AS IsSticky
				,(ISNULL(cb.FacebookCommentCount, 0)) AS FacebookCommentCount
				,cb.HostURL AS HostUrl
				,cb.MainImagePath OriginalImgPath
				,'' AS MakeName
				,'' AS ModelName
				,'' AS ModelMaskingName
				,'' AS SubCategory
			FROM Con_EditCms_Basic cb WITH (NOLOCK)
			INNER JOIN Con_EditCms_Cars Car WITH (NOLOCK) ON Car.BasicId = CB.Id
				AND Car.IsActive = 1
			WHERE cb.IsActive = 1
				AND cb.IsPublished = 1
				AND cb.ApplicationID = @ApplicationId
				AND cb.DisplayDate <= GETDATE()
				AND cb.CategoryId IN (
					SELECT ListMember
					FROM fnSplitCSVValuesWithIdentity(@contentTypes)
					)
				AND (
					@MakeId IS NULL
					OR Car.MakeId = @MakeId
					)
				AND (
					@ModelId IS NULL
					OR Car.ModelId = @ModelId
					)
			)
		SELECT TOP (@totalRecords) *
		FROM cte
		ORDER BY DisplayDate DESC
	END
	ELSE
	BEGIN

	       CREATE TABLE  #temp1
		            (BasicId	INT,
					CategoryId	INT,
					Title	varchar(250),
					ArticleUrl	varchar(300),
					DisplayDate	datetime,
					AuthorName	varchar(100),
					Description	varchar(8000),
					PublishedDate	datetime,
					VIEWS	int,
					IsSticky	bit,
					FacebookCommentCount	int,
					HostUrl	varchar(100),
					OriginalImgPath	varchar(300),
					MakeName	varchar(50),
					ModelName	varchar(50),
					ModelMaskingName	varchar(50),
					SubCategory	varchar(50),
					tempVal	int);

       
	   INSERT INTO #temp1 (BasicId
	                      ,CategoryId
						  ,Title
						  ,ArticleUrl
						  ,DisplayDate
						  ,AuthorName
						  ,Description
						  ,PublishedDate
						  ,VIEWS
						  ,IsSticky
						  ,FacebookCommentCount
						  ,HostUrl
 						  ,OriginalImgPath
						  ,MakeName
						  ,ModelName
						  ,ModelMaskingName
						  ,SubCategory
						  ,tempVal)
		SELECT TOP 1 cb.Id AS BasicId
			,cb.CategoryId AS CategoryId
			,cb.Title AS Title
			,cb.Url AS ArticleUrl
			,CB.DisplayDate AS DisplayDate
			,CB.AuthorName AS AuthorName
			,cb.Description AS Description
			,CB.PublishedDate
			,cb.VIEWS AS VIEWS
			,(ISNULL(cb.IsSticky, 0)) AS IsSticky
			,(ISNULL(cb.FacebookCommentCount, 0)) AS FacebookCommentCount
			,cb.HostURL AS HostUrl
			,cb.MainImagePath OriginalImgPath
			,'' AS MakeName
			,'' AS ModelName
			,'' AS ModelMaskingName
			,'' AS SubCategory
			,1 AS tempVal--just to make result of 1st query on top
	-- 	into #temp1
		FROM Con_EditCms_Basic cb WITH (NOLOCK)
		WHERE cb.IsActive = 1
			AND cb.IsPublished = 1
			AND cb.ApplicationID = @ApplicationId
			AND cb.DisplayDate <= GETDATE()
			AND cb.CategoryId IN (
				SELECT ListMember
				FROM fnSplitCSVValuesWithIdentity(@contentTypes)
				)
			AND CB.AuthorId=1571 AND CB.IsFeatured=1
		ORDER BY cb.DisplayDate DESC

		
		set @RecordFound= @@ROWCOUNT;
		--if exists(select * from #temp1) set @totalRecords=@totalRecords-1

		 CREATE TABLE  #temp2
		            (BasicId	INT,
					CategoryId	INT,
					Title	varchar(250),
					ArticleUrl	varchar(300),
					DisplayDate	datetime,
					AuthorName	varchar(100),
					Description	varchar(8000),
					PublishedDate	datetime,
					VIEWS	int,
					IsSticky	bit,
					FacebookCommentCount	int,
					HostUrl	varchar(100),
					OriginalImgPath	varchar(300),
					MakeName	varchar(50),
					ModelName	varchar(50),
					ModelMaskingName	varchar(50),
					SubCategory	varchar(50),
					tempVal	int);

        INSERT INTO #temp2 (BasicId
	                      ,CategoryId
						  ,Title
						  ,ArticleUrl
						  ,DisplayDate
						  ,AuthorName
						  ,Description
						  ,PublishedDate
						  ,VIEWS
						  ,IsSticky
						  ,FacebookCommentCount
						  ,HostUrl
 						  ,OriginalImgPath
						  ,MakeName
						  ,ModelName
						  ,ModelMaskingName
						  ,SubCategory
						  ,tempVal)
		SELECT TOP (@totalRecords-@RecordFound) cb.Id AS BasicId
			,cb.CategoryId AS CategoryId
			,cb.Title AS Title
			,cb.Url AS ArticleUrl
			,CB.DisplayDate AS DisplayDate
			,CB.AuthorName AS AuthorName
			,cb.Description AS Description
			,CB.PublishedDate
			,cb.VIEWS AS VIEWS
			,(ISNULL(cb.IsSticky, 0)) AS IsSticky
			,(ISNULL(cb.FacebookCommentCount, 0)) AS FacebookCommentCount
			,cb.HostURL AS HostUrl
			,cb.MainImagePath OriginalImgPath
			,'' AS MakeName
			,'' AS ModelName
			,'' AS ModelMaskingName
			,'' AS SubCategory
			,0 AS tempVal--just to make result of 1st query on top
		--into #temp2
		FROM Con_EditCms_Basic cb WITH (NOLOCK)
		LEFT JOIN #temp1 T ON T.BasicId=cb.Id
		WHERE cb.IsActive = 1
			AND cb.IsPublished = 1
			AND cb.ApplicationID = @ApplicationId
			AND cb.DisplayDate <= GETDATE()
			AND cb.CategoryId IN (
				SELECT ListMember
				FROM fnSplitCSVValuesWithIdentity(@contentTypes)
				)
		AND T.BasicId is null
		ORDER BY cb.DisplayDate DESC

		select * from #temp1 UNION select * from #temp2
		order by tempVal desc,displaydate desc

		drop table #temp1;
		drop table #temp2;
	END
END
