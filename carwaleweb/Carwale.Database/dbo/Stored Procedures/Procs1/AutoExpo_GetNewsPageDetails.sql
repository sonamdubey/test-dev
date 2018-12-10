IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AutoExpo_GetNewsPageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AutoExpo_GetNewsPageDetails]
GO

	
-- =============================================       
-- Author:  <Ravi Koshal>        
-- Create date: <2/01/2014>          
-- Description: <Returns the details to be shown on viewing a news page in Autoexpo site.  
--               Also increases the view count if the news has been viewed after being uploaded on AutoExpo.     
--Modified By: Rakesh Yadav
--Date Modified: 13 Jan 2014
--Description: Added queries to fetch next and prev news data
-- =============================================            
CREATE PROCEDURE [dbo].[AutoExpo_GetNewsPageDetails] -- EXEC [cw].[AutoExpo_GetNewsPageDetails] 8782, 1     
	-- Add the parameters for the stored procedure here   
	@BasicId NUMERIC(18, 0) = 0
	,@IsPublished BIT = 1
	,@OrderBy VARCHAR(20) = NULL
	,--Added by Rakesh Yadav
	@MakeId INT = NULL
	,--Added by Rakesh Yadav
	@Title VARCHAR(250) OUTPUT
	,@AuthorName VARCHAR(100) OUTPUT
	,@DisplayDate DATETIME OUTPUT
	,@DisplayDate1 DATETIME = NULL OUTPUT
	,@Url VARCHAR(200) OUTPUT
	,@Content VARCHAR(MAX) OUTPUT
	,@IsMainImage BIT OUTPUT
	,@HostUrl VARCHAR(250) OUTPUT
	,@ImagePathLarge VARCHAR(100) OUTPUT
	,@ImagePathThumbnail VARCHAR(100) OUTPUT
	,@Views INT OUTPUT
	,@Views1 INT = NULL OUTPUT
	,@Tag VARCHAR(100) OUTPUT
	,@MainImgCaption VARCHAR(250) OUTPUT
	,@Caption VARCHAR(250) OUTPUT
	,
	--Added by Rakesh Yadav
	@NextId NUMERIC(18, 0) = NULL OUTPUT
	,@NextAuthor VARCHAR(100) = NULL OUTPUT
	,@NextDispDate DATETIME = NULL OUTPUT
	,@NextTitle VARCHAR(250) = NULL OUTPUT
	,@PrevId NUMERIC(18, 0) = NULL OUTPUT
	,@PrevAuthor VARCHAR(100) = NULL OUTPUT
	,@PrevDispDate DATETIME = NULL OUTPUT
	,@PrevTitle VARCHAR(250) = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from            
	-- interfering with SELECT statements.            
	SET NOCOUNT ON;

	IF (@IsPublished = 1)
	BEGIN
		UPDATE Con_EditCms_Basic
		SET [Views] = [Views] + 1
		WHERE Id = @BasicId
	END

	SELECT @AuthorName = CB.AuthorName
		,@DisplayDate = CB.DisplayDate
		,@Title = CB.Title
		,@Url = CB.Url
		,@IsMainImage = CI.IsMainImage
		,@HostUrl = CI.HostURL
		,@ImagePathThumbnail = CI.ImagePathThumbnail
		,@ImagePathLarge = CI.ImagePathLarge
		,@Content = PC.Data
		,@Views = CB.VIEWS
	FROM Con_EditCms_Basic CB
	INNER JOIN Con_EditCms_Pages P ON P.BasicId = CB.Id
	INNER JOIN Con_EditCms_PageContent PC ON PC.PageId = P.Id
	INNER JOIN Con_EditCms_Images CI ON CB.Id = CI.BasicId
	WHERE CB.Id = @BasicId
		AND YEAR(CB.PublishedDate) >= 2013
		AND CI.IsMainImage = 1

	IF @MakeId IS NOT NULL
	BEGIN
		--next news
		SELECT TOP 1 @NextId = ceb1.Id
			,@NextTitle = ceb1.Title
			,@NextAuthor = ceb1.AuthorName
			,@NextDispDate = ceb1.DisplayDate
		FROM Con_EditCms_Basic ceb1
		INNER JOIN Con_EditCms_Images AS CI ON CI.BasicId = ceb1.Id
		INNER JOIN Con_EditCms_Cars CC ON ceb1.Id = CC.basicId
			AND CC.MakeId = @MakeId
		WHERE ceb1.IsActive = 1
			AND ceb1.IsPublished = 1
			AND ceb1.CategoryId = 9
			AND CI.IsMainImage = 1
			AND YEAR(ceb1.PublishedDate) >= 2013
			AND ceb1.DisplayDate < (
				SELECT ceb.DisplayDate
				FROM Con_EditCms_Basic ceb
				WHERE ceb.Id = @BasicId
				)
		ORDER BY ceb1.DisplayDate DESC

		--prev news
		SELECT TOP 1 @PrevId = ceb1.Id
			,@PrevTitle = ceb1.Title
			,@PrevAuthor = ceb1.AuthorName
			,@PrevDispDate = ceb1.DisplayDate
		FROM Con_EditCms_Basic ceb1
		INNER JOIN Con_EditCms_Images AS CI ON CI.BasicId = ceb1.Id
		INNER JOIN Con_EditCms_Cars CC ON ceb1.Id = CC.basicId
			AND CC.MakeId = @MakeId
		WHERE ceb1.IsActive = 1
			AND ceb1.IsPublished = 1
			AND ceb1.CategoryId = 9
			AND CI.IsMainImage = 1
			AND YEAR(ceb1.PublishedDate) >= 2013
			AND ceb1.DisplayDate > (
				SELECT ceb.DisplayDate
				FROM Con_EditCms_Basic ceb
				WHERE ceb.Id = @BasicId
				)
		ORDER BY ceb1.DisplayDate ASC
	END
	ELSE
	BEGIN
		IF @OrderBy = 'CD0'
		BEGIN
			--next news by display date
			SELECT TOP 1 @NextId = ceb1.Id
				,@NextTitle = ceb1.Title
				,@NextAuthor = ceb1.AuthorName
				,@NextDispDate = ceb1.DisplayDate
			FROM Con_EditCms_Basic ceb1
			INNER JOIN Con_EditCms_Images AS CI ON CI.BasicId = ceb1.Id
			WHERE ceb1.IsActive = 1
				AND ceb1.IsPublished = 1
				AND ceb1.CategoryId = 9
				AND CI.IsMainImage = 1
				AND YEAR(ceb1.PublishedDate) >= 2013
				AND ceb1.DisplayDate < (
					SELECT ceb.DisplayDate
					FROM Con_EditCms_Basic ceb
					WHERE ceb.Id = @BasicId
					)
			ORDER BY ceb1.DisplayDate DESC

			--prev news by display date
			SELECT TOP 1 @PrevId = ceb1.Id
				,@PrevTitle = ceb1.Title
				,@PrevAuthor = ceb1.AuthorName
				,@PrevDispDate = ceb1.DisplayDate
			FROM Con_EditCms_Basic ceb1
			INNER JOIN Con_EditCms_Images AS CI ON CI.BasicId = ceb1.Id
			WHERE ceb1.IsActive = 1
				AND ceb1.IsPublished = 1
				AND ceb1.CategoryId = 9
				AND CI.IsMainImage = 1
				AND YEAR(ceb1.PublishedDate) >= 2013
				AND ceb1.DisplayDate > (
					SELECT ceb.DisplayDate
					FROM Con_EditCms_Basic ceb
					WHERE ceb.Id = @BasicId
					)
			ORDER BY ceb1.DisplayDate ASC
		END
		ELSE
		BEGIN
			SELECT @Views1 = ceb.VIEWS
				,@DisplayDate1 = ceb.DisplayDate
			FROM Con_EditCms_Basic ceb
			WHERE ceb.Id = @BasicId

			--next news by views 
			SELECT TOP 1 @NextId = ceb1.Id
				,@NextTitle = ceb1.Title
				,@NextAuthor = ceb1.AuthorName
				,@NextDispDate = ceb1.DisplayDate
			FROM Con_EditCms_Basic ceb1
			WHERE ceb1.IsActive = 1
				AND ceb1.IsPublished = 1
				AND ceb1.CategoryId = 9
				AND ceb1.Id <> @BasicId
				AND ceb1.VIEWS <= @Views1
				AND 1 = CASE 
					WHEN ceb1.VIEWS = @Views
						AND ceb1.DisplayDate > @DisplayDate1
						THEN 0
					ELSE 1
					END
			ORDER BY ceb1.VIEWS DESC
				,ceb1.DisplayDate DESC

			--prev news by views
			SELECT TOP 1 @PrevId = ceb1.Id
				,@PrevTitle = ceb1.Title
				,@PrevAuthor = ceb1.AuthorName
				,@PrevDispDate = ceb1.DisplayDate
			FROM Con_EditCms_Basic ceb1
			WHERE ceb1.IsActive = 1
				AND ceb1.IsPublished = 1
				AND ceb1.CategoryId = 9
				AND ceb1.Id <> @BasicId
				AND ceb1.VIEWS >= @Views
				AND 1 = CASE 
					WHEN ceb1.VIEWS = @Views
						AND ceb1.DisplayDate < @DisplayDate
						THEN 0
					ELSE 1
					END
			ORDER BY ceb1.VIEWS ASC
				,ceb1.DisplayDate ASC
		END
	END
END
