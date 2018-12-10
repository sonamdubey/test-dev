IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetNewsPageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetNewsPageDetails]
GO

	
-- =============================================            
-- Author:  <Reshma Shetty>            
-- Create date: <24/09/2012>            
-- Description: <Returns the details to be shown on viewing a news page.            
--               Also increases the view count if the news has been viewed after being uploaded on CarWale.>     
---Modified By:Ravi koshal 8/20/2013 -- Added SocialPluginsCount.   
---Description:added query  to retrieve next and prev news data... 
-- Modified By : Ravi Koshal 2/11/2014 -- Added categoryid = 1  
-- Modified by : Manish on 14-07-2014 for solving bug on production urgently fetch the value of tag variable.    
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging 
-- =============================================            
CREATE PROCEDURE [cw].[GetNewsPageDetails] -- EXEC [cw].[GetNewsPageDetails] 8782, 1            
	-- Add the parameters for the stored procedure here        
	    
	@BasicId NUMERIC(18, 0) = 0
	,@ApplicationId INT
	,@IsPublished BIT = 1
	,@NextId NUMERIC(18, 0) OUTPUT
	,@NextUrl VARCHAR(200) OUTPUT
	,@NextTitle VARCHAR(250) OUTPUT
	,@PrevId NUMERIC(18, 0) OUTPUT
	,@PrevUrl VARCHAR(200) OUTPUT
	,@PrevTitle VARCHAR(250) OUTPUT
	,@Tag VARCHAR(100) OUTPUT
	,@AuthorName VARCHAR(100) OUTPUT
	,@DisplayDate DATETIME OUTPUT
	,@Title VARCHAR(250) OUTPUT
	,@url VARCHAR(200) OUTPUT
	,@Views INT OUTPUT
	,@Content VARCHAR(max) OUTPUT
	,@HostUrl VARCHAR(250) OUTPUT
	,@ImagePathLarge VARCHAR(100) OUTPUT
	,@ImagePathThumbnail VARCHAR(100) OUTPUT
	,@MainImgCaption VARCHAR(250) OUTPUT
	,@Caption VARCHAR(250) OUTPUT
	,@Id NUMERIC OUTPUT
	,@FacebookCommentCount INT OUTPUT
	,@MaskingName VARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from            
	-- interfering with SELECT statements.            
	SET NOCOUNT ON;

	---used to get number of views            
	--IF (@IsPublished = 1)
	--BEGIN
	--	UPDATE Con_EditCms_Basic
	--	SET [Views] = [Views] + 1
	--	WHERE Id = @BasicId
	--END

	------------------------added by Manish on 14-07-2014-------------------------------------------------------------------------------------------------

	SELECT 
		@Tag= COALESCE(@Tag + ',', '') + tag
		
	FROM Con_EditCms_Basic CB WITH (NOLOCK) 
	INNER JOIN Con_EditCms_Pages CP WITH (NOLOCK) ON CP.BasicId = CB.Id
	INNER JOIN Con_EditCms_PageContent CPC  WITH (NOLOCK) ON CPC.PageId = CP.Id
	LEFT JOIN Con_EditCms_BasicTags BT WITH (NOLOCK) ON BT.BasicId = CB.Id
	LEFT JOIN Con_EditCms_Tags TG WITH (NOLOCK) ON TG.Id = BT.TagId
	LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
		AND CEI.IsMainImage = 1
		AND CEI.IsActive = 1
	LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON SPC.TypeId = CB.Id
	WHERE CB.IsActive = 1
		AND CB.ApplicationID = @ApplicationId
		AND CP.BasicId = CB.Id
		AND CP.IsActive = 1
		AND CPC.PageId = CP.Id
		AND CB.Id = @BasicId
		AND CB.IsPublished = @IsPublished



-------------------------------------------------------------------------------------------------------------------------


	-- used to get all parameters related to news.            
	SELECT TOP 1 @AuthorName = CB.AuthorName
		,@DisplayDate = CB.DisplayDate
		,@Title = CB.Title
		,@url = CB.Url
		,@Views = CB.VIEWS
		,@Content = CPC.Data
		,@HostUrl = CEI.HostURL
		,@ImagePathLarge = CEI.ImagePathLarge
		,@Caption = CEI.Caption
		,@MainImgCaption = CB.MainImgCaption
		--,@Tag = TG.Tag
		,@ImagePathThumbnail = CEI.ImagePathThumbnail
		,@Id = CB.Id
		,@FacebookCommentCount = SPC.FacebookCommentCount
		,@MaskingName = CA.MaskingName
	FROM Con_EditCms_Basic CB WITH (NOLOCK)
	INNER JOIN Con_EditCms_Pages CP WITH (NOLOCK)  ON CP.BasicId = CB.Id
	INNER JOIN Con_EditCms_PageContent CPC  WITH (NOLOCK) ON CPC.PageId = CP.Id
	LEFT JOIN Con_EditCms_BasicTags BT WITH (NOLOCK) ON BT.BasicId = CB.Id
	LEFT JOIN Con_EditCms_Tags TG WITH (NOLOCK) ON TG.Id = BT.TagId
	LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
		AND CEI.IsMainImage = 1
		AND CEI.IsActive = 1
	LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON SPC.TypeId = CB.Id
	INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
	WHERE CB.IsActive = 1
		AND CB.ApplicationID = @ApplicationId
		AND CP.BasicId = CB.Id
		AND CP.IsActive = 1
		AND CPC.PageId = CP.Id
		AND CB.Id = @BasicId
		AND CB.IsPublished = @IsPublished

	---to retrieve previous news data         
	SELECT TOP 1 @PrevId = CB.Id
		,@PrevTitle = CB.Title
		,@PrevUrl = CB.Url
	FROM CON_EDITCMS_BASIC CB WITH (NOLOCK)
	INNER JOIN Con_EditCms_Pages CP WITH (NOLOCK) ON CP.BasicId = CB.Id
	WHERE CB.ID > @BasicId
		AND CB.ApplicationID = @ApplicationId
		AND CB.IsActive = 1
		AND CB.IsPublished = 1
		AND CP.BasicId = CB.Id
		AND CB.CategoryId = 1
	ORDER BY CB.ID

	--to retrieve next news data     
	SELECT TOP 1 @NextId = CB.Id
		,@NextTitle = CB.Title
		,@NextUrl = CB.Url
	FROM CON_EDITCMS_BASIC CB WITH (NOLOCK)
	INNER JOIN Con_EditCms_Pages CP WITH (NOLOCK) ON CP.BasicId = CB.Id
	WHERE CB.ID < @BasicId
		AND CB.ApplicationID = @ApplicationId
		AND CB.IsActive = 1
		AND CB.IsPublished = 1
		AND CP.BasicId = CB.Id
		AND CB.CategoryId = 1
	ORDER BY CB.ID DESC
END


